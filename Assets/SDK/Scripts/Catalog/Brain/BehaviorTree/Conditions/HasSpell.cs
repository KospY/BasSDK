#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad.AI.Action
{
	public partial class HasSpell : ConditionNode
    {
        public string spellID;
        public float spellMinDistance = 8;
        public float spellMaxDistance = 30;

    }
}