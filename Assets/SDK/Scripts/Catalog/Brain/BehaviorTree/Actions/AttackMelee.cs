using UnityEngine;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Action
{
	public class AttackMelee : ActionNode
    {
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), HorizontalGroup("Inputs/Horiz"), LabelWidth(100)] 
#endif
        public Target target = Target.CurrentTarget;
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), HorizontalGroup("Inputs/Horiz"), EnableIf("target", optionalValue: Target.InputTransform), HideIf("target", optionalValue: Target.InputCreature), LabelWidth(200)] 
#endif
        public string inputTransformVariableName = "";
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), HorizontalGroup("Inputs/Horiz"), ShowIf("target", optionalValue: Target.InputCreature), LabelWidth(200)] 
#endif
        public string inputCreatureVariableName = "";

#if ODIN_INSPECTOR
        [HorizontalGroup("Horiz"), BoxGroup("Horiz/Melee"), LabelWidth(160)] 
#endif
        public bool useMeleeDelay = false;
#if ODIN_INSPECTOR
        [BoxGroup("Horiz/Melee"), DisableIf("target", optionalValue: Target.None), LabelWidth(160)] 
#endif
        public bool defensePreferenceChance = true;
#if ODIN_INSPECTOR
        [BoxGroup("Horiz/Melee"), DisableIf("target", optionalValue: Target.None), LabelWidth(160)] 
#endif
        public bool checkCurrentTargetSight = true;
#if ODIN_INSPECTOR
        [BoxGroup("Horiz/Melee"), DisableIf("target", optionalValue: Target.None), LabelWidth(160)] 
#endif
        public bool checkCloseAllies = true;
#if ODIN_INSPECTOR
        [BoxGroup("Horiz/Melee"), DisableIf("target", optionalValue: Target.None), LabelWidth(160)] 
#endif
        public bool checkNavMesh = true;

#if ODIN_INSPECTOR
        [BoxGroup("Horiz/Range"), EnableIf("target", optionalValue: Target.None), LabelWidth(140)] 
#endif
        public float forceMeleeRange = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Horiz/Range"), EnableIf("target", optionalValue: Target.None), LabelWidth(140)] 
#endif
        public float forceMeleeRangeMoved = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Horiz/Range"), EnableIf("target", optionalValue: Target.None), LabelWidth(140)] 
#endif
        public float forceWeaponReach = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Horiz/Range"), EnableIf("target", optionalValue: Target.None), LabelWidth(140)] 
#endif
        public float forceMaxRangeDelta = 0;

#if ODIN_INSPECTOR
        [BoxGroup("Horiz/Misc"), LabelWidth(100)] 
#endif
        public bool stopOnReset = false;

#if ODIN_INSPECTOR
        [BoxGroup("Horiz/Debug"), EnableIf("target", optionalValue: Target.None)] 
#endif
        public int pickClipSequentialMaxIndex = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Horiz/Debug"), EnableIf("target", optionalValue: Target.None)] 
#endif
        public bool updateClipRange = false;

        public enum Target
        {
            CurrentTarget,
            InputCreature,
            InputTransform,
            None,
        }

    }
}

