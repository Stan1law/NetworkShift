using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mission03DiagnosisController : MonoBehaviour
{
    [Header("Diagnosis Panel")]
    [SerializeField] private GameObject panel;

    [Header("Diagnosis Choices")]
    [SerializeField] private Toggle ethernetChoice;
    [SerializeField] private Toggle switchChoice;
    [SerializeField] private Toggle ipChoice;
    [SerializeField] private Toggle fileServerChoice;

    [Header("Feedback")]
    [SerializeField] private TMP_Text feedbackText;

    [Header("Repair UI")]
    [SerializeField] private Mission03RepairController repairController;

    [Header("Player Control References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private PlayerInteract playerInteract;

    public void Show()
    {
        if (panel != null)
            panel.SetActive(true);

        // Disable player controls while the UI is open.
        DisablePlayerControls();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (InteractionUI.Instance != null)
            InteractionUI.Instance.Hide();
    }

    public void Hide()
    {
        if (panel == null)
            return;

        panel.SetActive(false);

        RestorePlayerControls();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ConfirmDiagnosis()
    {
        Debug.Log("CONFIRM BUTTON CLICKED");

        Mission03Manager manager =
            FindFirstObjectByType<Mission03Manager>();

        if (manager == null)
        {
            Debug.LogWarning("Mission03Manager was not found.");
            return;
        }

        if (!ethernetChoice.isOn &&
            !switchChoice.isOn &&
            !ipChoice.isOn &&
            !fileServerChoice.isOn)
        {
            feedbackText.text =
                "Please select a possible cause before confirming.";

            return;
        }

        bool isIPConfiguration = ipChoice.isOn;

        bool correct = manager.SubmitDiagnosis(isIPConfiguration);

        if (correct)
        {
            Debug.Log("Mission 03 diagnosis completed.");

            Hide();

            if (repairController != null)
            {
                repairController.Show();
            }
            else
            {
                Debug.LogWarning(
                    "Mission03RepairController is not assigned."
                );
            }
        }
        else
        {
            feedbackText.text =
                "INCORRECT DIAGNOSIS\n\n" +
                "Continue investigating the network evidence.";
        }
    }

    private void Update()
    {
        if (panel != null &&
            panel.activeSelf &&
            Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
        }
    }

    private void DisablePlayerControls()
    {
        if (playerMovement != null)
            playerMovement.enabled = false;

        if (mouseLook != null)
            mouseLook.enabled = false;

        if (playerInteract != null)
            playerInteract.enabled = false;
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
}