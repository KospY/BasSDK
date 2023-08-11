using System;
using System.Collections;
using UnityEngine;
using ThunderRoad.AI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	public class BehaviorTreeData : CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public Type type = Type.Misc;

#if ODIN_INSPECTOR
        [BoxGroup("General"), ShowInInspector, ListDrawerSettings(Expanded = true)] 
#endif
        public Node rootNode = new Node();

        [NonSerialized]
        public Blackboard blackboard;

        [NonSerialized]
        public Creature creature;

        protected string lastDebugString = "";

        public enum Type
        {
            MasterTree,
            SubTree,
            WIP,
            Misc,
        }
    }
}