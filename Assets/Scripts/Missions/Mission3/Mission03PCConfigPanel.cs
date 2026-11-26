using UnityEngine;
using TMPro;

public class Mission03PCConfigPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text configurationText;

    [Header("Player Control References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private PlayerInteract playerInteract;

    [Header("Diagnosis")]
    [SerializeField] private Mission03DiagnosisController diagnosisController;

    public void Show()
    {
        // Disable player controls
        DisablePlayerControls();

        // Show panel
        panel.SetActive(true);

        configurationText.text =
            "PC8 NETWORK CONFIGURATION\n\n" +
            "IP Address: 192.168.2.20\n" +
            "Subnet Mask: 255.255.255.0\n" +
            "Gateway: 192.168.2.1\n\n" +
            "Press ESC to close.";

        // Show cursor
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        // Hide interaction prompt
        if (InteractionUI.Instance != null)
        {
            InteractionUI.Instance.Hide();
        }
    }

    public void Hide()
    {
        panel.SetActive(false);

        RestorePlayerControls();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (diagnosisController != null)
        {
            diagnosisController.Show();
        }
    }

    private void Update()
    {
        if (panel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
        }
    }

    private void DisablePlayerControls()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        if (mouseLook != null)
        {
            mouseLook.enabled = false;
        }

        if (playerInteract != null)
        {
            playerInteract.enabled = false;
        }
    }

    private void RestorePlayerControls()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        if (mouseLook != null)
        {
            mouseLook.enabled = true;
        }

        if (playerInteract != null)
        {
            playerInteract.enabled = true;
        }
    }
}