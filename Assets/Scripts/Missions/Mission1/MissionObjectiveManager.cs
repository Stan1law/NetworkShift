using UnityEngine;
using TMPro;

public class MissionObjectiveManager : MonoBehaviour
{
    public TMP_Text objectiveText;

    public bool PCInspected { get; private set; }
    public bool CableFound { get; private set; }
    public bool SwitchInspected { get; private set; }
    public bool CableConnected { get; private set; }

    private void Start()
    {
        UpdateObjectives();
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

        // Don't allow the cable objective before PC7 is inspected.
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

        // Don't allow the switch before the cable is found.
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

        // Don't allow connection before the investigation is complete.
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
    }
}