using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;
using UnityEngine.Video;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class SkillTreeData : CatalogData
    {
        public string localizationGroupID;
        public string displayName;
        public string description;
        public int maxTier = 3;
        public bool showInInfuser = true;

        public Color color = Color.white;
        [ColorUsage(true, true)]
        public Color emissionColor = Color.white;

        public float orbPitch = 1;
        public float costMultiplier = 1;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllItemIds))]
#endif
        public string crystalItemId;

        public string orbIconAddress;
        public string infuserTopParticleAddress;
        public string infuserBeamAudioAddress;
        public string musicAddress;
        public string videoAddress;
        public string iconEnabledAddress;
        public string iconDisabledAddress;

        [NonSerialized]
        public ItemData crystalItemData;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllItemIds()
        {
            return Catalog.GetDropdownAllID(Category.Item);
        }
#endif

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            crystalItemData = Catalog.GetData<ItemData>(crystalItemId);
        }
    }
}
