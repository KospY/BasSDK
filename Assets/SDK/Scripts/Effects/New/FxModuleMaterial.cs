using System;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad.Plugins;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/FxModuleMaterial")]
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

        private void Awake()
        {
            meshRenderer = this.GetComponent<MeshRenderer>();
            if (!meshRenderer) meshRenderer = this.GetComponent<SkinnedMeshRenderer>();

            List<Property> properties = new List<Property>();
            properties.AddRange(floatProperties);
            properties.AddRange(colorProperties);

            foreach (Property property in properties)
            {
                property.id = Shader.PropertyToID(property.name);
                if (property.link == EffectLink.Intensity) { intensityProperties.Add(property); }
                else if (property.link == EffectLink.Speed) { speedProperties.Add(property); }
            }
        }

        public override void SetIntensity(float intensity)
        {
            foreach (Property property in intensityProperties)
            {
                foreach (Material material in meshRenderer.materialInstances())
                {
                    property.Update(material, intensity);
                }
            }
        }

        public override void SetSpeed(float speed)
        {
            foreach (Property property in speedProperties)
            {
                foreach (Material material in meshRenderer.materialInstances())
                {
                    property.Update(material, speed);
                }
            }
        }

        public override void Stop()
        {
            SetIntensity(0);
        }
    }
}
