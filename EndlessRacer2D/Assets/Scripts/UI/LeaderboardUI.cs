using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    public GameObject leaderboardPanel;
    public TMP_Text leaderboardText;
    public Button viewLeaderboardButton;

    void Start()
    {
        leaderboardPanel.SetActive(false);

        if (viewLeaderboardButton != null)
        {
            viewLeaderboardButton.onClick.RemoveAllListeners();
            viewLeaderboardButton.onClick.AddListener(() =>
            {
                Debug.Log("Leaderboard Button Clicked (from runtime binding)");
                ShowLeaderboard();
            });
        }
    }

    public void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);
        viewLeaderboardButton.gameObject.SetActive(false);

        if (BackendManager.Instance != null)
        {
            BackendManager.Instance.leaderboardText = leaderboardText;
            BackendManager.Instance.FetchLeaderboard();
        }
        else
        {
            Debug.LogError("❌ BackendManager.Instance is null");
        }
    }

    public void HideLeaderboard()
    {
        leaderboardPanel.SetActive(false);
        viewLeaderboardButton.gameObject.SetActive(true);
    }
}
