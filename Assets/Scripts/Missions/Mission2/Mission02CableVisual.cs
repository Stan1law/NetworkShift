using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Mission02CableVisual : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private Transform pcConnectionPoint;
    private Transform switchPortPoint;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        if (!lineRenderer.enabled)
            return;

        if (pcConnectionPoint == null || switchPortPoint == null)
            return;

        lineRenderer.SetPosition(0, pcConnectionPoint.position);
        lineRenderer.SetPosition(1, switchPortPoint.position);
    }

    public void ShowConnection(
        Transform pcPoint,
        Transform switchPoint)
    {
        pcConnectionPoint = pcPoint;
        switchPortPoint = switchPoint;

        lineRenderer.enabled = true;

        lineRenderer.SetPosition(0, pcPoint.position);
        lineRenderer.SetPosition(1, switchPoint.position);
    }
}