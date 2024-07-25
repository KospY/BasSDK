using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Effects/FxModuleMaterial.html")]
    public class FxModuleMaterial : FxModule
    {
        public List<Property.Float> floatProperties = new List<Property.Float>();
        public List<Property.Color> colorProperties = new List<Property.Color>();

        protected List<Property> intensityProperties = new List<Property>();
        protected List<Property> speedProperties = new List<Property>();
        protected Renderer meshRenderer;

        public class Property
        {
            public string name;
            public EffectLink link;
            [NonSerialized]
            public int id;

            public virtual void Update(Material material, float value) { }

            [Serializable]
            public class Float : Property
            {
                public AnimationCurve curve;
                public override void Update(Material material, float value)
                {
                    material.SetFloat(id, curve.Evaluate(value));
                }
            }

            [Serializable]
            public class Color : Property
            {
                [GradientUsage(true)]
                public Gradient gradient;
                public override void Update(Material material, float value)
                {
                    material.SetColor(id, gradient.Evaluate(value));
                }
            }
        }

    }
}
