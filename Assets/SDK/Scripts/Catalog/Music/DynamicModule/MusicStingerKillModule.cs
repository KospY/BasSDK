namespace ThunderRoad
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class MusicStingerKillModule : MusicDynamicStingerModule
    {
        #region Enum
        public enum KillType
        {
            mele,
            range,
            indirectCause
        }
        #endregion Enum

        #region InternalClass
        public class KillInfos
        {
            public WeakReference creatureKilled;
            public float killTime;
            public float finalBlowDamageRatio;

            public KillInfos(Creature creatureKilled, float killTime, float finalBlowDamageRatio)
            {
                this.creatureKilled = new WeakReference(creatureKilled);
                this.killTime = killTime;
                this.finalBlowDamageRatio = finalBlowDamageRatio;
            }
        }
        #endregion InternalClass

        #region Fields
        public KillType killType;
        [Range(0.0f, 1.0f), Tooltip("Threshold for the kill damage divided by creature max health : 1 mean one shot only")]
        public float damageRatioThreshold = 0.0f;
        public float timeComboKill = 2.0f;

        private WeakReference _previousCreature = null;
        private static List<KillInfos> killInfosList;
        private static int comboMax;
        private static int nbStingerKillConnected;
        #endregion Fields

    }
}
