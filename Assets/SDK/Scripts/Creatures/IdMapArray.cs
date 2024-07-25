using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
namespace ThunderRoad
{
    [PreferBinarySerialization]
    public class IdMapArray : ScriptableObject
    {
        [Tooltip("The path to the ID map texture.")]
        public string idMapPath;
        [Tooltip("The factor to scale the ID map down by.")]
#if ODIN_INSPECTOR
        [ReadOnly]
#endif    
        public int scale = 2;
        public NibbleArray nibbleArray;
        //mainly for recreating the texture from the array
#if ODIN_INSPECTOR
        [ReadOnly]
#endif    
        public int originalWidth;
#if ODIN_INSPECTOR
        [ReadOnly]
#endif    
        public int originalHeight;
#if ODIN_INSPECTOR
        [ReadOnly]
#endif    
        public int width;
#if ODIN_INSPECTOR
        [ReadOnly]
#endif    
        public int height;        
#if ODIN_INSPECTOR
        [ShowInInspector]
        [ReadOnly]
#endif        
        private int estimatedIDMapSize;
#if ODIN_INSPECTOR
        [ShowInInspector]
        [ReadOnly]
#endif        
        private int estimatedIDMapArraySize;  
        public Texture2D debugTexture;

        public int GetIdAtUV(float ux, float uy)
        {
            if (nibbleArray == null || nibbleArray.Length == 0)
            {
                Debug.LogError("ID map array is null.");
                return -1;
            }
            
            //convert the uv to pixel coordinates
            int x = (int)(ux * width);
            int y = (int)(uy * height);

            int i = y * width + x;
            byte id = nibbleArray[i];
            return id;    
        }
        
        [Button]
        public void ConvertArrayToTexture()
        {
            if(nibbleArray == null || nibbleArray.Length == 0){
                Debug.LogError("ID map array is null.");
                return;
            }
            
            //we need to load the catalog to get the colour ids
            Catalog.EditorLoadAllJson();
            
            Texture2D texture = new Texture2D(width,height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    try
                    {
                        int i = y * width + x;
                        byte id = nibbleArray[i];
                        Color color = Catalog.gameData.GetIDMapColor(id);
                        texture.SetPixel(x, y, color);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"Error setting pixel at {x}, {y}: {e.Message}");
                    }
                }
            }
            texture.Apply();
            debugTexture = texture;
            //update the estimated sizes
            //ID map is RGB so 3 bytes per pixel
            estimatedIDMapSize = originalWidth * originalHeight * 3;
            //ID map array is 1 byte per 2 pixels
            estimatedIDMapArraySize =  nibbleArray.Length / 2;
        }

        private void OnValidate()
        {
            debugTexture = null;
        }
    }
}
