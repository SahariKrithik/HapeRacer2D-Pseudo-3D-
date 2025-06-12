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
        leaderboardPanel.SetActive(false);

        // 🟩 Safely ensure the button is always hooked
        viewLeaderboardButton.onClick.RemoveAllListeners();
        viewLeaderboardButton.onClick.AddListener(ShowLeaderboard);
    }

    public void ShowLeaderboard()
    {
        Debug.Log("Leaderboard Button Clicked");

        leaderboardPanel.SetActive(true);
        viewLeaderboardButton.gameObject.SetActive(false);

        if (BackendManager.Instance != null)
        {
            BackendManager.Instance.leaderboardText = leaderboardText;
            Debug.Log("BackendManager.Instance: True");
            Debug.Log("leaderboardText assigned: " + (leaderboardText != null));
            BackendManager.Instance.FetchLeaderboard();
        }
        else
        {
            Debug.LogError("❌BackendManager.Instance is null");
        }
    }

    public void HideLeaderboard()
    {
        leaderboardPanel.SetActive(false);
        viewLeaderboardButton.gameObject.SetActive(true);
    }
}
