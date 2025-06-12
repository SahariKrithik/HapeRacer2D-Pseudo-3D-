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
        leaderboardPanel.SetActive(false); // Hide by default
    }

    public void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);
        viewLeaderboardButton.gameObject.SetActive(false); //  Hide the button
        BackendManager.Instance.leaderboardText = leaderboardText;
        BackendManager.Instance.FetchLeaderboard();
    }

    public void HideLeaderboard()
    {
        leaderboardPanel.SetActive(false);
        viewLeaderboardButton.gameObject.SetActive(true);
    }
}
