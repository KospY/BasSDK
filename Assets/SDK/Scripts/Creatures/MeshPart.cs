using UnityEngine;
using System;
using ThunderRoad.Manikin;
using UnityEditor;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{

    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Creatures/MeshPart.html")]
    [DisallowMultipleComponent()]
	public class MeshPart : MonoBehaviour
	{
        [Tooltip("References the Skinned Mesh Renderer that creates the collider for the part.\n\nIt is recommended, if your parts have LODs, to use the lowest LOD for this, as it will be more performant.")]
        public SkinnedMeshRenderer skinnedMeshRenderer;
        [Tooltip("This defines the default physics material if your part does not have an ID mesh. If it does have an ID mesh, it is recommended to set this to \"Flesh\".")]
        public PhysicMaterial defaultPhysicMaterial;
        [Tooltip("The ID map texture. This is used to determine the physics material used for armor detection for the part.")]
        public Texture2D idMap;
        [Tooltip("For better performance, it is recommended to use the convert button to set the ID map to an ID Map array, for better performance.")]
        public IdMapArray idMapArray;
        [Tooltip("The factor to scale the ID map down by.")]
        public int scale = 4;
        
   
        
        
#if UNITY_EDITOR
        
        [Button]
        public void ConvertIdMapToIdMapArray()
        {
            if(idMap == null){
                Debug.LogError("ID map texture is null.");
                return;
            }
            //we need to load the catalog to get the colour ids
            Catalog.EditorLoadAllJson();
            
            var idMapPath = UnityEditor.AssetDatabase.GetAssetPath(idMap);
            
            //Get the importer for the texture
            TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(idMapPath);
            //Make sure the texture is readable
            importer.isReadable = true;
            //disable mipmaps
            importer.mipmapEnabled = false;
            //make sure not SRGB
            importer.sRGBTexture = false;
            //make sure the texture is not compressed
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.maxTextureSize = 8192;
            //make sure theres no overrides for different platforms
            importer.ClearPlatformTextureSettings("Standalone");
            importer.ClearPlatformTextureSettings("Android");
            //apply the changes
            importer.SaveAndReimport();
            idMap = AssetDatabase.LoadAssetAtPath<Texture2D>(idMapPath);
            if (idMap == null)
            {
                Debug.LogError("ID map texture is null.");
                return;
            }
            
            if (idMapArray == null)
            {
                //create instance and save it to the same path as the texture
                idMapArray = ScriptableObject.CreateInstance<IdMapArray>();
                AssetDatabase.CreateAsset(idMapArray, idMapPath.Replace(".png", "_array.asset"));
            }
            idMapArray.idMapPath = idMapPath;
            idMapArray.originalWidth = idMap.width;
            idMapArray.originalHeight = idMap.height;
            idMapArray.scale = scale;
            int width = idMapArray.originalWidth / scale;
            int height = idMapArray.originalHeight / scale;
            idMapArray.width = width;
            idMapArray.height = height;
            idMapArray.nibbleArray = new NibbleArray(width * height);
            //Loop over the scaled texture
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //sample a pixel, offset by the scale. Clamp so we don't go out of bounds
                    int pX = Mathf.Clamp(x * scale, 0, idMapArray.originalWidth - 1);
                    int pY = Mathf.Clamp(y * scale, 0, idMapArray.originalHeight - 1);
                    Color pixel = idMap.GetPixel(pX, pY);
                    //get the id from the catalog
                    int id = Catalog.gameData.GetIDMapId(pixel);
                    try
                    {
                        // Convert pixel color to nibble
                        
                        int i = y * width + x;
                        idMapArray.nibbleArray[i] = (byte)id;
                    } catch (Exception e)
                    {
                        Debug.LogError($"Error converting pixel at ({x},{y}). Color: {pixel}. ID: {id} on texture: {idMap.name} : {e.Message}");
                    }
                }
            }
            idMapArray.debugTexture = null;
            idMap = null;
            //save the idmaparray
            EditorUtility.SetDirty(idMapArray);
            AssetDatabase.SaveAssets();
        }

        
#endif
	}
}
