using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EventsWatcher : MonoBehaviour {
    public Slider music_slide;
    public GameObject transparent;// Transparent medium for loading sign
    void SceneLoading()
    {
        // Start the loading Process
        transparent.SetActive(true);
    }
    void Awake()
    {
        if(music_slide)
        music_slide.value = (float) PlayerPrefs.GetInt("music");
    }
    public void Music(Slider component)
    {
        //Debug.Log("Triggered");
        // Lets store the float value to text so that we can parse int out of it
        string value = component.value + "";
        //Set the value as music bool value
        PlayerPrefs.SetInt("music",int.Parse(value));
    }
    public void Sound(Slider component)
    {
        //Debug.Log("Triggered");
        // Lets store the float value to text so that we can parse int out of it
        string value = component.value + "";
        //Set the value as sound bool value
        PlayerPrefs.SetInt("sound", int.Parse(value));
    }
    public void About()
    {
        // When clicked on About just Open up a webpage
        Application.OpenURL("http://gamestream.com/aboutus.html");
    }
    public void Support()
    {
        //When clicked on Support just Open up a webpage
        Application.OpenURL("http://forums.gamestream.com");
    }
    public void Back()
    {
        // When Back button is pressed just load to the homepage scene
        Application.LoadLevelAsync(0);
        SceneLoading();
    }
}
