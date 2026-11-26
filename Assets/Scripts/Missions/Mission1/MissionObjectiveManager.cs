
using UnityEngine;
using TMPro;

public class MissionObjectiveManager : MonoBehaviour
{
    [Header("Objective UI")]
    public TMP_Text objectiveText;
    [SerializeField] private GameObject objectivePanel;
    [SerializeField]
    private MissionCompleteUI missionCompleteUI;

    public bool CableConnectedToPC { get; private set; }

    public bool PCInspected { get; private set; }
    public bool CableFound { get; private set; }
    public bool SwitchInspected { get; private set; }
    public bool CableConnected { get; private set; }

    private void Start()
    {
        UpdateObjectives();
    }

    public void SetObjectivePanelVisible(bool visible)
    {
        if (objectivePanel != null)
        {
            objectivePanel.SetActive(visible);
        }
    }

    public void CompleteCableConnectedToPC()
    {
        if (CableConnectedToPC)
            return;

        if (!CableFound)
        {
            Debug.Log("Find the Ethernet cable first.");
            return;
        }

        CableConnectedToPC = true;
        Debug.Log("Ethernet cable connected to PC7.");
    }

    public void CompletePCInspection()
    {
        if (PCInspected)
            return;

        PCInspected = true;
        Debug.Log("Objective completed: Inspect PC7");
        UpdateObjectives();
    }

    public void CompleteCableFound()
    {
        if (CableFound)
            return;

        if (!PCInspected)
        {
            Debug.Log("You should inspect PC7 first.");
            return;
        }

        CableFound = true;
        Debug.Log("Objective completed: Find the Ethernet Cable");
        UpdateObjectives();
    }

    public void CompleteNetworkSwitch()
    {
        if (SwitchInspected)
            return;

        if (!CableFound)
        {
            Debug.Log("Find the Ethernet cable before inspecting the switch.");
            return;
        }

        SwitchInspected = true;
        Debug.Log("Objective completed: Inspect the Network Switch");
        UpdateObjectives();
    }

    public void CompleteCableConnected()
    {
        if (CableConnected)
            return;

        if (!PCInspected || !CableFound || !SwitchInspected)
        {
            Debug.Log("Complete the troubleshooting steps first.");
            return;
        }

        CableConnected = true;
        Debug.Log("Objective completed: Connect the Cable");
        UpdateObjectives();
        MissionComplete();
    }

    private void UpdateObjectives()
    {
        if (objectiveText == null)
            return;

        string text = "MISSION 01 — NETWORK OUTAGE\n\n";

        text += (PCInspected ? "[X] " : "[ ] ")
            + "Inspect PC7\n";

        text += (CableFound ? "[X] " : "[ ] ")
            + "Find the Ethernet Cable\n";

        text += (SwitchInspected ? "[X] " : "[ ] ")
            + "Inspect the Network Switch\n";

        text += (CableConnected ? "[X] " : "[ ] ")
            + "Connect the Cable";

        objectiveText.text = text;
    }


    private void MissionComplete()
    {
        Debug.Log("MISSION 01 COMPLETE!");

        // Hide the normal mission objectives.
        SetObjectivePanelVisible(false);

        // Display the completion screen.
        if (missionCompleteUI != null)
        {
            missionCompleteUI.ShowMissionComplete();
        }
        else
        {
            Debug.LogError(
                "MissionObjectiveManager: MissionCompleteUI is not assigned!"
            );
        }
    }

}
