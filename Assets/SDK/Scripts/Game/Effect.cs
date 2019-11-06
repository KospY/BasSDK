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

        public Step step = Step.Loop;
        public enum Step
        {
            Start,
            Loop,
            End,
        }

        [Header("Mesh Display")]
        public bool meshDisplay;

        [Header("Intensity to mesh size")]
        public bool meshSize;
        public AnimationCurve curveMeshSize;

        public virtual void Play()
        {

        }

        public virtual void Stop()
        {

        }

        public virtual void SetIntensity(float value)
        {
            if (meshSize)
            {
                float meshSizeValue = curveMeshSize.Evaluate(value);
                transform.localScale = new Vector3(meshSizeValue, meshSizeValue, meshSizeValue);
            }
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

        public virtual void SetMeshRenderer(MeshRenderer meshRenderer)
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
