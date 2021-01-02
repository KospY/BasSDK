using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
        [BoxGroup("Module")]
#endif
        public Effect.Step step = Effect.Step.Start;
#if ODIN_INSPECTOR
        [BoxGroup("Module"), ShowIf("step", Effect.Step.Custom)]
#endif
        public string stepCustomId;
#if ODIN_INSPECTOR
        [BoxGroup("Module")]
#endif
        public bool disabled;

#if PrivateSDK
        public virtual void Refresh(EffectData effectData, bool editorLoad = false)
        {

        }

        public virtual IEnumerator RefreshCoroutine(EffectData effectData, bool editorLoad = false)
        {
            yield return new WaitForEndOfFrame();
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

        public virtual bool IsEnabled()
        {
            return !disabled;
        }

#if PrivateSDK
        public virtual bool Spawn(EffectData effectData, Vector3 position, Quaternion rotation, out Effect effect, Transform parent = null, CollisionHandler targetCollisionHandler = null, bool pooled = true)
        {
            effect = null;
            return false;
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
