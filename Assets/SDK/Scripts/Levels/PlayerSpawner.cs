using UnityEngine;
using System.Collections.Generic;

namespace ThunderRoad
{
    public class PlayerSpawner : MonoBehaviour
    {
        public static List<PlayerSpawner> all = new List<PlayerSpawner>();

        private void Awake()
        {
            all.Add(this);
        }

        private void OnDestroy()
        {
            all.Remove(this);
        }

        public static PlayerSpawner GetLevelStart()
        {
#if DUNGEN
            if (Level.current.dungeonGenerator && Level.current.dungeonGenerator.Generator.Status == DunGen.GenerationStatus.Complete)
            {
                foreach (DunGen.Tile tile in Level.current.dungeonGenerator.Generator.CurrentDungeon.AllTiles)
                {
                    foreach (PlayerSpawner playerSpawner in all)
                    {
                        if (!playerSpawner.isActiveAndEnabled) continue;
                        Room room = playerSpawner.GetComponentInParent<Room>();
                        if (room && room.tile == tile)
                        {
                            return playerSpawner;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
#endif
            foreach (PlayerSpawner playerSpawner in all)
            {
                if (!playerSpawner.isActiveAndEnabled) continue;
                return playerSpawner;
            }
            return null;
        }
    }
}
