using System;

#if !ODIN_INSPECTOR

namespace TriInspector
{
    //Tri inspector doesnt have a inline button attribute
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class InlineButtonAttribute : ButtonAttribute
    {
        public InlineButtonAttribute() : base()
        {
        }
        
        public InlineButtonAttribute(string name) : base(name)
        {
        }
        
        public InlineButtonAttribute(ButtonSizes buttonSize, string name = null) : base(buttonSize, name)
        {
        }
        
        public InlineButtonAttribute(string action, string label = null)
        {
            UnityEngine.Debug.LogWarning("InlineButtonAttribute with action is not supported in tri inspector");
        }
    }
    
    public class ValueDropdownAttribute : Attribute
    {
        public ValueDropdownAttribute(string valuesGetter)
        {
            UnityEngine.Debug.LogWarning("ValueDropdownAttribute is not supported in tri inspector, it must be migrated to their Dropdown attribute");
        }
    }
    
    public class FoldoutGroupAttribute : Attribute
    {
        public FoldoutGroupAttribute(string group)
        {
            UnityEngine.Debug.LogWarning("FoldoutGroupAttribute is not supported in tri inspector, it must be migrated to their DeclareFoldoutGroup and group attribute");
        }
    }
    //We cant override buttons or inline buttons here, because we use EasyButtons instead, so we need to make sure things that use buttons do:
    /*
     * #if ODIN_INSPECTOR
     * using Sirenix.OdinInspector;
     * #else
     * using TriInspector;
     */
    // they also need to wrap the attributes in #if ODIN_INSPECTOR

    public enum SdfIconType
    {
        None
    }
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class MinMaxSliderAttribute : Attribute
    {
        public MinMaxSliderAttribute(float minValue, float maxValue, bool showFields = false)
        { }

        public MinMaxSliderAttribute(string minValueGetter, float maxValue, bool showFields = false)
        { }

        public MinMaxSliderAttribute(float minValue, string maxValueGetter, bool showFields = false)
        { }

        public MinMaxSliderAttribute(string minValueGetter, string maxValueGetter, bool showFields = false)
        { }
        public MinMaxSliderAttribute(string minMaxValueGetter, bool showFields = false)
        { }
    }
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class TableListAttribute : Attribute
    { }
    
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public abstract class PropertyGroupAttribute : Attribute
    {
        public PropertyGroupAttribute(string groupId, float order)
        { }
        public PropertyGroupAttribute(string groupId)
            : this(groupId, 0.0f)
        { }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public sealed class BoxGroupAttribute : PropertyGroupAttribute
    {
        // Empty stub for when ODIN is not installed
        public BoxGroupAttribute(string group, bool showLabel = true, bool centerLabel = false, float order = 0.0f)
            : base(group, order)
        { }
    }
    
    
#if UNITY_EDITOR
    public class OdinEditor : UnityEditor.Editor
    {
        protected virtual void OnDisable()
        {
            
        }
    }
#endif
}


#endif
