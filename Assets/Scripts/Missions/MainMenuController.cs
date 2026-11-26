
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string missionOneSceneName = "Mission_01";

    public void StartShift()
    {
        if (!Application.CanStreamedLevelBeLoaded(missionOneSceneName))
        {
            Debug.LogError("Cannot load Mission 1. Check the scene name and Build Profiles.");
            return;
        }

        SceneManager.LoadScene(missionOneSceneName);
    }
}
