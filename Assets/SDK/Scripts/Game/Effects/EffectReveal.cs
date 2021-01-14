using UnityEngine;
using System;
using System.Collections.Generic;
#if ProjectCore
using RainyReignGames.RevealMask;
#endif
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class EffectReveal : Effect
    {
        public Texture maskTexture;

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

        public List<Renderer> targetRenderers;


        public override void Despawn()
        {
            InvokeDespawnCallback();
            if (Application.isPlaying)
            {
                Destroy(this.gameObject);
            }
            else
            {
                DestroyImmediate(this.gameObject);
            }
        }
    }
}
