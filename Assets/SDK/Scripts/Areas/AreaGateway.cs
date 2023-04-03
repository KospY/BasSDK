using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
#if UNITY_EDITOR
using System.IO;
#endif //UNITY_EDITOR
namespace ThunderRoad
{
    
    public class AreaGateway : MonoBehaviour
    {
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
        public Area area;
        [NonSerialized]
        public int indexConnection;

        [NonSerialized]
        public SpawnableArea connectedSpawnableArea;
        [NonSerialized]
        public Area connectedArea;
        [NonSerialized]
        public int connectedAreaIndexConnection;

        private Bounds _worldBounds;
        private bool _isActive = false;

        private const float FAKE_VIEW_FADE_TIME = 0.3f;
        private float _fakeviewOpacity = 0.0f;
        private Coroutine _fakeviewFadeCoroutine = null;
        private bool _firstCheckAfterAreaEnter = false;

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

        public void Init(SpawnableArea spawnableArea, int indexGateway, SpawnableArea connectedSpawnableArea, int connectedAreaIndexConnection)
        {
            area = spawnableArea.SpawnedArea;
            this.indexConnection = indexGateway;

            InitFakeView(spawnableArea, indexGateway, connectedSpawnableArea);

            this.connectedAreaIndexConnection = connectedAreaIndexConnection;
            this.connectedSpawnableArea = connectedSpawnableArea;

            connectedSpawnableArea.SpawnAreaEvent += OnConnectedAreaSpawn;
            connectedSpawnableArea.DespawnAreaEvent += OnConnectedAreaDeSpawn;
            if (connectedSpawnableArea.IsSpawned)
            {
                OnConnectedAreaSpawn(connectedSpawnableArea);
            }

            // Compute WorldBounds

            _worldBounds = GetWorldBounds();
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

        public void InitFakeView(SpawnableArea spawnableArea, int indexGateway, SpawnableArea connectedSpawnableArea)
        {
            fakeView = this.GetComponentInChildren<ReflectionSorcery>();
            if (fakeView == null) return;

            FakeViewData fakeViewData = spawnableArea.GetFakeViewData(indexGateway);
            if (fakeViewData == null)
            {
                fakeView.gameObject.SetActive(false);
                fakeView = null;
            }
            else
            {
                float connectedAreaRotationAngle = AreaRotationHelper.GetRotationDegreeFromRotation(connectedSpawnableArea.Rotation);
                Quaternion connectedAreaRotation = Quaternion.Euler(0.0f, -connectedAreaRotationAngle, 0.0f);

                float areaRotationAngle = AreaRotationHelper.GetRotationDegreeFromRotation(spawnableArea.Rotation);
                Quaternion areaRotation = Quaternion.Euler(0.0f, areaRotationAngle, 0.0f);

                Quaternion rotation = Quaternion.Euler(0.0f, connectedAreaRotationAngle - areaRotationAngle, 0.0f);

                // Flip renderer
                PassRotationMatrix renderer = fakeView.GetComponentInChildren<PassRotationMatrix>();
                renderer.transform.rotation *= Quaternion.Euler(0.0f, 180.0f, 0.0f);

                fakeView.resolution = fakeViewData.resolution;
                fakeView.mask = fakeViewData.mask;
                fakeView.capturePosition = fakeViewData.capturePosition;

                Matrix4x4 capturedMatrix = Matrix4x4.Rotate(connectedAreaRotation * areaRotation);
                bool isVertical = spawnableArea.GetConnection(spawnableArea.AreaDataId, indexGateway).IsVertical();
                if (isVertical)
                {
                    fakeView.roomVolumePosition = fakeViewData.roomVolumePosition;
                    fakeView.roomVolumeRotation = fakeViewData.roomVolumeRotation;
                    fakeView.roomVolumeScale = fakeViewData.roomVolumeScale;
                    capturedMatrix = fakeViewData.capturedMatrix;

                    fakeView.transform.rotation *= rotation;
                }
                else
                {
                    fakeView.roomVolumePosition = rotation * fakeViewData.roomVolumePosition;
                    fakeView.roomVolumeRotation = fakeViewData.roomVolumeRotation;
                    Vector3 roomVolumeScale = rotation * fakeViewData.roomVolumeScale;
                    roomVolumeScale.x = Mathf.Abs(roomVolumeScale.x);
                    roomVolumeScale.y = Mathf.Abs(roomVolumeScale.y);
                    roomVolumeScale.z = Mathf.Abs(roomVolumeScale.z);
                    fakeView.roomVolumeScale = roomVolumeScale;
                    capturedMatrix = Matrix4x4.Rotate(connectedAreaRotation * areaRotation) * fakeViewData.capturedMatrix;
                }

                fakeView.SetCaptureTexture(fakeViewData.captureTexture, capturedMatrix);
            }

        }

        public void OnDestroy()
        {
            if (connectedSpawnableArea != null)
            {
                connectedSpawnableArea.SpawnAreaEvent -= OnConnectedAreaSpawn;
                connectedSpawnableArea.DespawnAreaEvent -= OnConnectedAreaDeSpawn;
            }

            CancelInvoke("SlowUpdate");
        }

        private void OnEnable()
        {
            if (_isActive)
            {
                InvokeRepeating("SlowUpdate", 0, 0.1f);
            }
        }

        private void OnDisable()
        {
            CancelInvoke("SlowUpdate");
        }


        private void OnConnectedAreaSpawn(SpawnableArea spawnableArea)
        {
            connectedArea = spawnableArea.SpawnedArea;
            if (fakeView != null
                && !_isActive
                && connectedArea != null
                && !connectedArea.IsActive)
            {
                connectedArea.Hide(true);
            }
        }

        private void OnConnectedAreaDeSpawn(SpawnableArea spawnableArea)
        {
            connectedArea = null;
        }

        public bool CheckActif(Vector3 playerPosition)
        {
            if (_worldBounds.Contains(playerPosition))
            {
                OnPlayerEnter();
                _firstCheckAfterAreaEnter = false;
                return true;
            }
            else
            {
                if (_firstCheckAfterAreaEnter)
                {
                    FakeviewOpacity = 1.0f;

                    if (connectedArea != null
                        && fakeView != null
                        && !_isActive)
                    {
                        connectedArea.Hide(true);
                    }
                }

                OnPlayerExit();
                _firstCheckAfterAreaEnter = false;
                return false;
            }
        }

        public void OnPlayerChangeArea(Area oldArea, Area newArea)
        {
            if (newArea
                && newArea == area)
            {
                area.Hide(false);
                _firstCheckAfterAreaEnter = true;
            }
            else if(oldArea && oldArea == area)
            {
                OnPlayerExit();
            }
        }

        public void OnPlayerEnter()
        {
            if (_isActive)
            {
                return;
            }

            _isActive = true;

            if (_firstCheckAfterAreaEnter)
            {
                if (connectedArea != null
                    && !connectedArea.IsActive)
                {
                    connectedArea.Hide(false);
                }

                FakeviewOpacity = 0.0f;
            }
            else
            {
                if (fakeView != null
                    && isActiveAndEnabled)
                {
                    // Start opacity coroutine
                    if (_fakeviewFadeCoroutine != null)
                    {
                        StopCoroutine(_fakeviewFadeCoroutine);
                    }

                    _fakeviewFadeCoroutine = StartCoroutine(fakeviewFadeTransparent());
                }
            }

            CancelInvoke("SlowUpdate");
            SlowUpdate();
            InvokeRepeating("SlowUpdate", 0.1f, 0.1f);
            if (connectedArea != null)
            {
                connectedArea.Hide(false);
            }

            area.spawnableArea.ApplyGlobalParameters(true, true, connectedSpawnableArea);
            connectedSpawnableArea.ApplyGlobalParameters(false, true, area.spawnableArea);
        }

        public void OnPlayerExit()
        {
            if (!_isActive)
            {
                return;
            }

            _isActive = false;
            CancelInvoke("SlowUpdate");
            if (area.IsActive)
            {
                if (fakeView != null
                    && isActiveAndEnabled)
                {
                    // Start opacity coroutine
                    if (_fakeviewFadeCoroutine != null)
                    {
                        StopCoroutine(_fakeviewFadeCoroutine);
                    }

                    _fakeviewFadeCoroutine = StartCoroutine(fakeviewFadeOpaque());
                }

                connectedArea.BlendAudio(0.0f);
                area.spawnableArea.ApplyGlobalParameters(true, false, connectedSpawnableArea);
                connectedArea.BlendLight(0.0f);
            }
            else
            {
                FakeviewOpacity = 1.0f;
            }
        }

        protected void SlowUpdate()
        {
            if (connectedArea == null)
            {
                return;
            }


            Transform playerTransform = PlayerTest.local.cam.transform;

            if (this.transform.position.PointInRadius(playerTransform.transform.position, fadeMaxDistance, out float doorDistanceRatio))
            {
                doorDistanceRatio = 1 - doorDistanceRatio; // 1 is further, 0 is closest

                float blendAudio = 1.0f - Mathf.InverseLerp(fadeAudioMinDistanceRatio, 1, doorDistanceRatio);
                connectedArea.BlendAudio(blendAudio);

                // The current light is this from the current area lighting so we want to blend at max half of the connected area (when we are the closets of the gate)
                float blendLight = 0.5f - Mathf.InverseLerp(0, lightBlendMaxDistanceRatio, doorDistanceRatio) / 2.0f;
                connectedArea.BlendLight(blendLight);
            }
            else
            {
                connectedArea.BlendAudio(0.0f);
                connectedArea.BlendLight(0.0f);
            }
        }

        private IEnumerator fakeviewFadeTransparent()
        {
            if (connectedArea != null
                && !connectedArea.IsActive)
            {
                connectedArea.Hide(false);
            }

            while (FakeviewOpacity > 0.0f)
            {
                yield return null;
                FakeviewOpacity -= Time.deltaTime / FAKE_VIEW_FADE_TIME;
            }

            _fakeviewFadeCoroutine = null;
        }

        private IEnumerator fakeviewFadeOpaque()
        {
            while (FakeviewOpacity < 1.0f)
            {
                yield return null;
                FakeviewOpacity += Time.deltaTime / FAKE_VIEW_FADE_TIME;
            }

            yield return null;

            if (connectedArea != null
                && !connectedArea.IsActive)
            {
                connectedArea.Hide(true);
            }

            _fakeviewFadeCoroutine = null;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, fadeMaxDistance * fadeAudioMinDistanceRatio);
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(this.transform.position, fadeMaxDistance * fadeRoomMinDistanceRatio);
            Gizmos.DrawWireSphere(this.transform.position, fadeMaxDistance);
        }

        #region Tool
#if UNITY_EDITOR
        public void SetupFakeView(AreaData areaData, int indexGateway, string prefabAdress)
        {
            List<AreaData.AreaConnectionTypeIdContainer> listConnectionType = areaData.connections[indexGateway].connectionTypeIdContainerList;
            if (listConnectionType == null
                || listConnectionType.Count == 0)
            {
                Debug.LogError("No Connection type in area : " + areaData.id + " for connection index : " + indexGateway);
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

            size += Vector2.one * 0.2f; // Enlarge a bit to be sure the gate is covering all.
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
            string fakesourceName = Path.GetFileNameWithoutExtension(areaFolderPath);
            fakesourceName += "_connection_" + indexGateway + "_Texture";

            reflectionSorcery.SetCaptureName(areaFolderPath, fakesourceName);
        }
#endif //UNITY_EDITOR
        #endregion Tool
    }
}