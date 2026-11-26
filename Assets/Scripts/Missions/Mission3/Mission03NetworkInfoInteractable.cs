using UnityEngine;

public class Mission03NetworkInfoInteractable : Interactable
{
    [SerializeField] private Mission03NetworkInfoPanel informationPanel;

    public override void Interact()
    {
        Debug.Log(
            "Office network information inspected."
        );

        Mission03Manager manager =
            FindFirstObjectByType<Mission03Manager>();

        if (manager != null)
        {
            manager.RecordNetworkInfoInspection();
        }

        if (informationPanel != null)
        {
            informationPanel.Show();
        }
    }
}