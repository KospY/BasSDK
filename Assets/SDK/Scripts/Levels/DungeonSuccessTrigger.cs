#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif	  
using ThunderRoad.Modules;
using UnityEngine;

namespace ThunderRoad
{
    public class DungeonSuccessTrigger : MonoBehaviour
    { 
#if ODIN_INSPECTOR
	    [ShowInInspector]
#endif	    
	    bool hasActivated = false;

        public void DungeonSuccess()
        {
		}
    }
}
