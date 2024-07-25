#if ODIN_INSPECTOR
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Condition
{
	public partial class HasSpell : ConditionNode
    {
        public string spellID;
        public float spellMinDistance = 8;
        public float spellMaxDistance = 30;

    }
}