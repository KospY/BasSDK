using UnityEngine;
using System.Collections.Generic;

namespace ThunderRoad
{
    [ExecuteInEditMode, DisallowMultipleComponent]
    public class MaterialSwapDuringLightmapBake : MonoBehaviour, ICheckAsset
    {
#if UNITY_EDITOR
        public Material materialToSwap;

        private bool swapped = false;
        private Material[] originalMaterials;

        private void OnEnable()
        {
            LightmapBakeHelper.onBakeStarted += OnBakeStarted;
            LightmapBakeHelper.onBakeCompleted += OnBakeCompleted;
        }

        private void OnDisable()
        {
            LightmapBakeHelper.onBakeStarted -= OnBakeStarted;
            LightmapBakeHelper.onBakeCompleted -= OnBakeCompleted;
        }

        private void OnBakeStarted()
        {
            if (materialToSwap == null) return;

            if (swapped)
            {
                Debug.LogErrorFormat(this, "Something wrong, OnBakeStarted but material is still swapped");
                return;
            }

            if (TryGetComponent(out MeshRenderer meshRenderer))
            {
                originalMaterials = meshRenderer.sharedMaterials;
                meshRenderer.sharedMaterials = new Material[] { materialToSwap };
                swapped = true;
            }
            else
            {
                Debug.LogErrorFormat(this, "No MeshRenderer found!");
            }

        }

        private void OnBakeCompleted(LightmapBakeHelper.BakeResult bakeResult)
        {
            if (swapped)
            {
                MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
                meshRenderer.sharedMaterials = originalMaterials;
                swapped = false;
            }
        }

        public List<Issue> GetIssues(bool includeLongCheck, bool experimental)
        {
            List<Issue> issues = new List<Issue>();

            MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
            if (meshRenderer)
            {
                if (materialToSwap == meshRenderer.sharedMaterial)
                {
                    issues.Add(new Issue(this, "MaterialSwapDuringLightmapBake - Mesh renderer still use the material to swap for", false));
                }
            }
            else
            {
                issues.Add(new Issue(this, "MaterialSwapDuringLightmapBake - No meshRenderer attached", false));
            }

            return issues;
        }
#endif
    }
}
