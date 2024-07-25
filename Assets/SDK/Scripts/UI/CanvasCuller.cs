using System.Collections;
using UnityEngine;

namespace ThunderRoad
{
    [RequireComponent(typeof(Collider))]
    public class CanvasCuller : ThunderBehaviour
    {
        public float fadeDuration = 0.5f;
        public float colliderSize = 3f;
        public Collider collider;
        public CanvasGroup canvasGroup;
        
    }
}
