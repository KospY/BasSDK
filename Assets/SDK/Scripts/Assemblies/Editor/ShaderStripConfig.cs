using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

namespace ThunderRoad
{
    [Serializable]
    [CreateAssetMenu(menuName = "ThunderRoad/Shaders/ShaderStripConfig")]
    public class ShaderStripConfig : ScriptableObject
    {

        [Header("Stripping")]
        public StripConfig windows;
        public StripConfig android;

        [Serializable]
        public class StripConfig
        {
            //public List<Shader> whitelist = new List<Shader>();
            public List<Shader> blacklist = new List<Shader>();
            public List<ShaderKeyword> blacklistShaderKeyword = new List<ShaderKeyword>();
            public List<string> blacklistGlobalKeywords = new List<string>();
            [NonSerialized]
            public Dictionary<Shader, HashSet<UnityEngine.Rendering.ShaderKeyword>> shaderKeywordStrip = new();
            [NonSerialized]
            public HashSet<UnityEngine.Rendering.ShaderKeyword> allShaderKeywordStrip = new();
            [NonSerialized]
            public HashSet<Shader> shaderStrip = new HashSet<Shader>();
            [NonSerialized]
            private string[] standardShaders;
            [NonSerialized]
            private bool isInitialized = false;
            [NonSerialized]
            public bool isAndroid = false;
            [NonSerialized]
            public bool isWindows = false;
            public void Init()
            {
                if (isInitialized) return;
                standardShaders = new[] { "Standard", "Standard (Specular setup)" };

                foreach (string stripKeyword in blacklistGlobalKeywords)
                {
                    allShaderKeywordStrip.Add(new UnityEngine.Rendering.ShaderKeyword(stripKeyword));
                }
                foreach (var stripShaderKeyword in blacklistShaderKeyword)
                {
                    if (stripShaderKeyword == null) continue;
                    if (stripShaderKeyword.shader == null) continue;
                    HashSet<UnityEngine.Rendering.ShaderKeyword> hashShaderKeyword = new HashSet<UnityEngine.Rendering.ShaderKeyword>();
                    foreach (var shaderKeyword in stripShaderKeyword.keywords)
                    {
                        hashShaderKeyword.Add(new UnityEngine.Rendering.ShaderKeyword(shaderKeyword));
                    }
                    shaderKeywordStrip.Add(stripShaderKeyword.shader, hashShaderKeyword);
                }

                foreach (Shader shader in blacklist)
                {
                    if (shader == null) continue;
                    shaderStrip.Add(shader);
                }
                isInitialized = true;
            }

            public bool ShouldStripFully(Shader shader, ShaderSnippetData snippet)
            {
                return shaderStrip.Contains(shader) || TryStripDeferred(snippet) || TryStripStandard(shader) || TryStripPostProcessing(shader);
            }

            public bool ShouldStripVariant(Shader shader, ShaderKeywordSet keywordSet)
            {
                return StripByGlobalKeywords(keywordSet) || StripByShaderKeywords(shader, keywordSet);
            }

            // Remove all DEFERRED passes, because we use FORWARD rendering only.
            private bool TryStripDeferred(ShaderSnippetData snippet)
            {
                return snippet.passType == UnityEngine.Rendering.PassType.Deferred;
            }

            // Remove all passes from the Standard shader. We don't use the Standard shader at all, but it's used
            // in Unity default GameObjects that we use for whiteboxing our levels.
            private bool TryStripStandard(Shader shader)
            {
                for (var i = 0; i < standardShaders.Length; i++)
                {
                    var shaderName = standardShaders[i];
                    if (string.Equals(shader.name, shaderName, System.StringComparison.Ordinal))
                    {
                        return true;
                    }
                }
                return false;
            }
            // Remove all post processing shaders on android since they are disabled anyway
            private bool TryStripPostProcessing(Shader shader)
            {
                //Strip post processing from android
                return isAndroid && (shader.name.Contains("UberPost") || shader.name.Contains("FinalPost"));
            }

            private bool StripByShaderKeywords(Shader shader, ShaderKeywordSet keywordSet)
            {
                if (shaderKeywordStrip.TryGetValue(shader, out var lookup))
                {
                    return StripByKeywords(keywordSet, lookup);
                }
                return false;
            }

            private bool StripByGlobalKeywords(ShaderKeywordSet keywordSet)
            {
                return StripByKeywords(keywordSet, allShaderKeywordStrip);
            }
            private bool StripByKeywords(ShaderKeywordSet keywordSet, IEnumerable<UnityEngine.Rendering.ShaderKeyword> keywords)
            {
                foreach (UnityEngine.Rendering.ShaderKeyword s in keywords)
                {
                    if (keywordSet.IsEnabled(s))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        [Serializable]
        public class ShaderKeyword
        {
            public Shader shader;
            public List<string> keywords = new List<string>();
        }

        public StripConfig GetStripConfig()
        {
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                //if its android, return the android config
                case BuildTarget.Android: {
                    android.isAndroid = true;
                    android.Init();
                    return android;
                }
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64: {
                    windows.isWindows = true;
                    windows.Init();
                    return windows;
                }
                default:
                    Debug.LogError($"Unknown build target: {EditorUserBuildSettings.activeBuildTarget} returning default config");
                    return new StripConfig();
            }

        }
        public static ShaderStripConfig GetSDKConfig()
        {
            ShaderStripConfig shaderStripConfig = AssetDatabase.LoadAssetAtPath<ShaderStripConfig>("Assets/SDK/Config/Rendering/Shaders/SDKShaderStripConfig.asset");
            if (shaderStripConfig == null)
            {
                shaderStripConfig = ScriptableObject.CreateInstance<ShaderStripConfig>();
                AssetDatabase.CreateAsset(shaderStripConfig, "Assets/SDK/Config/Rendering/Shaders/SDKShaderStripConfig.asset");
                AssetDatabase.SaveAssets();
            }
            return shaderStripConfig;
        }

    }
}
