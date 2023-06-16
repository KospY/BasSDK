using System;
using System.Collections;

namespace ThunderRoad
{
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
