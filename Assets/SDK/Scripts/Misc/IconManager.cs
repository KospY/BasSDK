using System.Reflection;
using UnityEngine;

namespace ThunderRoad
{
    public class IconManager
    {
        public enum LabelIcon
        {
            Gray = 0,
            Blue,
            Teal,
            Green,
            Yellow,
            Orange,
            Red,
            Purple
        }

        public enum Icon
        {
            CircleGray = 0,
            CircleBlue,
            CircleTeal,
            CircleGreen,
            CircleYellow,
            CircleOrange,
            CircleRed,
            CirclePurple,
            DiamondGray,
            DiamondBlue,
            DiamondTeal,
            DiamondGreen,
            DiamondYellow,
            DiamondOrange,
            DiamondRed,
            DiamondPurple
        }

        private static GUIContent[] labelIcons;
        private static GUIContent[] largeIcons;

        public static void SetIcon(GameObject gObj, LabelIcon icon)
        {
            if (!Application.isEditor) return;

            if (labelIcons == null)
            {
                labelIcons = GetTextures("sv_label_", string.Empty, 0, 8);
            }

            SetIcon(gObj, labelIcons[(int)icon].image as Texture2D);
        }

        public static void SetIcon(GameObject gObj, Icon icon)
        {
            if (!Application.isEditor) return;

            if (largeIcons == null)
            {
                largeIcons = GetTextures("sv_icon_dot", "_pix16_gizmo", 0, 16);
            }

            SetIcon(gObj, largeIcons[(int)icon].image as Texture2D);
        }

        public static void SetIcon(GameObject gObj, Texture2D texture)
        {
#if UNITY_EDITOR
            var ty = typeof(UnityEditor.EditorGUIUtility);
            var mi = ty.GetMethod("SetIconForObject", BindingFlags.NonPublic | BindingFlags.Static);
            mi.Invoke(null, new object[] { gObj, texture });
#endif
        }

        private static GUIContent[] GetTextures(string baseName, string postFix, int startIndex, int count)
        {
            GUIContent[] guiContentArray = new GUIContent[count];
            for (int index = 0; index < count; ++index)
            {
#if UNITY_EDITOR
                guiContentArray[index] = UnityEditor.EditorGUIUtility.IconContent(baseName + (object)(startIndex + index) + postFix);
#endif
            }
            return guiContentArray;
        }
    }
}