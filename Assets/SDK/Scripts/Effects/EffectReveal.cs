using UnityEngine;
using System;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Effects/EffectReveal.html")]
    public class EffectReveal : Effect
    {
        public Texture maskTexture;

        public Direction applyOn = Direction.Target;

        public enum Direction
        {
            Source,
            Target,
        }

        public float depth = 1.2f;
        public float offsetDistance = 0.05f;

        public float minSize = 0.05f;
        public float maxSize = 0.1f;

        public Vector4 minChannelMultiplier = Vector4.one;
        public Vector4 maxChannelMultiplier = Vector4.one;

        [NonSerialized]
        public float playTime;

        [NonSerialized]
        public float currentSize;
        [NonSerialized]
        public Vector4 currentChannelMultiplier;

        public CollisionHandler collisionHandler;


    }
}
