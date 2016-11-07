using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using GooglePlayGames.BasicApi;
public class JoinnCreateEvents : MonoBehaviour {
	
	// Use this for initialization
	public void Start () {
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
			// registers a callback to handle game invitations.
			.Build();
		PlayGamesPlatform.InitializeInstance(config);
		
		// recommended for debugging:
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();
	}
	
	// Update is called once per frame
	public void Update () {
		
	}
}
