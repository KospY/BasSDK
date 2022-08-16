using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/FxModuleVfx")]
    public class FxModuleVfx : FxModule
    {
        public List<Property.Float> floatProperties = new List<Property.Float>();
        public List<Property.Color> colorProperties = new List<Property.Color>();

        protected List<Property> intensityProperties = new List<Property>();
        protected List<Property> speedProperties = new List<Property>();
        protected VisualEffect vfx;

        public class Property
        {
            public string name;
            public EffectLink link;
            [NonSerialized]
            public int id;

            public virtual void Update(VisualEffect vfx, float value) { }

            [Serializable]
            public class Float : Property
            {
                public AnimationCurve curve;
                public override void Update(VisualEffect vfx, float value)
                {
                    vfx.SetFloat(id, curve.Evaluate(value));
                }
            }

            [Serializable]
            public class Color : Property
            {
                [GradientUsage(true)]
                public Gradient gradient;
                public override void Update(VisualEffect vfx, float value)
                {
                    vfx.SetVector4(id, gradient.Evaluate(value));
                }
            }
        }

        private void Awake()
        {
            vfx = this.GetComponent<VisualEffect>();

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

        public override bool IsPlaying()
        {
            return (vfx.aliveParticleCount > 0 ? true : false);
        }

        public override void Play()
        {
            base.Play();
            vfx.Play();
        }

        public override void SetIntensity(float intensity)
        {
            foreach (Property property in intensityProperties)
            {
                property.Update(vfx, intensity);
            }
        }

        public override void SetSpeed(float speed)
        {
            foreach (Property property in speedProperties)
            {
                property.Update(vfx, speed);
            }
        }

        public override void Stop()
        {
            vfx.Stop();
        }
    }
}
