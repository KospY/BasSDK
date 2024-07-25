using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ThunderRoad.Reveal
{
    /// <summary>
    /// This property drawer [ColorMask], allows you to set any combinations of Alpha, Blue, Green, or Red, for the shader's color mask.
    /// </summary>
    public class ColorMaskDrawer : MaterialPropertyDrawer
    {
        static string[] displayOptions = new string[4] { "A", "B", "G", "R" };

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, MaterialProperty prop, String label, MaterialEditor editor)
        {
            int val = EditorGUI.MaskField(position, prop.displayName, (int)prop.floatValue, displayOptions);
            prop.floatValue = (val < 0) ? 15 : val;
        }
    }
}
