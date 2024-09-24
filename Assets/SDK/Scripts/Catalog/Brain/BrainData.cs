using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using UnityEngine.Profiling;
using Newtonsoft.Json;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class BrainData : CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Pooling")]
#endif
        public int pooledCount;
#if ODIN_INSPECTOR
        [BoxGroup("Cycle")]
#endif
        public bool cycle = true;
#if ODIN_INSPECTOR
        [BoxGroup("Cycle"), ShowIf("cycle", true)]
#endif
        public float cyclePlayerMinDistance = 2f;
#if ODIN_INSPECTOR
        [BoxGroup("Cycle"), ShowIf("cycle", true)]
#endif
        public float cyclePlayerMaxDistance = 20f;
#if ODIN_INSPECTOR
        [BoxGroup("Cycle"), ShowInInspector, ReadOnly]
#endif
        [NonSerialized]
        public float cycleSpeed = 1;

#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public int difficulty = 0;
#if ODIN_INSPECTOR
        [BoxGroup("General"), ShowInInspector, ReadOnly]
#endif
        [NonSerialized]
        public bool isActive;

#if ODIN_INSPECTOR
        [BoxGroup("BehaviorTree"), ValueDropdown(nameof(GetAllBehaviorTreeID))]
#endif
        public string treeID;
#if ODIN_INSPECTOR
        [BoxGroup("BehaviorTree")]
#endif
        public bool treeDebugLog = false;
#if ODIN_INSPECTOR
        [BoxGroup("BehaviorTree"), ShowInInspector, ReadOnly]
#endif
        [NonSerialized]
        public bool hasTree;
#if ODIN_INSPECTOR
        [BoxGroup("BehaviorTree")]
#endif
        public bool updateTree = true;

#if ODIN_INSPECTOR
        [BoxGroup("Modules"), ShowInInspector]
#endif
        public List<Module> modules;

        public class Module
        {
#if ODIN_INSPECTOR
            [ShowInInspector, ReadOnly, HorizontalGroup("UsingPartHoriz"), LabelWidth(100)]
#endif
            [NonSerialized]
            public bool useHead, useArmLeft, useArmRight, useTorso, useLegs;

            [JsonIgnore]
            public Creature creature { get; protected set; }

        }

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllBehaviorTreeID()
        {
            return Catalog.GetDropdownAllID(Category.BehaviorTree);
        }
#endif

        public override int GetCurrentVersion()
        {
            return 2;
        }

        public T GetModule<T>(bool autoAddModuleIfMissing = true) where T : Module, new()
        {
            
            for (int index = modules.Count - 1; index >= 0; index--)
            {
                Module brainModule = modules[index];
                if (brainModule.GetType().IsSameOrSubclassOf(typeof(T)))
                {
                             
                    return brainModule as T;
                }
            }
            if (autoAddModuleIfMissing)
            {
                T t = new T();
                modules.Add(t);
                return t;
            }
            return null;
        }
    }
}
