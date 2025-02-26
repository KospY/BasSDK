using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace ThunderRoad.AssetSorcery
{
    [CustomEditor(typeof(AssetSorceryShader))]
    public class AssetSorceryShaderEditor : AssetSorceryEditorCommon<AssetSorceryShaderAsset, Shader, AFilterCommon<Shader>>
    {
        public int variantCountStripped, variantCountTotal;
        public int variantCountStrippedOriginal, variantCountTotalOriginal;

        protected override void Apply()
        {
            //Debug.Log("AssetSorceryShaderEditor: Apply");
            base.Apply();
        }

        private AssetSorceryShaderAsset itemDataRef;

        protected override void InitializeExtraDataInstance(Object extraTarget, int targetIndex)
        {
            //Debug.Log("AssetSorceryShaderEditor: InitializeExtraDataInstance");
            base.InitializeExtraDataInstance(extraTarget, targetIndex);
            if (extraDataSerializedObject == null || extraDataSerializedObject.targetObject == null || target == null) return;
            var itemData = extraDataSerializedObject.targetObject as AssetSorceryShaderAsset;

            var shader = itemData.GetInputAsset(false);
            itemData.shaderSettings.passItems = itemData.FindAllPasses(itemData.shaderSettings.passItems, shader, itemData.GetSource());

            itemData.shaderSettings.shaderKeywords = itemData.FindAllKeywords(itemData.shaderSettings.shaderKeywords, itemData.GetSource());

            itemDataRef = itemData;
            //

            UpdateVariantCount();
        }

        public static int GetVariantCount(Shader shaderIn, bool strip)
        {
            System.Type t = typeof(ShaderUtil);
            // internal static extern ulong GetVariantCount(Shader s, bool usedBySceneOnly);
            MethodInfo dynMethod = t.GetMethod("GetVariantCount", BindingFlags.NonPublic | BindingFlags.Static);

            int variantCount = (int) ((ulong) dynMethod.Invoke(null, new object[] {shaderIn, strip}));

            //Debug.Log("variantCount: " + variantCount + ":" + shaderIn.name);
            return variantCount;
        }

        private void OpenSourceGen()
        {
            var itemData = extraDataSerializedObject.targetObject as AssetSorceryShaderAsset;
            var item = target as AssetSorceryShader;
            //var shader = itemData.GetInputAsset();
            var shader = AssetDatabase.LoadAssetAtPath<Shader>(item.assetPath);
            var shaderSource = itemData.GetSource();

            var package = itemData;

            //

            var sourceResult = AssetSorceryShader.ProcessShaderSource(true,shaderSource, package, shader.name);

            //

            var nom = package.shaderSettings.customShaderName;
            nom = nom.Replace("/", "-");

            var writePath = Directory.GetParent(Application.dataPath) + "/Temp/Compiled-" + nom + "-Source.shader";
            Debug.Log("Write file: " + writePath);
            File.WriteAllText(writePath, sourceResult);

            Application.OpenURL(writePath);
        }

        private void GetPasses()
        {
            Debug.Log("GetPasses");
            var itemData = extraDataSerializedObject.targetObject as AssetSorceryShaderAsset;
            var shader = itemData.GetInputAsset(false);
            itemData.shaderSettings.passItems = itemData.FindAllPasses(itemData.shaderSettings.passItems, shader, itemData.GetSource());
        }

        private void GetStatsCurrent(bool allKeywords)
        {
            var itemData = extraDataSerializedObject.targetObject as AssetSorceryShaderAsset;
            var item = target as AssetSorceryShader;
            //var shader = itemData.GetInputAsset();
            var shader = AssetDatabase.LoadAssetAtPath<Shader>(item.assetPath);
            //var shaderSource = itemData.GetSource();


            var package = itemData;

            //

            //var sourceResult = AssetSorceryShader.ProcessShaderSource(shaderSource, package, shader.name,false);

            //

            var nom = package.shaderSettings.customShaderName;
            nom = nom.Replace("/", "-");

            //var writePath = Directory.GetParent(Application.dataPath) + "/Temp/Compiled-" + nom + "-Source.shader";
            //Debug.Log("Write file: " + writePath);
            //File.WriteAllText(writePath, sourceResult);

            //
            var shaderResult = shader;
            //var shaderResult = ShaderUtil.CreateShaderAsset(sourceResult, false);
            //shaderResult.name = package.shaderSettings.customShaderName;

            //Debug.Log("Shader result: " + shaderResult.name + ":" + package.shaderSettings.customShaderName);

            //

            System.Type t = typeof(ShaderUtil);
            MethodInfo dynMethod = t.GetMethod("OpenCompiledShader", BindingFlags.NonPublic | BindingFlags.Static);

            var platformsMask = 16; // D3D
            platformsMask = 512; // GLES3x
            //platformsMask = 8192; // Vulkan
            var currentMode = 3; // Custom

            //int defaultMask = (1 << System.Enum.GetNames(typeof(UnityEditor.Rendering.ShaderCompilerPlatform)).Length - 1);
            //platformsMask = EditorPrefs.GetInt("ShaderInspectorPlatformMask", defaultMask);
            //currentMode = EditorPrefs.GetInt("ShaderInspectorPlatformMode", 1);
            //Debug.Log("platformsMask: " + platformsMask + " currentMode: " + currentMode);

            var includeAllVariants = false;
            var preprocessOnly = false;
            var stripLineDirectives = false;

            if (allKeywords) includeAllVariants = true;

            dynMethod.Invoke(null, new object[] {shaderResult, currentMode, platformsMask, includeAllVariants, preprocessOnly, stripLineDirectives});

            var compiledSource = ReadCompiledShader(shaderResult);

            //var instructionsCount = ParseInstructions(compiledSource);


            var instructionsCount = ShaderToMali(compiledSource, package.shaderSettings.customShaderName, out string malilog);

            //package.mali = malilog;

            Debug.Log("Instructions: " + instructionsCount);
        }

        private void GetStatsAll(bool allKeywords)
        {
            var itemData = extraDataSerializedObject.targetObject as AssetSorceryShaderAsset;
            //var shader = itemData.entries.First().item;
            //var shader = itemData.GetEntryForCurrentRP().item;
            var shader = itemData.GetInputAsset(false);
            var shaderSource = itemData.GetSource();

            //var path = AssetDatabase.GetAssetPath(shader);
            //var shaderSource = File.ReadAllText(path);
            //Debug.Log("path: " + path);

            //Debug.Log(shaderSource);
            //var shaderResult = AssetSorceryShader.ProcessShaderSourceA(shaderSource);

            var package = ScriptableObject.CreateInstance<AssetSorceryShaderAsset>();

            package.targetShader = shader;

            package.name = "Hello";

            bool debug = false;

            package.shaderSettings.shaderKeywords = package.FindAllKeywords(package.shaderSettings.shaderKeywords, shaderSource, debug);

            package.name = shader.name;

            Debug.Log("itemData: " + itemData.name + ":" + package.name + ":" + shader.name);
            foreach (var o in package.shaderSettings.shaderKeywords)
            {
                //Debug.Log(o.displayName + ":" + o.line);
            }

            float instructionAllCount = 0;
            for (int i = -1; i < package.shaderSettings.shaderKeywords.Count; i++)
            {
                string keywordName = "All";
                if (i > 0)
                {
                    keywordName = package.shaderSettings.shaderKeywords[i].keywordName;
                    //if (keywordName != "_MOSS") continue;
                    //if (!package.shaderSettings.shaderKeywords[i].enabled) continue;
                    keywordName = keywordName.Replace("_", "");
                    keywordName = keywordName.Replace(" ", "");
                    Debug.Log("Compile minus one keyword: " + keywordName);
                    for (int j = 0; j < package.shaderSettings.shaderKeywords.Count; j++)
                    {
                        package.shaderSettings.shaderKeywords[j].enabledDesktop = j != i;
                    }

                    package.ForceSave();
                    for (int j = 0; j < package.shaderSettings.shaderKeywords.Count; j++)
                    {
                        Debug.Log(keywordName + " - " + package.shaderSettings.shaderKeywords[j].keywordName + ":" + package.shaderSettings.shaderKeywords[j].enabledDesktop);
                    }
                }

                package.shaderSettings.customShaderName = keywordName;

                var sourceResult = AssetSorceryShader.ProcessShaderSource(true,shaderSource, package, shader.name);

                sourceResult = sourceResult.Replace("shader_feature_local", "multi_compile");
                sourceResult = sourceResult.Replace("shader_feature", "multi_compile");

                var writePath = Directory.GetParent(Application.dataPath) + "/Temp/Compiled-" + package.shaderSettings.customShaderName + "-Source.shader";
                Debug.Log("Write file: " + writePath);
                File.WriteAllText(writePath, sourceResult);


                //

                var shaderResult = ShaderUtil.CreateShaderAsset(sourceResult, false);
                shaderResult.name = package.shaderSettings.customShaderName;

                Debug.Log("Shader result: " + shaderResult.name + ":" + package.shaderSettings.customShaderName);

                //

                //Debug.Log(shader.name + " : " + shaderResult.name);
                //var variantCount = GetVariantCount(shaderResult, true);
                //Debug.Log("variantCount: " + variantCount);

                System.Type t = typeof(ShaderUtil);
                MethodInfo dynMethod = t.GetMethod("OpenCompiledShader", BindingFlags.NonPublic | BindingFlags.Static);
                //int defaultMask = (1 << System.Enum.GetNames(typeof(UnityEditor.Rendering.ShaderCompilerPlatform)).Length - 1);

                var platformsMask = 16; // D3D
                platformsMask = 512; // GLES3x
                //platformsMask = 8192; // Vulkan
                var currentMode = 3; // Custom

                int defaultMask = (1 << System.Enum.GetNames(typeof(UnityEditor.Rendering.ShaderCompilerPlatform)).Length - 1);
                //platformsMask = EditorPrefs.GetInt("ShaderInspectorPlatformMask", defaultMask);
                //currentMode = EditorPrefs.GetInt("ShaderInspectorPlatformMode", 1);
                //Debug.Log("platformsMask: " + platformsMask + " currentMode: " + currentMode);

                var includeAllVariants = false;
                var preprocessOnly = false;
                var stripLineDirectives = false;

                if (allKeywords) includeAllVariants = true;

                dynMethod.Invoke(null, new object[] {shaderResult, currentMode, platformsMask, includeAllVariants, preprocessOnly, stripLineDirectives});

                var compiledSource = ReadCompiledShader(shaderResult);

                //var instructionsCount = ParseInstructions(compiledSource);


                var instructionsCount = ShaderToMali(compiledSource, package.shaderSettings.customShaderName, out string malilog);


                if (i < 0)
                {
                    instructionAllCount = instructionsCount;
                    Debug.Log("Instructions for all: " + instructionAllCount);
                }
                else
                {
                    var calcCount = (instructionAllCount - instructionsCount);
                    Debug.Log("Instructions without: " + keywordName + " : " + calcCount + " / " + instructionAllCount);

                    foreach (var o in itemData.shaderSettings.shaderKeywords)
                    {
                        if (o.keywordName == package.shaderSettings.shaderKeywords[i].keywordName)
                        {
                            o.instructions = calcCount;
                            o.mali = malilog;
                        }
                    }
                }
            }

            //itemData.ForceSave();
            //foreach (var o in itemDataRef.shaderSettings.shaderKeywords)
            //{
            //    Debug.Log(o.displayName +":"+o.keywordName);
            //}
        }

        private float ShaderToMali(string source, string fileName, out string log)
        {
            // \#ifdef FRAGMENT\s+(.*?)(}+\s*\#endif)/gis
            // http://regexstorm.net/tester
            // https://regexr.com/

            var regex = new Regex(@"\#ifdef FRAGMENT\s+(.*?)(}+\s*\#endif)", RegexOptions.Singleline);
            var matchitems = regex.Matches(source);

            Debug.Log("ShaderToMali: Split found: " + matchitems.Count);

            log = "";

            float instructionsEmitted = 0;

            for (int i = 0; i < matchitems.Count; i++)
            {
                var split = matchitems[i].Value.Split(new[] {'\r', '\n'}).ToList();
                split.RemoveAt(0);
                split.RemoveAt(split.Count - 1);
                var combined = String.Join(separator: "\n", split.ToArray());

                // TODO get pass name instead of numbering them?

                fileName = fileName.Replace("/", "-");

                var writePath = Directory.GetParent(Application.dataPath) + "/Temp/Compiled-" + fileName + "-Mari-" + i + ".frag";
                Debug.Log("Write file: " + writePath);
                File.WriteAllText(writePath, combined);

                //

                //https://developer.arm.com/Tools%20and%20Software/Arm%20Performance%20Studio#Downloads
                //string fullPath = @"C:\Program Files\Arm\Mali Developer Tools\Mali Offline Compiler v6.4.0\malisc.exe";
                //string fullPath = @"C:\Program Files\Arm\Arm Mobile Studio 2022.4\mali_offline_compiler\malioc.exe";
                string fullPath = @"C:\Program Files\Arm\Arm Performance Studio 2024.5\mali_offline_compiler\malioc.exe";


                var maliResult = RunProcess(fullPath, false, writePath);

                var splitMali = maliResult.Split(new[] {'\r', '\n'}).ToList();

                if (i > 0) log += "------- " + i + "\n";
                foreach (var o in splitMali)
                {
                    if (!(o.Contains("Cycles:") || o.Contains("Emitted:") || o.Contains("cycles:"))) continue;
                    log += o + "\n";
                }

                //log = maliResult;


                // :\s+[0-9,'.']+
                //var regex2 = new Regex(@"Emitted:\s+[0-9,'.']+");
                var regex2 = new Regex(@"cycles:\s+[0-9,'.']+");
                var matchitems2 = regex2.Matches(maliResult);

                foreach (Match matchitem in matchitems2)
                {
                    //var rep = matchitem.Value.Replace("Emitted:", "");
                    var rep = matchitem.Value.Replace("cycles:", "");
                    Debug.Log("matchitem: rep: " + rep);
                    var emitted = float.Parse(rep);
                    Debug.Log("matchitem: " + rep + ":" + emitted);
                    Debug.Log("Mali results: " + i + ":" + matchitems2.Count + " : " + emitted);
                    instructionsEmitted += emitted;
                }

                //break; // Just use the first found fragment, 
            }

            Debug.Log("ShaderToMali: Total instructionsEmitted: " + instructionsEmitted);

            return instructionsEmitted;
        }

        /// <summary>
        /// https://forum.unity.com/threads/execute-an-exe-on-windows-from-the-editor-and-read-its-return-code.984515/
        /// </summary>
        static string RunProcess(string command, bool runShell, string args = null)
        {
            string projectCurrentDir = Directory.GetCurrentDirectory();
            //command = projectCurrentDir + "/" + command;

            UnityEngine.Debug.Log(string.Format("{0} Run command: {1}", DateTime.Now, command));

            ProcessStartInfo ps = new ProcessStartInfo(command);
            ps.WindowStyle = ProcessWindowStyle.Hidden;
            using (Process p = new Process())
            {
                ps.UseShellExecute = runShell;
                if (!runShell)
                {
                    ps.RedirectStandardOutput = true;
                    ps.RedirectStandardError = true;
                    ps.StandardOutputEncoding = System.Text.ASCIIEncoding.ASCII;
                }

                if (args != null && args != "")
                {
                    ps.Arguments = args;
                }

                p.StartInfo = ps;
                p.Start();
                p.WaitForExit();
                if (!runShell)
                {
                    string output = p.StandardOutput.ReadToEnd().Trim();
                    if (!string.IsNullOrEmpty(output))
                    {
                        //UnityEngine.Debug.Log(string.Format("{0} Output: {1}", DateTime.Now, output));
                        return output;
                    }

                    string errors = p.StandardError.ReadToEnd().Trim();
                    if (!string.IsNullOrEmpty(errors))
                    {
                        UnityEngine.Debug.LogError(string.Format("{0} Output: {1}", DateTime.Now, errors));
                    }
                }
            }

            return "";
        }

        public enum eCompilePlatform
        {
            current,
            d3d,
            gles3x,
            vulkan
        }
        
        public void CompileShader(Shader shaderIn, eCompilePlatform platform)
        {
            const bool INCLUDE_ALL_VARIANTS = false;
            System.Type t = typeof(ShaderUtil);
            MethodInfo dynMethod = t.GetMethod("OpenCompiledShader", BindingFlags.NonPublic | BindingFlags.Static);
            int defaultMask = (1 << System.Enum.GetNames(typeof(UnityEditor.Rendering.ShaderCompilerPlatform)).Length - 1);

            //https://github.com/Unity-Technologies/UnityCsReference/blob/2019.2/Editor/Mono/ShaderUtil.bindings.cs
            //extern internal static void OpenCompiledShader(Shader shader, int mode, int externPlatformsMask, bool includeAllVariants);

            //https://github.com/Unity-Technologies/UnityCsReference/blob/2020.3/Editor/Mono/ShaderUtil.bindings.cs
            //extern internal static void OpenCompiledShader(Shader shader, int mode, int externPlatformsMask, bool includeAllVariants, bool preprocessOnly, bool stripLineDirectives);

            //https://github.com/Unity-Technologies/UnityCsReference/blob/2021.3/Editor/Mono/ShaderUtil.bindings.cs
            //extern internal static void OpenCompiledShader(Shader shader, int mode, int externPlatformsMask, bool includeAllVariants, bool preprocessOnly, bool stripLineDirectives);

            //extern internal static void CompileShaderForTargetCompilerPlatform(Shader shader, ShaderCompilerPlatform platform);


#if UNITY_2020
            dynMethod.Invoke(null, new object[] {shaderIn, 1, defaultMask, INCLUDE_ALL_VARIANTS, false, false});
#else
            //dynMethod.Invoke(null, new object[] {shaderIn, 1, defaultMask, INCLUDE_ALL_VARIANTS, false, false});
            //defaultMask = (int)UnityEditor.Rendering.ShaderCompilerPlatform.D3D;
            //defaultMask = 4;
            //defaultMask =  EditorPrefs.GetInt("ShaderInspectorPlatformMask", defaultMask);
            //defaultMask = defaultMask = (1 << System.Enum.GetNames(typeof(UnityEditor.Rendering.ShaderCompilerPlatform)).Length - 1);
            //int platformMask = ShaderUtil.GetAvailableShaderCompilerPlatforms();
            //var currentMode = EditorPrefs.GetInt("ShaderInspectorPlatformMode", 1);


            //System.Type t = typeof(ShaderUtil);
            //MethodInfo dynMethod = t.GetMethod("OpenCompiledShader", BindingFlags.NonPublic | BindingFlags.Static);
            //int defaultMask = (1 << System.Enum.GetNames(typeof(UnityEditor.Rendering.ShaderCompilerPlatform)).Length - 1);

            var platformsMask = 16; // D3D
            platformsMask = 512; // GLES3x
            //platformsMask = 8192; // Vulkan
            var currentMode = 3; // Custom

            switch (platform)
            {
                case eCompilePlatform.current:
                    platformsMask = defaultMask;
                    break;
                case eCompilePlatform.d3d:
                    platformsMask = 16;
                    break;
                case eCompilePlatform.gles3x:
                    platformsMask = 512;
                    break;
                case eCompilePlatform.vulkan:
                    platformsMask = 8192;
                    break;
            }

            //platformsMask = EditorPrefs.GetInt("ShaderInspectorPlatformMask", defaultMask);
            //currentMode = EditorPrefs.GetInt("ShaderInspectorPlatformMode", 1);

            var includeAllVariants = false;
            var preprocessOnly = false;
            var stripLineDirectives = false;

            //Debug.Log("CompileShader: platformsMask: " + defaultMask + " currentMode: " + currentMode);

            dynMethod.Invoke(null, new object[] {shaderIn, currentMode, platformsMask, includeAllVariants, preprocessOnly, stripLineDirectives});

            ReadCompiledShader(shaderIn);

            //System.Type t = typeof(ShaderUtil);
            //MethodInfo dynMethod2 = t.GetMethod("CompileShaderForTargetCompilerPlatform", BindingFlags.NonPublic | BindingFlags.Static);
            //dynMethod2.Invoke(null, new object[] {shaderIn, ShaderCompilerPlatform.D3D});


#endif
        }

        public string ReadCompiledShader(Shader shaderIn)
        {
            var path = AssetDatabase.GetAssetPath(shaderIn);
            var fileName = Path.GetFileName(path);

            fileName = shaderIn.name.Replace("/", "-");
            //Debug.Log("Filename: " + fileName);

            fileName += ".shader";

            //fileName = fileName.Replace(".ASshader", ".shader");

            var tempPath = Directory.GetParent(Application.dataPath) + "/Temp/Compiled-" + fileName;
            Debug.Log("Load temp: " + tempPath);
            var contents = File.ReadAllText(tempPath);

            //File.Delete(tempPath);
            //System.IO.File.Move(tempPath, tempPath+".jpg");
            //File.Delete(tempPath+".jpg");


            return contents;
        }

        public int ParseInstructions(string textIn)
        {
            // \d+(?= instruction slots used)

            var regex = new Regex(@"\d+(?= instruction slots used)");

            MatchCollection matchitems = regex.Matches(textIn);

            int counter = 0;

            foreach (Match o in matchitems)
            {
                var val = Int32.Parse(o.Value);
                counter += val;

                //Debug.Log("Found: " + o.Value +":"+ counter);
            }

            Debug.Log("Instruction count: " + matchitems.Count + " - " + counter);
            return counter;
        }

        //public override VisualElement CreateInspectorGUI()
        //{
        //    Debug.Log("CreateInspectorGUI");
        //    UpdateVariantCount();
        //    return base.CreateInspectorGUI();
        //}

        //protected override void Awake()
        //{
        //    Debug.Log("Awake");
        //    //UpdateVariantCount();
        //    base.Awake();
        //}

        /// <summary>
        /// Ran every time you open inspector
        /// </summary>
        public void UpdateVariantCount()
        {
            var itemData = extraDataSerializedObject.targetObject as AssetSorceryShaderAsset;
            if (itemData.debug) Debug.Log("UpdateVariantCount: " + itemData.name);

            var item = target as AssetSorceryShader;
            if (item == null) return;
            var shader = AssetDatabase.LoadAssetAtPath<Shader>(item.assetPath);
            if (shader == null) return;


            variantCountStripped = GetVariantCount(shader, true);
            variantCountTotal = GetVariantCount(shader, false);
            variantCountStrippedOriginal = GetVariantCount(itemData.GetInputAsset(false), true);
            variantCountTotalOriginal = GetVariantCount(itemData.GetInputAsset(false), false);
        }

        public override void OnInspectorGUI()
        {
            if (target == null)
            {
                base.OnDisable();
                return;
            }

            //var itemData = extraDataSerializedObject.targetObject as AssetSorceryShaderAsset;

            if (GUILayout.Button(new GUIContent("CompileShader - GLES", "Not a valid shader used for debugging purposes")))
            {
                var item = target as AssetSorceryShader;
                var shader = AssetDatabase.LoadAssetAtPath<Shader>(item.assetPath);
                CompileShader(shader, eCompilePlatform.gles3x);
            }
            
            if (GUILayout.Button(new GUIContent("CompileShader - D3D", "Not a valid shader used for debugging purposes")))
            {
                var item = target as AssetSorceryShader;
                var shader = AssetDatabase.LoadAssetAtPath<Shader>(item.assetPath);
                CompileShader(shader, eCompilePlatform.d3d);
            }
            
            if (GUILayout.Button(new GUIContent("CompileShader - Vulkan", "Not a valid shader used for debugging purposes")))
            {
                var item = target as AssetSorceryShader;
                var shader = AssetDatabase.LoadAssetAtPath<Shader>(item.assetPath);
                CompileShader(shader, eCompilePlatform.vulkan);
            }

            if (GUILayout.Button(new GUIContent("OpenSourceGen", "Open a temp shader containing the resulting generated shader source after AssetSorceryShader has commented out or altered keywords")))
            {
                //var item = target as AssetSorceryShader;
                //var shader = AssetDatabase.LoadAssetAtPath<Shader>(item.assetPath);
                OpenSourceGen();
            }

            if (GUILayout.Button("GetVariantCount"))
            {
                UpdateVariantCount();
            }


            if (GUILayout.Button("GetStatsCurrent"))
            {
                GetStatsCurrent(false);
            }

            //if (GUILayout.Button("GetPasses"))
            //{
            //    GetPasses();
            //}


            if (GUILayout.Button("GetStatsAll"))
            {
                GetStatsAll(false);
            }

            if (GUILayout.Button("GetStatsAll-AllKeywords"))
            {
                GetStatsAll(true);
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Export Normal Shader Files"))
            {
                ExportNormalShaderFile();
            }

            if (GUILayout.Button("Export Normal Shader Files For All"))
            {
                AssetSorceryShader.ExportNormalShaderFilesForAll();
            }

            var style = new GUIStyle();
            style.normal.textColor = Color.white;

            GUILayout.Space(10);

            AssetSorceryPlatform.DrawPlatformButtons(target);
            GUILayout.Space(10);

            if (GUILayout.Button("ReplaceShader DEEP"))
            {
                ReplaceShaderDeep();
            }

            if (GUILayout.Button("ReplaceShader Scene"))
            {
                ReplaceShaderScene(false);
            }

            GUILayout.Space(10);

            EditorGUILayout.TextField("Platform: " + AssetSorceryPlatform.AssetSorceryGetPlatform(), style);

            EditorGUILayout.TextField("Variants New: " + variantCountStripped + " : " + variantCountTotal, style);
            EditorGUILayout.TextField("Variants Org: " + variantCountStrippedOriginal + " : " + variantCountTotalOriginal, style);


            //ApplyRevertGUI();
            //ShouldHideOpenButton();
            base.OnInspectorGUI();
        }

        private void ExportNormalShaderFile()
        {
            var itemData = extraDataSerializedObject.targetObject as AssetSorceryShaderAsset;
            var item = target as AssetSorceryShader;

            AssetSorceryShader.ExportNormalShaderFiles(itemData, Path.GetDirectoryName(item.assetPath));
            AssetDatabase.Refresh();
        }

        public int ReplaceShaderScene(bool dryRun, IEnumerable<Renderer> renderersIn = null)
        {
            var itemData = extraDataSerializedObject.targetObject as AssetSorceryShaderAsset;
            var item = target as AssetSorceryShader;
            //var newShader = AssetDatabase.LoadAssetAtPath<Shader>(item.assetPath);

            return ReplaceShaderScene2(item.assetPath, dryRun, renderersIn);
        }


        public static int ReplaceShaderScene2(string path, bool dryRun, IEnumerable<Renderer> renderersIn = null)
        {
            var itemData = AssetSorceryShader.ReadFromPath(path);

            //var itemData = extraDataSerializedObject.targetObject as AssetSorceryShaderAsset;
            //var item = target as AssetSorceryShader;
            //var newShader = itemData.targetShader;//AssetDatabase.LoadAssetAtPath<Shader>( );

            var newShader = AssetDatabase.LoadAssetAtPath<Shader>(path);

            //var asset = AssetDatabase.LoadAssetAtPath<AssetSorceryShaderAsset>(path);
            //var ass = AssetSorceryShader.GetAtPath(path);
            //var ass2 = AssetSorceryShader.ReadFromPath(path);

            //itemData.GetAssetPath()
            //Debug.Log("newShader:"+newShader.name);
//            Debug.Log("path:"+AssetDatabase.GetAssetPath(itemData));


            //return 0;

            int count = 0;

            List<Material> materials = new List<Material>();

            if (renderersIn == null || !renderersIn.Any()) renderersIn = FindObjectsOfType<Renderer>(true);

            foreach (var o in renderersIn)
            {
                foreach (var m in o.sharedMaterials)
                {
                    if (m == null) continue;
                    if (!materials.Contains(m)) materials.Add(m);
                }
            }


            foreach (var oldShader in itemData.replaceShaders)
            {
                foreach (var m in materials)
                {
                    if (m.shader == oldShader)
                    {
                        Debug.Log(m.shader + " -> " + newShader.name, m);
                        count++;
                        if (!dryRun) m.shader = newShader;
                    }
                }
            }

            return count;
        }

        private void ReplaceShaderDeep()
        {
            var itemData = extraDataSerializedObject.targetObject as AssetSorceryShaderAsset;

            var item = target as AssetSorceryShader;
            var newShader = AssetDatabase.LoadAssetAtPath<Shader>(item.assetPath);

            foreach (var oldShader in itemData.replaceShaders)
            {
                GUIDTools.ReplaceShaderGUID(oldShader, newShader);
            }
        }
    }
}
