using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

[RequireComponent(typeof(LineRenderer))]
public class CircleLineRenderer : MonoBehaviour
{
    private const int NUM_CAP_VERTICES = 0;

    [Tooltip("The radius of the circle perimeter")]
    public float radius = 2;

    protected LineRenderer lineRenderer;

    [Tooltip("constant ratio to adjust, reduce this value for more vertices")]
    public float e = 0.01f;

    public float angle = 360f;

    private Material material;


    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        material = lineRenderer.material;
        lineRenderer.useWorldSpace = false;
        lineRenderer.numCapVertices = NUM_CAP_VERTICES;
    }

    private void Start()
    {
        RefreshRadius();
    }

    [Button]
    public void RefreshRadius()
    {
        SetRadius(radius);
    }

    public void SetRadius(float pRadius)
    {
        radius = pRadius;
        float th = Mathf.Acos(2 * Mathf.Pow(1 - e / radius, 2) - 1); //th is in radian
        int numberOfVertices = Mathf.CeilToInt(2 * Mathf.PI / th);

        lineRenderer.positionCount = numberOfVertices + 1;
        for (int i = 0; i < numberOfVertices + 1; i++)
        {
            float angle = (this.angle / (float)numberOfVertices) * (float)i;
            lineRenderer.SetPosition(i, radius * new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0));
        }
    }

    public void SetFullColor(Color color)
    {
        SetStartColor(color);
        SetEndColor(color);
    }

    public void SetStartColor(Color c)
    {
        material.SetColor("_Start_Color", c);
    }

    public void SetEndColor(Color c)
    {
        material.SetColor("_End_Color", c);
    }

    public void SetAlphaIntensity(float intensity)
    {
        material.SetFloat("_Alpha_Intensity", intensity);
    }

    public void Show()
    {
        lineRenderer.enabled = true;
    }

    public void Hide()
    {
        lineRenderer.enabled = false;
    }
}