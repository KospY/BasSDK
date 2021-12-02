using System;
using UnityEngine;

namespace ThunderRoad
{
    public class Effect : MonoBehaviour
    {
        public DespawnCallback despawnCallback;
        public delegate void DespawnCallback(Effect effect);

        public bool isPooled;

        public bool isOutOfPool;

#if PrivateSDK
        [NonSerialized]
        public EffectModule module;
#endif

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

        public virtual void SetIntensity(float value, bool loopOnly = false)
        {

        }

        public virtual void SetSpeed(float value, bool loopOnly = false)
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

        public virtual void SetSize(float value)
        {

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
            this.transform.position = position;
            this.transform.rotation = rotation;
            SetSpeed(speed, true);
        }

        public virtual void CollisionStay(Vector3 position, Quaternion rotation, float speed, float intensity)
        {
            this.transform.position = position;
            this.transform.rotation = rotation;
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
            if (despawnCallback != null)
            {
                despawnCallback.Invoke(this);
                despawnCallback = null;
            }
        }
    }
}
