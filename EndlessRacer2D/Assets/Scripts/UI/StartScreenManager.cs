using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartScreenManager : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_Text walletDisplayText;
    public TMP_Text leaderboardText;

    void Start()
    {
        // Rebind the leaderboardText reference
        if (BackendManager.Instance != null)
        {
            BackendManager.Instance.leaderboardText = leaderboardText;
        }
    }
    public void StartGame()
    {
        string playerName = nameInput.text.Trim();
        string walletAddress = walletDisplayText.text.Trim();

        if (string.IsNullOrEmpty(playerName) || walletAddress == "Your Wallet Address" || string.IsNullOrEmpty(walletAddress))
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
