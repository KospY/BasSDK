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
	[Serializable]
    public class MaterialData : CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("General"), ReadOnly] 
#endif
        public int physicMaterialHash;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public string xpReference;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public int apparelProtectionLevel = 0;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public List<Color> idMapColors = new List<Color>();
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public List<EffectBundle> defaultEffects = new List<EffectBundle>();
        protected Collision defaultCollision;

#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public bool isMetal;

#if ODIN_INSPECTOR
        [BoxGroup("Collision(s)")] 
#endif
        public List<Collision> collisions = new List<Collision>();

        [Serializable]
        public class Collision
        {
            [NonSerialized]
            public MaterialData sourceMaterialData;

#if ODIN_INSPECTOR
            [HorizontalGroup("Split", 400)]
            [BoxGroup("Split/Materials")]
            [ValueDropdown("GetAllMaterialID", IsUniqueList = true), LabelText("Target")] 
#endif
            public List<string> targetMaterialIds = new List<string>();
            [NonSerialized]
            public List<MaterialData> targetMaterials = new List<MaterialData>();

#if ODIN_INSPECTOR
            [BoxGroup("Split/Effect(s)")]
            [LabelText("EffectID / Ignored modules")] 
#endif
            public List<EffectBundle> effects = new List<EffectBundle>();

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllMaterialID()
            {
                return Catalog.GetDropdownAllID(Category.Material, "Default");
            } 
#endif

            public enum Parenting
            {
                None,
                SourceCollider,
                TargetCollider,
            }

            public enum Align
            {
                Normal,
                Velocity,
            }

            public enum Rotation
            {
                SourceCollider,
                TargetCollider,
                Random,
            }

            public void OnCatalogRefresh(MaterialData source)
            {
                sourceMaterialData = source;

                targetMaterials = new List<MaterialData>();
                foreach (string materialId in targetMaterialIds)
                {
                    MaterialData materialData = Catalog.GetData<MaterialData>(materialId);
                    if (materialData != null) targetMaterials.Add(materialData);
                }

                for (int i = effects.Count - 1; i >= 0; i--)
                {
                    effects[i].OnCatalogRefresh();
                    if (Application.isPlaying && effects[i].effectData == null)
                    {
                        effects.RemoveAt(i);
                    }
                }
            }
        }

        public override int GetCurrentVersion()
        {
            return 3;
        }

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            physicMaterialHash = Animator.StringToHash(id + " (Instance)");
            if (collisions != null)
            {
                foreach (Collision collision in collisions)
                {
                    collision.OnCatalogRefresh(this);
                }
            }
            defaultCollision = new Collision();
            defaultCollision.effects = defaultEffects;
            defaultCollision.OnCatalogRefresh(this);
        }

    }
}