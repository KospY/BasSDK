using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class DataIdContainer<T> where T : CatalogData
    {
        #region Data
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllCategoryTypeId))]
#endif //ODIN_INSPECTOR
        public string dataId;
        #endregion Data

        #region Properties
        protected virtual bool ShowDataInInspector => true;
#if ODIN_INSPECTOR
        [ShowInInspector] 
#endif
        [JsonIgnore]
        public ThunderRoad.Category Category { get { return Catalog.GetCategory(typeof(T)); } }
#if ODIN_INSPECTOR
        [ShowInInspector]
        [ShowIf("ShowDataInInspector")] 
#endif
        [JsonIgnore]
        public T Data
        {
            get
            {
                if (string.IsNullOrEmpty(dataId))
                {
                    return null;
                }

                T data;

                if (Catalog.IsJsonLoaded())
                {
	                if (Catalog.TryGetData<T>(dataId, out data))
	                {
		                return data;
	                }

	                Debug.LogError("Catalog, can not find data id : " + dataId + " of type : " + Category);
                }
                return null;
            }
        }
        #endregion Properties


        #region Methods
        public DataIdContainer(string dataId)
        {
            this.dataId = dataId;
        }

        public DataIdContainer()
        {
            this.dataId = string.Empty;
        }

        public override bool Equals(object obj)
        {
            if (obj is string otherId)
            {
                if (dataId == null && otherId == null)
                {
                    return true;
                }

                return dataId.Equals(otherId);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(dataId, ShowDataInInspector, Category, Data);
        }
#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllCategoryTypeId()
        {
            return Catalog.GetDropdownAllID(Category);
        }
#endif
        #endregion Methods
    }


    [Serializable]
    public abstract class DataIdContainerChoice
    {
        #region Data
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllCategoryTypeId))]
#endif //ODIN_INSPECTOR
        public string dataId;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllowedCategoryType))]
#endif //ODIN_INSPECTOR
        public ThunderRoad.Category category;
        #endregion Data

        #region Properties
        protected virtual bool ShowDataInInspector => true;

#if ODIN_INSPECTOR
        [ShowInInspector]
        [ShowIf("ShowDataInInspector")]
#endif
        [JsonIgnore]
        public CatalogData Data
        {
            get
            {
                if (string.IsNullOrEmpty(dataId))
                {
                    return null;
                }

                CatalogData data = Catalog.GetData(category, dataId);
                if (data != null) return data;

                Debug.LogError("Catalog, can not find data id : " + dataId + " of type : " + category);
                return null;
            }
        }
        #endregion Properties


        #region Methods
        public DataIdContainerChoice(string dataId, ThunderRoad.Category category)
        {
            this.dataId = dataId;
            this.category = category;
        }

        public DataIdContainerChoice()
        {
            this.dataId = string.Empty;
            this.category = GetCategoryAllowed()[0];
        }

        public abstract ThunderRoad.Category[] GetCategoryAllowed ();

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllCategoryTypeId()
        {
            return Catalog.GetDropdownAllID(category);
        }
#endif

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<ThunderRoad.Category>> GetAllowedCategoryType()
        {
            List<ValueDropdownItem<ThunderRoad.Category>> list = new List<ValueDropdownItem<ThunderRoad.Category>>();
            ThunderRoad.Category[] allowed = GetCategoryAllowed();
            for (int i = 0; i < allowed.Length; i++)
            {
                list.Add(new ValueDropdownItem<ThunderRoad.Category>(allowed[i].ToString(), allowed[i]));
            }

            return list;
        }
#endif
        #endregion Methods
    }
}