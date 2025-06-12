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

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0;

        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        int finalScore = scoreManager.GetScore();
        int prevHigh = PlayerPrefs.GetInt("HighScore", 0);

        // Save high score if beaten
        if (finalScore > prevHigh)
        {
            PlayerPrefs.SetInt("HighScore", finalScore);
            PlayerPrefs.Save();
            newHighScoreText.SetActive(true);
        }

        // Update UI
        finalScoreText.text = "Score: " + finalScore;
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.Save();
        Debug.Log("High Score reset to 0");

        if (finalScoreText != null)
            finalScoreText.text = "Score: 0";

        if (newHighScoreText != null)
            newHighScoreText.SetActive(false);
    }


}
