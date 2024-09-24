using ThunderRoad.Skill.SpellPower;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad.Skill.Spell
{
    public class SkillEmergencyExit : SkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Status"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string statusId;

        protected StatusData statusData;
#if ODIN_INSPECTOR
        [BoxGroup("Status")]
#endif
        public float radius;
#if ODIN_INSPECTOR
        [BoxGroup("Status")]
#endif
        public float duration;
#if ODIN_INSPECTOR
        [BoxGroup("Effect"), ValueDropdown("GetAllEffectID")]
#endif
        public string effectId;

        protected EffectData effectData;
        
        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            statusData = Catalog.GetData<StatusData>(statusId);
            effectData = Catalog.GetData<EffectData>(effectId);
        }

    }
}
