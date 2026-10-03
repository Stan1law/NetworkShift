using UnityEngine;

public class Mission02PCInteractable : Interactable
{
    [SerializeField] private string pcName = "PC1";
    [SerializeField] private Mission02CableVisual cableVisual;

    private bool isSelected = false;

    public bool IsSelected => isSelected;

    public Mission02CableVisual CableVisual => cableVisual;

    public override void Interact()
    {
        isSelected = true;

        Debug.Log($"{pcName} selected. Connect this PC to the network switch.");

        Mission02NetworkManager manager =
            FindFirstObjectByType<Mission02NetworkManager>();

        if (manager != null)
        {
            manager.SelectPC(this);
        }
    }
}