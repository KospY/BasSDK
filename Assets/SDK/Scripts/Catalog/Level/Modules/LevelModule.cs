using System;
using System.Collections;

namespace ThunderRoad
{
#if UNITY_EDITOR
    // This is needed just for the ModCatalogEditor to work
    [Serializable]
#endif
    public abstract class LevelModule : Module
    {
        [NonSerialized]
        public Level level;

        public virtual IEnumerator OnPlayerSpawnCoroutine()
        {
            yield break;
        }
    }
}
