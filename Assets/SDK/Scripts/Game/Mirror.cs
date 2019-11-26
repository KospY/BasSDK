using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Rendering.LWRP;
using UnityEngine.Rendering;

namespace BS
{
    public class Mirror : MonoBehaviour
    {
        public static Mirror local;
        public ReflectionDirection reflectionDirection = ReflectionDirection.Up;
        [Range(0, 1)]
        public float quality = 1;
        [Range(0, 1)]
        public float Intensity = 1f;
        public bool reflectionWithoutGI = true;
        public Color backgroundColor = Color.black;
        public LayerMask cullingMask = ~0;
        public Collider workingArea;
        public MeshRenderer mirrorMesh;
        public MeshRenderer meshToHide;

        public enum ReflectionDirection
        {
            Up,
            Down,
            Forward,
            Back,
            Left,
            Right,
        }

#if ProjectCore
        private Vector3 reflectionLocalDirection;
        private Vector3 reflectionWorldDirection;
        private Camera reflectionCamera;
        private RenderTexture leftEyeRenderTexture;
        private RenderTexture rightEyeRenderTexture;

        void OnValidate()
        {
            Refresh();
        }

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
            if (!XRSettings.enabled) return;
            local = this;
            mirrorMesh.material = GameManager.local.mirrorMaterial;
            reflectionCamera = new GameObject("ReflectionCamera").AddComponent<Camera>();
            reflectionCamera.useOcclusionCulling = false;
            reflectionCamera.transform.SetParent(this.transform, true);
            reflectionCamera.enabled = false;
            Refresh();
        }

        public void Refresh()
        {
            if (reflectionDirection == ReflectionDirection.Forward) reflectionLocalDirection = Vector3.forward;
            else if (reflectionDirection == ReflectionDirection.Up) reflectionLocalDirection = Vector3.up;
            else if (reflectionDirection == ReflectionDirection.Back) reflectionLocalDirection = Vector3.back;
            else if (reflectionDirection == ReflectionDirection.Down) reflectionLocalDirection = Vector3.down;
            else if (reflectionDirection == ReflectionDirection.Left) reflectionLocalDirection = Vector3.left;
            else if (reflectionDirection == ReflectionDirection.Right) reflectionLocalDirection = Vector3.right;
            reflectionWorldDirection = this.transform.TransformDirection(reflectionLocalDirection);
            if (Application.isPlaying)
            {
                leftEyeRenderTexture = new RenderTexture((int)(quality * XRSettings.eyeTextureWidth), (int)(quality * XRSettings.eyeTextureHeight), 24);
                rightEyeRenderTexture = new RenderTexture((int)(quality * XRSettings.eyeTextureWidth), (int)(quality * XRSettings.eyeTextureHeight), 24);
                leftEyeRenderTexture.filterMode = rightEyeRenderTexture.filterMode = FilterMode.Bilinear;
                leftEyeRenderTexture.antiAliasing = rightEyeRenderTexture.antiAliasing = 1;
                leftEyeRenderTexture.hideFlags = rightEyeRenderTexture.hideFlags = HideFlags.HideAndDontSave;
                leftEyeRenderTexture.autoGenerateMips = rightEyeRenderTexture.autoGenerateMips = false;
                leftEyeRenderTexture.wrapMode = rightEyeRenderTexture.wrapMode = TextureWrapMode.Clamp;
                if (reflectionCamera)
                {
                    reflectionCamera.backgroundColor = backgroundColor;
                    reflectionCamera.cullingMask = cullingMask;
                }
                if (mirrorMesh.material)
                {
                    if (reflectionWithoutGI) mirrorMesh.material.EnableKeyword("_FULLMIRROR");
                    else mirrorMesh.material.DisableKeyword("_FULLMIRROR");
                    mirrorMesh.material.SetFloat("_ReflectionIntensity", Intensity);
                }
            }
        }

        public void ExecuteBeforeCameraRender(ScriptableRenderContext context, Camera camera)
        {
            if (camera != reflectionCamera)
            {
                RenderCam(context, camera);
            }
        }

        public void RenderCam(ScriptableRenderContext context, Camera camera)
        {
            // Disable rendering when current camera is not in the working area
            if (workingArea && !workingArea.bounds.Contains(camera.transform.position)) return;
            // Get reflection direction
            reflectionWorldDirection = this.transform.TransformDirection(reflectionLocalDirection);
            // Get mirror plane
            Vector3 pos = this.transform.position;
            Vector3 normal = reflectionWorldDirection;
            float d = -Vector3.Dot(normal, this.transform.position);
            Vector4 mirrorPlane = new Vector4(normal.x, normal.y, normal.z, d);
            // Get reflection matrix
            Matrix4x4 reflectionMatrix = CalculateReflectionMatrix(mirrorPlane);
            // Set camera position
            reflectionCamera.transform.position = reflectionMatrix.MultiplyPoint(camera.transform.position);
            Debug.DrawRay(reflectionCamera.transform.position, reflectionCamera.transform.forward * 1, Color.blue);

            GL.invertCulling = true;
            bool orgFog = RenderSettings.fog;
            RenderSettings.fog = false;
            if (meshToHide) meshToHide.enabled = false;

            RenderEye(Side.Left, reflectionMatrix, context, camera);
            RenderEye(Side.Right, reflectionMatrix, context, camera);

            RenderSettings.fog = orgFog;
            GL.invertCulling = false;
            if (meshToHide) meshToHide.enabled = true;
        }

        private void RenderEye(Side side, Matrix4x4 reflectionMatrix, ScriptableRenderContext context, Camera camera)
        {
            reflectionCamera.stereoTargetEye = side == Side.Left ? StereoTargetEyeMask.Left : StereoTargetEyeMask.Right;

            // View matrix reflection
            reflectionCamera.worldToCameraMatrix = camera.GetStereoViewMatrix(side == Side.Left ? Camera.StereoscopicEye.Left : Camera.StereoscopicEye.Right) * reflectionMatrix;

            // Set projection matrix
            reflectionCamera.projectionMatrix = camera.GetStereoNonJitteredProjectionMatrix(side == Side.Left ? Camera.StereoscopicEye.Left : Camera.StereoscopicEye.Right);

            // Clip plane
            Vector3 cpos = reflectionCamera.worldToCameraMatrix.MultiplyPoint(this.transform.position);
            Vector3 cnormal = reflectionCamera.worldToCameraMatrix.MultiplyVector(reflectionWorldDirection).normalized;
            Vector4 clipPlane = new Vector4(cnormal.x, cnormal.y, cnormal.z, -Vector3.Dot(cpos, cnormal));
            reflectionCamera.projectionMatrix = reflectionCamera.CalculateObliqueMatrix(clipPlane);
        
            // Render to texture
            reflectionCamera.targetTexture = side == Side.Left ? leftEyeRenderTexture : rightEyeRenderTexture;

            // Setting depth texture on LWRP setting will break mirror when render scale in below 1.0
            UnityEngine.Rendering.Universal.UniversalRenderPipeline.RenderSingleCamera(context, reflectionCamera);

            //material.SetTexture(side == Side.Left ? "_LeftEye" : "_RightEye", side == Side.Left ? leftEyeRenderTexture : rightEyeRenderTexture);
            mirrorMesh.material.SetTexture(side == Side.Left ? "_LeftEye" : "_RightEye", side == Side.Left ? leftEyeRenderTexture : rightEyeRenderTexture);
        }

        private static Matrix4x4 CalculateReflectionMatrix(Vector4 plane)
        {
            Matrix4x4 reflectionMatrix = Matrix4x4.zero;
            reflectionMatrix.m00 = (1F - 2F * plane[0] * plane[0]);
            reflectionMatrix.m01 = (-2F * plane[0] * plane[1]);
            reflectionMatrix.m02 = (-2F * plane[0] * plane[2]);
            reflectionMatrix.m03 = (-2F * plane[3] * plane[0]);

            reflectionMatrix.m10 = (-2F * plane[1] * plane[0]);
            reflectionMatrix.m11 = (1F - 2F * plane[1] * plane[1]);
            reflectionMatrix.m12 = (-2F * plane[1] * plane[2]);
            reflectionMatrix.m13 = (-2F * plane[3] * plane[1]);

            reflectionMatrix.m20 = (-2F * plane[2] * plane[0]);
            reflectionMatrix.m21 = (-2F * plane[2] * plane[1]);
            reflectionMatrix.m22 = (1F - 2F * plane[2] * plane[2]);
            reflectionMatrix.m23 = (-2F * plane[3] * plane[2]);

            reflectionMatrix.m30 = 0F;
            reflectionMatrix.m31 = 0F;
            reflectionMatrix.m32 = 0F;
            reflectionMatrix.m33 = 1F;
            return reflectionMatrix;
        }

        protected virtual void OnDrawGizmosSelected()
        {
            DrawGizmoArrow(this.transform.position, reflectionWorldDirection * 0.5f, Color.blue);
        }

        public static void DrawGizmoArrow(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Gizmos.color = color;
            Gizmos.DrawRay(pos, direction);
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        }
#endif
    }
}
