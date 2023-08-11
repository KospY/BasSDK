﻿using System;
using UnityEngine;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Effect")]
    public class Effect : ThunderBehaviour
    {
        public DespawnCallback despawnCallback;
        public delegate void DespawnCallback(Effect effect);

        public bool isPooled;

        public bool isOutOfPool;

        [NonSerialized]
        public EffectModule module;


        public Step step = Step.Start;
        public enum Step
        {
            Start,
            Loop,
            End,
            Custom,
        }

        public int stepCustomHashId;

        public virtual void Play()
        {

        }

        public virtual void Stop()
        {

        }

        public virtual void End(bool loopOnly = false)
        {

        }
        
        public float effectIntensity;
        public virtual void SetIntensity(float value, bool loopOnly = false)
        {
            effectIntensity = value;
        }
        
        public float effectSpeed;
        public virtual void SetSpeed(float value, bool loopOnly = false)
        {

        }

        public virtual void SetHaptic(HapticDevice hapticDevice, GameData.HapticClip hapticClipFallBack)
        {

        }

        public virtual void SetMainGradient(Gradient gradient)
        {

        }

        public virtual void SetSecondaryGradient(Gradient gradient)
        {

        }

        public virtual void SetSource(Transform transform)
        {

        }

        public virtual void SetTarget(Transform transform)
        {

        }
        
        public float effectSize;
        public virtual void SetSize(float value)
        {
            effectSize = value;
        }

        public virtual void SetMesh(Mesh mesh)
        {

        }

        public virtual void SetRenderer(Renderer renderer, bool secondary)
        {

        }

        public virtual void SetCollider(Collider collider)
        {

        }

        public virtual void SetNoise(bool noise)
        {

        }

        public virtual void CollisionStay(Vector3 position, Quaternion rotation, float speed)
        {
            this.transform.SetPositionAndRotation(position, rotation);
            SetSpeed(speed, true);
        }

        public virtual void CollisionStay(Vector3 position, Quaternion rotation, float speed, float intensity)
        {
            this.transform.SetPositionAndRotation(position, rotation);
            SetSpeed(speed, true);
            SetIntensity(intensity, true);
        }

        public virtual void CollisionStay(float speed)
        {
            SetSpeed(speed, true);
        }

        public virtual void CollisionStay(float speed, float intensity)
        {
            SetSpeed(speed, true);
            SetIntensity(intensity, true);
        }

        public virtual void Despawn()
        {

        }

        protected void InvokeDespawnCallback()
        {
            despawnCallback?.Invoke(this);
            despawnCallback = null;
        }
    }
}
