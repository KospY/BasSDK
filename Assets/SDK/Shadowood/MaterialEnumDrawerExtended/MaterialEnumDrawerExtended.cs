#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;


public class MaterialEnumDrawerExtended: MaterialPropertyDrawer
{
    private readonly GUIContent[] names;
    private readonly int[] values;

    // Single argument: enum type name; entry names & values fetched via reflection
    public MaterialEnumDrawerExtended(string enumName)
    {
        var loadedTypes = TypeCache.GetTypesDerivedFrom(typeof(Enum));
        try
        {
            var enumType = loadedTypes.FirstOrDefault(x => x.Name == enumName || x.FullName == enumName);
            var enumNames = Enum.GetNames(enumType);
            this.names = new GUIContent[enumNames.Length];
            for (int i = 0; i < enumNames.Length; ++i)
                this.names[i] = new GUIContent(enumNames[i]);

            var enumVals = Enum.GetValues(enumType);
            values = new int[enumVals.Length];
            for (var i = 0; i < enumVals.Length; ++i)
                values[i] = (int)enumVals.GetValue(i);
        }
        catch (Exception)
        {
            Debug.LogWarningFormat("Failed to create MaterialEnumExtended, enum {0} not found", enumName);
            throw;
        }
    }

    // name,value,name,value,... pairs: explicit names & values
    public MaterialEnumDrawerExtended(string n1, float v1) : this(new[] {n1}, new[] {v1}) {}
    public MaterialEnumDrawerExtended(string n1, float v1, string n2, float v2) : this(new[] { n1, n2 }, new[] { v1, v2 }) {}
    public MaterialEnumDrawerExtended(string n1, float v1, string n2, float v2, string n3, float v3) : this(new[] { n1, n2, n3 }, new[] { v1, v2, v3 }) {}
    public MaterialEnumDrawerExtended(string n1, float v1, string n2, float v2, string n3, float v3, string n4, float v4) : this(new[] { n1, n2, n3, n4 }, new[] { v1, v2, v3, v4 }) {}
    public MaterialEnumDrawerExtended(string n1, float v1, string n2, float v2, string n3, float v3, string n4, float v4, string n5, float v5) : this(new[] { n1, n2, n3, n4, n5 }, new[] { v1, v2, v3, v4, v5 }) {}
    public MaterialEnumDrawerExtended(string n1, float v1, string n2, float v2, string n3, float v3, string n4, float v4, string n5, float v5, string n6, float v6) : this(new[] { n1, n2, n3, n4, n5, n6 }, new[] { v1, v2, v3, v4, v5, v6 }) {}
    public MaterialEnumDrawerExtended(string n1, float v1, string n2, float v2, string n3, float v3, string n4, float v4, string n5, float v5, string n6, float v6, string n7, float v7) : this(new[] { n1, n2, n3, n4, n5, n6, n7 }, new[] { v1, v2, v3, v4, v5, v6, v7 }) {}
    public MaterialEnumDrawerExtended(string n1, float v1, string n2, float v2, string n3, float v3, string n4, float v4, string n5, float v5, string n6, float v6, string n7, float v7, string n8, float v8) : this(new[] { n1, n2, n3, n4, n5, n6, n7, n8 }, new[] { v1, v2, v3, v4, v5, v6, v7, v8 }) {}
    public MaterialEnumDrawerExtended(string n1, float v1, string n2, float v2, string n3, float v3, string n4, float v4, string n5, float v5, string n6, float v6, string n7, float v7, string n8, float v8, string n9, float v9) : this(new[] { n1, n2, n3, n4, n5, n6, n7, n8, n9 }, new[] { v1, v2, v3, v4, v5, v6, v7, v8, v9 }) {}
    public MaterialEnumDrawerExtended(string n1, float v1, string n2, float v2, string n3, float v3, string n4, float v4, string n5, float v5, string n6, float v6, string n7, float v7, string n8, float v8, string n9, float v9, string n10, float v10) : this(new[] { n1, n2, n3, n4, n5, n6, n7, n8, n9, n10 }, new[] { v1, v2, v3, v4, v5, v6, v7, v8, v9, v10 }) {}
    public MaterialEnumDrawerExtended(string n1, float v1, string n2, float v2, string n3, float v3, string n4, float v4, string n5, float v5, string n6, float v6, string n7, float v7, string n8, float v8, string n9, float v9, string n10, float v10, string n11, float v11) : this(new[] { n1, n2, n3, n4, n5, n6, n7, n8, n9, n10, n11 }, new[] { v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11 }) {}
    
    public MaterialEnumDrawerExtended(string n1, float v1, string n2, float v2, string n3, float v3, string n4, float v4, string n5, float v5, string n6, float v6, string n7, float v7, string n8, 
        float v8, string n9, float v9, string n10, float v10, string n11, float v11, string n12, float v12) : this(new[] { n1, n2, n3, n4, n5, n6, n7, n8, n9, n10, n11, n12  }, 
        new[] { v1, v2, v3, v4, v5, v6, v7, v8, 
            v9, v10, v11, v12 }) {}
    
    public MaterialEnumDrawerExtended(string n1, float v1, string n2, float v2, string n3, float v3, string n4, float v4, string n5, float v5, string n6, float v6, string n7, float v7, string n8, 
        float v8, string n9, float v9, string n10, float v10, string n11, float v11, string n12, float v12, string n13, float v13) : this(new[] { n1, n2, n3, n4, n5, n6, n7, n8, n9, n10, n11, n12, n13, n13 }, 
        new[] { v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13 }) {}
    
    public MaterialEnumDrawerExtended(string n1, float v1, string n2, float v2, string n3, float v3, string n4, float v4, string n5, float v5, string n6, float v6, string n7, float v7, string n8, 
        float v8, string n9, float v9, string n10, float v10, string n11, float v11, string n12, float v12, string n13, float v13, string n14, float v14) : this(new[] { n1, n2, n3, n4, n5, n6, n7, n8, n9, n10, n11, n12, n13, n14}, 
        new[] { v1, v2, v3, v4, v5, v6, v7, v8, 
            v9, v10, v12, v13, v14 }) {}
    
    public MaterialEnumDrawerExtended(string n1, float v1, string n2, float v2, string n3, float v3, string n4, float v4, string n5, float v5, string n6, float v6, string n7, float v7, string n8, 
        float v8, string n9, float v9, string n10, float v10, string n11, float v11, string n12, float v12, string n13, float v13, string n14, float v14, string n15, float v15) : this(new[] { n1, n2, n3, n4, n5, n6, n7, n8, n9, n10, n11, n12, n13, n14, n15 }, 
        new[] { v1, v2, v3, v4, v5, v6, v7, v8, 
            v9, v10, v11, v12, v13, v14, v15}) {}
    
    public MaterialEnumDrawerExtended(string n1, float v1, string n2, float v2, string n3, float v3, string n4, float v4, string n5, float v5, string n6, float v6, string n7, float v7, string n8, 
        float v8, string n9, float v9, string n10, float v10, string n11, float v11, string n12, float v12, string n13, float v13, string n14, float v14, string n15, float v15, string n16, float v16) : this(new[] { n1, n2, n3, n4, n5, n6, n7, n8, n9, n10, n11, n12, n13, n14, n15, n16 }, 
        new[] { v1, v2, v3, v4, v5, v6, v7, v8, 
            v9, v10, v11, v12, v13, v14, v15, v16 }) {}
    
    public MaterialEnumDrawerExtended(string n1, float v1, string n2, float v2, string n3, float v3, string n4, float v4, string n5, float v5, string n6, float v6, string n7, float v7, string n8, 
        float v8, string n9, float v9, string n10, float v10, string n11, float v11, string n12, float v12, string n13, float v13, string n14, float v14, string n15, float v15, string n16, float v16, 
        string n17, float v17) : this(new[] { n1, n2, n3, n4, n5, n6, n7, n8, n9, n10, n11, n12, n13, n14, n15, n16, n17 }, 
        new[] { v1, v2, v3, v4, v5, v6, v7, v8, 
            v9, v10, v11, v12, v13, v14, v15, v16, 
            v17 }) {}
    
    public MaterialEnumDrawerExtended(string n1, float v1, string n2, float v2, string n3, float v3, string n4, float v4, string n5, float v5, string n6, float v6, string n7, float v7, string n8, 
        float v8, string n9, float v9, string n10, float v10, string n11, float v11, string n12, float v12, string n13, float v13, string n14, float v14, string n15, float v15, string n16, float v16, 
        string n17, float v17, string n18, float v18) : this(new[] { n1, n2, n3, n4, n5, n6, n7, n8, n9, n10, n11, n12, n13, n14, n15, n16, n17, n18 }, 
        new[] { v1, v2, v3, v4, v5, v6, v7, v8, 
            v9, v10, v11, v12, v13, v14, v15, v16, 
            v17, v18 }) {}
    
    public MaterialEnumDrawerExtended(string n1, float v1, string n2, float v2, string n3, float v3, string n4, float v4, string n5, float v5, string n6, float v6, string n7, float v7, string n8, 
        float v8, string n9, float v9, string n10, float v10, string n11, float v11, string n12, float v12, string n13, float v13, string n14, float v14, string n15, float v15, string n16, float v16, 
        string n17, float v17, string n18, float v18, string n19, float v19) : this(new[] { n1, n2, n3, n4, n5, n6, n7, n8, n9, n10, n11, n12, n13, n14, n15, n16, n17, n18, n19 }, 
        new[] { v1, v2, v3, v4, v5, v6, v7, v8, 
            v9, v10, v11, v12, v13, v14, v15, v16, 
            v17, v18, v19 }) {}
    
    public MaterialEnumDrawerExtended(string n1, float v1, string n2, float v2, string n3, float v3, string n4, float v4, string n5, float v5, string n6, float v6, string n7, float v7, string n8, 
        float v8, string n9, float v9, string n10, float v10, string n11, float v11, string n12, float v12, string n13, float v13, string n14, float v14, string n15, float v15, string n16, float v16, 
        string n17, float v17, string n18, float v18, string n19, float v19, string n20, float v20) : this(new[] { n1, n2, n3, n4, n5, n6, n7, n8, n9, n10, n11, n12, n13, n14, n15, n16, n17, n18, n19, n20 }, 
        new[] { v1, v2, v3, v4, v5, v6, v7, v8, 
            v9, v10, v11, v12, v13, v14, v15, v16, 
            v17, v18, v19, v20 }) {}

    public MaterialEnumDrawerExtended(string[] enumNames, float[] vals)
    {
        this.names = new GUIContent[enumNames.Length];
        for (int i = 0; i < enumNames.Length; ++i)
            this.names[i] = new GUIContent(enumNames[i]);

        values = new int[vals.Length];
        for (int i = 0; i < vals.Length; ++i)
            values[i] = (int)vals[i];
    }

    static bool IsPropertyTypeSuitable(MaterialProperty prop)
    {
        return prop.type == MaterialProperty.PropType.Float || prop.type == MaterialProperty.PropType.Range || prop.type == MaterialProperty.PropType.Int;
    }

    public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
    {
        if (prop.type != MaterialProperty.PropType.Float && prop.type != MaterialProperty.PropType.Range && prop.type != MaterialProperty.PropType.Int)
        {
            return 18 * 2.5f; // EditorGUI.kSingleLineHeight
        }
        return base.GetPropertyHeight(prop, label, editor);
    }

    public override void OnGUI(Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor)
    {
        if (!IsPropertyTypeSuitable(prop))
        {
            //GUIContent c = EditorGUIUtility.TempContent("Enum used on a non-float property: " + prop.name, EditorGUIUtility.GetHelpIcon(MessageType.Warning));
            //EditorGUI.LabelField(position, c, EditorStyles.helpBox);
            return;
        }

        //MaterialEditor.BeginProperty(position, prop);

        if (prop.type == MaterialProperty.PropType.Float || prop.type == MaterialProperty.PropType.Range)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = prop.hasMixedValue;

            var value = (int)prop.floatValue;
            int selectedIndex = -1;
            for (var index = 0; index < values.Length; index++)
            {
                var i = values[index];
                if (i == value)
                {
                    selectedIndex = index;
                    break;
                }
            }

            var selIndex = EditorGUI.Popup(position, label, selectedIndex, names);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                prop.floatValue = (float)values[selIndex];
            }
        }
        else
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = prop.hasMixedValue;

            var value = prop.intValue;
            int selectedIndex = -1;
            for (var index = 0; index < values.Length; index++)
            {
                var i = values[index];
                if (i == value)
                {
                    selectedIndex = index;
                    break;
                }
            }

            var selIndex = EditorGUI.Popup(position, label, selectedIndex, names);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                prop.intValue = values[selIndex];
            }
        }

        //MaterialEditor.EndProperty();
    }
}
#endif