using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
public class Events : MonoBehaviour {
    public GameObject transparent;// Transparent medium for loading sign
    void Start()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }
    public void Online()
    {
        const int MinOpponents = 1;
        const int MaxOpponents = 7;
        const int Variant = 0;  // default
        PlayGamesPlatform.Instance.TurnBased.CreateWithInvitationScreen(MinOpponents, MaxOpponents,
            Variant, OnMatchStarted);
    }
    void OnMatchStarted(bool success, TurnBasedMatch match)
    {
        if (success)
        {
            Debug.Log("YES");
            // go to the game screen and play!
        }
        else
        {
            Debug.Log("NO");
            // show error message
        }
    }
    void SceneLoading()
    {
        // Start the loading Process
        transparent.SetActive(true);
    }
    public void Back()
    {
        SceneLoading(); // Calls the SceneLoading Screen
        Application.LoadLevelAsync(0); // Load to the homepage
    }
}
