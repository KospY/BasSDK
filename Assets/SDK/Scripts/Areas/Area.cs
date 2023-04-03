using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Collections;
using System.Linq;
using Unity.Jobs;
using Unity.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class Area : MonoBehaviour
    {
        #region InternalClass
        public class BlendAudioSource
        {
            private AudioSource audioSource;
            private FxModuleAudio audioFx;
            private float orgVolume;
            public BlendAudioSource(AudioSource audioSource)
            {
                FxModuleAudio audioFx = audioSource.GetComponentInParent<FxModuleAudio>();
                if(audioFx != null && audioFx.audioSource == audioSource)
                {
                    this.audioFx = audioFx;
                    this.orgVolume = EffectAudio.DecibelToLinear(audioFx.volumeDb);
                    this.audioSource = null;
                    return;
                }

                this.audioFx = null;
                this.audioSource = audioSource;
                this.orgVolume = audioSource.volume;
            }

            /// <summary>
            /// Apply volume as lerp between 0 and original volume
            /// </summary>
            /// <param name="t"> t the lerp parameter (t = 1.0f will apply original volume)</param>
            public void ApplyVolume(float t)
            {
                float newVolume = Mathf.Lerp(0, this.orgVolume, t);
                if (audioFx != null)
                {
                    audioFx.volumeDb = EffectAudio.LinearToDecibel(newVolume);
                    audioFx.Refresh();
                    return;
                }

                audioSource.volume = newVolume;
            }
        }

        [Serializable]
        public class MeshColliderRef
        {
            public bool convex;
            public Mesh mesh;
        }

        public struct BakeJob : IJobParallelFor
        {
            // see doc : https://docs.unity3d.com/2019.4/Documentation/ScriptReference/Physics.BakeMesh.html
            public const int MESH_PER_JOB = 10;
            private NativeArray<int> meshIds;
            private NativeArray<bool> meshConvex;

            public BakeJob(NativeArray<int> meshIds, NativeArray<bool> meshConvex)
            {
                this.meshIds = meshIds;
                this.meshConvex = meshConvex;
            }

            public void Execute(int index)
            {
                Physics.BakeMesh(meshIds[index], meshConvex[index]);
            }
        }

        #endregion InternalClass

        #region Fields
        #region SerializedFields        
#if UNITY_EDITOR
        [Header("CatalogData")]
        [Tooltip("Contains the Area ID")]
        public DataIdContainer<AreaData> areaData;
        public int lightingPresetIndex = 0;
        [Header("Tool boundary")]
        public bool boundIncludeInactive;
        public bool boundIncludeDisableOnPlay;

        [Tooltip("Ignores the reference mesh when a boundery bounds is calculated")]
        public Renderer[] boundRendererToIgnore;

#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public bool modifiedInImport = false;
#endif //UNITY_EDITOR

        [Header("Culling")]
#if ODIN_INSPECTOR
        [InlineButton("UpdateCullingVolume", "Update Volume")]
#endif
        [Tooltip("Spawns the area across a number of frames during load time (Recommended 2)")]
        public float spawnRoomObjectsAcrossFrames = 2;

        [Header("Audio blend")]
#if ODIN_INSPECTOR
        [InlineButton("AddLoopedAudioSources")]
#endif
        [Tooltip("Lists looping audio sources which will blend between eachother.")]
        public List<AudioSource> audioSourcesToBlend = new List<AudioSource>();

        [Tooltip("This is the root for everyting that should not be disabled when culled")]
        public GameObject rootNoCulling;

        // Hiden SerializedFields
        [HideInInspector]
        public LightingGroup lightingGroup;
        [HideInInspector]
        public Ocean[] oceans;
        [HideInInspector]
        public UnityEngine.VFX.VisualEffect[] visualEffects;
        [HideInInspector]
        public ParticleSystem[] particlesSystem;
        [HideInInspector]
        public AreaGateway[] gateways;
        [HideInInspector]
        public List<PlayerSpawner> playerSpawners = new List<PlayerSpawner>();
        [HideInInspector]
        public ItemSpawnerLimiter itemLimiterSpawner = null;
        [HideInInspector]
        public List<CreatureSpawner> creatureSpawners = new List<CreatureSpawner>();
        [HideInInspector]
        public List<CreatureSpawner> creatureNoLimiteSpawners = new List<CreatureSpawner>();
        [HideInInspector]
        public ReflectionProbe[] reflectionProbes;
        [HideInInspector]
        public LightProbeVolume[] lightProbeVolumes;
        [HideInInspector]
        public MeshRenderer[] meshRenderers;
        [HideInInspector]
        public List<MeshColliderRef> meshColliderRefList = new List<MeshColliderRef>();
        [HideInInspector]
        public List<Collider> colliderList = new List<Collider>();
        #endregion SerializedFields

        #region NonSerializedFields
#if ODIN_INSPECTOR
        [ShowInInspector, Sirenix.OdinInspector.ReadOnly]
#endif
        [NonSerialized]
        public Dictionary<int, AreaGateway> connectedGateways = new Dictionary<int, AreaGateway>();

#if ODIN_INSPECTOR
        [ShowInInspector, Sirenix.OdinInspector.ReadOnly]
#endif
        [NonSerialized]
        public bool initialized;

        [Header("Culling")]
#if ODIN_INSPECTOR
        [ShowInInspector, Sirenix.OdinInspector.ReadOnly]
#endif
        [NonSerialized]
        public List<BlendAudioSource> blendAudioSources;

#if ODIN_INSPECTOR
        [ShowInInspector, Sirenix.OdinInspector.ReadOnly]
#endif
        [NonSerialized]
        public List<Item> items = new List<Item>();

#if ODIN_INSPECTOR
        [ShowInInspector, Sirenix.OdinInspector.ReadOnly]
#endif
        [NonSerialized]
        public List<Creature> creatures = new List<Creature>();

#if ODIN_INSPECTOR
        [ShowInInspector, Sirenix.OdinInspector.ReadOnly]
#endif
        [NonSerialized]
        public bool isCulled;

#if ODIN_INSPECTOR
        [ShowInInspector, Sirenix.OdinInspector.ReadOnly]
#endif
        [NonSerialized]
        public bool isHidden;

        [NonSerialized]
        public SpawnableArea spawnableArea;

        [NonSerialized]
        public bool IsActive = false;

        protected Coroutine toggleRoomObjectCoroutine;
        #endregion NonSerializedFields
        #endregion Fields

        #region Properties
        public bool IsInitialized { get { return initialized; } }
        #endregion Properties

        #region Event
        [Header("Event")]
        public UnityEvent onPlayerEnter = new UnityEvent();
        public UnityEvent onPlayerExit = new UnityEvent();
        public UnityEvent onInitialized = new UnityEvent();

        public delegate void CullChangeEvent(bool isCulled);
        public event CullChangeEvent onCullChange;

        public delegate void HideChangeEvent(bool isHide);
        public event HideChangeEvent onHideChange;
        #endregion Event

        #region Methods
        public void Init(SpawnableArea spawnableArea, bool inLoadingMenu)
        {
            this.spawnableArea = spawnableArea;

            System.Random rng = new System.Random(Level.seed + spawnableArea.managedId);

            // Disable all player spawner (reactivate if we need it later)
            DisableAllPlayerSpawner();

            // Audio ambient loop to blend
            blendAudioSources = new List<BlendAudioSource>();
            int audioSourcesToBlendCount = audioSourcesToBlend.Count;
            for (int i = 0; i < audioSourcesToBlendCount; i++)
            {
                AudioSource audioSource = audioSourcesToBlend[i];
                if (audioSource)
                {
                    BlendAudioSource blendAudioSource = new BlendAudioSource(audioSource);
                    blendAudioSources.Add(blendAudioSource);
                    blendAudioSource.ApplyVolume(0.0f);
                }
            }

            if (gateways != null)
            {
                for (int indexGateway = 0; indexGateway < gateways.Length; indexGateway++)
                {
                    SpawnableArea.ConnectedArea connectedAreaInfo = spawnableArea.GetConnectedArea(spawnableArea.AreaDataId, indexGateway);
                    if (connectedAreaInfo == null)
                    {
                        gateways[indexGateway].gameObject.SetActive(false);
                    }
                    else
                    {
                        gateways[indexGateway].Init(spawnableArea, indexGateway, connectedAreaInfo.connectedArea, connectedAreaInfo.connectedAreaConnectionIndex);
                        connectedGateways.Add(indexGateway, gateways[indexGateway]);
                    }
                }
            }
            else
            {
                Debug.LogError("No Gateway in area : " + spawnableArea.AreaDataId);
            }

            if(rootNoCulling == null)
            {
                Debug.LogError("Area " + spawnableArea.AreaDataId + " has no RootNoCulling. \nSomething went wrong with the area import");
                return;
            }

            // Light volume
            GameObject rootLightProbeVolumes = new GameObject("LightProbeVolumes");
            rootLightProbeVolumes.transform.SetParentOrigin(rootNoCulling.transform);

            int lightProbeVolumesCount = lightProbeVolumes.Length;
            for (int i = 0; i < lightProbeVolumesCount; i++)
            {
                // Isolate probe volume to avoid separating them from script (colliders being moved after)
                lightProbeVolumes[i].transform.SetParent(rootLightProbeVolumes.transform, true);
            }

            int colliderCount = colliderList.Count;
            for (int i = 0; i < colliderCount; i++)
            {
                if (colliderList[i] == null)
                {
                    Debug.LogError("A collider is null in area " + spawnableArea.AreaDataId);
                    continue;
                }

                colliderList[i].enabled = true;

            }

            StartCoroutine(InitCoroutine(inLoadingMenu, rng));
        }

        protected bool IsAdditionalNonLOD0Collider(LODGroup lODGroup, Collider collider)
        {
            LOD[] lods = lODGroup.GetLODs();

            if (lods.Length == 0)
                return false;
            if (!LODHaveRenderers(lods[0]))
                return false;

            for (int indexLods = 1; indexLods < lods.Length; indexLods++)
            {
                Renderer[] renderers = lods[indexLods].renderers;
                int rendererCount = renderers.Length;
                for (int indexRenderer = 0; indexRenderer < rendererCount; indexRenderer++)
                {
                    Renderer renderer = renderers[indexRenderer];
                    if (!renderer)
                        continue;

                    Collider[] lodColliders = renderer.gameObject.GetComponentsInChildren<Collider>();
                    int lodColliderCount = lodColliders.Length;
                    for (int indexLodCollider = 0; indexLodCollider < lodColliderCount; indexLodCollider++)
                    {
                        Collider lodCollider = lodColliders[indexLodCollider];
                        if (lodCollider == collider)
                            return true;
                    }
                }
            }
            return false;
        }

        protected bool LODHaveRenderers(LOD lod)
        {
            int rendererCount = lod.renderers.Length;
            for (int i = 0; i < rendererCount; i++)
            {
                if (lod.renderers[i] != null)
                {
                    return true;
                }
            }
            return false;
        }

        IEnumerator InitCoroutine(bool inLoadingMenu, System.Random rng)
        {

            int reflectionProbesCount = reflectionProbes.Length;
            for (int i = 0; i < reflectionProbesCount; i++)
            {
                ReflectionProbe reflectionProbe = reflectionProbes[i];
                // Rotate reflection probes
                InitReflectionProbeRotation(reflectionProbe);
                // Force Custom parameters
                reflectionProbe.mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
                reflectionProbe.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.ViaScripting;
                reflectionProbe.timeSlicingMode = inLoadingMenu ? UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.NoTimeSlicing : UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.IndividualFaces;
                // Capture cubemap
                int renderId = reflectionProbe.RenderProbe();

                if (!reflectionProbe.IsFinishedRendering(renderId))
                {
                    yield return Yielders.EndOfFrame;
                }

                while (!reflectionProbe.realtimeTexture)
                {
                    yield return Yielders.EndOfFrame;
                }

                reflectionProbe.bakedTexture = reflectionProbe.realtimeTexture;
                reflectionProbe.realtimeTexture = null;
                reflectionProbe.mode = UnityEngine.Rendering.ReflectionProbeMode.Baked;
            }


            // Wait lighting group to be initialized
            if (lightingGroup != null)
            {
                while (!lightingGroup.initialized)
                {
                    yield return null;
                }

                StaticBatch();
            }

            if (itemLimiterSpawner != null)
            {
                itemLimiterSpawner.randomGen = rng;
                itemLimiterSpawner.SpawnAll();

                // Wait for all item to spawn
                while (itemLimiterSpawner.IsItemSpawning())
                {
                    yield return null;
                }
            }


            yield return InitCullCoroutine(spawnableArea.IsCulled);
            ForceHide(isHidden);
            initialized = true;
            onInitialized?.Invoke();
        }


        public void Shuffle<T>(IList<T> list, System.Random randomGen)
        {
            int n = list.Count;

            while (n > 1)
            {
                n--;
                int k = randomGen.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public IEnumerator BakeMeshCollider()
        {
            if (meshColliderRefList == null || meshColliderRefList.Count == 0)
            {
                yield break;
            }

            int meshColliderCount = meshColliderRefList.Count;
            NativeArray<int> meshIds = new NativeArray<int>(meshColliderCount, Allocator.Persistent);
            NativeArray<bool> meshConvex = new NativeArray<bool>(meshColliderCount, Allocator.Persistent);

            for (int i = 0; i < meshColliderCount; ++i)
            {
                if (meshColliderRefList[i] == null || meshColliderRefList[i].mesh == null)
                {
                    yield break;
                }

                meshIds[i] = meshColliderRefList[i].mesh.GetInstanceID();
                meshConvex[i] = meshColliderRefList[i].convex;
            }

            // This spreads the expensive operation over all cores.
            BakeJob job = new BakeJob(meshIds, meshConvex);
            JobHandle jobHandle = job.Schedule(meshIds.Length, BakeJob.MESH_PER_JOB);

            while (!jobHandle.IsCompleted)
            {
                yield return null;
            }

            jobHandle.Complete(); // job is already complete here but we need to call complete to avoid log error

            meshIds.Dispose();
            meshConvex.Dispose();
        }

        private void OnDestroy()
        {
            // As we set the renderTexture to baked texture, we have to release the render texture from memory manually
            if (reflectionProbes != null)
            {
                int reflectionProbeCount = reflectionProbes.Length;
                for (int i = 0; i < reflectionProbeCount; i++)
                {
                    ReflectionProbe reflectionProbe = reflectionProbes[i];
                    if (reflectionProbe.realtimeTexture)
                    {
                        reflectionProbe.realtimeTexture.Release();
                    }
                    if (reflectionProbe.bakedTexture && (reflectionProbe.bakedTexture is RenderTexture))
                    {
                        (reflectionProbe.bakedTexture as RenderTexture).Release();
                    }
                }
            }

        }

        public void DisableAllPlayerSpawner()
        {
            if (playerSpawners == null) return;
            int count = playerSpawners.Count;
            for (int i = 0; i < count; i++)
            {
                playerSpawners[i].gameObject.SetActive(false);
            }
        }

        public PlayerSpawner GetPlayerSpawner()
        {
            if (playerSpawners.Count == 0) return null;

            else if (playerSpawners.Count == 1) return playerSpawners[0];

            PlayerSpawner defaultspawner = playerSpawners[0];
            float val = UnityEngine.Random.value * 100;
            for (int i = playerSpawners.Count - 1; i >= 0; i--)
            {

                // Default the spawners to 50 if set to -1.
                if (playerSpawners[i].spawnWeight == -1)
                {
                    playerSpawners[i].spawnWeight = 50;
                }

                if (playerSpawners[i].spawnWeight < val)
                {
                    playerSpawners.Remove(playerSpawners[i]);
                }
            }

            //return playerSpawners.Count == 0 ? defaultspawner : playerSpawners[UnityEngine.Random.Range(0, playerSpawners.Count)];
            PlayerSpawner selectedSpawner = playerSpawners.Count == 0 ? defaultspawner : playerSpawners[UnityEngine.Random.Range(0, playerSpawners.Count)];
            if (!selectedSpawner.gameObject.activeInHierarchy)
            {
                selectedSpawner.transform.parent = transform;
                selectedSpawner.gameObject.SetActive(true);
            }

            return selectedSpawner;
        }

        public void InitReflectionProbeRotation(ReflectionProbe reflectionProbe)
        {
            // Keep the original center and size if you want to use more than one time
            Bounds bounds = new Bounds(reflectionProbe.center, reflectionProbe.size);

            Matrix4x4 mat = Matrix4x4.TRS(Vector3.zero, transform.rotation, transform.localScale);
            Vector3 newMax = mat.MultiplyPoint3x4(bounds.max);
            Vector3 newMin = mat.MultiplyPoint3x4(bounds.min);

            bounds = new Bounds();
            bounds.Encapsulate(newMax);
            bounds.Encapsulate(newMin);

            reflectionProbe.center = bounds.center;
            reflectionProbe.size = bounds.size;
        }

        [Button]
        public void StaticBatch()
        {
            if (Catalog.gameData.dungeonStaticBatching)
            {
                List<GameObject> objectsToBatch = new List<GameObject>();
                int meshRendererCount = meshRenderers.Length;
                for (int i = 0; i < meshRendererCount; i++)
                {
                    MeshRenderer meshRenderer = meshRenderers[i];
                    if (meshRenderer.lightmapIndex >= 0 && !meshRenderer.gameObject.CompareTag("NoRoomStaticBatching")) // We can't check if static is checked at runtime, so check if lightmap exist
                    {
                        objectsToBatch.Add(meshRenderer.gameObject);
                    }
                }
                StaticBatchingUtility.Combine(objectsToBatch.ToArray(), this.gameObject);
                Debug.Log("Static batched " + objectsToBatch.Count + " objects for room " + this.name);
            }
        }

        public void OnPlayerEnter(Area previousArea)
        {
            IsActive = true;

            lightingGroup.ApplySceneSettings(applySun: false);

            if (blendAudioSources != null)
            {
                int blendAudioSourceCount = blendAudioSources.Count;
                for (int i = 0; i < blendAudioSourceCount; i++)
                {
                    blendAudioSources[i].ApplyVolume(1.0f);
                }
            }

            onPlayerEnter.Invoke();

            if (connectedGateways != null)
            {
                foreach (KeyValuePair<int, AreaGateway> pair in connectedGateways)
                {
                    pair.Value.OnPlayerChangeArea(previousArea, this);
                }
            }


            Transform playerTransform = PlayerTest.local.transform;
            if (!CheckActifGateways(playerTransform.position))
            {
                spawnableArea.ApplyGlobalParameters(true, false, null);
            }
        }

        public void OnPlayerExit(Area newArea)
        {
            IsActive = false;
            onPlayerExit.Invoke();

            foreach (KeyValuePair<int, AreaGateway> pair in connectedGateways)
            {
                pair.Value.OnPlayerChangeArea(this, newArea);
            }
        }

        public bool CheckActifGateways(Vector3 playerPosition)
        {
            bool isActifGateway = false;
            foreach (KeyValuePair<int, AreaGateway> pair in connectedGateways)
            {
                if (pair.Value.CheckActif(playerPosition))
                {
                    isActifGateway = true;
                }
            }

            return isActifGateway;
        }

        /// <summary>
        /// Blend audio with parameter t
        /// </summary>
        /// <param name="t"> when t = 0 volume is 0, when t is 1 volume will be the original one</param>
        public void BlendAudio(float t)
        {
            foreach (Area.BlendAudioSource blendAudioSource in blendAudioSources)
            {
                blendAudioSource.ApplyVolume(t);
            }
        }

        /// <summary>
        /// Blend between Current light preset and the area preset
        /// This does not change the current preset
        /// </summary>
        /// <param name="t">When t is 0 current level preset is fully apply, when t is 1 this area preset is fully apply</param>
        public void BlendLight(float t)
        {
            lightingGroup.BlendSceneSettingsWithCurrent(t,showDebugLine: false, applySun: false);
        }


        public void RegisterCreature(Creature creature)
        {
            if (!creatures.Contains(creature))
                creatures.Add(creature);
        }

        public void UnRegisterCreature(Creature creature)
        {
            if (creatures.Contains(creature))
                creatures.Remove(creature);
        }

        public void SetCull(bool cull)
        {
            if (!initialized)
            {
                return;
            }

            if (cull && !isCulled)
            {
                isCulled = true;
                foreach (Transform transform in this.transform)
                {
                    if (transform == rootNoCulling.transform)
                        continue;
                    transform.gameObject.SetActive(false);
                }


                if (onCullChange != null)
                {
                    onCullChange.Invoke(true);
                }
            }
            else if (!cull && isCulled)
            {
                isCulled = false;
                foreach (Transform transform in this.transform)
                {
                    if (transform == rootNoCulling.transform)
                        continue;
                    transform.gameObject.SetActive(true);
                }

            }
        }

        public IEnumerator InitCullCoroutine (bool cull)
        {
            if (cull && !isCulled)
            {
                isCulled = true;
                foreach (Transform transform in this.transform)
                {
                    if (transform == rootNoCulling.transform)
                        continue;
                    transform.gameObject.SetActive(false);
                }


                if (onCullChange != null)
                {
                    onCullChange.Invoke(true);
                }
            }
            else if (!cull && isCulled)
            {
                isCulled = false;
                foreach (Transform transform in this.transform)
                {
                    if (transform == rootNoCulling.transform)
                        continue;
                    transform.gameObject.SetActive(true);
                }

            }
            yield break;
        }

        public void Hide(bool hide)
        {
            if (!initialized)
            {
                // do not hide before initialized (reflection probe bake)
                isHidden = hide;
                return;
            }

            if (hide != isHidden)
            {
                ForceHide(hide);
            }
        }

        private void ForceHide(bool hide)
        {
            // Hide Gate
            foreach (KeyValuePair<int, AreaGateway> pair in connectedGateways)
            {
                pair.Value.gameObject.SetActive(!hide);
            }

            // Hide Ocean
            if (oceans != null)
            {
                for (int i = 0; i < oceans.Length; i++)
                {
                    oceans[i].gameObject.SetActive(!hide);
                }
            }

            // Hide VisualEffect
            if (visualEffects != null)
            {
                for (int i = 0; i < visualEffects.Length; i++)
                {
                    visualEffects[i].enabled = !hide;
                }
            }

            // Hide Particle Systems
            if (particlesSystem != null)
            {
                for (int i = 0; i < particlesSystem.Length; i++)
                {
                    particlesSystem[i].gameObject.SetActive(!hide);
                }
            }


            isHidden = hide;

            if (onHideChange != null)
            {
                onHideChange.Invoke(isHidden);
            }
        }


        #endregion Methods

        #region Editor
#if UNITY_EDITOR
        public void OnImport(UnityEditor.AssetPostprocessor postProcess)
        {
            lightingGroup = this.GetComponent<LightingGroup>();

            oceans = GetComponentsInChildren<Ocean>();
            visualEffects = GetComponentsInChildren<UnityEngine.VFX.VisualEffect>();
            particlesSystem = GetComponentsInChildren<ParticleSystem>();
            gateways = GetComponentsInChildren<AreaGateway>();

            playerSpawners = new List<PlayerSpawner>(this.GetComponentsInChildren<PlayerSpawner>(true));

            itemLimiterSpawner = GetComponent<ItemSpawnerLimiter>();
            if (itemLimiterSpawner != null)
            {
                itemLimiterSpawner.spawnOnStart = false;
            }

            CreatureSpawner[] allCreatureSpawner = this.GetComponentsInChildren<CreatureSpawner>();
            creatureSpawners = new List<CreatureSpawner>();
            creatureNoLimiteSpawners = new List<CreatureSpawner>();
            for (int i = 0; i < allCreatureSpawner.Length; i++)
            {
                allCreatureSpawner[i].spawnOnStart = false;
                if (allCreatureSpawner[i].ignoreRoomMaxNPC)
                {
                    creatureNoLimiteSpawners.Add(allCreatureSpawner[i]);
                }
                else
                {
                    creatureSpawners.Add(allCreatureSpawner[i]);
                }
            }

            // Set Reflection probe to baked

            reflectionProbes = GetComponentsInChildren<ReflectionProbe>();

            foreach (ReflectionProbe reflectionProbe in reflectionProbes)
            {
                reflectionProbe.mode = UnityEngine.Rendering.ReflectionProbeMode.Baked;
                reflectionProbe.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.ViaScripting;
            }

            lightProbeVolumes = GetComponentsInChildren<LightProbeVolume>(true);
            meshRenderers = GetComponentsInChildren<MeshRenderer>(true);


            // Set collider to no culling
            if (rootNoCulling == null)
            {
                rootNoCulling = new GameObject("NoCulling");
                postProcess.context.AddObjectToAsset("NoCulling", rootNoCulling);
            }
            rootNoCulling.transform.SetParentOrigin(this.transform);

            // Copy all colliders to a dedicated gameobject and destroy original ones
            GameObject rootStaticColliders = new GameObject("StaticColliders");
            postProcess.context.AddObjectToAsset("StaticColliders", rootStaticColliders);

            rootStaticColliders.transform.SetParentOrigin(rootNoCulling.transform);

            meshColliderRefList.Clear();
            colliderList = new List<Collider>();
            HashSet<int> presentMesh = new HashSet<int>();

            int count = 0;
            foreach (Collider collider in GetComponentsInChildren<Collider>())
            {
                if (!isActiveInArea(collider.gameObject))
                {
                    continue;
                }
                if (collider.isTrigger)
                {
                    continue;
                }
                if (!collider.enabled)
                {
                    continue;
                }
                if (collider.gameObject.GetComponentInParent<Rigidbody>(true))
                {
                    continue;
                }

                DisableOnCondition disableOnCondition = collider.gameObject.GetComponentInParent<DisableOnCondition>(true);
                if (disableOnCondition)
                {
                    if (disableOnCondition.condition == DisableOnCondition.Condition.OnPlay || disableOnCondition.condition == DisableOnCondition.Condition.OnRoomCulled)
                    {
                        continue;
                    }
                }

                if (collider.gameObject.GetComponentInParent<Zone>(true))
                {
                    continue;
                }


                GameObject colliderGo = collider.gameObject;

                // Destroy LOD1,2,3 colliders if any
                LODGroup lODGroup = collider.transform.GetComponentInParent<LODGroup>(true);
                if (lODGroup && IsAdditionalNonLOD0Collider(lODGroup, collider))
                {
                    //Destroy(collider);
                    collider.enabled = false;
                    continue;
                }

                // Isolate colliders as disabling them is costly
                GameObject colGo = new GameObject(collider.name);
                collider.Clone(colGo);
                colGo.name += $"({count})";
                colGo.layer = collider.gameObject.layer;
                colGo.transform.SetParent(rootStaticColliders.transform);
                colGo.transform.position = collider.transform.position;
                colGo.transform.rotation = collider.transform.rotation;
                colGo.transform.SetGlobalScale(collider.transform.lossyScale);
                //Destroy(collider);
                collider.enabled = false;

                Collider newCollider = colGo.GetComponent<Collider>();

                if (newCollider is MeshCollider meshCollider)
                {
                    meshCollider.enabled = false;
                    if (meshCollider.sharedMesh != null && presentMesh.Add(meshCollider.sharedMesh.GetInstanceID()))
                    {
                        MeshColliderRef meshColliderRef = new MeshColliderRef();
                        meshColliderRef.convex = meshCollider.convex;
                        Mesh mesh = meshCollider.sharedMesh;
                        meshColliderRef.mesh = mesh;
                        meshColliderRefList.Add(meshColliderRef);

                        if (!mesh.isReadable)
                        {
                            string meshPath = AssetDatabase.GetAssetPath(mesh);
                            if (ModelImporter.GetAtPath(meshPath) is ModelImporter meshImporter)
                            {
                                meshImporter.isReadable = true;
                                EditorUtility.SetDirty(meshImporter);
                                AssetDatabase.WriteImportSettingsIfDirty(meshPath);
                            }
                        }
                    }
                }

                colliderList.Add(newCollider);

                postProcess.context.AddObjectToAsset("Collider" + count, newCollider.gameObject);
                count++;
            }

            // Check That area audio loader on import has no audio preload 
            foreach(AudioLoader audioLoader in GetComponentsInChildren<AudioLoader>())
            {
                AudioClip audioClip = null;
                if (audioLoader.useAudioClipAddress)
                {
                    audioClip = Catalog.EditorLoad<AudioClip>(audioLoader.audioClipAddress);
                }
                else
                {
                    audioClip = audioLoader.audioClipReference.editorAsset;
                }

                if (audioClip != null)
                {
                    if (!audioClip.loadInBackground)
                    {
                        string audioPath = AssetDatabase.GetAssetPath(audioClip);
                        if (AudioImporter.GetAtPath(audioPath) is AudioImporter audioImporter)
                        {
                            audioImporter.loadInBackground = true;
                            EditorUtility.SetDirty(audioImporter);
                            AssetDatabase.WriteImportSettingsIfDirty(audioPath);
                        }
                    }
                }
            }

            AssetDatabase.SaveAssets();
            modifiedInImport = true;
        }

        private bool isActiveInArea(GameObject go)
        {
            if (go == gameObject) return true;
            if (!go.activeSelf) return false;

            return isActiveInArea(go.transform.parent.gameObject);
        }

        [Button]
        public void ActiveCollider()
        {
            foreach (Collider collider in colliderList)
            {
                if (collider != null)
                {
                    collider.enabled = true;
                }
            }
        }

        [Button]
        public void DisableMeshCollider()
        {
            foreach (Collider collider in colliderList)
            {
                if (collider is MeshCollider)
                {
                    collider.enabled = false;
                }
            }
        }

        [Button]
        public void ResetImport()
        {
            ActiveCollider();

            lightingGroup = null;
            oceans = null;
            visualEffects = null;
            particlesSystem = null;
            gateways = null;
            playerSpawners = null;
            itemLimiterSpawner = null;
            creatureSpawners = null;
            reflectionProbes = null;
            lightProbeVolumes = null;
            meshRenderers = null;
            rootNoCulling = null;
            meshColliderRefList = null;
            colliderList = null;

        }

        [Button]
        public void AddSelectionToBoundRendererIgnorer()
        {
            HashSet<Renderer> tempHash = new HashSet<Renderer>();

            if (boundRendererToIgnore != null)
            {
                foreach (Renderer temp in boundRendererToIgnore)
                {
                    tempHash.Add(temp);
                }
            }

            foreach (GameObject go in Selection.gameObjects)
            {
                Renderer renderer = go.GetComponent<Renderer>();
                if (renderer != null)
                {
                    tempHash.Add(renderer);
                }

                Renderer[] childRenderer = go.GetComponentsInChildren<Renderer>();
                foreach (Renderer child in childRenderer)
                {
                    tempHash.Add(child);
                }

                Collider col = go.GetComponent<Collider>();
                if (col != null)
                {
                    Debug.LogError("A collider is present in go : " + go.name);
                }

                Collider[] colChild = go.GetComponentsInChildren<Collider>();
                if (colChild.Length > 0)
                {
                    Debug.LogError("A collider is present in go : " + go.name);
                }
            }

            boundRendererToIgnore = tempHash.ToArray();
        }

        [Button]
        public void UpdateDataBounds()
        {
            Catalog.EditorLoadAllJson();
            AreaData data = areaData.Data;
            if (data == null) return;

            Bounds bounds = new Bounds();
            bool hasBounds = false;

            // Renderers
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>(boundIncludeInactive))
            {
                if (boundRendererToIgnore != null)
                {
                    bool isIgnored = false;
                    for (int i = 0; i < boundRendererToIgnore.Length; i++)
                    {
                        if (renderer == boundRendererToIgnore[i])
                        {
                            isIgnored = true;
                            break;
                        }
                    }

                    if (isIgnored)
                    {
                        continue;
                    }
                }

                if (renderer is SpriteRenderer) continue;
                if (renderer is ParticleSystemRenderer) continue;
                if (!boundIncludeDisableOnPlay)
                {
                    DisableOnCondition disableOnCondition = renderer.GetComponentInParent<DisableOnCondition>();
                    if (disableOnCondition && disableOnCondition.condition == DisableOnCondition.Condition.OnPlay) continue;
                }
                if (renderer.gameObject.layer == 1) continue; // Ignore transparent VFX
                if (renderer.gameObject.layer == 2) continue; // igonore ignore raycast
                if (renderer.gameObject.layer == 4) continue; // Ignore water
                if (renderer.gameObject.layer == 5) continue; // Ignore UI

                if (hasBounds)
                {
                    bounds.Encapsulate(renderer.bounds);
                }
                else
                {
                    bounds = renderer.bounds;
                }
                hasBounds = true;
            }

            // Colliders
            ActiveCollider();
            foreach (Collider collider in GetComponentsInChildren<Collider>(boundIncludeInactive))
            {
                if (collider.isTrigger) continue;
                if (collider.gameObject.layer == 1) continue; // Ignore transparent VFX
                if (collider.gameObject.layer == 2) continue; // igonore ignore raycast
                if (collider.gameObject.layer == 4) continue; // Ignore water
                if (collider.gameObject.layer == 5) continue; // Ignore UI
                if (!boundIncludeDisableOnPlay)
                {
                    DisableOnCondition disableOnCondition = collider.GetComponentInParent<DisableOnCondition>();
                    if (disableOnCondition && disableOnCondition.condition == DisableOnCondition.Condition.OnPlay) continue;
                }
                if (hasBounds)
                {
                    bounds.Encapsulate(collider.bounds);
                }
                else
                {
                    bounds = collider.bounds;
                }
                hasBounds = true;
            }

            DisableMeshCollider();

            // Fix any zero or negative extents
            const float minExtents = 0.01f;
            Vector3 extents = bounds.extents;

            if (extents.x == 0f)
            {
                extents.x = minExtents;
            }
            else if (extents.x < 0f)
            {
                extents.x *= -1f;
            }

            if (extents.y == 0f)
            {
                extents.y = minExtents;
            }
            else if (extents.y < 0f)
            {
                extents.y *= -1f;
            }

            if (extents.z == 0f)
            {
                extents.z = minExtents;
            }
            else if (extents.z < 0f)
            {
                extents.z *= -1f;
            }

            bounds.extents = extents;
            data.Bounds = bounds;
            data.ForceBoudaryStopAtConnection();

            Catalog.SaveToJson(data);
        }

        [Button]
        public void UpdateDataConnectionPositionAndDirection()
        {
            Catalog.EditorLoadAllJson();
            AreaData data = areaData.Data;
            if (data == null) return;

            if (data.connections == null)
            {
                data.connections = new List<AreaData.AreaConnection>();
            }

            List<AreaData.AreaConnection> connectionList = new List<AreaData.AreaConnection>();
            AreaGateway[] gateways = GetComponentsInChildren<AreaGateway>();
            for (int i = 0; i < gateways.Length; i++)
            {
                AreaData.AreaConnection connection = new AreaData.AreaConnection();
                connectionList.Add(connection);

                connection.connectionTypeIdContainerList = new List<AreaData.AreaConnectionTypeIdContainer>();
                connection.position = gateways[i].transform.position;
                AreaRotationHelper.Face face;
                if (AreaRotationHelper.TryGetFaceFromQuaterion(gateways[i].transform.rotation, out face))
                {
                    connection.face = face;
                }
                else
                {
                    Debug.LogError("Cannot find a proper Face for gateway index : " + i);
                }
            }

            int count = Math.Min(data.connections.Count, connectionList.Count);
            for (int i = 0; i < count; i++)
            {
                connectionList[i].connectionTypeIdContainerList = data.connections[i].connectionTypeIdContainerList;
                if (connectionList[i].connectionTypeIdContainerList == null)
                {
                    connectionList[i].connectionTypeIdContainerList = new List<AreaData.AreaConnectionTypeIdContainer>();
                }

                connectionList[i].overrideBlockerTableAdress = data.connections[i].overrideBlockerTableAdress;
            }

            data.connections = connectionList;

            Catalog.SaveToJson(data);
        }


        private void OnDrawGizmos()
        {
            if (spawnableArea != null)
            {
                Gizmos.color = Color.magenta;
                Bounds bounds = spawnableArea.Bounds;
                Gizmos.DrawWireCube(bounds.center, bounds.size);
            }
            else if (Application.isPlaying == false && areaData.Data != null)
            {
                Gizmos.matrix = this.transform.localToWorldMatrix;
                Gizmos.color = Color.magenta;
                Bounds bounds = areaData.Data.Bounds;
                Gizmos.DrawWireCube(bounds.center, bounds.size);

            }
        }


        public void AddLoopedAudioSources()
        {
            foreach (AudioSource audioSource in this.GetComponentsInChildren<AudioSource>())
            {
                if (audioSource.loop)
                {
                    if (!audioSourcesToBlend.Contains(audioSource))
                        audioSourcesToBlend.Add(audioSource);
                }
            }
            UnityEditor.EditorUtility.SetDirty(this.gameObject);
        }
#endif

        #endregion Editor
    }
}