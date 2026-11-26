using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;


public class Mission02NetworkManager : MonoBehaviour
{
    private Mission02PCInteractable selectedPC;

    private Dictionary<Mission02PCInteractable, Mission02SwitchPort> pcConnections =
        new Dictionary<Mission02PCInteractable, Mission02SwitchPort>();

    private Dictionary<Mission02SwitchPort, Mission02PCInteractable> portConnections =
        new Dictionary<Mission02SwitchPort, Mission02PCInteractable>();

    [Header("Held Cable")]
    [SerializeField] private Mission02HeldCableVisual heldCableVisual;

    [Header("Mission Results UI")]
    [SerializeField] private GameObject resultsPanel;

    [Header("Mission Results UI")]
    [SerializeField] private GameObject Mission02sresultsPanel;
    [SerializeField] private TMP_Text resultsText;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button returnToMenuButton;

    [Header("Scene Names")]
    [SerializeField] private string nextSceneName = "Mission_03";
    [SerializeField] private string menuSceneName = "Lobby";

    private bool missionCompleted = false;

    private int incorrectConnectionAttempts = 0;

    private int hintsUsed = 0;

    private int verificationAttempts = 0;

    private void Start()
    {
        if (resultsPanel != null)
            resultsPanel.SetActive(false);

        if (continueButton != null)
            continueButton.onClick.AddListener(ContinueToNextMission);

        if (returnToMenuButton != null)
            returnToMenuButton.onClick.AddListener(ReturnToMenu);
    }

    private void OnDestroy()
    {
        if (continueButton != null)
            continueButton.onClick.RemoveListener(ContinueToNextMission);

        if (returnToMenuButton != null)
            returnToMenuButton.onClick.RemoveListener(ReturnToMenu);
    }

    private void ContinueToNextMission()
    {
        LoadScene(nextSceneName);
    }

    private void ReturnToMenu()
    {
        LoadScene(menuSceneName);
    }

    private void LoadScene(string sceneName)
    {
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError(
                "Cannot load scene '" + sceneName +
                "'. Check Build Profiles and the exact scene name."
            );
            return;
        }

        SceneManager.LoadScene(sceneName);
    }


    public void SelectPC(Mission02PCInteractable pc)
    {
        if (missionCompleted)
        {
            Debug.Log("Mission 02 is already complete.");
            return;
        }

        if (pcConnections.ContainsKey(pc))
        {
            Debug.Log(
                $"{pc.name} is already connected to Switch Port " +
                $"{pcConnections[pc].PortNumber}."
            );

            return;
        }

        selectedPC = pc;

        Debug.Log(
            $"{pc.name} is now the active PC for network connection."
        );

        Transform pcPoint =
            pc.transform.Find("CableConnectionPoint");

        if (heldCableVisual != null && pcPoint != null)
        {
            heldCableVisual.BeginCarry(pcPoint);
        }
        else
        {
            Debug.LogWarning(
                "Held cable or PC CableConnectionPoint is missing."
            );
        }
    }

    private void ShowResults()
    {
        if (resultsText != null)
        {
            resultsText.text =
                "MISSION 02 COMPLETE\n\n" +
                "NETWORK SUCCESSFULLY BUILT\n\n" +
                "[X] All four PCs connected\n" +
                "[X] Network verification passed";
        }

        if (resultsPanel != null)
            resultsPanel.SetActive(true);
        else
            Debug.LogError("Mission 2 Results Panel is not assigned.");
    }


    public Mission02PCInteractable GetSelectedPC()
    {
        return selectedPC;
    }


    public void ConnectSelectedPC(Mission02SwitchPort port)
    {
        if (missionCompleted)
        {
            Debug.Log("Mission 02 is already complete.");
            return;
        }

        if (heldCableVisual != null)
        {
            heldCableVisual.EndCarry();
        }

        if (selectedPC == null)
        {
            Debug.Log(
                "Select a PC before choosing a switch port."
            );

            return;
        }

        if (pcConnections.ContainsKey(selectedPC))
        {
            Debug.Log(
                $"{selectedPC.name} is already connected to Switch Port " +
                $"{pcConnections[selectedPC].PortNumber}."
            );

            selectedPC = null;

            return;
        }

        if (portConnections.ContainsKey(port))
        {
            RecordIncorrectConnectionAttempt();

            Debug.Log(
                $"Port {port.PortNumber} is already occupied by " +
                $"{portConnections[port].name}."
            );

            return;
        }

        Mission02PCInteractable connectedPC = selectedPC;

        pcConnections.Add(
            connectedPC,
            port
        );

        portConnections.Add(
            port,
            connectedPC
        );

        Debug.Log(
            $"{connectedPC.name} connected to Switch Port {port.PortNumber}."
        );


        // Show cable
        if (connectedPC.CableVisual != null)
        {
            Transform pcPoint =
                connectedPC.transform.Find("CableConnectionPoint");

            if (pcPoint != null)
            {
                connectedPC.CableVisual.ShowConnection(
                    pcPoint,
                    port.transform
                );
            }
            else
            {
                Debug.LogWarning(
                    $"CableConnectionPoint was not found on {connectedPC.name}."
                );
            }
        }
        else
        {
            Debug.LogWarning(
                $"{connectedPC.name} does not have a Cable Visual assigned."
            );
        }


        selectedPC = null;


        Debug.Log(
            $"Network progress: {GetConnectedPCCount()}/4 PCs connected."
        );
    }


    public int GetConnectedPCCount()
    {
        return pcConnections.Count;
    }

    public void RecordHintUsed()
    {
        hintsUsed++;

        Debug.Log(
            $"Hint used. Total hints: {hintsUsed}"
        );
    }

    public int GetHintsUsed()
    {
        return hintsUsed;
    }

    public void RecordIncorrectConnectionAttempt()
    {
        incorrectConnectionAttempts++;

        Debug.Log(
            $"Incorrect connection attempt. " +
            $"Total incorrect attempts: {incorrectConnectionAttempts}"
        );
    }

    public int GetIncorrectConnectionAttempts()
    {
        return incorrectConnectionAttempts;
    }

    public void RecordVerificationAttempt()
    {
        verificationAttempts++;

        Debug.Log(
            $"Network verification attempt: {verificationAttempts}"
        );
    }

    public int GetVerificationAttempts()
    {
        return verificationAttempts;
    }


    public bool VerifyNetwork()
    {
        int connectedPCs = GetConnectedPCCount();

        if (connectedPCs < 4)
        {
            Debug.Log(
                $"NETWORK TEST FAILED — {connectedPCs}/4 PCs are connected."
            );

            return false;
        }

        Debug.Log(
            "NETWORK TEST PASSED — All four computers are connected through the network switch."
        );

        CompleteMission();

        return true;
    }


    private void CompleteMission()
    {
        if (missionCompleted)
            return;

        missionCompleted = true;

        ShowResults();

        Debug.Log("===============================");
        Debug.Log("MISSION 02 COMPLETE!");
        Debug.Log("Network successfully constructed and verified.");
        Debug.Log("===============================");
    }
}