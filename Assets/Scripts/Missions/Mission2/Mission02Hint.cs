using UnityEngine;

public class Mission02Hint : MonoBehaviour
{
    [SerializeField] private GameObject hintPanel;

    private bool isHintOpen = false;

    private void Start()
    {
        if (hintPanel != null)
        {
            hintPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleHint();
        }
    }

    private void ToggleHint()
    {
        if (hintPanel == null)
        {
            Debug.LogWarning("Hint panel is not assigned.");
            return;
        }

        isHintOpen = !isHintOpen;

        hintPanel.SetActive(isHintOpen);

        if (isHintOpen)
        {
            Mission02NetworkManager manager =
                FindFirstObjectByType<Mission02NetworkManager>();

            if (manager != null)
            {
                manager.RecordHintUsed();
            }
            else
            {
                Debug.LogWarning(
                    "Mission02NetworkManager was not found."
                );
            }
        }
    }
}