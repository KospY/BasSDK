using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Mirror")]
    public class Mirror : MonoBehaviour
    {
        // A simple hook for armour edit events if required.
        public delegate void OnArmourEditModeChanged(bool state);
        public delegate void OnRenderStateChanged(bool state);

        public static Mirror local;

        public bool useOcclusionCulling = false;
        public bool allowArmourEditing = false;
        [Space]
        public ReflectionDirection reflectionDirection = ReflectionDirection.Up;
        [Range(0, 1)]
        public float quality = 1;
        [Range(0, 1)]
        public float Intensity = 1f;
        public bool reflectionWithoutGI = true;
        public Color backgroundColor = Color.black;
        public LayerMask cullingMask = ~0;
        public MeshRenderer mirrorMesh;
        public MeshRenderer[] meshToHide;
        private Vector3 reflectionLocalDirection;
        private Vector3 reflectionWorldDirection;

        private bool hasInvokedEvent; // Used to prevent multiple events from being invoked.
        public event OnArmourEditModeChanged OnArmourEditModeChangedEvent;
        public event OnRenderStateChanged OnRenderStateChangedEvent;

        [NonSerialized]
        public bool isRendering;

        internal protected bool active = true;

        public bool stopDuringCreaturePartUpdate = true;
        protected bool creaturePartUpdating;

        public enum Side
        {
            Left, //have left first to match Unity's convention if using them as ints
            Right
        }

        public enum ReflectionDirection
        {
            Up,
            Down,
            Forward,
            Back,
            Left,
            Right,
        }

        void OnValidate()
        {
            Refresh();
        }

        [Button]
        public void SetActive(bool active)
        {
            this.active = active;
        }

        [ContextMenu("Refresh")]
        public void Refresh()
        {
            if (reflectionDirection == ReflectionDirection.Forward)
                reflectionLocalDirection = Vector3.forward;
            else if (reflectionDirection == ReflectionDirection.Up)
                reflectionLocalDirection = Vector3.up;
            else if (reflectionDirection == ReflectionDirection.Back)
                reflectionLocalDirection = Vector3.back;
            else if (reflectionDirection == ReflectionDirection.Down)
                reflectionLocalDirection = Vector3.down;
            else if (reflectionDirection == ReflectionDirection.Left)
                reflectionLocalDirection = Vector3.left;
            else if (reflectionDirection == ReflectionDirection.Right)
                reflectionLocalDirection = Vector3.right;
            reflectionWorldDirection = this.transform.TransformDirection(reflectionLocalDirection);
        }

        /// <summary>
        /// Toggle the armour edit mode.
        /// This is used in the Lever event for the bench.
        /// </summary>
        public void SetEditMode(bool state)
        {
        }

        public static void DrawGizmoArrow(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Gizmos.color = color;
            Gizmos.DrawRay(pos, direction);
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        }
    }
}
