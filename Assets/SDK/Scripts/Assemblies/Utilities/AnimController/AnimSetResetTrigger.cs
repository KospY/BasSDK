using UnityEngine;

namespace ThunderRoad.Utilities
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/AnimSetResetTrigger")]
    public class AnimSetResetTrigger : StateMachineBehaviour
    {
        public string triggerName = "Index";
        public SetOption setOrReset;
        public Transition transition = Transition.OnEnter;

        public enum SetOption
        {
            Set,
            Reset
        }

        public enum Transition
        {
            OnEnter,
            OnExit,
        }

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (transition == Transition.OnEnter) SetReset(animator);
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (transition == Transition.OnExit) SetReset(animator);
        }

        private void SetReset(Animator animator)
        {
            switch (setOrReset)
            {
                case SetOption.Set:
                    animator.SetTrigger(triggerName);
                    break;
                case SetOption.Reset:
                    animator.ResetTrigger(triggerName);
                    break;
            }
        }
    }
}
