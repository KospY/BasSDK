using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/ZoneViewEnabler")]
    [AddComponentMenu("ThunderRoad/Levels/Zone view")]
    public class ZoneViewEnabler : MonoBehaviour
    {
        public GameObject target;
        protected List<BoxCollider> workingZones;

        private void OnEnable()
        {
            RenderPipelineManager.beginCameraRendering += ExecuteBeforeCameraRender;
        }

        private void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= ExecuteBeforeCameraRender;
        }

        private void Awake()
        {
            workingZones = new List<BoxCollider>(this.GetComponentsInChildren<BoxCollider>());
            foreach (BoxCollider boxCollider in workingZones)
            {
                boxCollider.isTrigger = true;
                boxCollider.gameObject.layer = 2;
            }
        }

        public void ExecuteBeforeCameraRender(ScriptableRenderContext context, Camera camera)
        {
            if (camera.gameObject.layer == 21)
            {
                foreach (BoxCollider boxCollider in workingZones)
                {
                    if (boxCollider.bounds.Contains(camera.transform.position))
                    {
                        if (!target.activeSelf) target.SetActive(true);
                        return;
                    }
                }
                if (target.activeSelf) target.SetActive(false);
            }
        }
    }
}
