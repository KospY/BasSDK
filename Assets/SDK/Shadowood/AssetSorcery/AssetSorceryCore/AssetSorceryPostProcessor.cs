using System;
using UnityEditor;
using UnityEngine;

namespace ThunderRoad.AssetSorcery
{
    class AssetSorceryPostProcessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            RegisterShaders(importedAssets);
        }

        static void RegisterShaders(string[] paths)
        {
            foreach (var assetPath in paths)
            {
                if (!assetPath.EndsWith("." + AssetSorceryShader.ASSETSORCERY_FILE_EXTENSION, StringComparison.InvariantCultureIgnoreCase)) continue;

                var mainObj = AssetDatabase.LoadMainAssetAtPath(assetPath) as Shader;

                if (mainObj != null)
                {
                    ShaderUtil.ClearShaderMessages(mainObj);
                    if (!ShaderUtil.ShaderHasError(mainObj)) ShaderUtil.RegisterShader(mainObj);
                }

                foreach (var obj in AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath))
                {
                    if (obj is Shader shader)
                    {
                        Debug.Log("obj:" + shader.name);
                        ShaderUtil.ClearShaderMessages(shader);
                        if (!ShaderUtil.ShaderHasError(shader)) ShaderUtil.RegisterShader(shader);
                    }
                }
            }
        }
    }
}
