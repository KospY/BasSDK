using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
	public class BrainModuleFollow : BrainData.Module
    {
        public enum FollowBehavior
        {
            NeverFollow,
            OnlyWhenSet,
            SearchForFollowable
        }

#if ODIN_INSPECTOR
        [BoxGroup("Follow")] 
#endif
        public FollowBehavior followBehavior = FollowBehavior.OnlyWhenSet;
#if ODIN_INSPECTOR
        [BoxGroup("Follow")] 
#endif
        public List<Brain.State> clearFollowBrainStates = new List<Brain.State>();

#if ODIN_INSPECTOR
        [BoxGroup("Followers")] 
#endif
        public bool allowFollowers = true;
#if ODIN_INSPECTOR
        [BoxGroup("Followers")] 
#endif
        public int maxFollowers = 3;

    }
}