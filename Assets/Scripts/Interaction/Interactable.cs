using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string interactionText = "Interact";

    public virtual void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
}