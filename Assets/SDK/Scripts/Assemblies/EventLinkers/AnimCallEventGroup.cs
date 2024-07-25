using ThunderRoad;
using UnityEngine;


[HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Event-Linkers/AnimCallEventGroup.html")]
public class AnimCallEventGroup : StateMachineBehaviour
{
    public string eventName = "Event";
    public Transition transition = Transition.OnEnter;

    public enum Transition
    {
        OnEnter,
        OnExit,
    }

    private UnityEventGrouper eventCaller;
    private bool alreadyErrored = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        eventCaller ??= animator.GetComponent<UnityEventGrouper>();
        if (eventCaller == null)
        {
            PrintMissingGrouperMessage(animator.gameObject);
            return;
        }
        if (transition == Transition.OnEnter && !string.IsNullOrEmpty(eventName))
        {
            eventCaller.ActivateNamedEvent(eventName);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        eventCaller ??= animator.GetComponent<UnityEventGrouper>();
        if (eventCaller == null)
        {
            PrintMissingGrouperMessage(animator.gameObject);
            return;
        }
        if (transition == Transition.OnExit && !string.IsNullOrEmpty(eventName))
        {
            eventCaller.ActivateNamedEvent(eventName);
        }
    }

    protected void PrintMissingGrouperMessage(GameObject source)
    {
        string message = $"You put an AnimCallEventGroup behaviour in the controller on {source.name}, but that GameObject has no UnityEventGrouper component!";
        if (!alreadyErrored)
        {
            Debug.LogError(message + "\nThis message only prints as an error once per state machine behaviour! All future error messages will be warnings.");
            alreadyErrored = true;
            return;
        }
        Debug.LogWarning(message);
    }
}
