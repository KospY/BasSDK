using UnityEngine;

[HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/AnimSetRootMotion")]
public class AnimSetRootMotion : StateMachineBehaviour
{
    public bool state;
    public Transition transition = Transition.OnEnter;

    public enum Transition
    {
        OnEnter,
        OnExit,
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (transition == Transition.OnEnter) animator.applyRootMotion = state;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (transition == Transition.OnExit) animator.applyRootMotion = state;
    }

}
