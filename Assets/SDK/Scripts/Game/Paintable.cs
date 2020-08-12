using UnityEngine;
using System.Collections.Generic;
using System;

namespace ThunderRoad
{
    public class Paintable : MonoBehaviour
    {
        public List<MaterialProperty> materialProperties = new List<MaterialProperty>();
        public bool cloneMaterial;


        [Serializable]
        public class MaterialProperty
        {
            public int materialIndex = 0;
            public PropertyType propertyType = PropertyType.Base;
            public string propertyName = "_BaseMap";
            public enum PropertyType
            {
                Base,
                Normal,
                Emission,
            }

            public MaterialProperty Clone()
            {
                return MemberwiseClone() as MaterialProperty;
            }
        }

    }
}