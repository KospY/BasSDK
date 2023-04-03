using System.Collections;
using System;

namespace ThunderRoad.AI
{
	public enum State
    {
        SUCCESS,
        FAILURE,
        RUNNING
    }

    public class Node
    {
        [NonSerialized]
        public State nodeState;

        public Node() { }

        public virtual void OnRun() { }

        public virtual void OnFail() { }

        public virtual void OnSuccess() { }

        public virtual State Evaluate() { return State.SUCCESS; }

        public virtual Node Clone()
        {
            Node node = MemberwiseClone() as Node;
            return node;
        }
        public virtual bool IsRunningNode<T>() where T : Node
        {
            if (GetType() == typeof(T) && nodeState == State.RUNNING) return true;
            return false;
        }

        public virtual void Reset() { }

        public virtual void Init(Creature p_creature, Blackboard p_blackboard) { }
        public virtual IEnumerator CompleteCloneCoroutine() { yield break; }
        public virtual IEnumerator InitCoroutine( Creature p_creature, Blackboard p_blackboard ) { Init(p_creature, p_blackboard); yield break;}
        public virtual void OnDrawGizmos() { }

        public virtual void OnDrawGizmosSelected() { }

        public virtual string GetDebugString()
        {
            if (nodeState == State.FAILURE)
            {
                return " <color=red>" + GetType().Name + "</color> ";
            }
            if (nodeState == State.RUNNING)
            {
                return " <color=yellow>" + GetType().Name + "</color> ";
            }
            if (nodeState == State.SUCCESS)
            {
                return " <color=green>" + GetType().Name + "</color> ";
            }
            return null;
        }
    }
}