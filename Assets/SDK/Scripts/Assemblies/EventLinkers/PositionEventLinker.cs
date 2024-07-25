using UnityEngine;
using UnityEngine.Events;

public class PositionEventLinker : MonoBehaviour
{
    public Transform target;

    public Axis axis = Axis.Z;
    public Direction direction = Direction.Positive;

    public bool useFixedUpdate = false;

    public enum Axis
    {
        X,
        Y,
        Z,
    }

    public enum Direction
    {
        Positive,
        Negative,
    }

    public float thresholdLow = 0.02f;
    public float thresholdHigh = 0.13f;
    public float maxDistance = 0.15f;

    [Header("Output")]
    public bool invert;

    public UnityEvent outputThresholdHighEnter = new UnityEvent();
    public UnityEvent outputThresholdHighExit = new UnityEvent();

    public UnityEvent outputThresholdLowEnter = new UnityEvent();
    public UnityEvent outputThresholdLowExit = new UnityEvent();

    public UnityEvent<float> outputRatio = new UnityEvent<float>();

    public float axisDistance = 0;
    public float ratio = 0;

    protected bool thresholdHighReached;
    protected bool thresholdLowReached;

    private void OnValidate()
    {
        maxDistance = Mathf.Clamp(maxDistance, 0, Mathf.Infinity);
        thresholdHigh = Mathf.Clamp(thresholdHigh, 0, maxDistance);
        thresholdLow = Mathf.Clamp(thresholdLow, 0, thresholdHigh);
    }

    public void Invert(bool inverted)
    {
        invert = inverted;
    }

    private void Update()
    {
        if (!useFixedUpdate)
        {
            UpdateDetection();
        }
    }

    private void FixedUpdate()
    {
        if (useFixedUpdate)
        {
            UpdateDetection();
        }
    }

    private void UpdateDetection()
    {
        if (axis == Axis.Z) axisDistance = this.transform.InverseTransformPoint(target.position).z;
        else if (axis == Axis.Y) axisDistance = this.transform.InverseTransformPoint(target.position).y;
        else if (axis == Axis.X) axisDistance = this.transform.InverseTransformPoint(target.position).x;

        if (direction == Direction.Positive && axisDistance >= 0)
        {
            axisDistance = Mathf.Clamp(axisDistance, 0, Mathf.Infinity);
        }
        if (direction == Direction.Negative && axisDistance <= 0)
        {
            axisDistance = Mathf.Clamp(Mathf.Abs(axisDistance), 0, Mathf.Infinity);
        }

        ratio = Mathf.InverseLerp(thresholdLow, thresholdHigh, axisDistance);

        if (!thresholdHighReached && axisDistance >= thresholdHigh)
        {
            outputThresholdHighEnter?.Invoke();
            thresholdHighReached = true;
        }
        else if (thresholdHighReached && axisDistance < thresholdHigh)
        {
            outputThresholdHighExit?.Invoke();
            thresholdHighReached = false;
        }

        if (!thresholdLowReached && axisDistance <= thresholdLow)
        {
            outputThresholdLowEnter?.Invoke();
            thresholdLowReached = true;
        }
        else if (thresholdLowReached && axisDistance > thresholdLow)
        {
            outputThresholdLowExit?.Invoke();
            thresholdLowReached = false;
        }

        if (invert) ratio = -ratio;
        outputRatio?.Invoke(ratio);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 dir = Vector3.zero;
        if (axis == Axis.Z) dir = this.transform.forward;
        else if (axis == Axis.Y) dir = this.transform.up;
        else if (axis == Axis.X) dir = this.transform.right;

        if (direction == Direction.Negative)
        {
            dir = -dir;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.transform.position + (dir * thresholdLow));
        Gizmos.color = Color.white;
        Gizmos.DrawLine(this.transform.position + (dir * thresholdLow), this.transform.position + (dir * thresholdHigh));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(this.transform.position + (dir * thresholdHigh), this.transform.position + (dir * maxDistance));
    }
}
