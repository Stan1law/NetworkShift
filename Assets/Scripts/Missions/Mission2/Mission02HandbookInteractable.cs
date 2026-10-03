using UnityEngine;

public class Mission02HandbookInteractable : Interactable
{
    [SerializeField] private GameObject handbookPanel;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private PlayerInteract playerInteract;

    private bool isPanelOpen = false;

    public override void Interact()
    {
        if (isPanelOpen)
            return;

        OpenHandbook();
    }

    private void OpenHandbook()
    {
        if (handbookPanel == null)
        {
            Debug.LogWarning("Handbook panel is not assigned.");
            return;
        }

        handbookPanel.SetActive(true);
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
    }

    private void Update()
    {
        if (!isPanelOpen)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseHandbook();
        }
    }

    private void CloseHandbook()
    {
        handbookPanel.SetActive(false);
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