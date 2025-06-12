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
            Debug.Log("Binding Leaderboard Button OnClick");

            viewLeaderboardButton.onClick.RemoveAllListeners();
            viewLeaderboardButton.onClick.AddListener(() =>
            {
                Debug.Log("Leaderboard Button Clicked (from runtime binding)");
                ShowLeaderboard();
            });
        }
        else
        {
            Debug.LogError("❌ viewLeaderboardButton is null");
        }
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
