using UnityEngine;


namespace Shadowood
{
    public class SingleLayerAttribute : PropertyAttribute
    {
    }
    public class RenderingLayerMaskAttribute : PropertyAttribute
    {
    }

#if UNITY_EDITOR
    [UnityEditor.CustomPropertyDrawer(typeof(SingleLayerAttribute))]
    class LayerAttributeEditor : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
        {
            property.intValue = UnityEditor.EditorGUI.LayerField(position, label, property.intValue);
        }
    }

    [UnityEditor.CustomPropertyDrawer(typeof(RenderingLayerMaskAttribute))]
    class RenderingLayerMaskAttributeEditor : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
        {
            // One day maybe
            //UniversalRenderPipelineGlobalSettings universalRenderPipelineGlobalSettings = GraphicsSettings.GetSettingsForRenderPipeline<UniversalRenderPipeline>() as UniversalRenderPipelineGlobalSettings;
            property.intValue = UnityEditor.EditorGUI.MaskField(position, label, property.intValue, new[] { "Default", "Layer 1", "Layer 2", "Layer 3", "Layer 4", "Layer 5", "Layer 6", "Layer 7" });
        }
    }
#endif
}