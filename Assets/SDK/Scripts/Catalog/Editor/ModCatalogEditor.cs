using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace ThunderRoad
{
    public class CatalogEditor : EditorWindow, ISerializationCallbackReceiver
    {
        public static Dictionary<string, CatalogData> openFiles = new();
        // These 2 fields are only for serialization
        private List<string> openFilesKeys;
        private List<CatalogData> openFilesValues;

        private MethodInfo guiOutlineMethod;
        [SerializeField]
        private TreeViewState treeState;
        private static CatalogTreeView treeView;
        [SerializeField]
        private float treeWidth;
        private MethodInfo resizerMethod;

        [SerializeField]
        private Vector2 scrollPos;

        private SerializedObject obj;
        private string currentPath;
        [SerializeField, SerializeReference]
        private CatalogData currentData;
        private SerializedProperty currentDataProp;
        private readonly Dictionary<Type, List<SerializedProperty>> props = new();

        [MenuItem("ThunderRoad (SDK)/Mod Catalog Editor")]
        private static void Open()
        {
            GetWindow<CatalogEditor>();
        }

        private void OnEnable()
        {
            // Look for all jsons
            List<string> paths = new();
            foreach (string file in Directory.GetFiles(Application.dataPath, "*.json", SearchOption.AllDirectories))
                if (!openFiles.ContainsKey(file))
                    paths.Add(Path.GetRelativePath(Application.dataPath, file));
            AddPaths(paths, false);

            // No clue why this is internal
            guiOutlineMethod = typeof(EditorGUI).GetMethod("DrawOutline", BindingFlags.NonPublic | BindingFlags.Static);

            treeState ??= new TreeViewState();
            treeView = new CatalogTreeView(openFiles, treeState);

            // I have no clue why this method is internal but it works so /shrug
            resizerMethod = typeof(EditorGUI).GetMethod("WidthResizer", BindingFlags.NonPublic | BindingFlags.Static, null, new Type[4] { typeof(Rect), typeof(float), typeof(float), typeof(float) }, null);
            treeWidth = EditorPrefs.GetFloat("ModCatalogEditor.treeWidth");

            if (obj == null)
            {
                obj = new SerializedObject(this);
                currentDataProp = obj.FindProperty("currentData");
            }
        }

        private static void AddPaths(List<string> jsonPaths, bool reload = true)
        {
            if (jsonPaths.Count == 0)
                return;

            List<string> jsons = new();
            foreach (string jsonPath in jsonPaths)
            {
                string absolutePath = Path.Combine(Application.dataPath, jsonPath);
                if (File.Exists(absolutePath))
                    jsons.Add(File.ReadAllText(absolutePath));
            }

            // Make sure serializer is able to deserialize properly
            Catalog.GetJsonNetSerializer();

            object[] deserializedObjects = Catalog.DeserializeJsons(jsons, jsonPaths);

            for (int i = 0; i < jsons.Count; i++)
            {
                if (deserializedObjects[i] is CatalogData data)
                {
                    openFiles[jsonPaths[i]] = data;
                    Catalog.LoadJson(deserializedObjects[i], jsons[i], jsonPaths[i], Path.GetFileName(Path.GetDirectoryName(jsonPaths[i])));
                }
            }

            if (reload)
                treeView.Reload();
        }

        private void OnGUI()
        {
            IList<int> selected = treeView.GetSelection();
            currentPath = null;
            currentData = null;
            CatalogTreeView.CatalogTreeViewItem currentItem = null;
            if (selected.Count != 0)
            {
                currentItem = treeView.items[selected[0]] as CatalogTreeView.CatalogTreeViewItem;
                if (currentItem != null)
                {
                    currentPath = currentItem.data.Key;
                    currentData = currentItem.data.Value;
                }
            }

            // Build toolbar
            Rect toolbarRect = new(0, 0, position.width, GUI.skin.button.CalcHeight(new GUIContent(""), position.width));
            using (new GUILayout.AreaScope(toolbarRect))
            {
                using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(true)))
                {
                    using (new EditorGUI.DisabledGroupScope(currentItem == null))
                    {
                        if (GUILayout.Button(new GUIContent("Save", "Saves current file.")))
                        {
                            string jsonString = JsonConvert.SerializeObject(currentData, Catalog.GetJsonNetSerializerSettings());
                            File.WriteAllText(Path.Combine(Application.dataPath, currentPath), jsonString);
                            AssetDatabase.ImportAsset(Path.Combine("Assets", currentPath));
                            currentItem.unsaved = false;
                            hasUnsavedChanges = treeView.GetRows().Any(row => row is CatalogTreeView.CatalogTreeViewItem catalogItem && catalogItem.unsaved);

                        }
                    }

                    if (GUILayout.Button(new GUIContent("Save All", "Saves all unsaved files.")))
                        SaveChanges();

                    if (GUILayout.Button(new GUIContent("Reload Catalog", "Clears all catalog data then reloads everything."))
                        && (!hasUnsavedChanges || EditorUtility.DisplayDialog("Warning!", "You have unsaved data in this JSON file.", "Proceed", "Cancel")))
                    {
                        Catalog.EditorLoadAllJson();
                        List<string> pathes = openFiles.Keys.ToList();
                        openFiles.Clear();
                        AddPaths(pathes);
                        hasUnsavedChanges = false;
                    }

                    GUIContent dropDownContent = new("New", "Create new catalog JSON.");
                    Rect dropDownRect = GUILayoutUtility.GetRect(dropDownContent, "MiniPullDown");
                    if (EditorGUI.DropdownButton(dropDownRect, dropDownContent, FocusType.Keyboard))
                    {
                        static void newAction(Type type)
                        {
                            string path = EditorUtility.SaveFilePanelInProject("New CatalogData", $"{Catalog.GetCategory(type)}_new", "json", "Enter file name for new CatalogData");
                            if (string.IsNullOrEmpty(path))
                                return;

                            CatalogData data = (CatalogData)Activator.CreateInstance(type);
                            // CatalogData init doesn't like null strings as the id
                            data.id = "";
                            data.version = data.GetCurrentVersion();
                            string jsonString = JsonConvert.SerializeObject(data, Catalog.GetJsonNetSerializerSettings());
                            File.WriteAllText(Path.Combine(Application.dataPath, "../", path), jsonString);
                            AssetDatabase.ImportAsset(path);
                        }
                        GenericMenu menu = new();
                        foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm => asm.GetTypes()))
                        {
                            if (type.IsSubclassOf(typeof(CatalogData)))
                            {
                                // This temp var is needed so that the callbak has the right type passed in
                                Type temp = type;
                                menu.AddItem(new GUIContent(type.Name), false, () => newAction(temp));
                            }
                        }
                        menu.DropDown(dropDownRect);
                    }
                }
            }

            Rect mainRect = new(0, toolbarRect.height, position.width, position.height - toolbarRect.height);
            guiOutlineMethod.Invoke(null, new object[3] { mainRect, 2, new Color(0.12f, 0.12f, 0.12f) });
            mainRect = new Rect(mainRect.x + 2, mainRect.y + 2, mainRect.width - 4, mainRect.height - 4);

            // Draw tree and resize bar
            Rect resizeRect = new(treeWidth - 2.5f, toolbarRect.height, 5, position.height - toolbarRect.height);
            treeWidth = (float)resizerMethod.Invoke(null, new object[4] { resizeRect, treeWidth, 16, position.width });
            
            treeView.OnGUI(new Rect(mainRect) { width = treeWidth });
            EditorPrefs.SetFloat("ModCatalogEditor.treeWidth", treeWidth);

            if (currentData == null)
                return;

            // Draw actual editor part
            float originalLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 200;
            
            using (new GUILayout.AreaScope(new Rect(mainRect) { x = mainRect.x + treeWidth, width = mainRect.width - treeWidth }))
            {
                using (EditorGUILayout.ScrollViewScope scrollView = new(scrollPos))
                {
                    scrollPos = scrollView.scrollPosition;

                    // Prevent loading mismatched data
                    if (currentData.version != currentData.GetCurrentVersion()
                        && !EditorUtility.DisplayDialog("Warning!", "Attempting to load a JSON with an incompatible version. It is recommended to back up before proceeding or data may be lost.", "Proceed", "Cancel"))
                    {
                        treeView.SetSelection(new List<int>());
                        return;
                    }

                    obj.Update();

                    Type currentDataType = currentData.GetType();
                    if (!props.ContainsKey(currentDataType))
                    {
                        List<SerializedProperty> dataProps = new();
                        props[currentDataType] = dataProps;
                        foreach (FieldInfo field in currentDataType.GetFields())
                        {
                            switch (field.Name)
                            {
                                case "hashId":
                                case "version":
                                    continue;
                            }

                            SerializedProperty prop = currentDataProp.FindPropertyRelative(field.Name);
                            if (prop != null)
                            {
                                dataProps.Add(prop);
                                EditorGUILayout.PropertyField(prop);
                            }
                        }
                    }
                    else
                    {
                        foreach (SerializedProperty prop in props[currentDataType])
                            EditorGUILayout.PropertyField(prop);
                    }

                    if (obj.ApplyModifiedProperties() && !currentItem.unsaved)
                    {
                        currentItem.unsaved = true;
                        hasUnsavedChanges = true;
                    }
                }
            }

            EditorGUIUtility.labelWidth = originalLabelWidth;
        }

        public override void SaveChanges()
        {
            AssetDatabase.StartAssetEditing();
            try
            {
                foreach (TreeViewItem item in treeView.GetRows())
                {
                    if (item is not CatalogTreeView.CatalogTreeViewItem catalogItem || !catalogItem.unsaved)
                        continue;

                    string jsonString = JsonConvert.SerializeObject(catalogItem.data.Value, Catalog.GetJsonNetSerializerSettings());
                    File.WriteAllText(Path.Combine(Application.dataPath, catalogItem.data.Key), jsonString);
                    AssetDatabase.ImportAsset(Path.Combine("Assets", catalogItem.data.Key));
                    catalogItem.unsaved = false;
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }
            hasUnsavedChanges = false;
        }

        public void OnBeforeSerialize()
        {
            openFilesKeys = new List<string>(openFiles.Count);
            openFilesValues = new List<CatalogData>(openFiles.Count);
            foreach (KeyValuePair<string, CatalogData> entry in openFiles)
            {
                openFilesKeys.Add(entry.Key);
                openFilesValues.Add(entry.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            openFiles = new Dictionary<string, CatalogData>();
            for (int i = 0; i < openFilesKeys.Count; i++)
                openFiles[openFilesKeys[i]] = openFilesValues[i];
        }

        private class JsonImportNotifier : AssetPostprocessor
        {   
            private static void OnPostprocessAllAssets(string[] importedAssets, string[] _, string[] __, string[] ___)
            {
                List<string> res = new();
                foreach (string path in importedAssets)
                    if (Path.GetExtension(path) == ".json")
                        res.Add(Path.GetRelativePath(Application.dataPath, path));
                AddPaths(res);
            }
        }
    }
}