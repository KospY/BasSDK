using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public abstract class GolemAbility : ScriptableObject
    {
#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public GolemAbilityType type = GolemAbilityType.Ranged;
#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public float weight = 1f;
#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public RampageType rampageType;

#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public Golem.Tier abilityTier = Golem.Tier.Any;

#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public bool stunOnExit;
#if ODIN_INSPECTOR
        [BoxGroup("General"), ShowIf(nameof(stunOnExit))]
#endif
        public float stunDuration = 0;

    }

    public enum GolemAbilityType
    {
        Climb, // Anything that triggers when something is climbing on the golem
        Melee, // Anything that triggers if the player is nearby the golem
        Ranged // Anything that triggers when the player is too far, and typically uses some kind of targeting
    }

    public enum RampageType
    {
        None,
        Melee,
        Ranged,
    }
}
