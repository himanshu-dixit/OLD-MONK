using UnityEngine;
using System.Collections;
public class FriendsEvents : MonoBehaviour {
	public GameObject transparent;
	// Use this for initialization
	public void Start () {

	}
	// Update is called once per frame
	public void Update () {
	
	}
	public void SceneLoading()
	{
		// Start the loading Process
		transparent.SetActive(true);
	}
	public void Friends(){
		SceneLoading(); // Calls the SceneLoading Screen
		Application.LoadLevelAsync(3); // Load to the homepage
	}
}
