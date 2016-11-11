using UnityEngine;
using System.Collections;

public class group : MonoBehaviour {

	// Use this for initialization
	void Start () {
		InvokeRepeating ("FindGroup", 0.2f, 0.2f);
	}
	void FindGroup(){
		string url = "http://localhost/Game/findGroup.php";
		WWWForm form = new WWWForm();
		string username = PlayerPrefs.GetString ("username");
		form.AddField("username", username);
		WWW www = new WWW(url, form);
		StartCoroutine(RequestGroup(www));
	}
	IEnumerator RequestGroup(WWW www)
	{
		yield return www;
//		Debug.Log (www.data);
		if (www.data!="") {
			StopAllCoroutines();
			PlayerPrefs.SetString ("group",www.data);
	
		}
	}    
	// Update is called once per frame
	void Update () {
	
	}
}
