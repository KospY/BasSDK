using System;
using UnityEngine;
using UnityEngine.UI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class UIScrollController : ScrollRect
    {
        public float verticalScrollSize;

        [Tooltip("Adjust the position of the content if the auto-hide option is enabled for the vertical and/or the horizontal scrollbars.")]
        public bool adjustContentPosition;
#if ODIN_INSPECTOR
        [ShowIf("adjustContentPosition")]
#endif
        [Tooltip("Set the left/right values for the vertical scrollbar and the top/bottom values for the horizontal scrollbar.")]
        public RectOffset noScrollbarsPadding;

        public bool controlVerticalSpeed;
#if ODIN_INSPECTOR
        [ShowIf("controlVerticalSpeed")]
#endif
        [Tooltip("Max vertical scroll speed: Maximum vertical pixels that can be displaced in one frame.")]
        public float maxVerticalDisplacement = 30f;

        private LayoutGroup contentLayoutGroup;
        private float previousGridPosition;
        private Vector2 resetGridPosition;
        private RectOffset defaultGridOffset;

        protected new void Awake()
        {
            base.Awake();

            contentLayoutGroup = content.GetComponent<LayoutGroup>();
            if (contentLayoutGroup != null)
            {
                var defaultPadding = contentLayoutGroup.padding;
                defaultGridOffset = new RectOffset(defaultPadding.left, defaultPadding.right, defaultPadding.top, defaultPadding.bottom);
            }
        }

        protected new void Start()
        {
            base.Start();

            if (verticalScrollbar != null)
            {
                // Keep the vertical scroll size constant
                onValueChanged.AddListener(delegate
                {
                    SetVerticalScrollBarSize(verticalScrollSize);
                });
            }

            UpdateResetPosition();
        }

        protected new void OnEnable()
        {
            base.OnEnable();

            previousGridPosition = content.localPosition.y;
        }

        protected new void LateUpdate()
        {
            base.LateUpdate();

            // Check if we want to adjust the scroll content position, in case some of the scrollbars
            // was auto-enabled or auto-disabled after adding or removing content from the scroll
            if (adjustContentPosition && contentLayoutGroup != null)
            {
                AdjustContentPosition();
            }
        }

        /// <summary>
        /// Unity UI default behavior changes the scrollbar size while we are scrolling, which messes up the scroll handle
        /// position, so we need to set the scrollbar size, each time the scroll value changes, to keep its size constant.
        /// </summary>
        /// <param name="size">Size of the vertical scroll</param>
        private void SetVerticalScrollBarSize(float size)
        {
            if (verticalScrollbar == null)
            {
                Debug.LogError("No vertical scroll bar assigned to the scroll: " + name);
                return;
            }

            verticalScrollbar.size = size;
        }

        /// <summary>
        /// Adjust the scroll content position, according to the scrollbars visibility,
        /// if the visibility setting is set to Auto-hide
        /// </summary>
        private void AdjustContentPosition()
        {
            var updatedPosition = false;
            // Check horizontal padding
            if (verticalScrollbar.isActiveAndEnabled)
            {
                if (contentLayoutGroup.padding.left != defaultGridOffset.left)
                {
                    updatedPosition = true;
                    contentLayoutGroup.padding.left = defaultGridOffset.left;
                    contentLayoutGroup.padding.right = defaultGridOffset.right;
                }
            }
            else
            {
                if (contentLayoutGroup.padding.left != noScrollbarsPadding.left)
                {
                    updatedPosition = true;
                    contentLayoutGroup.padding.left = noScrollbarsPadding.left;
                    contentLayoutGroup.padding.right = noScrollbarsPadding.right;
                }
            }

            // Check vertical padding
            if (horizontalScrollbar.isActiveAndEnabled)
            {
                if (contentLayoutGroup.padding.top != defaultGridOffset.top)
                {
                    updatedPosition = true;
                    contentLayoutGroup.padding.top = defaultGridOffset.top;
                    contentLayoutGroup.padding.bottom = defaultGridOffset.bottom;
                }
            }
            else
            {
                if (contentLayoutGroup.padding.top != noScrollbarsPadding.top)
                {
                    updatedPosition = true;
                    contentLayoutGroup.padding.top = noScrollbarsPadding.top;
                    contentLayoutGroup.padding.bottom = noScrollbarsPadding.bottom;
                }
            }

            if(updatedPosition)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(content);
            }
        }

        /// <summary>
        /// ScrollRect.LateUpdate calls SetContentAnchoredPosition with very tiny values every frame,
        /// only if scrolling is not needed and even when velocity is zero.
        /// SetContentAnchoredPosition makes text jitter. Check before setting position.
        /// </summary>
        protected override void SetContentAnchoredPosition(Vector2 position)
        {
            if (Application.isPlaying && verticalScrollbar != null && !verticalScrollbar.IsActive())
            {
                if (content.anchoredPosition == Vector2.zero) return;

                position = Vector2.zero;
            }

            // fires every frame in editor, but without text jitters
            if (position != Vector2.zero && Approximately(content.anchoredPosition, position) && Approximately(position, Vector2.zero))
            {
                position = Vector2.zero;
            }

            base.SetContentAnchoredPosition(position);
        }

        /// <summary>
        /// Called when scrolling would occur.
        /// Prevent setting when vertical scrollbar is disabled and scrolling is not needed to prevent jittering.
        /// </summary>
        protected override void SetNormalizedPosition(float value, int axis)
        {
            if (Application.isPlaying && verticalScrollbar != null && !verticalScrollbar.IsActive()) return;

            base.SetNormalizedPosition(value, axis);

            // Check scroll speed
            LayoutGroupValueChanged();
        }

        private static bool Approximately(Vector2 vec1, Vector2 vec2, float threshold = 0.01f)
        {
            return ((vec1.x < vec2.x) ? (vec2.x - vec1.x) : (vec1.x - vec2.x)) <= threshold
                   && ((vec1.y < vec2.y) ? (vec2.y - vec1.y) : (vec1.y - vec2.y)) <= threshold;
        }

        /// <summary>
        /// Control scroll max speed. The scroll rect default behavior is to increase the scroll speed proportionally
        /// to the amount of content present in the scroll. This means that if the scroll has a lot of content, it is
        /// almost impossible to scroll small portions at a time.
        /// </summary>
        public void LayoutGroupValueChanged()
        {
            if (controlVerticalSpeed)
            {
                var displacementAttempt = previousGridPosition - content.localPosition.y;
                var absoluteDisplacement = Math.Abs(displacementAttempt);

                // Slow down scrolling speed (discard no significant movement at the extremes of the grid)
                if (absoluteDisplacement > 0.05f && absoluteDisplacement > maxVerticalDisplacement)
                {
                    var displacement = displacementAttempt < 0 ? -maxVerticalDisplacement : maxVerticalDisplacement;
                    content.localPosition = new Vector2(resetGridPosition.x, previousGridPosition - displacement);
                }

                previousGridPosition = content.transform.localPosition.y;
            }

            // Disable horizontal movement (even though the scrolls don't have an horizontal scrollbar)
            content.localPosition = new Vector2(resetGridPosition.x, content.transform.localPosition.y);
        }

        public void ResetPosition()
        {
            content.localPosition = resetGridPosition;
            previousGridPosition = content.localPosition.y;
        }

        public void UpdateResetPosition()
        {
            resetGridPosition = new Vector2(content.localPosition.x, content.localPosition.y);
        }
    }
}