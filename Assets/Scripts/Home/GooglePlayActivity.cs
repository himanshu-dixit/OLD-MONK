using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi.Multiplayer;
public class GooglePlayActivity : MonoBehaviour
{
    //Just Before Start
    void Awake()
    {


    }
    public void MatchDelegate(TurnBasedMatch match, bool success)
    {
        if (success)
        {

            // If recived the turn based notifcation while the game was not running
        }
        else
        {

            // If didn't recived
        }
    }
    public void PlayArea()
    {
        //What to do when clicked Play Offline
        Application.LoadLevel("playarea");
    }
    // Use this for initialization
    void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .WithMatchDelegate(MatchDelegate)
                .Build();
        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
        // Make the user sign in if he has not
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log("YES LOGGED IN");
                //Logged in
            }
            else
            {
                Debug.Log("NO LOGGED IN");
                //Not Logged in
            }
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
