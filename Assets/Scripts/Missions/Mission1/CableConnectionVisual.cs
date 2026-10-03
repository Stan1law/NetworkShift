using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(LineRenderer))]
public class CableConnectionVisual : MonoBehaviour
{
    // The preferred, visible fields in the Inspector
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform cableConnectionPoint; // position 0 (on NetworkSwitch)
    [SerializeField] private Transform cableEndPoint; // position 1

    // Compatibility: old serialized field names that may exist on the GameObject
    // Keep them hidden so Inspector shows only the new fields, but preserve values
    [FormerlySerializedAs("connectedCable")]
    [SerializeField, HideInInspector] private GameObject connectedCable_compat;

    [FormerlySerializedAs("connectionPoint")]
    [SerializeField, HideInInspector] private Transform connectionPoint_compat;

    [FormerlySerializedAs("cableLength")]
    [SerializeField, HideInInspector] private float cableLength_compat = 1.5f;

    private void Reset()
    {
        // Auto-assign the LineRenderer on the same GameObject when added
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
    }

    private void Awake()
    {
        // If the new fields are empty but old serialized values exist, migrate them
        if (lineRenderer == null && connectedCable_compat != null)
        {
            lineRenderer = connectedCable_compat.GetComponent<LineRenderer>();
        }

        if (cableConnectionPoint == null && connectionPoint_compat != null)
        {
            cableConnectionPoint = connectionPoint_compat;
        }

        // Ensure we have a LineRenderer reference
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer != null)
        {
            // Ensure exactly 2 positions and start hidden
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = false;
        }
    }

    private void Update()
    {
        // Only update positions while visible
        if (lineRenderer == null || !lineRenderer.enabled)
            return;

        if (cableConnectionPoint != null)
            lineRenderer.SetPosition(0, cableConnectionPoint.position);

        if (cableEndPoint != null)
            lineRenderer.SetPosition(1, cableEndPoint.position);
    }

    // Enables the line renderer and updates positions immediately
    public void ShowCable()
    {
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer is not assigned on CableConnectionVisual.");
            return;
        }

        lineRenderer.positionCount = 2;
        lineRenderer.enabled = true;

        // Immediately set positions so it doesn't pop in
        if (cableConnectionPoint != null)
            lineRenderer.SetPosition(0, cableConnectionPoint.position);

        if (cableEndPoint != null)
            lineRenderer.SetPosition(1, cableEndPoint.position);
    }

    // Disables the line renderer so the cable is hidden
    public void HideCable()
    {
        if (lineRenderer == null)
            return;

        lineRenderer.enabled = false;
    }

    // Backwards-compatible method used by existing interaction scripts
    public void ConnectCable()
    {
        ShowCable();
    }
}
