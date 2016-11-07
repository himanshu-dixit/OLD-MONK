using UnityEngine;
using System.Collections;

public class Handler : MonoBehaviour {
    public GameObject transparent;// Transparent medium for loading sign
    void SceneLoading()
    {
        // Start the loading Process
        transparent.SetActive(true);
    }
    //For Play Button
    public void Play()
    {
        SceneLoading(); // Calls the sceneLoading screen
        Application.LoadLevelAsync(2);
    }
    //For settings Button
    public void Settings()
    {
        SceneLoading(); // Calls the sceneLoading screen
        Application.LoadLevelAsync(1);
    }
	// When the scene starts
	public void Start(){

	}
    // For Facebook Button
    public void Facebook()
    {
        //Application.LoadLevelAsync("");
    }
    //In case user want to logout from google play
    public void Logout()
    {
        // The function triggered when logout is cliocked
    }
    //For Arena Button
    public void Arena()
    {
        //SceneLoading(); Calls the sceneLoading screen
       // Application.LoadLevelAsync("");
    }
    //For Upgrade Button
    public void Upgrade()
    {
        //SceneLoading();  Calls the sceneLoading screen
    }
}
