using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class EffectModule
    {
#if ODIN_INSPECTOR
        [HorizontalGroup("Split", width: 0.5f, LabelWidth = 150)]
        [BoxGroup("Split/Module filters")]
#endif
        public Effect.Step step = Effect.Step.Start;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Module filters"), ShowIf("step", Effect.Step.Custom)]
#endif
        public string stepCustomId;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Module filters")]
#endif
        public PlateformFilter plateformFilter = PlateformFilter.Windows | PlateformFilter.Android;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Module filters")]
#endif
        public bool isGore = false;
        
#if ODIN_INSPECTOR
        [BoxGroup("Split/Collision filters")]
#endif
        public DamagerFilter damagerStateFilter = DamagerFilter.Inactive | DamagerFilter.Active;

#if ODIN_INSPECTOR
    [BoxGroup("Split/Collision filters")]
#endif
        public DamageTypeFilter damageTypeFilter = DamageTypeFilter.Energy | DamageTypeFilter.Blunt | DamageTypeFilter.Slash | DamageTypeFilter.Pierce;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Collision filters")]
#endif
        public PenetrationFilter penetrationFilter = PenetrationFilter.None | PenetrationFilter.Hit | PenetrationFilter.Pressure;

#if ODIN_INSPECTOR
        [HorizontalGroup("Split2", width: 0.25f, LabelWidth = 150)]
        [BoxGroup("Split2/Imbues filters")]
#endif
        public FilterLogic imbuesFilterLogic = FilterLogic.AnyExcept;

#if ODIN_INSPECTOR
        [BoxGroup("Split2/Imbues filters"), ValueDropdown("GetAllSpellCastChargeID", IsUniqueList = true)]
#endif
        public string[] imbuesFilter = new string[0];
        protected int[] imbueHashes = new int[0];

#if ODIN_INSPECTOR
        [BoxGroup("Split2/Collider group filters")]
#endif
        public FilterLogic colliderGroupFilterLogic = FilterLogic.AnyExcept;

#if ODIN_INSPECTOR
        [BoxGroup("Split2/Collider group filters"), ValueDropdown("GetAllColliderGroupID", IsUniqueList = true)]
#endif
        public string[] colliderGroupsFilter = new string[0];
        protected int[] colliderGroupHashes = new int[0];

#if ODIN_INSPECTOR
        [BoxGroup("Split2/Damagers filters")]
#endif
        public FilterLogic damagersFilterLogic = FilterLogic.AnyExcept;

#if ODIN_INSPECTOR
        [BoxGroup("Split2/Damagers filters"), ValueDropdown("GetAllDamagerID", IsUniqueList = true)]
#endif
        public string[] damagersFilter = new string[0];
        protected int[] damagerHashes = new int[0];

        [Flags]
        public enum PlateformFilter
        {
            Windows = 1,
            Android = 2,
        }

        [Flags]
        public enum PenetrationFilter
        {
            None = 1,
            Pressure = 2,
            Hit = 4,
            Skewer = 8,
        }

        [Flags]
        public enum DamagerFilter
        {
            Inactive = 1,
            Active = 2,
        }

        [Flags]
        public enum DamageTypeFilter
        {
            Energy = 1,
            Blunt = 2,
            Slash = 4,
            Pierce = 8,
        }
#if PrivateSDK
        public virtual void OnCatalogRefresh(EffectData effectData, bool editorLoad = false)
        {
            imbueHashes = new int[imbuesFilter.Length];
            for (int i = 0; i < imbuesFilter.Length; i++)
            {
                imbueHashes[i] = Animator.StringToHash(imbuesFilter[i].ToLower());
            }
            colliderGroupHashes = new int[colliderGroupsFilter.Length];
            for (int i = 0; i < colliderGroupsFilter.Length; i++)
            {
                colliderGroupHashes[i] = Animator.StringToHash(colliderGroupsFilter[i].ToLower());
            }
            damagerHashes = new int[damagersFilter.Length];
            for (int i = 0; i < damagersFilter.Length; i++)
            {
                damagerHashes[i] = Animator.StringToHash(damagersFilter[i].ToLower());
            }
        }

        public virtual IEnumerator RefreshCoroutine(EffectData effectData, bool editorLoad = false)
        {
            yield return new WaitForEndOfFrame();
        }
#endif
#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllSpellCastChargeID()
        {
            return Catalog.GetDropdownAllID<SpellCastCharge>();
        }
        public List<ValueDropdownItem<string>> GetAllColliderGroupID()
        {
            return Catalog.GetDropdownAllID<ColliderGroupData>();
        }

        public List<ValueDropdownItem<string>> GetAllDamagerID()
        {
            return Catalog.GetDropdownAllID<DamagerData>();
        }
#endif

        [Button]
        public virtual void Clean()
        {

        }

        [Button]
        public virtual void CopyHDRToNonHDR()
        {

        }

#if PrivateSDK
        public virtual bool Spawn(EffectData effectData, Vector3 position, Quaternion rotation, out Effect effect, Transform parent = null, CollisionInstance collisionInstance = null, bool pooled = true)
        {
            effect = null;
            // Damage check
            if (!damagerStateFilter.HasFlag(DamagerFilter.Inactive) && collisionInstance != null && !collisionInstance.damageStruct.active)
            {
                return false;
            }
            if (!damagerStateFilter.HasFlag(DamagerFilter.Active) && collisionInstance != null && collisionInstance.damageStruct.active)
            {
                return false;
            }

            // Penetration filter check
            if (!penetrationFilter.HasFlag(PenetrationFilter.None) && collisionInstance != null && collisionInstance.damageStruct.penetration == DamageStruct.Penetration.None)
            {
                return false;
            }
            if (!penetrationFilter.HasFlag(PenetrationFilter.Pressure) && collisionInstance != null && collisionInstance.damageStruct.penetration == DamageStruct.Penetration.Pressure)
            {
                return false;
            }
            if (!penetrationFilter.HasFlag(PenetrationFilter.Hit) && collisionInstance != null && collisionInstance.damageStruct.penetration == DamageStruct.Penetration.Hit)
            {
                return false;
            }
            if (!penetrationFilter.HasFlag(PenetrationFilter.Skewer) && collisionInstance != null && collisionInstance.damageStruct.penetration == DamageStruct.Penetration.Skewer)
            {
                return false;
            }

            // Damage type filter check
            if (!damageTypeFilter.HasFlag(DamageTypeFilter.Blunt) && collisionInstance != null && collisionInstance.damageStruct.damageType == DamageType.Blunt)
            {
                return false;
            }
            if (!damageTypeFilter.HasFlag(DamageTypeFilter.Slash) && collisionInstance != null && collisionInstance.damageStruct.damageType == DamageType.Slash)
            {
                return false;
            }
            if (!damageTypeFilter.HasFlag(DamageTypeFilter.Pierce) && collisionInstance != null && collisionInstance.damageStruct.damageType == DamageType.Pierce)
            {
                return false;
            }
            if (!damageTypeFilter.HasFlag(DamageTypeFilter.Energy) && collisionInstance != null && collisionInstance.damageStruct.damageType == DamageType.Energy)
            {
                return false;
            }
            return true;
        }
#endif

        protected T EditorLoad<T>(string address) where T : UnityEngine.Object
        {
            if (address == null || address == "") return null;
#if UNITY_EDITOR
            string subAddress = null;
            if (address.Contains("["))
            {
                subAddress = address.Split('[', ']')[1];
                address = address.Split('[', ']')[0];
            }

            UnityEditor.AddressableAssets.Settings.AddressableAssetSettings settings = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings;
            List<UnityEditor.AddressableAssets.Settings.AddressableAssetEntry> allEntries = new List<UnityEditor.AddressableAssets.Settings.AddressableAssetEntry>(settings.groups.SelectMany(g => g.entries));
            UnityEditor.AddressableAssets.Settings.AddressableAssetEntry foundEntry = allEntries.FirstOrDefault(e => e.address == address);

            if (foundEntry != null)
            {
                if (subAddress != null)
                {
                    UnityEngine.Object[] objects = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(foundEntry.AssetPath); //loads all sub-assets from one asset

                    if (objects.Length > 0)
                    {
                        for (int i = 0; i < objects.Length; i++) // loop on all sub-assets loaded
                        {
                            if (objects[i].name == subAddress && objects[i].GetType() == typeof(T)) // if the name AND the type match we found it
                            {
                                return (objects[i]) as T;
                            }
                        }
                    }
                }
                else
                {
                    return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(foundEntry.AssetPath);
                }
            }
            return null;
#else
            Debug.LogError("Can't load addressable asset with editor load option!");
            return null;
#endif
        }
    }
}
