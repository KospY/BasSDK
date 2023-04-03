using System.Collections;
using UnityEngine;

#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	public class BrainModuleGrabbed : BrainData.Module
    {
        public bool freeOneHand = true;
        public float freeOneHandDelay = 2;
        public bool freeBothHand = true;
        public float freeBothHandDelay = 5;
        public float grabThrowMinVelocity = 2.0f;
    }
}