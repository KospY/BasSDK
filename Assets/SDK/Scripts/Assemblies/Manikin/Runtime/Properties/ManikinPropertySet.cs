using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad.Manikin
{
    [CreateAssetMenu(menuName = "Manikin/Property Set")]
    public class ManikinPropertySet : ScriptableObject
    {
        public List<ManikinPropertyConverterBase> converters;

        /*private void OnEnable()
        {
            foreach(ManikinPropertyConverterBase converter in converters)
            {
                converter.InitializePropertyID();
            }
        }*/

        public void ApplyProperties(GameObject obj, float[] values, bool useSRPBatcher, Renderer renderer = null, int materialIndex = 0, object payload = null)
        {
            if (converters == null) return;
            int convertersCount = converters.Count;
            for (var i = 0; i < convertersCount; i++)
            {
                ManikinPropertyConverterBase converter = converters[i];
                converter.ApplyProperty(obj, values, useSRPBatcher, renderer, materialIndex, payload);
            }
        }
    }
}