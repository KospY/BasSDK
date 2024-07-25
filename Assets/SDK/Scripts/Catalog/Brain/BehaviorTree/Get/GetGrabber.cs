using UnityEngine;
using System;
using ThunderRoad.Skill.SpellPower;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Get
{
	public class GetGrabber : ActionNode
    {
        [Flags]
        public enum HandleOptions
        {
            SpecificHandle = 0,
            CarryHandle = 1,
            MuffleHandle = 2,
            ChokeHandle = 4
        }

        public enum FromDirection
        {
            Either,
            FrontOnly,
            BackOnly
        }

        public enum Faction
        {
            OnlyEnemy,
            Either,
            OnlyFriend,
            OnlyPlayer,
        }

        public Faction factionCondition = Faction.OnlyEnemy;
#if ODIN_INSPECTOR
        [BoxGroup("Options"), HorizontalGroup("Options/Horiz"), LabelWidth(200)] 
#endif
        public bool anyHandle = true;
#if ODIN_INSPECTOR
        [BoxGroup("Options"), HorizontalGroup("Options/Horiz2"), LabelWidth(200), DisableIf("anyHandle", optionalValue: true)] 
#endif
        public HandleOptions handleSpecifiers = HandleOptions.SpecificHandle;
#if ODIN_INSPECTOR
        [BoxGroup("Options"), HorizontalGroup("Options/Horiz3"), LabelWidth(200), DisableIf("anyHandle", optionalValue: true), EnableIf("handleSpecifiers", optionalValue: HandleOptions.SpecificHandle)] 
#endif
        public string handleName = "None";
#if ODIN_INSPECTOR
        [BoxGroup("Options"), HorizontalGroup("Options/Horiz4"), LabelWidth(200)] 
#endif
        public FromDirection fromDirection = FromDirection.Either;
        public float rememberDuration = 20f;
#if ODIN_INSPECTOR
        [BoxGroup("Outputs"), HorizontalGroup("Outputs/Horiz", MinWidth = 300), LabelWidth(200)] 
#endif
        public bool outputCreatureAsCurrentTarget = true;
#if ODIN_INSPECTOR
        [BoxGroup("Outputs"), HorizontalGroup("Outputs/Horiz"), LabelWidth(200), DisableIf("outputCreatureAsCurrentTarget")] 
#endif
        public string outputCreatureVariableName = "";

    }
}