using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThunderRoad.AssetSorcery
{
    public interface AFilterCommon<T> where T : Object
    {
        public bool FilterTest();

        public T GetItem();
        void Validate();
    }

    // TODO old, can remove now
    [Serializable]
    public class FilterPlatformOld<TT> where TT : Object
    {
        public TT item;
    }

    [Serializable]
    public class FilterPlatform<TT> : AFilterCommon<TT> where TT : Object
    {
        public bool FilterTest()
        {
            var desiredPlatform = AssetSorceryPlatform.AssetSorceryGetPlatformCalculated();
            return platform == desiredPlatform || platform == AssetSorceryPlatformRuntime.ePlatformAS.Auto; // Auto acts as Any match if on the item
        }

        public TT GetItem()
        {
            return item;
        }

        public void Validate()
        {
            //
        }

        public TT item;

        public AssetSorceryPlatformRuntime.ePlatformAS platform = AssetSorceryPlatformRuntime.ePlatformAS.Auto;
    }

    /*
    
    [Serializable]
    public class FilterCommonOld<TT> where TT : Object
    {
#if ENABLE_UNITYVERSION
                var curVersion = AssetSorceryCommonItems.GetUnityVersion();
#endif

#if ENABLE_UNITYPIPELINE
            var currentPipe = RenderPipelineUtils.DetectPipeline();
            var target = GetTargetForSpecificRP(currentPipe);
#endif
        
        //
        
#if ENABLE_UNITYPIPELINE
            [Tooltip(@"Any - match any render pipeline
MatchOnly - used when pointing to a Shader we want to match against to swap from ( eg Standard shader ) but we do not want to copy the source from and swap to it
Standard - Built-In / DIRP / DRP / Default Render Pipeline
URP - Universal Render Pipeline
HDRP - High Definition Render Pipeline")]
            public AssetSorceryCommonItems.SRPTarget srpTarget = AssetSorceryCommonItems.SRPTarget.StandardSource;
#endif

#if ENABLE_UNITYVERSION
            public AssetSorceryCommonItems.UnityVersion UnityVersionMin = AssetSorceryCommonItems.UnityVersion.Min;
            public AssetSorceryCommonItems.UnityVersion UnityVersionMax = AssetSorceryCommonItems.UnityVersion.Max;
#endif
        
        public bool IsValid()
        {
#if ENABLE_UNITYPIPELINE
                if (entryCommon.srpTarget != AssetSorceryCommonItems.SRPTarget.Any && (entryCommon.srpTarget == AssetSorceryCommonItems.SRPTarget.Standard || target != entryCommon.srpTarget)) continue;
#endif
#if ENABLE_UNITYVERSION
                    if (e.UnityVersionMax == AssetSorceryCommonItems.UnityVersion.Min && e.UnityVersionMin == AssetSorceryCommonItems.UnityVersion.Min) e.UnityVersionMax = AssetSorceryCommonItems.UnityVersion.Max;
                    if (curVersion < e.UnityVersionMin || curVersion > e.UnityVersionMax) continue;
#endif
            return true; // TODO
        }
        
        public TT item;
    }
    
    */

    // TODO wont help due to serialization in editor inspector?
    public class AssetSorceryAssetCommonBase<T> : AssetSorceryAssetCommon<AFilterCommon<T>, T> where T : Object
    {
        public List<FilterPlatform<T>> filters = new List<FilterPlatform<T>>();

        public override AFilterCommon<T> ReturnFilterMatch()
        {
            AFilterCommon<T> match = null;

            foreach (var o in filters)
            {
                if (!o.FilterTest()) continue;
                match = o;
                break;
            }

            return match;
        }

        public override void DrawCustomInspector(SerializedObject data)
        {
            //
        }

        public override List<AFilterCommon<T>> GetFilterEntries()
        {
            return new List<AFilterCommon<T>>(filters);
        }
    }

    [Serializable]
    public abstract class AssetSorceryAssetCommon<TFilterCommon, TT> : ScriptableObject where TT : Object where TFilterCommon : AFilterCommon<TT>
    {
        public bool realtimeUpdate;

        // TODO old, can remove now
        [HideInInspector] internal List<FilterPlatformOld<TT>> entries = new List<FilterPlatformOld<TT>>();

        [ContextMenu("ForceSave")]
        public void ForceSave()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        private void OnValidate()
        {
            Validate();
        }

        /// <summary>
        /// When editing the assets content you can write the data back into the file with this
        /// </summary>
        public void WriteAssetContentBack(string directoryPath, bool assetDatabaseRefresh = true)
        {
            WriteJSON(EditorJsonUtility.ToJson(this, true), directoryPath);
            ForceSave();
            if (assetDatabaseRefresh) AssetDatabase.Refresh();
        }

        public static void WriteJSON(string data, string directoryPath)
        {
            //Debug.Log("WriteJSON: " + directoryPath);
            File.WriteAllText(directoryPath, data);
        }

        //

        public abstract void DrawCustomInspector(SerializedObject data);

        public abstract List<AFilterCommon<TT>> GetFilterEntries();

        private void Validate()
        {
            var entries = GetFilterEntries();
            foreach (var aFilterCommon in entries)
            {
                aFilterCommon.Validate();

                var item = aFilterCommon.GetItem();

                var path = AssetDatabase.GetAssetPath(item);

                if (path.Contains("unity_builtin_extra") || path.Contains("Library/unity default resources"))
                {
                    Debug.LogWarning("You cannot use built in unity resources/files");
                }
            }

            /* // TODO fix
            foreach (var e in entries)
            {
                var path = AssetDatabase.GetAssetPath(e.GetItem());

#if ENABLE_UNITYPIPELINE
                if (e.srpTarget != AssetSorceryCommonItems.SRPTarget.Standard)
                {
#endif
                if (path.Contains("unity_builtin_extra") || path.Contains("Library/unity default resources"))
                {
                    Debug.LogWarning("You cannot use built in shaders directly please download and point to 'Built in shaders' from the Unity download archive: https://unity3d.com/get-unity/download/archive, setting target to 'MatchOnly'");
#if ENABLE_UNITYPIPELINE
                        e.srpTarget = AssetSorceryCommonItems.SRPTarget.Standard;
#endif
                }
#if ENABLE_UNITYPIPELINE
                }
#endif
            }*/
        }


        public TFilterCommon GetEntryForCurrentRP(bool silent)
        {
            var entryCommonResult = default(TFilterCommon);

            Validate();

            return ReturnFilterMatch();
/*
            foreach (TFilterCommon entryCommon in entries)
            {
                if (!entryCommon.IsValid()) continue;
                if (entryCommonResult != null && !silent) Debug.LogWarning("Found multiple possible entries");
                entryCommonResult = entryCommon;
            }

            if (entryCommonResult == null) Debug.LogError("No match found!");

            return entryCommonResult;*/
        }


        /// <summary>
        /// (not implemented) Get path to the resulting imported asset created by the Scripted Importer
        /// </summary>
        public string GetAssetPath()
        {
            // TODO not implemented
            return "";
        }


        public abstract TFilterCommon ReturnFilterMatch();


        public TT GetInputAsset(bool silent)
        {
            var entry = GetEntryForCurrentRP(silent);
            return entry.GetItem();
        }


        public static string ConvertShaderGraphWithGuid(string path)
        {
            //var path = AssetDatabase.GUIDToAssetPath(guid);
            var assetImporter = AssetImporter.GetAtPath(path);

            var shader = AssetDatabase.LoadAssetAtPath<Shader>(path);

            if (assetImporter.GetType().FullName != "UnityEditor.ShaderGraph.ShaderGraphImporter")
            {
                Debug.LogError("Not a shader graph importer");
            }

            // var graphData = GetGraphData(importer);
            // var generator = new Generator(graphData, null, GenerationMode.ForReals, assetName, null, true);
            // See Library\PackageCache\com.unity.shadergraph@12.1.9\Editor\Importers\ShaderGraphImporterEditor.cs


            var shaderGraphImporterAssembly = AppDomain.CurrentDomain.GetAssemblies().First(assembly => { return assembly.GetType("UnityEditor.ShaderGraph.ShaderGraphImporterEditor") != null; });
            var shaderGraphImporterEditorType = shaderGraphImporterAssembly.GetType("UnityEditor.ShaderGraph.ShaderGraphImporterEditor");

            var getGraphDataMethod = shaderGraphImporterEditorType
                .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                .First(info => { return info.Name.Contains("GetGraphData"); });

            var graphData = getGraphDataMethod.Invoke(null, new object[] {assetImporter});
            var generatorType = shaderGraphImporterAssembly.GetType("UnityEditor.ShaderGraph.Generator");
            var generatorConstructor = generatorType.GetConstructors().First();

            //var shaderGraphName = Path.GetFileNameWithoutExtension(path);
            //var assetName = $"Converted/{shaderGraphName}";
            var assetName = shader.name;

            var generator = generatorConstructor.Invoke(new object[] {graphData, null, 1, assetName, null, true});

            var generatedShaderMethod = generator.GetType().GetMethod("get_generatedShader",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var generatedShader = generatedShaderMethod.Invoke(generator, new object[] { });

            return (string) generatedShader;
        }

        public string GetSource()
        {
            string source = "";

            var entry = GetEntryForCurrentRP(false);

            if (entry != null)
            {
                var path = AssetDatabase.GetAssetPath(entry.GetItem());
                if (string.IsNullOrEmpty(path))
                {
                    // Will be empty on first creation, override ProcessSourceToObject to handle empty source and create a default asset
                    //Debug.LogWarning("path empty");
                }
                else
                {
                    if (path.Contains("unity_builtin_extra") || path.Contains("Library/unity default resources"))
                    {
                        Debug.LogError("You cannot use unity internal resources directly");
                    }
                    else
                    {
                        // TODO
                        //if (path.EndsWith(AssetSorceryShader.ASSETSORCERY_FILE_EXTENSION))
                        //{
                        //    return AssetSorceryShader.ReadFromPath(path).GetSource();
                        //}

                        if (path.EndsWith(".shadergraph"))
                        {
                            return ConvertShaderGraphWithGuid(path);
                        }

                        source = System.IO.File.ReadAllText(path);
                    }
                }
            }

            //if(source==null|source=="")source = 

            return source;
        }

#if ENABLE_UNITYPIPELINE
        public AssetSorceryCommonItems.SRPTarget GetTargetForSpecificRP(RenderPipelineUtils.PipelineType pipe)
        {
            AssetSorceryCommonItems.SRPTarget target = AssetSorceryCommonItems.SRPTarget.Any;
            switch (pipe)
            {
                case RenderPipelineUtils.PipelineType.Unsupported:
                    break;
                case RenderPipelineUtils.PipelineType.BuiltInPipeline:
                    target = AssetSorceryCommonItems.SRPTarget.StandardSource;
                    break;
                case RenderPipelineUtils.PipelineType.UniversalPipeline:
                    target = AssetSorceryCommonItems.SRPTarget.URP;
                    break;
                case RenderPipelineUtils.PipelineType.HDPipeline:
                    target = AssetSorceryCommonItems.SRPTarget.HDRP;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pipe), pipe, null);
            }

            return target;
        }

        public EntryCommon GetEntryForSpecificRP(RenderPipelineUtils.PipelineType pipe)
        {
            var target = GetTargetForSpecificRP(pipe);
            return GetEntryForSpecificTarget(target);
        }

        public EntryCommon GetEntryForSpecificTarget(AssetSorceryCommonItems.SRPTarget target)
        {
            EntryCommon s = null;
            Validate();
            int i = 0;
            foreach (var entryCommon in entries)
            {
#if ENABLE_UNITYPIPELINE
                if (entryCommon.srpTarget != target) continue;
#endif
                if (s != null) Debug.LogWarning("Found multiple possible entries: " + i, entryCommon.item);
                s = entryCommon;
                i++;
            }

            return s;
        }
#endif
/*
        public EntryCommon GetEntryForSpecificShader(Shader currentShader)
        {
            EntryCommon s = null;
            Validate();

            for (int i = 0; i < entries.Count; i++)
            {
                var entryCommon = entries[i];
                if (entryCommon.item == currentShader)
                {
#if ENABLE_UNITYPIPELINE
                    Debug.Log("Found entry: " + i + ":" + entryCommon.srpTarget, entryCommon.item);
#endif
                    if (s != null)
                    {
                        Debug.LogWarning("Found multiple possible entries");
                    }

                    s = entryCommon;
                }
            }

            return s;
        }*/
    }
}
