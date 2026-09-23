using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CableConnectionPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject panelGameObject;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text instructionText;
    [SerializeField] private Button cancelButton;

    [Header("Player Control References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private PlayerInteract playerInteract;

    private NetworkSwitchInteractable currentNetworkSwitch;

    private void OnEnable()
    {
        if (cancelButton != null)
            cancelButton.onClick.AddListener(OnCancelClicked);
    }

    private void OnDisable()
    {
        if (cancelButton != null)
            cancelButton.onClick.RemoveListener(OnCancelClicked);
    }

    private void Update()
    {
        // Handle Escape key to cancel
        if (panelGameObject != null && panelGameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            OnCancelClicked();
        }
    }

    public void OpenConnectionUI(NetworkSwitchInteractable networkSwitch)
    {
        if (networkSwitch == null)
        {
            Debug.LogError("CableConnectionPanel: NetworkSwitchInteractable reference is null!");
            return;
        }

        currentNetworkSwitch = networkSwitch;

        // Disable player controls IMMEDIATELY
        DisablePlayerControls();

        // Show the UI panel
        if (panelGameObject != null)
            panelGameObject.SetActive(true);

        // Set UI text
        if (titleText != null)
            titleText.text = "CONNECT ETHERNET CABLE";

        if (instructionText != null)
            instructionText.text = "Select a port to connect";

        // Unlock and show cursor
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        // Hide interaction prompt
        if (InteractionUI.Instance != null)
            InteractionUI.Instance.Hide();

        Debug.Log("Cable Connection UI opened.");
    }

    public void OnPortClicked(int portNumber)
    {
        if (currentNetworkSwitch == null)
        {
            Debug.LogError("CableConnectionPanel: No active network switch!");
            return;
        }

        bool connectionSuccessful =
            currentNetworkSwitch.OnPortSelected(portNumber);

        if (connectionSuccessful)
        {
            CloseConnectionUI();
        }
        else
        {
            if (instructionText != null)
            {
                instructionText.text =
                    $"Port {portNumber} is incorrect. Try another port.";
            }

            Debug.Log($"Port {portNumber} was incorrect. Connection UI remains open.");
        }
    }

    public void OnCancelClicked()
    {
        Debug.Log("Cable Connection UI cancelled.");
        CloseConnectionUI();
    }

    private void CloseConnectionUI()
    {
        // Hide the UI panel
        if (panelGameObject != null)
            panelGameObject.SetActive(false);

        // Restore player controls
        RestorePlayerControls();

        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentNetworkSwitch = null;
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
