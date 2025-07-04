using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool isGameOver = false;

    [Header("UI")]
    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;
    public GameObject newHighScoreText;
    public Button restartButton;

    private int serverHighScore = -1; // fetched from server

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        string wallet = PlayerPrefs.GetString("WalletAddress", "0xNoWallet");
        BackendManager.Instance.FetchUserHighScore(wallet, (fetchedScore) =>
        {
            serverHighScore = fetchedScore;
            //    Debug.Log("Fetched server high score: " + serverHighScore);
        });
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0;

        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        int finalScore = scoreManager.GetScore();

        // ✅ Update local PlayerPrefs high score
        scoreManager.CheckHighScore();

        bool isNewHighScore = finalScore > serverHighScore;

        if (isNewHighScore)
        {
            newHighScoreText.SetActive(true);
        }

        // ✅ Submit to backend (already correct)
        string playerName = PlayerPrefs.GetString("PlayerName", "Guest");
        string wallet = PlayerPrefs.GetString("WalletAddress", "0xNoWallet").Trim().ToLower();
        BackendManager.Instance.SubmitScore(playerName, wallet, finalScore);

        // ✅ Update UI
        finalScoreText.text = "Score: " + finalScore;
        gameOverPanel.SetActive(true);
    }


    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScreen");
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResetHighScore()
    {
        // No longer needed; server-managed
        if (finalScoreText != null)
            finalScoreText.text = "Score: 0";

        if (newHighScoreText != null)
            newHighScoreText.SetActive(false);
    }
}
