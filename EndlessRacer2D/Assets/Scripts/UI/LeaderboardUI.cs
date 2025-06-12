using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    public GameObject leaderboardPanel;
    public TMP_Text leaderboardText;
    public Button viewLeaderboardButton;

    private void Start()
    {
        leaderboardPanel.SetActive(false); // Hide initially
    }

    public void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);
        viewLeaderboardButton.gameObject.SetActive(false);

        Debug.Log("Leaderboard Button Clicked");
        Debug.Log($"BackendManager.Instance: {BackendManager.Instance != null}");
        Debug.Log($"leaderboardText assigned: {leaderboardText != null}");


        // Force update leaderboardText reference each time
        if (BackendManager.Instance != null)
        {
            BackendManager.Instance.leaderboardText = leaderboardText;
            BackendManager.Instance.FetchLeaderboard();
        }
        else
        {
            Debug.LogError("BackendManager.Instance is null");
        }
    }

    public void HideLeaderboard()
    {
        leaderboardPanel.SetActive(false);
        viewLeaderboardButton.gameObject.SetActive(true);
    }
}
