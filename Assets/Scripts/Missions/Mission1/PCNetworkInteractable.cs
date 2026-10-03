using UnityEngine;

public class PCNetworkInteractable : Interactable
{
    [SerializeField] private string computerName = "PC7";

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
            Debug.Log($"{computerName}: Network connection is still disconnected.");
            return;
        }

        Debug.Log($"{computerName}: Network cable disconnected.");
        Debug.Log("Employee PC has no network connection.");

        objectiveManager.CompletePCInspection();
    }
}