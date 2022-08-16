using UnityEngine;
using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/ToggleObjects")]
	public class ToggleObjects : MonoBehaviour
    {
        public List<Renderer> renderers = new List<Renderer>();
        public List<Behaviour> behaviours = new List<Behaviour>();
        public List<GameObject> gameObjects = new List<GameObject>();

        public bool updateOnAndroidExport = true;

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
    }
}