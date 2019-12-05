using System;
using UnityEngine;

namespace BS
{
    public class Effect : MonoBehaviour
    {
        [NonSerialized]
        public EffectInstance effectInstance;

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

        public virtual void Stop()
        {

        }

        public virtual void SetIntensity(float value)
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

        public virtual void Despawn()
        {

        }
    }
}
