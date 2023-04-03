using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad.Manikin
{
    [CreateAssetMenu(menuName = "Manikin/Converters/Blendshape Converter")]
    public class ManikinPropertyBlendshapeConverter : ManikinPropertyConverterBase
    {
        public string BlendShapeName;
        public float multiplier = 100f;

        public override void ApplyProperty(GameObject obj, float[] values, bool useSRPBatcher = false, Renderer renderer = null, int materialIndex = 0, object payload = null)
        {
            if (string.IsNullOrEmpty(BlendShapeName))
                return;

            SkinnedMeshRenderer smr = renderer as SkinnedMeshRenderer;

            int blendShapeIndex = smr.sharedMesh.GetBlendShapeIndex(BlendShapeName); //this might be relatively slow.
            if (blendShapeIndex >= 0 && blendShapeIndex < smr.sharedMesh.blendShapeCount)
            {
                smr.SetBlendShapeWeight(blendShapeIndex, values[0] * multiplier);                
            }
            else
            {
                //Debug.LogError("Blendshape index is out of range! " + BlendShapeName);
            }
        }
    }
}
