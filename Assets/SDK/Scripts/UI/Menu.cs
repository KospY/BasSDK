using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad
{
    public class Menu : MonoBehaviour
    {
        public Transform page1;
        public Transform page2;
        public List<CustomReference> customReferences;

        public bool TryGetCustomReference<T>(string name, out T custom) where T : Component
        {
            custom = GetCustomReference<T>(name, false);
            return custom != null;
        }

        public T GetCustomReference<T>(string name, bool printError = true) where T : Component
        {
            CustomReference customReference = customReferences.Find(cr => cr.name == name);
            if (customReference != null)
            {
                if (customReference.transform is T) return (T)customReference.transform;
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
    }
}