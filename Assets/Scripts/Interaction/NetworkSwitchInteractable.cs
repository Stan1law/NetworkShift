
using UnityEngine;

public class NetworkSwitchInteractable : Interactable
{
    [Header("UI References")]
    [SerializeField] private CableConnectionVisual cableVisual;
    [SerializeField] private CableConnectionPanel cableConnectionPanel;
    [SerializeField] private NetworkSwitchInspectionPanel inspectionPanel;
    [SerializeField] private Mission01HeldCableVisual heldCableVisual;

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

        if (!objectiveManager.CableFound)
        {
            Debug.Log("Find the Ethernet cable before inspecting the switch.");
            return;
        }

        // First interaction: inspect the switch.
        if (!objectiveManager.SwitchInspected)
        {
            objectiveManager.CompleteNetworkSwitch();

            if (inspectionPanel != null)
            {
                inspectionPanel.OpenInspectionUI();
            }
            else
            {
                Debug.LogError(
                    "NetworkSwitchInteractable: Inspection Panel is not assigned!"
                );
            }

            return;
        }

        // Open the port-selection UI until the connection succeeds.
        if (!objectiveManager.CableConnected)
        {
            if (cableConnectionPanel != null)
            {
                cableConnectionPanel.OpenConnectionUI(this);
            }
            else
            {
                Debug.LogError(
                    "NetworkSwitchInteractable: Cable Connection Panel is not assigned!"
                );
            }

            return;
        }

        Debug.Log("Network Switch already repaired.");
    }


    public bool OnPortSelected(int portNumber)
    {
        // 1. Validate the port number.
        if (portNumber < 1 || portNumber > 10)
        {
            Debug.LogError($"Invalid port {portNumber}. Choose a port from 1 to 10.");
            return false;
        }

        // 2. Only Port 7 is correct.
        if (portNumber != 7)
        {
            Debug.Log($"Port {portNumber} is incorrect. Port 7 is required.");
            return false;
        }

        // 3. Find the mission manager.
        MissionObjectiveManager objectiveManager =
            FindFirstObjectByType<MissionObjectiveManager>();

        if (objectiveManager == null)
        {
            Debug.LogError("MissionObjectiveManager not found!");
            return false;
        }

        // 4. Verify that the cable is connected to PC7.
        EthernetCableInteractable cable =
            FindFirstObjectByType<EthernetCableInteractable>();

        if (cable == null || !cable.IsConnectedToPC ||
            !objectiveManager.CableConnectedToPC)
        {
            Debug.Log("Connect the Ethernet cable to PC7 first.");
            return false;
        }

        // 5. Connect the cable to the switch.
        if (!cable.ConnectToSwitch())
            return false;

        // 6. Display the permanent PC7-to-switch cable.
        if (cableVisual == null)
        {
            Debug.LogError("Cable Visual is not assigned!");
            return false;
        }

        cableVisual.ShowCable();

        if (heldCableVisual != null)
        {
            heldCableVisual.HideHeldCable();
        }
        else
        {
            Debug.LogError(
                "NetworkSwitchInteractable: Mission 1 Held Cable Visual is not assigned!"
            );
            return false;
        }

    
      

        // 8. Complete the mission objective.
        objectiveManager.CompleteCableConnected();

        Debug.Log("Port 7 connected. Held cable hidden; connection visual retained.");
        return true;
    }

}
