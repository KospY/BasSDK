using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad
{
    public interface IToolControllable
    {
        public void CopyTo(GameObject other) => (other.AddComponent(GetType()) as IToolControllable).CopyFrom(this);

        public abstract void CopyFrom(IToolControllable original);

        public abstract void Remove();
    }
}
