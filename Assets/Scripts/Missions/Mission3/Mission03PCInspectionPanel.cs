using UnityEngine;
using TMPro;

public class Mission03PCInspectionPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text inspectionText;

    [Header("Player Control References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private PlayerInteract playerInteract;

    public void Show()
    {
        // Disable player controls first
        DisablePlayerControls();

        // Show inspection panel
        panel.SetActive(true);


        inspectionText.text =
            "PC8 NETWORK STATUS\n\n" +
            "Connection: CONNECTED\n" +
            "Ethernet Link: ACTIVE\n" +
            "Network Access: FAILED\n" +
            "File Server: UNREACHABLE\n\n" +
            "The physical link is active, but PC8 cannot reach the company file server.\n\n" +
            "Press ESC to close.";


        // Show cursor
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        // Explicitly hide interaction prompt
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