using UnityEngine;
using System.Collections.Generic;
using System;
#if ProjectCore
using Sirenix.OdinInspector;
using PaintIn3D;
#endif

namespace ThunderRoad
{
    public class Paintable : MonoBehaviour
    {
        public List<MaterialProperty> materialProperties = new List<MaterialProperty>();
        public bool cloneMaterial;

#if ProjectCore
        [NonSerialized]
        public P3dPaintable p3dPaintable;
#endif

        [Serializable]
        public class MaterialProperty
        {
            public int materialIndex = 0;
            public PropertyType propertyType = PropertyType.Base;
            public string propertyName = "_BaseMap";
#if ProjectCore
            [NonSerialized]
            public P3dPaintableTexture paintableTexture;
#endif
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

#if ProjectCore

        protected P3dMaterialCloner materialCloner;

        void Start()
        {
            // Default value
            if (materialProperties.Count == 0)
            {
                materialProperties.Add(new MaterialProperty());
            }
            if (cloneMaterial)
            {
                materialCloner = this.gameObject.AddComponent<P3dMaterialCloner>();
                materialCloner.Activate();
            }

            p3dPaintable = this.gameObject.AddComponent<P3dPaintable>();

            foreach (MaterialProperty materialProperty in materialProperties)
            {
                Material material = P3dHelper.GetMaterial(this.gameObject, materialProperty.materialIndex);
                if (material)
                {
                    Texture orgTexture = material.GetTexture(materialProperty.propertyName);
                    materialProperty.paintableTexture = this.gameObject.AddComponent<P3dPaintableTexture>();
                    materialProperty.paintableTexture.Slot = new P3dSlot(materialProperty.materialIndex, materialProperty.propertyName);
                    materialProperty.paintableTexture.Texture = orgTexture;
                    materialProperty.paintableTexture.Height = orgTexture.height;
                    materialProperty.paintableTexture.Width = orgTexture.width;
                    materialProperty.paintableTexture.Activate();
                }
                else
                {
                    Debug.LogWarning("Paintable material index [" + materialProperty.materialIndex + "] not found");
                }
            }
        }

        public void DestroyPaintComponents()
        {
            foreach (MaterialProperty materialProperty in materialProperties)
            {
                Destroy(materialProperty.paintableTexture);
            }
            if (materialCloner) Destroy(materialCloner);
            if (p3dPaintable) Destroy(p3dPaintable);
        }

        [Button]
        public void Clear()
        {
            foreach (MaterialProperty materialProperty in materialProperties)
            {
                materialProperty.paintableTexture.Clear(materialProperty.paintableTexture.Texture, Color.white);
            }
        }

        public void Paint(P3dCommandDecal command, MaterialProperty.PropertyType propertyType)
        {
            foreach (MaterialProperty materialProperty in materialProperties)
            {
                if (materialProperty.propertyType == propertyType)
                {
                    command.ClearMask();
                    command.ApplyAspect(materialProperty.paintableTexture.Current);
                    P3dPaintableManager.Submit(command, p3dPaintable, materialProperty.paintableTexture);
                }
            }
        }
#endif
    }
}