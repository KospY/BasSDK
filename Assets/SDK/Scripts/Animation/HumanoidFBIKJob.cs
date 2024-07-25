using UnityEngine;
using UnityEngine.Animations;
//using UnityEngine.Animations.Rigging;
//using UnityEngine.Animations.Rigging;
//using UnityEngine.Animations.Rigging;

namespace ThunderRoad
{
    //[BurstCompile]
    public struct HumanoidFullBodyIKJob : IAnimationJob
	{
		public PropertySceneHandle headLook;
		public PropertySceneHandle fullBodyLook;
		public PropertySceneHandle feetLook;
		public PropertySceneHandle bodyLook;
		public PropertySceneHandle rightArmLook;
		public PropertySceneHandle leftArmLook;

		public TransformStreamHandle leftArmReferenceBone;
		public TransformStreamHandle rightArmReferenceBone;
		public TransformStreamHandle rightHandReferenceBone;
		public TransformStreamHandle leftHandReferenceBone;

		// the point at which we'd be looking
		public bool useLookPoint;
		public Vector3 lookPoint;
		public PropertySceneHandle lookPitch;
		public PropertySceneHandle lookYaw;



		public PropertySceneHandle lookWeight;

		public PropertySceneHandle angleLimitBody;
		public Vector2 angleLimitHands;

		public TransformSceneHandle rootHandle;
		public TransformStreamHandle headHandle;
		public Vector3 headForward;
		public Vector3 headUp;



		public float bodyRotationMultiplier;

		public float humanoidHeight;

		public float legsDistance ;
		public TransformStreamHandle hipReference;


		public float heightOffset;

		public float precisionWeight;


		public float leftUp;
		public float rightUp;
		public float hipUp;

		public float bodyLookInterpSpeed;
		public float armLookInterpSpeed;

		public float footIKWeight;
	


		public Vector3 rightPlantPosGoal;
		public Vector3 rightPlantPos;

		public PropertySceneHandle rightPlanted;
		public float footInterpSpeed;


		public Vector3 leftPlantPosGoal;
		public Vector3 leftPlantPos;
		public PropertySceneHandle leftPlanted;
		public PropertySceneHandle alignFeetProperty;


		public bool replantFootLeft;
		public bool replantFootRight;

        public PropertySceneHandle leftArmAdjustmentWeight;
        public PropertySceneHandle rightArmAdjustmentWeight;
        public PropertySceneHandle rightArmAdjustmentY;
        public PropertySceneHandle leftArmAdjustmentY;

		public void ProcessRootMotion(AnimationStream stream)
		{
			//stream.velocity = Vector3.zero;
			//stream.angularVelocity = Vector3.zero;
		}
		public void ProcessAnimation(AnimationStream stream)
		{
		}

	}
	

	[System.Serializable]
	public struct AffineTransform
	{
		/// <summary>Translation component of the AffineTransform.</summary>
		public Vector3 translation;
		/// <summary>Rotation component of the AffineTransform.</summary>
		public Quaternion rotation;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="t">Translation component of the AffineTransform.</param>
		/// <param name="r">Rotation component of the AffineTransform.</param>
		public AffineTransform(Vector3 t, Quaternion r)
		{
			translation = t;
			rotation = r;
		}

	}
}