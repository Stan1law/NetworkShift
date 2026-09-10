using UnityEngine;

public class NetworkSwitchInteractable : Interactable
{
    [SerializeField] private CableConnectionVisual cableVisual;
    [SerializeField] private CableConnectionPanel cableConnectionPanel;

    public override void Interact()
    {
        MissionObjectiveManager objectiveManager =
            FindFirstObjectByType<MissionObjectiveManager>();

        if (objectiveManager == null)
        {
            Debug.LogError("NetworkSwitchInteractable: MissionObjectiveManager not found!");
            return;
        }

        // First interaction: inspect the switch
        if (!objectiveManager.SwitchInspected)
        {
            Debug.Log("Network Switch inspected!");

            objectiveManager.CompleteNetworkSwitch();

            return;
        }

        // Second interaction: open cable connection UI
        if (objectiveManager.CableFound && !objectiveManager.CableConnected)
        {
            Debug.Log("Opening Cable Connection UI...");

            if (cableConnectionPanel != null)
            {
                cableConnectionPanel.OpenConnectionUI(this);
            }
            else
            {
                Debug.LogError("NetworkSwitchInteractable: CableConnectionPanel reference not assigned!");
            }

            return;
        }

        Debug.Log("Network Switch already repaired.");
    }

    public void OnPortSelected(int portNumber)
    {
        // Validate port number is between 1 and 10
        if (portNumber < 1 || portNumber > 10)
        {
            Debug.LogError($"NetworkSwitchInteractable: Invalid port number {portNumber}. Must be between 1 and 10.");
            return;
        }

        Debug.Log($"NetworkSwitchInteractable: Port {portNumber} selected, connecting cable...");

        MissionObjectiveManager objectiveManager =
            FindFirstObjectByType<MissionObjectiveManager>();

        if (objectiveManager == null)
        {
            Debug.LogError("NetworkSwitchInteractable: MissionObjectiveManager not found in OnPortSelected!");
            return;
        }

        // Display physical cable connection
        if (cableVisual != null)
        {
            cableVisual.ConnectCable();
        }
        else
        {
            Debug.LogError("NetworkSwitchInteractable: CableConnectionVisual reference not assigned!");
        }

        // Complete the mission objective
        objectiveManager.CompleteCableConnected();

        Debug.Log($"Cable connected to port {portNumber}. Mission objective completed.");
    }
}
