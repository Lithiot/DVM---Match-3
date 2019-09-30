using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GooglePlayServicesController : MonoBehaviour
{
    public Text signedText;


    private void Awake()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        .RequestEmail()
        .RequestIdToken()
        .Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
    }

    private void Start()
    {
        TriggerArchievement(GPGSIds.achievement_welcome);
    }

    public void SignIn()
    {
        signedText.text = "Trying";

        Debug.Log("trying to connect");
        Social.localUser.Authenticate((bool success) =>
        {
            signedText.text = success.ToString();
        });
    }

    public static void TriggerArchievement(string id)
    {
        Social.ReportProgress(id, 100.0f, (bool success) => {});
    }

    public static void ShowArchievementsUI()
    {
        Social.ShowAchievementsUI();
    }

    public static void AddScoreToBoard(string id, long points)
    {
        Social.ReportScore(points, id, (bool success) => {});
    }

    public static void ShowHighScoreUI()
    {
        Social.ShowLeaderboardUI();
    }
}
