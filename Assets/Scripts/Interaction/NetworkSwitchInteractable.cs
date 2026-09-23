using UnityEngine;

public class NetworkSwitchInteractable : Interactable
{
    [SerializeField] private CableConnectionVisual cableVisual;
    [SerializeField] private CableConnectionPanel cableConnectionPanel;
    [SerializeField] private NetworkSwitchInspectionPanel inspectionPanel;

    public override void Interact()
    {
        MissionObjectiveManager objectiveManager =
            FindFirstObjectByType<MissionObjectiveManager>();

        if (objectiveManager == null)
        {
            Debug.LogError(
                "NetworkSwitchInteractable: MissionObjectiveManager not found!"
            );
            return;
        }

        // First interaction: inspect the switch
        if (!objectiveManager.SwitchInspected)
        {
            Debug.Log("Network Switch inspected!");

            // Complete the inspection objective
            objectiveManager.CompleteNetworkSwitch();

            // Show inspection information
            if (inspectionPanel != null)
            {
                inspectionPanel.OpenInspectionUI();
            }
            else
            {
                Debug.LogError(
                    "NetworkSwitchInteractable: Inspection Panel reference not assigned!"
                );
            }

            return;
        }

        // Second interaction after cable is found:
        // open the cable connection UI
        if (objectiveManager.CableFound &&
            !objectiveManager.CableConnected)
        {
            Debug.Log("Opening Cable Connection UI...");

            if (cableConnectionPanel != null)
            {
                cableConnectionPanel.OpenConnectionUI(this);
            }
            else
            {
                Debug.LogError(
                    "NetworkSwitchInteractable: CableConnectionPanel reference not assigned!"
                );
            }

            return;
        }

        Debug.Log("Network Switch already repaired.");
    }

    public bool OnPortSelected(int portNumber)
    {
        if (portNumber < 1 || portNumber > 10)
        {
            Debug.LogError(
                $"NetworkSwitchInteractable: Invalid port number {portNumber}. " +
                "Port must be between 1 and 10."
            );

            return false;
        }

        // Port 7 is the required port for Mission 1.
        if (portNumber != 7)
        {
            Debug.Log($"Port {portNumber} is incorrect. Port 7 is required.");

            return false;
        }

        Debug.Log("Correct port selected: Port 7.");

        MissionObjectiveManager objectiveManager =
            FindFirstObjectByType<MissionObjectiveManager>();

        if (objectiveManager == null)
        {
            Debug.LogError(
                "NetworkSwitchInteractable: MissionObjectiveManager not found!"
            );

            return false;
        }

        if (cableVisual != null)
        {
            cableVisual.ConnectCable();
        }
        else
        {
            Debug.LogError(
                "NetworkSwitchInteractable: CableConnectionVisual reference not assigned!"
            );

            return false;
        }

        objectiveManager.CompleteCableConnected();

        Debug.Log("Cable connected to Port 7. Mission objective completed.");

        return true;
    }
}