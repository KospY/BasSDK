using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Experimental.Animations;
using UnityEngine.Playables;


namespace ThunderRoad
{
	public class HumanoidFullBodyIK : ThunderBehaviour
	{
		public Vector3 lookAtPoint;
		public Transform lookAtTarget;


		public override ManagedLoops EnabledManagedLoops => ManagedLoops.Update;

		[Range(0, 2f)]
		public float heightOffset = 0;
		[Range(0, 1f)]
		public float lookWeight = 0;
		public float lookWeightTarget = 0;

		[Range(0, 1f)]
		public float fullBodylookWeight = 0;

		[Range(0, 1f)]
		public float bodyLookWeight = 0.5f;

		[Range(0, 1f)]
		public float headLookWeight = 1f;

		[Range(0, 1f)]
		public float ArmRLookWeight = 0;

		[Range(0, 1f)]
		public float ArmLLookWeight = 0;


		[Range(0, 1f)]
		public float precisionWeight = 0;

		[Range(-90, 90f)]
		public float lookPitch = 0;

		[Range(-90, 90f)]
		public float lookYaw = 0;

		public bool useLookPoint;


		public float height;


		// Feet planting

		public bool rightPlanted;
		public bool leftPlanted;

		public Vector3 velocity;


		public float forwardSpeed = 0;
		public float backSpeed = 0;
		public float leftSpeed = 0;
		public float rightSpeed = 0;

		[Range(0, 15f)]
		public float interpSpeed = 7.5f;
		//[Range(0, 5f)]
		public float leftUp = 0;
		//[Range(0, 5f)]
		public float rightUp = 0;
		//[Range(-5, 5f)]
		public float hipUp = 0;



		public bool alignPitch = false;
		public bool AlignFeetIKHeight = false;
		public bool AlignFeetIKStep = false;

		public Quaternion prevRotation = Quaternion.identity;


		public float angleLimitBody;
		public float angleRot;

		public float rotationSpeed = 20;

		public float feetDistance = 0;


		public Vector3 headForward;
		public Vector3 headUp;

	}
}