using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ThunderRoad.AssetSorcery
{
    public partial class AssetSorceryCreationMenus
    {
        [MenuItem(ASSETSORCERY_MENU_PATH + AssetSorceryMaterial.ASSETSORCERY_FILE_EXTENSION, priority = 300)]
        public static void CreateAssetSorceryMaterialMenu()
        {
            CreateAssetSorceryMaterial();
        }

        public static string CreateAssetSorceryMaterial(string pathIn = null, Material itemIn = null)
        {
            string directoryPath = "Assets";
/*
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

                var fileName = AssetSorceryMaterial.ASSETSORCERY_FILE_EXTENSION + "." + AssetSorceryMaterial.ASSETSORCERY_FILE_EXTENSION;
                directoryPath = AssetDatabase.GenerateUniqueAssetPath(directoryPath + fileName);
                var content = ScriptableObject.CreateInstance<AssetSorceryMaterialAsset>();
                File.WriteAllText(directoryPath, EditorJsonUtility.ToJson(content));
                AssetDatabase.Refresh();
            }*/

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

                var fileName = AssetSorceryMaterial.ASSETSORCERY_FILE_EXTENSION + "." + AssetSorceryMaterial.ASSETSORCERY_FILE_EXTENSION;
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
                itemIn = Resources.Load("ASDummyMat", typeof(Material)) as Material;
                if (itemIn == null) Debug.LogError("Error finding dummy material");
            }

            var content = ScriptableObject.CreateInstance<AssetSorceryMaterialAsset>();
            content.filters = new List<FilterPlatform<Material>>
            {
                new FilterPlatform<Material>
                {
                    item = itemIn
                }
            };

            content.customMatName = itemIn.name + " - AssetSorceryMaterial";

            content.WriteAssetContentBack(directoryPath);

            return directoryPath;
        }
    }
}
