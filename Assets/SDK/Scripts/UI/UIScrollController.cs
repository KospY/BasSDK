using System;
using UnityEngine;

namespace ThunderRoad
{
    public class UIScrollController : MonoBehaviour
    {
        [SerializeField] private float maxVerticalDisplacement = 30f;
        [SerializeField] private Transform layoutGroup;
        
        private float previousGridPosition;
        private float resetGridPosition;

        private void OnEnable()
        {
            previousGridPosition = layoutGroup.localPosition.y;
        }

        // Delegate for the event ScrollRect.OnValueChanged(Vector2 value)
        public void LayoutGroupValueChanged()
        {
            var displacementAttempt = previousGridPosition - layoutGroup.localPosition.y;
            var absoluteDisplacement = Math.Abs(displacementAttempt);

            // Slow down scrolling speed (discard no significant movement at the extremes of the grid)
            if (absoluteDisplacement > 0.05f && absoluteDisplacement > maxVerticalDisplacement)
            {
                var displacement = displacementAttempt < 0 ? -maxVerticalDisplacement : maxVerticalDisplacement;

                layoutGroup.localPosition = new Vector2(layoutGroup.localPosition.x, previousGridPosition - displacement);
            }

            previousGridPosition = layoutGroup.transform.localPosition.y;
        }

        public void ResetPosition()
        {
            layoutGroup.localPosition = new Vector2(layoutGroup.localPosition.x, resetGridPosition);
            previousGridPosition = layoutGroup.localPosition.y;
        }

        public void UpdateResetPosition()
        {
            resetGridPosition = layoutGroup.localPosition.y;
        }
    }
}