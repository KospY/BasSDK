using System.Collections.Generic;
using System;
using Newtonsoft.Json;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    public abstract class ContainerContent<T, V> : ContainerContent where T : CatalogData, IContainerLoadable<T> where V : ContainerContent
    {
        [JsonIgnore]
        [NonSerialized]
        public IContainerLoadable<T> loadable;

        [JsonIgnore]
        public T data
        {
            get
            {
                if (loadable == null) LoadData();
                return (T)loadable;
            }
        }

        [JsonIgnore]
        public override CatalogData catalogData => data;

        public override ContainerContent Clone()
        {
            return CloneGeneric();
        }

        public abstract V CloneGeneric();

        public override bool OnCatalogRefresh()
        {
            return LoadData();
        }

        public bool LoadData()
        {
            if (string.IsNullOrEmpty(referenceID)) return true;
            loadable = Catalog.GetData<T>(referenceID, false);
            return loadable != null;
        }
#if ODIN_INSPECTOR
        public override string GetTypeString()
        {
            return (Catalog.TryGetCategory<T>(out Category category) ? category.ToString() : "Unknown") + $" ({GetType().Name})";
        }

        public override List<ValueDropdownItem<string>> DropdownOptions()
        {
            return Catalog.GetDropdownAllID<T>();
        }
#endif
    }
}
