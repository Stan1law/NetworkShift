using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetworkSwitchInspectionPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject panelGameObject;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text deviceText;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text portText;
    [SerializeField] private TMP_Text instructionText;
    [SerializeField] private Button closeButton;

    [Header("Player Control References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private PlayerInteract playerInteract;

    private void OnEnable()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseInspectionUI);
    }

    private void OnDisable()
    {
        if (closeButton != null)
            closeButton.onClick.RemoveListener(CloseInspectionUI);
    }

    private void Update()
    {
        if (panelGameObject != null &&
            panelGameObject.activeSelf &&
            Input.GetKeyDown(KeyCode.Escape))
        {
            CloseInspectionUI();
        }
    }

    public void OpenInspectionUI()
    {
        DisablePlayerControls();

        if (panelGameObject != null)
            panelGameObject.SetActive(true);

        if (titleText != null)
            titleText.text = "NETWORK SWITCH INSPECTION";

        if (deviceText != null)
            deviceText.text = "Device: Core Network Switch";

        if (statusText != null)
            statusText.text = "Status: CONNECTION INCOMPLETE";

        if (portText != null)
            portText.text = "Port 7: DISCONNECTED";

        if (instructionText != null)
            instructionText.text =
                "An Ethernet cable is required to restore the network connection.";

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        if (InteractionUI.Instance != null)
            InteractionUI.Instance.Hide();

        Debug.Log("Network Switch Inspection UI opened.");
    }

    public void CloseInspectionUI()
    {
        if (panelGameObject != null)
            panelGameObject.SetActive(false);

        RestorePlayerControls();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Network Switch Inspection UI closed.");
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