using UnityEngine;

namespace NativeAlert
{

	public class AndroidNativeAlert
	{
		#if UNITY_ANDROID
		static AndroidJavaClass _plugin;

		static AndroidNativeAlert()
		{
			_plugin = new AndroidJavaClass("com.astricstore.nativealert.NativeAlert");
		}

		public static void ShowAlert(string title, string message)
		{
			_plugin.CallStatic("ShowAlert",title,message,"","","");
		}

		public static void ShowAlert(string title, string message,string btn1)
		{
			_plugin.CallStatic("ShowAlert",title,message,btn1,"","");
		}

		public static void ShowAlert(string title, string message,string btn1,string btn2)
		{
			_plugin.CallStatic("ShowAlert",title,message,btn1,btn2,"");
		}

		public static void ShowAlert(string title, string message,string btn1, string btn2, string btn3)
		{
			_plugin.CallStatic("ShowAlert",title,message,btn1,btn2,btn3);
		}
		#endif
	}
}


