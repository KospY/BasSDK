using UnityEngine;
using System;
using RainyReignGames.RevealMask;
using System.Collections.Generic;

namespace ThunderRoad
{
    public class EffectReveal : Effect
    {
        public Texture maskTexture;

        public float depth = 1.2f;
        public float offsetDistance = 0.05f;

        public RevealData[] revealData;

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

        public override void Play()
        {
            if (step == Step.Start || step == Step.End)
            {
                RevealMaskProjection.Project(this.transform.position + (this.transform.forward * offsetDistance), -this.transform.forward, this.transform.up, depth, currentSize, maskTexture, currentChannelMultiplier, targetRenderers.ToArray(), revealData);
                Invoke("Despawn", 2);
            }
            playTime = Time.time;
        }

        public override void Stop()
        {
            SetIntensity(0);
        }

        public override void End(bool loopOnly = false)
        {
            Despawn();
        }
        /*
        public override void CollisionStay(Vector3 position, Quaternion rotation, float intensity)
        {
            base.CollisionStay(position, rotation, intensity);
        }
        */
        public override void SetIntensity(float value, bool loopOnly = false)
        {
            if (!loopOnly || (loopOnly && step == Step.Loop))
            {
                currentSize = Mathf.Lerp(minSize, maxSize, value);
                currentChannelMultiplier = Vector4.Lerp(minChannelMultiplier, maxChannelMultiplier, value);
            }
        }

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
