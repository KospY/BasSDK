using UnityEngine;
using RenderPipeline = UnityEngine.Rendering.RenderPipelineManager;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class OceanGridManager : MonoBehaviour
{
    public bool debug;

    public Transform targetTransform;

    public bool rotate = true;

    public bool scale = false;

    public Vector3 scaleVal = new Vector3(1,1,1);

    public Vector3 offset;

    private void OnEnable()
    {
        RenderPipeline.beginCameraRendering -= UpdateCamera;
        RenderPipeline.beginCameraRendering += UpdateCamera;

        RenderPipeline.endFrameRendering -= EndFrame;
        RenderPipeline.endFrameRendering += EndFrame;
    }

    void Reset()
    {
        targetTransform = transform;
    }

    private void EndFrame(ScriptableRenderContext arg1, Camera[] renderCamera)
    {
    }

    void OnDisable()
    {
        RenderPipeline.beginCameraRendering -= UpdateCamera;
        RenderPipeline.endFrameRendering -= EndFrame;
    }

    private void UpdateCamera(ScriptableRenderContext arg1, Camera targetCamera)
    {
        if (!isActiveAndEnabled) return;


        if (targetCamera.cameraType == CameraType.Preview) return;
        if (targetCamera.cameraType == CameraType.Reflection) return;


        if (debug) Debug.Log("Run: " + targetCamera.name + ":" + targetCamera.GetHashCode(), targetCamera);
        /*
        bool renderNeeded = false;
#if UNITY_EDITOR
        // only render the first sceneView (so we can see debug info in a second sceneView)
        //int index = Mathf.Clamp(SceneViewIndex, 0, SceneView.sceneViews.Count - 1);
        //SceneView view = SceneView.sceneViews[index] as SceneView;
        // || renderCamera.CompareTag("SpectatorCamera") // TODO fix: Tag: SpectatorCamera is not defined.
        renderNeeded = renderCamera.CompareTag("MainCamera") || (renderCamera.cameraType == CameraType.SceneView && renderCamera.name.IndexOf("Preview Camera") == -1); //  && view.camera == renderCamera
#else
            //renderNeeded = renderCamera.CompareTag("MainCamera") || renderCamera.CompareTag("SpectatorCamera"); // TODO fix: Tag: SpectatorCamera is not defined.
            renderNeeded = renderCamera.CompareTag("MainCamera");
#endif
*/

        targetTransform.position = new Vector3(targetCamera.transform.position.x, targetTransform.position.y, targetCamera.transform.position.z);

        if (rotate)
        {
            Vector3 lookDir = Vector3.zero - targetCamera.transform.forward;
            float radians = Mathf.Atan2(lookDir.x, lookDir.z);
            float degrees = radians * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, degrees, 0);
            targetTransform.rotation = targetRotation;
        }

        if (scale)
        {
            var distance = Vector3.Distance(transform.position, targetCamera.transform.position);
            //distance *= distance;

            Mathf.Log(distance);
            //distance = Mathf.Clamp(1,)
            var sca = distance ;
            targetTransform.localScale = new Vector3(sca * scaleVal.x,1,sca*scaleVal.z); 
        }
        
        //targetTransform.localPosition = new Vector3(targetTransform.localPosition.x+offset.x,targetTransform.localPosition.y, targetTransform.localPosition.z+offset.z);

    }
}
