using UnityEngine;

[ExecuteInEditMode]
public class ReflectionSorceryTransformWatcher : MonoBehaviour
{
    private Transform transformCache;
    private Matrix4x4 lastMatrix;
    public bool changed;

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        transformCache = transform;
    }

    private void Update()
    {
        var matrix = transformCache.localToWorldMatrix;
        if (lastMatrix != matrix)
        {
            lastMatrix = matrix;
            transformCache.hasChanged = true;
            
        }
        changed = transformCache.hasChanged;
    }
    
}
