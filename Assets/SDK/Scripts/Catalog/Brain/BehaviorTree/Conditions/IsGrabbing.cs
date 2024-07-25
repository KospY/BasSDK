#if ODIN_INSPECTOR
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Condition
{
	public partial class IsGrabbing : ConditionNode
    {
        public bool weaponOnly = false;
        public Side hand = Side.Right;
        public ItemModuleAI.WeaponClass weaponClass;
        public Filter weaponClassFilter = Filter.Equal;

        public enum Filter
        {
            Equal,
            NotEqual,
        }

    }
}