using System.Collections;
using UnityEngine;

#if DUNGEN
using DunGen;
using System;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class RoomObject : MonoBehaviour
    {
#if DUNGEN
        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public Room currentRoom;
        protected Item item;
        protected RagdollPart ragdollPart;
        protected bool isCulled;


        protected void OnEnable()
        {
            if (!isCulled)
            {
                if (Level.current && Level.current.dungeon)
                {
                    Level.current.dungeon.onRoomVisibilityChange.AddListener(OnRoomVisibilityChange);
                }
            }
        }

        protected void OnDisable()
        {
            if (!isCulled)
            {
                if (Level.current && Level.current.dungeon)
                {
                    Level.current.dungeon.onRoomVisibilityChange.RemoveListener(OnRoomVisibilityChange);
                }
                currentRoom = null;
            }
        }

        private void Start()
        {
            if (!item && !ragdollPart)
            {
                Refresh();
            }
        }

        [Button]
        public void Refresh()
        {
            if (Level.current && Level.current.dungeon)
            {
                foreach (Room room in Level.current.dungeon.rooms)
                {
                    if (room.Contains(this.transform.position))
                    {
                        currentRoom = room;
                        SetCull(currentRoom.isCulled);
                        return;
                    }
                }
            }
        }


        protected void OnRoomVisibilityChange(Room room)
        {
            if (Level.current.dungeon)
            {
                if (currentRoom)
                {
                    if (room == currentRoom)
                    {
                        SetCull(currentRoom.isCulled);
                    }
                }
                else
                {
                    Refresh();
                }
            }
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == Common.roomAndVolumeLayer && other.TryGetComponent<Room>(out Room room))
            {
                currentRoom = room;
                SetCull(currentRoom.isCulled);
            }
        }

        protected void SetCull(bool cull)
        {
            if (isCulled == cull || (item && item.holder)) return;
            isCulled = cull;
            this.gameObject.SetActive(!cull);
        }
#endif
    }
}