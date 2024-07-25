using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ThunderRoad.Utilities
{
	public class AnimTimeStretch : StateMachineBehaviour
	{

		public AnimationCurve TimeStretchCurve;
		public string multiplierParameter;
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{

		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{

		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateUpdate(animator, stateInfo, layerIndex);


			TimeStretchCurve.Evaluate(stateInfo.normalizedTime % 1f);
			animator.SetFloat(multiplierParameter, TimeStretchCurve.Evaluate(stateInfo.normalizedTime % 1f));

		}

	}
}