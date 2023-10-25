using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererScript : MonoBehaviour
{
    public Transform startPoint;  // The starting point for the line
    public Transform endPoint;    // The ending point for the line
    public float lineThickness = 0.1f;  // Thickness of the line
    public Color lineColor = Color.white; // Color of the line

    private LineRenderer lineRenderer;

    void Start()
    {
        // Create a LineRenderer component if not already present
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // Set LineRenderer properties
        lineRenderer.startWidth = lineThickness;
        lineRenderer.endWidth = lineThickness;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
    }

    void Update()
    {
        // Update the line's positions based on the transforms
        if (startPoint != null && endPoint != null)
        {
            lineRenderer.SetPosition(0, startPoint.position);
            lineRenderer.SetPosition(1, endPoint.position);
        }
    }

    public float GetLineLength() {
        //Debug.Log("Line length is: " + (endPoint.position - startPoint.position).magnitude);
        return (endPoint.position - startPoint.position).magnitude;
    }
}
