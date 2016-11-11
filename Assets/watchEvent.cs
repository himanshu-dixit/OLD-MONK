using UnityEngine;
using System.Collections;

public class watchEvent : MonoBehaviour {
	public GameObject background;
	public GameObject username;
	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetString ("username")!="") {
	
		} else {
			background.SetActive(true);
			username.SetActive (true);
		}
	}
	// Update is called once per frame
	void Update () {

	}
}
