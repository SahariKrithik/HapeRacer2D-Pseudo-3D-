using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class StartScreenManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField nameInput;
    public TMP_InputField manualWalletInput;
    public TMP_Text walletDisplayText;
    public TMP_Text leaderboardText;
    public Button leaderboardButton;
    public LeaderboardUI leaderboardUI;
    public Button closeLeaderboardButton;

    void Start()
    {
        // Auto-fill from last session
        string savedName = PlayerPrefs.GetString("PlayerName", "");
        string savedWallet = PlayerPrefs.GetString("WalletAddress", "");

        if (!string.IsNullOrEmpty(savedName))
        {
            nameInput.text = savedName;
        }

        if (!string.IsNullOrEmpty(savedWallet))
        {
            manualWalletInput.text = savedWallet;
            walletDisplayText.text = savedWallet;
        }

        // Rebind leaderboard
        if (BackendManager.Instance != null)
        {
            BackendManager.Instance.RebindLeaderboard(
                leaderboardButton,
                leaderboardUI,
                leaderboardText,
                closeLeaderboardButton
            );
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        Application.ExternalEval("enablePasteClipboard()");
#endif
    }

    public void StartGame()
    {
        string playerName = nameInput.text.Trim();
        string walletAddress = manualWalletInput.text.Trim();

        if (string.IsNullOrEmpty(playerName) || string.IsNullOrEmpty(walletAddress))
        {
            Debug.LogWarning("Please enter your name and wallet address before starting.");
            return;
        }

        walletDisplayText.text = walletAddress;

        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.SetString("WalletAddress", walletAddress);
        PlayerPrefs.Save();

        Debug.Log($"Starting game for {playerName} with wallet {walletAddress}");

        SceneManager.LoadScene("MainGame");
    }

    // Called from paste button (right-click or UI)
    public void PasteFromClipboard()
    {
        manualWalletInput.text = GUIUtility.systemCopyBuffer;
    }

    // Called from JavaScript (Clipboard API)
    public void SetWalletInputFromJS(string value)
    {
        Debug.Log("Received clipboard text from JS: " + value);
        manualWalletInput.text = value;
    }
}
