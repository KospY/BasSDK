using System;
using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using Unity.EditorCoroutines.Editor;
#endif

namespace ThunderRoad
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class LightmapBakeHelper
    {
#if UNITY_EDITOR
	    [InitializeOnLoadMethod]
	    static void OnLightmapBakeHelperLoaded()
	    {
        }
        public static bool bakeInProgress { get; private set; }

        // Use ASTC_4x4 to avoid color compression artifacts
        public static TextureImporterFormat defaultAndroidLightmapColorFormat = TextureImporterFormat.ASTC_4x4;

        // Use RGB 16 Bit to avoid white artifacts when baking on GPU
        public static TextureImporterFormat defaultLightmapDirectionalFormat = TextureImporterFormat.DXT1;

        static bool stopBakeIfCPU;
        static EditorCoroutine coroutine;

        /// <summary>
        /// Called when bake complete. Return false if bake has been canceled.
        /// </summary>
        public static event Action onBakeStarted;
        /// <summary>
        /// Called when bake complete. Return false if bake has been canceled and true if succesfull
        /// </summary>
        public static event Action<BakeResult> onBakeCompleted;
        /// <summary>
        /// Called when bake complete after onBakeCompleted ran. Return false if bake has been canceled and true if succesfull
        /// </summary>
        public static event Action<BakeResult> onBakeLateCompleted;

        [NonSerialized]
        public static BakeResult lastBakeResult = BakeResult.None;

        public enum BakeResult
        {
            None,
            FailedToStart,
            SwitchedToCPU,
            Timeout,
            CanceledByUser,
            Successfull,
        }

        static LightmapBakeHelper()
        {
            Lightmapping.bakeStarted += OnBakeStarted;
            Lightmapping.bakeCompleted += OnBakeCompleted;
        }

        public static void SetEditorContributeGI(MeshRenderer meshRenderer, bool active)
        {
            StaticEditorFlags staticEditorFlags = GameObjectUtility.GetStaticEditorFlags(meshRenderer.gameObject);
            if (active && !staticEditorFlags.HasFlag(StaticEditorFlags.ContributeGI))
            {
                staticEditorFlags = staticEditorFlags | StaticEditorFlags.ContributeGI;
                GameObjectUtility.SetStaticEditorFlags(meshRenderer.gameObject, staticEditorFlags);
                meshRenderer.receiveGI = ReceiveGI.Lightmaps;
            }
            if (!active && staticEditorFlags.HasFlag(StaticEditorFlags.ContributeGI))
            {
                staticEditorFlags = staticEditorFlags & ~(StaticEditorFlags.ContributeGI);
                GameObjectUtility.SetStaticEditorFlags(meshRenderer.gameObject, staticEditorFlags);
                meshRenderer.receiveGI = ReceiveGI.LightProbes;
            }
        }

        public static void Bake(bool stopIfCPU = false)
        {
            stopBakeIfCPU = stopIfCPU;
            Lightmapping.Cancel();

            if (Lightmapping.giWorkflowMode != Lightmapping.GIWorkflowMode.OnDemand)
            {
                Debug.LogError("GIWorkflowMode must be set to OnDemand");
                return;
            }

            if (stopBakeIfCPU) Lightmapping.lightingSettings.lightmapper = LightingSettings.Lightmapper.ProgressiveGPU;

            if (Lightmapping.BakeAsync())
            {
                bakeInProgress = true;
            }
            else
            {
                Debug.LogError("Lightmaps bake failed to start");
                bakeInProgress = false;
                lastBakeResult = BakeResult.FailedToStart;
                onBakeCompleted?.Invoke(lastBakeResult);
                onBakeLateCompleted?.Invoke(lastBakeResult);
            }
        }

        public static void CancelBake()
        {
            Lightmapping.Cancel();
            Lightmapping.ForceStop();
        }

        static void OnBakeStarted()
        {
            Debug.Log("Lightmaps baking started...");
            bakeInProgress = true;
            onBakeStarted?.Invoke();
            if (coroutine != null) EditorCoroutineUtility.StopCoroutine(coroutine);
            coroutine = EditorCoroutineUtility.StartCoroutineOwnerless(BakeCoroutine());
        }

        static IEnumerator BakeCoroutine()
        {
            float lastBakeProgress = 0;
            double bakeStuckTime = 0;
            DateTime lastTime = DateTime.Now;
            while (Lightmapping.isRunning)
            {
                if (lastBakeProgress == Lightmapping.buildProgress)
                {
                    bakeStuckTime += (DateTime.Now - lastTime).TotalSeconds;
                }
                else
                {
                    lastBakeProgress = Lightmapping.buildProgress;
                    bakeStuckTime = 0;
                }

                lastTime = DateTime.Now;

                if (bakeStuckTime > 600)
                {
                    Debug.LogError("Bake seem to be stuck since 10min, stopping...");
                    System.Media.SystemSounds.Beep.Play();
                    CancelBake();

                    lastBakeResult = BakeResult.Timeout;
                    bakeInProgress = false;
                    coroutine = null;
                    onBakeCompleted?.Invoke(lastBakeResult);
                    onBakeLateCompleted?.Invoke(lastBakeResult);
                }

                if (stopBakeIfCPU && Lightmapping.lightingSettings.lightmapper == LightingSettings.Lightmapper.ProgressiveCPU)
                {
                    Debug.LogError("Lightmapper switched to CPU!");
                    System.Media.SystemSounds.Beep.Play();
                    CancelBake();

                    lastBakeResult = BakeResult.SwitchedToCPU;
                    stopBakeIfCPU = false;
                    bakeInProgress = false;
                    coroutine = null;
                    onBakeCompleted?.Invoke(lastBakeResult);
                    onBakeLateCompleted?.Invoke(lastBakeResult);

                    yield break;
                }
                yield return null;
            }

            Debug.LogError("Lightmaps baking canceled");
            lastBakeResult = BakeResult.CanceledByUser;
            stopBakeIfCPU = false;
            bakeInProgress = false;
            coroutine = null;
            onBakeCompleted?.Invoke(lastBakeResult);
            onBakeLateCompleted?.Invoke(lastBakeResult);
        }

        static void OnBakeCompleted()
        {
            Debug.Log("Lightmaps baking completed");
            if (coroutine != null) EditorCoroutineUtility.StopCoroutine(coroutine);
            stopBakeIfCPU = false;
            bakeInProgress = false;
            coroutine = null;
            lastBakeResult = BakeResult.Successfull;
            onBakeCompleted?.Invoke(lastBakeResult);
            onBakeLateCompleted?.Invoke(lastBakeResult);

        }
#endif
    }
}