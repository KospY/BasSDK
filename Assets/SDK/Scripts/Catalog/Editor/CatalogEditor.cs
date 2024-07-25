#if ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ThunderRoad.AI;
using ThunderRoad.Skill;

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
                //Get all categories order by name
                var categories = ((ThunderRoad.Category[])Enum.GetValues(typeof(ThunderRoad.Category))).OrderBy(x => x.ToString());
                //Loop through all categories
                foreach (ThunderRoad.Category category in categories)
                {
                    //Get the catalog data for this category
                    CatalogCategory catalogCategory = Catalog.data[(int)category];
                    if (catalogCategory == null) continue;
                    string categoryName = category.ToString();
                    string categoryNameSlash = $"{categoryName}s/";
                    foreach (CatalogData catalogData in catalogCategory.catalogDatas.OrderBy(i => i.SortKey()))
                    {
                        catalogData.CatalogEditorRefresh();
                        switch (category)
                        {
                            case Category.Item:
                                if (catalogData is ItemData itemData)
                                {
                                    bool needIcon = itemData.allowedStorage.HasFlag(ItemData.Storage.Inventory) && !itemData.despawnOnStoredInInventory;
                                    if (itemData.allowedStorage.HasFlag(ItemData.Storage.Container)) needIcon = true;
                                    Texture2D iconTexture = itemData != null && itemData.icon != null ? itemData.icon.texture : (needIcon ? Texture2D.whiteTexture : Texture2D.grayTexture) ;
                                    tree.Add($"{categoryNameSlash}{(catalogData as ItemData).type}/{catalogData.groupPath}{catalogData.id}", catalogData, iconTexture);
                                }
                                break;
                            case Category.EffectGroup:
                                tree.Add($"{Category.Effect}s{(catalogData as EffectGroupData).GetPath()}", catalogData);
                                break;
                            case Category.Effect: {
                                string path = null;
                                if ((catalogData as EffectData).groupId != null && (catalogData as EffectData).groupId != "")
                                {
                                    EffectGroupData effectGroupData = Catalog.GetData<EffectGroupData>((catalogData as EffectData).groupId);
                                    path = $"{categoryName}s{(effectGroupData != null ? effectGroupData.GetPath() : "")}/{catalogData.id}";
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
                                break;
                            }
                            case Category.DamageModifier: {
                                tree.Add(categoryNameSlash + catalogData.id, catalogData);
                                int i = 0;
                                foreach (DamageModifierData.Collision collision in (catalogData as DamageModifierData).collisions)
                                {
                                    tree.Add($"{categoryNameSlash}{catalogData.id}/{(collision.sourceMaterialIds.Count > 0 ? string.Join(",", collision.sourceMaterialIds) : "Any")} > {(collision.targetMaterialIds.Count > 0 ? string.Join(",", collision.targetMaterialIds) : "Any")}", collision);
                                    i++;
                                }
                                break;
                            }
                            case Category.Creature: {
                                tree.Add(categoryNameSlash + catalogData.id, catalogData);
                                int i = 0;
                                foreach (CreatureData.PartData creaturePartData in (catalogData as CreatureData).ragdollData.parts)
                                {
                                    tree.Add($"{categoryNameSlash}{catalogData.id}/{(creaturePartData.bodyPartTypes > 0 ? creaturePartData.bodyPartTypes.ToString() : "Any")}", creaturePartData);
                                    i++;
                                }
                                break;
                            }
                            case Category.ColliderGroup: {
                                tree.Add(categoryNameSlash + catalogData.id, catalogData);
                                int i = 0;
                                foreach (ColliderGroupData.Modifier modifier in (catalogData as ColliderGroupData).modifiers)
                                {
                                    tree.Add($"{categoryNameSlash}{catalogData.id}/{modifier.tierFilter}", modifier);
                                    i++;
                                }
                                break;
                            }
                            case Category.Brain: {
                                tree.Add(categoryNameSlash + catalogData.id, catalogData);
                                int i = 0;
                                BrainData brainData = catalogData as BrainData;
                                if (brainData.modules != null)
                                {
                                    foreach (BrainData.Module module in brainData.modules)
                                    {
                                        tree.Add($"{categoryNameSlash}{catalogData.id}/{module.GetType().Name}", module);
                                        i++;
                                    }
                                }
                                break;
                            }
                            case Category.Material: {
                                tree.Add(categoryNameSlash + catalogData.id, catalogData);
                                int i = 0;
                                foreach (MaterialData.Collision collision in (catalogData as MaterialData).collisions)
                                {
                                    tree.Add($"{categoryNameSlash}{catalogData.id}/{(collision.targetMaterialIds.Count > 0 ? string.Join(",", collision.targetMaterialIds) : "Any")}", collision);
                                    i++;
                                }
                                break;
                            }
                            case Category.Text: {
                                tree.Add(categoryNameSlash + catalogData.id, catalogData);
                                int i = 0;
                                if ((catalogData as TextData).textGroups == null)
                                    continue;
                                foreach (TextData.TextGroup textGroup in (catalogData as TextData).textGroups)
                                {
                                    tree.Add($"{categoryNameSlash}{catalogData.id}/{textGroup.id}", textGroup);
                                    i++;
                                }
                                break;
                            }
                            // spell and skills are treated the same since spells are a skill
                            // case Category.Spell:
                            // {
                            //     SkillData skillData = catalogData as SkillData;
                            //     string treeName = skillData.GetCatalogEditorTreeName();
                            //     // tree.Add($"Skills/{(treeName == "" ? "No Tree Set" : treeName)}/{catalogData.id}", catalogData);
                            //     tree.Add($"Skills/{(treeName == "" ? "No Tree Set" : treeName)}/{(!skillData.showInTree ? "-- " : "")}T{skillData.tier + 1}{(skillData.isTierBlocker ? "*" : "")} {skillData.skillTreeDisplayName}", catalogData);
                            //     break;
                            // }
                            case Category.Skill: {
                                SkillData skillData = catalogData as SkillData;
                                string treeName = skillData.GetCatalogEditorTreeName();

                                if (skillData is AISkillData)
                                {
                                    tree.Add($"{categoryNameSlash}AI/{skillData.id}", catalogData);
                                    break;
                                }

                                tree.Add($"{categoryNameSlash}{(treeName == "" ? "No Tree Set" : treeName)}/"
                                         + $"{(!skillData.showInTree && !skillData.isDefaultSkill ? "-- " : "")}"
                                         + $"T{(skillData.isDefaultSkill ? 0 : skillData.tier + 1)}"
                                         + $"{(skillData.isTierBlocker ? "*" : "")} {skillData.id}",
                                    catalogData, skillData.icon);

                                break;
                            }
                            case Category.EntityModule:
                                tree.Add($"{categoryNameSlash}{catalogData.id}", catalogData);
                                break;
                            case Category.BehaviorTree:
                                tree.Add($"{categoryNameSlash}{(catalogData as BehaviorTreeData).type}/{catalogData.id}", catalogData);
                                break;
                            case Category.Status:
                                tree.Add($"{categoryName}es/{catalogData.id}", catalogData);
                                break;
                            case Category.Custom:
                                var type = catalogData.GetType().GetNiceName();
                                if (string.IsNullOrEmpty(catalogData.groupPath))
                                {
                                    tree.Add($"{categoryNameSlash}{type}/{catalogData.id}", catalogData);
                                }
                                else
                                {
                                    tree.Add($"{categoryNameSlash}{type}/{catalogData.groupPath}/{catalogData.id}", catalogData);
                                }
                                break;
                            default:
                                if (string.IsNullOrEmpty(catalogData.groupPath))
                                {
                                    tree.Add($"{categoryNameSlash}{catalogData.id}", catalogData);
                                }
                                else
                                {
                                    tree.Add($"{categoryNameSlash}{catalogData.groupPath}/{catalogData.id}", catalogData);
                                }
                                break;
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
	                    //ignore abstract classes
	                    if (type.IsAbstract) continue;
                        if (type.IsSubclassOf(typeof(CatalogData))) typeList.Add(type);
                    }

                    TypeSelector selector = new TypeSelector(typeList, false);
                    selector.SetSelection(typeof(ItemData));

                    selector.SelectionConfirmed += selection =>
                    {
                        Type selectedType = selection.FirstOrDefault();
                        CatalogData newCatalogData = (CatalogData)Activator.CreateInstance(selectedType);
                        newCatalogData.id = $"New {selectedType}";
                        if (Catalog.TryGetCategory(selectedType, out Category category))
                        {
                            Catalog.GetDataList(category).Add(newCatalogData);
                            newCatalogData.OnCatalogRefresh();
                            base.ForceMenuTreeRebuild();
                            base.TrySelectMenuItemWithObject(newCatalogData);
                        }
                        else
                        {
	                        Debug.LogError($"Could not find category for type {selectedType}, unable to add to the catalog");
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
	                if (Catalog.gameData == null) return;
                    object orgSelection = selected != null ? selected.Value : null;
                    Catalog.Refresh();
                    base.ForceMenuTreeRebuild();
                    if (orgSelection != null)
                    {
	                    base.TrySelectMenuItemWithObject(orgSelection);
                    }
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Load JSON", EditorIcons.Download.Raw, "Load all json from StreamingAssets")))
                {
	                //subscribe to events
	                Catalog.OnRefreshProgress += OnCatalogOnOnRefreshProgress;
	                Catalog.EditorLoadAllJson(true, true, false);
	                base.ForceMenuTreeRebuild();
	                Catalog.OnRefreshProgress -= OnCatalogOnOnRefreshProgress;
                    
                }
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Load JSON with Mods", EditorIcons.Download.Raw, "Load all json from StreamingAssets, including mods")))
                {
	                Catalog.OnRefreshProgress += OnCatalogOnOnRefreshProgress;
	                Catalog.EditorLoadAllJson(true, true, true);
	                base.ForceMenuTreeRebuild();
	                Catalog.OnRefreshProgress -= OnCatalogOnOnRefreshProgress;
                }
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Unload JSON", EditorIcons.Eject.Raw, "Unload all json")))
                {
                    ModManager.loadedMods.Clear();
                    ModManager.gameModsLoaded = false;
                    Catalog.Clear();
                    base.ForceMenuTreeRebuild();
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();

            if (selected is { Value: CatalogData or GameData })
            {
                SirenixEditorGUI.Title(selected.Value.GetType().ToString(), (selected.Value is CatalogData) ? (selected.Value as CatalogData).id : "", TextAlignment.Center, true);

                SirenixEditorGUI.BeginHorizontalToolbar();
                {
                    GUILayout.Label("");
                    if (SirenixEditorGUI.ToolbarButton(new GUIContent("Save Category", EditorIcons.Pen.Raw, "Save Category")))
                    {
                        if (selected.Value is CatalogData selectedData)
                        {
                            foreach (CatalogData catalogData in Catalog.GetDataList(Catalog.GetCategory(selectedData.GetType())))
                            {
                                Catalog.SaveToJson(catalogData);
                            }
                        }
                        else if (selected.Value is GameData)
                        {
                            Catalog.SaveGameData();
                        }
                    }
                    if (SirenixEditorGUI.ToolbarButton(new GUIContent("Save", EditorIcons.Pen.Raw, "Save JSON")))
                    {
                        if (selected.Value is CatalogData selectedData)
                        {
                            Catalog.SaveToJson(selectedData);
                        }
                        else if (selected.Value is GameData)
                        {
                            Catalog.SaveGameData();
                        }
                    }

                    if (SirenixEditorGUI.ToolbarButton(new GUIContent("Delete", EditorIcons.X.Raw, "Delete selected object")))
                    {
                        if (selected.Value is CatalogData selectCatalogData)
                        {
                            if (Catalog.TryGetCategory(selected.Value.GetType(), out Category category))
                            {
	                            object previousMenuItem = null;
	                            if(selected.PrevVisualMenuItem != null && selected.PrevVisualMenuItem.Value != null) previousMenuItem = selected.PrevVisualMenuItem.Value;
                                if (File.Exists(selectCatalogData.filePath))
                                {
                                    File.Delete(selectCatalogData.filePath);
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
                            System.Diagnostics.Process.Start($"{FileManager.GetFullPath(FileManager.Type.JSONCatalog, FileManager.Source.Default, (selected.Value as CatalogData).GetLatestOverrideFolder())}/{category}s");
                        }
                    }

                    if (SirenixEditorGUI.ToolbarButton(new GUIContent("Clone", EditorIcons.Next.Raw, "Clone the selected object")))
                    {
                        if (selected.Value is CatalogData selectedData)
                        {
                            if (Catalog.TryGetCategory(selectedData.GetType(), out Category category))
                            {
                                var clone = selectedData.CloneJson();
                                clone.id = $"{clone.id} Copy";
                                Catalog.GetDataList(category).Add(clone);
                                base.TrySelectMenuItemWithObject(clone);
                                clone.sourceFolders = selectedData.sourceFolders;
                                clone.standaloneData = selectedData.standaloneData;
                            }
                        }
                        base.ForceMenuTreeRebuild();
                    }
                }
                SirenixEditorGUI.EndHorizontalToolbar();
            }
        }
        private void OnCatalogOnOnRefreshProgress(int total, int current, string message)
        {
	        EditorUtility.DisplayProgressBar($"Loading JSON", $"Loading {message} [{current}/{total}]", (float)current / total);
	        if (total == current)
	        {
		        EditorUtility.ClearProgressBar();
	        }
        }
    }
}
#endif