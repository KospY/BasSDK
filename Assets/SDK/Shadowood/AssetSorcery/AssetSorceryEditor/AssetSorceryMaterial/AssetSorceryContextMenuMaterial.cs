using System.IO;
using UnityEditor;
using UnityEngine;

namespace ThunderRoad.AssetSorcery
{
    public static class AssetSorceryContextMenuMaterial
    {
        [MenuItem(AssetSorceryConvertMenuUtility.ASSETSORCERY_CTXMENU_PREFIX + "Convert Material to " + AssetSorceryMaterial.ASSETSORCERY_FILE_EXTENSION, true)]
        private static bool MenuItemValidation()
        {
            var selection = Selection.activeObject;

            if (selection == null) return false;
            bool anyValid = false;

            foreach (Object o in Selection.objects)
            {
                var path = AssetDatabase.GetAssetPath(o);
                if (IsItemValidForMaterial(path))
                {
                    anyValid = true;
                    break;
                }
            }

            return anyValid;
        }


        [MenuItem(AssetSorceryConvertMenuUtility.ASSETSORCERY_CTXMENU_PREFIX + "Convert Material to " + AssetSorceryMaterial.ASSETSORCERY_FILE_EXTENSION, false, 1000000)]
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
            var asset = AssetDatabase.LoadAssetAtPath<Material>(path);
            var currentAsset = asset;

            if (!IsItemValidForMaterial(path)) return;


            // TODO handle when finding multiple matching

            // TODO handle keeping same GUID


            var currentName = currentAsset.name; //.Replace("/", "-");

            string dir = Path.GetDirectoryName(path);
            if (AssetSorceryGetDirectoryType.GetDirectoryType(path) == AssetSorceryGetDirectoryType.eDirectoryType.ReadOnlyPackageCache)
            {
                Debug.LogWarning("source file is within read only PackageCache, save to Assets instead");
                dir = "Assets/";
            }

            var newPath = AssetSorceryCreationMenus.CreateAssetSorceryMaterial(dir + "/" + currentName + "-" + AssetSorceryMaterial.ASSETSORCERY_FILE_EXTENSION + "." + AssetSorceryMaterial.ASSETSORCERY_FILE_EXTENSION, currentAsset);
            var newAsset = AssetDatabase.LoadAssetAtPath<Material>(newPath);
            Selection.activeObject = newAsset;


            //AssetSorceryCommonItems.SaveProject();
        }

        /// <summary>
        /// Checks if item at path is a Material, is using not using ASshader, is not a ASshader asset file, shader does not match against an exclusion list and is not in a read only package.
        /// </summary>
        public static bool IsItemValidForMaterial(string path)
        {
            var assetType = AssetDatabase.GetMainAssetTypeAtPath(path);
            if (assetType != typeof(Material)) return false;

            var asset = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (asset == null) return false;

            var shaderPath = AssetDatabase.GetAssetPath(asset.shader);
            if (shaderPath.EndsWith(AssetSorceryShader.ASSETSORCERY_FILE_EXTENSION)) return false;

            //var upgrader = new SISMaterialUpgrader();
            //SISMaterialUpgrader.AddIgnoredShaders(ref upgrader.DRPUpgradersSkip);

            //foreach (string s in upgrader.DRPUpgradersSkip)
            //{
            //    if (asset.shader.name == s) return false;
            //}

            if (AssetSorceryGetDirectoryType.GetDirectoryType(path) == AssetSorceryGetDirectoryType.eDirectoryType.ReadOnlyPackageCache) return false;

            return true;
        }
    }
}
