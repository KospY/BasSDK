using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
#if UNITY_EDITOR
using System.IO;
#endif //UNITY_EDITOR
namespace ThunderRoad
{

    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Areas/AreaGateway.html")]
    public class AreaGateway : MonoBehaviour, ILiteMemoryToggle
    {
        #region Fields
        public Bounds localBounds;

        [Header("Fade")]
        public float fadeMaxDistance = 15f;
        [Range(0, 1)]
        public float lightBlendMaxDistanceRatio = 0.25f;
        [Range(0, 1)]
        public float fadeRoomMinDistanceRatio = 0.6f;
        [Range(0, 1)]
        public float fadeAudioMinDistanceRatio = 0.1f;

        [NonSerialized]
        public ReflectionSorcery fakeView;
        [NonSerialized]
        public Matrix4x4 capturedMatrix;
        [NonSerialized]
        public bool isFakeviewData;
        [NonSerialized]
        public bool isFakeviewDataInUse;


        [NonSerialized]
        public Area area;
        [NonSerialized]
        public int indexConnection;

        [NonSerialized]
        public SpawnableArea connectedSpawnableArea;
        [NonSerialized]
        public Area connectedArea;
        [NonSerialized]
        public AreaData.AreaConnection connectedConnection;

        private bool _isInitialized = false;

        private Bounds _worldBounds;
        private bool _isActive = false;

        private const float FAKE_VIEW_FADE_TIME = 0.3f;
        private const float AUDIO_FADE_TIME = 3.0f;
        private float _fakeviewOpacity = 0.0f;
        private Coroutine _fakeviewFadeCoroutine = null;
        private Coroutine _audioFadeCoroutine = null;
        private bool _firstCheckAfterAreaEnter = false;

        private float _maxBlendAudio;
        private float _lerpBlendAudio;
        #endregion Fields

        #region Properties
        /// <summary>
        /// 1.0 is opaque, 0.0 is transparent
        /// </summary>
        public float FakeviewOpacity
        {
            get { return _fakeviewOpacity; }
            set
            {
                _fakeviewOpacity = value;
                if (_fakeviewOpacity > 1.0f) _fakeviewOpacity = 1.0f;
                if (_fakeviewOpacity < 0.0f) _fakeviewOpacity = 0.0f;
                if (fakeView == null)
                {
                    return;
                }

                fakeView.SetFade(_fakeviewOpacity);
            }
        }

        public float MaxBlendAudio
        {
get => _maxBlendAudio;
set => _maxBlendAudio = value;
        }

        public float LerpBlendAudio
        {
get => _lerpBlendAudio;
set => _lerpBlendAudio = value;
        }

        #endregion Properties

        #region Methods
        public MonoBehaviour GetMonoBehaviourReference()
        {
            return this;
        }

        public Bounds GetWorldBounds()
        {
            Vector3 boundsWorldCenter = transform.TransformPoint(localBounds.center);
            Vector3 boundsWorldSize = transform.rotation * localBounds.size;

            boundsWorldSize.x = Mathf.Abs(boundsWorldSize.x);
            boundsWorldSize.y = Mathf.Abs(boundsWorldSize.y);
            boundsWorldSize.z = Mathf.Abs(boundsWorldSize.z);

            return new Bounds(boundsWorldCenter, boundsWorldSize);
        }

        public void SetLiteMemory(bool isInLiteMemory)
        { }
 //ProjectCore
        #endregion Methods

        #region Tool
#if UNITY_EDITOR
 //ProjectCore

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, fadeMaxDistance * fadeAudioMinDistanceRatio);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, fadeMaxDistance * lightBlendMaxDistanceRatio);
            Gizmos.color = Color.gray;
            //Gizmos.DrawWireSphere(this.transform.position, fadeMaxDistance * fadeRoomMinDistanceRatio);
            Gizmos.DrawWireSphere(this.transform.position, fadeMaxDistance);
        }


        // This is a button only to set up the fake view correctly the first time in scene.
        public void SetupFakeView(AreaData areaData, int indexGateway, string prefabAdress)
        {
            List<AreaData.AreaConnectionTypeIdContainer> listConnectionType = areaData.connections[indexGateway].connectionTypeIdContainerList;
            if (listConnectionType == null
                || listConnectionType.Count == 0)
            {
                Debug.LogError($"No Connection type in area : {areaData.id} for connection index : {indexGateway}");
                return;
            }

            Vector2 size = Vector2.zero;
            foreach (AreaData.AreaConnectionTypeIdContainer connectionTypeId in listConnectionType)
            {
                if (connectionTypeId == null) continue;
                AreaConnectionTypeData connectionType = connectionTypeId.Data;
                if (connectionType == null) continue;

                Vector2 tempSize = connectionType.size;
                if (tempSize == null) continue;

                if (tempSize.x > size.x) size.x = tempSize.x;
                if (tempSize.y > size.y) size.y = tempSize.y;
            }

            // Gateway position is on the bottom of the gateway if horrizontal so we need to add half the vlue to be on the center
            Vector3 newFakeViewPosition = transform.position;
            bool isVertical = areaData.connections[indexGateway].IsVertical();
            if (!isVertical)
            {
                newFakeViewPosition += Vector3.up * size.y / 2.0f;
            }

            ReflectionSorcery reflectionSorcery = GetComponentInChildren<ReflectionSorcery>();
            if (reflectionSorcery == null)
            {
                // rotation must be identity in world space
                GameObject newFakeView = UnityEngine.Object.Instantiate(new GameObject(), newFakeViewPosition, Quaternion.identity, transform);
                reflectionSorcery = newFakeView.AddComponent<ReflectionSorcery>();
            }
            else
            {
                // rotation must be identity in world space
                reflectionSorcery.transform.position = newFakeViewPosition;
                reflectionSorcery.transform.rotation = Quaternion.identity;
            }

            Quaternion connectionRotation = transform.rotation * Quaternion.Euler(0.0f, 180.0f, 0.0f);
            reflectionSorcery.Setup();

            Transform renderVolume = reflectionSorcery.transform.GetChild(0);
            renderVolume.localPosition = Vector3.zero;
            renderVolume.rotation = connectionRotation;
            renderVolume.localScale = new Vector3(size.x, size.y, 0.0f);

            Bounds worldbounds = GetWorldBounds();

            reflectionSorcery.roomVolumePosition = worldbounds.center - reflectionSorcery.transform.position;
            reflectionSorcery.roomVolumeRotation = Vector3.zero;
            reflectionSorcery.roomVolumeScale = worldbounds.size;

            if (isVertical)
            {
                reflectionSorcery.capturePosition = Vector3.up * 0.5f;
            }
            else
            {
                reflectionSorcery.capturePosition = Vector3.up *  2.0f / 3.0f;
            }

            PassRotationMatrix passRotationMatrix = renderVolume.GetComponent<PassRotationMatrix>();
            passRotationMatrix.Setup();

            string areaFolderPath = Path.GetDirectoryName(prefabAdress);
            string fakesourceName = $"{areaData.id}_{indexGateway}";

            reflectionSorcery.SetCaptureName(areaFolderPath, fakesourceName);
        }

        public void BakeFakeViewData(AreaData areaData, int indexGateway, string prefabAdress)
        {

            ReflectionSorcery fakeView = GetComponentInChildren<ReflectionSorcery>();
            FakeViewData fakeViewData = null;
            string fakeViewDataPath = Path.Combine(fakeView.FolderLocation, fakeView.CaptureName + "_FakeView.asset");
            if (string.IsNullOrEmpty(areaData.connections[indexGateway].fakeViewAddress))
            {
                // Create FakeView Data
                fakeViewData = Common.EditorCreateOrReplaceAsset(ScriptableObject.CreateInstance<FakeViewData>(), fakeViewDataPath);
            }
            else
            {
                fakeViewData = Catalog.EditorLoad<FakeViewData>(areaData.connections[indexGateway].fakeViewAddress);
                if (fakeViewData == null)
                {
                    Debug.LogError("Can not find fake view data for Area : " + areaData.id + " with connection " + indexConnection + "Create a new one");
                    // Create FakeView Data
                    string newFakeViewDataPath = Path.Combine(fakeView.FolderLocation, fakeView.CaptureName + "_FakeView.asset");
                    fakeViewData = Common.EditorCreateOrReplaceAsset(ScriptableObject.CreateInstance<FakeViewData>(), newFakeViewDataPath);
                }
                else
                {
                    string path = UnityEditor.AssetDatabase.GetAssetPath(fakeViewData);
                    if (!path.Equals(fakeViewDataPath))
                    {
                        // rename
                        UnityEditor.AssetDatabase.RenameAsset(path, fakeView.CaptureName + "_FakeView.asset");
                    }
                }

            }

            // Set Cubemap
            fakeViewData.resolution = fakeView.resolution;
            fakeViewData.mask = fakeView.mask;
            fakeViewData.capturePosition = fakeView.capturePosition;
            fakeViewData.roomVolumePosition = fakeView.roomVolumePosition;
            fakeViewData.roomVolumeRotation = fakeView.roomVolumeRotation;
            fakeViewData.roomVolumeScale = fakeView.roomVolumeScale;

            fakeView.Capture();
            string texturePath = fakeView.FolderLocation + "/" + fakeView.CaptureName + ".exr";
            fakeViewData.captureTexture = UnityEditor.AssetDatabase.LoadAssetAtPath<Cubemap>(texturePath);
            fakeViewData.capturedMatrix = fakeView.CapturedMatrix;

            UnityEditor.EditorUtility.SetDirty(fakeViewData);
            UnityEditor.AssetDatabase.SaveAssets();
        }
#endif //UNITY_EDITOR
        #endregion Tool
    }
}