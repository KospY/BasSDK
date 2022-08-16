using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR;
using UnityEngine.XR.Management;

namespace ThunderRoad
{

    public class PlayerTest : MonoBehaviour
    {
        public static PlayerTest local;
        public Camera cam;
        [NonSerialized]
        public UniversalAdditionalCameraData cameraData;
        public float moveSpeed = 4.0f;
        public float turnSpeed = 4.0f;
        public float jumpForce = 20;

        protected new Rigidbody rigidbody;
        protected new CapsuleCollider collider;

        public delegate void SpawnEvent(PlayerTest player);
        public static event SpawnEvent onSpawn;

        void Awake()
        {
            local = this;
            if (Level.master)
            {
                Destroy(this.gameObject);
                return;
            }
            cam = this.GetComponentInChildren<Camera>();
            cameraData = cam.GetComponent<UniversalAdditionalCameraData>();
            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<CapsuleCollider>();
            StartCoroutine(LoadXR());
        }

        void Start()
        {
#if PrivateSDK
            if (Level.current && Level.current.dungeon && !Level.current.dungeon.initialized)
            {
                Level.current.dungeon.onDungeonGenerated += OnDungeonGenerated;
                rigidbody.isKinematic = true;
            }
#endif
            if (onSpawn != null) onSpawn.Invoke(this);
        }

        private IEnumerator LoadXR()
        {
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
            if (XRGeneralSettings.Instance.Manager.activeLoader != null)
            {
                XRGeneralSettings.Instance.Manager.StartSubsystems();
                InputDevice headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
                while (!headDevice.isValid)
                {
                    headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
                yield return new WaitForSeconds(1);
                }
                Time.fixedDeltaTime = Time.timeScale / XRDevice.refreshRate;
                Time.maximumDeltaTime = Time.fixedDeltaTime * 2;
            }
        }

#if DUNGEN
        private void OnDungeonGenerated(EventTime eventTime)
        {
            if (eventTime == EventTime.OnEnd)
            {
                PlayerSpawner playerSpawner = Level.current.dungeon.rooms[0].GetPlayerSpawner();
                if (playerSpawner)
                {
                    Level.current.dungeon.playerTransform = cam.transform;
                    this.transform.SetPositionAndRotation(playerSpawner.transform.position, playerSpawner.transform.rotation);
                }
                rigidbody.isKinematic = false;
            }
        }
#endif

        private void OnDisable()
        {
            if (XRGeneralSettings.Instance.Manager.activeLoader != null)
            {
                XRGeneralSettings.Instance.Manager.StopSubsystems();
            }
        }

        private void OnDestroy()
        {
            if (XRGeneralSettings.Instance.Manager.activeLoader != null)
            {
                XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            }
        }

        void FixedUpdate()
        {
            collider.center = new Vector3(this.transform.InverseTransformPoint(cam.transform.position).x, collider.center.y, this.transform.InverseTransformPoint(cam.transform.position).z);

            InputDevice leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            if (leftDevice.isValid)
            {
                // Move
                leftDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 axis);
                Vector3 moveDirection = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0) * new Vector3(axis.x, 0, axis.y);
                moveDirection *= moveSpeed;
                if (moveDirection.magnitude < 0.1f) moveDirection = Vector3.zero;
                rigidbody.velocity = new Vector3(moveDirection.x, rigidbody.velocity.y, moveDirection.z);
            }

            InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            if (rightDevice.isValid)
            {
                // Turn
                rightDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 axis);
                if (axis.x > 0.1f || axis.x < -0.1f) this.transform.RotateAround(cam.transform.position, Vector3.up, axis.x * turnSpeed);

                // Jump
                rightDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool axisClick);
                rightDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool buttonClick);
                if (axisClick || buttonClick)
                {
                    rigidbody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Zone"))
            {
                Zone zone = other.GetComponent<Zone>();
                if (!zone) return;
                if (zone.teleportPlayer)
                {
                    this.transform.position = zone.customTeleportTarget ? zone.customTeleportTarget.position : PlayerSpawner.current.transform.position;
                    this.transform.rotation = zone.customTeleportTarget ? zone.customTeleportTarget.rotation : PlayerSpawner.current.transform.rotation;
                }
                zone.playerEnterEvent.Invoke(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Zone"))
            {
                Zone zone = other.GetComponent<Zone>();
                zone.playerExitEvent.Invoke(this);
            }
        }
    }
}