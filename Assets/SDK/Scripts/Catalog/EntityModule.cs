using System;
using System.Collections;

namespace ThunderRoad
{
    public class EntityModule : CatalogData
    {
        [NonSerialized]
        public ThunderEntity entity;

        public virtual void Load(ThunderEntity entity) { this.entity = entity; }
    }
}