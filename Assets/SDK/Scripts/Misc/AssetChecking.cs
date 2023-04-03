using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.AddressableAssets;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.AddressableAssets.Settings;
#endif

namespace ThunderRoad
{
    public interface ICheckAsset
    {
#if UNITY_EDITOR
        List<Issue> GetIssues(bool includeLongCheck, bool experimental);
#endif
    }

#if UNITY_EDITOR

    [Serializable]
    public class AssetObjectReference
    {
        public string guid;

        public AssetObjectReference(UnityEngine.Object obj)
        {
            this.guid = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(obj)).ToString();
        }

        public AssetObjectReference(string guid)
        {
            this.guid = guid;
        }

        public AssetObjectReference(Scene scene)
        {
            this.guid = AssetDatabase.GUIDFromAssetPath(scene.path).ToString();
        }

        public AssetObjectReference() { }

        public bool IsValid()
        {
            if (!string.IsNullOrEmpty(guid) && !string.IsNullOrEmpty(AssetDatabase.GUIDToAssetPath(guid)))
            {
                return true;
            }
            return false;
        }

        public T GetObject<T>() where T : UnityEngine.Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(GetPath());
        }

        public Type GetObjectType()
        {
            return AssetDatabase.GetMainAssetTypeAtPath(GetPath());
        }

        public string GetPath()
        {
            return AssetDatabase.GUIDToAssetPath(guid);
        }

        public AddressableAssetEntry GetEntry()
        {
            return AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(guid);
        }

        public string GetName()
        {
            if (!IsValid()) return "None";
            return Path.GetFileNameWithoutExtension(GetPath());
        }

        public void Open(string childPath = null)
        {
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (GetObjectType() == typeof(SceneAsset))
            {
                if (SceneManager.GetActiveScene().path != GetPath())
                {
                    if (prefabStage) EditorSceneManager.CloseScene(prefabStage.scene, true);
                    AssetDatabase.OpenAsset(GetObject<UnityEngine.Object>());
                }
            }
            else if (prefabStage)
            {
                if (prefabStage.assetPath != GetPath())
                {
                    AssetDatabase.OpenAsset(GetObject<UnityEngine.Object>());
                }
            }
            else
            {
                AssetDatabase.OpenAsset(GetObject<UnityEngine.Object>());
            }
            if (!string.IsNullOrEmpty(childPath))
            {
                GameObject childGameObject = GameObject.Find(childPath);
                Selection.activeObject = childGameObject;
                EditorGUIUtility.PingObject(childGameObject);
            }
        }
    }

    [Serializable]
    public class AssetData<T> where T : UnityEngine.Object
    {
        public Reference defaultReference;
        public Reference androidReference;

        public bool androidExport;
        public State stateExportToAndroid = State.None;

        public enum State
        {
            None,
            Running,
            Failure,
            Completed,
        }

        [Serializable]
        public class Reference : AssetObjectReference
        {
            public bool foldoutIssues;
            public List<Issue> issues = new List<Issue>();

            public bool bakeLightmaps;
            public bool bakeCulling;

            public State bakeLightmapsState = State.None;
            public State bakeCullingState = State.None;

            public int bakeFailCount;

            public Reference(UnityEngine.Object obj) : base(obj)
            {
                this.guid = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(obj)).ToString();
            }

            public Reference(string guid) : base(guid)
            {
                this.guid = guid;
            }

            public Reference() : base() { }

            public void AddIssue(Issue newIssue)
            {
                foreach (Issue issue in issues)
                {
                    if (issue.reference.guid == newIssue.reference.guid && issue.message == newIssue.message)
                    {
                        if (newIssue.major) issue.major = true;
                        return;
                    }
                }
                issues.Add(newIssue);
            }

            public void Check(bool includeLongCheck, bool experimental)
            {
                issues.Clear();
                UnityEngine.Object assetObject = GetObject<UnityEngine.Object>();
                if (assetObject == null) return;
                if (assetObject is ICheckAsset)
                {
                    issues.AddRange((assetObject as ICheckAsset).GetIssues(includeLongCheck, experimental));
                }
                if (assetObject is GameObject)
                {
                    foreach (Component component in (assetObject as GameObject).GetComponentsInChildren<Component>(true))
                    {
                        if (component is ICheckAsset)
                        {
                            issues.AddRange((component as ICheckAsset).GetIssues(includeLongCheck, experimental));
                        }
                    }
                }
                else if (assetObject is SceneAsset)
                {
                    SceneSetup[] orgSceneSetup = EditorSceneManager.GetSceneManagerSetup();

                    if (orgSceneSetup != null && orgSceneSetup.Length > 1 || orgSceneSetup[0].path != GetPath())
                    {
                        EditorSceneManager.OpenScene(GetPath(), OpenSceneMode.Single);
                    }
                    else
                    {
                        orgSceneSetup = null;
                    }

                    EditorSceneManager.OpenScene(GetPath(), OpenSceneMode.Single);
                    foreach (Component component in GameObject.FindObjectsOfType<Component>())
                    {
                        if (component is ICheckAsset)
                        {
                            issues.AddRange((component as ICheckAsset).GetIssues(includeLongCheck, experimental));
                        }
                    }

                    if (orgSceneSetup != null) EditorSceneManager.RestoreSceneManagerSetup(orgSceneSetup);
                }
            }
        }

        public AssetData(string defaultGuid, string androidGuid = null)
        {
            defaultReference = new Reference(defaultGuid);
            if (!string.IsNullOrEmpty(androidGuid))
            {
                androidReference = new Reference(androidGuid);
            }
        }

        public AssetData(AddressableAssetEntry defaultEntry, AddressableAssetEntry androidEntry = null)
        {
            defaultReference = new Reference(defaultEntry.guid);
            if (androidEntry != null && !string.IsNullOrEmpty(androidEntry.guid))
            {
                androidReference = new Reference(androidEntry.guid);
            }
            else
            {
                androidReference = new Reference();
            }
        }

        public Reference GetReference(bool android)
        {
            return android ? androidReference : defaultReference;
        }
    }

    public delegate bool FixCallBack(UnityEngine.Object source, params object[] fixData);

    [Serializable]
    public class Issue
    {
        public AssetObjectReference reference;
        public string childPath;
        public string message;
        public bool major;

        public FixCallBack fixCallback;
        public object[] fixData;

        public Issue(UnityEngine.Object source, string message, bool major, FixCallBack fixCallBack = null, params object[] fixData)
        {
            if (source is GameObject)
            {
                var rootPrefab = PrefabUtility.GetCorrespondingObjectFromSource(source);
                if (rootPrefab)
                {
                    reference = new AssetObjectReference(rootPrefab);
                    childPath = (source as Component).gameObject.GetPathFromNearestInstanceRoot();
                }
                else
                {
                    reference = new AssetObjectReference((source as GameObject).scene);
                    childPath = (source as GameObject).GetPathFromRoot();
                }
            }
            if (source is Component)
            {
                var rootPrefab = PrefabUtility.GetCorrespondingObjectFromSource((source as Component).gameObject);
                if (rootPrefab)
                {
                    reference = new AssetObjectReference(rootPrefab);
                    childPath = (source as Component).gameObject.GetPathFromNearestInstanceRoot();
                }
                else
                {
                    reference = new AssetObjectReference((source as Component).gameObject.scene);
                    childPath = (source as Component).gameObject.GetPathFromRoot();
                }
            }
            else
            {
                reference = new AssetObjectReference(source);
                childPath = null;
            }
            this.message = message;
            this.major = major;
            this.fixCallback = fixCallBack;
            this.fixData = fixData;        
        }

        public string GetName()
        {
            return $"{reference.GetName()} : {childPath}";
        }

        public bool TryFix()
        {
            if (fixCallback != null)
            {
                var obj = AssetDatabase.LoadAssetAtPath(reference.GetPath(), reference.GetObjectType());
                return fixCallback.Invoke(obj, fixData);
            }
            else
            {
                return true;
            }
        }
    }
#endif
}
