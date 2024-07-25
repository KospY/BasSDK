using UnityEngine;

#if ODIN_INSPECTOR
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Action
{
	public class ConsoleLog : ActionNode
    {
        public string consoleOutput = "Console log output";
        public State returnValue = State.SUCCESS;
        public int alertLevel = 0;

    }
}
