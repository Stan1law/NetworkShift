using UnityEngine;

public class Mission03SwitchInteractable : Interactable
{
    [SerializeField] private Mission03SwitchInspectionPanel inspectionPanel;

    public override void Interact()
    {
        Debug.Log(
            "Network switch inspected. Port 8 appears active."
        );

        Mission03Manager manager =
            FindFirstObjectByType<Mission03Manager>();

        if (manager != null)
        {
            manager.RecordSwitchInspection();
        }

        if (inspectionPanel != null)
        {
            inspectionPanel.Show();
        }
    }
}