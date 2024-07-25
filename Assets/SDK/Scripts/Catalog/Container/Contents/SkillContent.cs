using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ThunderRoad.Skill;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [System.Serializable]
    public class SkillContent : ContainerContent<SkillData, SkillContent>
    {
        public SkillContent() {}
        public override SkillContent CloneGeneric()
        {
            SkillContent result = new SkillContent(this.referenceID);
            return result;
        }

        public SkillContent(string referenceID)
        {
            this.referenceID = referenceID;
        }

        public SkillContent(SkillData skillData)
        {
            if (skillData is SpellData) 
                Debug.LogError($"SkillContent {skillData.id} was given a SpellData in its constructor. This will result in the spell not being loaded!");
            referenceID = skillData.id;
        }
    }
}
