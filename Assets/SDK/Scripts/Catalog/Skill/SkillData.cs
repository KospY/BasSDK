using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class SkillData : CatalogData
    {
        public string displayName;
        public string description;

        public string prefabAddress;

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllEffectID")]
#endif
        public string orbLinkEffectId;
        [NonSerialized]
        public EffectData orbLinkEffectData;

        public string imageAddress = "";
        [NonSerialized]
        public Texture image = null;

        public string videoAddress = "";
        [NonSerialized]
        public VideoClip video = null;

        public int depth;

        public int upgrades = 0;

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllSkillID")]
#endif
        public string[] dependancies;
        [NonSerialized]
        public SkillData[] dependantSkills;

        public string treeName;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllEffectID()
        {
            return Catalog.GetDropdownAllID(Category.Effect);
        }

        public List<ValueDropdownItem<string>> GetAllSkillID()
        {
            return Catalog.GetDropdownAllID(Category.Skill);
        }
#endif

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            if (orbLinkEffectId != null && orbLinkEffectId != "") orbLinkEffectData = Catalog.GetData<EffectData>(orbLinkEffectId);
            dependantSkills = new SkillData[dependancies != null ? dependancies.Length : 0];
            if (dependancies != null)
            {
                for (int i = 0; i < dependancies.Length; i++)
                {
                    if (dependancies[i] != null && dependancies[i] != "") dependantSkills[i] = Catalog.GetData<SkillData>(dependancies[i]);
                }
            }
        }
    }
}