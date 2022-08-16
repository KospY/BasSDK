using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad
{
    [Flags]
    public enum ManagedLoops
    {
        FixedUpdate = 1,
        Update = 2,
        LateUpdate = 4
    }

}
