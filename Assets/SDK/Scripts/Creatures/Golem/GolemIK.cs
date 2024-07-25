using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

public class GolemIK : MonoBehaviour
{
#if ODIN_INSPECTOR
	[Title("General")]
#endif
    public float alphaMoveSpeed = 3f;	
	public float angleMoveSpeed= 5f;	


	public Animator animator;


#if ODIN_INSPECTOR
    [Title("Right Hand IK")]
#endif

    [Range(0, 1)]
	public float rightHandIKWeight = 0;
	[Range(0, 1)]
	public float rightHandAlpha = 0;
	[Range(0, 1)]
	public float rightHandAlphaTarget = 0;
	public bool rightArmCheckGround = true;

	[Range(-90, 90)]
#if ODIN_INSPECTOR
    [ShowIf(nameof(rightArmCheckGround))]
#endif
    public float rightHandAngle = 0;

	public Vector3 localRightOffset = Vector3.zero;

#if ODIN_INSPECTOR
    [Title("Left Hand IK")]
#endif
    [Range(0, 1)]
	public float leftHandIkWeight = 0;
	[Range(0, 1)]
	public float leftHandAlpha = 0;
	[Range(0, 1)]
	public float leftHandAlphaTarget = 0;

	public bool leftUseAngle = true;
	[Range(-90, 90)]
#if ODIN_INSPECTOR
    [ShowIf(nameof(leftUseAngle))]
#endif
	public float leftHandAngle = 0;
	public Vector3 localLeftOffset = Vector3.zero;

    // debug for the stretch curve
#if ODIN_INSPECTOR
    [Title("Debug")]
#endif
    public bool debugAnimStretchCurve = false;
#if ODIN_INSPECTOR
    [ShowIf(nameof(debugAnimStretchCurve))]
#endif
    public AnimationCurve timeStretchCurve;
#if ODIN_INSPECTOR
    [ShowIf(nameof(debugAnimStretchCurve))]
#endif
	public string multParameterName;


	void Start()
    {
		// The animator needs to be on this object
		if (!animator)
			animator = GetComponent<Animator>();
	}


    void Update()
    {
		if (!animator)
			return;
		
        rightHandAlpha = Mathf.MoveTowards(rightHandAlpha, rightHandAlphaTarget, Time.deltaTime * alphaMoveSpeed);
        leftHandAlpha = Mathf.MoveTowards(leftHandAlpha, leftHandAlphaTarget, Time.deltaTime * alphaMoveSpeed);
        if (debugAnimStretchCurve)
        {
            float speed = timeStretchCurve.Evaluate(animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f);
            animator.SetFloat(multParameterName, speed);
        }
	}

	public float angle = 0;
	[Range(-2,2f)]
	public float armThickness = 0;
	private void OnAnimatorIK(int layerIndex)
	{

		// Right hand offset

		Vector3 rightOffset = localRightOffset;
		GetOffsetForHand(ref rightOffset,ref rightHandAngle, rightHandAlignToGround, AvatarIKGoal.RightHand);



		// Left hand
		Vector3 leftOffset = animator.transform.rotation * localLeftOffset;
		GetOffsetForHand(ref leftOffset, ref leftHandAngle, leftHandAlignToGround, AvatarIKGoal.LeftHand);

		SetHandIK(AvatarIKGoal.RightHand, rightHandIKWeight, rightHandAlpha, rightOffset);
        SetHandIK(AvatarIKGoal.LeftHand, leftHandIkWeight,  leftHandAlpha, leftOffset);
	}


	void GetOffsetForHand(ref Vector3 offset,ref float handAngle,bool alignToGround, AvatarIKGoal goal)
	{
		offset = animator.transform.rotation * offset;

		if (rightArmCheckGround)
		{
			Transform shoulder;
			Transform hand;

			if (goal == AvatarIKGoal.RightHand)
			{
				shoulder = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
				hand = animator.GetBoneTransform(HumanBodyBones.RightHand);
				
			}
			else
			{
				shoulder = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
				hand = animator.GetBoneTransform(HumanBodyBones.LeftHand);

			}

			RaycastHit hitInfo;
			// Check from the hips but in XZ of feet
			var rayOrigin = hand.position;
			rayOrigin.y = shoulder.position.y;
			float distance = 5f;
			Physics.Raycast(rayOrigin, transform.up * -1, out hitInfo, distance, 1 << 0);

			//Debug.DrawLine(rayOrigin, rayOrigin + (transform.up * -1f) * distance);

			angle = 0;
			if (hitInfo.collider)
			{

				Vector3 handPos = (hand.position + hand.forward * armThickness);
				Vector3 shoulderPos = (shoulder.position + hand.forward * armThickness);

				Vector3 shoulderToHand = (handPos - shoulder.position);

				if (handPos.y < hitInfo.point.y )
				{
					float dist = Mathf.Sqrt(Mathf.Pow(shoulderToHand.magnitude, 2) - Mathf.Pow(hitInfo.distance, 2));

					Vector3 dir = Vector3.ProjectOnPlane(shoulderToHand.normalized, transform.up).normalized * dist;
					//Debug.DrawLine(hitInfo.point, hitInfo.point + dir, Color.black) ;
					//Debug.DrawLine(shoulder.position, shoulder.position+ shoulderToHand, Color.red) ;
					//Debug.DrawLine(shoulder.position, shoulder.position+ ((hitInfo.point + dir) - shoulder.position).normalized, Color.green) ;

					angle = Vector3.Angle(shoulderToHand.normalized, ((hitInfo.point + dir) - shoulder.position).normalized);
				}
				else if(handPos.y >hitInfo.point.y && alignToGround)
				{
					Vector3 dir =  hitInfo.point- shoulder.position;
					// Since the point is always below the angle will always be minus
					angle = -Vector3.Angle(shoulderToHand.normalized, dir.normalized);
				}

				
			}
			handAngle = FInterpTo(handAngle, angle, Time.deltaTime, angleMoveSpeed);

			var local = shoulder.InverseTransformPoint(hand.position);
			local = Quaternion.AngleAxis(-handAngle, Vector3.forward) * local;
			local = shoulder.TransformPoint(local);
			offset = local - hand.position;
		}
	}

    protected float FInterpTo(float Current, float Target, float DeltaTime, float InterpSpeed)
    {
        // If no interp speed, jump to target value
        if (InterpSpeed <= 0.0f)
        {
            return Target;
        }

        // Distance to reach
        float Dist = Target - Current;

        // If distance is too small, just set the desired location
        float f = Mathf.Abs(Dist);
        if (f < 0.0001)
        {
            return Target;
        }

        // Delta Move, Clamp so we do not over shoot.
        float DeltaMove = Dist * Mathf.Clamp(DeltaTime * InterpSpeed, 0.0f, 1.0f);

        return Current + DeltaMove;
    }

    void SetHandIK(AvatarIKGoal goal, float weight, float alpha, Vector3 offset)
	{
		// Take the current hand position as the base of the goal
		Vector3 currentPos = animator.GetBoneTransform((goal == AvatarIKGoal.RightHand) ? HumanBodyBones.RightHand : HumanBodyBones.LeftHand).position;
		// Blend between that and the offset
		Vector3 finalPos = Vector3.Lerp(currentPos, currentPos + offset, alpha);

		animator.SetIKPosition(goal, finalPos);
		animator.SetIKPositionWeight(goal, weight);
	}

	public bool rightHandAlignToGround = false;
	public bool leftHandAlignToGround = false;
	public void ActivateRightHandIK()
    {
		rightHandAlignToGround = true;
	}
	public void DeactivateRightHandIK()
	{
		rightHandAlignToGround = false;

	}
	public void ActivateLeftHandIK()
	{
		leftHandAlignToGround = true;

	}
	public void DeactivateLeftHandIK()
	{
		leftHandAlignToGround = false;

	}






}
