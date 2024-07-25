using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Needle.ShaderGraphMarkdown
{
    internal class EditorGUIExt2
    {
        private static EditorGUIExt2.Styles ms_Styles = new EditorGUIExt2.Styles();
        private static int repeatButtonHash = "repeatButton".GetHashCode();
        private static float nextScrollStepTime = 0.0f;
        private static int firstScrollWait = 250;
        private static int scrollWait = 30;
        private static int kFirstScrollWait = 250;
        private static int kScrollWait = 30;
        private static DateTime s_NextScrollStepTime = DateTime.Now;
        private static Vector2 s_MouseDownPos = Vector2.zero;
        private static EditorGUIExt2.DragSelectionState s_MultiSelectDragSelection = EditorGUIExt2.DragSelectionState.None;
        private static Vector2 s_StartSelectPos = Vector2.zero;
        private static List<bool> s_SelectionBackup = (List<bool>) null;
        private static List<bool> s_LastFrameSelections = (List<bool>) null;
        internal static int s_MinMaxSliderHash = "MinMaxSlider".GetHashCode();
        private static bool adding = false;
        private static int initIndex = 0;
        private static int scrollControlID;
        private static EditorGUIExt2.MinMaxSliderState s_MinMaxSliderState;
        private static bool[] initSelections;

        //

        internal static float kLabelFloatMinW
        {
            get { return (float) ((double) EditorGUIUtility.labelWidth + (double) EditorGUIUtility.fieldWidth + 5.0); }
        }

        internal static float kLabelFloatMaxW
        {
            get { return (float) ((double) EditorGUIUtility.labelWidth + (double) EditorGUIUtility.fieldWidth + 5.0); }
        }

        //

        internal static Rect GetSliderRect(bool hasLabel, params GUILayoutOption[] options)
        {
            return GetSliderRect(hasLabel, GUI.skin.horizontalSlider, options);
        }

        internal static Rect GetSliderRect(
            bool hasLabel,
            GUIStyle sliderStyle,
            params GUILayoutOption[] options)
        {
            return GUILayoutUtility.GetRect(hasLabel ? kLabelFloatMinW : EditorGUIUtility.fieldWidth, (float) ((double) kLabelFloatMaxW + 5.0 + 100.0), 18f, 18f, sliderStyle, options);
        }

        //
        public static void MinMaxSlider(
            ref float minValue,
            ref float maxValue,
            float minLimit,
            float maxLimit,
            Texture2D texture = null,
            params GUILayoutOption[] options)
        {
            var rect = GetSliderRect(false, options);
            GUILayoutUtility.GetRect(rect.width, rect.height);
            MinMaxSlider(GetSliderRect(false, options), ref minValue, ref maxValue, minLimit, maxLimit,texture);
        }

        public static void MinMaxSlider(
            Rect position,
            ref float minValue,
            ref float maxValue,
            float minLimit,
            float maxLimit,
            Texture2D texture=null)
        {
            DoMinMaxSlider(EditorGUI.IndentedRect(position), GUIUtility.GetControlID(s_MinMaxSliderHash, FocusType.Passive), ref minValue, ref maxValue, minLimit, maxLimit,texture);
        }

        private static GUIStyle m_MinMaxHorizontalSliderThumb;

        static void SetupStyles()
        {
            // Screw you Unity for making everything internal/private
            var type = typeof(EditorStyles);
            var currentFieldInfo = type.GetField("s_Current", BindingFlags.NonPublic | BindingFlags.Static);
            var toolbarSearchField = type.GetField("m_MinMaxHorizontalSliderThumb", BindingFlags.NonPublic | BindingFlags.Instance);
            m_MinMaxHorizontalSliderThumb = toolbarSearchField.GetValue(currentFieldInfo.GetValue(null)) as GUIStyle;
        }


        internal static GUIStyle minMaxHorizontalSliderThumb
        {
            get
            {
                if (m_MinMaxHorizontalSliderThumb == null) SetupStyles();
                return m_MinMaxHorizontalSliderThumb;
                //return EditorStyles.s_Current.m_MinMaxHorizontalSliderThumb;
                //return GUI.skin.horizontalSliderThumb;
            }
        }

        private static void DoMinMaxSlider(
            Rect position,
            int id,
            ref float minValue,
            ref float maxValue,
            float minLimit,
            float maxLimit,
            Texture2D texture = null)
        {
            float size = maxValue - minValue;
            EditorGUI.BeginChangeCheck();
            DoMinMaxSlider(position, id, ref minValue, ref size, minLimit, maxLimit, minLimit, maxLimit, GUI.skin.horizontalSlider, minMaxHorizontalSliderThumb, true, texture);
            if (!EditorGUI.EndChangeCheck())
                return;
            maxValue = minValue + size;
        }

        public static void MinMaxSlider(
            string label,
            ref float minValue,
            ref float maxValue,
            float minLimit,
            float maxLimit,
            params GUILayoutOption[] options)
        {
            EditorGUI.MinMaxSlider(GetSliderRect(true, options), label, ref minValue, ref maxValue, minLimit, maxLimit);
        }

        public static void MinMaxSlider(
            GUIContent label,
            ref float minValue,
            ref float maxValue,
            float minLimit,
            float maxLimit,
            params GUILayoutOption[] options)
        {
            EditorGUI.MinMaxSlider(GetSliderRect(true, options), label, ref minValue, ref maxValue, minLimit, maxLimit);
        }
        //

        private static bool DoRepeatButton(
            Rect position,
            GUIContent content,
            GUIStyle style,
            FocusType focusType)
        {
            int controlId = GUIUtility.GetControlID(EditorGUIExt2.repeatButtonHash, focusType, position);
            switch (UnityEngine.Event.current.GetTypeForControl(controlId))
            {
                case UnityEngine.EventType.MouseDown:
                    if (position.Contains(UnityEngine.Event.current.mousePosition))
                    {
                        GUIUtility.hotControl = controlId;
                        UnityEngine.Event.current.Use();
                    }

                    return false;
                case UnityEngine.EventType.MouseUp:
                    if (GUIUtility.hotControl != controlId)
                        return false;
                    GUIUtility.hotControl = 0;
                    UnityEngine.Event.current.Use();
                    return position.Contains(UnityEngine.Event.current.mousePosition);
                case UnityEngine.EventType.Repaint:
                    style.Draw(position, content, controlId, false, position.Contains(UnityEngine.Event.current.mousePosition));
                    return controlId == GUIUtility.hotControl && position.Contains(UnityEngine.Event.current.mousePosition);
                default:
                    return false;
            }
        }

        private static bool ScrollerRepeatButton(int scrollerID, Rect rect, GUIStyle style)
        {
            bool flag1 = false;
            if (EditorGUIExt2.DoRepeatButton(rect, GUIContent.none, style, FocusType.Passive))
            {
                bool flag2 = EditorGUIExt2.scrollControlID != scrollerID;
                EditorGUIExt2.scrollControlID = scrollerID;
                if (flag2)
                {
                    flag1 = true;
                    EditorGUIExt2.nextScrollStepTime = Time.realtimeSinceStartup + 1f / 1000f * (float) EditorGUIExt2.firstScrollWait;
                }
                else if ((double) Time.realtimeSinceStartup >= (double) EditorGUIExt2.nextScrollStepTime)
                {
                    flag1 = true;
                    EditorGUIExt2.nextScrollStepTime = Time.realtimeSinceStartup + 1f / 1000f * (float) EditorGUIExt2.scrollWait;
                }

                if (UnityEngine.Event.current.type == UnityEngine.EventType.Repaint)
                    HandleUtility.Repaint();
            }

            return flag1;
        }

        public static void MinMaxScroller(
            Rect position,
            int id,
            ref float value,
            ref float size,
            float visualStart,
            float visualEnd,
            float startLimit,
            float endLimit,
            GUIStyle slider,
            GUIStyle thumb,
            GUIStyle leftButton,
            GUIStyle rightButton,
            bool horiz)
        {
            float num1 = !horiz ? size * 10f / position.height : size * 10f / position.width;
            Rect position1;
            Rect rect1;
            Rect rect2;
            if (horiz)
            {
                position1 = new Rect(position.x + leftButton.fixedWidth, position.y, position.width - leftButton.fixedWidth - rightButton.fixedWidth, position.height);
                rect1 = new Rect(position.x, position.y, leftButton.fixedWidth, position.height);
                rect2 = new Rect(position.xMax - rightButton.fixedWidth, position.y, rightButton.fixedWidth, position.height);
            }
            else
            {
                position1 = new Rect(position.x, position.y + leftButton.fixedHeight, position.width, position.height - leftButton.fixedHeight - rightButton.fixedHeight);
                rect1 = new Rect(position.x, position.y, position.width, leftButton.fixedHeight);
                rect2 = new Rect(position.x, position.yMax - rightButton.fixedHeight, position.width, rightButton.fixedHeight);
            }

            float num2 = Mathf.Min(visualStart, value);
            float num3 = Mathf.Max(visualEnd, value + size);
            EditorGUIExt2.MinMaxSlider(position1, ref value, ref size, num2, num3, num2, num3, slider, thumb, horiz,null);
            if (EditorGUIExt2.ScrollerRepeatButton(id, rect1, leftButton))
                value -= num1 * ((double) visualStart < (double) visualEnd ? 1f : -1f);
            if (EditorGUIExt2.ScrollerRepeatButton(id, rect2, rightButton))
                value += num1 * ((double) visualStart < (double) visualEnd ? 1f : -1f);
            if (UnityEngine.Event.current.type == UnityEngine.EventType.MouseUp && UnityEngine.Event.current.type == UnityEngine.EventType.Used)
                EditorGUIExt2.scrollControlID = 0;
            if ((double) startLimit < (double) endLimit)
                value = Mathf.Clamp(value, startLimit, endLimit - size);
            else
                value = Mathf.Clamp(value, endLimit, startLimit - size);
        }

        public static void MinMaxSlider(
            Rect position,
            ref float value,
            ref float size,
            float visualStart,
            float visualEnd,
            float startLimit,
            float endLimit,
            GUIStyle slider,
            GUIStyle thumb,
            bool horiz,
            Texture texture)
        {
            EditorGUIExt2.DoMinMaxSlider(position, GUIUtility.GetControlID(EditorGUIExt2.s_MinMaxSliderHash, FocusType.Passive), ref value, ref size, visualStart, visualEnd, startLimit, endLimit, slider, thumb, horiz, texture);
        }

        private static float ThumbSize(bool horiz, GUIStyle thumb)
        {
            return horiz ? ((double) thumb.fixedWidth != 0.0 ? thumb.fixedWidth : (float) thumb.padding.horizontal) : ((double) thumb.fixedHeight != 0.0 ? thumb.fixedHeight : (float) thumb.padding.vertical);
        }

        private static void TextureField(string name, Texture texture, float width, float height, bool transparent = false, Material mat = null, ColorWriteMask mask = ColorWriteMask.All, Rect rectIn = new Rect())
        {
            //GUILayout.BeginVertical();

            //var lastRect = GUILayoutUtility.GetLastRect();
            var placeholderContent = new GUIContent("");
            var style = new GUIStyle(GUI.skin.button);
            style.fixedHeight = height;
            style.fixedWidth = width;
            //style.border = new RectOffset(0,0,0,0);
            //style.margin = new RectOffset(0,0,0,0);

            var rect = GUILayoutUtility.GetRect(placeholderContent, style);
            if (rectIn != new Rect()) rect = rectIn;
            style.alignment = TextAnchor.UpperLeft;


            //placeholderContent = new GUIContent("");
            style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.UpperLeft;
            //var recttext = GUILayoutUtility.GetRect(placeholderContent, style);
            var recttext = GUILayoutUtility.GetLastRect();

            if (transparent)
            {
                Color guiColor = GUI.color; // Save the current GUI color
                GUI.color = Color.clear; // This does the magic

                EditorGUI.DrawTextureTransparent(rect, texture);

                GUI.color = guiColor; // Get back to previous GUI color
            }
            else
            {
                EditorGUI.DrawPreviewTexture(rect, texture, mat); // ScaleMode.ScaleToFit, width / height, 0, mask
            }

            //E
            var tc = new GUIContent(name);
            tc.text = name;

            if (name != "") EditorGUI.LabelField(recttext, tc);
            //var result = (Texture2D)EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(130), GUILayout.Height(20),GUILayoutOption);
            //GUILayout.EndVertical();
            //return result;
        }
        
        internal static void DoMinMaxSlider(
            Rect position,
            int id,
            ref float value,
            ref float size,
            float visualStart,
            float visualEnd,
            float startLimit,
            float endLimit,
            GUIStyle slider,
            GUIStyle thumb,
            bool horiz,
            Texture texture = null)
        {
            UnityEngine.Event current = UnityEngine.Event.current;
            bool flag = (double) size == 0.0;

            float min1 = Mathf.Min(visualStart, visualEnd);
            float max = Mathf.Max(visualStart, visualEnd);
            float min2 = Mathf.Min(startLimit, endLimit);
            float max2 = Mathf.Max(startLimit, endLimit);
            
        


            EditorGUIExt2.MinMaxSliderState minMaxSliderState = EditorGUIExt2.s_MinMaxSliderState;
            if (GUIUtility.hotControl == id && minMaxSliderState != null)
            {
                min1 = minMaxSliderState.dragStartLimit;
                min2 = minMaxSliderState.dragStartLimit;
                max = minMaxSliderState.dragEndLimit;
                max2 = minMaxSliderState.dragEndLimit;
            }

            float num2 = 0.0f;

            // Limits the handles to within the entire slider length
            float num3 = Mathf.Clamp(value, min1, max);
            float num4 = Mathf.Clamp(value + size, min1, max) - num3;
            //float num3 = value;
            //float num4 = value + size - num3;

            float num5 = (double) visualStart > (double) visualEnd ? -1f : 1f;
            if (slider == null || thumb == null)
                return;
            Rect rect = thumb.margin.Remove(slider.padding.Remove(position));
            float num6 = EditorGUIExt2.ThumbSize(horiz, thumb);
            float num7;

            Rect position1; // Main slider rect
            Rect leftHandleRect; // Left handle rect
            Rect rightHandleRect; // Right hand rect

            float num8;
            if (horiz)
            {
                float height = (double) thumb.fixedHeight != 0.0 ? thumb.fixedHeight : rect.height;
                num7 = (float) (((double) position.width - (double) slider.padding.horizontal - (double) num6) / ((double) max - (double) min1));

                position1 = new Rect((num3 - min1) * num7 + rect.x, rect.y, num4 * num7 + num6, height);
                leftHandleRect = new Rect(position1.x, position1.y, (float) thumb.padding.left, position1.height);
                rightHandleRect = new Rect(position1.xMax - (float) thumb.padding.right, position1.y, (float) thumb.padding.right, position1.height);
                num8 = current.mousePosition.x - position.x;
            }
            else
            {
                float width = (double) thumb.fixedWidth != 0.0 ? thumb.fixedWidth : rect.width;
                num7 = (float) (((double) position.height - (double) slider.padding.vertical - (double) num6) / ((double) max - (double) min1));
                position1 = new Rect(rect.x, (num3 - min1) * num7 + rect.y, width, num4 * num7 + num6);
                leftHandleRect = new Rect(position1.x, position1.y, position1.width, (float) thumb.padding.top);
                rightHandleRect = new Rect(position1.x, position1.yMax - (float) thumb.padding.bottom, position1.width, (float) thumb.padding.bottom);
                num8 = current.mousePosition.y - position.y;
            }
            
            TextureField("", texture, texture.width, texture.height, transparent: true, mask: ColorWriteMask.All, rectIn:position);
            GUILayoutUtility.GetRect(0, -texture.height);

            switch (current.GetTypeForControl(id))
            {
                case UnityEngine.EventType.MouseDown:

                    if (current.button != 0 || !position.Contains(current.mousePosition)|| (double) min1 - (double) max == 0.0)
                    {
                        break;
                    }

                    if (minMaxSliderState == null)
                        minMaxSliderState = EditorGUIExt2.s_MinMaxSliderState = new EditorGUIExt2.MinMaxSliderState();

                    minMaxSliderState.whereWeDrag = 0;

                    minMaxSliderState.dragStartLimit = startLimit;
                    minMaxSliderState.dragEndLimit = endLimit;
                    
                    //if (position1.Contains(current.mousePosition)) {
                    if (position.Contains(current.mousePosition)) {
                        if (size < 0)
                        {
                            // position1.size = new Vector2(position1.size.y, position1.size.x);
                            var twidth = 12;
                           
                            leftHandleRect.x += twidth/2;
                            rightHandleRect.x -= twidth/2;
                        }
                        
                        minMaxSliderState.dragStartPos = num8;
                        minMaxSliderState.dragStartValue = value;
                        minMaxSliderState.dragStartSize = size;
                        minMaxSliderState.dragStartValuesPerPixel = num7;
                       
                        minMaxSliderState.whereWeDrag = !leftHandleRect.Contains(current.mousePosition) ? (!rightHandleRect.Contains(current.mousePosition) ? 0 : 2) : 1;

                        //minMaxSliderState.whereWeDrag = 0; // Drag entire slider
                        //minMaxSliderState.whereWeDrag = 1; // Drag start of slider
                        //minMaxSliderState.whereWeDrag = 2; // Drag end of slider

                        GUIUtility.hotControl = id;
                        current.Use();
                        break;
                    }

                    
                    
                    if (slider == GUIStyle.none)
                        break;
                    
                    break; // Disabled below, seems to correct slider if in bad state outside allowed ranges? or jumps slider to where cursor is
                    
                    if ((double) size != 0.0 & flag)
                    {
                        if (horiz)
                        {
                            if ((double) num8 > (double) position1.xMax - (double) position.x)
                                value += (float) ((double) size * (double) num5 * 0.899999976158142);
                            else
                                value -= (float) ((double) size * (double) num5 * 0.899999976158142);
                        }
                        else if ((double) num8 > (double) position1.yMax - (double) position.y)
                            value += (float) ((double) size * (double) num5 * 0.899999976158142);
                        else
                            value -= (float) ((double) size * (double) num5 * 0.899999976158142);

                        //minMaxSliderState.whereWeDrag = 0;
                        GUI.changed = true;
                        EditorGUIExt2.s_NextScrollStepTime = DateTime.Now.AddMilliseconds((double) EditorGUIExt2.kFirstScrollWait);
                        float num9 = horiz ? current.mousePosition.x : current.mousePosition.y;
                        float num10 = horiz ? position1.x : position1.y;
                        minMaxSliderState.whereWeDrag = (double) num9 > (double) num10 ? 4 : 3;
                    }
                    else
                    {
                        value = !horiz ? (float) (((double) num8 - (double) position1.height * 0.5) / (double) num7 + (double) min1 - (double) size * 0.5) : (float) (((double) num8 - (double) position1.width * 0.5) / (double) num7 + (double) min1 - (double) size * 0.5);
                        minMaxSliderState.dragStartPos = num8;
                        minMaxSliderState.dragStartValue = value;
                        minMaxSliderState.dragStartSize = size;
                        minMaxSliderState.dragStartValuesPerPixel = num7;
                        //minMaxSliderState.whereWeDrag = 0;
                        GUI.changed = true;
                    }

                    GUIUtility.hotControl = id;
                    //value = Mathf.Clamp(value, min2, num1 - size);
                    current.Use();
                    break;
                case UnityEngine.EventType.MouseUp:
                    if (GUIUtility.hotControl != id)
                        break;
                    current.Use();
                    GUIUtility.hotControl = 0;
                    break;
                case UnityEngine.EventType.MouseDrag:
                    //Debug.Log("Drag: " + id + ":" + minMaxSliderState.whereWeDrag);
                    if (GUIUtility.hotControl != id)
                        break;

                    float draggedAmount = (num8 - minMaxSliderState.dragStartPos) / minMaxSliderState.dragStartValuesPerPixel;

                    switch (minMaxSliderState.whereWeDrag)
                    {
                        case 0: // Drag whole slider
                            // Limit entire slider from leaving slider range
                            if (size<0)
                            {
                                //value = minMaxSliderState.dragStartValue;
                                //value = minMaxSliderState.dragStartValue + draggedAmount;
                                value = Mathf.Clamp(minMaxSliderState.dragStartValue + draggedAmount, min2-size, max2);
                            }
                            else
                            {
                                
                                value = Mathf.Clamp(minMaxSliderState.dragStartValue + draggedAmount, min2, max2 - size);
                            }
                            
                           //value = Mathf.Clamp(minMaxSliderState.dragStartValue + draggedAmount, 0, 1);

                            //value = minMaxSliderState.dragStartValue + draggedAmount;
                            break;
                        case 1: // Drag start of slider
                            value = minMaxSliderState.dragStartValue + draggedAmount;
                            size = minMaxSliderState.dragStartSize - draggedAmount;
                            // Stop dragging handle past other handle end causing inversion
                            //if ((double) value < (double) min2)
                            //{
                            //    size -= min2 - value;
                            //    value = min2;
                            //}
                            //if ((double) size < (double) num2)
                            //{
                            //    value -= num2 - size;
                            //    size = num2;
                            //    break;
                            //}
                            break;
                        case 2: // Drag end of slider
                            size = minMaxSliderState.dragStartSize + draggedAmount;
                            // Stop dragging handle past other handle end causing inversion
                            //if ((double) value + (double) size > (double) num1)
                            //{
                            //    size = num1 - value;
                            //}
                            //if ((double) size < (double) num2)
                            //{
                            //    size = num2;
                            //    break;
                            //}
                            break;
                    }

                    GUI.changed = true;
                    current.Use();
                    break;
                case UnityEngine.EventType.Repaint:
                    
                    GUI.color = new Color(11,11,11,0.2f);
                    slider.Draw(position, GUIContent.none, id); // Slider background line
               
                    if (size < 0)
                    {
                       // position1.size = new Vector2(position1.size.y, position1.size.x);
                       var twidth = 12;
                        //position1 = new Rect(position1.x+position1.width-twidth, position1.y,Mathf.Abs(position1.width)+ (twidth*2), position1.height);
                        leftHandleRect.x += twidth/2;
                        rightHandleRect.x -= twidth/2;
                        //GUI.color = new Color(0.7f,0.7f,1,0.8f);
                    }

                    Rect position1b = position1;
                    position1b.width -= 10;
                    position1b.position= new Vector2(position1b.position.x+5f,position1b.position.y);
                    if (size < 0)
                    {
                        GUI.color = new Color(0.3f, 0.3f, 1, 0.8f);
                        thumb.Draw(position1b, GUIContent.none, id); // Slider itself with handles
                    }

                    GUI.color = new Color(11,11,11,0.8f);
                    thumb.Draw(position1, GUIContent.none, id); // Slider itself with handles
                    GUI.color = new Color(1,1,1,1);
                    // Draws little hand cursor when hovering
                    EditorGUIUtility.AddCursorRect(leftHandleRect, horiz ? MouseCursor.ResizeHorizontal : MouseCursor.ResizeVertical, minMaxSliderState == null || minMaxSliderState.whereWeDrag != 1 ? -1 : id);
                    EditorGUIUtility.AddCursorRect(rightHandleRect, horiz ? MouseCursor.ResizeHorizontal : MouseCursor.ResizeVertical, minMaxSliderState == null || minMaxSliderState.whereWeDrag != 2 ? -1 : id);
                    if (GUIUtility.hotControl != id || !position.Contains(current.mousePosition) || (double) min1 - (double) max == 0.0)
                        break;
                    if (position1.Contains(current.mousePosition))
                    {
                        if (minMaxSliderState == null || minMaxSliderState.whereWeDrag != 3 && minMaxSliderState.whereWeDrag != 4)
                            break;
                        GUIUtility.hotControl = 0;
                        break;
                    }

                    if (DateTime.Now < EditorGUIExt2.s_NextScrollStepTime)
                        break;
                    int num12 = (horiz ? (double) current.mousePosition.x : (double) current.mousePosition.y) > (horiz ? (double) position1.x : (double) position1.y) ? 4 : 3;
                    if (minMaxSliderState != null && num12 != minMaxSliderState.whereWeDrag)
                        break;
                    if ((double) size != 0.0 & flag)
                    {
                        if (horiz)
                        {
                            if ((double) num8 > (double) position1.xMax - (double) position.x)
                                value += (float) ((double) size * (double) num5 * 0.899999976158142);
                            else
                                value -= (float) ((double) size * (double) num5 * 0.899999976158142);
                        }
                        else if ((double) num8 > (double) position1.yMax - (double) position.y)
                            value += (float) ((double) size * (double) num5 * 0.899999976158142);
                        else
                            value -= (float) ((double) size * (double) num5 * 0.899999976158142);

                        if (minMaxSliderState != null)
                            minMaxSliderState.whereWeDrag = -1;
                        GUI.changed = true;
                    }

                    //value = Mathf.Clamp(value, min2, max2 - size);
                    EditorGUIExt2.s_NextScrollStepTime = DateTime.Now.AddMilliseconds((double) EditorGUIExt2.kScrollWait);
                    break;
            }
     
            
            
        }

        public static bool DragSelection(Rect[] positions, ref bool[] selections, GUIStyle style)
        {
            int controlId = GUIUtility.GetControlID(34553287, FocusType.Keyboard);
            UnityEngine.Event current = UnityEngine.Event.current;
            int b = -1;
            for (int index = positions.Length - 1; index >= 0; --index)
            {
                if (positions[index].Contains(current.mousePosition))
                {
                    b = index;
                    break;
                }
            }

            switch (current.GetTypeForControl(controlId))
            {
                case UnityEngine.EventType.MouseDown:
                    if (current.button == 0 && b >= 0)
                    {
                        GUIUtility.keyboardControl = 0;
                        bool flag1 = false;
                        if (selections[b])
                        {
                            int num = 0;
                            foreach (bool flag2 in selections)
                            {
                                if (flag2)
                                {
                                    ++num;
                                    if (num > 1)
                                        break;
                                }
                            }

                            if (num == 1)
                                flag1 = true;
                        }

                        if (!current.shift && !EditorGUI.actionKey)
                        {
                            for (int index = 0; index < positions.Length; ++index)
                                selections[index] = false;
                        }

                        EditorGUIExt2.initIndex = b;
                        EditorGUIExt2.initSelections = (bool[]) selections.Clone();
                        EditorGUIExt2.adding = true;
                        if ((current.shift || EditorGUI.actionKey) && selections[b])
                            EditorGUIExt2.adding = false;
                        selections[b] = !flag1 && EditorGUIExt2.adding;
                        GUIUtility.hotControl = controlId;
                        current.Use();
                        return true;
                    }

                    break;
                case UnityEngine.EventType.MouseUp:
                    if (GUIUtility.hotControl == controlId)
                    {
                        GUIUtility.hotControl = 0;
                        break;
                    }

                    break;
                case UnityEngine.EventType.MouseDrag:
                    if (GUIUtility.hotControl == controlId && current.button == 0)
                    {
                        if (b < 0)
                        {
                            Rect rect = new Rect(positions[0].x, positions[0].y - 200f, positions[0].width, 200f);
                            if (rect.Contains(current.mousePosition))
                                b = 0;
                            rect.y = positions[positions.Length - 1].yMax;
                            if (rect.Contains(current.mousePosition))
                                b = selections.Length - 1;
                        }

                        if (b < 0)
                            return false;
                        int num1 = Mathf.Min(EditorGUIExt2.initIndex, b);
                        int num2 = Mathf.Max(EditorGUIExt2.initIndex, b);
                        for (int index = 0; index < selections.Length; ++index)
                        {
                            int num3 = index < num1 ? 0 : (index <= num2 ? 1 : 0);
                            selections[index] = num3 == 0 ? EditorGUIExt2.initSelections[index] : EditorGUIExt2.adding;
                        }

                        current.Use();
                        return true;
                    }

                    break;
                case UnityEngine.EventType.Repaint:
                    for (int index = 0; index < positions.Length; ++index)
                        style.Draw(positions[index], GUIContent.none, controlId, selections[index]);
                    break;
            }

            return false;
        }

        private static bool Any(bool[] selections)
        {
            for (int index = 0; index < selections.Length; ++index)
            {
                if (selections[index])
                    return true;
            }

            return false;
        }

        internal enum HighLevelEvent
        {
            None,
            Click,
            DoubleClick,
            ContextClick,
            BeginDrag,
            Drag,
            EndDrag,
            Delete,
            SelectionChanged,
            Copy,
            Paste,
        }

        public static HighLevelEvent MultiSelection(
            Rect rect,
            Rect[] positions,
            GUIContent content,
            Rect[] hitPositions,
            ref bool[] selections,
            bool[] readOnly,
            out int clickedIndex,
            out Vector2 offset,
            out float startSelect,
            out float endSelect,
            GUIStyle style)
        {
            int controlId = GUIUtility.GetControlID(41623453, FocusType.Keyboard);
            UnityEngine.Event current = UnityEngine.Event.current;
            offset = Vector2.zero;
            clickedIndex = -1;
            startSelect = endSelect = 0.0f;
            if (current.type == UnityEngine.EventType.Used)
                return HighLevelEvent.None;
            bool flag1 = false;
            if (UnityEngine.Event.current.type != UnityEngine.EventType.Layout && GUIUtility.keyboardControl == controlId)
                flag1 = true;
            switch (current.GetTypeForControl(controlId))
            {
                case UnityEngine.EventType.MouseDown:
                    if (current.button == 0)
                    {
                        GUIUtility.hotControl = controlId;
                        GUIUtility.keyboardControl = controlId;
                        EditorGUIExt2.s_StartSelectPos = current.mousePosition;
                        int indexUnderMouse = EditorGUIExt2.GetIndexUnderMouse(hitPositions, readOnly);
                        if (UnityEngine.Event.current.clickCount == 2 && indexUnderMouse >= 0)
                        {
                            for (int index = 0; index < selections.Length; ++index)
                                selections[index] = false;
                            selections[indexUnderMouse] = true;
                            current.Use();
                            clickedIndex = indexUnderMouse;
                            return HighLevelEvent.DoubleClick;
                        }

                        if (indexUnderMouse >= 0)
                        {
                            if (!current.shift && !EditorGUI.actionKey && !selections[indexUnderMouse])
                            {
                                for (int index = 0; index < hitPositions.Length; ++index)
                                    selections[index] = false;
                            }

                            int num = current.shift ? 1 : (EditorGUI.actionKey ? 1 : 0);
                            selections[indexUnderMouse] = num == 0 || !selections[indexUnderMouse];
                            EditorGUIExt2.s_MouseDownPos = current.mousePosition;
                            EditorGUIExt2.s_MultiSelectDragSelection = EditorGUIExt2.DragSelectionState.None;
                            current.Use();
                            clickedIndex = indexUnderMouse;
                            return HighLevelEvent.SelectionChanged;
                        }

                        bool flag2;
                        if (!current.shift && !EditorGUI.actionKey)
                        {
                            for (int index = 0; index < hitPositions.Length; ++index)
                                selections[index] = false;
                            flag2 = true;
                        }
                        else
                            flag2 = false;

                        EditorGUIExt2.s_SelectionBackup = new List<bool>((IEnumerable<bool>) selections);
                        EditorGUIExt2.s_LastFrameSelections = new List<bool>((IEnumerable<bool>) selections);
                        EditorGUIExt2.s_MultiSelectDragSelection = EditorGUIExt2.DragSelectionState.DragSelecting;
                        current.Use();
                        return flag2 ? HighLevelEvent.SelectionChanged : HighLevelEvent.None;
                    }

                    break;
                case UnityEngine.EventType.MouseUp:
                    if (GUIUtility.hotControl == controlId)
                    {
                        GUIUtility.hotControl = 0;
                        if (EditorGUIExt2.s_StartSelectPos != current.mousePosition)
                            current.Use();
                        if (EditorGUIExt2.s_MultiSelectDragSelection == EditorGUIExt2.DragSelectionState.None)
                        {
                            clickedIndex = EditorGUIExt2.GetIndexUnderMouse(hitPositions, readOnly);
                            if (current.clickCount == 1)
                                return HighLevelEvent.Click;
                            break;
                        }

                        EditorGUIExt2.s_MultiSelectDragSelection = EditorGUIExt2.DragSelectionState.None;
                        EditorGUIExt2.s_SelectionBackup = (List<bool>) null;
                        EditorGUIExt2.s_LastFrameSelections = (List<bool>) null;
                        return HighLevelEvent.EndDrag;
                    }

                    break;
                case UnityEngine.EventType.MouseDrag:
                    if (GUIUtility.hotControl == controlId)
                    {
                        if (EditorGUIExt2.s_MultiSelectDragSelection == EditorGUIExt2.DragSelectionState.DragSelecting)
                        {
                            float num1 = Mathf.Min(EditorGUIExt2.s_StartSelectPos.x, current.mousePosition.x);
                            float num2 = Mathf.Max(EditorGUIExt2.s_StartSelectPos.x, current.mousePosition.x);
                            EditorGUIExt2.s_SelectionBackup.CopyTo(selections);
                            for (int index = 0; index < hitPositions.Length; ++index)
                            {
                                if (!selections[index])
                                {
                                    float num3 = hitPositions[index].x + hitPositions[index].width * 0.5f;
                                    if ((double) num3 >= (double) num1 && (double) num3 <= (double) num2)
                                        selections[index] = true;
                                }
                            }

                            current.Use();
                            startSelect = num1;
                            endSelect = num2;
                            bool flag2 = false;
                            for (int index = 0; index < selections.Length; ++index)
                            {
                                if (selections[index] != EditorGUIExt2.s_LastFrameSelections[index])
                                {
                                    flag2 = true;
                                    EditorGUIExt2.s_LastFrameSelections[index] = selections[index];
                                }
                            }

                            return flag2 ? HighLevelEvent.SelectionChanged : HighLevelEvent.None;
                        }

                        offset = current.mousePosition - EditorGUIExt2.s_MouseDownPos;
                        current.Use();
                        if (EditorGUIExt2.s_MultiSelectDragSelection != EditorGUIExt2.DragSelectionState.None)
                            return HighLevelEvent.Drag;
                        EditorGUIExt2.s_MultiSelectDragSelection = EditorGUIExt2.DragSelectionState.Dragging;
                        return HighLevelEvent.BeginDrag;
                    }

                    break;
                case UnityEngine.EventType.KeyDown:
                    if (flag1 && (current.keyCode == KeyCode.Backspace || current.keyCode == KeyCode.Delete))
                    {
                        current.Use();
                        return HighLevelEvent.Delete;
                    }

                    break;
                case UnityEngine.EventType.Repaint:
                    if (GUIUtility.hotControl == controlId && EditorGUIExt2.s_MultiSelectDragSelection == EditorGUIExt2.DragSelectionState.DragSelecting)
                    {
                        float num1 = Mathf.Min(EditorGUIExt2.s_StartSelectPos.x, current.mousePosition.x);
                        float num2 = Mathf.Max(EditorGUIExt2.s_StartSelectPos.x, current.mousePosition.x);
                        Rect position = new Rect(0.0f, 0.0f, rect.width, rect.height);
                        position.x = num1;
                        position.width = num2 - num1;
                        if ((double) position.width > 1.0)
                            GUI.Box(position, "", EditorGUIExt2.ms_Styles.selectionRect);
                    }

                    Color color = GUI.color;
                    for (int index = 0; index < positions.Length; ++index)
                    {
                        GUI.color = readOnly == null || !readOnly[index] ? (!selections[index] ? color * new Color(0.9f, 0.9f, 0.9f, 1f) : color * new Color(0.3f, 0.55f, 0.95f, 1f)) : color * new Color(0.9f, 0.9f, 0.9f, 0.5f);
                        style.Draw(positions[index], content, controlId, selections[index]);
                    }

                    GUI.color = color;
                    break;
                case UnityEngine.EventType.ValidateCommand:
                case UnityEngine.EventType.ExecuteCommand:
                    if (flag1)
                    {
                        bool flag2 = current.type == UnityEngine.EventType.ExecuteCommand;
                        string commandName = current.commandName;
                        if (!(commandName == "Delete"))
                        {
                            if (!(commandName == "Copy"))
                            {
                                if (commandName == "Paste")
                                {
                                    current.Use();
                                    if (flag2)
                                        return HighLevelEvent.Paste;
                                }
                            }
                            else
                            {
                                current.Use();
                                if (flag2)
                                    return HighLevelEvent.Copy;
                            }
                        }
                        else
                        {
                            current.Use();
                            if (flag2)
                                return HighLevelEvent.Delete;
                        }

                        break;
                    }

                    break;
                case UnityEngine.EventType.ContextClick:
                    int indexUnderMouse1 = EditorGUIExt2.GetIndexUnderMouse(hitPositions, readOnly);
                    if (indexUnderMouse1 >= 0)
                    {
                        clickedIndex = indexUnderMouse1;
                        GUIUtility.keyboardControl = controlId;
                        current.Use();
                        return HighLevelEvent.ContextClick;
                    }

                    break;
            }

            return HighLevelEvent.None;
        }

        private static int GetIndexUnderMouse(Rect[] hitPositions, bool[] readOnly)
        {
            Vector2 mousePosition = UnityEngine.Event.current.mousePosition;
            for (int index = hitPositions.Length - 1; index >= 0; --index)
            {
                if ((readOnly == null || !readOnly[index]) && hitPositions[index].Contains(mousePosition))
                    return index;
            }

            return -1;
        }

        internal static Rect FromToRect(Vector2 start, Vector2 end)
        {
            Rect rect = new Rect(start.x, start.y, end.x - start.x, end.y - start.y);
            if ((double) rect.width < 0.0)
            {
                rect.x += rect.width;
                rect.width = -rect.width;
            }

            if ((double) rect.height < 0.0)
            {
                rect.y += rect.height;
                rect.height = -rect.height;
            }

            return rect;
        }

        private class Styles
        {
            public GUIStyle selectionRect = (GUIStyle) "SelectionRect";
        }

        private class MinMaxSliderState
        {
            public float dragStartPos = 0.0f;
            public float dragStartValue = 0.0f;
            public float dragStartSize = 0.0f;
            public float dragStartValuesPerPixel = 0.0f;
            public float dragStartLimit = 0.0f;
            public float dragEndLimit = 0.0f;
            public int whereWeDrag = -1;
        }

        private enum DragSelectionState
        {
            None,
            DragSelecting,
            Dragging,
        }
    }
}
