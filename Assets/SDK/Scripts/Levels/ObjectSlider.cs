using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using UnityEngine.AI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class ObjectSlider : MonoBehaviour
    {
        [Header("General")]
        public float maxheight = 2.7f;
        [Range(0, 1)]
        public float positionOnStart = 0;
        public float reachOffset = 0.01f;
        public float navMeshObstacleMaxHeight = 2f;

        [Header("Drive")]
        public bool driveEnabled = true;
        [Range(0, 1)]
        public float drivePositionOnStart = 0;

        [Header("Dynamic drive")]
        public bool dynamicDriveEnabled = true;
        public float dynamicDriveSpeed = 0.05f;
        public float dynamicDriveCurveDuration = 3f;
        public AnimationCurve dynamicDriveCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

        [Header("Drive (Forward / Reverse)")]
        public Vector2 driveSpring = new Vector2(5000, 5000);
        public Vector2 driveDamper = new Vector2(1000, 1000);
        public Vector2 driveMaxForce = new Vector2(5000, 5000);

        [Header("Audio")]
        public float audioReachMinVelocity = 0.1f;
        public float audioReachMaxVelocity = 1f;
        public FxModule effectAudioReachStart;
        public FxModule effectAudioReachEnd;

        [Header("Event")]
        public UnityEvent targetReachEvent = new UnityEvent();
        public enum State
        {
            Close,
            Open,
            InBetween,
        };


        [Button]
        public void Open()
        {
            SetDrive(1);
        }

        [Button]
        public void Close()
        {
            SetDrive(0);
        }

        [Button]
        public void HoldDrive()
        {
        }

        [Button]
        public void StopDrive()
        {
        }

        [Button]
        public void SetPosition(float linearPosition)
        {
        }

        [Button]
        public void SetDrive(float linearPosition)
        {
        }


        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(this.transform.position, this.transform.up * maxheight);
        }
    }
}