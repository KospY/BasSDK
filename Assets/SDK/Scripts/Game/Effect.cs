using System;
using UnityEngine;

namespace BS
{
    public class Effect : MonoBehaviour
    {
        public DespawnCallback despawnCallback;
        public delegate void DespawnCallback(Effect effect);
#if ProjectCore
        [NonSerialized]
        public EffectModule module;
#endif
        public Step step = Step.Start;
        public enum Step
        {
            Start,
            Loop,
            End,
        }

        public virtual void Play()
        {

        }

        public virtual void Stop(bool loopOnly = false)
        {

        }

        public virtual void SetIntensity(float value, bool loopOnly = false)
        {

        }

        public virtual void SetMainGradient(Gradient gradient)
        {

        }

        public virtual void SetSecondaryGradient(Gradient gradient)
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
#if ProjectCore
        public virtual void SetCollisionHandler(CollisionHandler collisionHandler)
        {

        }
#endif
        public virtual void CollisionStay(Vector3 position, Quaternion rotation, float intensity)
        {
            this.transform.position = position;
            this.transform.rotation = rotation;
            SetIntensity(intensity, true);
        }

        public virtual void CollisionStay(float intensity)
        {
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
