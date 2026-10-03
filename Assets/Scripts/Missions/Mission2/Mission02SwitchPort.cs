using UnityEngine;

public class Mission02SwitchPort : Interactable
{
    [SerializeField] private int portNumber = 1;

    public int PortNumber => portNumber;

    public override void Interact()
    {
        Mission02NetworkManager manager =
            FindFirstObjectByType<Mission02NetworkManager>();

        if (manager == null)
        {
            Debug.LogWarning("Mission02NetworkManager was not found.");
            return;
        }

        Mission02PCInteractable selectedPC = manager.GetSelectedPC();

        if (selectedPC == null)
        {
            Debug.Log("Select a PC before choosing a switch port.");
            return;
        }

        manager.ConnectSelectedPC(this);
    }
}