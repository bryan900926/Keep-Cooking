using UnityEngine;
using TMPro;
using System.Collections;

public class LeaderboardUI : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds0_5 = new WaitForSeconds(0.5f);
    [SerializeField] private TextMeshProUGUI leaderboardText; // Drag a big text box here

    [SerializeField] private TextMeshProUGUI myRankText; // Drag a smaller text box here

    // Call this when you open the High Score Menu
    public void UpdateLeaderboardDisplay()
    {
        LoadingTextAnimation();

        LeaderboardManager.Instance.GetTopScores((members) =>
        {
            StopAllCoroutines();
            // This code runs after the data comes back from the cloud
            leaderboardText.text = ""; // Clear text

            foreach (var member in members)
            {
                leaderboardText.text += member.rank + ". ";
                leaderboardText.text += "id: " + member.player.id + " - "; // Or member.player.name if setup
                leaderboardText.text += member.score + "\n";
            }
        });
        LeaderboardManager.Instance.GetMyRank((rank, score) =>
    {
        if (rank == 0)
        {
            myRankText.text = "You are unranked";
        }
        else
        {
            myRankText.text = $"Your Rank: {rank} ({score} $)";
        }
    });
    }

    private void LoadingTextAnimation()
    {
        StartCoroutine(LoadingTextCoroutine());
    }

    private IEnumerator LoadingTextCoroutine()
    {
        while (true)
        {
            leaderboardText.text = "Loading";
            yield return _waitForSeconds0_5;
            leaderboardText.text = "Loading.";
            yield return _waitForSeconds0_5;
            leaderboardText.text = "Loading..";
            yield return _waitForSeconds0_5;
            leaderboardText.text = "Loading...";
            yield return _waitForSeconds0_5;
        }
    }
}
