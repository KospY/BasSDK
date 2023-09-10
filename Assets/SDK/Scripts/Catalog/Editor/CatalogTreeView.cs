using System.Collections.Generic;
using System.Linq;
using System;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace ThunderRoad
{
    public class CatalogTreeView : TreeView
    {
        public Dictionary<string, CatalogData> openFiles;
        public Dictionary<int, TreeViewItem> items = new();

        public CatalogTreeView(Dictionary<string, CatalogData> openFiles, CatalogTreeViewState treeViewState) : base(treeViewState)
        {
            this.openFiles = openFiles;

            // Default foldout height is 8, so a 7 offset will make the height 15
            customFoldoutYOffset = 7;

            Reload();
        }

        public void SetUnsaved(IList<int> unsavedIDs) => (state as CatalogTreeViewState).UnsavedIDs = (List<int>)unsavedIDs;

        public IList<int> GetUnsaved() => (state as CatalogTreeViewState).UnsavedIDs;

        protected override TreeViewItem BuildRoot()
        {
            TreeViewItem root = new(0, -1, "Root");
            Dictionary<Category, TreeViewItem> categories = new();
            int i = 0;
            foreach (Category category in Enum.GetValues(typeof(Category)).Cast<Category>().OrderBy(val => val.ToString()))
            {
                TreeViewItem item = new(++i, 0, category.ToString());
                items[i] = item;
                categories[category] = item;
                root.AddChild(item);
            }

            foreach (KeyValuePair<string, CatalogData> entry in openFiles)
            {
                CatalogTreeViewItem item = new(++i, 2, entry.Value, entry.Key);
                Category dataCategory = Catalog.GetCategory(item.data.GetType());
                items[i] = item;
                categories[dataCategory].AddChild(item);
            }

            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            // Background color
            if (Event.current.type == EventType.Repaint && !args.selected && !args.isRenaming)
            {
                Rect rect = args.rowRect;
                rect.x = 0;
                if (args.item.depth == 0)
                {
                    DefaultStyles.backgroundOdd.Draw(rect, false, false, false, false);

                    // Bottom line color
                    rect.y += rect.height - 1;
                    rect.height = 1;
                    new GUIStyle(GUI.skin.horizontalScrollbar)
                    {
                        fixedHeight = 1
                    }.Draw(rect, false, false, false, false);
                }
                else
                    DefaultStyles.backgroundEven.Draw(rect, false, false, false, false);
            }

            // Render text
            args.rowRect.x = GetContentIndent(args.item) + GetFoldoutIndent(args.item);
            args.rowRect.height = GetCustomRowHeight(args.row, args.item);
            if (args.item is CatalogTreeViewItem item)
            {
                bool unsaved = GetUnsaved().Contains(item.id);
                GUIContent idGUIContent = new(item.data.id + (unsaved ? "*" : ""));
                GUI.Label(args.rowRect, idGUIContent);

                args.rowRect.x += GUI.skin.label.CalcSize(idGUIContent).x;
                GUIStyle italics = new(GUI.skin.label) 
                { 
                    fontStyle = FontStyle.Italic, 
                    normal = { textColor = Color.gray },
                    hover = { textColor = Color.gray }
                };
                GUI.Label(args.rowRect, new GUIContent($"({item.path})"), italics);
            }
            else
                GUI.Label(args.rowRect, new GUIContent(args.label));
        }

        protected override float GetCustomRowHeight(int row, TreeViewItem item)
            => item.depth == 0 ? 30 : 20;

        protected override bool CanMultiSelect(TreeViewItem item)
            => false;

        protected override bool DoesItemMatchSearch(TreeViewItem item, string search)
        {
            if (item is CatalogTreeViewItem catalogItem)
                return catalogItem.path.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 
                    || catalogItem.data.id.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
            else
                return base.DoesItemMatchSearch(item, search);
        }
    }

    public class CatalogTreeViewItem : TreeViewItem
    {
        public CatalogData data;
        public string path;

        public CatalogTreeViewItem(int id, int depth, CatalogData data, string path) : base(id, depth)
        {
            this.data = data;
            this.path = path;
        }
    }

    [Serializable]
    public class  CatalogTreeViewState : TreeViewState
    {
        [SerializeField]
        private List<int> m_UnsavedIDs = new();

        public List<int> UnsavedIDs { get => m_UnsavedIDs; set => m_UnsavedIDs = value; }
    }
}