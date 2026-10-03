using TMPro;
using UnityEngine;

public class Mission02VerificationStation : Interactable
{
    [SerializeField] private GameObject verificationPanel;
    [SerializeField] private TMP_Text verificationText;
    [SerializeField] private GameObject resultsPanel;
[SerializeField] private TMP_Text resultsText;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private PlayerInteract playerInteract;

    private bool isPanelOpen = false;
    private int verificationAttempts = 0;


    public override void Interact()
    {
        if (isPanelOpen)
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

        Debug.Log(
            $"Network verification attempt: {verificationAttempts}"
        );

        verificationPanel.SetActive(true);
        isPanelOpen = true;


        // Disable player controls
        if (playerMovement != null)
            playerMovement.enabled = false;

        if (mouseLook != null)
            mouseLook.enabled = false;

        if (playerInteract != null)
            playerInteract.enabled = false;


        // Show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


        // FAILED
        if (connectedPCs < 4)
        {
            verificationText.text =
                "NETWORK TEST FAILED\n\n" +
                $"{connectedPCs} of 4 computers are connected.\n\n" +
                "Check the network connections.";

            Debug.Log(
                $"NETWORK TEST FAILED — {connectedPCs}/4 PCs are connected."
            );
        }

        // PASSED
        else
        {
            verificationPanel.SetActive(false);

            resultsPanel.SetActive(true);

            resultsText.text =
                "MISSION 02 COMPLETE\n\n" +
                "NETWORK TEST PASSED\n\n" +
                "Network connections: " +
                manager.GetConnectedPCCount() + " / 4\n\n" +
                "Incorrect attempts: " +
                manager.GetIncorrectConnectionAttempts() + "\n\n" +
                "Hints used: " +
                manager.GetHintsUsed() + "\n\n" +
                "Verification attempts: " +
                manager.GetVerificationAttempts() + "\n\n" +
                "SKILL PRACTICED\n\n" +
                "Build a network using a central network switch " +
                "and apply the principles of star topology.\n\n" +
                "Press ESC to continue.";

            Debug.Log(
                "MISSION 02 COMPLETE!"
            );
        }
    }


    private void Update()
    {
        if (!isPanelOpen)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseVerificationPanel();
        }
    }


    private void CloseVerificationPanel()
    {
        verificationPanel.SetActive(false);

        if (resultsPanel != null)
            resultsPanel.SetActive(false);

        isPanelOpen = false;


        // Restore player controls
        if (playerMovement != null)
            playerMovement.enabled = true;

        if (mouseLook != null)
            mouseLook.enabled = true;

        if (playerInteract != null)
            playerInteract.enabled = true;


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}