using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
    [RequireComponent(typeof(BoxCollider))]
    public class KeyButton : ThunderBehaviour
    {
        public string keyId;
        public Canvas canvas;
        public TextMeshPro textMesh;

        private void OnValidate()
        {
            //add a canvas for images
            if (gameObject.TryGetOrAddComponentInChildren<Canvas>(out var foundCanvas))
            {
                this.canvas = foundCanvas;
                foundCanvas.name = "Canvas";
                if (!foundCanvas.TryGetComponent<Image>(out var image))
                {
                    image = this.canvas.gameObject.AddComponent<Image>();
                    image.color = Color.clear;
                }
            }

            //add a textMeshPro for text
            if (gameObject.TryGetOrAddComponentInChildren<TextMeshPro>(out var foundTextMesh))
            {
                textMesh = foundTextMesh;
                textMesh.name = "TextMeshPro";
            }

            if (string.IsNullOrEmpty(keyId))
            {
                Debug.LogError($"Keyboard Key {this.name} does not have an ID set. Please set an ID and map it to a valid KeyboardData", this);
            }
        }
    }
}
