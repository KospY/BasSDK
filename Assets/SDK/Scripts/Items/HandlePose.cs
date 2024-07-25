using UnityEngine;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Items/HandlePose.html")]
	public class HandlePose : MonoBehaviour
    {
        [Tooltip("References the handle this handpose is attached to.")]
        public Handle handle;
        [Tooltip("Depicts which hand this handpose directs to.")]
        public Side side = Side.Right;
        [Tooltip("ID of the handpose that is set to default if the target weight is zero.")]
        [CatalogPicker(new[] {Category.HandPose})]
        public string defaultHandPoseId = "HandleDefault";

        [Tooltip("A per-HandlePose override for the Handle's SpellOrbTarget")]
        public Transform spellOrbTarget;
        [NonSerialized]
        public HandPoseData defaultHandPoseData;
        protected HandPoseData.Pose defaultHandPose;

        [Range(0f, 1f)]
        [Tooltip("Blends the \"Default\" handpose and the \"Target\" handpose, allowing you to create more unique and fitting handposes without needing to create new ones.")]
        public float targetWeight;
        [NonSerialized]
        public float lastTargetWeight = -1;

        [Tooltip("ID of the handpose that is used to blend against the default handpose. Handpose that is used if the target weight is one.")]
        [CatalogPicker(new[] {Category.HandPose})]
        public string targetHandPoseId;
        [NonSerialized]
        public HandPoseData targetHandPoseData;
        protected HandPoseData.Pose targetHandPose;

        private void Awake()
        {
            if (!handle) handle = this.GetComponentInParent<Handle>();
        }

        private void Start()
        {
            LoadHandPosesData();
        }

        public void LoadHandPosesData()
        {
            defaultHandPoseData = Catalog.GetData<HandPoseData>(defaultHandPoseId) ?? 
                                  Catalog.GetData<HandPoseData>("HandleDefault");
            targetHandPoseData = Catalog.GetData<HandPoseData>(targetHandPoseId) ??
                                  Catalog.GetData<HandPoseData>("HandleDefault");
        }

#if UNITY_EDITOR

        [Header("Editor Only")]
        [Tooltip("Allows you to select a creature to test the handpose on.")]
        public Creature creature;
        [CatalogCreatureNamePicker()]
        [Tooltip("Uses the ID of the creature to ensure that the hand bones are correct.")]
        public string creatureName = "HumanMale";

        
#if UNITY_EDITOR
        public virtual void OnValidate()
        {
#if TESTINGLOCALLY
            return;
#endif
            if (!this.InPrefabScene() && !Application.isBatchMode)
            {
                UnityEditor.EditorApplication.delayCall += _OnValidate;
            }
        }
        private void _OnValidate()
        {
            UnityEditor.EditorApplication.delayCall -= _OnValidate;
            if(this == null) return;
            if (!gameObject.activeInHierarchy) return;
            if (!handle) handle = this.GetComponentInParent<Handle>();
            if (!Application.isPlaying)
            {
#if UNITY_EDITOR
                
                UpdateName();
                if (creature)
                {
                    Catalog.EditorLoadAllJson();
                    defaultHandPoseData = Catalog.GetData<HandPoseData>(defaultHandPoseId);
                    if (defaultHandPoseData != null) defaultHandPose = defaultHandPoseData.GetCreaturePose(creatureName);

                    targetHandPoseData = Catalog.GetData<HandPoseData>(targetHandPoseId);
                    if (targetHandPoseData != null) targetHandPose = targetHandPoseData.GetCreaturePose(creatureName);
                    foreach (RagdollHand hand in creature.GetComponentsInChildren<RagdollHand>())
                    {
                        if (hand.side == Side.Right) creature.handRight = hand;
                        if (hand.side == Side.Left) creature.handLeft = hand;
                    }
                    creature.GetHand(side).poser.defaultHandPoseData = defaultHandPoseData;
                    creature.GetHand(side).poser.targetHandPoseData = targetHandPoseData;
                    creature.GetHand(side).poser.globalRatio = true;
                    creature.GetHand(side).poser.EditorRefreshPose(creature);
                    creature.GetHand(side).poser.SetTargetWeight(targetWeight);
                }
#endif
            }
        }
#endif
        

        [Button]
        public void EditorCreatureGrab()
        {
            if (creature)
            {
                Item item = handle.GetComponentInParent<Item>();
                Transform alignObject = handle.transform.root;
                if (item != null) alignObject = item.transform;

                Transform objectGrip = new GameObject("ObjectGrip").transform;
                objectGrip.SetParent(item ? item.transform : handle.transform.root);
                objectGrip.position = this.transform.position + (handle.transform.up * handle.GetDefaultAxisLocalPosition());
                objectGrip.rotation = this.transform.rotation;

                foreach (RagdollHand hand in creature.GetComponentsInChildren<RagdollHand>())
                {
                    if (hand.side == Side.Right) creature.handRight = hand;
                    if (hand.side == Side.Left) creature.handLeft = hand;
                }
                creature.GetHand(side).poser.SetGripFromPose(defaultHandPoseData);
                creature.GetHand(side).poser.targetWeight = targetWeight;
                creature.GetHand(side).poser.EditorRefreshPose(creature);

                alignObject.MoveAlign(objectGrip, creature.GetHand(side).grip.position, creature.GetHand(side).grip.rotation);

                DestroyImmediate(objectGrip.gameObject);
            }
        }

        protected void OnDrawGizmosSelected()
        {
            if (Catalog.gameData != null)
            {
                if (defaultHandPoseData == null || defaultHandPoseData.id != defaultHandPoseId)
                {
                    defaultHandPoseData = Catalog.GetData<HandPoseData>(defaultHandPoseId);
                    if (defaultHandPoseData != null) defaultHandPose = defaultHandPoseData.GetCreaturePose(creatureName);
                }

                if (targetHandPoseData == null || targetHandPoseData.id != targetHandPoseId)
                {
                    targetHandPoseData = Catalog.GetData<HandPoseData>(targetHandPoseId);
                    if (targetHandPoseData != null) targetHandPose = targetHandPoseData.GetCreaturePose(creatureName);
                }
            }
            if (!handle) handle = GetComponentInParent<Handle>();
            Vector3 defaultAxisPosition = transform.position;
            Vector3 offset = -transform.position;
            if (handle)
            {
                defaultAxisPosition = handle.GetDefaultAxisPosition(side);
                offset = -handle.transform.position;
            }
            if (defaultHandPose != null && CheckQuaternion(defaultHandPose.GetFingers(side).gripLocalRotation))
            {
                Matrix4x4 gripMatrix = Matrix4x4.TRS(defaultAxisPosition + (transform.position + offset), this.transform.rotation * Quaternion.Inverse(defaultHandPose.GetFingers(side).gripLocalRotation), Vector3.one);
                gripMatrix *= Matrix4x4.TRS(-defaultHandPose.GetFingers(side).gripLocalPosition, Quaternion.identity, Vector3.one);

                Gizmos.matrix = gripMatrix;
                Gizmos.color = Common.HueColourValue(HueColorName.Red);

                // Need to spin the transform so the transform.TransformPoint works correctly for left hand
                Quaternion originalRotation = transform.rotation;
                if (side == Side.Left) transform.rotation *= Quaternion.Euler(0f, 0f, 180f);
                Vector3 rootLocalPosition = gripMatrix.inverse.MultiplyPoint3x4(this.transform.TransformPoint(defaultHandPose.GetFingers(side).rootLocalPosition) + (defaultAxisPosition + offset) + (transform.position + offset));
                if (side == Side.Left) transform.rotation = originalRotation;

                if (targetWeight > 0 && targetHandPoseId != null && targetHandPoseId != "" && targetHandPose != null)
                {
                    Gizmos.DrawLine(rootLocalPosition, Vector3.Lerp(defaultHandPose.GetFingers(side).thumb.proximal.localPosition, targetHandPose.GetFingers(side).thumb.proximal.localPosition, targetWeight));
                    Gizmos.DrawLine(rootLocalPosition, Vector3.Lerp(defaultHandPose.GetFingers(side).index.proximal.localPosition, targetHandPose.GetFingers(side).index.proximal.localPosition, targetWeight));
                    Gizmos.DrawLine(rootLocalPosition, Vector3.Lerp(defaultHandPose.GetFingers(side).middle.proximal.localPosition, targetHandPose.GetFingers(side).middle.proximal.localPosition, targetWeight));
                    Gizmos.DrawLine(rootLocalPosition, Vector3.Lerp(defaultHandPose.GetFingers(side).ring.proximal.localPosition, targetHandPose.GetFingers(side).ring.proximal.localPosition, targetWeight));
                    Gizmos.DrawLine(rootLocalPosition, Vector3.Lerp(defaultHandPose.GetFingers(side).little.proximal.localPosition, targetHandPose.GetFingers(side).little.proximal.localPosition, targetWeight));
                    Gizmos.color = Common.HueColourValue(HueColorName.Orange);
                    GizmoDrawFinger(defaultHandPose.GetFingers(side).thumb, targetHandPose.GetFingers(side).thumb, gripMatrix);
                    GizmoDrawFinger(defaultHandPose.GetFingers(side).index, targetHandPose.GetFingers(side).index, gripMatrix);
                    GizmoDrawFinger(defaultHandPose.GetFingers(side).middle, targetHandPose.GetFingers(side).middle, gripMatrix);
                    GizmoDrawFinger(defaultHandPose.GetFingers(side).ring, targetHandPose.GetFingers(side).ring, gripMatrix);
                    GizmoDrawFinger(defaultHandPose.GetFingers(side).little, targetHandPose.GetFingers(side).little, gripMatrix);
                }
                else
                {
                    Gizmos.DrawLine(rootLocalPosition, defaultHandPose.GetFingers(side).thumb.proximal.localPosition);
                    Gizmos.DrawLine(rootLocalPosition, defaultHandPose.GetFingers(side).index.proximal.localPosition);
                    Gizmos.DrawLine(rootLocalPosition, defaultHandPose.GetFingers(side).middle.proximal.localPosition);
                    Gizmos.DrawLine(rootLocalPosition, defaultHandPose.GetFingers(side).ring.proximal.localPosition);
                    Gizmos.DrawLine(rootLocalPosition, defaultHandPose.GetFingers(side).little.proximal.localPosition);
                    Gizmos.color = Common.HueColourValue(HueColorName.Orange);
                    GizmoDrawFinger(defaultHandPose.GetFingers(side).thumb, null, gripMatrix);
                    GizmoDrawFinger(defaultHandPose.GetFingers(side).index, null, gripMatrix);
                    GizmoDrawFinger(defaultHandPose.GetFingers(side).middle, null, gripMatrix);
                    GizmoDrawFinger(defaultHandPose.GetFingers(side).ring, null, gripMatrix);
                    GizmoDrawFinger(defaultHandPose.GetFingers(side).little, null, gripMatrix);
                }
            }
        }

#endif

        public void UpdateName()
        {
            if (handle)
            {
                if (side == Side.Right)
                {
                    this.name = "OrientRight" + (handle.orientationDefaultRight == this ? "_Default" : "");
                }
                else if (side == Side.Left)
                {
                    this.name = "OrientLeft" + (handle.orientationDefaultLeft == this ? "_Default" : "");
                }
            }
        }

        protected void GizmoDrawFinger(HandPoseData.Pose.Finger finger, HandPoseData.Pose.Finger finger2, Matrix4x4 matrix)
        {
            Vector3 proximalLocalPosition = finger.proximal.localPosition;
            Quaternion proximalLocalRotation = finger.proximal.localRotation;

            Vector3 intermediateLocalPosition = finger.intermediate.localPosition;
            Quaternion intermediateLocalRotation = finger.intermediate.localRotation;

            Vector3 distalLocalPosition = finger.distal.localPosition;
            Quaternion distalLocalRotation = finger.distal.localRotation;

            Vector3 tipLocalPosition = finger.tipLocalPosition;

            if (finger2 != null)
            {
                proximalLocalPosition = Vector3.Lerp(finger.proximal.localPosition, finger2.proximal.localPosition, targetWeight);
                proximalLocalRotation = Quaternion.Lerp(finger.proximal.localRotation, finger2.proximal.localRotation, targetWeight);

                intermediateLocalPosition = Vector3.Lerp(finger.intermediate.localPosition, finger2.intermediate.localPosition, targetWeight);
                intermediateLocalRotation = Quaternion.Lerp(finger.intermediate.localRotation, finger2.intermediate.localRotation, targetWeight);

                distalLocalPosition = Vector3.Lerp(finger.distal.localPosition, finger2.distal.localPosition, targetWeight);
                distalLocalRotation = Quaternion.Lerp(finger.distal.localRotation, finger2.distal.localRotation, targetWeight);

                tipLocalPosition = Vector3.Lerp(finger.tipLocalPosition, finger2.tipLocalPosition, targetWeight);
            }

            Gizmos.matrix = matrix;
            Gizmos.DrawWireSphere(proximalLocalPosition, 0.006f);

            if (CheckQuaternion(proximalLocalRotation))
            {
                Gizmos.matrix *= Matrix4x4.TRS(proximalLocalPosition, proximalLocalRotation, Vector3.one);
                Gizmos.DrawWireSphere(intermediateLocalPosition, 0.005f);
                Gizmos.DrawLine(Vector3.zero, intermediateLocalPosition);
            }

            if (CheckQuaternion(intermediateLocalRotation))
            {
                Gizmos.matrix *= Matrix4x4.TRS(intermediateLocalPosition, intermediateLocalRotation, Vector3.one);
                Gizmos.DrawWireSphere(distalLocalPosition, 0.003f);
                Gizmos.DrawLine(Vector3.zero, distalLocalPosition);
            }

            if (CheckQuaternion(distalLocalRotation))
            {
                Gizmos.matrix *= Matrix4x4.TRS(distalLocalPosition, distalLocalRotation, Vector3.one);
                Gizmos.DrawWireSphere(tipLocalPosition, 0.002f);
                Gizmos.DrawLine(Vector3.zero, tipLocalPosition);
            }
        }

        protected bool CheckQuaternion(Quaternion quaternion)
        {
            if ((quaternion.x + quaternion.y + quaternion.z + quaternion.w) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}