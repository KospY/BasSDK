using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/FxModuleMaterial")]
    public class FxModuleMaterialBlock : FxModule
    {
        public List<MeshRenderer> meshRenderers;
        public List<Property.Float> floatProperties = new List<Property.Float>();
        public List<Property.Color> colorProperties = new List<Property.Color>();
        protected MaterialPropertyBlock propertyBlock;


        [Serializable]
        public class Property
        {
            public string name;
            public EffectLink link;

            protected int id;
            protected MaterialPropertyBlock materialPropertyBlock;

            public void Init(MaterialPropertyBlock materialPropertyBlock) 
            {
                this.id = Shader.PropertyToID(name);
                this.materialPropertyBlock = materialPropertyBlock;
            }

            public virtual void UpdatePropertyBlock(float value) { }

            [Serializable]
            public class Float : Property
            {
                public AnimationCurve curve;
                public override void UpdatePropertyBlock(float value)
                {
                    materialPropertyBlock.SetFloat(id, curve.Evaluate(value));
                }
            }

            [Serializable]
            public class Color : Property
            {
                [GradientUsage(true, ColorSpace.Linear)]
                public Gradient gradient;
                public override void UpdatePropertyBlock(float value)
                {
                    // For some reason, using setColor don't apply the same exact color as in the shader
                    materialPropertyBlock.SetVector(id, gradient.Evaluate(value));
                }
            }
        }

    }
}
