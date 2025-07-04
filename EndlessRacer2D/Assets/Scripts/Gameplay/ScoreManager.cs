using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    private int score = 0;
    private int highScore = 0;
    private float scoreTimer = 0f;

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateUI();
    }

    void Update()
    {
        scoreTimer += Time.deltaTime * 10f; // accumulate fractional points

        if (scoreTimer >= 1f)
        {
            int pointsToAdd = Mathf.FloorToInt(scoreTimer);
            score += pointsToAdd;
            scoreTimer -= pointsToAdd;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        highScoreText.text = "High: " + highScore;
    }


    public void AddPoints(int amount)
    {
        score += amount;
        UpdateUI();
    }

    public void CheckHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();

            Debug.Log("New high score achieved! Submitting to backend...");

            // Get saved name and wallet
            string playerName = PlayerPrefs.GetString("PlayerName", "Player");
            string wallet = PlayerPrefs.GetString("WalletAddress", "").Trim().ToLower();

            // Submit to backend
            if (!string.IsNullOrEmpty(wallet))
            {
                BackendManager.Instance.SubmitScore(playerName, wallet, highScore);
            }
            else
            {
                Debug.LogWarning("Wallet address missing. Cannot submit score.");
            }
        }
    }


    public int GetScore() => score;
}
