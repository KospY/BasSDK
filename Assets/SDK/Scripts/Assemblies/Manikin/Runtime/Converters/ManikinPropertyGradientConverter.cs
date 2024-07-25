using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ThunderRoad.Manikin
{
    [CreateAssetMenu(menuName = "Manikin/Converters/Gradient Converter")]
    public class ManikinPropertyGradientConverter : ManikinPropertyShaderTargetConverter
    {
        public Gradient gradient;

        protected override void ApplyMaterialUpdates(GameObject obj, float[] values, Material[] materials, int materialIndex)
        {
            //If we're playing, use the already cached propertyID.
            if (Application.isPlaying)
            {
                materials[materialIndex].SetVector(TargetShaderPropertyID, gradient.Evaluate(values[0]));
            }
            else
            {
                materials[materialIndex].SetVector(targetShaderProperty, gradient.Evaluate(values[0]));
            }
        }

        protected override void ApplyMaterialPropertyBlock(GameObject obj, float[] values, Renderer renderer = null, int materialIndex = 0)
        {
#if UNITY_EDITOR
            Assert.IsTrue(values.Length == 1, "values array length should equal 1 for this converter.");
#endif
            renderer.GetPropertyBlock(propertyBlock, materialIndex);

            if (Application.isPlaying)
            {
                propertyBlock.SetVector(TargetShaderPropertyID, gradient.Evaluate(values[0]));
            }
#if UNITY_EDITOR
            else
            {
                //Use the string shader name while not playing and force refresh the scene view.
                propertyBlock.SetVector(targetShaderProperty, gradient.Evaluate(values[0]));
                UnityEditor.EditorWindow view = UnityEditor.EditorWindow.GetWindow<UnityEditor.SceneView>();
                view.Repaint();
            }
#endif
            renderer.SetPropertyBlock(propertyBlock, materialIndex);
        }
    }
}
