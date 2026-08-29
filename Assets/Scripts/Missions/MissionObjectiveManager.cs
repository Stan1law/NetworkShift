using UnityEngine;
using TMPro;

public class MissionObjectiveManager : MonoBehaviour
{
    public TMP_Text objectiveText;

    public bool SwitchInspected { get; private set; }
    public bool CableFound { get; private set; }
    public bool CableConnected { get; private set; }

    private void Start()
    {
        UpdateObjectives();
    }

    public void CompleteNetworkSwitch()
    {
        if (SwitchInspected)
            return;

        SwitchInspected = true;

        Debug.Log("Objective completed: Inspect the Network Switch");

        UpdateObjectives();
    }

    public void CompleteCableFound()
    {
        if (CableFound)
            return;

        CableFound = true;

        Debug.Log("Objective completed: Find the Ethernet Cable");

        UpdateObjectives();
    }

    public void CompleteCableConnected()
    {
        if (CableConnected)
            return;

        CableConnected = true;

        Debug.Log("Objective completed: Connect the Cable");

        UpdateObjectives();

        MissionComplete();
    }

    private void UpdateObjectives()
    {
        string text = "MISSION 01 — NETWORK OUTAGE\n\n";

        text += (SwitchInspected ? "[X] " : "[ ] ")
            + "Inspect the Network Switch\n";

        text += (CableFound ? "[X] " : "[ ] ")
            + "Find the Ethernet Cable\n";

        text += (CableConnected ? "[X] " : "[ ] ")
            + "Connect the Cable";

        objectiveText.text = text;
    }

    private void MissionComplete()
    {
        Debug.Log("MISSION 01 COMPLETE!");
    }
}