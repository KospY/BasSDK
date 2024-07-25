using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad.Utilities
{
	public class BusyState : StateMachineBehaviour
	{
		public static Dictionary<Animator, int> animatorBusyStates = new Dictionary<Animator, int>();

		public string busyBool = "IsBusy";

		private Animator animator;

		public enum Transition
		{
			OnEnter,
			OnExit,
		}

		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.animator = animator;
			if (animatorBusyStates.TryGetValue(animator, out int current)) animatorBusyStates[animator] = current + 1;
			else animatorBusyStates[animator] = 1;
			UpdateBusyBool();
		}

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.animator = animator;
			if (animatorBusyStates.TryGetValue(animator, out int current)) animatorBusyStates[animator] = current - 1;
			else animatorBusyStates[animator] = 0;
			UpdateBusyBool();
		}

		private void UpdateBusyBool()
        {
			int current = 0;
			animatorBusyStates.TryGetValue(animator, out current);
			animator.SetBool(busyBool, current > 0);
        }
	}
}