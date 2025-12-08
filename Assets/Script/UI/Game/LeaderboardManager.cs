using UnityEngine;
using LootLocker.Requests;
using System.Collections;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    // The "Key" you typed in the LootLocker dashboard
    private const string leaderboardKey = "global_highscore";

    // Hidden variable to track if we are logged in
    bool isLoggedIn = false;

    private LeaderboardUI leaderboardUI;

    void Awake()
    {
        // Singleton Pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        leaderboardUI = GetComponent<LeaderboardUI>();
    }

    void Start()
    {
        // Automatically login as "Guest" when the game starts
        StartCoroutine(LoginRoutine());
    }

    IEnumerator LoginRoutine()
    {
        bool done = false;

        // 1. Start Guest Session
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Player was logged in");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                isLoggedIn = true;
            }
            else
            {
                Debug.Log("Could not start session");
            }
            done = true;
        });

        yield return new WaitUntil(() => done == true);
    }

    // CALL THIS when the player finishes the game
    public void SubmitScore(float scoreToUpload)
    {
        if (!isLoggedIn)
        {
            Debug.Log("Cannot upload score: Not logged in.");
            return;
        }

        string playerID = PlayerPrefs.GetString("PlayerID");

        LootLockerSDKManager.SubmitScore(playerID, (int) scoreToUpload, leaderboardKey, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Score Uploaded Successfully: " + scoreToUpload);
                leaderboardUI.UpdateLeaderboardDisplay();
            }
            else
            {
                Debug.Log("Failed to upload score: " + response.errorData);
            }
        });
    }

    public void GetTopScores(System.Action<LootLockerLeaderboardMember[]> onComplete)
    {
        if (!isLoggedIn) return;

        int count = 15; // How many scores to get

        LootLockerSDKManager.GetScoreList(leaderboardKey, count, 0, (response) =>
        {
            if (response.success)
            {
                onComplete?.Invoke(response.items);
            }
            else
            {
                Debug.LogWarning("Failed to fetch leaderboard");
            }
        });
    }

    // Add this inside your LeaderboardManager class
    public void GetMyRank(System.Action<int, int> onComplete)
    {
        if (!isLoggedIn) return;

        string playerID = PlayerPrefs.GetString("PlayerID");

        LootLockerSDKManager.GetMemberRank(leaderboardKey, playerID, (response) =>
        {
            if (response.success)
            {
                Debug.Log($"Found player rank: {response.rank}");
                onComplete?.Invoke(response.rank, response.score);
            }
            else
            {
                Debug.Log("Failed to get player rank: " + response.errorData);
                onComplete?.Invoke(0, 0);
            }
        });
    }
}