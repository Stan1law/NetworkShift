using TMPro;
using UnityEngine;

public class Mission02VerificationStation : Interactable
{
    [Header("Verification UI")]
    [SerializeField] private GameObject verificationPanel;
    [SerializeField] private TMP_Text verificationText;

    [Header("Results UI")]
    [SerializeField] private GameObject resultsPanel;
    [SerializeField] private TMP_Text resultsText;
    [SerializeField] private GameObject objectivePanel;

    [Header("Player Controls")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private PlayerInteract playerInteract;

    private bool isPanelOpen = false;
    private bool missionCompleted = false;

    private int verificationAttempts = 0;

    public override void Interact()
    {
        if (isPanelOpen || missionCompleted)
            return;

        Mission02NetworkManager manager =
            FindFirstObjectByType<Mission02NetworkManager>();

        if (manager == null)
        {
            Debug.LogWarning(
                "Mission02NetworkManager was not found."
            );
            return;
        }

        int connectedPCs = manager.GetConnectedPCCount();

        manager.RecordVerificationAttempt();
        verificationAttempts++;

        verificationPanel.SetActive(true);
        isPanelOpen = true;

        SetPlayerControls(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (connectedPCs < 4)
        {
            verificationText.text =
                "NETWORK TEST FAILED\n\n" +
                $"{connectedPCs} of 4 computers are connected.\n\n" +
                "Check the network connections, then try again.";

            Debug.Log(
                $"NETWORK TEST FAILED — {connectedPCs}/4 PCs connected."
            );

            return;
        }

        // Network test passed.
        verificationPanel.SetActive(false);
        isPanelOpen = false;
        missionCompleted = true;

        if (InteractionUI.Instance != null)
            InteractionUI.Instance.Hide();

        // Hide the gameplay objective behind the results screen.
        if (objectivePanel != null)
            objectivePanel.SetActive(false);

        if (resultsPanel != null)
            resultsPanel.SetActive(true);

        if (resultsText != null)
        {
            resultsText.text =
                "MISSION 02 COMPLETE\n\n" +
                "NETWORK TEST PASSED\n\n" +
                "All four employee PCs are connected.\n\n" +
                "Network connections: " +
                manager.GetConnectedPCCount() + " / 4\n\n" +
                "Incorrect connection attempts: " +
                manager.GetIncorrectConnectionAttempts() + "\n\n" +
                "Hints used: " +
                manager.GetHintsUsed() + "\n\n" +
                "Verification attempts: " +
                manager.GetVerificationAttempts() + "\n\n" +
                "SKILL PRACTICED\n\n" +
                "Build a network using a central network switch " +
                "and apply the principles of star topology.\n\n" +
                "Press ESC to finish.";
        }

        Debug.Log("MISSION 02 COMPLETE!");
    }

    private void Update()
    {
        if (isPanelOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseVerificationPanel();
            return;
        }

        if (missionCompleted && Input.GetKeyDown(KeyCode.Escape))
        {
            FinishMission();
        }
    }

    private void CloseVerificationPanel()
    {
        if (verificationPanel != null)
            verificationPanel.SetActive(false);

        isPanelOpen = false;

        SetPlayerControls(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SetPlayerControls(bool enabledState)
    {
        if (playerMovement != null)
            playerMovement.enabled = enabledState;

        if (mouseLook != null)
            mouseLook.enabled = enabledState;

        if (playerInteract != null)
            playerInteract.enabled = enabledState;
    }

    // Called by the results screen's Finish button.
    public void FinishMission()
    {
        if (resultsPanel != null)
            resultsPanel.SetActive(false);

        // Keep the objective hidden after mission completion.
        if (objectivePanel != null)
            objectivePanel.SetActive(false);

        SetPlayerControls(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Mission 02 results closed.");
    }
}