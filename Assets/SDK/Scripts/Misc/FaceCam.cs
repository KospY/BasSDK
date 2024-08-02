#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using UnityEngine;
using UnityEngine.Serialization;

namespace ThunderRoad
{
    public class FaceCam : MonoBehaviour
    {
        public bool useWorldUp = false;
        public bool fadeOut;
        [ShowIf("fadeOut"), MinMaxSlider(0, 3)]
        public Vector2 fadeOutDistance = new Vector2(0.5f, 1.5f);
        [FormerlySerializedAs("targetRenderer")]
        [FormerlySerializedAs("renderer")]
        [ShowIf("fadeOut")]
        public Renderer fadeRenderer;

        private Transform mainCameraTransform;
        private Transform cachedTransform;

        private void Awake()
        {
            cachedTransform = transform;
            fadeRenderer ??= GetComponent<Renderer>();
        }

        private void Update()
        {
            if (mainCameraTransform == null)
            {
                if (Camera.main != null)
                {
                    mainCameraTransform = Camera.main.transform;
                }
            }
            else
            {
                cachedTransform.LookAt(2 * cachedTransform.position - mainCameraTransform.position, useWorldUp ? Vector3.up : mainCameraTransform.up);
            }

            if (!mainCameraTransform || !fadeOut || !fadeRenderer) return;

            float alpha = Mathf.InverseLerp(fadeOutDistance.y, fadeOutDistance.x,
                Vector3.Distance(transform.position, mainCameraTransform.position));
            switch (fadeRenderer)
            {
                case SpriteRenderer sprite:
                    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
                    break;
                case MeshRenderer mesh:
                    mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g, mesh.material.color.b,
                        alpha);
                    break;
            }
        }
    }
}
