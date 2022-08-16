using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/WayPoint")]
    public class WayPoint : MonoBehaviour
    {
        [Header("Turn")]
        public bool turnToDirection = false;
        public float turnSpeedRatio = 1;

        [Header("Wait")]
        public Vector2 waitMinMaxDuration = new Vector2(0, 0);

        [Header("Animation")]
        public bool playAnimation;
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllAnimationID")]
#endif
        public string animationId;
        public float animationTurnMinAngle = 30;
        public Vector2 animationRandomMinMaxDelay = new Vector2(0, 0);

        [Header("Action subtree")]
        public string actionBehaviorTreeID = "";
        public Transform target;
        public int requiredTreeSuccessCount = 3;
        public int failuresToSkip = 10;

        protected static NavMeshPath navMeshPath;

#if UNITY_EDITOR
        [Header("Positioning")]
        public bool doRadiusCheck = false;
        public float intendedAgentRadius = 0.35f;
        public float intendedAgentHeight = 1f;
        [NonSerialized]
        protected Vector3 rayHitPosition;
        [NonSerialized]
        protected bool isGood = false;
        [NonSerialized]
        protected Vector3[] lastPosition = new Vector3[2];
        [NonSerialized]
        public static List<(MeshCollider, bool)> meshColliders = new List<(MeshCollider, bool)>();
        [NonSerialized]
        public UnityEditor.SceneManagement.StageHandle activeScene;
#endif

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllAnimationID()
        {
            return Catalog.GetDropdownAllID(Catalog.Category.Animation);
        }
#endif


        private void OnValidate()
        {
            //IconManager.SetIcon(this.gameObject, IconManager.LabelIcon.Purple);
            navMeshPath = new NavMeshPath();
        }

        public void OnDrawGizmos()
        {
            if (this.transform.parent)
            {
                int index = this.transform.GetSiblingIndex();
                Transform nextWaypoint = this.transform.parent.GetChild(index < (this.transform.parent.childCount - 1) ? index + 1 : 0);
                if (nextWaypoint)
                {
                    NavMesh.CalculatePath(this.transform.position, nextWaypoint.transform.position, -1, navMeshPath);

                    if (navMeshPath.status == NavMeshPathStatus.PathComplete)
                    {
                        float arrowLenght = 0.5f;
                        float arrowPosition = 0.8f; //from 0 to 1
                        int arrowAngle = 40;
                        Color groundLinkColor = Color.white;
                        Color linkColor = Color.magenta;
#if UNITY_EDITOR
                        foreach (Transform waypoint in this.transform.parent)
                        {
                            if (waypoint.gameObject == UnityEditor.Selection.activeGameObject)
                            {
                                groundLinkColor = Color.magenta;
                                linkColor = Color.white;
                                break;
                            }
                        }
#endif
                        Gizmos.color = navMeshPath.status == NavMeshPathStatus.PathPartial ? Color.yellow : groundLinkColor;
                        Gizmos.DrawSphere(this.transform.position, 0.15f);
                        Gizmos.DrawLine(this.transform.position, navMeshPath.corners[0]);
                        if (turnToDirection) Common.DrawGizmoArrow(this.transform.position, this.transform.forward * 0.5f, Gizmos.color);

                        for (int i = 0; i < navMeshPath.corners.Length; i++)
                        {
                            Vector3 dir = new Vector3();
                            if (i < navMeshPath.corners.Length - 1)
                            {
                                dir = navMeshPath.corners[i + 1] - navMeshPath.corners[i];
                            }
                            if (dir != Vector3.zero)
                            {
                                Gizmos.color = linkColor;
                                Gizmos.DrawRay(navMeshPath.corners[i], dir);
                                Vector3 right = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 180 + arrowAngle, 0) * Vector3.forward;
                                Vector3 left = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 180 - arrowAngle, 0) * Vector3.forward;
                                Gizmos.DrawRay(navMeshPath.corners[i] + (dir * arrowPosition), right * arrowLenght);
                                Gizmos.DrawRay(navMeshPath.corners[i] + (dir * arrowPosition), left * arrowLenght);
                            }
                        }
                    }
                    else if (navMeshPath.status == NavMeshPathStatus.PathInvalid)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(this.transform.position, 0.15f);
                    }
                }
            }
        }

        public void OnDrawGizmosSelected()
        {
            if (this.transform.parent)
            {
                int index = this.transform.GetSiblingIndex();
                this.name = "Waypoint" + index;
            }
            else
            {
                this.name = "Waypoint_PleaseINeedAParent";
            }
#if UNITY_EDITOR
            if (!doRadiusCheck) return;
            if (transform.position != lastPosition[0])
            {
                rayHitPosition = transform.position;
                isGood = false;
            }
            if (transform.position == lastPosition[0] && lastPosition[0] != lastPosition[1] && !Application.isPlaying)
            {
                if (meshColliders.Count == 0)
                {
                    var newScene = UnityEditor.SceneManagement.StageUtility.GetCurrentStageHandle();
                    if (newScene != activeScene) meshColliders.Clear();
                    activeScene = newScene;
                    foreach (MeshCollider meshCollider in activeScene != null ? activeScene.FindComponentsOfType<MeshCollider>() : FindObjectsOfType<MeshCollider>())
                    {
                        meshColliders.Add((meshCollider, meshCollider.convex));
                    }
                }
                for (int i = meshColliders.Count - 1; i >= 0; i--)
                {
                    (MeshCollider, bool) meshCollider = meshColliders[i];
                    if (meshCollider.Item1 == null)
                    {
                        meshColliders.RemoveAt(i);
                        continue;
                    }
                    if ((meshCollider.Item1.transform.position - transform.position).sqrMagnitude < (intendedAgentRadius * 5f) * (intendedAgentRadius * 5f))
                    {
                        meshCollider.Item1.convex = true;
                    }
                }

                LayerMask groundMask = 1 << 1 | 1 << 12;
                
                var physicsScene = gameObject.scene.GetPhysicsScene();
                if (physicsScene.Raycast(rayHitPosition, Vector3.down, out RaycastHit hit, 10f, groundMask, QueryTriggerInteraction.Ignore))
                {
                    rayHitPosition = hit.point;
                    isGood = true;
                    Collider[] results = new Collider[25];
                    if (physicsScene.OverlapCapsule(rayHitPosition + new Vector3(0f, intendedAgentRadius, 0f), rayHitPosition + new Vector3(0f, intendedAgentHeight - (intendedAgentRadius), 0f), intendedAgentRadius, results, groundMask, QueryTriggerInteraction.Ignore) > 0)
                    {
                        for (int i = 0; i < results.Length; i++)
                        {
                            if (results[i] == null) continue;
                            Vector3 closestColliderPoint = results[i].ClosestPoint(rayHitPosition);
                            if (closestColliderPoint.y > rayHitPosition.y)
                            {
                                isGood = false;
                                break;
                            }
                        }
                    }
                }
                foreach ((MeshCollider, bool) meshCollider in meshColliders)
                {
                    meshCollider.Item1.convex = meshCollider.Item2;
                }
            }
            lastPosition[1] = lastPosition[0];
            lastPosition[0] = transform.position;
            Gizmos.color = Color.grey;
            Gizmos.DrawWireSphere(rayHitPosition, intendedAgentRadius);
            Gizmos.color = isGood ? Color.green : Color.red;
            Gizmos.DrawWireSphere(rayHitPosition + new Vector3(0f, intendedAgentRadius, 0f), intendedAgentRadius);
            Gizmos.DrawWireSphere(rayHitPosition + new Vector3(0f, intendedAgentHeight - (intendedAgentRadius), 0f), intendedAgentRadius);
#endif
        }

        public static void SpawnerDrawGizmos(Transform spawner, Transform waypoints)
        {
            if (waypoints && waypoints.childCount > 0)
            {
                NavMesh.CalculatePath(spawner.position, waypoints.GetChild(0).transform.position, -1, navMeshPath);

                if (navMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                    if (spawner.position.y > navMeshPath.corners[0].y)
                    {
                        Gizmos.color = navMeshPath.status == NavMeshPathStatus.PathPartial ? Color.yellow : Color.gray;
                        Gizmos.DrawSphere(spawner.position, 0.15f);
                        Gizmos.color = navMeshPath.status == NavMeshPathStatus.PathPartial ? Color.yellow : Color.green;
                        Gizmos.DrawLine(spawner.position, navMeshPath.corners[0]);

                        for (int i = 0; i < navMeshPath.corners.Length; i++)
                        {
                            Vector3 dir = new Vector3();
                            if (i < navMeshPath.corners.Length - 1)
                            {
                                dir = navMeshPath.corners[i + 1] - navMeshPath.corners[i];
                            }
                            if (dir != Vector3.zero)
                            {
                                Gizmos.color = Color.gray;
                                Gizmos.DrawRay(navMeshPath.corners[i], dir);
                            }
                        }
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(spawner.position, 0.15f);
                    }
                }
                else if (navMeshPath.status == NavMeshPathStatus.PathInvalid)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(spawner.position, 0.15f);
                }
            }
            else
            {
                if (NavMesh.SamplePosition(spawner.position, out NavMeshHit navMeshHit, 2, -1) && spawner.position.y > navMeshHit.position.y)
                {
                    Gizmos.color = Color.gray;
                    Gizmos.DrawSphere(spawner.position, 0.15f);
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(spawner.position, navMeshHit.position);
                }
                else
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(spawner.position, 0.15f);
                }
            }
        }
    }
}