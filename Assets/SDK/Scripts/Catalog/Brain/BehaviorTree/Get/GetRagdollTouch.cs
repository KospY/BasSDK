using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Get
{
    public class GetRagdollTouch : ActionNode
    {
        const string SkulkerSkillId = "Skulker";
        public enum TouchDamage
        {
            Either,
            NoDamage,
            OnlyIfDamage
        }

        public enum TouchSource
        {
            AnyCreature,
            OnlyEnemy,
            OnlyFriend,
            OnlyPlayer,
        }

        public enum TouchType
        {
            AnyTouch,
            OnlyPart,
            OnlyItem
        }


        public enum BodyState
        {
            Any,
            StandingAlive,
            DeadOrDestabilized,
            DeadOnly,
        }

        public TouchDamage damageCondition = TouchDamage.Either;
        public TouchSource factionCondition = TouchSource.OnlyEnemy;
        public TouchType typeCondition = TouchType.AnyTouch;
        public BodyState bodyStateCondition = BodyState.StandingAlive;
        public float rememberDuration = 20f;
#if ODIN_INSPECTOR
        [BoxGroup("Outputs"), HorizontalGroup("Outputs/Horiz", MinWidth = 300), LabelWidth(200)] 
#endif
        public bool outputCreatureAsCurrentTarget = true;
#if ODIN_INSPECTOR
        [BoxGroup("Outputs"), HorizontalGroup("Outputs/Horiz"), LabelWidth(200), DisableIf("outputCreatureAsCurrentTarget")] 
#endif
        public string outputCreatureVariableName = "";
#if ODIN_INSPECTOR
        [BoxGroup("Outputs"), HorizontalGroup("Outputs/Horiz"), LabelWidth(200)] 
#endif
        public string outputPartTransformVariableName = "";

    }
}