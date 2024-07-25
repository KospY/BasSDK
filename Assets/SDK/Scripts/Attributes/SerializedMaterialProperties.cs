using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR

[System.Serializable]
public class SerializedMaterialProperties
{
    #region InternalClass
    [System.Serializable]
    public class SerializedMaterialProperty
    {
        public enum Type
        {
            Color,
            Vector,
            Float,
            Texture,
            Int
        }

        public enum RotationModifier
        {
            NotRotated,
            AddDegree,
            MinusDegree,
            AddRadiant,
            MinuRadiant
        }

        [SerializeField]
        private Type _type;

        [SerializeField]
        private string _name;
        [SerializeField]
        private string _displayName;

        [SerializeField]
        private Color _colorValue;
        [SerializeField]
        private Vector4 _vectorValue;
        [SerializeField]
        private float _floatValue;
        [SerializeField]
        private bool _isRange = false;
        [SerializeField]
        private Vector2 _rangeLimits;
        [SerializeField]
        private RotationModifier _rotationModifier;
        [SerializeField]
        private Texture _textureValue;
        [SerializeField]
        private int _intValue;

#if UNITY_EDITOR
        public static SerializedMaterialProperty CreateProperty(MaterialProperty property)
        {
            SerializedMaterialProperty output = new SerializedMaterialProperty();
            output._name = property.name;
            output._displayName = property.displayName;

            switch (property.type)
            {
                case MaterialProperty.PropType.Color:
                    {
                        output._type = Type.Color;
                        output._colorValue = property.colorValue;
                    }
                    break;

                case MaterialProperty.PropType.Vector:
                    {
                        output._type = Type.Vector;
                        output._vectorValue = property.vectorValue;
                    }
                    break;

                case MaterialProperty.PropType.Float:
                    {
                        output._type = Type.Float;
                        output._floatValue = property.floatValue;
                        if(output._name.ToLower().Contains("rotation") || output._displayName.ToLower().Contains("rotation"))
                        {
                            output._rotationModifier = RotationModifier.MinusDegree;
                        }
                    }
                    break;

                case MaterialProperty.PropType.Range:
                    {
                        output._type = Type.Float;
                        output._floatValue = property.floatValue;
                        output._rangeLimits = property.rangeLimits;
                        output._isRange = true;
                        if (output._name.ToLower().Contains("rotation") || output._displayName.ToLower().Contains("rotation"))
                        {
                            output._rotationModifier = RotationModifier.MinusDegree;
                        }
                    }
                    break;

                case MaterialProperty.PropType.Texture:
                    {
                        output._type = Type.Texture;
                        output._textureValue = property.textureValue;
                    }
                    break;

                case MaterialProperty.PropType.Int:
                    {
                        output._type = Type.Int;
                        output._intValue = property.intValue;
                    }
                    break;

                default:
                    break;
            }

            return output;
        }
#endif //UNITY_EDITOR

        public bool IsSameProperty(SerializedMaterialProperty other)
        {
            return string.Equals(_name, other._name) && _type == other._type;
        }


        /// <summary>
        /// Copy all data from an other SerializedMaterialProperty to the current SerializedMaterialProperty
        /// </summary>
        /// <param name="other">The other SerializedMaterialProperty you want to copy the data</param>
        /// <returns>true if the data needed changes</returns>
        public bool CopyFrom(SerializedMaterialProperty other)
        {
            bool hasChanged = false;

            if (_type != other._type)
            {
                _type = other._type;
                hasChanged = true;
            }

            if (_name != other._name)
            {
                _name = other._name;
                hasChanged = true;
            }

            if (_colorValue != other._colorValue)
            {
                _colorValue = other._colorValue;
                hasChanged = true;
            }


            if (_vectorValue != other._vectorValue)
            {
                _vectorValue = other._vectorValue;
                hasChanged = true;
            }

            if (_floatValue != other._floatValue)
            {
                _floatValue = other._floatValue;
                hasChanged = true;
            }

            if (_rotationModifier != other._rotationModifier)
            {
                _rotationModifier = other._rotationModifier;
                hasChanged = true;
            }

            if (_textureValue != other._textureValue)
            {
                _textureValue = other._textureValue;
                hasChanged = true;
            }

            if (_intValue != other._intValue)
            {
                _intValue = other._intValue;
                hasChanged = true;
            }

#if UNITY_EDITOR
            // We don't need to trakc those value they are just for editor display
            _displayName = other._displayName;
            _rangeLimits = other._rangeLimits;
            _isRange = other._isRange;
#endif //UNITY_EDITOR

            return hasChanged;
        }

        public void UpdatePropertyBlock(MaterialPropertyBlock materialPropertyBlock, float rotationDegrees)
        {
            switch (_type)
            {
                case Type.Color:
                    {
                        if (materialPropertyBlock.HasColor(_name))
                        {
                            materialPropertyBlock.SetColor(_name, _colorValue);
                        }
                    }
                    break;

                case Type.Vector:
                    {
                        if (materialPropertyBlock.HasVector(_name))
                        {
                            materialPropertyBlock.SetVector(_name, _vectorValue);
                        }
                    }
                    break;

                case Type.Float:
                    {
                        if (materialPropertyBlock.HasFloat(_name))
                        {
                            float value = _floatValue;
                            switch (_rotationModifier)
                            {
                                case RotationModifier.NotRotated:
                                    break;
                                case RotationModifier.AddDegree:
                                    value += rotationDegrees;
                                    if (_isRange && value > _rangeLimits[1])
                                    {
                                        value -= 360.0f;
                                    }
                                    break;
                                case RotationModifier.MinusDegree:
                                    value -= rotationDegrees;
                                    if (_isRange && value < _rangeLimits[0])
                                    {
                                        value += 360.0f;
                                    }
                                    break;
                                case RotationModifier.AddRadiant:
                                    value += rotationDegrees * Mathf.Deg2Rad;
                                    if (_isRange && value > _rangeLimits[1])
                                    {
                                        value -= Mathf.PI * 2.0f;
                                    }
                                    break;
                                case RotationModifier.MinuRadiant:
                                    value -= rotationDegrees;
                                    if (_isRange && value < _rangeLimits[0])
                                    {
                                        value += Mathf.PI * 2.0f;
                                    }
                                    break;
                                default:
                                    break;
                            }

                            materialPropertyBlock.SetFloat(_name, value);
                        }
                    }
                    break;

                case Type.Texture:
                    {
                        if (materialPropertyBlock.HasTexture(_name))
                        {
                            materialPropertyBlock.SetTexture(_name, _textureValue);
                        }
                    }
                    break;

                case Type.Int:
                    {
                        if (materialPropertyBlock.HasInt(_name))
                        {
                            materialPropertyBlock.SetInt(_name, _intValue);
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        public void ApplyToMat(Material material, float rotationDegrees)
        {
            switch (_type)
            {
                case Type.Color:
                    {
                        if (material.HasColor(_name))
                        {
                            material.SetColor(_name, _colorValue);
                        }
                    }
                    break;

                case Type.Vector:
                    {
                        if (material.HasVector(_name))
                        {
                            material.SetVector(_name, _vectorValue);
                        }
                    }
                    break;

                case Type.Float:
                    {
                        float value = _floatValue;
                        switch (_rotationModifier)
                        {
                            case RotationModifier.NotRotated:
                                break;
                            case RotationModifier.AddDegree:
                                value += rotationDegrees;
                                if (_isRange && value > _rangeLimits[1])
                                {
                                    value -= 360.0f;
                                }
                                break;
                            case RotationModifier.MinusDegree:
                                value -= rotationDegrees;
                                if (_isRange && value < _rangeLimits[0])
                                {
                                    value += 360.0f;
                                }
                                break;
                            case RotationModifier.AddRadiant:
                                value += rotationDegrees * Mathf.Deg2Rad;
                                if (_isRange && value > _rangeLimits[1])
                                {
                                    value -= Mathf.PI * 2.0f;
                                }
                                break;
                            case RotationModifier.MinuRadiant:
                                value -= rotationDegrees;
                                if (_isRange && value < _rangeLimits[0])
                                {
                                    value += Mathf.PI * 2.0f;
                                }
                                break;
                            default:
                                break;
                        }

                        material.SetFloat(_name, value);
                    }
                    break;

                case Type.Texture:
                    {
                        if (material.HasTexture(_name))
                        {
                            material.SetTexture(_name, _textureValue);
                        }
                    }
                    break;

                case Type.Int:
                    {
                        if (material.HasInt(_name))
                        {
                            material.SetInt(_name, _intValue);
                        }
                    }
                    break;

                default:
                    break;
            }
        }
    }
    #endregion InternalClass

    [SerializeField]
    private Material _material;

    [SerializeField]
    private SerializedMaterialProperty[] _properties;

    private Material _instancedMaterial = null;

    /// <summary>
    /// Get the raw material
    /// </summary>
    public Material Mat => _material;

    /// <summary>
    /// Get the Instanced modified Material
    /// </summary>
    public Material GetInstancedMat(float rotationDegrees)
    {
        if (_instancedMaterial != null) return _instancedMaterial;

        _instancedMaterial = new Material(_material);
        _instancedMaterial.name += "(Instanced)";
        for (int i = 0; i < _properties.Length; i++)
        {
            _properties[i].ApplyToMat(_instancedMaterial, rotationDegrees);
        }

        return _instancedMaterial;
    }

    public void ReleaseInstancedMaterial()
    {
        if (_instancedMaterial == null) return;
        if(Application.isPlaying)
        {
            UnityEngine.Object.Destroy(_instancedMaterial);
        }
        else
        {
            UnityEngine.Object.DestroyImmediate(_instancedMaterial);
        }
        _instancedMaterial = null;
    }

    public void UpdatePropertyblock(MaterialPropertyBlock materialPropertyBlock, float rotationDegrees)
    {
        for (int i = 0; i < _properties.Length; i++)
        {
            _properties[i].UpdatePropertyBlock(materialPropertyBlock, rotationDegrees);
        }
    }

    /// <summary>
    /// Copy all data from an other SerializedMaterialProperties to the current SerializedMaterialProperties
    /// </summary>
    /// <param name="other">The other SerializedMaterialProperties you want to copy the data</param>
    /// <returns>true if the data needed changes</returns>
    public bool CopyFrom(SerializedMaterialProperties other)
    {
        if (_material != other._material)
        {
            _material = other._material;
            if (other._properties == null)
            {
                _properties = null;
            }
            else
            {
                _properties = new SerializedMaterialProperty[other._properties.Length];
                for (int i = 0; i < other._properties.Length; i++)
                {
                    _properties[i] = new SerializedMaterialProperty();
                    _properties[i].CopyFrom(other._properties[i]);
                }
            }

            return true;
        }

        if (_properties == null && other._properties == null)
        {
            return false;
        }

        if (_properties != null && other._properties == null)
        {
            _properties = null;
            return true;
        }

        if (_properties == null && other._properties != null)
        {
            _properties = new SerializedMaterialProperty[other._properties.Length];
            for (int i = 0; i < other._properties.Length; i++)
            {
                _properties[i] = new SerializedMaterialProperty();
                _properties[i].CopyFrom(other._properties[i]);
            }
            return true;
        }

        if (_properties.Length != other._properties.Length)
        {
            _properties = new SerializedMaterialProperty[other._properties.Length];
            for (int i = 0; i < other._properties.Length; i++)
            {
                _properties[i] = new SerializedMaterialProperty();
                _properties[i].CopyFrom(other._properties[i]);
            }
            return true;
        }

        bool hasChanged = false;
        for (int i = 0; i < _properties.Length; i++)
        {
            if (_properties[i].CopyFrom(other._properties[i]))
            {
                hasChanged = true;
            }
        }

        return hasChanged;
    }

#if UNITY_EDITOR

    public void UpdateMaterialProperties()
    {
        if (_material == null)
        {
            _properties = null;
            return;
        }

        SerializedMaterialProperty[] newPropertyArray = GetSerializedProperties(_material);

        if (_properties == null || _properties.Length == 0)
        {
            _properties = newPropertyArray;
            return;
        }

        for (int indexNew = 0; indexNew < newPropertyArray.Length; indexNew++)
        {
            SerializedMaterialProperty newProperty = newPropertyArray[indexNew];
            for (int indexOld = 0; indexOld < _properties.Length; indexOld++)
            {
                SerializedMaterialProperty oldProperty = _properties[indexOld];
                if (newProperty.IsSameProperty(oldProperty))
                {
                    // replace with old
                    newPropertyArray[indexNew] = oldProperty;
                    break;
                }
            }
        }

        _properties = newPropertyArray;
    }

    public static SerializedMaterialProperties CreateProperties(Material mat)
    {
        SerializedMaterialProperties output = new SerializedMaterialProperties();
        output._material = RetrieveOriginalMaterial(mat);

        if (output._material == null)
        {
            output._properties = null;
            return null;
        }

        output._properties = GetSerializedProperties(mat);

        return output;
    }

    private static Material RetrieveOriginalMaterial(Material mat)
    {
        if (mat == null) return null;

        if (!mat.name.Contains("(Instanced)"))
        {
            return mat;
        }

        string name = mat.name.Replace("(Instanced)", string.Empty);

        string[] results = AssetDatabase.FindAssets(name + " t:Material");
        for (int i = 0; i < results.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(results[i]);
            string resultName = System.IO.Path.GetFileNameWithoutExtension(assetPath);

            if (resultName.Equals(name))
            {
                return AssetDatabase.LoadAssetAtPath<Material>(assetPath);
            }
        }

        Debug.LogError("Can not find original material from " + mat.name);
        return null;
    }

    private static SerializedMaterialProperty[] GetSerializedProperties(Material mat)
    {
        SerializedMaterialProperty[] output;
        Material[] mats = new Material[] { mat };
        MaterialProperty[] matProperties = MaterialEditor.GetMaterialProperties(mats);
        output = new SerializedMaterialProperty[matProperties.Length];

        for (int i = 0; i < matProperties.Length; i++)
        {
            output[i] = SerializedMaterialProperty.CreateProperty(matProperties[i]);
        }

        return output;
    }
#endif //UNITY_EDITOR
}