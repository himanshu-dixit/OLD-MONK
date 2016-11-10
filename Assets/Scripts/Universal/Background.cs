using UnityEngine;
using System.Collections;
public class Background : MonoBehaviour {
    public AudioClip music;
    private AudioSource Audio;
    private bool canPlay;
    void Start()
    {
        Audio = GetComponent<AudioSource>();
        canPlay = true;
    }
	void Update () {
        //If there is no previous record of music bool set it to true
        if (!PlayerPrefs.HasKey("music"))
        {
            PlayerPrefs.SetInt("music", 1);
        }
        // bool to tell if music should be played or not. Make it default to false
        bool playMusic = false;
        if (PlayerPrefs.GetInt("music") == 1)
        {
            playMusic = true;
        }
        if (playMusic)
        {
            // If script can play music,music is there and it is not triggering second time then
            if (music && canPlay && !Audio.isPlaying)
            {
                //Set the Audio Clip and Play it
                Audio.clip = music;
                Audio.Play();
                //Set this to false so that the upper condition could not be execute second time unless The slider is switched
                canPlay = false;
            }
        }
        else
        {
            //Check not to stop the audio again and again
            if (!canPlay)
            {
                //Stop the audio
                Audio.Stop();
            }
            //Set this to false so that the upper condition could not be execute second time unless The slider is switched
            canPlay = true;
        }
	}
}
