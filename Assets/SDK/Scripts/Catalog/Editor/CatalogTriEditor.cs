using System;
using System.Collections.Generic;
using System.Linq;
using TriInspector;
using TriInspector.Utilities;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThunderRoad
{
    internal class CatalogTriEditor : EditorWindow
    {
        private MenuTree _menuTree;
        private SearchField _searchField;

        private ScriptableObject _current;
        private SerializedObject _currentSerializedObject;
        private TriPropertyTree _currentPropertyTree;

        private Vector2 _currentScroll;

        [MenuItem("ThunderRoad (SDK)/Tri Catalog Editor")]
        public static void Open()
        {
            var window = GetWindow<CatalogTriEditor>();
            window.titleContent = new GUIContent("Tri Catalog Editor");
            window.Show();
        }

        private void OnEnable()
        {
            _menuTree = new MenuTree(new TreeViewState());
            _menuTree.SelectedTypeChanged += ChangeCurrentObject;

            _searchField = new SearchField();
            _searchField.downOrUpArrowKeyPressed += _menuTree.SetFocusAndEnsureSelectedItem;

            _menuTree.Reload();
        }

        private void OnDisable()
        {
            ChangeCurrentObject(null);
        }

        private void OnGUI()
        {
            //Layout of the menu should be a toolbar along the top, then a vertical split with the menu on the left and the element on the right
            // -- search field -- | -- clear button -- | -- load json button --
            // -- menu tree -- | -- element --
            using (new GUILayout.VerticalScope())
            {
                using (new GUILayout.HorizontalScope())
                {
                    DrawToolBar();
                }
                using (new GUILayout.HorizontalScope())
                {
                    using (new GUILayout.VerticalScope(GUILayout.Width(200)))
                    {
                        DrawMenu();
                    }

                    var separatorRect = GUILayoutUtility.GetLastRect();
                    separatorRect.xMin = separatorRect.xMax;
                    separatorRect.xMax += 1;
                    GUI.Box(separatorRect, "");

                    using (new GUILayout.VerticalScope())
                    {
                        DrawElement();
                    }
                }
            }
        }

        private void DrawToolBar()
        {
            //show a button to clear the search
            if (GUILayout.Button("Clear", EditorStyles.toolbarButton))
            {
                _menuTree.searchString = string.Empty;
            }
            //a button called new to create a new instance of a a catalog Data
            if (GUILayout.Button("New", EditorStyles.toolbarButton))
            { }

            //save json button
            if (GUILayout.Button("Save", EditorStyles.toolbarButton))
            { }

            //refresh button
            if (GUILayout.Button("Refresh", EditorStyles.toolbarButton))
            {
                if (Catalog.gameData == null) return;
                //object orgSelection = selected != null ? selected.Value : null;
                Catalog.Refresh();
                //base.ForceMenuTreeRebuild();
                // if (orgSelection != null)
                // {
                //     base.TrySelectMenuItemWithObject(orgSelection);
                // }
            }
            //load json button
            if (GUILayout.Button("Load Json", EditorStyles.toolbarButton))
            {
                Catalog.OnRefreshProgress += OnCatalogOnOnRefreshProgress;
                Catalog.EditorLoadAllJson(true, true, false);
                //base.ForceMenuTreeRebuild();
                Catalog.OnRefreshProgress -= OnCatalogOnOnRefreshProgress;
            }
            //load json with mods button
            if (GUILayout.Button("Load Json with Mods", EditorStyles.toolbarButton))
            {
                Catalog.OnRefreshProgress += OnCatalogOnOnRefreshProgress;
                Catalog.EditorLoadAllJson(true, true, true);
                //base.ForceMenuTreeRebuild();
                Catalog.OnRefreshProgress -= OnCatalogOnOnRefreshProgress;
            }
            //unload json button
            if (GUILayout.Button("Unload Json", EditorStyles.toolbarButton))
            {
                ModManager.loadedMods.Clear();
                ModManager.gameModsLoaded = false;
                Catalog.Clear();
                //base.ForceMenuTreeRebuild();
            }
        }
        private void DrawMenu()
        {
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar, GUILayout.ExpandWidth(true)))
            {
                GUILayout.Space(5);
                _menuTree.searchString = _searchField.OnToolbarGUI(_menuTree.searchString, GUILayout.ExpandWidth(true));
                GUILayout.Space(5);
            }

            var menuRect = GUILayoutUtility.GetRect(0, 100000, 0, 100000);
            _menuTree.OnGUI(menuRect);
        }

        private void DrawElement()
        {
            if (_currentPropertyTree == null)
            {
                return;
            }

            using (var scrollScope = new GUILayout.ScrollViewScope(_currentScroll))
            {
                _currentScroll = scrollScope.scrollPosition;

                using (new GUILayout.VerticalScope(SampleWindowStyles.Padding))
                {
                    GUILayout.Label(_current.name, SampleWindowStyles.HeaderDisplayNameLabel);

                    _currentSerializedObject.UpdateIfRequiredOrScript();
                    _currentPropertyTree.Update();
                    _currentPropertyTree.RunValidationIfRequired();

                    GUILayout.Space(10);
                    GUILayout.Label("Preview", EditorStyles.boldLabel);

                    using (PushEditorTarget(_current))
                    using (new GUILayout.VerticalScope(SampleWindowStyles.BoxWithPadding))
                    {
                        _currentPropertyTree.Draw();
                    }

                    if (_currentSerializedObject.ApplyModifiedProperties())
                    {
                        _currentPropertyTree.RequestValidation();
                    }

                    if (_currentPropertyTree.RepaintRequired)
                    {
                        Repaint();
                    }
                }
            }
        }

        internal static TriGuiHelper.EditorScope PushEditorTarget(Object obj)
        {
            return new TriGuiHelper.EditorScope(obj);
        }

        private void ChangeCurrentObject(ScriptableObject so)
        {
            if (_current != null)
            {
                DestroyImmediate(_current);
                _current = null;
            }

            if (_currentSerializedObject != null)
            {
                _currentSerializedObject.Dispose();
                _currentSerializedObject = null;
            }

            if (_currentPropertyTree != null)
            {
                _currentPropertyTree.Dispose();
                _currentPropertyTree = null;
            }

            _currentScroll = Vector2.zero;

            if (so != null)
            {
                _current = so;
                _current.name = GetTypeNiceName(so.GetType());
                _current.hideFlags = HideFlags.DontSave;

                _currentSerializedObject = new SerializedObject(_current);
                _currentPropertyTree = new TriPropertyTreeForSerializedObject(_currentSerializedObject);
            }
        }

        private static string GetTypeNiceName(Type type)
        {
            var name = type.Name;

            if (name.Contains('_'))
            {
                var index = name.IndexOf('_');
                name = name.Substring(index + 1);
            }

            if (name.EndsWith("Sample"))
            {
                name = name.Remove(name.Length - "Sample".Length);
            }

            return name;
        }

        private void OnCatalogOnOnRefreshProgress(int total, int current, string message)
        {
            EditorUtility.DisplayProgressBar($"Loading JSON", $"Loading {message} [{current}/{total}]", (float)current / total);
            if (total == current)
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private class MenuTree : TreeView
        {
            private readonly Dictionary<string, GroupItem> _groups = new Dictionary<string, GroupItem>();

            public event Action<ScriptableObject> SelectedTypeChanged;

            public MenuTree(TreeViewState state) : base(state)
            { }

            protected override bool CanMultiSelect(TreeViewItem item)
            {
                return false;
            }

            protected override void SelectionChanged(IList<int> selectedIds)
            {
                base.SelectionChanged(selectedIds);

                ScriptableObject obj = selectedIds.Count > 0 && FindItem(selectedIds[0], rootItem) is TreeItem sampleItem
                    ? sampleItem.so
                    : null;

                SelectedTypeChanged?.Invoke(obj);
            }

            protected override TreeViewItem BuildRoot()
            {
                var root = new TreeViewItem(-1, -1);

                // get all of the catalogDatas from the catalog and make a scriptable object for each
                if (Catalog.data != null)
                {
                    var id = 0;
                    //Get all categories order by name
                    var categories = ((ThunderRoad.Category[])Enum.GetValues(typeof(ThunderRoad.Category))).OrderBy(x => x.ToString());
                    //Loop through all categories
                    foreach (ThunderRoad.Category category in categories)
                    {
                        var group = category.ToString();

                        if (!_groups.TryGetValue(group, out var groupItem))
                        {
                            _groups[group] = groupItem = new GroupItem(++id, group);

                            root.AddChild(groupItem);
                        }

                        //Get the catalog data for this category
                        CatalogCategory catalogCategory = Catalog.data[(int)category];
                        if(catalogCategory == null) continue;
                        if (catalogCategory.catalogDatas.IsNullOrEmpty()) continue;
                        foreach (CatalogData catalogData in catalogCategory.catalogDatas.OrderBy(i => i.SortKey()))
                        {
                            catalogData.CatalogEditorRefresh();
                            //make a catalogDataHolder for it
                            ICatalogData catalogDataHolder;
                            switch (catalogData)
                            {
                                case ItemData itemData:
                                    catalogDataHolder = CreateInstance<CatalogDataHolder<ItemData>>();
                                    break;
                                case EffectGroupData effectGroupData:
                                    catalogDataHolder = CreateInstance<CatalogDataHolder<EffectGroupData>>();
                                    break;
                                default:
                                    catalogDataHolder = CreateInstance<CatalogDataHolder<CatalogData>>();
                                    break;
                            }
                            catalogDataHolder.DataId = catalogData.id;
                            catalogDataHolder.LoadJson();
                            groupItem.AddChild(new TreeItem(++id, catalogDataHolder.So, catalogData.id));
                        }
                    }
                }
                return root;
            }

            private class GroupItem : TreeViewItem
            {
                public GroupItem(int id, string name) : base(id, 0, name)
                { }
            }

            private class TreeItem : TreeViewItem
            {
                public ScriptableObject so { get; }
                public string name;
                public TreeItem(int id, ScriptableObject so, string name) : base(id, 1, name)
                {
                    this.so = so;
                    this.name = name;
                }
            }
        }

        internal static class SampleWindowStyles
        {
            public static readonly GUIStyle Padding;
            public static readonly GUIStyle BoxWithPadding;
            public static readonly GUIStyle HeaderDisplayNameLabel;

            static SampleWindowStyles()
            {
                Padding = new GUIStyle(GUI.skin.label) {
                    padding = new RectOffset(5, 5, 5, 5),
                };

                BoxWithPadding = new GUIStyle(TriEditorStyles.Box) {
                    padding = new RectOffset(5, 5, 5, 5),
                };

                HeaderDisplayNameLabel = new GUIStyle(EditorStyles.largeLabel) {
                    fontStyle = FontStyle.Bold,
                    fontSize = 17,
                    margin = new RectOffset(5, 5, 5, 0),
                };
            }
        }

        [Serializable]
        public class CatalogDataHolder<T> : ScriptableObject, ICatalogData where T : CatalogData
        {
            public T data;
            [Button]
            public void SaveJson()
            {
                Catalog.SaveToJson(data);
            }
            [Button]
            public void LoadJson()
            {
                Catalog.EditorLoadAllJson();
                data = Catalog.GetData<T>(DataId);
            }
            public string DataId { get; set; }
            public ScriptableObject So => this;
        }

        public interface ICatalogData
        {
            string DataId { get; set; }
            public void SaveJson();
            public void LoadJson();
            public ScriptableObject So { get; }
        }
    }
}
