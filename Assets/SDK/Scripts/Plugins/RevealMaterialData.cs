using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad.Plugins
{
    [CreateAssetMenu(menuName = "RainyReignGames/Reveal Material Data Asset")]
    public class RevealMaterialData : ScriptableObject
    {
        [System.Serializable]
        public struct FloatProperty
        {
            public string name;
            public float value;
            private int id;

            public int GetShaderID()
            {
                if (id == 0)
                {
                    id = Shader.PropertyToID(name);
                    return id;
                }
                return id;
            }
        }
        [System.Serializable]
        public struct ColorProperty
        {
            public string name;
            [ColorUsage(false, true)]
            public Color value;
            private int id;

            public int GetShaderID()
            {
                if (id == 0)
                {
                    id = Shader.PropertyToID(name);
                    return id;
                }
                return id;
            }
        }
        [System.Serializable]
        public struct TextureProperty
        {
            public string name;
            public Texture value;
            private int id;

            public int GetShaderID()
            {
                if (id == 0)
                {
                    id = Shader.PropertyToID(name);
                    return id;
                }
                return id;
            }
        }
        [System.Serializable]
        public struct Vector4Property
        {
            public string name;
            public Vector4 value;
            private int id;

            public int GetShaderID()
            {
                if (id == 0)
                {
                    id = Shader.PropertyToID(name);
                    return id;
                }
                return id;
            }
        }

        public Shader shader;
        public FloatProperty[] revealFloatProperties = new FloatProperty[0];
        public ColorProperty[] revealColorProperties = new ColorProperty[0];
        public TextureProperty[] revealTextureProperties = new TextureProperty[0];
        public Vector4Property[] revealVector4Properties = new Vector4Property[0];

        public static bool shaderChange = true;

        public void SetPropertiesOnMaterial(Material material)
        {
            if (shaderChange && material.shader != null)
            {
                material.shader = shader;
            }
            for (int i = 0; i < revealFloatProperties.Length; i++)
            {
                material.SetFloat(revealFloatProperties[i].GetShaderID(), revealFloatProperties[i].value);
            }
            for (int i = 0; i < revealColorProperties.Length; i++)
            {
                material.SetColor(revealColorProperties[i].GetShaderID(), revealColorProperties[i].value);
            }
            for (int i = 0; i < revealTextureProperties.Length; i++)
            {
                material.SetTexture(revealTextureProperties[i].GetShaderID(), revealTextureProperties[i].value);
            }
            for (int i = 0; i < revealVector4Properties.Length; i++)
            {
                material.SetVector(revealVector4Properties[i].GetShaderID(), revealVector4Properties[i].value);
            }
        }

        public void GetPropertyValuesFromMaterial(Material material)
        {
            for (int i = 0; i < revealFloatProperties.Length; i++)
            {
                if (material.HasProperty(revealFloatProperties[i].name))
                {
                    revealFloatProperties[i].value = material.GetFloat(revealFloatProperties[i].name);
                }
            }
            for (int i = 0; i < revealColorProperties.Length; i++)
            {
                if (material.HasProperty(revealColorProperties[i].name))
                {
                    revealColorProperties[i].value = material.GetColor(revealColorProperties[i].name);
                }
            }
            for (int i = 0; i < revealVector4Properties.Length; i++)
            {
                if (material.HasProperty(revealVector4Properties[i].name))
                {
                    revealVector4Properties[i].value = material.GetVector(revealVector4Properties[i].name);
                }
            }
            for (int i = 0; i < revealTextureProperties.Length; i++)
            {
                if (material.HasProperty(revealTextureProperties[i].name))
                {
                    revealTextureProperties[i].value = material.GetTexture(revealTextureProperties[i].name);
                }
            }
        }
    }
}
