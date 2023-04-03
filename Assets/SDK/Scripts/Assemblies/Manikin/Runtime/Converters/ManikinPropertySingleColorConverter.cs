using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace ThunderRoad.Manikin
{
    [CreateAssetMenu(menuName = "Manikin/Converters/Single Color Converter")]
    public class ManikinPropertySingleColorConverter : ManikinPropertyShaderTargetConverter
    {
        protected override void ApplyMaterialUpdates(GameObject obj, float[] values, Material[] materials, int materialIndex)
        {
#if UNITY_EDITOR
            Assert.IsTrue(values.Length == 4, "values array length should equal 4 for this converter.");
            Assert.IsTrue(materialIndex >= 0 && materialIndex < materials.Length, "material index must be in range of the materials array! Index was "
                + materialIndex + ". Materials length is " + materials.Length + " for converter " + name);
#endif
            //If we're playing, use the already cached propertyID.
            if (Application.isPlaying)
            {
                materials[materialIndex].SetColor(TargetShaderPropertyID, new Color32(Convert.ToByte(values[0] * 255f), Convert.ToByte(values[1] * 255f), Convert.ToByte(values[2] * 255f), Convert.ToByte(values[3] * 255f)));
            }
            else
            {
                materials[materialIndex].SetColor(targetShaderProperty, new Color32(Convert.ToByte(values[0] * 255f), Convert.ToByte(values[1] * 255f), Convert.ToByte(values[2] * 255f), Convert.ToByte(values[3] * 255f)));
            }
        }

        protected override void ApplyMaterialPropertyBlock(GameObject obj, float[] values, Renderer renderer = null, int materialIndex = 0)
        {
#if UNITY_EDITOR
            Assert.IsTrue(values.Length == 4, "values array length should equal 4 for this converter.");
#endif
            renderer.GetPropertyBlock(propertyBlock, materialIndex);

            if (Application.isPlaying)
            {
                propertyBlock.SetColor(targetShaderProperty, new Color(values[0], values[1], values[2], values[3]));
            }
#if UNITY_EDITOR
            else
            {
                //Use the string shader name while not playing and force refresh the scene view.
                propertyBlock.SetColor(targetShaderProperty, new Color(values[0], values[1], values[2], values[3]));
                UnityEditor.EditorWindow view = UnityEditor.EditorWindow.GetWindow<UnityEditor.SceneView>();
                view.Repaint();
            }
#endif
            renderer.SetPropertyBlock(propertyBlock, materialIndex);
        }
    }
}
