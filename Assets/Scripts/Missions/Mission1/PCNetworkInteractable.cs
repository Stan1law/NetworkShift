
using UnityEngine;

public class PCNetworkInteractable : Interactable
{
    [SerializeField] private string computerName = "PC7";
    [SerializeField] private PC7InspectionPanel inspectionPanel;

    public override void Interact()
    {
        MissionObjectiveManager objectiveManager =
            FindFirstObjectByType<MissionObjectiveManager>();

        if (objectiveManager == null)
        {
            Debug.LogError(
                "PCNetworkInteractable: MissionObjectiveManager not found!"
            );
            return;
        }

        if (objectiveManager.PCInspected)
        {
            Debug.Log(
                $"{computerName}: Network connection is still disconnected."
            );

            if (inspectionPanel != null)
                inspectionPanel.Open();

            return;
        }

        Debug.Log($"{computerName}: Network cable disconnected.");
        Debug.Log("Employee PC has no network connection.");

        objectiveManager.CompletePCInspection();

        if (inspectionPanel != null)
            inspectionPanel.Open();
        else
            Debug.LogWarning("PC7 Inspection Panel is not assigned.");
    }
}
