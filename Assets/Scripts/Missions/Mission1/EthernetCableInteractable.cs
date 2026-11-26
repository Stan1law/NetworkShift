
using UnityEngine;

public class EthernetCableInteractable : Interactable
{
    [Header("Cable Pickup")]
    [SerializeField] private Transform carryPoint;
    [SerializeField]
    private Vector3 carriedLocalPosition =
        new Vector3(0f, 0f, 0.15f);
    [SerializeField]
    private Vector3 carriedLocalRotation =
        Vector3.zero;

    [Header("Cable Connection")]
    [SerializeField] private Transform switchConnectionPoint;

    private bool pickedUp;

    public bool IsPickedUp => pickedUp;
    public bool IsConnectedToPC { get; private set; }
    public bool IsConnectedToSwitch { get; private set; }

    public void ConnectToPC(Transform pcConnectionPoint)
    {
        if (!pickedUp || pcConnectionPoint == null)
        {
            Debug.LogWarning(
                "Pick up the cable and assign a PC connection point first."
            );
            return;
        }

        transform.SetParent(null);
        transform.SetPositionAndRotation(
            pcConnectionPoint.position,
            pcConnectionPoint.rotation
        );

        IsConnectedToPC = true;

        Debug.Log("Ethernet cable attached to PC7.");
    }



    public void HideAfterSuccessfulConnection()
    {
        // Stop showing the physical cable model in the player's hand.
        pickedUp = false;

        // Hide only this cable GameObject.
        gameObject.SetActive(false);

        Debug.Log("Held Ethernet cable hidden after successful connection.");
    }



    public bool ConnectToSwitch()
    {
        if (!IsConnectedToPC)
        {
            Debug.LogWarning(
                "Connect the Ethernet cable to PC7 before the switch."
            );
            return false;
        }

        if (switchConnectionPoint == null)
        {
            Debug.LogError(
                "EthernetCableInteractable: Switch Connection Point is not assigned!"
            );
            return false;
        }

        IsConnectedToSwitch = true;

        Debug.Log("Ethernet cable connected to the switch.");
        return true;
    }

    public override void Interact()
    {
        if (pickedUp)
        {
            Debug.Log("Ethernet cable is already picked up.");
            return;
        }

        MissionObjectiveManager objectiveManager =
            FindFirstObjectByType<MissionObjectiveManager>();

        if (objectiveManager == null)
        {
            Debug.LogError("MissionObjectiveManager not found!");
            return;
        }

        if (!objectiveManager.PCInspected)
        {
            Debug.Log("Inspect PC7 before picking up the Ethernet cable.");
            return;
        }

        if (carryPoint == null)
        {
            Debug.LogError(
                "EthernetCableInteractable: Carry Point is not assigned!"
            );
            return;
        }

        objectiveManager.CompleteCableFound();

        if (!objectiveManager.CableFound)
            return;

        pickedUp = true;

        transform.SetParent(carryPoint, false);
        transform.localPosition = carriedLocalPosition;
        transform.localRotation =
            Quaternion.Euler(carriedLocalRotation);

        foreach (Collider cableCollider in
                 GetComponentsInChildren<Collider>())
        {
            cableCollider.enabled = false;
        }

        Debug.Log("Ethernet cable picked up and is following the player.");
    }
}
