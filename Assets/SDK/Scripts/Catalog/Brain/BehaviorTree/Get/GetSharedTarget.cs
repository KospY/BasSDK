using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Get
{
	public class GetSharedTarget : ActionNode
    {
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public bool checkRoomAlert = true;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public bool checkCloseFriends = true;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public float closeFriendMaxDistance = 10;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public bool checkShortestPath = false;

#if ODIN_INSPECTOR
        [BoxGroup("Outputs"), HorizontalGroup("Outputs/Horiz", MinWidth = 300), LabelWidth(200)] 
#endif
        public bool outputCreatureAsCurrentTarget = true;
#if ODIN_INSPECTOR
        [BoxGroup("Outputs"), HorizontalGroup("Outputs/Horiz"), LabelWidth(200), DisableIf("outputCreatureAsCurrentTarget")] 
#endif
        public string outputCreatureVariableName = "";
#if ODIN_INSPECTOR
        [BoxGroup("Outputs"), HorizontalGroup("Outputs/Horiz2"), LabelWidth(200)] 
#endif
        public string outputTransformVariableName = "";

    }
}
