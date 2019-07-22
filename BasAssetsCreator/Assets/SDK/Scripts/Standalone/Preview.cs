using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if FULLGAME
using Sirenix.OdinInspector;
#endif

namespace BS
{
    public class Preview : MonoBehaviour
    {
        private float size = 1;
        protected virtual void OnDrawGizmosSelected()
        {
            //Deletes Preview Script if the object is no longer assigned as preview
            if (!this.GetComponentInParent<ItemDefinition>() || this.GetComponentInParent<ItemDefinition>().preview != this.transform)
            {
                DestroyImmediate(this);
            }
            if (this)
            {
                //Gets initial preview size
                if (this.transform.lossyScale == Vector3.one && transform.GetComponentInParent<ItemDefinition>())
                {
                    size = transform.GetComponentInParent<ItemDefinition>().previewSize;
                }
                //Updates previewSize in item definition when object is scaled, and locks actual scale to 1.
                else if (transform.GetComponentInParent<ItemDefinition>())
                {
                    size = transform.lossyScale.x;
                    gameObject.GetComponentInParent<ItemDefinition>().previewSize = size;
                    transform.localScale = Vector3.one;
                }
                //Draws Preview Gizmos
                Gizmos.color = Common.HueColourValue(HueColorNames.Green);
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawWireCube(Vector3.zero, new Vector3(size / this.transform.lossyScale.x, size / this.transform.lossyScale.y, 0));
                Common.DrawGizmoArrow(Vector3.zero, (Vector3.back * 0.3f) / this.transform.lossyScale.z, Color.blue, 0.15f / this.transform.lossyScale.z);
                Common.DrawGizmoArrow(Vector3.zero, (Vector3.up * 0.3f) / this.transform.lossyScale.y, Common.HueColourValue(HueColorNames.Green), 0.15f / this.transform.lossyScale.y);
                
            }
        }
    }
}