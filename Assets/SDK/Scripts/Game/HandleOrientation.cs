using UnityEngine;
using System.Collections.Generic;
using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class HandleOrientation : MonoBehaviour
    {
        public Side side = Side.Right;
        [NonSerialized]
        public HandleDefinition handleDefinition;
        protected Mesh handMesh;

        public virtual void OnValidate()
        {
            if (!Application.isPlaying)
            {
                if (Application.isEditor)
                {
                    UnityEngine.Object[] asset = Resources.LoadAll("Hand");
                    for (int i = 0; i < asset.Length; i++)
                    {
                        if (asset[i] is Mesh) handMesh = asset[i] as Mesh;
                    }
                }

                handleDefinition = this.GetComponentInParent<HandleDefinition>();
                UpdateName();
            }
        }

        private void Awake()
        {
            handleDefinition = this.GetComponentInParent<HandleDefinition>();
            if (handleDefinition == null)
            {
                Debug.LogError("No HandleDefinition found for HandleOrientation " + this.name);
            }
        }

        public void UpdateName()
        {
            if (handleDefinition)
            {
                if (side == Side.Right)
                {
                    this.name = "OrientRight" + (handleDefinition.orientationDefaultRight == this ? "_Default" : "");
                }
                else if (side == Side.Left)
                {
                    this.name = "OrientLeft" + (handleDefinition.orientationDefaultLeft == this ? "_Default" : "");
                }
            }
        }

        protected void OnDrawGizmosSelected()
        {
            Gizmos.matrix = Matrix4x4.TRS(handleDefinition.transform.TransformPoint(this.transform.localPosition.x, this.transform.localPosition.y + handleDefinition.GetDefaultAxisLocalPosition(), this.transform.localPosition.z), this.transform.rotation, Vector3.one);
            float size = 35;

            if (side == Side.Right)
            {
                Gizmos.color = Common.HueColourValue(HueColorName.Green);
                Gizmos.DrawWireMesh(handMesh, Vector3.zero, Quaternion.Euler(180, 0, 0), new Vector3(-size, size, size));
            }
            else if (side == Side.Left)
            {
                Gizmos.color = Common.HueColourValue(HueColorName.Red);
                Gizmos.DrawWireMesh(handMesh, Vector3.zero, Quaternion.Euler(180, 0, 0), new Vector3(size, size, size));
            }

            Gizmos.color = Common.HueColourValue(HueColorName.Yellow);
            Gizmos.DrawWireSphere(new Vector3(0, 0, 0), 0.002f);

            Gizmos.matrix = Matrix4x4.TRS(handleDefinition.transform.TransformPoint(0, handleDefinition.GetDefaultAxisLocalPosition(), 0), this.transform.rotation, Vector3.one);
            Gizmos.color = Common.HueColourValue(HueColorName.Purple);
            Gizmos.DrawWireSphere(new Vector3(0, 0, 0), 0.005f);
            Gizmos.DrawWireSphere(new Vector3(0, 0, 0), 0.001f);
        }
    }
}
