using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ThunderRoad.Plugins
{
    public static class RendererExtension
    {
        public static Material materialInstance(this Renderer renderer)
        {
            if(renderer.TryGetComponent(out MaterialInstance materialInstance))
            {
                return materialInstance.material;
            }
            Debug.LogWarning("No MaterialInstance component found on " + renderer.name, renderer);
            return null;
        }

        public static bool TryGetMaterialInstance(this Renderer renderer, out Material material)
        {
            if (renderer.TryGetComponent(out MaterialInstance materialInstance))
            {
                material = materialInstance.material;
                return (material != null);
            }
            material = null;
            return false;
        }

        public static bool TrySetMaterialInstance(this Renderer renderer, Material material)
        {
            if (renderer.TryGetComponent(out MaterialInstance materialInstance))
            {
                materialInstance.material = material;
                return true;
            }
            return false;
        }

        public static Material[] materialInstances(this Renderer renderer)
        {
            if (renderer.TryGetComponent(out MaterialInstance materialInstance))
            {
                return materialInstance.materials;
            }
            Debug.LogWarning("No MaterialInstance component found on " + renderer.name, renderer);
            return new Material[0];
        }

        public static void materialInstances(this Renderer renderer, List<Material> materials)
        {
            materials.Clear();
            if (renderer.TryGetComponent(out MaterialInstance materialInstance))
            {
                materials.AddRange(materialInstance.materials);
            }
            else
            {
                Debug.LogWarning("No MaterialInstance component found on " + renderer.name, renderer);
            }
        }

        public static bool TryGetMaterialInstances(this Renderer renderer, out Material[] materials)
        {
            if (renderer.TryGetComponent(out MaterialInstance materialInstance))
            {
                materials = materialInstance.materials;
                return (materials != null);
            }
            materials = null;
            return false;
        }

        public static bool TrySetMaterialInstances(this Renderer renderer, Material[] materials)
        {
            if (renderer.TryGetComponent(out MaterialInstance materialInstance))
            {
                materialInstance.materials = materials;
                return true;
            }
            return false;
        }

        public static void ClearMaterialPropertyBlocks(this Renderer renderer)
        {
            renderer.SetPropertyBlock(null);
            if (renderer.sharedMaterials != null)
            {
                Material[] materials = renderer.sharedMaterials;
                if (materials != null)
                {
                    for (int i = 0; i < materials.Length; i++)
                    {
                        renderer.SetPropertyBlock(null, i);
                    }
                }
            }
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Manikin/Clear MaterialPropertyBlocks")]
        static void ClearMaterialPropertyBlocks(MenuCommand command)
        {
            GameObject[] objects = Selection.gameObjects;

            foreach(GameObject obj in objects)
            {
                if(obj.TryGetComponent(out Renderer renderer))
                {
                    renderer.ClearMaterialPropertyBlocks();
                }
            }
        }

        [MenuItem("GameObject/Manikin/Clear MaterialPropertyBlocks", true, 10)]
        static bool ClearMaterialPropertyBlocks()
        {
            if (Selection.activeGameObject != null)
            {
                return (Selection.activeGameObject.GetComponent<Renderer>() != null);
            }
            return false;
        }
#endif
    }
}
