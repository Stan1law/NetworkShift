
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CableConnectionVisual : MonoBehaviour
{
    [Header("Cable References")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform cableConnectionPoint;
    [SerializeField] private Transform cableEndPoint;

    [Header("Cable Shape")]
    [SerializeField, Min(8)] private int segments = 24;
    [SerializeField, Min(0f)] private float sagAmount = 0.5f;

    private void Awake()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer == null)
        {
            Debug.LogError("CableConnectionVisual: LineRenderer is missing.");
            enabled = false;
            return;
        }

        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = segments + 1;
        lineRenderer.enabled = false;
    }

    private void LateUpdate()
    {
        if (lineRenderer == null || !lineRenderer.enabled)
            return;

        UpdateCableShape();
    }

    private void UpdateCableShape()
    {
        if (cableConnectionPoint == null || cableEndPoint == null)
            return;

        Vector3 start = cableConnectionPoint.position;
        Vector3 end = cableEndPoint.position;

        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments;

            Vector3 point = Vector3.Lerp(start, end, t);

            // Maximum sag in the middle, zero sag at both endpoints.
            point.y -= 4f * sagAmount * t * (1f - t);

            lineRenderer.SetPosition(i, point);
        }
    }

    public void ShowCable()
    {
        if (lineRenderer == null)
            return;

        lineRenderer.positionCount = segments + 1;
        lineRenderer.enabled = true;
        UpdateCableShape();
    }

    public void HideCable()
    {
        if (lineRenderer != null)
            lineRenderer.enabled = false;
    }

    public void ConnectCable()
    {
        ShowCable();
    }
}
