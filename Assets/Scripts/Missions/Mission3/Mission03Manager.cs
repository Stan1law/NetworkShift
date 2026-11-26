
using UnityEngine;

public class Mission03Manager : MonoBehaviour
{
    // Investigation
    private bool pc8Inspected;
    private bool switchInspected;
    private bool networkInfoInspected;

    // Diagnosis
    private bool correctDiagnosis;
    private int wrongDiagnosisAttempts;

    // Configuration
    private string currentIP = "192.168.2.20";
    private string currentSubnet = "255.255.255.0";
    private string currentGateway = "192.168.2.1";

    private readonly string correctIP = "192.168.1.20";
    private readonly string correctSubnet = "255.255.255.0";
    private readonly string correctGateway = "192.168.1.1";

    private int configurationAttempts;
    private int verificationAttempts;

    // Mission performance
    private float missionStartTime;
    private float completionTimeSeconds;
    private bool missionCompleted;

    private void Awake()
    {
        missionStartTime = Time.time;
    }

    // Investigation
    public void RecordPC8Inspection()
    {
        pc8Inspected = true;
        Debug.Log("PC8 inspected. Physical connection appears normal.");
    }

    public void RecordSwitchInspection()
    {
        switchInspected = true;
        Debug.Log("Network switch inspected. Port 8 appears active.");
    }

    public void RecordNetworkInfoInspection()
    {
        networkInfoInspected = true;
        Debug.Log("Office network information inspected.");
    }

    public bool CanDiagnose()
    {
        return pc8Inspected && switchInspected && networkInfoInspected;
    }

    public bool HasInspectedPC8()
    {
        return pc8Inspected;
    }

    public bool HasInspectedNetworkInfo()
    {
        return networkInfoInspected;
    }

    public bool HasInspectedSwitch()
    {
        return switchInspected;
    }

    // Diagnosis
    public bool SubmitDiagnosis(bool isIPConfiguration)
    {
        if (!CanDiagnose())
        {
            Debug.Log("Complete the investigation before diagnosing the problem.");
            return false;
        }

        if (isIPConfiguration)
        {
            correctDiagnosis = true;
            Debug.Log("Correct diagnosis: IP configuration.");
            return true;
        }

        wrongDiagnosisAttempts++;
        Debug.Log("Incorrect diagnosis. Continue investigating the evidence.");
        return false;
    }

    public bool IsDiagnosisCorrect()
    {
        return correctDiagnosis;
    }

    public int GetWrongDiagnosisAttempts()
    {
        return wrongDiagnosisAttempts;
    }

    // Configuration repair
    public bool CanConfigure()
    {
        return correctDiagnosis;
    }

    public bool ApplyConfiguration(
        string newIP,
        string newSubnet,
        string newGateway)
    {
        if (!CanConfigure())
        {
            Debug.Log("Correctly diagnose the problem before changing the configuration.");
            return false;
        }

        configurationAttempts++;

        bool correct =
            newIP == correctIP &&
            newSubnet == correctSubnet &&
            newGateway == correctGateway;

        if (!correct)
        {
            Debug.Log("Incorrect network configuration.");
            return false;
        }

        currentIP = newIP;
        currentSubnet = newSubnet;
        currentGateway = newGateway;

        Debug.Log("Network configuration updated successfully.");
        return true;
    }

    public bool IsConfigurationCorrect()
    {
        return currentIP == correctIP
            && currentSubnet == correctSubnet
            && currentGateway == correctGateway;
    }

    public int GetConfigurationAttempts()
    {
        return configurationAttempts;
    }

    // Verification and completion
    public void RecordVerificationAttempt()
    {
        verificationAttempts++;
    }

    public int GetVerificationAttempts()
    {
        return verificationAttempts;
    }

    public void CompleteMission()
    {
        if (missionCompleted)
            return;

        missionCompleted = true;
        completionTimeSeconds = Time.time - missionStartTime;

        Debug.Log("Mission 03 completed and performance recorded.");
    }

    public bool IsMissionCompleted()
    {
        return missionCompleted;
    }

    public string GetCompletionTime()
    {
        float seconds = missionCompleted
            ? completionTimeSeconds
            : Time.time - missionStartTime;

        int totalSeconds = Mathf.FloorToInt(seconds);
        int minutes = totalSeconds / 60;
        int remainingSeconds = totalSeconds % 60;

        return $"{minutes:00}:{remainingSeconds:00}";
    }
}
