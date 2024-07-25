using UnityEngine;

namespace ThunderRoad.AI.Action
{
	public class AttackThrow : AttackRanged
    {
        public SidePref preferredThrowHand = SidePref.PreferRight;

        public enum SidePref
        {
            PreferRight,
            PreferLeft,
            OnlyRight,
            OnlyLeft
        }

        public bool grabRock = false;

    }
}
