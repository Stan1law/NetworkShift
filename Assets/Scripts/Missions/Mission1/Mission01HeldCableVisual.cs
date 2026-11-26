
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Mission01HeldCableVisual : MonoBehaviour
{
    [Header("Cable References")]
    [SerializeField] private Transform cableHoldPoint;

    [Header("Cable Shape")]
    [SerializeField, Min(8)] private int segmentCount = 20;
    [SerializeField, Min(0f)] private float sagAmount = 0.12f;

    private LineRenderer lineRenderer;
    private Transform pcConnectionPoint;
    private bool isCarryingCable;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = segmentCount + 1;
        lineRenderer.enabled = false;
    }

    private void LateUpdate()
    {
        if (!isCarryingCable ||
            cableHoldPoint == null ||
            pcConnectionPoint == null)
        {
            return;
        }

        DrawCable(pcConnectionPoint.position, cableHoldPoint.position);
    }

    public void BeginCarry(Transform pcPoint)
    {
        if (pcPoint == null || cableHoldPoint == null)
        {
            Debug.LogWarning(
                "Mission01HeldCableVisual: Assign the PC point and CableHoldPoint."
            );
            return;
        }

        pcConnectionPoint = pcPoint;
        isCarryingCable = true;
        lineRenderer.enabled = true;
        DrawCable(pcConnectionPoint.position, cableHoldPoint.position);
    }

    public void EndCarry()
    {
        isCarryingCable = false;
        lineRenderer.enabled = false;
        pcConnectionPoint = null;
    }

    public void HideHeldCable()
    {
        isCarryingCable = false;

        if (lineRenderer != null)
            lineRenderer.enabled = false;

        Debug.Log("Mission 1 held cable visual hidden.");
    }

    private void DrawCable(Vector3 start, Vector3 end)
    {
        for (int i = 0; i <= segmentCount; i++)
        {
            float t = (float)i / segmentCount;
            Vector3 point = Vector3.Lerp(start, end, t);
            point.y -= 4f * sagAmount * t * (1f - t);
            lineRenderer.SetPosition(i, point);
        }
    }
}
