using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TriggerRangeRenderer : MonoBehaviour
{
    public Color rendererColor { get; set; }
    private LineRenderer lineRenderer;

    [Range(0.1f, 100f)]
    public float radius = 1.0f;

    [Range(3, 256)]
    public int numSegments = 128;

    private void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        ActiveRadiusRenderer(false);
    }
    public void ActiveRadiusRenderer(bool isActive, float radius=1f)
    {
        if (isActive)
        {
            this.radius = radius;
            DoRenderer();
        }
        else
            lineRenderer.enabled = false;
    }

    private void DoRenderer()
    {
        lineRenderer.SetColors(rendererColor, rendererColor);
        lineRenderer.SetWidth(0.5f, 0.5f);
        lineRenderer.SetVertexCount(numSegments + 1);
        lineRenderer.useWorldSpace = false;

        float deltaTheta = (float)(2.0 * Mathf.PI) / numSegments;
        float theta = 0f;

        for (int i = 0; i < numSegments + 1; i++)
        {
            float x = radius * Mathf.Cos(theta);
            float z = radius * Mathf.Sin(theta);
            Vector3 pos = new Vector3(x, 0, z);
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }
}