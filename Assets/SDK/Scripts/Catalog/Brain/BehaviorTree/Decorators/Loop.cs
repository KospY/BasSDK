using System.Collections;

namespace ThunderRoad.AI.Decorator
{
	public class Loop : DecoratorNode
    {
        public Node loopingNode;
        public Mode mode = Mode.LoopingNodeFirst;
        public enum Mode
        {
            LoopingNodeFirst,
            LoopingNodeLast,
        }
    }
}
