#if ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ThunderRoad.AI;

namespace ThunderRoad
{
    public class CatalogEditor : OdinMenuEditorWindow
    {
        protected OdinMenuTree tree;

        [MenuItem("ThunderRoad (SDK)/Catalog Editor")]
        private static void Open()
        {
            var window = GetWindow<CatalogEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;
            if (Catalog.data != null)
            {
                tree.Add("Settings", ThunderRoadSettings.current);
                if (Catalog.gameData != null) tree.Add("Game", Catalog.gameData);

                var categories = ((ThunderRoad.Category[])Enum.GetValues(typeof(ThunderRoad.Category))).OrderBy(x => x.ToString());
                foreach (ThunderRoad.Category category in categories)
                {
                    CatalogCategory catalogCategory = Catalog.data[(int)category];
                    if (catalogCategory == null) continue;
                    string categoryName = category.ToString();
                    string categoryNameSlash = categoryName + "s/";
                    foreach (CatalogData catalogData in catalogCategory.catalogDatas.OrderBy(i => i.id))
                    {
                        if (category == Category.Item)
                        {
                            tree.Add(categoryNameSlash + (catalogData as ItemData).type.ToString() + "/" + catalogData.id, catalogData, (catalogData as ItemData).icon);
                        }
                        else if (category == Category.EffectGroup)
                        {
                            tree.Add(Category.Effect.ToString() + "s" + (catalogData as EffectGroupData).GetPath(), catalogData);
                        }
                        else if (category == Category.Effect)
                        {
                            string path = null;
                            if ((catalogData as EffectData).groupId != null && (catalogData as EffectData).groupId != "")
                            {
                                EffectGroupData effectGroupData = Catalog.GetData<EffectGroupData>((catalogData as EffectData).groupId);
                                path = categoryName + "s" + (effectGroupData != null ? effectGroupData.GetPath() : "") + "/" + catalogData.id;
                                tree.Add(path, catalogData);
                            }
                            else
                            {
                                path = categoryNameSlash + catalogData.id;
                                tree.Add(path, catalogData);
                            }
                            /*
                            int i = 0;
                            foreach (EffectModule effectModule in (catalogData as EffectData).modules)
                            {
                                tree.Add(path + "/" + i + " > " + effectModule.GetType().Name, effectModule);
                                i++;
                            }*/
                        }
                        else if (category == Category.Area)
                        {
                            tree.Add(categoryNameSlash + (catalogData as AreaData).groupPath + "/" + catalogData.id, catalogData);
                        }
                        else if (category == Category.LootTable)
                        {
                            tree.Add(categoryNameSlash + (catalogData as LootTable).groupPath + "/" + catalogData.id, catalogData);
                        }
                        else if (category == Category.DamageModifier)
                        {
                            tree.Add(categoryNameSlash + catalogData.id, catalogData);
                            int i = 0;
                            foreach (DamageModifierData.Collision collision in (catalogData as DamageModifierData).collisions)
                            {
                                tree.Add(categoryNameSlash + catalogData.id + "/" + (collision.sourceMaterialIds.Count > 0 ? string.Join(",", collision.sourceMaterialIds) : "Any") + " > " + (collision.targetMaterialIds.Count > 0 ? string.Join(",", collision.targetMaterialIds) : "Any"), collision);
                                i++;
                            }
                        }
                        else if (category == Category.Creature)
                        {
                            tree.Add(categoryNameSlash + catalogData.id, catalogData);
                            int i = 0;
                            foreach (CreatureData.PartData creaturePartData in (catalogData as CreatureData).ragdollData.parts)
                            {
                                tree.Add(categoryNameSlash + catalogData.id + "/" + (creaturePartData.bodyPartTypes > 0 ? creaturePartData.bodyPartTypes.ToString() : "Any"), creaturePartData);
                                i++;
                            }
                        }
                        else if (category == Category.ColliderGroup)
                        {
                            tree.Add(categoryNameSlash + catalogData.id, catalogData);
                            int i = 0;
                            foreach (ColliderGroupData.Modifier modifier in (catalogData as ColliderGroupData).modifiers)
                            {
                                tree.Add(categoryNameSlash + catalogData.id + "/" + modifier.tierFilter.ToString(), modifier);
                                i++;
                            }
                        }
                        else if (category == Category.Brain)
                        {
                            tree.Add(categoryNameSlash + catalogData.id, catalogData);
                            int i = 0;
                            foreach (BrainData.Module module in (catalogData as BrainData).modules)
                            {
                                tree.Add(categoryNameSlash + catalogData.id + "/" + module.GetType().Name, module);
                                i++;
                            }
                        }
                        else if (category == Category.Material)
                        {
                            tree.Add(categoryNameSlash + catalogData.id, catalogData);
                            int i = 0;
                            foreach (MaterialData.Collision collision in (catalogData as MaterialData).collisions)
                            {
                                tree.Add(categoryNameSlash + catalogData.id + "/" + (collision.targetMaterialIds.Count > 0 ? string.Join(",", collision.targetMaterialIds) : "Any"), collision);
                                i++;
                            }
                        }
                        else if (category == Category.Text)
                        {
                            tree.Add(categoryNameSlash + catalogData.id, catalogData);
                            int i = 0;
                            if ((catalogData as TextData).textGroups == null)
                                continue;
                            foreach (TextData.TextGroup textGroup in (catalogData as TextData).textGroups)
                            {
                                tree.Add(categoryNameSlash + catalogData.id + "/" + textGroup.id, textGroup);
                                i++;
                            }
                        }
                        else if (category == Category.Skill)
                        {
                            string treeName = (catalogData as SkillData).treeName;
                            tree.Add(categoryNameSlash + (treeName == "" ? "Cross" : treeName) + "/" + catalogData.id, catalogData);
                        }
                        else if (category == Category.BehaviorTree)
                        {
                            tree.Add(categoryNameSlash + (catalogData as BehaviorTreeData).type.ToString() + "/" + catalogData.id, catalogData);
                        }
                        else
                        {
                            tree.Add(categoryNameSlash + catalogData.id, catalogData);
                        }
                    }
                }
            }

            /*
            // Add drag handles to items, so they can be easily dragged into the inventory if characters etc...
            tree.EnumerateTree().Where(x => x.Value as Item).ForEach(AddDragHandles);

            // Add icons to characters and items.
            tree.EnumerateTree().AddIcons<Character>(x => x.Icon);
            tree.EnumerateTree().AddIcons<Item>(x => x.Icon);
            */
            return tree;
        }

        private void AddDragHandles(OdinMenuItem menuItem)
        {
            menuItem.OnDrawItem += x => DragAndDropUtilities.DragZone(menuItem.Rect, menuItem.Value, false, false);
        }

        protected override void OnBeginDrawEditors()
        {
            if (this.MenuTree == null) return;
            var selected = this.MenuTree.Selection.FirstOrDefault();

            SirenixEditorGUI.BeginHorizontalToolbar(this.MenuTree.Config.SearchToolbarHeight);
            {
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("New", EditorIcons.Plus.Raw, "Create a new catalog object")))
                {
                    List<Type> typeList = new List<Type>();

                    foreach (Type type in typeof(CatalogData).Assembly.GetTypes())
                    {
                        if (type.IsSubclassOf(typeof(CatalogData))) typeList.Add(type);
                    }

                    TypeSelector selector = new TypeSelector(typeList, false);
                    selector.SetSelection(typeof(ItemData));

                    selector.SelectionConfirmed += selection =>
                    {
                        Type selectedType = selection.FirstOrDefault();
                        CatalogData newCatalogData = (CatalogData)Activator.CreateInstance(selectedType);
                        newCatalogData.id = "New " + selectedType;
                        if (Catalog.TryGetCategory(selectedType, out Category category))
                        {
                            Catalog.GetDataList(category).Add(newCatalogData);
                            newCatalogData.OnCatalogRefresh();
                            base.ForceMenuTreeRebuild();
                            base.TrySelectMenuItemWithObject(newCatalogData);
                        }

                    };
                    selector.ShowInPopup();
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Save JSON", EditorIcons.Pen.Raw, "Save all json to StreamingAssets")))
                {
                    Catalog.SaveAllJson();
                    base.ForceMenuTreeRebuild();
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Refresh", EditorIcons.Refresh.Raw)))
                {
                    object orgSelection = selected.Value;
                    Catalog.Refresh();
                    base.ForceMenuTreeRebuild();
                    base.TrySelectMenuItemWithObject(orgSelection);
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Load JSON", EditorIcons.Download.Raw, "Load all json from StreamingAssets")))
                {
                    Catalog.loadMods = false;
                    Catalog.EditorLoadAllJson(true);
                    Catalog.Refresh();
                    base.ForceMenuTreeRebuild();
                }
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Unload JSON", EditorIcons.Eject.Raw, "Unload all json")))
                {
                    Catalog.Clear();
                    base.ForceMenuTreeRebuild();
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();

            if (selected != null && selected.Value != null && selected.Value is CatalogData)
            {
                SirenixEditorGUI.Title(selected.Value.GetType().ToString(), (selected.Value is CatalogData) ? (selected.Value as CatalogData).id : "", TextAlignment.Center, true);

                SirenixEditorGUI.BeginHorizontalToolbar();
                {
                    GUILayout.Label("");
                    if (SirenixEditorGUI.ToolbarButton(new GUIContent("Save", EditorIcons.Pen.Raw, "Save JSON")))
                    {
                        if (selected.Value is CatalogData)
                        {
                            Catalog.SaveToJson((selected.Value as CatalogData));
                        }
                    }

                    if (SirenixEditorGUI.ToolbarButton(new GUIContent("Delete", EditorIcons.X.Raw, "Delete selected object")))
                    {
                        if (selected.Value is CatalogData)
                        {
                            if (Catalog.TryGetCategory(selected.Value.GetType(), out Category category))
                            {
                                object previousMenuItem = selected.PrevVisualMenuItem.Value;
                                if (File.Exists((selected.Value as CatalogData).filePath))
                                {
                                    File.Delete((selected.Value as CatalogData).filePath);
                                }
                                Catalog.GetDataList(category).Remove(selected.Value as CatalogData);
                                base.TrySelectMenuItemWithObject(previousMenuItem);
                            }
                        }
                        base.ForceMenuTreeRebuild();
                    }
                    if (SirenixEditorGUI.ToolbarButton(new GUIContent("Open JSON folder", EditorIcons.Folder.Raw, "Open the folder containing the JSON of the selected object")))
                    {
                        if (Catalog.TryGetCategory(selected.Value.GetType(), out Category category))
                        {
                            System.Diagnostics.Process.Start(FileManager.GetFullPath(FileManager.Type.JSONCatalog, FileManager.Source.Default, (selected.Value as CatalogData).GetLatestOverrideFolder()) + "/" + category.ToString() + "s");
                        }
                    }

                    if (SirenixEditorGUI.ToolbarButton(new GUIContent("Clone", EditorIcons.Next.Raw, "Clone the selected object")))
                    {
                        if (selected.Value is CatalogData)
                        {
                            if (Catalog.TryGetCategory(selected.Value.GetType(), out Category category))
                            {
                                JsonSerializerSettings jsonSerializerSettings = Catalog.GetJsonNetSerializerSettings();
                                string json = JsonConvert.SerializeObject(selected.Value, typeof(CatalogData), jsonSerializerSettings);
                                CatalogData clone = JsonConvert.DeserializeObject<CatalogData>(json, jsonSerializerSettings);
                                clone.id = clone.id + " Copy";
                                Catalog.GetDataList(category).Add(clone);
                                base.TrySelectMenuItemWithObject(clone);
                                clone.sourceFolders = (selected.Value as CatalogData)?.sourceFolders;
                            }
                        }
                        base.ForceMenuTreeRebuild();
                    }
                }
                SirenixEditorGUI.EndHorizontalToolbar();
            }
        }
    }
}
#endif