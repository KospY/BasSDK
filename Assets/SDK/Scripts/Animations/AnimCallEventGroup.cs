using ThunderRoad;
using UnityEngine;


[HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/AnimCallEventGroup")]
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
    
}
