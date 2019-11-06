using UnityEngine;
using System;
using UnityEngine.Experimental.VFX;

namespace BS
{
    [ExecuteInEditMode]
    public class EffectMesh : Effect
    {
        [Header("Color Gradient")]
        public LinkedGradient linkBaseColor = LinkedGradient.None;

        protected MeshRenderer rend;

        [Header("Mesh Display")]
        public bool meshDisplay;

        [Header("Intensity to mesh size")]
        public bool meshSize;
        public AnimationCurve curveMeshSize;

        [NonSerialized]
        public float currentValue;

        [NonSerialized, GradientUsage(true)]
        public Gradient currentMainGradient;
        [NonSerialized, GradientUsage(true)]
        public Gradient currentSecondaryGradient;

        [NonSerialized]
        public MaterialPropertyBlock materialPropertyBlock;

        private void OnValidate()
        {
            rend = GetComponent<MeshRenderer>();
            materialPropertyBlock = new MaterialPropertyBlock();
        }

        private void Awake()
        {
            rend = GetComponent<MeshRenderer>();
            rend.enabled = false;
        }

        public override void Play()
        {
            rend.enabled = true;
            if (meshDisplay)
                gameObject.GetComponent<MeshRenderer>().enabled = true;
        }

        public override void Stop()
        {
            rend.enabled = false;
            if (meshDisplay)
                gameObject.GetComponent<MeshRenderer>().enabled = false;
        }

        public override void SetIntensity(float value)
        {
            Debug.Log("value: " + value);

            currentValue = value;

            if (meshSize)
            {
                float meshSizeValue = curveMeshSize.Evaluate(value);
                Debug.Log("meshSizeValue : " + meshSizeValue);
                transform.localScale = new Vector3(meshSizeValue, meshSizeValue, meshSizeValue);
                Debug.Log("transform.localScale : " + transform.localScale);
            }

            // Set material color
            bool updatePropertyBlock = false;
            if (linkBaseColor == LinkedGradient.Main && currentMainGradient != null)
            {
                materialPropertyBlock.SetColor("_BaseColor", currentMainGradient.Evaluate(value));
                updatePropertyBlock = true;
            }
            else if (linkBaseColor == LinkedGradient.Secondary && currentSecondaryGradient != null)
            {
                materialPropertyBlock.SetColor("_BaseColor", currentSecondaryGradient.Evaluate(value));
                updatePropertyBlock = true;
            }
            if (updatePropertyBlock) rend.SetPropertyBlock(materialPropertyBlock);
        }

        public override void SetMainGradient(Gradient gradient)
        {
            currentMainGradient = gradient;
            SetIntensity(currentValue);
        }

        public override void SetSecondaryGradient(Gradient gradient)
        {
            currentSecondaryGradient = gradient;
            SetIntensity(currentValue);
        }

        public override void Despawn()
        {
            rend.enabled = false;
    #if ProjectCore
                if (Application.isPlaying)
                {
                    EffectInstance orgEffectInstance = effectInstance;
                    effectInstance = null;
                    EffectModuleVfx.Despawn(this);
                    orgEffectInstance.OnEffectDespawn();
                }
    #endif
        }
    }
}
