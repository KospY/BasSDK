using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

namespace ThunderRoad
{
    [VFXBinder("Custom/Player Head Position")]
    class VFXPlayerHeadPositionBinder : VFXBinderBase
    {
        [VFXPropertyBinding("UnityEngine.Vector3")]
        public ExposedProperty targetProperty = "Head Position";

        public override bool IsValid(VisualEffect component)
        {
            if (!Application.isPlaying) return component.HasVector3(targetProperty);
            return false;
        }

        public override void UpdateBinding(VisualEffect component)
        {
            if (!Application.isPlaying) return;
        }

        public override string ToString()
        {
            return $"Player head position : '{targetProperty}' -> Player Head";
        }
    }
}