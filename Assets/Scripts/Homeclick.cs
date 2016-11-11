using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Homeclick : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    public void playOffline()
    {
        Debug.Log("OK");
        Application.LoadLevel("playArea");
    }
	// Update is called once per frame
	void Update () {

	}
	public void enterPress(GameObject test){
		string value = test.GetComponent<Text>().text;
		string url = "http://localhost/Game/makeUsername.php";
		WWWForm form = new WWWForm();
		form.AddField("username", value);
		WWW www = new WWW(url, form);
		StartCoroutine(RequestUsername(www));
	}
	IEnumerator RequestUsername(WWW www)
	{
		yield return www;
		if (www.data!="") {
			PlayerPrefs.SetString ("username",www.data);
			Debug.Log (www.data);
			Application.LoadLevel (Application.loadedLevelName); // Lets reload the Scene
		}
	}    
}
