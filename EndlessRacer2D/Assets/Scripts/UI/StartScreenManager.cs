using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class StartScreenManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField nameInput;
    public TMP_Text walletDisplayText;
    public TMP_Text leaderboardText;
    public Button leaderboardButton;
    public LeaderboardUI leaderboardUI;
    public Button closeLeaderboardButton;

    void Start()
    {
        if (BackendManager.Instance != null)
        {
            BackendManager.Instance.RebindLeaderboard(
                leaderboardButton,
                leaderboardUI,
                leaderboardText,
                closeLeaderboardButton
            );
        }
    }

    public void StartGame()
    {
        string playerName = nameInput.text.Trim();
        string walletAddress = walletDisplayText.text.Trim();

        if (string.IsNullOrEmpty(playerName) ||
            string.IsNullOrEmpty(walletAddress) ||
            walletAddress == "Your Wallet Address")
        {
            Debug.LogWarning("Please enter your name and connect your wallet before starting.");
            return;
        }

        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.SetString("WalletAddress", walletAddress);
        PlayerPrefs.Save();

        Debug.Log($"Starting game for {playerName} with wallet {walletAddress}");

        SceneManager.LoadScene("MainGame");
    }

    public void ReceiveInputFromOverlay(string value)
    {
        nameInput.text = value;
    }
}
