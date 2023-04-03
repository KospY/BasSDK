using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UIMenu : MonoBehaviour
    {
        public RawImage icon;
        public Transform navigationPane;
        public Transform contentArea;
        public List<CustomReference> customReferences;

        public Toggle MenuToggle { get; private set; }
        public List<Toggle> NavigationPaneToggles { get; private set; }

        public bool TryGetCustomReference<T>(string name, out T custom) where T : Component
        {
            custom = GetCustomReference<T>(name, false);
            return custom != null;
        }

        public T GetCustomReference<T>(string name, bool printError = true) where T : Component
        {
            var customReference = customReferences.Find(cr => cr.name == name);
            if (customReference != null)
            {
                if (customReference.transform is T component) return component;
                if (typeof(T) == typeof(Transform)) return customReference.transform.transform as T;
                return customReference.transform.GetComponent<T>();
            }
            else
            {
                if (printError) Debug.LogError("Cannot find menu custom reference " + name);
                return null;
            }
        }

        public Transform GetCustomReference(string name, bool printError = true) => GetCustomReference<Transform>(name, printError);

        public void SetToggleButtons(Toggle menuToggle, List<Toggle> navigationPaneToggles)
        {
            MenuToggle = menuToggle;
            NavigationPaneToggles = new List<Toggle>(navigationPaneToggles);
        }
    }
}