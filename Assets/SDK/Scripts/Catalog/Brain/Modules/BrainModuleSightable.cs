using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.AI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	public class ShootPosition
    {
        public Vector3 position;
        public bool valid;
        public Creature creature;
    }

    public class BrainModuleSightable : BrainData.Module
    {
#if ODIN_INSPECTOR
        [HorizontalGroup("Split")]
        [BoxGroup("Split/Sight"), LabelWidth(200)] 
#endif
        public float sightThickness = 0.1f;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Sight"), LabelWidth(200)] 
#endif
        public float sightDetectionMaxDistance = 20;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Sight"), LabelWidth(200)] 
#endif
        public float sightMaxDistance = 30;
        // Defines this creature's ability to see in the dark. Objects with a brightness below the minimum brightness aren't visible to the creature, objects above are visible.
        // Max brightness is used for determining how alert a creature may be to the object. Objects above max brightness contribute fully to creature alertedness increases
#if ODIN_INSPECTOR
        [BoxGroup("Split/Sight"), LabelWidth(200), MinMaxSlider(0f, 1f, showFields: true)] 
#endif
        public Vector2 minMaxBrightness = new Vector2(0f, 0.25f);
#if ODIN_INSPECTOR
        [BoxGroup("Split/Sight"), LabelWidth(200)] 
#endif
        public float shootPointSightThickness = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Sight"), LabelWidth(200)] 
#endif
        public List<SightableRagdollPart> sightableRagdollParts;

        public enum SightableRagdollPart
        {
            TargetPart,
            HeadPart,
            RootPart,
            HandRightPart,
            HandLeftPart,
            FootRightPart,
            FootLeftPart
        }

#if ODIN_INSPECTOR
        [BoxGroup("Split/Shoot Position"), LabelWidth(200)] 
#endif
        public int shootPositionCircleSteps = 15;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Shoot Position"), LabelWidth(200)] 
#endif
        public int shootPositionDepthSteps = 4;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Shoot Position"), LabelWidth(200)] 
#endif
        public int shootPositionMinRadius = 8;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Shoot Position"), LabelWidth(200)] 
#endif
        public float navSamplePositionMaxDistance = 50;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Shoot Position"), LabelWidth(200)] 
#endif
        public float lastShootUpdateDelay = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Shoot Position"), LabelWidth(200)] 
#endif
        public float lastShootUpdatePositionMaxDistance = 0.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Shoot Position"), LabelWidth(200)] 
#endif
        public bool debug;

#if ODIN_INSPECTOR
        [HorizontalGroup("Split2")]
        [BoxGroup("Split2/Instance"), ShowInInspector, ReadOnly] 
#endif
        [NonSerialized]
        public ShootPoint[] shootpoints = new ShootPoint[50];
#if ODIN_INSPECTOR
        [BoxGroup("Split2/Instance"), ShowInInspector, ReadOnly] 
#endif
        [NonSerialized]
        public int shootpointCount;
        protected float lastUpdateShootPointsTime;
        protected Vector3 lastUpdateShootPointsLocation;

#if ODIN_INSPECTOR
        [BoxGroup("Split2/Instance"), ShowInInspector, ReadOnly, TableList()] 
#endif
        [NonSerialized]
        public ShootPosition[] shootPositions;
        protected float lastUpdateShootPositionsTime;
        protected Vector3 lastUpdateShootPositionsLocation;

    }
}