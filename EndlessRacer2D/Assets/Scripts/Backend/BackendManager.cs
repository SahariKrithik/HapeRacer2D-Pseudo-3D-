using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class BackendManager : MonoBehaviour
{
    public static BackendManager Instance;

    [Header("UI References")]
    public TMP_Text leaderboardText;

    [Header("Server Settings")]
    public string serverURL = "http://localhost:5000"; // change to hosted URL in production

    private void Awake()
    {
        // Singleton setup: Only one instance survives across scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 🟥 Submit the current score to backend
    public void SubmitScore(string name, string wallet, int score)
    {
        StartCoroutine(SubmitScoreRoutine(name, wallet, score));
    }

    // 🟦 Fetch leaderboard from backend
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
        json = "{\"scores\":" + json + "}"; // Wrap array for JsonUtility

        ScoreListWrapper wrapper = JsonUtility.FromJson<ScoreListWrapper>(json);
        string result = "Leaderboard:\n";

        for (int i = 0; i < wrapper.scores.Length; i++)
        {
            var s = wrapper.scores[i];
            result += $"{i + 1}. {s.name} - {s.score}\n";
        }

        return result;
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
