using UnityEngine;

public class EthernetCableInteractable : Interactable
{
    public override void Interact()
    {
        Debug.Log("Ethernet Cable found!");

        MissionObjectiveManager objectiveManager =
            FindFirstObjectByType<MissionObjectiveManager>();

        if (objectiveManager != null)
        {
            objectiveManager.CompleteCableFound();
        }
        else
        {
            Debug.LogError("MissionObjectiveManager not found!");
        }
    }
}