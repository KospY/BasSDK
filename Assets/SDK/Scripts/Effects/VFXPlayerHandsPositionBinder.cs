using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

namespace ThunderRoad
{
    [VFXBinder("Custom/Player Hands Position")]
    class VFXPlayerHandsPositionBinder : VFXBinderBase
    {
        [VFXPropertyBinding("UnityEngine.Vector3")]
        public ExposedProperty targetProperty = "Left Hand Position";

        public Side handSide = Side.Left;

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
            return $"Player hand postion : '{targetProperty}' -> {handSide}";
        }
    }
}