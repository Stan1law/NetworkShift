using UnityEngine;

public class Mission02Briefing : MonoBehaviour
{
    [SerializeField] private GameObject briefingPanel;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private PlayerInteract playerInteract;

    private void Start()
    {
        briefingPanel.SetActive(true);

        // Disable player controls while briefing is open
        if (playerMovement != null)
            playerMovement.enabled = false;

        if (mouseLook != null)
            mouseLook.enabled = false;

        if (playerInteract != null)
            playerInteract.enabled = false;

        // Allow the player to use the UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartMission()
    {
        briefingPanel.SetActive(false);

        // Restore player controls
        if (playerMovement != null)
            playerMovement.enabled = true;

        if (mouseLook != null)
            mouseLook.enabled = true;

        if (playerInteract != null)
            playerInteract.enabled = true;

        // Return to first-person mouse control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}