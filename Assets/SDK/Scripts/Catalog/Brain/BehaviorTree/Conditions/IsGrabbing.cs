#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad.AI.Action
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