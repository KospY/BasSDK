using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ThunderRoad.Plugins
{
    [RequireComponent(typeof(Renderer))]
    public class MaterialInstance : MonoBehaviour
    {
        public Renderer CachedRenderer => cachedRenderer ?? (cachedRenderer = GetComponent<Renderer>());
        private Renderer cachedRenderer = null;

        public bool IsInstanced { get { return isInstanced; } }

        [SerializeField, HideInInspector]
        private Material[] defaultMaterials = null;
        [SerializeField, HideInInspector]
        private Material[] instanceMaterials = null;
        [SerializeField, HideInInspector]
        private bool isInstanced;

        private const string instancePostfix = " (MaterialInstance)";

        /// <summary>
        /// Returns the first instantiated Material assigned to the renderer, similar to <see href="https://docs.unity3d.com/ScriptReference/Renderer-material.html">Renderer.material</see>.
        /// </summary>
        public Material material
        {
            get { return AcquireMaterial(); }
            set { UpdateInstances(new Material[1] { value }); }
        }

        /// <summary>
        /// Returns all the instantiated materials of this object, similar to <see href="https://docs.unity3d.com/ScriptReference/Renderer-materials.html">Renderer.materials</see>.
        /// </summary>
        public Material[] materials
        {
            get { return AcquireMaterials(); }
            set { UpdateInstances(value); }
        }

        private Material AcquireMaterial()
        {
            AcquireMaterials();
            return instanceMaterials[0];
        }

        [ContextMenu("Acquire Materials")]
        private Material[] AcquireMaterials()
        {
            if (!isInstanced && CachedRenderer?.sharedMaterials != null)
            {
                CreateInstances();
            }

            return instanceMaterials;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (PrefabUtility.IsPartOfPrefabAsset(this))
            {
                defaultMaterials = null;
                isInstanced = false;
            }
            else
            {
                bool defaultMaterialsExist = (defaultMaterials != null && defaultMaterials.Length > 0);
                bool instanceMaterialsExist = (instanceMaterials != null && instanceMaterials.Length > 0);

                if ((defaultMaterialsExist && !instanceMaterialsExist) || (!defaultMaterialsExist && instanceMaterialsExist))
                {
                    instanceMaterials = null;
                    defaultMaterials = null;
                    isInstanced = false;
                }

                if(isInstanced && !defaultMaterialsExist && !instanceMaterialsExist)
                {
                    instanceMaterials = null;
                    defaultMaterials = null;
                    isInstanced = false;
                }

                if(!isInstanced && defaultMaterialsExist && instanceMaterialsExist)
                {
                    isInstanced = true;
                }
            }
        }
#endif

        private void OnDestroy()
        {
            RestoreRenderer();
        }

        [ContextMenu("Restore Renderer")]
        public void RestoreRenderer()
        {
            if (isInstanced && CachedRenderer != null)
            {
                CachedRenderer.sharedMaterials = defaultMaterials;
                DestroyMaterials(instanceMaterials);
                instanceMaterials = null;
                defaultMaterials = null;
                isInstanced = false;
            }
        }

        private void UpdateInstances(Material[] materials)
        {
            if (!isInstanced)
            {
                defaultMaterials = CachedRenderer.sharedMaterials;
            }
            instanceMaterials = materials;
            CachedRenderer.sharedMaterials = materials;
            isInstanced = true;
        }

        private void CreateInstances()
        {
#if UNITY_EDITOR
            //we don't do anything currently for non-instanced prefabs
            if (PrefabUtility.IsPartOfPrefabAsset(this))
                return;
#endif

            defaultMaterials = CachedRenderer.sharedMaterials;
            isInstanced = true;

            if (Application.isPlaying)
            {
                instanceMaterials = CachedRenderer.materials;
            }
#if UNITY_EDITOR
            else
            {
                //Workaround to not get spammed with error messages at edit time.
                instanceMaterials = new Material[defaultMaterials.Length];
                for (int i = 0; i < instanceMaterials.Length; i++)
                {
                    if (defaultMaterials[i] != null)
                    {
                        instanceMaterials[i] = new Material(defaultMaterials[i]);
                    }
                }
                CachedRenderer.sharedMaterials = instanceMaterials;
                PrefabUtility.RecordPrefabInstancePropertyModifications(this);
            }

            //do this only in editor for inspecting purposes
            if (instanceMaterials != null)
            {
                for (int i = 0; i < instanceMaterials.Length; i++)
                {
                    if (instanceMaterials[i] != null && defaultMaterials[i] != null)
                    {
                        string name = defaultMaterials[i].name + instancePostfix;
                        instanceMaterials[i].name = name;
                    }
                }
            }
#endif
        }

        private static void DestroyMaterials(Material[] materials)
        {
            if (materials != null)
            {
                for (var i = 0; i < materials.Length; ++i)
                {
                    DestorySafe(materials[i]);
                }
            }
        }

        private static void DestorySafe(UnityEngine.Object toDestroy)
        {
            if (toDestroy != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(toDestroy);
                }
                else
                {
#if UNITY_EDITOR
                    // Let Unity handle unload of unused assets if lifecycle is transitioning from editor to play mode
                    // Deferring the call during this transition would destroy reference only after play mode Awake, leading to possible broken material references on TMPro objects
                    if (!EditorApplication.isPlayingOrWillChangePlaymode)
                    {
                        EditorApplication.delayCall += () =>
                        {
                            if (toDestroy != null)
                            {
                                DestroyImmediate(toDestroy);
                            }
                        };
                    }
#endif
                }
            }
        }
    }
}
