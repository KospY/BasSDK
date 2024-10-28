using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class AreaManager : ThunderBehaviour
    {
        #region Static
        public static AreaManager Instance = null;
        #endregion Static

        #region Fields

        private SpawnableArea _currentArea = null;
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        private List<SpawnableArea> _currentTree = null;
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        // Contains the count of all area with depth under or equal the index (_currentTreeDepth[2] will return the number of how many areas has a depth of 0 or 1)
        // Because _currentTree is sorted by depth you can use the value to iterate all area under the index as depth
        private List<int> _currentTreeDepthIndex = null;
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        // all area that has a lower or equal depth need to set memory lite state to false (so it would start load if not already loaded)
        private int _areaFullMemoryDepth = 100;
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        // all area that has a higher depth need to set memory lite to true (so it would unload)
        private int _areaLiteMemoryDepth = 100;
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        // all area that have higher or equal depth than _areaCullDepth ned to be culled
        private int _areaCullDepth = 2;

#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        private int _areaUniqueId = 0;

        private Transform _playerHeadTransform = null;
        #endregion Fields

        #region Properties
        public override ManagedLoops EnabledManagedLoops => ManagedLoops.Update;


        public SpawnableArea CurrentArea { get { return _currentArea; } }


        public bool initialized;

        public LoreManager LoreManager;
        #endregion Properties

        #region Events
        public delegate void PlayerAreaChangeEvent(SpawnableArea newArea, SpawnableArea previousArea);
        public event PlayerAreaChangeEvent OnPlayerChangeAreaEvent;

        public delegate void InitializedEvent(EventTime eventTime);
        public event InitializedEvent OnInitializedEvent;
        #endregion

    }
}