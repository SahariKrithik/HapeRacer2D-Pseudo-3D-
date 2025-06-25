using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class BackendManager : MonoBehaviour
{
    public static BackendManager Instance;

    [Header("UI References")]
    public TMP_Text leaderboardText;
    public Button viewLeaderboardButton;
    public Button closeLeaderboardButton;
    public LeaderboardUI leaderboardUI;

    [Header("Server Settings")]
    public string serverURL = "https://endlessracer2d.onrender.com/";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        serverURL = "https://endlessracer2d.onrender.com";
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        TryRebindLeaderboardButtons();
    }

    private void Start()
    {
        TryRebindLeaderboardButtons();
    }

    private void TryRebindLeaderboardButtons()
    {
        if (viewLeaderboardButton != null && leaderboardUI != null)
        {
            viewLeaderboardButton.onClick.RemoveAllListeners();
            viewLeaderboardButton.onClick.AddListener(() =>
            {
                Debug.Log("Leaderboard Button Clicked (rebinding)");
                leaderboardText = leaderboardUI.leaderboardText;
                leaderboardUI.ShowLeaderboard();
            });
        }

        if (closeLeaderboardButton != null && leaderboardUI != null)
        {
            closeLeaderboardButton.onClick.RemoveAllListeners();
            closeLeaderboardButton.onClick.AddListener(() =>
            {
                Debug.Log("Close Leaderboard Button Clicked (rebinding)");
                leaderboardUI.HideLeaderboard();
            });
        }
    }

    public void RebindLeaderboard(Button openButton, LeaderboardUI ui, TMP_Text text, Button closeButton)
    {
        viewLeaderboardButton = openButton;
        leaderboardUI = ui;
        leaderboardText = text;
        closeLeaderboardButton = closeButton;
        TryRebindLeaderboardButtons();
    }

    public void SubmitScore(string name, string wallet, int score)
    {
        StartCoroutine(SubmitScoreRoutine(name, wallet, score));
    }

    public void FetchLeaderboard()
    {
        StartCoroutine(GetLeaderboardRoutine());
    }

    IEnumerator SubmitScoreRoutine(string name, string wallet, int score)
    {
        string json = JsonUtility.ToJson(new ScoreData(name, wallet, score));

        using UnityWebRequest request = new UnityWebRequest(serverURL + "/submit-score", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log(" Score submitted: " + request.downloadHandler.text);
        else
            Debug.LogError(" Submit failed: " + request.error);
    }

    IEnumerator GetLeaderboardRoutine()
    {
        using UnityWebRequest request = UnityWebRequest.Get(serverURL + "/leaderboard");
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log(" Leaderboard received: " + json);
            leaderboardText.text = FormatLeaderboard(json);
        }
        else
        {
            Debug.LogError(" Failed to fetch leaderboard: " + request.error);
        }
    }

    string FormatLeaderboard(string json)
    {
        json = "{\"scores\":" + json + "}";
        ScoreListWrapper wrapper = JsonUtility.FromJson<ScoreListWrapper>(json);
        string result = "Leaderboard:\n";

        for (int i = 0; i < wrapper.scores.Length; i++)
        {
            var s = wrapper.scores[i];
            result += $"{i + 1}. {s.name} - {s.score}\n";
        }

        return result;
    }
    public void FetchUserHighScore(string wallet, System.Action<int> onResult)
    {
        StartCoroutine(FetchUserHighScoreRoutine(wallet, onResult));
    }

    IEnumerator FetchUserHighScoreRoutine(string wallet, System.Action<int> onResult)
    {
        using UnityWebRequest request = UnityWebRequest.Get(serverURL + "/user-high-score?wallet=" + wallet);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            if (int.TryParse(request.downloadHandler.text, out int highScore))
            {
                onResult?.Invoke(highScore);
            }
            else
            {
                Debug.LogWarning("Failed to parse high score from server.");
                onResult?.Invoke(0);
            }
        }
        else
        {
            Debug.LogError("Failed to fetch user high score: " + request.error);
            onResult?.Invoke(0);
        }
    }


    [System.Serializable]
    public class ScoreData
    {
        public string name;
        public string wallet;
        public int score;

        public ScoreData(string name, string wallet, int score)
        {
            this.name = name;
            this.wallet = wallet;
            this.score = score;
        }
    }

    [System.Serializable]
    public class ScoreListWrapper
    {
        public ScoreData[] scores;
    }
}
