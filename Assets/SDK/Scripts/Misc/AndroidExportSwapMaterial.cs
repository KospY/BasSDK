using UnityEngine;

namespace ThunderRoad
{
	[HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/AndroidExportSwapMaterial.html")]
    [AddComponentMenu("ThunderRoad/Misc/Swap Material On Android Export")]
	[ExecuteInEditMode, DisallowMultipleComponent]
    public class AndroidExportSwapMaterial : MonoBehaviour
    {
#if UNITY_EDITOR
        public Material materialToSwap;
        
        public void SwapMaterial()
        {
            if (materialToSwap == null) return;

            if (TryGetComponent(out MeshRenderer meshRenderer))
            {
                if(meshRenderer.sharedMaterials.Length > 1)
                {
                    Debug.LogErrorFormat(this, "More than one material found, not swapping!");
                    return;
                }
                meshRenderer.sharedMaterials = new Material[] { materialToSwap };
            }
            else
            {
                Debug.LogErrorFormat(this, $"No MeshRenderer found! {this.gameObject.GetPathFromRoot()}");
            }
            
            //destroy self
            DestroyImmediate(this);

        }
#endif
    }

}
