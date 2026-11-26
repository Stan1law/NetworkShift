
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Mission02CableVisual : MonoBehaviour
{
    [Header("Cable Shape")]
    [SerializeField, Min(2)] private int floorSegments = 12;
    [SerializeField, Min(0.01f)] private float cableRadius = 0.015f;
    [SerializeField] private float floorOffset = 0.025f;

    private LineRenderer lineRenderer;
    private Transform pcConnectionPoint;
    private Transform switchPortPoint;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.widthMultiplier = cableRadius;
        lineRenderer.positionCount = floorSegments + 3;
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        if (!lineRenderer.enabled ||
            pcConnectionPoint == null ||
            switchPortPoint == null)
        {
            return;
        }

        DrawFloorCable();
    }

    public void ShowConnection(
        Transform pcPoint,
        Transform switchPoint)
    {
        if (pcPoint == null || switchPoint == null)
        {
            Debug.LogWarning("Cable endpoints are missing.");
            return;
        }

        pcConnectionPoint = pcPoint;
        switchPortPoint = switchPoint;

        lineRenderer.enabled = true;
        DrawFloorCable();
    }

    private void DrawFloorCable()
    {
        Vector3 start = pcConnectionPoint.position;
        Vector3 end = switchPortPoint.position;

        float floorY = floorOffset;

        Vector3 startFloor = new Vector3(start.x, floorY, start.z);
        Vector3 endFloor = new Vector3(end.x, floorY, end.z);

        lineRenderer.positionCount = floorSegments + 3;

        // Cable leaves the PC and drops toward the floor.
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, startFloor);

        // Cable travels along the floor toward the switch.
        for (int i = 1; i <= floorSegments; i++)
        {
            float t = (float)i / floorSegments;
            Vector3 point = Vector3.Lerp(startFloor, endFloor, t);
            lineRenderer.SetPosition(i + 1, point);
        }

        // Final point connects to the switch port.
        lineRenderer.SetPosition(
            floorSegments + 2,
            end
        );
    }
}