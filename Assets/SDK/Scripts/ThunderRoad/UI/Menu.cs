using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad
{
    public class Menu : MonoBehaviour
    {
        public Transform page1;
        public Transform page2;
        public List<CustomReference> customReferences;

        public Transform GetCustomReference(string name)
        {
            CustomReference customReference = customReferences.Find(cr => cr.name == name);
            if (customReference != null)
            {
                return customReference.transform;
            }
            else
            {
                Debug.LogError("Cannot find menu custom reference " + name);
                return null;
            }
        }
    }
}