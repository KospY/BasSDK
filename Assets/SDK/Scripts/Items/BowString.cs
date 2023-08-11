﻿using System;
using UnityEngine;
using System.Collections.Generic;

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
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/BowString")]
    public class BowString : ThunderBehaviour
    {
        [Header("Draw and animation")]
        public new Animation animation;
        [Tooltip("This allows you to adjust the animation time so that the pink line matches where your bow is drawn to better.")]
        public AnimationCurve pullCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        [Tooltip("Defines how far your bow string can be pulled (in meters). This gets set automatically by the auto-configure, but can be manually adjusted if you feel it's wrong.")]
        public float stringDrawLength = 0.5f;
        [Tooltip("Set the minimum speed for the bow to fire an arrow.")]
        public float minFireVelocity = 4;
        [Range(0f, 0.1f)]
        [Tooltip("Defines the minimum distance the handle has to move to register a pull happening.")]
        public float minPull = 0.01f;
        [Tooltip("As the pull difficulty increases, the player's hand will become weaker. Allows you to make it \"tougher\" to achieve full draw.")]
        public AnimationCurve pullDifficultyByDraw = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

        [Header("Resting and nocking")]
        public Transform restLeft;
        public Transform restRight;
        public string ammoCategory = "Arrow";
        [Tooltip("Allow the player to always grab the string, even if the bow itself isn't grabbed. Defaults to false.")]
        public bool stringAlwaysGrabbable = false;
        [Tooltip("Sets whether or not arrows can be nocked when holding the non-main handle. Defaults to true.")]
        public bool nockOnlyMainHandle = true;
        [Tooltip("Defines whether or not to drop the arrow when the bow is ungrabbed. If set to false, the bow can hold an arrow even when not held.")]
        public bool loseNockOnUngrab = true;
        [Tooltip("If true, allows arrows to drop out of the bow. If false, prevents arrows from falling out of the bow.")]
        public bool allowOverdraw = true;

        [Header("Audio")]
        [Tooltip("Plays when the bow is released and the arrow gets fired.")]
        public AudioContainer audioContainerShoot;
        [Tooltip("Plays on a loop while the player is pulling the string back.")]
        public AudioContainer audioContainerDraw;
        [Tooltip("Plays on loop while the string is moving (Either being pulled back, or snapping forward when released)")]
        public AudioClip audioClipString;

        public Item item { get; protected set; }
        public PhysicBody pb { get; protected set; }
        public ConfigurableJoint stringJoint { get; protected set; }
        public Handle stringHandle { get; protected set; }
        public Vector3 orgBowStringPos { get; protected set; }

        public ItemModuleBow module { get; protected set; }

        protected bool setupFinished = false;
        protected float currentTargetRatio = 0f;

        private string clipName;

        private void TryAssignReferences()
        {
            if (item == null) item = GetComponentInParent<Item>();
            if (pb == null) pb = gameObject.GetPhysicBody();
            if (pb == null) pb = gameObject.AddComponent<Rigidbody>().AsPhysicBody();
            if (stringJoint == null) stringJoint = GetComponent<ConfigurableJoint>();
            if (stringJoint == null) stringJoint = gameObject.AddComponent<ConfigurableJoint>();
            if (stringHandle == null) stringHandle = GetComponent<Handle>() ?? GetComponentInChildren<Handle>();
            if (stringHandle == null)
            {
                Debug.LogError($"Could not assign Handle reference! Make sure that this BowString component ({gameObject.name}) has a Handle on it, or has a Handle as a child object!");
            }
        }

        private void JointSetup(bool init, float allowance = 0f)
        {
            if (init && (!setupFinished || !Application.isPlaying))
            {
                SetStringTargetRatio(0f);
                orgBowStringPos = pb.transform.localPosition;
                SetStringSpring(module?.stringSpring ?? 500f);
            }
            stringJoint.SetConnectedPhysicBody(item.gameObject.GetPhysicBody());
            ;
            stringJoint.autoConfigureConnectedAnchor = false;
            stringJoint.configuredInWorldSpace = false;
            stringJoint.anchor = Vector3.zero;
            stringJoint.linearLimit = new SoftJointLimit() {
                limit = 0.5f * (stringDrawLength + allowance),
                contactDistance = 0.01f
            };
            stringJoint.xMotion = ConfigurableJointMotion.Locked;
            stringJoint.yMotion = ConfigurableJointMotion.Locked;
            stringJoint.zMotion = ConfigurableJointMotion.Limited;
            stringJoint.angularXMotion = ConfigurableJointMotion.Locked;
            stringJoint.angularYMotion = ConfigurableJointMotion.Locked;
            stringJoint.angularZMotion = ConfigurableJointMotion.Locked;
            stringJoint.connectedAnchor = orgBowStringPos - new Vector3(0f, 0f, (0.5f * stringDrawLength) + (0.5f * allowance));
            setupFinished = true;
        }

        // This method should be exposed when exporting the SDK; modders may have an interest in utilizing this method through an event linker or other Unity Event
        public void SetStringTargetRatio(float targetRatio)
        {
            currentTargetRatio = targetRatio;
            stringJoint.targetPosition = new Vector3(0f, 0f, -0.5f * stringDrawLength) + new Vector3(0f, 0f, targetRatio * stringDrawLength);
        }

        // This method should be exposed when exporting the SDK; modders may have an interest in utilizing this method through an event linker or other Unity Event
        public void SetStringSpring(float spring)
        {
            JointDrive jointDrive = stringJoint.zDrive;
            jointDrive.positionSpring = spring;
            stringJoint.zDrive = jointDrive;
        }

        // This property should be exposed when exporting the SDK; modders may have an interest in utilizing this property through an event linker or other Unity Event
        public bool blockStringRelease
        {
            get
            {
                bool startValue = _blockStringRelease;
                _blockStringRelease = false;
                return startValue;
            }
            set { _blockStringRelease = value; }
        }

        private bool _blockStringRelease;

        // This method should be exposed when exporting the SDK; modders may have an interest in utilizing this method through an event linker or other Unity Event
        public void SetMinFireVelocity(float fireVelocity)
        {
        }

        // This method should be exposed when exporting the SDK; modders may have an interest in utilizing this method through an event linker or other Unity Event
        public void ReleaseString()
        {
        }

        // This method should be exposed when exporting the SDK; modders may have an interest in utilizing this method through an event linker or other Unity Event
        public void SpawnAndAttachArrow(string arrowID)
        {
        }

        // This method should be exposed when exporting the SDK; modders may have an interest in utilizing this method through an event linker or other Unity Event
        public void RemoveArrow(bool despawn)
        {
        }


#if UNITY_EDITOR
        [Header("Editor only")]
        [Range(0f, 1f)]
        [Tooltip("You can use this to test the pull ratio, to ensure the string lines up with the pink line.")]
        public float testPullRatio;
        public bool showDebugArrow = true;
        [Range(0f, 2f)]
        [Tooltip("Set how long the \"debug arrow\" should be.")]
        public float debugArrowLength = 0.74557f;
        public SkinnedMeshRenderer bowMesh;
        [Range(0f, 0.1f)]
        [Tooltip("If your string is too thick and no vertices are gotten, or your string is too thin and gets too many vertices, change this.")]
        public float vertexGrabDistance = 0.01f;
        [Range(0f, 0.1f)]
        [Tooltip("Adjusts how much allowance the auto-configurator gets for figuring out the pull curve. If you have weird spikes in your pull curve graph, decrease this.")]
        public float pullDeltaMax = 0.05f;
        [NonSerialized]
        private float? lastPull;

        private void OnValidate()
        {
            if (!this.InPrefabScene() || gameObject.scene == null || !gameObject.scene.isLoaded || Application.isPlaying) return;

            bool itemHasErrors = false;
            if (animation == null)
            {
                Debug.LogError("Animation component is not set!");
                itemHasErrors = true;
            }
            else
            {
                animation.playAutomatically = false;
                if (animation.clip == null)
                {
                    Debug.LogError("Animation clip is not set on Animation component! You have to drag and drop your animation clip into both the animation field, and into the animations field.");
                    itemHasErrors = true;
                }
                else if (!animation.clip.legacy)
                {
                    Debug.LogWarning("Animation is not set to legacy! It is being changed automatically.");
                    animation.clip.legacy = true;
                }
                if (!itemHasErrors)
                {
                    clipName = animation.clip.name;
                }
            }
            if (restLeft == null || restRight == null)
            {
                Debug.LogError($"Your rest(s) are not set! Still need setting: {(restLeft == null ? "Rest Left, " : "")}{(restRight == null ? "Rest right" : "")}");
                itemHasErrors = true;
            }
            if (audioContainerDraw == null || audioContainerShoot == null || audioClipString == null)
            {
                Debug.LogWarning("Your bow audios are not properly set! Your bow will not have audio.");
            }
            if (itemHasErrors)
            {
                Debug.LogError("This bow has errors and will not function in-game! The remainder of the item validation will not proceed.");
                return;
            }
            TryAssignReferences();
            JointSetup(true);
            if (lastPull == null)
            {
                testPullRatio = 0f;
                lastPull = 1f;
            }
            if (Mathf.Approximately(testPullRatio, lastPull.Value)) return;
            SampleAnimation(testPullRatio, true);
            lastPull = testPullRatio;
        }

        [Button]
        private void AutoConfigure()
        {
            if (bowMesh == null)
            {
                Debug.LogError("Can't auto-config without a skinned mesh renderer set!");
                return;
            }
            List<Vector3> startVertices = new List<Vector3>();
            List<int> trackedVertices = new List<int>();
            SampleAnimation(0f);
            Mesh baked = new Mesh();
            bowMesh.BakeMesh(baked, true);
            baked.GetVertices(startVertices);
            for (int i = 0; i < startVertices.Count; i++)
            {
                Vector3 worldVertex = bowMesh.transform.localToWorldMatrix.MultiplyPoint3x4(startVertices[i]);
                if ((worldVertex - transform.position).sqrMagnitude <= vertexGrabDistance * vertexGrabDistance) trackedVertices.Add(i);
            }
            int trackedCount = trackedVertices.Count;
            Debug.Log($"Got {trackedCount} tracked vertices to follow for bow draw length.");
            if (trackedCount == 0)
            {
                Debug.LogError($"In order to properly auto-configure, there have to be enough mesh vertices within a certain radius to the transform center." + $"\nIncrease Vertex Grab Distance or move the {gameObject.name} transform to better line up with the bow's mesh.");
                return;
            }
            pullCurve = new AnimationCurve();
            AddLinearKey(pullCurve, 0f, 0f);
            SampleAnimation(1f);
            bowMesh.BakeMesh(baked, true);
            List<Vector3> meshVertices = new List<Vector3>();
            baked.GetVertices(meshVertices);
            Vector3 trackedAverage = new Vector3();
            for (int i = 0; i < trackedCount; i++)
            {
                if ((meshVertices[trackedVertices[i]] - startVertices[trackedVertices[i]]).sqrMagnitude < vertexGrabDistance * vertexGrabDistance) continue;
                trackedAverage += bowMesh.transform.localToWorldMatrix.MultiplyPoint3x4(meshVertices[trackedVertices[i]]);
            }
            trackedAverage /= trackedCount;
            stringDrawLength = Mathf.Abs(transform.InverseTransformPoint(trackedAverage).z);
            for (float f = 0.01f; f < 1f; f += 0.01f)
            {
                SampleAnimation(f);
                bowMesh.BakeMesh(baked, true);
                baked.GetVertices(meshVertices);
                trackedAverage = new Vector3();
                for (int i = 0; i < trackedCount; i++)
                {
                    if ((meshVertices[trackedVertices[i]] - startVertices[trackedVertices[i]]).sqrMagnitude < vertexGrabDistance * vertexGrabDistance) continue;
                    trackedAverage += bowMesh.transform.localToWorldMatrix.MultiplyPoint3x4(meshVertices[trackedVertices[i]]);
                }
                trackedAverage /= trackedCount;
                float pullRatio = Mathf.Abs(transform.InverseTransformPoint(trackedAverage).z / stringDrawLength);
                if (Mathf.Abs(pullCurve.GetLastTime() - pullRatio) > pullDeltaMax) continue;
                AddLinearKey(pullCurve, pullRatio, f);
            }
            AddLinearKey(pullCurve, 1f, 1f);
            TryAssignReferences();
            JointSetup(true);
            SampleAnimation(0f);
            testPullRatio = 0f;
        }

        private void AddLinearKey(AnimationCurve curve, float time, float value)
        {
            curve.AddKey(time, value);
            AnimationUtility.SetKeyLeftTangentMode(curve, curve.keys.Length - 1, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyRightTangentMode(curve, curve.keys.Length - 1, AnimationUtility.TangentMode.Linear);
        }

        private void SampleAnimation(float time, bool useCurve = false)
        {
            AnimationState state = animation[clipName];
            state.speed = 0;
            state.enabled = true;
            state.normalizedTime = useCurve && pullCurve != null ? pullCurve.Evaluate(time) : time;
            state.weight = 1f;
            animation.Sample();
        }

        private void OnDrawGizmos()
        {
            if (this.InPrefabScene() && item == null) item = GetComponentInParent<Item>();
            if (Mathf.Approximately(stringDrawLength, 0f) || item == null || restLeft == null || restRight == null) return;
            if (!Application.isPlaying) orgBowStringPos = item.transform.InverseTransformPoint(transform.position);
            Vector3 originalPosition = item.transform.TransformPoint(orgBowStringPos);
            Vector3 maxDrawPos = originalPosition - (transform.forward * stringDrawLength);
            Gizmos.color = Color.white;
            Gizmos.DrawLine(maxDrawPos - (transform.up * 0.03f), maxDrawPos + (transform.up * 0.03f));
            Gizmos.DrawLine(originalPosition - (transform.up * 0.03f), originalPosition + (transform.up * 0.03f));
            for (float f = 0; f < 1f; f += 0.01f)
            {
                float sample = 1f - pullDifficultyByDraw.Evaluate(f);
                Gizmos.color = sample > 0.5f ? Color.green : (sample > 0.01f ? Color.yellow : Color.red);
                Gizmos.DrawLine(originalPosition - (f * stringDrawLength * transform.forward), originalPosition - (Mathf.Max((f + 0.01f) * stringDrawLength, stringDrawLength) * transform.forward));
            }
            Vector3 arrowDrawPos = originalPosition - (transform.forward * testPullRatio * stringDrawLength);
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(arrowDrawPos - (transform.up * 0.03f), arrowDrawPos + (transform.up * 0.03f));
            if (!showDebugArrow) return;
            bool fallingOut = Mathf.Min(Vector3.Distance(arrowDrawPos, restLeft.position), Vector3.Distance(arrowDrawPos, restRight.position)) > debugArrowLength;
            Gizmos.color = Color.Lerp(fallingOut ? Color.red : Color.green, Color.black, 0.75f);
            Gizmos.DrawWireSphere(arrowDrawPos, 0.02f);
            Vector3 arrowEnd = arrowDrawPos + (transform.forward * debugArrowLength);
            Gizmos.DrawWireSphere(arrowEnd, 0.02f);
            for (int i = 0; i < 4; i++)
            {
                float direction = i < 2 ? -0.02f : 0.02f;
                bool horiz = i % 2 == 0;
                Vector3 offset = (transform.right * (horiz ? direction : 0f)) + (transform.up * (horiz ? 0f : direction));
                Gizmos.DrawLine(arrowDrawPos + offset, arrowEnd + offset);
            }
        }
#endif
    }
}
