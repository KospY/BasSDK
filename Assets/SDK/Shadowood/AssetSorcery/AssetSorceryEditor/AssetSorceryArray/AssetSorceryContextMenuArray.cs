using System.IO;
using UnityEditor;
using UnityEngine;

namespace ThunderRoad.AssetSorcery
{
    public static class AssetSorceryContextMenuTexture
    {
        [MenuItem(AssetSorceryConvertMenuUtility.ASSETSORCERY_CTXMENU_PREFIX + "Convert TextureArray to " + AssetSorceryArray.ASSETSORCERY_FILE_EXTENSION, true)]
        private static bool MenuItemValidation()
        {
            var selection = Selection.activeObject;

            if (selection == null) return false;
            bool anyValid = false;

            foreach (Object o in Selection.objects)
            {
                var path = AssetDatabase.GetAssetPath(o);
                if (IsItemValidForArray(path))
                {
                    anyValid = true;
                    break;
                }
            }

            return anyValid;
        }


        [MenuItem(AssetSorceryConvertMenuUtility.ASSETSORCERY_CTXMENU_PREFIX + "Convert TextureArray to " + AssetSorceryArray.ASSETSORCERY_FILE_EXTENSION, false, 1000000)]
        private static void MenuItem()
        {
            // AssetSorceryCommonItems.SaveProject();

            // TODO support selecting multiple items

            //foreach (Object selection in Selection.objects)
            //{
            //    //var path = AssetDatabase.GetAssetPath(selection);
            //    //AssetSorceryConvertMenuUtility.ConvertMaterial(path, upgrader);
            //    Undo.RecordObject(selection, "Changed Shader");
            //}

            var selection = Selection.activeObject;
            var path = AssetDatabase.GetAssetPath(selection);
            var asset = AssetDatabase.LoadAssetAtPath<Texture2DArray>(path);
            var currentAsset = asset;

            if (!IsItemValidForArray(path)) return;


            // TODO handle when finding multiple matching

            // TODO handle keeping same GUID


            var currentName = currentAsset.name; //.Replace("/", "-");

            string dir = Path.GetDirectoryName(path);
            if (AssetSorceryGetDirectoryType.GetDirectoryType(path) == AssetSorceryGetDirectoryType.eDirectoryType.ReadOnlyPackageCache)
            {
                Debug.LogWarning("source file is within read only PackageCache, save to Assets instead");
                dir = "Assets/";
            }

            var newPath = AssetSorceryCreationMenus.CreateAssetSorceryArray(dir + "/" + currentName + "-" + AssetSorceryArray.ASSETSORCERY_FILE_EXTENSION + "." + AssetSorceryArray.ASSETSORCERY_FILE_EXTENSION, currentAsset);
            var newAsset = AssetDatabase.LoadAssetAtPath<Texture2DArray>(newPath);
            Selection.activeObject = newAsset;

            //AssetSorceryCommonItems.SaveProject();
        }

        /// <summary>
        /// Checks if item at path is a Texture2DArray, is using not using ASarray, is not a ASarray asset file and is not in a read only package.
        /// </summary>
        public static bool IsItemValidForArray(string path)
        {
            var assetType = AssetDatabase.GetMainAssetTypeAtPath(path);

            //Debug.Log("IsItemValidForArray: " + path+":"+assetType);

            if (assetType != typeof(Texture2DArray)) return false;

            var asset = AssetDatabase.LoadAssetAtPath<Texture2DArray>(path);
            if (asset == null) return false;

            var filePath = AssetDatabase.GetAssetPath(asset);
            if (filePath.EndsWith(AssetSorceryArray.ASSETSORCERY_FILE_EXTENSION)) return false;

            if (AssetSorceryGetDirectoryType.GetDirectoryType(path) == AssetSorceryGetDirectoryType.eDirectoryType.ReadOnlyPackageCache) return false;

            return true;
        }
    }
}
