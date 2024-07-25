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
        private List<int> _currentTreeDepthIndex = null;
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        private int _areaFullMemoryDepth = 100;
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        private int _areaLiteMemoryDepth = 100;
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
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