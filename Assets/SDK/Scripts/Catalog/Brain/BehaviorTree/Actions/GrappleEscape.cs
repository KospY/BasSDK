using UnityEngine;
using System.Collections;

#if ODIN_INSPECTOR
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Action
{
	public class GrappleEscape : ActionNode
    {
        public float initialSuccessChance = 0.25f;
        public float attemptsMultiplier = 0.1f;
        public Vector2 timeBetweenAttempts = new Vector2(1f, 2f);
        public float twoGrabPenalty = 0.15f;
        public float maximumSuccessChance = 0.5f;
        public bool resetAttemptsOnDamage = true;
    }
}
