using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ThunderRoad.AssetSorcery
{
    public static class AssetSorceryConvertMenuUtility
    {
        public const string ASSETSORCERY_CTXMENU_PREFIX = "Assets/AssetSorcery - ";

        /// <summary>
        /// Find a matching SISShaderAsset from passed in Shader
        /// </summary>
        private static AssetSorceryShaderAsset FindMatchingSISShaderAsset(Shader currentShader)
        {
            var allShaders = AssetSorceryCommonItems.FindAssetsByType<Shader>();
            foreach (var shader in allShaders)
            {
                var path2 = AssetDatabase.GetAssetPath(shader);
                if (!path2.EndsWith(AssetSorceryShader.ASSETSORCERY_FILE_EXTENSION)) continue;

                var package = AssetSorceryShader.ReadFromPath(path2);
                if (package == null) continue;

                foreach (var packageEntry in package.GetFilterEntries())
                {
                    if (packageEntry.GetItem() == currentShader) return package;
                }
            }

            return null;
        }

        /// <summary>
        /// Find all AssetSorceryShaderAsset
        /// </summary>
        private static List<AssetSorceryShaderAsset> FindAllAssetSorceryShaderAsset()
        {
            var result = new List<AssetSorceryShaderAsset>();
            var allShaders = AssetSorceryCommonItems.FindAssetsByType<Shader>();
            foreach (var shader in allShaders)
            {
                var path2 = AssetDatabase.GetAssetPath(shader);
                if (!path2.EndsWith(AssetSorceryShader.ASSETSORCERY_FILE_EXTENSION)) continue;
                var package = AssetSorceryShader.ReadFromPath(path2);
                if (package == null) continue;
                result.Add(package);
            }

            return result;
        }

        // <summary>
        /// Find all AssetSorceryShaderAsset and get all Shaders
        /// </summary>
        public static List<Shader> FindAllAssetSorceryShaderAssetShaders()
        {
            var result = new List<Shader>();
            var all = FindAllAssetSorceryShaderAsset();
            foreach (AssetSorceryShaderAsset sisShaderAsset in all)
            {
                foreach (var entryCommon in sisShaderAsset.GetFilterEntries())
                {
                    if (!result.Contains(entryCommon.GetItem())) result.Add(entryCommon.GetItem());
                }
            }

            return result;
        }

        // TODO split out to partial class or something

        /// <summary>
        /// Checks if item at path is a Material, is using ASshader, is not a ASshader asset file and is not in a read only package.
        /// </summary>
        public static bool IsItemMaterialAndASshader(string path)
        {
            var assetType = AssetDatabase.GetMainAssetTypeAtPath(path);
            if (assetType != typeof(Material)) return false;

            var asset = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (asset == null) return false;

            var shaderPath = AssetDatabase.GetAssetPath(asset.shader);
            if (!shaderPath.EndsWith(AssetSorceryShader.ASSETSORCERY_FILE_EXTENSION)) return false;

            if (AssetSorceryGetDirectoryType.GetDirectoryType(path) == AssetSorceryGetDirectoryType.eDirectoryType.ReadOnlyPackageCache) return false;

            return true;
        }
    }
}
