using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad.Skill.Spell;
using UnityEngine;
using Object = UnityEngine.Object;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.Skill.SpellMerge
{
    [Serializable]
    public class SpellMergeGravity : SpellMergeData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Throw"), ShowIf("allowThrow")] 
#endif
        public float throwDefaultForce = 22;
#if ODIN_INSPECTOR
        [BoxGroup("Throw"), ShowIf("allowThrow")] 
#endif
        public float throwRagdollLimbForce = 6;

#if ODIN_INSPECTOR
        [BoxGroup("Lift")] 
#endif
        public float mergingLiftRadius = 5f;
#if ODIN_INSPECTOR
        [BoxGroup("Lift")] 
#endif
        public float liftMinForce = 0.3f;
#if ODIN_INSPECTOR
        [BoxGroup("Lift")] 
#endif
        public float liftMaxForce = 0.6f;
#if ODIN_INSPECTOR
        [BoxGroup("Lift")] 
#endif
        public float liftRagdollForceMultiplier = 2f;
#if ODIN_INSPECTOR
        [BoxGroup("Lift")] 
#endif
        public float liftDrag = 1f;
#if ODIN_INSPECTOR
        [BoxGroup("Lift")] 
#endif
        public Vector3 randomTorqueRange = new Vector3(0.2f, 0.2f, 0.2f);
#if ODIN_INSPECTOR
        [BoxGroup("Lift")] 
#endif
        public float playerGravityRatio = 0.5f;

#if ODIN_INSPECTOR
        [BoxGroup("Bubble"), ValueDropdown(nameof(GetAllEffectID))] 
#endif
        public string bubbleEffectId;
        protected EffectData bubbleEffectData;
#if ODIN_INSPECTOR
        [BoxGroup("Bubble")] 
#endif
        public float bubbleDuration = 10;
#if ODIN_INSPECTOR
        [BoxGroup("Bubble")] 
#endif
        public float bubbleEffectMaxScale = 15;
#if ODIN_INSPECTOR
        [BoxGroup("Bubble")] 
#endif
        public AnimationCurve bubbleScaleCurveOverTime;
#if ODIN_INSPECTOR
        [BoxGroup("Bubble")]
#endif
        public float bubbleEndNormalizedTime = 0.85f;
#if ODIN_INSPECTOR
        [BoxGroup("Bubble")]
#endif
        public AnimationCurve bubbleEndCurve;
#if ODIN_INSPECTOR
        [BoxGroup("Bubble")] 
#endif
        public float bubbleHandSeparationMaxAngle = 45f;

#if ODIN_INSPECTOR
        [BoxGroup("Bubble")] 
#endif
        public bool enableHandControl;
        
#if ODIN_INSPECTOR
        [BoxGroup("Bubble"), ValueDropdown(nameof(GetAllStatusEffectID))] 
#endif
        public string statusId = "BubbleGravity";

#if ODIN_INSPECTOR
        [BoxGroup("Bubble"), ShowIf("enableHandControl")] 
#endif
        public float handControlForce = 0.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Bubble"), ShowIf("enableHandControl")] 
#endif
        public ForceMode handControlForceMode = ForceMode.VelocityChange;

        protected static Vector3[] randomTorqueArray;
        protected static float[] randomHeightArray;

        protected bool capturedPlayer;

        [NonSerialized]
        public static bool bubbleActive;
        [NonSerialized]
        public static bool closing;

        [NonSerialized]
        public Vector3 bubblePosition;


        public delegate void BubbleEvent(Mana mana, Vector3 position, Zone zone);

        public event BubbleEvent OnBubbleOpen;
        public event BubbleEvent OnBubbleClose;

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            if (!string.IsNullOrEmpty(throwEffectId))
                throwEffectData = Catalog.GetData<EffectData>(throwEffectId);
            if (!string.IsNullOrEmpty(bubbleEffectId))
                bubbleEffectData = Catalog.GetData<EffectData>(bubbleEffectId);
            if (randomTorqueArray == null)
            {
                randomTorqueArray = new Vector3[5];
                randomTorqueArray[0] = new Vector3(UnityEngine.Random.Range(-randomTorqueRange.x, randomTorqueRange.x), UnityEngine.Random.Range(-randomTorqueRange.y, randomTorqueRange.y), UnityEngine.Random.Range(-randomTorqueRange.z, randomTorqueRange.z));
                randomTorqueArray[1] = new Vector3(UnityEngine.Random.Range(-randomTorqueRange.x, randomTorqueRange.x), UnityEngine.Random.Range(-randomTorqueRange.y, randomTorqueRange.y), UnityEngine.Random.Range(-randomTorqueRange.z, randomTorqueRange.z));
                randomTorqueArray[2] = new Vector3(UnityEngine.Random.Range(-randomTorqueRange.x, randomTorqueRange.x), UnityEngine.Random.Range(-randomTorqueRange.y, randomTorqueRange.y), UnityEngine.Random.Range(-randomTorqueRange.z, randomTorqueRange.z));
                randomTorqueArray[3] = new Vector3(UnityEngine.Random.Range(-randomTorqueRange.x, randomTorqueRange.x), UnityEngine.Random.Range(-randomTorqueRange.y, randomTorqueRange.y), UnityEngine.Random.Range(-randomTorqueRange.z, randomTorqueRange.z));
                randomTorqueArray[4] = new Vector3(UnityEngine.Random.Range(-randomTorqueRange.x, randomTorqueRange.x), UnityEngine.Random.Range(-randomTorqueRange.y, randomTorqueRange.y), UnityEngine.Random.Range(-randomTorqueRange.z, randomTorqueRange.z));
            }
            if (randomHeightArray == null)
            {
                randomHeightArray = new float[5];
                randomHeightArray[0] = 0f;
                randomHeightArray[1] = 0.03f;
                randomHeightArray[2] = -0.04f;
                randomHeightArray[3] = 0.02f;
                randomHeightArray[4] = -0.05f;
            }
        }

    }
}
