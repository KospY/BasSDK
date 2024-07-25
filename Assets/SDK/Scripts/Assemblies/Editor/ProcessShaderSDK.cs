using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Rendering;
using UnityEngine;

namespace ThunderRoad
{
    public class ProcessShaderSDK : IPreprocessShaders
    {
        private ShaderStripConfig shaderStripConfig;
        private ShaderStripConfig.StripConfig stripConfig;
        public ProcessShaderSDK()
        {
            shaderStripConfig = ShaderStripConfig.GetSDKConfig();
            if (shaderStripConfig == null)
            {
                Debug.LogError("ShaderStripPreProcess: ShaderStripConfig is null, please check the configuration file.");
                return;
            }
            stripConfig = shaderStripConfig.GetStripConfig();
        }


        public int callbackOrder => 0;

        public void OnProcessShader(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> data)
        {
            if (shaderStripConfig == null) return;
            if (stripConfig.ShouldStripFully(shader, snippet)) data.Clear();
            for (int i = 0; i < data.Count; i++)
            {
                //check if we should strip this variant
                if (!stripConfig.ShouldStripVariant(shader, data[i].shaderKeywordSet)) continue;
                data.RemoveAt(i--);
            }
        }
    }
}
