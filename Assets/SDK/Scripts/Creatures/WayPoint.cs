using UnityEngine;
using UnityEngine.AI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class WayPoint : MonoBehaviour
    {
        public Vector2 stopLenght = new Vector2(1, 3);
#if UNITY_EDITOR
        protected static NavMeshPath navMeshPath;
#endif
        private void OnValidate()
        {
            IconManager.SetIcon(this.gameObject, IconManager.LabelIcon.Purple);
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
                if (NavMesh.SamplePosition(spawner.position, out NavMeshHit navMeshHit, 100, -1) && spawner.position.y > navMeshHit.position.y)
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