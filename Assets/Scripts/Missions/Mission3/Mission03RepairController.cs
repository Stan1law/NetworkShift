
using UnityEngine;
using TMPro;

public class Mission03RepairController : MonoBehaviour
{
    [Header("Repair UI")]
    [SerializeField] private GameObject repairPanel;
    [SerializeField] private TMP_InputField ipInput;
    [SerializeField] private TMP_InputField subnetInput;
    [SerializeField] private TMP_InputField gatewayInput;
    [SerializeField] private TMP_Text feedbackText;

    [Header("Player Controls")]
    [SerializeField] private MonoBehaviour playerMovement;
    [SerializeField] private MonoBehaviour mouseLook;
    [SerializeField] private MonoBehaviour playerInteract;

    [Header("Verification UI")]
    [SerializeField] private Mission03VerificationController verificationController;

    public void Show()
    {
        DisablePlayerControls();

        if (repairPanel != null)
            repairPanel.SetActive(true);

        if (feedbackText != null)
        {
            feedbackText.text =
                "Correct diagnosis: IP configuration.\n\n" +
                "Enter the correct network settings below.";
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (InteractionUI.Instance != null)
            InteractionUI.Instance.Hide();
    }

    public void Hide()
    {
        if (repairPanel != null)
            repairPanel.SetActive(false);

        RestorePlayerControls();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ApplyConfiguration()
    {
        Mission03Manager manager =
            FindFirstObjectByType<Mission03Manager>();

        if (manager == null)
        {
            SetFeedback("ERROR\nMission manager unavailable.");
            Debug.LogError("Mission03Manager was not found.");
            return;
        }

        if (ipInput == null ||
            subnetInput == null ||
            gatewayInput == null)
        {
            SetFeedback("ERROR\nOne or more configuration fields are missing.");
            Debug.LogError("Repair input fields are not assigned.");
            return;
        }

        string newIP = ipInput.text.Trim();
        string newSubnet = subnetInput.text.Trim();
        string newGateway = gatewayInput.text.Trim();

        bool success = manager.ApplyConfiguration(
            newIP,
            newSubnet,
            newGateway
        );

        if (!success)
        {
            SetFeedback(
                "INCORRECT CONFIGURATION\n\n" +
                "Check the Office Network Info and try again."
            );

            Debug.Log("Mission 03 configuration was incorrect.");
            return;
        }

        Debug.Log("Mission 03 configuration completed.");

        SetFeedback(
            "CONFIGURATION ACCEPTED\n\n" +
            "PC8's network settings are correct.\n\n" +
            "Opening the connection test..."
        );

        if (verificationController == null)
        {
            Debug.LogWarning(
                "Mission03VerificationController is not assigned."
            );

            return;
        }

        // Close repair UI, then open verification UI.
        Hide();
        verificationController.Show();
    }

    private void SetFeedback(string message)
    {
        if (feedbackText != null)
            feedbackText.text = message;
    }

    private void DisablePlayerControls()
    {
        if (playerMovement != null)
            playerMovement.enabled = false;

        if (mouseLook != null)
            mouseLook.enabled = false;

        if (playerInteract != null)
            playerInteract.enabled = false;

        if (InteractionUI.Instance != null)
            InteractionUI.Instance.Hide();
    }

    private void RestorePlayerControls()
    {
        if (playerMovement != null)
            playerMovement.enabled = true;

        if (mouseLook != null)
            mouseLook.enabled = true;

        if (playerInteract != null)
            playerInteract.enabled = true;
    }

    private void Update()
    {
        if (repairPanel != null &&
            repairPanel.activeInHierarchy &&
            Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
        }
    }
}
