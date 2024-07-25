using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [RequireComponent(typeof(BoxCollider))]
    [AddComponentMenu("ThunderRoad/Levels/Audio Portal")]
    public class AudioPortal : MonoBehaviour
    {
        [Header("General")]
        [ReadOnly]
        public BoxCollider boxCollider;
        public Vector2 size = Vector2.one;
        public float depthForward = 1;
        public float depthBackward = 1;

        [Header("Door Occlusion")]
        public List<DoorReference> doorReferences;
        public float doorMaxAngle = 90;

        [Serializable]
        public class DoorReference
        {
            public Transform frame;
            public Transform door;
        }

        [Header("Areas")]
        public AudioArea backwardArea;
        public AudioArea forwardArea;
 //projectcore
        
        [Serializable]
        public class AudioSourceBlend
        {
            public Area area = Area.Forward;
            public AudioSource audioSource;
            public Vector2 minMaxVolume = new Vector2(0, 1);
            public AudioLowPassFilter audioLowPassFilter;
            public AnimationCurve cutoffFrequencyCurve;
            [NonSerialized, ShowInInspector, ReadOnly]
            public bool targetReached;
            
            [Button]
            public void ResetcutoffFrequencyCurve()
            {
                cutoffFrequencyCurve = new AnimationCurve(new Keyframe(0, 100f), new Keyframe(1f, 22000f, 30000f, 30000f));
            }
        }

        public enum Area
        {
            Forward,
            Backward,
        }

 //projectcore
        private void OnDrawGizmos()
        {
            Gizmos.matrix = this.transform.localToWorldMatrix;
            Gizmos.color = new Color(0.5f, 0.5f, 0, 0.25f);
            Gizmos.DrawCube(Vector3.zero, new Vector3(size.x, size.y, 0));
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
        }
    }
}