using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad.Manikin
{
    /// <summary>
    /// Properties is a hierarchial system the stores a list of properties and propagates them down to children properties.
    /// </summary>
    [System.Serializable]
    public struct ManikinProperty
    {
        [System.Serializable]
        public enum PropertyType
        {
            Float_Signed,
            Float_Unsigned,
            Float_Unclamped,
            Int, //this just gets casted to a float in the shader
            Float4,
            Color,
            Color_HDR,
            Material
        }

        public PropertyType propertyType;
        [Tooltip("Apply these properties to the attached Renderer or simply store the values.")]
        public bool apply;
        public float[] values;
        public ManikinPropertySet set;
        [Tooltip("Bitmask of corresponding material index to apply this property to.")]
        public int materialIndices;

        public object payload;

        public void ApplyProperties(GameObject obj, bool useSRPBatcher, Renderer renderer)
        {
            if (set == null)
            {
                //TODO, hacky, check if we have a parent, then we know this is a prefab on it's own and not to display the error.
                if (obj.transform.parent != null)
                {
                    Debug.LogError($"Missing ManikinPropertySet on ManikinPartProperty! {obj.transform.parent.name}/{obj.name}");
                }
                return;
            }

            if (!apply) return;

            switch (materialIndices)
            {
                case 0:
                    set.ApplyProperties(obj, values, useSRPBatcher, renderer, 0, payload);
                    break;
                case -1:
                    for (int bitIndex = 0; bitIndex < 31; bitIndex++)
                    {
                        set.ApplyProperties(obj, values, useSRPBatcher, renderer, bitIndex, payload);
                    }
                    break;
                default:
                    for (int bitIndex = 0; bitIndex < 31; bitIndex++)
                    {
                        if ((materialIndices & (1 << bitIndex)) != 0)
                        {
                            set.ApplyProperties(obj, values, useSRPBatcher, renderer, bitIndex, payload);
                        }
                    }
                    break;
            }
            
        }

        public bool[] MaterialIndicesMaskToBools()
        {
            bool[] flags = new bool[32];

            for (int bitIndex = 0; bitIndex < 32; bitIndex++)
            {
                if ((materialIndices & (1 << bitIndex)) != 0)
                {
                    flags[bitIndex] = true;
                }
            }
            return flags;
        }
    }

    [ExecuteInEditMode]
    public class ManikinProperties : MonoBehaviour
    {
        //Wrapper because JSONUtility can't serialize arrays directly.
        [System.Serializable]
        public struct JsonProperties
        {
            public ManikinProperty[] properties;
        }

        public bool useSRPBatcher = true;
        public ManikinProperty[] properties;

        public bool applyBitmaskEveryUpdate = false;

        public string occlusionID;
        public int occlusionIDHash;
        public int occlusionBitmask;

        public string occlusionBitmaskProperty = "_Bitmask";
        private int occlusionBitmaskPropertyID = -1;

        private MaterialPropertyBlock materialPropertyBlock;

        public Dictionary<int, int> occlusionBitmasks = new Dictionary<int, int>(); //only used by the parent properties.

        public delegate void PropertiesUpdatedHandler();
        public event PropertiesUpdatedHandler PropertiesUpdated;

        [SerializeField]
        private Renderer _renderer;
        [SerializeField]
        private ManikinProperties parentProperties;
        [SerializeField]
        private List<ManikinProperties> childrenProperties = new List<ManikinProperties>();
        private MaterialInstance materialInstance;


#if UNITY_EDITOR
        private void OnValidate()
        {
            //if (_renderer == null) { _renderer = GetComponent<Renderer>(); }
            _renderer = GetComponent<Renderer>();

            if (parentProperties == null)
            {
                parentProperties = FindManikinPropertiesInParent(transform.parent);
                if (parentProperties != null)
                {
                    //Tell the parent about our properties
                    parentProperties.AddPartPropertiesToChildrenProperties(this);
                }
            }


            occlusionIDHash = Animator.StringToHash(occlusionID);
        }

#endif
        protected void RemoveParentReference()
        {
            parentProperties = null;
        }

        private void PartList_UpdatePartsCompleted(ManikinPart[] prefabPartsAdded)
        {
            UpdateProperties();
        }

        private ManikinProperties FindManikinPropertiesInParent(Transform obj)
        {
            while (true)
            {
                if (obj == null) return null;
                if (obj.TryGetComponent(out ManikinProperties props)) return props;
                obj = obj.parent;
            }
        }

        public void AddPartPropertiesToChildrenProperties(ManikinProperties partProperty)
        {
            if (childrenProperties != null)
            {
                if (!childrenProperties.Contains(partProperty))
                {
                    childrenProperties.Add(partProperty);
                    //UpdateProperties();
                }
            }
        }

        public void RemovePartPropertiesFromChildrenProperties(ManikinProperties partProperty)
        {
            if (childrenProperties != null)
            {
                if (childrenProperties.Contains(partProperty))
                {
                    childrenProperties.Remove(partProperty);
                    //UpdateProperties(); //is this actually needed?
                }
            }
        }

        public void ApplyOcclusionBitmask()
        {
            if (occlusionBitmaskPropertyID != -1 && _renderer != null)
            {
                if (materialInstance == null)
                {
                    materialInstance = _renderer.GetComponent<MaterialInstance>();
                }

                if (materialInstance != null)
                {
                    Material[] materials = materialInstance.materials; //_renderer.materialInstances();
                    if (materials != null)
                    {
                        foreach (Material mat in materials)
                        {
                            if (mat != null)
                            {
                                mat.SetFloat(occlusionBitmaskPropertyID, occlusionBitmask);
                            }
                        }
                    }
                }
            }
        }

        public void UpdateProperties()
        {
#if UNITY_EDITOR
            //Temp, for now, don't apply any properties if this component isn't an instance. 
            //Properties values should be able to still cascade downward, just not get applied
            //to the material because it has no instance.
            if (!Application.isPlaying && !UnityEditor.PrefabUtility.IsPartOfPrefabInstance(this))
            {
                Debug.LogWarning("Returning because not part of prefab instance: " + name);
                return;
            }
#endif
            ApplyOcclusionBitmask();
            //if (occlusionBitmasks.Count > 0)

            //TODO, added dirty flag to only update dirty properties?

            //Set the bitmask for all children with matching occlusionID
            for (int i = 0; i < childrenProperties.Count; i++)
            {
                ManikinProperties manikinProperties = childrenProperties[i];
                if (occlusionBitmasks.TryGetValue(manikinProperties.occlusionIDHash, out int value))
                {
                    manikinProperties.occlusionBitmask = value;
                }
                else
                {
                    manikinProperties.occlusionBitmask = 0;
                }
            }

            if (properties != null)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    properties[i].ApplyProperties(gameObject, useSRPBatcher, _renderer);
                }

                for (int i = childrenProperties.Count - 1; i >= 0; i--)
                {
                    ManikinProperties manikinProperties = childrenProperties[i];
                    if (manikinProperties != null)
                    {
                        int count = properties.Length;
                        for (int j = 0; j < count; j++)
                        {
                            ManikinProperty property = properties[j];
                            manikinProperties.UpdatePropertyValues(property.values, property.propertyType, property.set, property.payload);
                        }
                        //Cascade properties downward
                        manikinProperties.UpdateProperties();
                    }
                    else
                    {
                        //clean up null children
                        childrenProperties.RemoveAt(i);
                    }
                }
            }

            if (parentProperties == null)
            {
                PropertiesUpdated?.Invoke();
            }
        }

        /// <summary>
        /// Cascades property values down the hierarchy.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="propertyType"></param>
        /// <param name="set"></param>
        /// <param name="payload"></param>
        private void UpdatePropertyValues(float[] values, ManikinProperty.PropertyType propertyType, ManikinPropertySet set, object payload)
        {
            if (properties == null)
                return;

            for (int i = 0; i < properties.Length; i++)
            {
                if (!properties[i].set.name.Equals(set.name)) continue;
                properties[i].values = new float[values.Length];
                properties[i].payload = payload;
                values.CopyTo(properties[i].values, 0);
                properties[i].propertyType = propertyType; //not strictly necessary but we'll automatically update the type too.
                //ApplyProperty(i);
                break;
            }
        }

        #region Property Accessors

        /// <summary>
        /// /// Adds a new Color ManikinProperty to the Properties array on this object.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="hdr"></param>
        /// <param name="manikinPropertySet"></param>
        /// <param name="apply"></param>
        /// <param name="materialIndices"></param>
        public void AddProperty(Color color, bool hdr, ManikinPropertySet manikinPropertySet, bool apply = false, int materialIndices = 0)
        {
            if (hdr)
            {
                AddProperty(new float[4] { color.r, color.g, color.b, color.a }, ManikinProperty.PropertyType.Color_HDR, manikinPropertySet, apply, materialIndices);
            }

            AddProperty(new float[4] { color.r, color.g, color.b, color.a }, ManikinProperty.PropertyType.Color, manikinPropertySet, apply, materialIndices);
        }

        /// <summary>
        /// Adds a new ManikinProperty to the Properties array on this object.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="propertyType"></param>
        /// <param name="manikinPropertySet"></param>
        /// <param name="apply"></param>
        /// <param name="materialIndices"></param>
        public void AddProperty(float[] values, ManikinProperty.PropertyType propertyType, ManikinPropertySet manikinPropertySet, bool apply = false, int materialIndices = 0)
        {
            //TODO switch properties to a list to avoid this.
            ManikinProperty[] extendedProperties = new ManikinProperty[properties.Length + 1];
            properties.CopyTo(extendedProperties, 0);
            extendedProperties[extendedProperties.Length - 1] = new ManikinProperty { values = values, propertyType = propertyType, set = manikinPropertySet, apply = apply, materialIndices = materialIndices };

            properties = extendedProperties;
        }

        /// <summary>
        /// Attempts to update a property with the matching ManikinPropertySet. Returns true if found and false if not found.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="propertyType"></param>
        /// <param name="manikinPropertySet"></param>
        /// <param name="apply"></param>
        /// <param name="materialIndices"></param>
        /// <returns></returns>
        public bool TryUpdateProperty(float[] values, ManikinProperty.PropertyType propertyType, ManikinPropertySet manikinPropertySet, bool apply = false, int materialIndices = 0)
        {
            return UpdateProperty(values, propertyType, manikinPropertySet, apply, materialIndices);
        }

        /// <summary>
        /// Attempts to update a color property with the matching ManikinPropertySet. Returns true if found and false if not found.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="hdr"></param>
        /// <param name="manikinPropertySet"></param>
        /// <param name="apply"></param>
        /// <param name="materialIndices"></param>
        /// <returns></returns>
        public bool TryUpdateProperty(Color color, bool hdr, ManikinPropertySet manikinPropertySet, bool apply = false, int materialIndices = 0)
        {
            if (hdr)
            {
                return UpdateProperty(new float[4] { color.r, color.g, color.b, color.a }, ManikinProperty.PropertyType.Color_HDR, manikinPropertySet, apply, materialIndices);
            }

            return UpdateProperty(new float[4] { color.r, color.g, color.b, color.a }, ManikinProperty.PropertyType.Color, manikinPropertySet, apply, materialIndices);
        }

        public bool TryUpdateProperty(Material newMat, ManikinPropertySet manikinPropertySet, bool apply = false, int materialIndices = 0)
        {
            return UpdateProperty(newMat, ManikinProperty.PropertyType.Material, manikinPropertySet, apply, materialIndices);
        }

        private bool UpdateProperty(float[] values, ManikinProperty.PropertyType propertyType, ManikinPropertySet manikinPropertySet, bool apply, int materialIndices)
        {
            if (properties == null)
                return false;

            for (int i = 0; i < properties.Length; i++)
            {
                if (!properties[i].set.name.Equals(manikinPropertySet.name)) continue;
                properties[i].values = new float[values.Length];
                values.CopyTo(properties[i].values, 0);
                properties[i].propertyType = propertyType;
                properties[i].apply = apply;
                properties[i].materialIndices = materialIndices;
                return true;
            }

            return false;
        }

        private bool UpdateProperty(Material value, ManikinProperty.PropertyType propertyType, ManikinPropertySet manikinPropertySet, bool apply, int materialIndices)
        {
            if (properties == null)
                return false;

            for (int i = 0; i < properties.Length; i++)
            {
                if (!properties[i].set.name.Equals(manikinPropertySet.name)) continue;
                properties[i].values = Array.Empty<float>();
                properties[i].propertyType = propertyType;
                properties[i].apply = apply;
                properties[i].materialIndices = materialIndices;
                properties[i].payload = value;
                return true;
            }

            return false;
        }

        #endregion

        public string ToJson(bool prettyPrint = false)
        {
            return JsonUtility.ToJson(new JsonProperties() { properties = this.properties }, prettyPrint);
        }

        public JsonProperties ToJson()
        {
            return new JsonProperties() { properties = this.properties };
        }

        public void FromJson(string json)
        {
            JsonProperties jsonProperties = JsonUtility.FromJson<JsonProperties>(json);
            properties = jsonProperties.properties;
        }

        public void FromJson(JsonProperties jsonProperties)
        {
            properties = jsonProperties.properties;
        }
    }
}
