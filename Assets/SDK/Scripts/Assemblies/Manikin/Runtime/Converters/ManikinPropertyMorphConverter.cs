using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad.Manikin
{
    [CreateAssetMenu(menuName = "Manikin/Converters/Morph Converter")]
    public class ManikinPropertyMorphConverter : ManikinPropertyConverterBase
    {
        public string morphName;

        [SerializeField, HideInInspector] 
        private int morphHash;

        private void OnValidate()
        {
            morphHash = Animator.StringToHash(morphName);
        }

        public override void ApplyProperty(GameObject obj, float[] values, bool useSRPBatcher = false, Renderer renderer = null, int materialIndex = 0, object payload = null)
        {
            if(obj.TryGetComponent(out ManikinPartMorphs partMorphs))
            {
                if(!partMorphs.UpdateMorphValue(morphHash, values[0]))
                {
                    Debug.LogWarning(morphName + " not found on " + obj.name);
                }
            }
        }
    }
}
