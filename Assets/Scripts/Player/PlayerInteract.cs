
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 3f;

    private Interactable currentInteractable;
    private bool interactionLocked;

    public void SetInteractionLocked(bool locked)
    {
        interactionLocked = locked;
        currentInteractable = null;

        if (InteractionUI.Instance != null)
            InteractionUI.Instance.Hide();
    }

    void Update()
    {
        if (interactionLocked)
            return;

        CheckForInteractable();

        if (Input.GetKeyDown(KeyCode.E) &&
            currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void CheckForInteractable()
    {
        if (Camera.main == null ||
            InteractionUI.Instance == null)
        {
            currentInteractable = null;
            return;
        }

        Ray ray = Camera.main.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0)
        );

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            Interactable interactable =
                hit.collider.GetComponentInParent<Interactable>();

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

    private void OnDisable()
    {
        currentInteractable = null;

        if (InteractionUI.Instance != null)
            InteractionUI.Instance.Hide();
    }
}
