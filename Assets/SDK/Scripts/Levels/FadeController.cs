using UnityEngine;
using UnityEngine.Rendering;

namespace ThunderRoad
{
    public class FadeController : MonoBehaviour
    {
        public float weight = 0;
        public Volume volume;
        public MeshRenderer sphereMesh;

        protected float currentWeight;
        protected MaterialPropertyBlock propertyBlock;

        private void OnValidate()
        {
            if (sphereMesh)
            {
                propertyBlock = new MaterialPropertyBlock();
            }
            Update();
        }

        private void Awake()
        {
            if (sphereMesh)
            {
                propertyBlock = new MaterialPropertyBlock();
            }
        }

        void Update()
        {
            if (currentWeight != weight)
            {
                currentWeight = weight;
                if (Common.IsAndroid)
                {
                    Shadowood.Tonemapping.SetExposureStatic(-10 * weight);
                    if (sphereMesh)
                    {
                        propertyBlock.SetColor("_BaseColor", new Color(0, 0, 0, weight));
                        sphereMesh.SetPropertyBlock(propertyBlock);
                    }
                }
                else
                {
                    if (volume.enabled)
                    {
                        if (volume.weight == 0)
                        {
                            volume.enabled = false;
                        }
                    }
                    else if (volume.weight > 0)
                    {
                        volume.enabled = true;
                    }
                    volume.weight = weight;
                }
            }
        }

        private void OnDisable()
        {
            if (Common.IsAndroid)
            {
                Shadowood.Tonemapping.SetExposureStatic(-10 * 0);
                if (sphereMesh)
                {
                    propertyBlock.SetColor("_Color", new Color(0, 0, 0, 0));
                    sphereMesh.SetPropertyBlock(propertyBlock);
                }
            }
            else
            {
                volume.weight = 0;
                volume.enabled = false;
            }
        }
    }
}