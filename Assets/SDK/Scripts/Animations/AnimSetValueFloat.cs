using UnityEngine;

[HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/AnimSetValueFloat")]
public class AnimSetValueFloat : StateMachineBehaviour
{
    public string parameter = "Index";
    public float value;
    public Transition transition = Transition.OnEnter;

    public enum Transition
    {
        OnEnter,
        OnExit,
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (transition == Transition.OnEnter) animator.SetFloat(parameter, value);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (transition == Transition.OnExit) animator.SetFloat(parameter, value);
    }

}
