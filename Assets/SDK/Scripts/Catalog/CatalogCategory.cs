using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad
{
    public class CatalogCategory
    {
        public ThunderRoad.Category category;
        public List<CatalogData> catalogDatas = new List<CatalogData>(100);

        //Lookup of the id to hash(id.lowercase)
        private Dictionary<string, int> idToHashId = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, int> idToHashIdCaseSensitive = new Dictionary<string, int>(); // double the memory cost, but we can do faster a lookup
        private Dictionary<int, CatalogData> hashIdToCatalogData = new Dictionary<int, CatalogData>();

        public CatalogCategory(ThunderRoad.Category category)
        {
            this.category = category;
        }

        public void Clear()
        {
            catalogDatas.Clear();
            idToHashId.Clear();
            idToHashIdCaseSensitive.Clear();
            hashIdToCatalogData.Clear();
        }

        public bool AddCatalogData(CatalogData catalogData)
        {
            //The hashes may already exist if something called GetHashId since it will add it there
            idToHashId.TryAdd(catalogData.id, catalogData.hashId);
            idToHashIdCaseSensitive.TryAdd(catalogData.id, catalogData.hashId);

            if (hashIdToCatalogData.TryAdd(catalogData.hashId, catalogData))
            {
                //Add the catalog data to the list
                catalogDatas.Add(catalogData);
                return true;
            }
            else
            {
                Debug.Log($"CatalogData: {catalogData.id} - {catalogData.hashId} has already been added to catalog.");
                return false;
            }

        }


        /// <summary>
        /// Returns a hash of the lower case version of the ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetHashId(string id)
        {
            if (string.IsNullOrEmpty(id)) return -1;
            //try to get the hash from the case sensitive one first,
            if (!idToHashIdCaseSensitive.TryGetValue(id, out int value))
            {
                //if it doesnt have it, try the case insensitive one
                //this would be like when we define it as DaggerCommon but a user spells it as daggercommon in their json
                //it ensures they get a "quick enough" lookup without having to do any allocations
                if (!idToHashId.TryGetValue(id, out value))
                {
                    //hash the lower case version of it if it doesnt exist
                    value = Animator.StringToHash(id.ToLower());
                    //we dont add the id.ToLower() to the key because the dictionary comparator is case insensitive, saves us allocations later on lookup
                    idToHashId.Add(id, value);
                }

                //add it to the case sensitive one too, so its faster next time
                idToHashIdCaseSensitive.Add(id, value);

            }

            return value;
        }

        public bool TryGetHashId(string id, out int hashId)
        {
            hashId = GetHashId(id);
            return hashId != -1;
        }

        public bool TryGetCatalogData(int hashId, out CatalogData catalogData)
        {
            catalogData = GetCatalogData(hashId);
            return catalogData != null;
        }

        public bool TryGetCatalogData(string id, out CatalogData catalogData)
        {
            catalogData = GetCatalogData(id);
            return catalogData != null;
        }

        public CatalogData GetCatalogData(string id)
        {
            if (TryGetHashId(id, out int hashId))
            {
                return GetCatalogData(hashId);
            }
            return null;
        }

        public CatalogData GetCatalogData(int hashId)
        {
            if (hashIdToCatalogData.TryGetValue(hashId, out CatalogData catalogData))
            {
                return catalogData;
            }
            return null;
        }
    }
}
