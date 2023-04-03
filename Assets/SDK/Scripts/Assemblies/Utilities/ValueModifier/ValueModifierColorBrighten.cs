using UnityEngine;

namespace ThunderRoad.Utilities
{
    public class ValueModifierColorBrighten : ValueModifierBase
    {
        [Range(-1f, 1f)] public float factor;

        public override ValueModifierSequence.ValueModifierType inputType =>
            ValueModifierSequence.ValueModifierType.Color;
        
        public override ValueModifierSequence.ValueModifierType outputType =>
            ValueModifierSequence.ValueModifierType.Color;

        public override string description =>
            "Modifies the brightness of a given color with a factor.\nNegative values give darker colors, positive values give brighter colors.";

        public override object ProcessColor(Color chainedValue)
        {
            return ChangeBrightness(chainedValue, factor);
        }

        public static Color ChangeBrightness(Color color, float factor)
        {
            var red = color.r;
            var green = color.g;
            var blue = color.b;

            if (factor < 0)
            {
                factor = 1f + factor;
                red *= factor;
                green *= factor;
                blue *= factor;
            }
            else
            {
                red = (1f - red) * factor + red;
                green = (1f - green) * factor + green;
                blue = (1f - blue) * factor + blue;
            }

            return new Color(red, green, blue);
        }
    }
}