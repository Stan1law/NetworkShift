
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PC7InspectionPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject panelGameObject;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text deviceText;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button connectCableButton;

    [Header("Cable Connection")]
    [SerializeField] private Mission01HeldCableVisual heldCableVisual;
    [SerializeField] private Transform pcConnectionPoint;

    [Header("Player Controls")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private PlayerInteract playerInteract;

    [Header("Cable Connection")]
    [SerializeField] private EthernetCableInteractable ethernetCable;
    [SerializeField] private Transform pc7CableConnectionPoint;

    private void ConnectCableToPC()
    {
        MissionObjectiveManager objectiveManager =
            FindFirstObjectByType<MissionObjectiveManager>();

        if (objectiveManager == null)
        {
            Debug.LogError("MissionObjectiveManager not found!");
            return;
        }

        if (!objectiveManager.CableFound)
        {
            if (messageText != null)
                messageText.text = "Find the Ethernet cable first.";

            return;
        }

        if (ethernetCable == null || pc7CableConnectionPoint == null)
        {
            Debug.LogError(
                "Assign the Ethernet Cable and PC7 Connection Point references."
            );

            if (messageText != null)
                messageText.text = "Cable connection setup is incomplete.";

            return;
        }

        if (!ethernetCable.IsPickedUp)
        {
            if (messageText != null)
                messageText.text = "Pick up the Ethernet cable first.";

            return;
        }

        if (ethernetCable.IsConnectedToPC)
        {
            if (messageText != null)
                messageText.text = "The cable is already attached to PC7.";

            return;
        }

        ethernetCable.ConnectToPC(pc7CableConnectionPoint);

        if (!ethernetCable.IsConnectedToPC)
            return;

        objectiveManager.CompleteCableConnectedToPC();

        if (objectiveManager.CableConnectedToPC)
        {
            if (heldCableVisual != null && pcConnectionPoint != null)
            {
                heldCableVisual.BeginCarry(pcConnectionPoint);
            }
            else
            {
                Debug.LogError(
                    "Assign Held Cable Visual and PC Connection Point " +
                    "on PC7InspectionPanel."
                );
            }
        }

        if (statusText != null)
        {
            statusText.text =
                "Connection: CABLE ATTACHED TO PC7\n" +
                "Switch Link: NOT CONNECTED";
        }

        if (messageText != null)
        {
            messageText.text =
                "Cable connected to PC7. Now inspect the network switch " +
                "and select Port 7.";
        }

        if (connectCableButton != null)
            connectCableButton.interactable = false;

        Debug.Log("PC7-side cable connection recorded.");
    }

    private void OnEnable()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(Close);

        if (connectCableButton != null)
            connectCableButton.onClick.AddListener(ConnectCableToPC);
    }

    private void OnDisable()
    {
        if (closeButton != null)
            closeButton.onClick.RemoveListener(Close);

        if (connectCableButton != null)
            connectCableButton.onClick.RemoveListener(ConnectCableToPC);
    }

    private void Update()
    {
        if (panelGameObject != null &&
            panelGameObject.activeSelf &&
            Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    public void Open()
    {
        if (panelGameObject == null)
        {
            Debug.LogError("PC7InspectionPanel: Panel reference is missing.");
            return;
        }

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (mouseLook != null)
            mouseLook.enabled = false;

        if (playerInteract != null)
            playerInteract.enabled = false;

        panelGameObject.SetActive(true);

        if (titleText != null)
            titleText.text = "PC7 — NETWORK STATUS";

        if (deviceText != null)
            deviceText.text = "Device: Employee Workstation PC7";

        if (statusText != null)
            statusText.text =
                "Connection: DISCONNECTED\n" +
                "Ethernet Link: INACTIVE";

        if (messageText != null)
            messageText.text =
                "No network connection detected.\n\n" +
                "Inspect the Ethernet cable and network switch.";

        if (InteractionUI.Instance != null)
            InteractionUI.Instance.Hide();

        MissionObjectiveManager objectiveManager =
            FindFirstObjectByType<MissionObjectiveManager>();

        if (objectiveManager != null)
            objectiveManager.SetObjectivePanelVisible(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("PC7 inspection UI opened.");
    }

    public void Close()
    {
        if (panelGameObject != null)
            panelGameObject.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (mouseLook != null)
            mouseLook.enabled = true;

        if (playerInteract != null)
            playerInteract.enabled = true;

        MissionObjectiveManager objectiveManager =
            FindFirstObjectByType<MissionObjectiveManager>();

        if (objectiveManager != null)
            objectiveManager.SetObjectivePanelVisible(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("PC7 inspection UI closed.");
    }
}
