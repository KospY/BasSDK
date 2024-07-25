#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace ThunderRoad
{
    [RequireComponent(typeof(Animator))]
    public class AnimationEvent : MonoBehaviour
    {
        [System.Serializable]
        public class AnimatorStateEvent
        {
            public string stateName;
            public UnityEvent startStateEvent;
            public UnityEvent endStateEvent;
            public UnityEvent[] customStateEvent;
        }

        public Animator animator;
        public AnimatorStateEvent[] statesEvent;

        public void invokeStartEvent()
        {
            AnimatorStateEvent stateEvent = GetCurrentAnimatorStateEvent(0);
            if (stateEvent == null) return;
            stateEvent.startStateEvent.Invoke();
        }

        public void invokeEndEvent()
        {
            AnimatorStateEvent stateEvent = GetCurrentAnimatorStateEvent(0);
            if (stateEvent == null) return;
            stateEvent.endStateEvent.Invoke();
        }
        public void invokeCustomEvent(int customIndex)
        {
            AnimatorStateEvent stateEvent = GetCurrentAnimatorStateEvent(0);
            if (stateEvent == null) return;
            if(customIndex >= 0 && stateEvent.customStateEvent.Length > customIndex)
            {
                stateEvent.customStateEvent[customIndex].Invoke();
            }
        }

        public AnimatorStateEvent GetCurrentAnimatorStateEvent(int layer = 0)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(layer);
            
            //get all of the states from the animator controller
#if UNITY_EDITOR
            var controller = animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
            if (controller == null)
            {
                Debug.LogError($"Can not find animator controller on {animator.gameObject.GetPathFromRoot()}");
                return null;
            }
            string foundStateName = string.Empty;
            foreach (AnimatorControllerLayer controllerLayer in controller.layers)
            {
                foreach (ChildAnimatorState state in controllerLayer.stateMachine.states)
                {
                    //check if this state is the current state
                    if (!currentState.IsName(state.state.name)) continue;
                    foundStateName = state.state.name;
                    foreach (AnimatorStateEvent stateEvent in statesEvent)
                    {
                        if (stateEvent.stateName == foundStateName)
                        {
                            return stateEvent;
                        }
                    }
                    
                }
            }
            Debug.LogError($"Can not find current animator state name : {currentState.fullPathHash} : {foundStateName} on {animator.gameObject.GetPathFromRoot()}");
            return null;
#else // UNITY_EDITOR
            foreach (AnimatorStateEvent stateEvent in statesEvent)
            {
                if (currentState.IsName(stateEvent.stateName))
                {
                    return stateEvent;
                }
            }
            Debug.LogError($"Can not find current animator state name : {currentState.fullPathHash} on {animator.gameObject.GetPathFromRoot()}");
            return null;
#endif // UNITY_EDITOR
            
        }
    }
}
