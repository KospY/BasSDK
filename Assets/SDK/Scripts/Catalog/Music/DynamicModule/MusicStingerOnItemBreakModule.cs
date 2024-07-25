namespace ThunderRoad
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class MusicStingerOnItemBreakModule : MusicDynamicStingerModule
    {
        #region Fields
        [Tooltip("id in  the list will trigger the stinger in the same index from stinger id list")]
        public string[] brokenItemTriggerIds;
        #endregion Fields

    }
}
