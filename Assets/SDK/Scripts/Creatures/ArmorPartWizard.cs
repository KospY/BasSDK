#if UNITY_EDITOR
// Copyright (c) WarpFrog. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using ThunderRoad.Manikin;
using TriInspector;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace ThunderRoad
{
    [DrawWithTriInspector]
    [HideMonoScript]
    [DeclareVerticalGroup("Top")]
    [DeclareHorizontalGroup("Top/Defaults")]
    [DeclareHorizontalGroup("Top/Addressables")]
    [DeclareHorizontalGroup("Top/AddressableOptions")]
    [DeclareHorizontalGroup("Top/Buttons")]
    public class ArmorPartWizard : MonoBehaviour
    {
        [Group("Top/Defaults")]
        [LabelWidth(95f)]
        public PhysicMaterial defaultMaterial;
        [Group("Top/Defaults")]
        [LabelText("Default Part SMR LOD Level")]
        [LabelWidth(165f)]
        public int defaultPartSMRLODLevel = 1;
        [Group("Top/Defaults")]
        [LabelWidth(65f)]
        public GameObject rigPrefab;
        [Group("Top/Addressables")]
        [LabelWidth(110f)]
        public AddressableAssetGroup partAddressables;
        [Group("Top/Addressables")]
        [LabelWidth(135f)]
        public AddressableAssetGroup wardrobeAddresables;
        [Group("Top/AddressableOptions")]
        [LabelWidth(85f)]
        public string addressPrefix = string.Empty;
        [Group("Top/AddressableOptions")]
        [LabelWidth(85f)]
        public string defaultLabels = string.Empty;
        [ListDrawerSettings(AlwaysExpanded = true, HideAddButton = true, HideRemoveButton = false)]
        public List<LODsArmorPart> armorParts = new List<LODsArmorPart>();

        [System.Serializable]
        [InlineProperty]
        [DeclareHorizontalGroup("PartHoriz")]
        public class LODsArmorPart
        {
            [HideLabel]
            [ReadOnly]
            public string name;
            [Group("PartHoriz")]
            [LabelText("Part SMR LOD")]
            [LabelWidth(85f)]
            [Dropdown(nameof(GetLODOptions))]
            public int partSMRlod = 1;
            [Group("PartHoriz")]
            [LabelWidth(95f)]
            public PhysicMaterial defaultMaterial;
            [LabelWidth(90f)]
            public ManikinWardrobeData wardrobeData;
            [ListDrawerSettings(AlwaysExpanded = true)]
            [LabelWidth(60f)]
            public List<LOD> lods = new List<LOD>();

            [NonSerialized]
            public GameObject root;

            [System.Serializable]
            [DeclareHorizontalGroup("LODHoriz", Sizes = new float[] { 270f, 0f, 0f, 0f })]
            [InlineProperty]
            public class LOD
            {
                [Group("LODHoriz")]
                [HideLabel]
                public SkinnedMeshRenderer renderer;
                [Group("LODHoriz")]
                [HideLabel]
                [OnValueChanged(nameof(WidthUpdate))]
                public RevealDecal.RevealMaskResolution revealWidth;
                [Group("LODHoriz")]
                [HideLabel]
                [OnValueChanged(nameof(RelationshipUpdate))]
                public Relationship relationship = Relationship.Equal;
                [Group("LODHoriz")]
                [HideLabel]
                [EnableIf(nameof(relationship), Relationship.Custom)]
                public RevealDecal.RevealMaskResolution revealHeight;

                public enum Relationship
                {
                    Equal,
                    Half,
                    Custom,
                }

                public LOD(SkinnedMeshRenderer meshRenderer)
                {
                    this.renderer = meshRenderer;
                    this.revealWidth = RevealDecal.RevealMaskResolution.Size_512;
                    this.revealHeight = RevealDecal.RevealMaskResolution.Size_512;
                }

                public void WidthUpdate()
                {
                    switch (relationship)
                    {
                        case Relationship.Equal:
                            revealHeight = revealWidth;
                            break;
                        case Relationship.Half:
                            revealHeight = (RevealDecal.RevealMaskResolution)Mathf.Max((int)revealWidth / 2, 32);
                            break;
                        default:
                            break;
                    }
                }

                public void RelationshipUpdate()
                {
                    switch (relationship)
                    {
                        case Relationship.Equal:
                            revealHeight = revealWidth;
                            break;
                        case Relationship.Half:
                            revealHeight = (RevealDecal.RevealMaskResolution)Mathf.Max((int)revealWidth / 2, 32);
                            break;
                        default:
                            break;
                    }
                }
            }

            public TriDropdownList<int> GetLODOptions()
            {
                TriDropdownList<int> result = new TriDropdownList<int>();
                for (int i = 0; i < lods.Count; i++)
                {
                    result.Add($"LOD{i}", i);
                }
                return result;
            }

            public LODsArmorPart(string name, PhysicMaterial defaultMaterial, int partSMRlod)
            {
                this.name = name;
                this.defaultMaterial = defaultMaterial;
                this.partSMRlod = partSMRlod;
            }
        }

        [Button]
        [Group("Top/Buttons")]
        public void GatherParts()
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            this.armorParts = new List<LODsArmorPart>();
            Dictionary<string, LODsArmorPart> armorParts = new Dictionary<string, LODsArmorPart>();
            foreach (Transform child in transform)
            {
                if (child.GetComponent<SkinnedMeshRenderer>() is not SkinnedMeshRenderer renderer) continue;
                int lodStringIndex = child.name.IndexOf("_LOD");
                if (lodStringIndex == -1) continue;
                string lodStrippedName = child.name.Substring(0, lodStringIndex);
                if (!armorParts.TryGetValue(lodStrippedName, out LODsArmorPart existingPart))
                {
                    existingPart = new LODsArmorPart(lodStrippedName, defaultMaterial, defaultPartSMRLODLevel);
                    armorParts[lodStrippedName] = existingPart;
                }
                if (existingPart.lods.IsNullOrEmpty()) existingPart.lods = new List<LODsArmorPart.LOD>();
                existingPart.lods.Add(new LODsArmorPart.LOD(renderer));
            }
            foreach (LODsArmorPart armorPart in armorParts.Values)
            {
                armorPart.lods = armorPart.lods.OrderBy(lod => lod.renderer.name).ToList();
                armorPart.partSMRlod = Mathf.Min(armorPart.partSMRlod, armorPart.lods.Count - 1);
                this.armorParts.Add(armorPart);
                if (wardrobeAddresables != null)
                {
                    foreach (AddressableAssetEntry wardrobeEntry in wardrobeAddresables.entries)
                    {
                        if (wardrobeEntry.TargetAsset is ManikinWardrobeData wardrobeData
                            && wardrobeData.assetPrefab != null
                            && settings.FindAssetEntry(wardrobeData.assetPrefab.AssetGUID) is AddressableAssetEntry partEntry
                            && partEntry.parentGroup == partAddressables
                            && partEntry.TargetAsset.name == armorPart.name)
                        {
                            armorPart.wardrobeData = wardrobeData;
                        }
                    }
                }
            }
        }

        [Button]
        [Group("Top/Buttons")]
        public void SetupParts()
        {
            foreach (LODsArmorPart armorPart in armorParts)
            {
                ManikinGroupPart manikinGroupPart = new GameObject(armorPart.name).AddComponent<ManikinGroupPart>();
                manikinGroupPart.transform.parent = transform;
                armorPart.root = manikinGroupPart.gameObject;
                Transform lod0Transform = armorPart.lods[0].renderer.transform;
                manikinGroupPart.transform.SetPositionAndRotation(lod0Transform.position, lod0Transform.rotation);
                manikinGroupPart.rigPrefab = rigPrefab;
                MeshPart meshPart = manikinGroupPart.gameObject.AddComponent<MeshPart>();
                meshPart.skinnedMeshRenderer = armorPart.lods[armorPart.partSMRlod].renderer;
                meshPart.defaultPhysicMaterial = armorPart.defaultMaterial;
                foreach (LODsArmorPart.LOD lod in armorPart.lods)
                {
                    lod.renderer.transform.parent = manikinGroupPart.transform;
                    ManikinGroupPart.PartLOD groupPartLOD = new ManikinGroupPart.PartLOD();
                    groupPartLOD.renderers = new List<Renderer> { lod.renderer };
                    manikinGroupPart.partLODs.Add(groupPartLOD);
                    ManikinSmrPart smrPart = lod.renderer.gameObject.AddComponent<ManikinSmrPart>();
                    MaterialInstance materialInstance = lod.renderer.gameObject.AddComponent<MaterialInstance>();
                    RevealDecal revealDecal = lod.renderer.gameObject.AddComponent<RevealDecal>();
                    revealDecal.maskHeight = lod.revealHeight;
                    revealDecal.maskWidth = lod.revealWidth;
                    smrPart.Initialize();
                }
                manikinGroupPart.Initialize();
                manikinGroupPart.InitializeLODs();
            }
        }

        [Button]
        [Group("Top/Buttons")]
        public void CreatePrefabs()
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            foreach (LODsArmorPart armorPart in armorParts)
            {
                string assetPath = string.Empty;
                string assetAddress = string.Empty;
                HashSet<string> labels = new HashSet<string>();
                if (armorPart.wardrobeData != null && armorPart.wardrobeData.assetPrefab != null)
                {
                    string existingAssetGUID = armorPart.wardrobeData.assetPrefab.AssetGUID;
                    AddressableAssetEntry existingAssetEntry = settings.FindAssetEntry(existingAssetGUID);
                    assetPath = existingAssetEntry.AssetPath;
                    assetAddress = existingAssetEntry.address;
                    foreach (string label in existingAssetEntry.labels)
                    {
                        labels.Add(label);
                    }
                }
                else
                {
                    string partsFolderFromMesh = AssetDatabase.GetAssetPath(armorPart.lods[0].renderer.sharedMesh);
                    partsFolderFromMesh = partsFolderFromMesh.Substring(0, partsFolderFromMesh.LastIndexOf('/')) + "/Parts";
                    assetPath = partsFolderFromMesh + armorPart.name + ".prefab";
                    assetAddress = addressPrefix + armorPart.name;
                    foreach (string label in defaultLabels.Split(','))
                    {
                        labels.Add(label.Trim());
                    }
                }
                if (AssetDatabase.AssetPathToGUID(assetPath) != string.Empty)
                {
                    AssetDatabase.DeleteAsset(assetPath);
                }
                PrefabUtility.SaveAsPrefabAssetAndConnect(armorPart.root, assetPath, InteractionMode.UserAction);
                if (armorPart.wardrobeData != null)
                {
                    armorPart.wardrobeData.assetPrefab = new AssetReferenceManikinPart(AssetDatabase.AssetPathToGUID(assetPath));
                }

                //make this asset addressable in the Default group and with the address "Achievements/{name}" and the label "achievement"
                if (Common.TryGetMoveOrCreateAddressableEntry(this, partAddressables, out AddressableAssetEntry entry))
                {
                    entry.address = assetAddress;
                    entry.labels.Clear();
                    foreach (string label in labels)
                    {
                        entry.SetLabel(label, true);
                    }
                    partAddressables.SetDirty(AddressableAssetSettings.ModificationEvent.EntryModified, entry, true, true);
                }
            }
        }
    }
}
#endif
