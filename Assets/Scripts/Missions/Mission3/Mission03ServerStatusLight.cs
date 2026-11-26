
using UnityEngine;

public class Mission03ServerStatusLight : MonoBehaviour
{
    [SerializeField] private Renderer lightRenderer;

    private Material runtimeMaterial;
    private Color originalEmissionColor;

    private static readonly Color ErrorColor =
        new Color(1f, 0f, 0f);

    private static readonly int EmissionColor =
        Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
        if (lightRenderer == null)
            lightRenderer = GetComponent<Renderer>();

        if (lightRenderer == null)
        {
            Debug.LogError(
                "Server status light has no Renderer.",
                this
            );
            enabled = false;
            return;
        }

        // Create a separate material instance for this light.
        runtimeMaterial = lightRenderer.material;

        originalEmissionColor =
            runtimeMaterial.GetColor(EmissionColor);
    }

    public void ShowConnectionError()
    {
        SetEmissionColor(ErrorColor * 5f);
    }

    public void ShowConnectionRestored()
    {
        SetEmissionColor(originalEmissionColor);
    }

    private void SetEmissionColor(Color color)
    {
        if (runtimeMaterial != null)
            runtimeMaterial.SetColor(EmissionColor, color);
    }

    private void OnDestroy()
    {
        if (runtimeMaterial != null)
            Destroy(runtimeMaterial);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F6))
            ShowConnectionError();

        if (Input.GetKeyDown(KeyCode.F7))
            ShowConnectionRestored();
    }

}
