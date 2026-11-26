using UnityEngine;
using TMPro;

public class Mission03NetworkInfoPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text informationText;

    [Header("Player Control References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private PlayerInteract playerInteract;

    public void Show()
    {
        DisablePlayerControls();

        panel.SetActive(true);

        informationText.text =
            "OFFICE NETWORK\n\n" +
            "Network: 192.168.1.x\n" +
            "Subnet Mask: 255.255.255.0\n" +
            "Gateway: 192.168.1.1\n" +
            "File Server: 192.168.1.10\n\n" +
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