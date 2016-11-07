using UnityEngine;
using System.Collections;

using NativeAlert;

public class NativeAlertExample : MonoBehaviour {

	void OnEnable()
	{
		NativeAlertListener.onFinish += OnAlertFinish;
		NativeAlertListener.onCancel += OnAlertCancel;
	}

	void OnDisable()
	{
		NativeAlertListener.onFinish -= OnAlertFinish;
		NativeAlertListener.onCancel -= OnAlertCancel;
	}

	void OnAlertFinish(string clickedBtn)
	{
		if (clickedBtn == "Yes") {
			GameObject.Find("Cube").GetComponent<Renderer>().material.color = new Color(Random.value,Random.value,Random.value);
			Debug.Log("Color changed");
		} else if (clickedBtn == "No") {
			Debug.Log("Color not changed");
		}
	}

	void OnAlertCancel()
	{
		log += "\n Cancelled";
	}

	string log = "";
	void OnGUI()
	{
		GUILayout.Label (log);
		Rect rect = new Rect (Screen.width/2 - 75,Screen.height/2-15,150,30);
		if (GUI.Button (rect,"Change Color")) {
#if UNITY_ANDROID
			AndroidNativeAlert.ShowAlert("Confirmation","You want to change color?", "Yes", "No");
#elif UNITY_IPHONE
			IOSNativeAlert.ShowAlert("Confirmation","You want to change color?", "Yes", "No");
#endif
		}
	}
}
