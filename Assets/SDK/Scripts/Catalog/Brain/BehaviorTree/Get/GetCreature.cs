using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad.AI.Get
{
	public class GetCreature : ActionNode
    {
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public Target target = Target.ClosestEnemy;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public bool checkFov = true;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public bool checkSight = true;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public bool checkShortestPath = false;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public float rememberDuration = 5;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public float focusDuration = 5;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public float maxDistance = 0;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public bool debugLines;

#if ODIN_INSPECTOR
        [BoxGroup("Outputs"), HorizontalGroup("Outputs/Horiz", MinWidth = 300), LabelWidth(200)] 
#endif
        public bool outputCreatureAsCurrentTarget = true;
#if ODIN_INSPECTOR
        [BoxGroup("Outputs"), HorizontalGroup("Outputs/Horiz"), LabelWidth(200), DisableIf("outputCreatureAsCurrentTarget")] 
#endif
        public string outputCreatureVariableName = "";
#if ODIN_INSPECTOR
        [BoxGroup("Outputs"), HorizontalGroup("Outputs/Horiz2", MinWidth = 300), LabelWidth(120)] 
#endif
        public OutputTransform outputTransform = OutputTransform.SightedPart;
#if ODIN_INSPECTOR
        [BoxGroup("Outputs"), HorizontalGroup("Outputs/Horiz2"), LabelWidth(200), DisableIf("outputTransform", optionalValue: OutputTransform.None)] 
#endif
        public string outputTransformVariableName = "";

        public enum Target
        {
            ClosestEnemy,
            ClosestFriend,
            ClosestFollowable,
            ClosestThreatened,
            ClosestPlayer,
            ClosestBody,
            Self,
        }

        public enum OutputTransform
        {
            None,
            CreatureRoot,
            SightedPart,
        }

        public enum Sight
        {
            Nothing,
            Creature,
            Target,
        }

    }
}
