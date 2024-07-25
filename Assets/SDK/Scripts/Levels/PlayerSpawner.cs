using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/PlayerSpawner.html")]
    public class PlayerSpawner : MonoBehaviour
    {
        public static List<PlayerSpawner> all = new List<PlayerSpawner>();
        public static List<PlayerSpawner> allActive = new List<PlayerSpawner>();
        public static PlayerSpawner current;

        [Tooltip("You can change this in the level settings or code to have unique spawn points, however if you just make a map with no added code or paramters, leave this at \"default\".")]
        public string id = "default";
        [Tooltip("-1 will use the default spawning chances, spawners with -1 will by default be 50% when used with weighted spawners.")]
        public int spawnWeight = -1;
        [Tooltip("If enabled, will spawn the player body.")]
        public bool spawnBody = true;
        public UnityEvent playerPreSpawnEvent;
        public UnityEvent playerSpawnEvent;

        public static Action<PlayerSpawner, EventTime> onSpawn;

        public enum Type
        {
            DefaultStart,
            AltnernateStart,
            Stage,
        }

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllContainerID()
        {
            return Catalog.GetDropdownAllID(Category.Container);
        }
#endif

        
#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            //draw some gizmos in the shape of a tposing player which is 1.75m tall, facing the direction of the spawner
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            //the feet should be starting at this transform position
            //draw the head
            Gizmos.DrawWireSphere(Vector3.up * 1.75f, 0.2f);
            //draw the body
            Gizmos.DrawLine(Vector3.up * 1.5f, Vector3.up * 0.5f);
            //draw the arms
            Gizmos.DrawLine(Vector3.up * 1.5f + Vector3.right * 0.5f, Vector3.up * 1.5f + Vector3.right * -0.5f);
            Gizmos.DrawLine(Vector3.up * 1.5f + Vector3.right * 0.5f, Vector3.up * 1.25f + Vector3.right * 0.5f);
            Gizmos.DrawLine(Vector3.up * 1.5f + Vector3.right * -0.5f, Vector3.up * 1.25f + Vector3.right * -0.5f);
            //draw the legs
            Gizmos.DrawLine(Vector3.zero + Vector3.right * 0.25f, Vector3.up * 0.5f);
            Gizmos.DrawLine(Vector3.zero + Vector3.right * -0.25f, Vector3.up * 0.5f);
            
            //Draw a handle showing the name of the spawner
            Handles.Label(transform.position, id);
            //Draw a blue arrow in the forward direction of the spawner
            Handles.color = Color.blue;
            Handles.ArrowHandleCap(0, transform.position, transform.rotation, 1, EventType.Repaint);
            
            Gizmos.matrix = Matrix4x4.identity;
            
           
        }
#endif // UNITY_EDITOR        
        
    }
}
