#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
namespace ThunderRoad.Skill.Spell
{
    public class SkillFireSapping : SkillSapStatusApplier
    {
#if ODIN_INSPECTOR        
        [BoxGroup("Status Effect")]
#endif        
        public float heatTransferPerBolt = 10f;
    }
}
