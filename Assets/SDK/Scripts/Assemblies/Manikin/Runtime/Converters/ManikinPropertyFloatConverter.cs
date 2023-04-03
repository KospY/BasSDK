using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad.Manikin
{
    [CreateAssetMenu(menuName = "Manikin/Converters/Float Converter")]
    public class ManikinPropertyFloatConverter : ManikinPropertyShaderTargetConverter
    {
        protected override void ApplyMaterialUpdates(GameObject obj, float[] values, Material[] materials, int materialIndex)
        {
            //If we're playing, use the already cached propertyID.
            if (Application.isPlaying)
            {
                materials[materialIndex].SetFloat(TargetShaderPropertyID, values[0]);
            }
            else
            {
                materials[materialIndex].SetFloat(targetShaderProperty, values[0]);
            }
        }

        protected override void ApplyMaterialPropertyBlock(GameObject obj, float[] values, Renderer renderer, int materialIndex)
        {
            renderer.GetPropertyBlock(propertyBlock, materialIndex);

            if (Application.isPlaying)
            {
                propertyBlock.SetFloat(TargetShaderPropertyID, values[0]);
            }
#if UNITY_EDITOR
            else
            {
                //Use the string shader name while not playing and force refresh the scene view.
                propertyBlock.SetFloat(targetShaderProperty, values[0]);
                UnityEditor.EditorWindow view = UnityEditor.EditorWindow.GetWindow<UnityEditor.SceneView>();
                view.Repaint();
            }
#endif
            renderer.SetPropertyBlock(propertyBlock, materialIndex);
        }
    }
}