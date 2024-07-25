using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad
{
    public interface ILiteMemoryToggle : IMonoBehaviourReference
    {
        public void SetLiteMemory(bool isInLiteMemory);
    }
}
