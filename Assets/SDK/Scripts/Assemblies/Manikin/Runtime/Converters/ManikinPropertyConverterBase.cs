using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad.Manikin
{
    public abstract class ManikinPropertyConverterBase : ScriptableObject
    {
        public abstract void ApplyProperty(GameObject obj, float[] values, bool useSRPBatcher = false, Renderer renderer = null, int materialIndex = 0, object payload = null);
    }
}