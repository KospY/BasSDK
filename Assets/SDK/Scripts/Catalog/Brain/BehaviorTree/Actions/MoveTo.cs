using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad.AI.Action
{
	public class MoveTo : ActionNode
    {
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), HorizontalGroup("Inputs/Horiz"), LabelWidth(100)] 
#endif
        public MoveTarget moveTarget = MoveTarget.CurrentTarget;
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), DisableIf("moveTarget", optionalValue: MoveTarget.CurrentTarget), HorizontalGroup("Inputs/Horiz"), LabelWidth(200)] 
#endif
        public string inputMoveTargetVariableName = "";

        public enum MoveTarget
        {
            CurrentTarget,
            InputTransform,
            InputCreature,
            InputPosition,
        }

#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), HorizontalGroup("Inputs/Horiz2"), LabelWidth(100)] 
#endif
        public TurnTarget turnTarget = TurnTarget.CurrentTarget;
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), HorizontalGroup("Inputs/Horiz2"), EnableIf("@this.turnTarget == TurnTarget.InputTransform || this.turnTarget == TurnTarget.InputCreature"), LabelWidth(200)] 
#endif
        public string inputTurnTargetVariableName = "";

#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), HorizontalGroup("Inputs/Horiz3"), LabelWidth(270)] 
#endif
        public bool forceTurnToUseMoveDirectionAtDistance = true;
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), HorizontalGroup("Inputs/Horiz3"), EnableIf("forceTurnToUseMoveDirectionAtDistance"), LabelWidth(200)] 
#endif
        public float turnMoveDirectionDistance = 10;
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), HorizontalGroup("Inputs/Horiz3"), EnableIf("forceTurnToUseMoveDirectionAtDistance"), LabelWidth(200)] 
#endif
        public float turnMoveDirectionSpeed = 1;

        public enum TurnTarget
        {
            CurrentTarget,
            CurrentTargetViewDir,
            InputTransform,
            InputCreature,
            MoveDirection
        }

#if ODIN_INSPECTOR
        [BoxGroup("Approach"), HorizontalGroup("Approach/Horiz"), LabelWidth(120), LabelText("Move speed ratio")] 
#endif
        [Range(0f, 1f)]
        public float approachMoveSpeedRatio = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Approach"), HorizontalGroup("Approach/Horiz"), LabelWidth(120), LabelText("Turn speed ratio")] 
#endif
        [Range(0f, 1f)]
        public float approachTurnSpeedRatio = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Approach"), HorizontalGroup("Approach/Horiz"), LabelWidth(120), LabelText("Run speed ratio")] 
#endif
        [Range(0f, 1f)]
        public float approachRunSpeedRatio = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Approach"), HorizontalGroup("Approach/Horiz"), LabelWidth(150)] 
#endif
        public bool moveCloserIfNoSight;

#if ODIN_INSPECTOR
        [BoxGroup("Strafe"), DisableIf("moveTarget", optionalValue: MoveTarget.InputTransform), HideLabel] 
#endif
        public bool strafeAroundTarget = false;
#if ODIN_INSPECTOR
        [BoxGroup("Strafe"), HorizontalGroup("Strafe/Horiz2"), EnableIf("strafeAroundTarget"), LabelWidth(120), LabelText("Move speed ratio")] 
#endif
        public float strafeMoveSpeedRatio = 0.75f;
#if ODIN_INSPECTOR
        [BoxGroup("Strafe"), HorizontalGroup("Strafe/Horiz2"), EnableIf("strafeAroundTarget"), LabelWidth(120), LabelText("Turn speed ratio")] 
#endif
        public float strafeTurnSpeedRatio = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Strafe"), HorizontalGroup("Strafe/Horiz2"), EnableIf("strafeAroundTarget"), LabelWidth(120), LabelText("Run speed ratio")] 
#endif
        public float strafeRunSpeedRatio = 0;

#if ODIN_INSPECTOR
        [BoxGroup("Strafe"), HorizontalGroup("Strafe/Horiz"), EnableIf("strafeAroundTarget"), LabelWidth(130)] 
#endif
        public bool checkMaxAllies = false;
#if ODIN_INSPECTOR
        [BoxGroup("Strafe"), HorizontalGroup("Strafe/Horiz"), EnableIf("checkMaxAllies"), LabelWidth(150)] 
#endif
        public float safeRangeOffset = 0.25f;
#if ODIN_INSPECTOR
        [BoxGroup("Strafe"), HorizontalGroup("Strafe/Horiz"), EnableIf("checkMaxAllies"), LabelWidth(150)] 
#endif
        public float safeRangeThickness = 2f;

#if ODIN_INSPECTOR
        [BoxGroup("Path"), HorizontalGroup("Path/Horiz"), LabelWidth(150)] 
#endif
        public bool reportUnreachable = false;
#if ODIN_INSPECTOR
        [BoxGroup("Path"), HorizontalGroup("Path/Horiz"), EnableIf("strafeAroundTarget"), LabelWidth(150)] 
#endif
        public bool useModuleStrafeDelay = false;
#if ODIN_INSPECTOR
        [BoxGroup("Path"), HorizontalGroup("Path/Horiz"), DisableIf("useModuleStrafeDelay"), LabelWidth(150)] 
#endif
        public float repathMinDelay = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Path"), HorizontalGroup("Path/Horiz"), DisableIf("useModuleStrafeDelay"), LabelWidth(150)] 
#endif
        public float repathMaxDelay = 0;

#if ODIN_INSPECTOR
        [BoxGroup("Radius"), HorizontalGroup("Radius/Horiz", MinWidth = 250), LabelWidth(100)] 
#endif
        public AutoRadius autoRadius = AutoRadius.None;
#if ODIN_INSPECTOR
        [BoxGroup("Radius"), EnableIf("autoRadius", optionalValue: AutoRadius.None), HorizontalGroup("Radius/Horiz", MinWidth = 300), LabelWidth(150)] 
#endif
        public float targetMinRadius = 0f;
#if ODIN_INSPECTOR
        [BoxGroup("Radius"), EnableIf("autoRadius", optionalValue: AutoRadius.None), HorizontalGroup("Radius/Horiz", MinWidth = 300), LabelWidth(150)] 
#endif
        public float targetMaxRadius = 0.1f;


        public enum AutoRadius
        {
            None,
            MeleeRange,
            TargetAgentRadius,
        }

#if ODIN_INSPECTOR
        [BoxGroup("Radius"), HorizontalGroup("Radius/Horiz2", MinWidth = 250), LabelWidth(100)] 
#endif
        public AngleMode angleMode = AngleMode.None;
#if ODIN_INSPECTOR
        [BoxGroup("Radius"), HorizontalGroup("Radius/Horiz2", MinWidth = 300), LabelWidth(200)] 
#endif
        public bool changeAngleUntilUnobstructed;
#if ODIN_INSPECTOR
        [BoxGroup("Radius"), DisableIf("angleMode", optionalValue: AngleMode.None), HorizontalGroup("Radius/Horiz2", MinWidth = 300), LabelWidth(150)] 
#endif
        public float targetRadiusMinAngle = 0f;
#if ODIN_INSPECTOR
        [BoxGroup("Radius"), DisableIf("angleMode", optionalValue: AngleMode.None), HorizontalGroup("Radius/Horiz2", MinWidth = 300), LabelWidth(150)] 
#endif
        public float targetRadiusMaxAngle = 0f;

        public enum AngleMode
        {
            None,
            Randomize,
            CheckAllies,
        }

    }
}