using UnityEngine;
using TMPro;

public class Mission03SwitchInspectionPanel : MonoBehaviour
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
        DisablePlayerControls();

        panel.SetActive(true);

        inspectionText.text =
            "NETWORK SWITCH STATUS\n\n" +
            "Port 8: ACTIVE\n" +
            "Link: DETECTED\n\n" +
            "The switch appears to be operating normally.\n\n" +
            "Press ESC to close.";

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

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