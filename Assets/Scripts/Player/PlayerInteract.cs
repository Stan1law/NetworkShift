using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 3f;

    private Interactable currentInteractable;

    void Update()
    {
        CheckForInteractable();

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void CheckForInteractable()
    {
        if (Camera.main == null)
        {
            Debug.Log("NO MAIN CAMERA");
            return;
        }

        if (InteractionUI.Instance == null)
        {
            Debug.Log("NO INTERACTION UI");
            return;
        }

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                currentInteractable = interactable;
                InteractionUI.Instance.Show(interactable.interactionText);
                return;
            }
        }

        currentInteractable = null;
        InteractionUI.Instance.Hide();
    }
}