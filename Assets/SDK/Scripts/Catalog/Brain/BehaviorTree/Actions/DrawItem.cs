#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Action
{
	public class DrawItem : CanDrawItem
    {
#if ODIN_INSPECTOR
        [DisableIf("drawRightWeapon"), LabelWidth(130)]
#endif
        public bool holsterRightItem = true;
#if ODIN_INSPECTOR
        [DisableIf("drawLeftWeapon"), LabelWidth(130)]
#endif
        public bool holsterLeftItem = false;
        public bool waitForFinished = false;
    }
}
