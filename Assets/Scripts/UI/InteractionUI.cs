using TMPro;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI Instance;

    public GameObject panel;
    public TMP_Text promptText;

    private void Awake()
    {
        Instance = this;

        panel.SetActive(false);
    }

    public void Show(string text)
    {
        panel.SetActive(true);
        promptText.text = "[E]\n" + text;
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}