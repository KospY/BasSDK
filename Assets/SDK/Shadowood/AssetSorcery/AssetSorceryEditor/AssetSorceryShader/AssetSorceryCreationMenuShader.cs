using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ThunderRoad.AssetSorcery
{
    public partial class AssetSorceryCreationMenus
    {
        public static string CreateAssetSorceryShader(string pathIn = null, Shader itemIn = null)
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

                var fileName = AssetSorceryShader.ASSETSORCERY_FILE_EXTENSION + "." + AssetSorceryShader.ASSETSORCERY_FILE_EXTENSION;
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
                itemIn = Shader.Find("Universal Render Pipeline/Lit");
                if (itemIn == null)
                {
                    Debug.LogError("URP Lit missing!!!");
                    return "";
                }
            }

            var content = ScriptableObject.CreateInstance<AssetSorceryShaderAsset>();

            content.targetShader = itemIn;

            content.shaderSettings.customShaderName = itemIn.name + " - ASshader";

            content.WriteAssetContentBack(directoryPath);

            return directoryPath;
        }

        [MenuItem(ASSETSORCERY_MENU_PATH + AssetSorceryShader.ASSETSORCERY_FILE_EXTENSION, priority = 300)]
        public static void CreateAssetSorceryShaderMenu()
        {
            CreateAssetSorceryShader();
        }
    }
}
