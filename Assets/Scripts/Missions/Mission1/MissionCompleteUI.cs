
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MissionCompleteUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject objectivePanel;
    [SerializeField] private GameObject interactionPanel;

    [Header("UI References")]
    [SerializeField] private TMP_Text objectivesText;
    [SerializeField] private Button returnToMenuButton;
    [SerializeField] private Button continueButton;

    [Header("Progression")]
    [SerializeField] private string nextSceneName = "Mission_02";

    [Header("Player Controls")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private PlayerInteract playerInteract;

    [Header("Scene Settings")]
    [SerializeField] private string menuSceneName = "Lobby";

    private bool missionCompleteShown;

    private void Awake()
    {
        if (continueButton != null)
            continueButton.onClick.AddListener(ContinueToNextMission);

        if (panel != null)
            panel.SetActive(false);

        if (returnToMenuButton != null)
            returnToMenuButton.onClick.AddListener(ReturnToMenu);
    }

    private void OnDestroy()
    {
        if (continueButton != null)
            continueButton.onClick.RemoveListener(ContinueToNextMission);

        if (returnToMenuButton != null)
            returnToMenuButton.onClick.RemoveListener(ReturnToMenu);
    }

    private void ContinueToNextMission()
    {
        if (string.IsNullOrWhiteSpace(nextSceneName))
        {
            Debug.Log("There is no next mission configured.");
            return;
        }

        if (Application.CanStreamedLevelBeLoaded(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError(
                "Cannot load next mission: " + nextSceneName +
                ". Check Build Profiles and the scene name."
            );
        }
    }


    private void Update()
    {
        if (!missionCompleteShown)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ContinueToNextMission();
        }
    }


    public void ShowMissionComplete()
    {
        if (missionCompleteShown)
            return;

        if (panel == null)
        {
            Debug.LogError(
                "MissionCompleteUI: MissionCompletePanel is not assigned."
            );
            return;
        }

        missionCompleteShown = true;

        // Hide the normal gameplay UI directly.
        if (objectivePanel != null)
            objectivePanel.SetActive(false);

        if (interactionPanel != null)
            interactionPanel.SetActive(false);

        // Stop gameplay controls.
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
            Debug.Log("Mission Complete: PlayerMovement disabled.");
        }

        CharacterController characterController =
            playerMovement != null
                ? playerMovement.GetComponent<CharacterController>()
                : null;

     

        if (mouseLook != null)
            mouseLook.enabled = false;


        if (playerInteract != null) 
        {
            playerInteract.SetInteractionLocked(true);
            playerInteract.enabled = false;
        }


        // Populate the completed objectives.
        if (objectivesText != null)
        {
            objectivesText.text =
                "[X] Inspect PC7\n\n" +
                "[X] Find the Ethernet Cable\n\n" +
                "[X] Inspect the Network Switch\n\n" +
                "[X] Connect the Cable";
        }

        // Show the completion screen last.
        panel.SetActive(true);

        Debug.Log("=== MISSION COMPLETE UI CHECK ===");
        Debug.Log("Objective Panel: " +
            (objectivePanel != null ? objectivePanel.activeSelf.ToString() : "MISSING"));
        Debug.Log("Interaction Panel: " +
            (interactionPanel != null ? interactionPanel.activeSelf.ToString() : "MISSING"));
        Debug.Log("Player Movement Enabled: " +
            (playerMovement != null ? playerMovement.enabled.ToString() : "MISSING"));
        Debug.Log("Mouse Look Enabled: " +
            (mouseLook != null ? mouseLook.enabled.ToString() : "MISSING"));


        // Release the mouse so the completion UI can be clicked.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Keep the first-person camera from capturing the mouse.
        if (mouseLook != null)
        {
            mouseLook.enabled = false;
        }

        // Disable interaction input while the completion UI is open.
        if (playerInteract != null)
        {
            playerInteract.enabled = false;
        }

        if (mouseLook != null)
        {
            mouseLook.enabled = false;
        }

        Debug.Log("Mission Complete: Cursor unlocked and visible.");

        Debug.Log("Mission Complete UI displayed; gameplay HUD hidden.");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log(
            $"Cursor check: visible={Cursor.visible}, " +
            $"lockState={Cursor.lockState}"
        );
    }

    private void ReturnToMenu()
    {
        if (Application.CanStreamedLevelBeLoaded(menuSceneName))
            SceneManager.LoadScene(menuSceneName);
        else
            Debug.LogError(
                "Cannot load scene '" + menuSceneName +
                "'. Check Build Profiles / Scene List."
            );
    }
}
