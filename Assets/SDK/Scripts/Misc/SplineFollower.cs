using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ThunderRoad.Splines;

namespace ThunderRoad
{
	[RequireComponent(typeof(Rigidbody))]
	public class SplineFollower : MonoBehaviour
	{
		public Transform anchorPoint;
		public ThunderSpline connectedSpline;
		public SplineAlignmentMode alignmentMode = SplineAlignmentMode.Soft;

		public UnityEvent OnReachFirstKnotEvent;
		public UnityEvent OnReachLastKnotEvent;

		private bool isAtFirstKnot;
		private bool isAtLastKnot;

		public enum SplineAlignmentMode
        {
			None,
			Soft,
			Medium,
			Hard
        }

		public PhysicBody pb { get; protected set; }
		public float splineNormalizedPosition { get; protected set; }

		protected ConfigurableJoint configJoint;
		private Vector3 lastWorldPosition;
		private Vector3 lastWorldForward;
		private Vector3 lastWorldUp;
		private List<Collider> myColliders;

		private float connectedSplineT0;
		private float connectedSplineTLast;

		protected void Start()
		{
			pb = gameObject.GetPhysicBody();
			myColliders = new List<Collider>();
			myColliders.AddRange(GetComponentsInChildren<Collider>());
			for (int i = myColliders.Count - 1; i >= 0; i--)
			{
				if (myColliders[i].gameObject.GetPhysicBodyInParent() != pb) myColliders.RemoveAt(i);
			}

			if(connectedSpline != null)
            {
				ThunderSpline spline = connectedSpline;
				connectedSpline = null;
				ConnectToSpline(spline);
            }
		}

		protected void OnCollisionEnter(Collision collision)
		{
			if (collision.collider.attachedRigidbody != null || collision.collider.attachedArticulationBody != null) return;
			ThunderSpline spline = collision.collider.GetComponentInParent<ThunderSpline>();
			if (spline != null || collision.collider.TryGetComponent(out spline))
			{
				ConnectToSpline(spline);
			}
		}

		public void ConnectToSpline(ThunderSpline spline)
		{
			if (connectedSpline != null)
			{
				DisconnectFromSpline();
			}
			connectedSpline = spline;

			var startKnot = connectedSpline.primarySpline[0];
			Vector4 startPos = new Vector4(startKnot.Position.x, startKnot.Position.y, startKnot.Position.z, 1.0f);
			startPos = connectedSpline.transform.localToWorldMatrix * startPos;
			connectedSpline.GetClosestSplinePoint((Vector3) startPos, out connectedSplineT0, out _, out _, out _, out _);

			int knotCount = connectedSpline.primarySpline.Count;
			var lastKnot = connectedSpline.primarySpline[knotCount - 1];
			Vector4 endPos = new Vector4(lastKnot.Position.x, lastKnot.Position.y, lastKnot.Position.z, 1.0f);
			endPos = connectedSpline.transform.localToWorldMatrix * endPos;
			connectedSpline.GetClosestSplinePoint((Vector3)endPos, out connectedSplineTLast, out _, out _, out _, out _);

			SetIgnoreCollision(spline.GetComponentsInChildren<Collider>(), true);
			configJoint = gameObject.AddComponent<ConfigurableJoint>();
			configJoint.anchor = anchorPoint != null ? transform.InverseTransformPoint(anchorPoint.position) : Vector3.zero;
			configJoint.autoConfigureConnectedAnchor = false;
			ConstrainToSpline();
			configJoint.xMotion = ConfigurableJointMotion.Free;
			configJoint.yMotion = configJoint.zMotion = ConfigurableJointMotion.Locked;

			if (connectedSpline.primarySpline != null)
			{
				var knot = connectedSpline.primarySpline[0];
				Vector4 tempPos = new Vector4(knot.Position.x, knot.Position.y, knot.Position.z, 1.0f);
				tempPos = connectedSpline.transform.localToWorldMatrix * tempPos;
				isAtFirstKnot = Vector3.Distance(pb.transform.position, tempPos) < 0.01f;

				knot = connectedSpline.primarySpline[connectedSpline.primarySpline.Count - 1];
				tempPos = new Vector4(knot.Position.x, knot.Position.y, knot.Position.z, 1.0f);
				tempPos = connectedSpline.transform.localToWorldMatrix * tempPos;
				isAtLastKnot = Vector3.Distance(pb.transform.position, tempPos) < 0.01f;
			}
		}

		public void DisconnectFromSpline()
		{
			SetIgnoreCollision(connectedSpline.GetComponentsInChildren<Collider>(), false);
			connectedSpline = null;
			Destroy(configJoint);
			configJoint = null;
		}

		protected void SetIgnoreCollision(Collider[] otherColliders, bool ignore)
		{
			foreach (Collider collider in otherColliders)
			{
				foreach (Collider myCollider in myColliders)
				{
					Physics.IgnoreCollision(collider, myCollider, ignore);
				}
			}
		}

		protected void FixedUpdate()
		{
			if (connectedSpline == null) return;
			ConstrainToSpline();
			if (connectedSpline.forceAlongSpline)
			{
				Vector3 force = connectedSpline.GetSplineForceAtNormalizedPointWithForward(splineNormalizedPosition, lastWorldForward);
				pb.AddForceAtPosition(force, lastWorldPosition, connectedSpline.forceCurves[connectedSpline.currentForceCurveIndex].forceMode);
			}
			if (connectedSpline.splineFriction)
			{
				Vector3 friction = connectedSpline.GetSplineFrictionForBodyAtT(splineNormalizedPosition, pb.velocity);
				pb.AddForceAtPosition(friction, lastWorldPosition, ForceMode.Force);
			}

			if(connectedSpline.primarySpline != null)
			{
				var knot = connectedSpline.primarySpline[0];
				Vector4 tempPos = new Vector4(knot.Position.x, knot.Position.y, knot.Position.z, 1.0f);
				tempPos = connectedSpline.transform.localToWorldMatrix * tempPos;
				if (Vector3.Distance(pb.transform.position, tempPos) < 0.01f)
				{
					if (!isAtFirstKnot)
					{
						OnReachFirstKnotEvent.Invoke();
                        connectedSpline.OnReachFirstKnotEvent.Invoke();
                        isAtFirstKnot = true;
					}
				}
				else
				{
					isAtFirstKnot = false;
				}

				knot = connectedSpline.primarySpline[connectedSpline.primarySpline.Count - 1];
				tempPos = new Vector4(knot.Position.x, knot.Position.y, knot.Position.z, 1.0f);
				tempPos = connectedSpline.transform.localToWorldMatrix * tempPos;
				if (Vector3.Distance(pb.transform.position, tempPos) < 0.01f)
				{
					if (!isAtLastKnot)
					{
						OnReachLastKnotEvent.Invoke();
                        connectedSpline.OnReachLastKnotEvent.Invoke();
                        isAtFirstKnot = true;
					}
				}
				else
				{
					isAtLastKnot = false;
				}
			}
		}

		// Update the joint, and if the object goes off the end, disconnect it from the spline
		protected void ConstrainToSpline()
		{
			// Update the joint axis
			float t;
			connectedSpline.GetClosestSplinePoint(transform.position, out t, out lastWorldPosition, out lastWorldForward, out lastWorldUp, out Vector3 right);
			this.splineNormalizedPosition = t;
			configJoint.connectedAnchor = lastWorldPosition;
			configJoint.axis = transform.InverseTransformDirection(lastWorldForward);
			configJoint.secondaryAxis = transform.InverseTransformDirection(lastWorldUp);
			// Handle the ends of the spline
			if (!connectedSpline.primarySpline.Closed && ((t <= connectedSplineT0 && connectedSpline.capStart) || (t >= connectedSplineTLast && connectedSpline.capEnd)))
			{
				if (anchorPoint != null)
				{
					transform.MoveAlign(anchorPoint, configJoint.connectedAnchor, transform.rotation);
				}
				else
				{
					transform.position = configJoint.connectedAnchor;
				}
				Vector3 stopVelocity = Vector3.Project(pb.velocity, lastWorldForward);
				int tint = Mathf.RoundToInt(t);
				pb.velocity -= stopVelocity;
				if (Vector3.Dot(tint == 1 ? Vector3.down : Vector3.up, lastWorldForward) > 0)
				{
					pb.AddForceAtPosition(Vector3.Project(-Physics.gravity, lastWorldForward), lastWorldPosition, ForceMode.Acceleration);
				}
			}
			switch (alignmentMode)
            {
				case SplineAlignmentMode.Soft:
					transform.rotation = Quaternion.LookRotation(transform.up, lastWorldForward);
					transform.RotateAround(transform.position, transform.right, 90);
					transform.RotateAround(transform.position, transform.up, 180);
					break;
				case SplineAlignmentMode.Medium:
					transform.rotation = Quaternion.LookRotation(lastWorldForward, transform.up);
					break;
				case SplineAlignmentMode.Hard:
					transform.rotation = Quaternion.LookRotation(lastWorldForward, lastWorldUp);
					break;
			}
		}
	}
}
