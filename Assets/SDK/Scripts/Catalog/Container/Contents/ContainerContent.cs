using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ThunderRoad
{
    [System.Serializable]
    public abstract class ContainerContent
    {
        [JsonIgnore]
#if ODIN_INSPECTOR
        [ReadOnly, ShowInInspector, HorizontalGroup("Fields", Width = 100), BoxGroup("Fields/Type"), HideLabel, PropertyOrder(0)]
        public string type => GetTypeString();
#endif
        [JsonMergeKey]
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(DropdownOptions)), HorizontalGroup("Fields", Width = 300), BoxGroup("Fields/Reference ID"), HideLabel, PropertyOrder(1)]
#endif
        public string referenceID;

        [JsonIgnore]
        public abstract CatalogData catalogData { get; }

        public virtual ContainerContent GetEndContent() => Clone();

        public abstract ContainerContent Clone();

        public abstract bool OnCatalogRefresh();

        public virtual string GetOutput() => referenceID;

#if ODIN_INSPECTOR
        public abstract string GetTypeString();

        public abstract List<ValueDropdownItem<string>> DropdownOptions();
#endif
    }
}
