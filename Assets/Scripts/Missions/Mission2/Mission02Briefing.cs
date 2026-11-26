
using UnityEngine;

public class Mission02Briefing : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject briefingPanel;
    [SerializeField] private GameObject objectivePanel;

    [Header("Player Controls")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private PlayerInteract playerInteract;

    private void Start()
    {
        // Show briefing and hide objective at mission start.
        if (briefingPanel != null)
            briefingPanel.SetActive(true);

        if (objectivePanel != null)
            objectivePanel.SetActive(false);

        // Disable player controls while briefing is open.
        if (playerMovement != null)
            playerMovement.enabled = false;

        if (mouseLook != null)
            mouseLook.enabled = false;

        if (playerInteract != null)
            playerInteract.enabled = false;

        // Allow the player to use the UI.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartMission()
    {
        // Close briefing and show the objective HUD.
        if (briefingPanel != null)
            briefingPanel.SetActive(false);

        if (objectivePanel != null)
            objectivePanel.SetActive(true);

        // Restore player controls.
        if (playerMovement != null)
            playerMovement.enabled = true;

        if (mouseLook != null)
            mouseLook.enabled = true;

        if (playerInteract != null)
            playerInteract.enabled = true;

        // Return to first-person mouse control.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
