using UnityEngine;

namespace ThunderRoad.Manikin
{
    public class ManikinPropertyShaderTargetConverter : ManikinPropertyConverterBase
    {
        public string targetShaderProperty;

        protected int TargetShaderPropertyID
        {
            get
            {
                if (_targetShaderPropertyID == -1)
                {
                    _targetShaderPropertyID = Shader.PropertyToID(targetShaderProperty);
                }
                return _targetShaderPropertyID;
            }
        }
        protected int _targetShaderPropertyID = -1;

        protected MaterialPropertyBlock propertyBlock;

        public override void ApplyProperty(GameObject obj, float[] values, bool useSRPBatcher = false, Renderer renderer = null, int materialIndex = 0, object payload = null)
        {
            if (renderer == null)
                return;

            if (useSRPBatcher)
            {
                Material[] materials = renderer.materialInstances();

                if (materials != null)
                {
                    for (int i = 0; i < materials.Length; i++)
                    {
                        ApplyMaterialUpdates(obj, values, materials, i);
                    }
                }
            }
            else
            {
                if (propertyBlock == null)
                {
                    propertyBlock = new MaterialPropertyBlock();
                }

                //Debug.LogError("Non SRP Batcher path!");
                ApplyMaterialPropertyBlock(obj, values, renderer, materialIndex);
            }
        }

        protected virtual void ApplyMaterialUpdates(GameObject obj, float[] values, Material[] materials, int materialIndex) { }

        protected virtual void ApplyMaterialPropertyBlock(GameObject obj, float[] values, Renderer renderer, int materialIndex) { }
    }
}