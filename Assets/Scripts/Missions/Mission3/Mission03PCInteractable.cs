using UnityEngine;

public class Mission03PCInteractable : Interactable
{
    [SerializeField] private string pcName = "PC8";

    [Header("Inspection UI")]
    [SerializeField] private Mission03PCInspectionPanel inspectionPanel;

    [Header("Diagnosis UI")]
    [SerializeField] private Mission03DiagnosisController diagnosisController;

    public override void Interact()
    {
        Mission03Manager manager =
            FindFirstObjectByType<Mission03Manager>();

        if (manager == null)
        {
            Debug.LogWarning("Mission03Manager was not found.");
            return;
        }

        // First PC8 interaction
        if (!manager.HasInspectedPC8())
        {
            Debug.Log(
                $"{pcName} inspected. Physical connection appears normal."
            );

            manager.RecordPC8Inspection();

            if (inspectionPanel != null)
            {
                inspectionPanel.Show();
            }

            return;
        }

        // PC8 has already been inspected.
        // Require the switch and office network information
        // before opening the diagnosis.
        if (!manager.HasInspectedNetworkInfo())
        {
            Debug.Log(
                "Investigate the office network information before diagnosing the problem."
            );

            return;
        }

        Debug.Log(
            "Investigation complete. Opening diagnosis."
        );

        if (diagnosisController != null)
        {
            diagnosisController.Show();
        }
    }
}