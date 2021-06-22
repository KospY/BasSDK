using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;

#if DUNGEN
using DunGen;
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
        [NonSerialized, ShowInInspector, ReadOnly]
        public Room currentRoom;
        protected Item item;
        protected RagdollPart ragdollPart;
        protected Rigidbody rb;
        protected bool isCulled;
        protected bool detectionEnabled;
        protected float detectionCycleSpeed = 1;
        protected float detectionCycleTime;


        protected void OnItemOrCreatureLoaded()
        {
            Refresh();
        }

        protected void OnDungeonGenerated()
        {
            detectionEnabled = true;
            Refresh();
            Level.current.dungeon.onDungeonGenerated -= OnDungeonGenerated;
        }

        //int lastEnableFrameCount;

        protected void OnEnable()
        {
            if (Level.current && Level.current.dungeon)
            {
                if (Level.current.dungeon.initialized)
                {
                    detectionEnabled = true;
                    Refresh();
                }
                else
                {
                    Level.current.dungeon.onDungeonGenerated += OnDungeonGenerated;
                }
                //lastEnableFrameCount = Time.frameCount;
                //triggerStart = true;
            }
        }

        protected void OnDisable()
        {
            detectionEnabled = false;
            //triggerEnabled = false;
            //triggerStart = false;
        }


        private void OnDestroy()
        {
            if (currentRoom)
            {
                currentRoom.UnRegisterObject(this);
                currentRoom = null;
            }
            isCulled = false;
        }

        [Button]
        public void Refresh(bool forceRoomSearch = false)
        {
            if (Level.current && Level.current.dungeon)
            {
                if (!forceRoomSearch && currentRoom)
                {
                    SetCull(currentRoom.isCulled);
                }
                else
                {
                    if (Level.current.dungeon.initialized)
                    {
                        foreach (Room room in Level.current.dungeon.rooms)
                        {
                            if (room.Contains(this.transform.position))
                            {
                                currentRoom = room;
                                currentRoom.RegisterObject(this);
                                SetCull(currentRoom.isCulled);
                                return;
                            }
                        }
                    }
                    else
                    {
                        foreach (Room room in GameObject.FindObjectsOfType<Room>())
                        {
                            if (room.Contains(this.transform.position))
                            {
                                currentRoom = room;
                                currentRoom.RegisterObject(this);
                                SetCull(currentRoom.isCulled);
                                return;
                            }
                        }
                    }
                    if (currentRoom)
                    {
                        currentRoom.UnRegisterObject(this);
                        currentRoom = null;
                    }
                }
            }
        }

        private void LateUpdate()
        {
            if (!detectionEnabled) return;

            if ((Time.time - detectionCycleTime) < detectionCycleSpeed) return;
            detectionCycleTime = Time.time;

            if (rb.IsSleeping()) return;

            if (currentRoom == null)
            {
                Room roomFound = Level.current.dungeon.SearchRoomFromPosition(this.transform.position);
                if (currentRoom != roomFound)
                {
                    currentRoom = roomFound;
                    currentRoom.RegisterObject(this);
                    SetCull(currentRoom.isCulled);
                }
            }
            else if (!currentRoom.tile.Bounds.Contains(this.transform.position))
            {
                Room roomFound = Level.current.dungeon.SearchRoomFromPosition(this.transform.position, currentRoom);
                if (currentRoom != roomFound)
                {
                    currentRoom.UnRegisterObject(this);
                    currentRoom = roomFound;
                    currentRoom.RegisterObject(this);
                    SetCull(currentRoom.isCulled);
                }
            }
        }
        /*
        protected void OnTriggerEnter(Collider other)
        {
            if (lastEnableFrameCount == Time.frameCount)
            {
                triggerEnabled = true;
                triggerStart = false;
                return;
            }
            if (!triggerEnabled) return;
            if (other.gameObject.layer == Common.roomAndVolumeLayer && other.TryGetComponent<Room>(out Room room))
            {
                if (currentRoom && currentRoom != room)
                {
                    currentRoom.UnRegisterObject(this);
                    currentRoom = null;
                }
                currentRoom = room;
                currentRoom.RegisterObject(this);
                SetCull(currentRoom.isCulled);
            }
        }*/

        public void SetCull(bool cull)
        {
            if (isCulled == cull || (item && item.holder)) return;
            isCulled = cull;
            this.gameObject.SetActive(!cull);
        }
#endif
    }
}