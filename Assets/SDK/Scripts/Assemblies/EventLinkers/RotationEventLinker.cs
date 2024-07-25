using UnityEngine;
using UnityEngine.Events;

public class RotationEventLinker : MonoBehaviour
{
    public Transform target;

    [Header("General")]
    public Axis axis = Axis.X;
    public bool invert;

    [Header("Min/Max angle")]
    public float positiveMinAngle = 5;
    public float positiveMaxAngle = 30;

    public float negativeMinAngle = 5;
    public float negativeMaxAngle = 30;

    [Header("Event")]
    public UnityEvent<float> output = new UnityEvent<float>();

    public enum Axis
    {
        X,
        Y,
        Z,
    }

    public void Invert(bool inverted)
    {
        invert = inverted;
    }

    private void Update()
    {
        float angle = 0;
        if (axis == Axis.X) angle = target.localRotation.eulerAngles.x;
        else if (axis == Axis.Y) angle = target.localRotation.eulerAngles.y;
        else if (axis == Axis.Z) angle = target.localRotation.eulerAngles.z;
        angle = (angle > 180) ? angle - 360 : angle;
        float ratio = Mathf.InverseLerp(angle >= 0 ? positiveMinAngle : negativeMinAngle, angle >= 0 ? positiveMaxAngle : negativeMaxAngle, Mathf.Abs(angle));
        if (angle < 0) ratio = -ratio;
        if (invert) ratio = -ratio;
        output?.Invoke(ratio);
    }
}
