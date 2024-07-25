using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace ThunderRoad.AssetSorcery
{
    public partial class AssetSorceryCreationMenus
    {
        [MenuItem(ASSETSORCERY_MENU_PATH + AssetSorceryArray.ASSETSORCERY_FILE_EXTENSION, priority = 300)]
        public static void CreateAssetSorceryArrayMenu()
        {
            CreateAssetSorceryArray();
        }

        public static string CreateAssetSorceryArray(string pathIn = null, Texture2DArray itemIn = null)
        {
            string directoryPath = "Assets";

            if (pathIn == null)
            {
                foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
                {
                    directoryPath = AssetDatabase.GetAssetPath(obj);
                    if (!string.IsNullOrEmpty(directoryPath) && File.Exists(directoryPath))
                    {
                        directoryPath = Path.GetDirectoryName(directoryPath);
                        break;
                    }
                }

                directoryPath = directoryPath.Replace("\\", "/");
                if (directoryPath.Length > 0 && directoryPath[directoryPath.Length - 1] != '/') directoryPath += "/";
                if (string.IsNullOrEmpty(directoryPath)) directoryPath = "Assets/";

                var fileName = AssetSorceryArray.ASSETSORCERY_FILE_EXTENSION + "." + AssetSorceryArray.ASSETSORCERY_FILE_EXTENSION;
                //Debug.Log("new path: " + directoryPath + " ---- " + fileName);
                directoryPath = AssetDatabase.GenerateUniqueAssetPath(directoryPath + fileName);
            }
            else
            {
                //directoryPath = pathIn;
                directoryPath = AssetDatabase.GenerateUniqueAssetPath(pathIn);
            }

            //Debug.Log("new path2: " + directoryPath );

            if (itemIn == null)
            {
                itemIn = Resources.Load("ASDummyArray", typeof(Texture2DArray)) as Texture2DArray;
                if (itemIn == null)
                {
                    Debug.LogError("Error finding dummy array");
                    itemIn = new Texture2DArray(128, 128, 1, TextureFormat.RGBAHalf, false);
                    itemIn.name = "DummyArray";

                    var pixels = new Color[itemIn.width * itemIn.height];
                    for (int i = 0; i < pixels.Length; i++)
                    {
                        pixels[i] = Color.white;
                    }

                    for (int i = 0; i < itemIn.depth; i++)
                    {
                        itemIn.SetPixels(pixels, i);
                    }

                    itemIn.Apply();
                    AssetDatabase.CreateAsset(itemIn, @"Assets\Experimental\Shadowood\AssetSorcery\AssetSorceryCore\AssetSorceryArray\Resources\ASDummyArray.asset");

                    itemIn = Resources.Load("ASDummyArray", typeof(Texture2DArray)) as Texture2DArray;
                }
            }

            var content = ScriptableObject.CreateInstance<AssetSorceryArrayAsset>();

            content.filters = new List<FilterPlatform<Texture2DArray>>()
            {
                new FilterPlatform<Texture2DArray>()
                {
                    item = itemIn,
                    platform = AssetSorceryPlatformRuntime.ePlatformAS.Mobile
                },
                new FilterPlatform<Texture2DArray>()
                {
                    item = itemIn,
                    platform = AssetSorceryPlatformRuntime.ePlatformAS.Desktop
                }
            };

            content.customName = itemIn.name + " - AssetSorceryArray";

            content.WriteAssetContentBack(directoryPath);

            return directoryPath;
        }
    }
}
