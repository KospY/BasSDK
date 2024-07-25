using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
#endif
namespace ThunderRoad.AssetSorcery
{
    //[CustomEditor(typeof (ScriptedImporter), true)]
    public class AssetSorceryEditorCommon<T, TT, EntryCommon> : ScriptedImporterEditor where T : AssetSorceryAssetCommon<EntryCommon,TT> where TT : Object where EntryCommon : AFilterCommon<TT>
    {
        private SerializedProperty shaderSettingsProps;
        private SerializedProperty entryProperties;
        //private SerializedProperty textureProps;
        private int lastHash;

        protected override System.Type extraDataType => typeof(T);
        
#if ODIN_INSPECTOR
        private PropertyTree myObjectTree;
#endif
        protected override void InitializeExtraDataInstance(Object extraTarget, int targetIndex)
        {
            var stack = (T) extraTarget;
            var path = ((AssetImporter) targets[targetIndex]).assetPath;
            if (File.Exists(path))
            {
                string fileContent = File.ReadAllText(path);
                EditorJsonUtility.FromJsonOverwrite(fileContent, stack);
                //stack.SaveToPath(path);
            }
        }

        protected override void Apply()
        {
            base.Apply();
            
            for (int i = 0; i < targets.Length; i++)
            {
                string path = ((AssetImporter) targets[i]).assetPath;
				//Debug.Log("AssetSorceryEditorCommon: " + path);
                File.WriteAllText(path, EditorJsonUtility.ToJson((T) extraDataTargets[i], true));
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();

            if (extraDataSerializedObject == null) return;
            entryProperties = extraDataSerializedObject.FindProperty("entries");
            shaderSettingsProps = extraDataSerializedObject.FindProperty("shaderSettings");
            //textureProps = extraDataSerializedObject.FindProperty("textureSettings");
        }

        public T GetAsset()
        {
            return extraDataSerializedObject.targetObject as T;
        }

        public override void OnInspectorGUI()
        {
            var asset = GetAsset();
            var entry = asset.GetEntryForCurrentRP(true);
            //var asset = sisasset.GetAsset();
            var style = new GUIStyle();
            style.normal.textColor = Color.white;
            if (entry != null && entry.GetItem() != null)
            {
#if ENABLE_UNITYPIPELINE
                EditorGUILayout.TextField("Matched: " + entry.srpTarget + " : " + entry.item.name, style);
#else
                EditorGUILayout.TextField("Matched: " + entry.GetItem().name, style);
#endif
                GUILayout.Space(10);
            }
            else
            {
                style.normal.textColor = Color.red;
#if ENABLE_UNITYPIPELINE
                var txt = "No Matches for: " + RenderPipelineUtils.DetectPipeline();
#else
                var txt = "No Matches";
#endif

#if ENABLE_UNITYVERSION
                txt += " : " + AssetSorceryCommonItems.GetUnityVersion();
#endif
                EditorGUILayout.TextField(txt, style);
            }

            extraDataSerializedObject.Update();

            //asset.realtimeUpdate = EditorGUILayout.Toggle("Realtime Update", asset.realtimeUpdate);

            EditorGUI.BeginChangeCheck();

            //if (entryProperties != null) EditorGUILayout.PropertyField(entryProperties);
            //if (shaderSettingsProps != null) EditorGUILayout.PropertyField(shaderSettingsProps);
            
#if ODIN_INSPECTOR
            // Use ODIN to draw inspector
            if (myObjectTree == null) myObjectTree = PropertyTree.Create(extraDataSerializedObject);
            myObjectTree.Draw(false);
#endif            

            asset.DrawCustomInspector(extraDataSerializedObject);
            //if (textureProps != null)
            //{
            //    if (asset is AssetSorceryTextureAsset texture)
            //    {
            //        EditorGUILayout.PropertyField(textureProps);
            //        texture.DrawCustomInspector();
            //    }
            //}


            extraDataSerializedObject.ApplyModifiedProperties();
            ApplyRevertGUI();
            ShouldHideOpenButton();

            if (EditorGUI.EndChangeCheck() && asset.realtimeUpdate && GUI.changed) // GUI.changed
            {
                //Debug.Log(asset.GetHashCode());

                int curHash = 0;

                for (int i = 0; i < targets.Length; i++)
                {
                    //string path = ((AssetImporter) targets[i]).assetPath;
                    // Debug.Log("targets: " + i + ":" + targets[i].GetHashCode()+":"+EditorJsonUtility.ToJson((T) extraDataTargets[i]).GetHashCode());
                    curHash += EditorJsonUtility.ToJson((T) extraDataTargets[i]).GetHashCode();
                }

                if (curHash != lastHash)
                {
                    lastHash = curHash;
                    Apply();
                    ApplyAndImport();
                }
            }
        }
    }
}
