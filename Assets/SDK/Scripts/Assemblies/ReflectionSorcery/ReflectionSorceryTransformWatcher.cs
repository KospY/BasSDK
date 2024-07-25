using UnityEngine;

[ExecuteInEditMode]
public class ReflectionSorceryTransformWatcher : MonoBehaviour
{
    private Transform transformCache;
    private Matrix4x4 lastMatrix;
    public bool changed;

}
