using System.Collections;
using UnityEngine;

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
    public class RoomOcclusion : MonoBehaviour
    {
        public Tile currentTile;
        protected Item item;
        protected RagdollPart ragdollPart;
        protected bool isCulled;

#if DUNGEN


        protected void OnEnable()
        {

            if (!isCulled)
            {
                if (Level.current && Level.current.dungeonGenerator && Level.current.adjacentRoomCulling)
                {
                    Level.current.adjacentRoomCulling.onTileVisibilityChanged += OnTileVisibilityChanged;
                }
            }
        }

        protected void OnDisable()
        {
            if (!isCulled)
            {
                if (Level.current && Level.current.dungeonGenerator && Level.current.adjacentRoomCulling)
                {
                    Level.current.adjacentRoomCulling.onTileVisibilityChanged -= OnTileVisibilityChanged;
                }
                currentTile = null;
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
            if (Level.current && Level.current.dungeonGenerator)
            {
                foreach (Tile tile in Level.current.dungeonGenerator.Generator.CurrentDungeon.AllTiles)
                {
                    if (tile.Bounds.Contains(this.transform.position))
                    {
                        currentTile = tile;
                        SetCull(currentTile.culled);
                        return;
                    }
                }
            }
        }

        protected void OnTileVisibilityChanged(Tile tile)
        {
            if (Level.current.dungeonGenerator.Generator.Status == GenerationStatus.Complete)
            {
                if (currentTile)
                {
                    if (tile == currentTile)
                    {
                        SetCull(currentTile.culled);
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
            if (other.gameObject.layer == Common.roomAndVolumeLayer && other.TryGetComponent<Tile>(out Tile tile))
            {
                currentTile = tile;
                SetCull(currentTile.culled);
            }
        }

        protected void SetCull(bool cull)
        {
            if (isCulled == cull) return;

            if (!isCulled && cull)
            {
                isCulled = true;
            }
            if (ragdollPart)
            {
            }
            else
            {
                this.gameObject.SetActive(!cull);
            }
        
            if (isCulled && !cull)
            {
                isCulled = false;
            }
        }
#endif
        }
}