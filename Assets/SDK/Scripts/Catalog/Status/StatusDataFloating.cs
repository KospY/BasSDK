#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad.Skill.Spell;
using UnityEngine;
using UnityEngine.Serialization;

namespace ThunderRoad
{
    public class StatusDataFloating : StatusData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Physic Modifier")]
#endif
        public float gravityMult = 0; 
#if ODIN_INSPECTOR
        [BoxGroup("Physic Modifier")]
#endif
        public float dragRatio = 2f; 
#if ODIN_INSPECTOR
        [BoxGroup("Physic Modifier")]
#endif
        public float gravityMultPlayer = 0.3f; 
#if ODIN_INSPECTOR
        [BoxGroup("Physic Modifier")]
#endif
        public float dragRatioPlayer = 3f; 
#if ODIN_INSPECTOR
        [BoxGroup("Physic Modifier")]
#endif
        public float massRatio = -1f; 
#if ODIN_INSPECTOR
        [BoxGroup("Physic Modifier")]
#endif
        public float angularDragRatio = 2f; 
#if ODIN_INSPECTOR
        [BoxGroup("Force")]
#endif
        public float upwardsForceCreature = 30f; 
#if ODIN_INSPECTOR
        [BoxGroup("Force")]
#endif
        public float upwardsForceItem = 3f; 

#if ODIN_INSPECTOR
        [BoxGroup("Force")]
#endif
        public float upwardsForcePlayer = 3f;
#if ODIN_INSPECTOR
        [BoxGroup("Force")]
#endif
        public ForceMode upwardsForceMode = ForceMode.VelocityChange;

#if ODIN_INSPECTOR
        [BoxGroup("Torque")]
#endif
        public float creatureTorqueMult = 60;
#if ODIN_INSPECTOR
        [BoxGroup("Torque")]
#endif
        public float itemTorqueMult = 20;
#if ODIN_INSPECTOR
        [BoxGroup("Torque")]
#endif
        public ForceMode torqueForceMode = ForceMode.VelocityChange;
#if ODIN_INSPECTOR
        [BoxGroup("Options")]
        public bool destabilizeCreatures = false;
#endif
#if ODIN_INSPECTOR
        [BoxGroup("Options")]
        public float environmentDamageMult = 0.1f;
#endif
#if ODIN_INSPECTOR
        [BoxGroup("Slam")]
        public bool allowSlamOnEnd = false;
#endif
#if ODIN_INSPECTOR
        [BoxGroup("Slam")]
        public float nonRootSlamMultiplier = 0.2f;
#endif
#if ODIN_INSPECTOR
        [BoxGroup("Slam"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string slamEffectId;

#if ODIN_INSPECTOR
        [BoxGroup("Slam"), ValueDropdown(nameof(GetAllSkillID))]
#endif
        public string lithobrakeId = "Lithobrake";
        [NonSerialized]
        public SkillHoverSlam lithobrakeData;
        
#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllSkillID()
        {
            return Catalog.GetDropdownAllID(Category.Skill);
        }
#endif


        [NonSerialized]
        public EffectData slamEffectData;

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            slamEffectData = Catalog.GetData<EffectData>(slamEffectId);
            lithobrakeData = Catalog.GetData<SkillHoverSlam>(lithobrakeId);
        }
    }

}
