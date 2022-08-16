using UnityEngine;

[HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/AnimSetValueInteger")]
public class AnimSetValueInteger : StateMachineBehaviour
{
    public string parameter = "Index";
    public int value;
    public Transition transition = Transition.OnEnter;
    public bool checkCurrentEqualsCondition = false;
    public int conditionValue = 0;

    public enum Transition
    {
        OnEnter,
        OnExit,
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (transition == Transition.OnEnter)
        {
            bool set = true;
            if (checkCurrentEqualsCondition && animator.GetInteger(parameter) != conditionValue) set = false;
            if (set) animator.SetInteger(parameter, value);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (transition == Transition.OnExit)
        {
            bool set = true;
            if (checkCurrentEqualsCondition && animator.GetInteger(parameter) != conditionValue) set = false;
            if (set) animator.SetInteger(parameter, value);
        }
    }
}
