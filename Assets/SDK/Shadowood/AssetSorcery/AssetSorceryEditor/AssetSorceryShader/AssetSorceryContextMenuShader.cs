using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThunderRoad.AssetSorcery
{
    public static class AssetSorceryContextMenuShader
    {
        [MenuItem(AssetSorceryConvertMenuUtility.ASSETSORCERY_CTXMENU_PREFIX + "Convert Shader to " + AssetSorceryShader.ASSETSORCERY_FILE_EXTENSION, true)]
        [MenuItem(AssetSorceryConvertMenuUtility.ASSETSORCERY_CTXMENU_PREFIX + "Convert and Replace Shader to " + AssetSorceryShader.ASSETSORCERY_FILE_EXTENSION, true)]
        private static bool MenuItemValidation()
        {
            var selection = Selection.activeObject;
            if (selection == null) return false;

            bool anyValid = false;
            foreach (Object o in Selection.objects)
            {
                var path = AssetDatabase.GetAssetPath(o);
                if (IsItemValidForShader(path))
                {
                    anyValid = true;
                    break;
                }
            }

            return anyValid;
        }

        public static bool IsItemValidForShader(string path)
        {
            if (path.EndsWith(AssetSorceryShader.ASSETSORCERY_FILE_EXTENSION)) return false;

            var assetType = AssetDatabase.GetMainAssetTypeAtPath(path);
            if (assetType != typeof(Shader)) return false;

            var asset = AssetDatabase.LoadAssetAtPath<Shader>(path);
            if (asset == null || asset.passCount <= 0) return false;

            var shaderPath = AssetDatabase.GetAssetPath(asset);
            if (shaderPath.EndsWith(AssetSorceryShader.ASSETSORCERY_FILE_EXTENSION)) return false;

            return true;
        }

        [MenuItem(AssetSorceryConvertMenuUtility.ASSETSORCERY_CTXMENU_PREFIX + "Convert Shader to " + AssetSorceryShader.ASSETSORCERY_FILE_EXTENSION, false, 1000000)]
        private static void MenuItem()
        {
            ConvertShader(false);
        }

        [MenuItem(AssetSorceryConvertMenuUtility.ASSETSORCERY_CTXMENU_PREFIX + "Convert and Replace Shader to " + AssetSorceryShader.ASSETSORCERY_FILE_EXTENSION, false, 1000000)]
        private static void MenuItem2()
        {
            ConvertShader(true);
        }

        public static void ConvertShader(bool keepGUID)
        {
            // TODO support selecting multiple items

            var selection = Selection.activeObject;
            var path = AssetDatabase.GetAssetPath(selection);
            var asset = AssetDatabase.LoadAssetAtPath<Shader>(path);

            if (asset == null)
            {
                Debug.LogError("ConvertShader: asset null A: " + path);
                return;
            }
            
            asset = AssetDatabase.LoadAssetAtPath<Shader>(path);
            if (asset == null)
            {
                Debug.LogError("ConvertShader: asset null B: " + path);
                return;
            }

            var currentAsset = asset;

            if (!IsItemValidForShader(path)) return;

            //Shader foundShader = AssetSorceryConvertMenuUtility.FindMatchingSISshader(currentShader);

            // TODO handle when finding multiple matching

            //if (foundShader)
            //{
            //    Debug.Log("Already existing " + AssetSorceryShader.ASSETSORCERY_FILE_EXTENSION + " Found: " + foundShader.name, foundShader);
            //    Selection.activeObject = foundShader;
            //}
            //else
            {
                //Debug.Log("No existing " + AssetSorceryShader.ASSETSORCERY_FILE_EXTENSION + " found, creating new one");
                var currentName = currentAsset.name.Replace("/", "-");

                string dir = Path.GetDirectoryName(path);
                if (AssetSorceryGetDirectoryType.GetDirectoryType(path) == AssetSorceryGetDirectoryType.eDirectoryType.ReadOnlyPackageCache)
                {
                    Debug.LogWarning("ConvertShader: source file is within read only PackageCache, save to Assets instead");
                    dir = "Assets/";
                }

                var newPath = AssetSorceryCreationMenus.CreateAssetSorceryShader(dir + "/" + currentName + "-" + AssetSorceryShader.ASSETSORCERY_FILE_EXTENSION + "." + AssetSorceryShader.ASSETSORCERY_FILE_EXTENSION, currentAsset);
                var newAsset = AssetDatabase.LoadAssetAtPath<Shader>(newPath);
                var asshader = AssetSorceryShader.ReadFromPath(newPath);
                asshader.replaceShaders.Add(asset);
                
                if (keepGUID) {GUIDTools.ReplaceShaderGUID(asset, newAsset);}

                Selection.activeObject = newAsset;
            }
        }
    }
}
