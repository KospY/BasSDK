using System;
using UnityEngine;

namespace BS
{
    public class Effect : MonoBehaviour
    {
#if ProjectCore
        [NonSerialized]
        public EffectInstance effectInstance;
#endif
        [NonSerialized]
        public float spawnTime;

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

        public virtual void SetColor(Color mainColor)
        {

        }

        public virtual void SetColor(Color mainColor, Color secondaryColor)
        {

        }

        public virtual void SetTarget(Transform transform)
        {

        }

        public virtual void SetMesh(Mesh mesh)
        {

        }

        public virtual void Despawn()
        {

        }
    }
}
