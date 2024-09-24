using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if ODIN_INSPECTOR
using Sirenix.Utilities;
#endif
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using Object = UnityEngine.Object;

namespace ThunderRoad.AssetSorcery
{
    [ScriptedImporter(4, "." + ASSETSORCERY_FILE_EXTENSION)]
    public class AssetSorceryShader : AssetSorceryCommon<AssetSorceryShaderAsset, Shader>
    {
        //[Tooltip("Rename the shader using this string")]
        //public string customShaderName = "Oculus/SIS/SIShader";

        public const string ASSETSORCERY_FILE_EXTENSION = "ASshader";

        //public override string GetExtension()
        //{
        //    return ASSETSORCERY_FILE_EXTENSION;
        //}
/*
        public static Shader ProcessShaderSourceA(string source)
        {
            var package = ScriptableObject.CreateInstance<AssetSorceryShaderAsset>();

            package.shaderSettings.customShaderName = "TestName";

            bool debug = true;
            
            package.FindAllKeywords(package.shaderSettings.shaderKeywords,source, debug);

            foreach (var o in package.shaderSettings.shaderKeywords)
            {
                Debug.Log(o.displayName+":"+o.line);  
            }

            var sourceResult = ProcessShaderSource(source, package);

            var shader = ShaderUtil.CreateShaderAsset( sourceResult, false);
            //shader.name = newName;
            return shader;
        }
        
*/

        public override void OnImportAsset(AssetImportContext ctx)
        {
            var package = GetPackage(ctx);
            ctx.DependsOnArtifact(AssetDatabase.GetAssetPath(package.targetShader));
            base.OnImportAsset(ctx);
        }

        // https://docs.unity3d.com/Manual/SL-SubShaderTags.html
        public enum eRenderQueue
        {
            none,
            Custom,
            Background,
            Geometry,
            AlphaTest,
            Transparent,
            Overlay
        }

        // https://docs.unity3d.com/Manual/SL-ShaderReplacement.html
        public enum eRenderType
        {
            none,
            Custom,
            Opaque,
            Transparent,
            TransparentCutout,
            Background,
        }

        public static void ExportNormalShaderFilesForAll()
        {
            var extension = ASSETSORCERY_FILE_EXTENSION;
            var guids = AssetDatabase.FindAssets("t:shader " + extension);
            Debug.Log($"ExportNormalShaderFilesForAll: {extension}:{guids.Length}");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (Path.GetExtension(path) != "." + ASSETSORCERY_FILE_EXTENSION) continue;
                //var package = AssetDatabase.LoadAssetAtPath<AssetSorceryShaderAsset>(path);
                Debug.Log("ExportNormalShaderFilesForAll: Path: " + path);
                var package = GetPackageFromPath(path);
                if (package == null) continue;
                var outputPath = Path.GetDirectoryName(path);
                //Debug.Log("Path: " + path + " : " + outputPath);
                ExportNormalShaderFiles(package, outputPath);
            }

            AssetDatabase.Refresh();
            //AssetDatabase.LoadAssetAtPath<Shader>(writePath);
        }

        public static void ExportNormalShaderFiles(AssetSorceryShaderAsset package, string outputPath)
        {
            if (string.IsNullOrEmpty(package.shaderSettings.outputShaderFName))
            {
                Debug.LogError("ExportNormalShaderFiles: No 'outputShaderFName' specified: " + package.GetAssetPath(), package);
                return;
            }

            var shaderSource = package.GetSource();

            //var customEditor = "RPOpenGraphGUI"; // Set to null if you want to keep shadergraph markdown, else it replaces the editor with OpenGraph

            foreach (AssetSorceryPlatformRuntime.ePlatformAS platform in Enum.GetValues(typeof(AssetSorceryPlatformRuntime.ePlatformAS)))
            {
                if (platform == AssetSorceryPlatformRuntime.ePlatformAS.Auto) continue;

                var outName = package.shaderSettings.outputShaderFName + "-" + platform.ToString();
                var outNameShader = package.shaderSettings.customShaderName + " - " + platform.ToString();

                //var sourceResult = AssetSorceryShader.ProcessShaderSource(platform, shaderSource, package, package.targetShader.name, outNameShader, customEditor);
                var sourceResult = AssetSorceryShader.ProcessShaderSource(platform, shaderSource, package, package.targetShader.name, outNameShader);

                var writePath = outputPath + "/" + outName;
                if (!writePath.EndsWith(".shader")) writePath += ".shader";

                Debug.Log("Write file: " + writePath);
                File.WriteAllText(writePath, sourceResult);

                //AssetDatabase.Refresh();
                //AssetDatabase.LoadAssetAtPath<Shader>(writePath);
            }
        }

        public static string ProcessShaderSource(string source, AssetSorceryShaderAsset package, string oldName, string newNameIn = "", string customEditor = "")
        {
            var platform = AssetSorceryPlatform.AssetSorceryGetPlatformCalculated();
            return ProcessShaderSource(platform, source, package, oldName, newNameIn, customEditor);
        }

        public static string ProcessShaderSource(AssetSorceryPlatformRuntime.ePlatformAS platform, string source, AssetSorceryShaderAsset package, string oldName, string newNameIn = "", string customEditor = "")
        {
            if (string.IsNullOrEmpty(source)) source = SOURCE_MISSING_SHADER;

            var debug = package.debug;
            var batchMode = Application.isBatchMode;
            var newName = package.shaderSettings.customShaderName;
            if (!string.IsNullOrEmpty(newNameIn)) newName = newNameIn;

            if (batchMode) AssetSorceryPlatform.SetEditorMode(false, false);

            if (!batchMode) Debug.Log($"ProcessShaderSource: {debug}:{oldName} -> {newName}");

            source = $"// ProcessShaderSource // {oldName} -> {newName}\n{source}";

            source = source.Replace($"Shader \"{oldName}\"", $"Shader \"{newName}\"");


            //replace \r\n and \r with \n and count lines
            source = source.Replace("\r\n", "\n");
            source = source.Replace("\r", "\n");

            //if (!package.shaderSettings.tessellation) source = source.Replace("#define SW_TESSELLATION", "//#define SW_TESSELLATION // AS-DISABLED");
            //if (!package.shaderSettings.tessellation) source = source.Replace("#if defined(SW_TESSELLATION)", "#if defined(SW_TESSELLATION) // AS-DISABLED");

            bool enableTess = false;
            switch (platform)
            {
                //case AssetSorceryPlatform.ePlatformAS.auto:
                //    enableTess = true;
                //    break;
                case AssetSorceryPlatformRuntime.ePlatformAS.Desktop:
                    if (package.shaderSettings.tessellationDesktop) enableTess = true;
                    break;
                case AssetSorceryPlatformRuntime.ePlatformAS.Mobile:
                    if (package.shaderSettings.tessellationMobile) enableTess = true;
                    break;
            }

            if (!enableTess)
            {
                source = source.Replace("#define ASE_TESSELLATION 1", "//#define ASE_TESSELLATION 1 // AS-DISABLED-TESS");
                source = source.Replace("#pragma require tessellation tessHW", "//#pragma require tessellation tessHW // AS-DISABLED-TESS");
                source = source.Replace("#pragma hull HullFunction", "//#pragma hull HullFunction // AS-DISABLED-TESS");
                source = source.Replace("#pragma domain DomainFunction", "//#pragma domain DomainFunction // AS-DISABLED-TESS");
                source = source.Replace("#define ASE_DISTANCE_TESSELLATION", "//#define ASE_DISTANCE_TESSELLATION // AS-DISABLED-TESS");
            }

            if (!string.IsNullOrEmpty(customEditor))
            {
                source = source.Replace("CustomEditor \"Needle.MarkdownShaderGUI\"", "CustomEditor \"" + customEditor + "\"");
            }


            /*
           bool forceOpaque = false;
           switch (platform)
           {
               //case AssetSorceryPlatform.ePlatformAS.auto:
               //    enableTess = true;
               //    break;
               case AssetSorceryPlatformRuntime.ePlatformAS.Desktop:
                   if (package.shaderSettings.forceOpaqueDesktop) forceOpaque = true;
                   break;
               case AssetSorceryPlatformRuntime.ePlatformAS.Mobile:
                   if (package.shaderSettings.forceOpaqueMobile) forceOpaque = true;
                   break;
           }
           // https://docs.unity3d.com/Manual/SL-SubShaderTags.html
           if (forceOpaque)
           {
               source = source.Replace("\"Queue\"=\"Transparent\"","\"Queue\"=\"Geometry\"");
           }
           //else
           //{
           //    source = source.Replace("\"\"Queue\"=\"Opaque\"\"","\"\"Queue\"=\"Transparent\"\"");
           //}
           */

            var lines = source.Split(
                new string[] {"\r\n", "\r", "\n"},
                StringSplitOptions.None
            ).ToList();

            if (!batchMode) Debug.Log($"ProcessShaderSource: Lines: {lines.Count}");


            var shader = package.GetInputAsset(true);
            if (!batchMode) Debug.Log($"ProcessShaderSource: Shader: {shader.name} platform:{platform}");

            bool willRecombineSource = false;

            foreach (var passItem in package.shaderSettings.passItems)
            {
                if (!passItem.present) continue;
                //if (passItem.enabled) continue;

                var enableThis = false;

                switch (platform)
                {
                    case AssetSorceryPlatformRuntime.ePlatformAS.Auto:
                        enableThis = true;
                        break;
                    case AssetSorceryPlatformRuntime.ePlatformAS.Desktop:
                        if (passItem.enabledDesktop) enableThis = true;
                        break;
                    case AssetSorceryPlatformRuntime.ePlatformAS.Mobile:
                        if (passItem.enabledMobile) enableThis = true;
                        break;
                }

                if (enableThis) continue;

                if (!batchMode)
                    Debug.Log($"ProcessShaderSource: Remove Pass A: {passItem.passName}");

                int passLineIndex = 0;
                int lastEndBracket = 0;
                int lastEndBracket2 = 0;
                int lastEndBracket3 = 0;
                bool passReplaceTrack = false;

                for (int i = 0; i < lines.Count; i++)
                {
                    bool finishEarly = false;

                    var line = lines[i];

                    //if (line.Contains("*ASEBEGIN")) finishEarly = true; // /*ASEBEGIN

                    line = line.Replace("\t", "");
                    line = line.Replace(" ", "");

                    if (line.Contains("}"))
                    {
                        lastEndBracket3 = lastEndBracket2;
                        lastEndBracket2 = lastEndBracket;
                        lastEndBracket = i;
                    }

                    var passFound = false;
                    if (line == ("Pass{")) passFound = true;
                    if (!passFound && line == ("Pass"))
                    {
                        var line2 = lines[i + 1];
                        line2 = line2.Replace("\t", "");
                        line2 = line2.Replace(" ", "");
                        if (line2.StartsWith("{")) passFound = true;
                    }


                    if (i >= lines.Count - 1 || finishEarly)
                    {
                        //Debug.Log("End: " + i +":"+passReplaceTrack);
                        if (passReplaceTrack) // Waiting to find end of pass, but reached end of document
                        {
                            passFound = true;
                            if (finishEarly) lastEndBracket = lastEndBracket3;
                        }
                    }


                    if (passFound)
                    {
                        //passesLastEndBracket = lastEndBracket;
                        passLineIndex = i;
                        if (passReplaceTrack)
                        {
                            passReplaceTrack = false;
                            lines.Insert(lastEndBracket + 1, $"// Pass End: {passItem.passName} */");
                            //i++;
                        }
                    }

                    if (line.Contains($"Name\"{passItem.passName}\""))
                    {
                        lines[passLineIndex] = $"/* {lines[passLineIndex]} // Pass Start: {passItem.passName}";
                        willRecombineSource = true;
                        passReplaceTrack = true;
                    }

                    if (finishEarly) break;
                }
            }

            for (int i = 0; i < lines.Count; i++)
            {
                bool finishEarly = false;
                var line = lines[i];

                // Remove amplify source/node data from shader
                if (line.StartsWith("/*ASEBEGIN")) finishEarly = true; // /*ASEBEGIN

                if (finishEarly)
                {
                    willRecombineSource = true;
                    
                    lines.RemoveRange(i,lines.Count-(i));
                    
                    //lines.SetLength(i);
                    //Debug.Log("Hello: " + i + ":" + lines.Count);
                }
            }


            if (willRecombineSource)
            {
                source = string.Join("\n", lines);
            }

                
            foreach (var o in package.shaderSettings.shaderKeywords)
            {
                var enableThis = false;
                var hideThis = false; // hide the properties from inspector, but keep the keyword and any replacements

                switch (platform)
                {
                    case AssetSorceryPlatformRuntime.ePlatformAS.Auto:
                        enableThis = true;
                        hideThis = false;
                        break;
                    case AssetSorceryPlatformRuntime.ePlatformAS.Desktop:
                        if (o.enabledDesktop) enableThis = true;
                        if (o.hideDesktop) hideThis = true;
                        break;
                    case AssetSorceryPlatformRuntime.ePlatformAS.Mobile:
                        if (o.enabledMobile) enableThis = true;
                        if (o.hideMobile) hideThis = true;
                        break;
                }

                // Loops thru all the keywords, if we are processin for 'desktop' platform and the keyword is set to be disabled for that platform then 'enableThis' is set to false
                // If 'enableThis' is false or 'hideThis' is true we loop thru all lines of source to hide the properties from the material inspector

                if (!enableThis)
                {
                    if (source.Contains(o.line)) // Find the exact line for the keyword and remove/replace it
                    {
                        if (debug && !batchMode) Debug.Log($"ProcessShaderSource: Removing: {o.line}");
                        source = source.Replace(o.line, $"// AS-REMOVED // {o.line}");
                    }

                    foreach (var line in lines) // Loop thru all lines and hide properties related to the keyword
                    {
                        if (line.Contains("/*ASEBEGIN")) break;
                        if (line.Contains("[Toggle(" + o.keywordLast + ")]"))
                            //if (line.Contains(o.keywordLast) && line.Contains("[") && line.Contains("]"))
                        {
                            if (!batchMode)
                                Debug.Log($"ProcessShaderSource: Hide Prop: {o.keywordLast} : {line}");
                            source = source.Replace(line, $"[HideInInspector]{line}// AS-REPLACED-PROP\n");
                        }
                    }
                }
                else
                {
                    bool replaceValid = false;
                    if (!string.IsNullOrEmpty(o.replaceDesktop) && platform == AssetSorceryPlatformRuntime.ePlatformAS.Desktop || !string.IsNullOrEmpty(o.replaceMobile) && platform == AssetSorceryPlatformRuntime.ePlatformAS.Mobile) replaceValid = true;

                    var editorMode = AssetSorceryPlatform.GetEditorMode();
                    if (editorMode && !string.IsNullOrEmpty(o.replaceEditor)) replaceValid = true;

                    // TODO simplify if/switch
                    if (replaceValid)
                    {
                        //if(!source.Contains(o.line))Debug.LogError("Line not found: '" + o.line +"'");
                        //if(!source.Contains(o.line + "\n"))Debug.LogError("Line not found 2: '" + o.line +"'");
                        //if(!source.Contains(o.line + "\r"))Debug.LogError("Line not found 3: '" + o.line +"'");

                        switch (platform)
                        {
                            case AssetSorceryPlatformRuntime.ePlatformAS.Desktop:
                                var replace = o.replaceDesktop;
                                if (editorMode && !string.IsNullOrEmpty(o.replaceEditor)) replace = o.replaceEditor;

                                source = source.Replace($"{o.line}\n", $"{replace}// AS-REPLACED // {o.line}\n");
                                //source = source.Replace(o.line + "\r", o.replaceDesktop + "// AS-REPLACED // " + o.line + "\n");
                                break;
                            case AssetSorceryPlatformRuntime.ePlatformAS.Mobile:
                                var replace2 = o.replaceMobile;
                                if (editorMode && !string.IsNullOrEmpty(o.replaceEditor)) replace2 = o.replaceEditor;

                                source = source.Replace($"{o.line}\n", $"{replace2}// AS-REPLACED // {o.line}\n");
                                //source = source.Replace(o.line + "\r", o.replaceMobile + "// AS-REPLACED // " + o.line + "\n");
                                break;
                        }
                    }
                    else
                    {
                        if (o.keywordTypeReplace != o.keywordType && o.keywordTypeReplace != AssetSorceryShaderKeyword.eKeywordType.none)
                        {
                            if (debug && !batchMode) Debug.Log($"ProcessShaderSource: KeywordType change: {o.keywordType} -> {o.keywordTypeReplace}");
                            var local = "";
                            if (o.local) local = "_local";
                            if (o.keywordType == AssetSorceryShaderKeyword.eKeywordType.shader_feature)
                            {
                                source = source.Replace($"#pragma {o.keywordType}{local} {o.keywordName}", $"#pragma {o.keywordTypeReplace}{local} __ {o.keywordName}// AS-KEYWORDTYPE CHANGED FROM {o.keywordType} // ");
                            }

                            if (o.keywordType == AssetSorceryShaderKeyword.eKeywordType.multi_compile)
                            {
                                source = source.Replace($"#pragma {o.keywordType}{local} __ {o.keywordName}", $"#pragma {o.keywordTypeReplace}{local} {o.keywordName}// AS-KEYWORDTYPE CHANGED FROM {o.keywordType} // ");
                            }

                            //source = source.Replace("[Toggle(" + o.keywordName + ")]", "[HideInInspector]");
                        }
                    }
                }


                if (!enableThis || hideThis)
                {
                    var needle = $"[Toggle({o.keywordLast}_ON)]";
                    if (source.Contains(needle)) source = source.Replace(needle, "[HideInInspector]");

                    var needle2 = $"[Feature({o.keywordLast})]";
                    if (source.Contains(needle2)) source = source.Replace(needle2, "// AS-FEATURE OFF //"); // Was just [HideInInspector]

                    var needle4 = $"[FeatureCommentOut({o.keywordLast})]";
                    if (source.Contains(needle4)) source = source.Replace(needle4, "// AS-FEATURE COMMENT OUT //");

                    var needle3 = $"[ShowIfDrawer({o.keywordLast})]";
                    if (source.Contains(needle3)) source = source.Replace(needle3, "[HideInInspector]");

                    var needle5 = $"[HideIfDrawer({o.keywordLast})]";
                    if (source.Contains(needle5)) source = source.Replace(needle5, "[HideInInspector]");
                }
            }

            //Debug.Log("Source length: " + source.Length);
            //var shader = ShaderUtil.CreateShaderAsset( source, false);
            //shader.name = newName;
            return source;
        }

        // package is read only here


        //  public override Object ProcessSourceToObject(AssetImportContext ctx, AssetSorceryShaderAsset package, AssetSorceryAssetCommon<Shader>.EntryCommon entry, string source, Shader entryItem)
        public override Object ProcessSourceToObject(AssetImportContext ctx, AssetSorceryShaderAsset package, AFilterCommon<Shader> entry, string source, Shader entryItem)
        {
            source = ProcessShaderSource(source, package, entryItem.name);
            return ShaderUtil.CreateShaderAsset(ctx, source, false);
        }

        private const string SOURCE_MISSING_SHADER = @"Shader ""Hidden/MissingShader""
{
    SubShader
    {
        Tags
        {
            ""RenderType""=""Opaque""
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include ""UnityCG.cginc""

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return fixed4(1, 0, 0, 1);
            }
            ENDCG
        }
    }
}";
    }
}
