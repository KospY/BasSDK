using System.Collections;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
namespace ThunderRoad.Skill.Spell
{
    public class SkillTeslaWires : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Wire")]
#endif
        public float duration = 3f;
#if ODIN_INSPECTOR
        [BoxGroup("Wire"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string arcLoopEffectId = "LightningArcLoop";
        protected EffectData arcLoopEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Wire"), ValueDropdown(nameof(GetAllSkillID))]
#endif
        public string skillArcwireId = "Arcwire";

        protected SkillArcwire arcwireData;

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            arcwireData = Catalog.GetData<SkillArcwire>(skillArcwireId);
            arcLoopEffectData = Catalog.GetData<EffectData>(arcLoopEffectId);
        }




 // ProjectCore        
    }

}
