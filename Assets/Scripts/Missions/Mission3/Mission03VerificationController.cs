
using UnityEngine;
using TMPro;

public class Mission03VerificationController : MonoBehaviour
{
    [Header("Verification UI")]
    [SerializeField] private GameObject verificationPanel;
    [SerializeField] private TMP_Text verificationText;

    [Header("Network Components")]
    [SerializeField] private GameObject networkSwitch;
    [SerializeField] private GameObject fileServer;

    [Header("Player Controls")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private PlayerInteract playerInteract;

    [Header("Mission Results UI")]
    [SerializeField] private GameObject resultsPanel;
    [SerializeField] private TMP_Text resultsText;

    private bool verificationOpen;
    private bool verificationPassed;

    public void Show()
    {
        if (verificationPanel == null)
        {
            Debug.LogError("VerificationPanel is not assigned.");
            return;
        }

        verificationPassed = false;
        verificationPanel.SetActive(true);
        verificationOpen = true;

        SetPlayerControls(false);

        if (InteractionUI.Instance != null)
            InteractionUI.Instance.Hide();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LateUpdate()
    {
        if (!verificationOpen)
            return;

        SetPlayerControls(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (InteractionUI.Instance != null)
            InteractionUI.Instance.Hide();
    }

    public void TestConnection()
    {
        Mission03Manager manager =
            FindFirstObjectByType<Mission03Manager>();

        if (manager == null)
        {
            ShowResult("TEST FAILED\n\nMission manager unavailable.");
            Debug.LogError("Mission03Manager was not found.");
            return;
        }

        manager.RecordVerificationAttempt();

        bool configurationCorrect =
            manager.IsConfigurationCorrect();

        bool switchAvailable =
            networkSwitch != null &&
            networkSwitch.activeInHierarchy;

        bool serverAvailable =
            fileServer != null &&
            fileServer.activeInHierarchy;

        if (configurationCorrect && switchAvailable && serverAvailable)
        {
            verificationPassed = true;
            manager.CompleteMission();

            ShowResult(
                "CONNECTION RESTORED!\n\n" +
                "PC8 can communicate with the company file server.\n\n" +
                "Network test: PASSED\n\n" +
                "Select Continue to view your results."
            );

            Debug.Log("Mission 03 connection verification passed.");
        }
        else
        {
            verificationPassed = false;

            ShowResult(
                "CONNECTION TEST FAILED\n\n" +
                "Check the PC8 configuration and network components, " +
                "then test again."
            );

            Debug.Log("Mission 03 connection verification failed.");
        }
    }

    private void ShowResult(string message)
    {
        if (verificationText != null)
            verificationText.text = message;
    }

    public void ContinueFromVerification()
    {
        // Prevent continuing to the results screen after a failed test.
        if (!verificationPassed)
        {
            ShowResult(
                "CONNECTION NOT VERIFIED\n\n" +
                "Pass the connection test before continuing."
            );
            return;
        }

        Mission03Manager manager =
            FindFirstObjectByType<Mission03Manager>();

        if (manager == null || !manager.IsMissionCompleted())
        {
            ShowResult("ERROR\n\nMission completion could not be confirmed.");
            return;
        }

        verificationOpen = false;

        if (verificationPanel != null)
            verificationPanel.SetActive(false);

        if (resultsPanel != null)
            resultsPanel.SetActive(true);

        if (resultsText != null)
        {
            resultsText.text =
                "MISSION COMPLETE!\n\n" +
                "CONNECTION RESTORED\n\n" +
                "PC8 can now communicate with the company file server.\n\n" +
                "PERFORMANCE SUMMARY\n\n" +
                "Investigation Evidence\n" +
                "- PC8 inspected: " + YesNo(manager.HasInspectedPC8()) + "\n" +
                "- Switch inspected: " + YesNo(manager.HasInspectedSwitch()) + "\n" +
                "- Office network information inspected: " +
                    YesNo(manager.HasInspectedNetworkInfo()) + "\n\n" +
                "Troubleshooting Performance\n" +
                "- Incorrect diagnosis attempts: " +
                    manager.GetWrongDiagnosisAttempts() + "\n" +
                "- Configuration attempts: " +
                    manager.GetConfigurationAttempts() + "\n" +
                "- Connection test attempts: " +
                    manager.GetVerificationAttempts() + "\n\n" +
                "Completion Time: " + manager.GetCompletionTime();
        }

        Debug.Log("Mission 03 results displayed.");
    }

    private string YesNo(bool value)
    {
        return value ? "Yes" : "No";
    }

    public void FinishMission()
    {
        if (resultsPanel != null)
            resultsPanel.SetActive(false);

        SetPlayerControls(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Mission 03 finish screen closed.");
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

    public void Hide()
    {
        verificationOpen = false;

        if (verificationPanel != null)
            verificationPanel.SetActive(false);

        SetPlayerControls(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
