using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GooglePlayServicesController : MonoBehaviour
{
    private void Awake()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        .RequestIdToken()
        .Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();

        SignIn();
    }

    private void Start()
    {
        Social.ReportProgress("CgkIrMXHw5UDEAIQAg", 0.0f, (bool success) => {
        });
        Social.ReportProgress("CgkIrMXHw5UDEAIQAg", 100.0f, (bool success) => {
        });
    }

    private void SignIn()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            Debug.Log("Loggin is: " + success);
        });
    }
}
