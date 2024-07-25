
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using System.Collections;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Action
{
    public class SetStanceFromData : ActionNode
    {
        public bool getStanceFromWeapons = true;
        public string specifiedID;
        public bool onlyOnce = false;

#if ODIN_INSPECTOR
#endif

    }
}
