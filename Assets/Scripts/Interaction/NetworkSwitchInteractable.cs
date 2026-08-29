using UnityEngine;

public class NetworkSwitchInteractable : Interactable
{
    public CableConnectionVisual cableVisual;

    public override void Interact()
    {
        MissionObjectiveManager objectiveManager =
            FindFirstObjectByType<MissionObjectiveManager>();

        if (objectiveManager == null)
        {
            Debug.LogError("MissionObjectiveManager not found!");
            return;
        }

        // First interaction: inspect the switch
        if (!objectiveManager.SwitchInspected)
        {
            Debug.Log("Network Switch inspected!");

            objectiveManager.CompleteNetworkSwitch();

            return;
        }

        // Second interaction: connect the cable
        if (objectiveManager.CableFound &&
            !objectiveManager.CableConnected)
        {
            Debug.Log("Ethernet Cable connected to Network Switch!");

            // SHOW THE CONNECTED CABLE
            if (cableVisual != null)
            {
                cableVisual.ConnectCable();
            }

            objectiveManager.CompleteCableConnected();

            return;
        }

        Debug.Log("Network Switch already repaired.");
    }
}