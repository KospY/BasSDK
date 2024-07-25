using UnityEngine;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/ToggleObjects.html")]
	public class ToggleObjects : MonoBehaviour
    {
        [Tooltip("List of renderers that you want to toggle on/off")]
        public List<Renderer> renderers = new List<Renderer>();
        [Tooltip("List of behaviours that you want to toggle on/off")]
        public List<Behaviour> behaviours = new List<Behaviour>();
        [Tooltip("List of gameObjects that you want to toggle on/off")]
        public List<GameObject> gameObjects = new List<GameObject>();

        [Tooltip("Update the renderers/gameobjects to LOD1 when you export to android.")]
        public bool updateOnAndroidExport = true;

        [Button]
        public void Toggle()
        {
            foreach (Renderer renderer in renderers)
            {
                if (renderer) renderer.enabled = !renderer.enabled;
            }
            foreach (Behaviour behaviour in behaviours)
            {
                if (behaviour) behaviour.enabled = !behaviour.enabled;
            }
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject) gameObject.SetActive(!gameObject.activeSelf);
            }
        }

        [Button]
        public void Toggle(bool enabled)
        {
            foreach (Renderer renderer in renderers)
            {
                if (renderer) renderer.enabled = enabled;
            }
            foreach (Behaviour behaviour in behaviours)
            {
                if (behaviour) behaviour.enabled = enabled;
            }
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject) gameObject.SetActive(enabled);
            }
        }

        [Button]
        public void GetChildRenderers()
        {
            GetChildRenderers(this.gameObject);
        }

        [Button]
        protected void GetChildRenderers(GameObject gameObject)
        {
            renderers.Clear();
            foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
            {
                if (renderer.hideFlags.HasFlag(HideFlags.DontSaveInEditor)) continue;
                if (!renderer.enabled) continue;
                renderers.Add(renderer);
            }
        }

        [Button]
        protected void CopyGameObjectsToRenderers()
        {
            foreach (GameObject gameObject in gameObjects)
            {
                foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
                {
                    if (renderer.hideFlags.HasFlag(HideFlags.DontSaveInEditor)) continue;
                    if (!renderer.enabled) continue;
                    renderers.Add(renderer);
                }
            }
        }
    }
}