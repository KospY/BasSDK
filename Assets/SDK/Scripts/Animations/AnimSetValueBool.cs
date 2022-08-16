using UnityEngine;

[HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/AnimSetValueBool")]
public class AnimSetValueBool : StateMachineBehaviour
{
    public string parameter = "Index";
    public bool value;
    public Transition transition = Transition.OnEnter;

    public enum Transition
    {
        OnEnter,
        OnExit,
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (transition == Transition.OnEnter) animator.SetBool(parameter, value);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (transition == Transition.OnExit) animator.SetBool(parameter, value);
    }

}
