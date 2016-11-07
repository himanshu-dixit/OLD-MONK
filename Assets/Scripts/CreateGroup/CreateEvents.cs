using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using GooglePlayGames;
using System.Collections.Generic;
using System.IO;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Nearby;
using System;
public class CreateEvents : MonoBehaviour,IDiscoveryListener,IMessageListener {
	public GameObject transparent;
	public GameObject GroupName;
	public GameObject passwordSe;
	public GameObject GroupContent;
	public GameObject Grprefab;
	public GameObject createGameObject;
	public GameObject joinGameObject;
	public GameObject inputF;
	public GameObject darkBack;
	public GameObject alertBox;
	public GameObject connectingScene;
	public GameObject myNameinConn;
	public GameObject playArea;
	public GameObject counter;
	List<Dictionary<string, string>> player = new List<Dictionary<string, string>>();
	public int seconds = 20;
	public void Timer(){
		if (seconds == 0) {
			CancelInvoke ("Timer");
			playArea.SetActive(true); // Loaded the scne
			connectingScene.SetActive(false);
		} else {
			seconds--;
		}
		
	}
	public void StartTheTimer(){
		if (player.Count == 5) {
			// Automatically start the playing scene when player count reaches 5
			seconds = 20;
			CancelInvoke ("Timer");
			playArea.SetActive(true); // Loaded the scne
			connectingScene.SetActive(false);
		} else {
			seconds = 20;
			CancelInvoke ("Timer");
			InvokeRepeating("Timer",1,1);
		}
	
	}
	public void Start(){

		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
			// registers a callback to handle game invitations.
			.Build();
		PlayGamesPlatform.InitializeInstance(config);
		
		// recommended for debugging:
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();
		PlayGamesPlatform.InitializeNearby((client) =>
		                                   {
			Debug.Log("Nearby connections initialized");
		});
		Dictionary<string, string> dic=  new Dictionary<string, string> {
			{"EndPoint", PlayGamesPlatform.Nearby.LocalEndpointId()},
			{ "Name", PlayerPrefs.GetString("myName") }, 
			{ "Avatar", PlayerPrefs.GetString("myAvatar") }
		};
		player.Add(dic);
	}
	public byte[] getplayerDictionary(){
		MemoryStream stream = new MemoryStream ();
		BinaryWriter writer = new BinaryWriter (stream);
		const int header = 4382;
		writer.Write (header);
		writer.Write (player.Count);
		for (int i =0; i<player.Count; i++) {
			writer.Write ((byte)player[i]["EndPoint"].Length);
			writer.Write (player[i]["EndPoint"].ToCharArray());
			writer.Write ((byte)player[i]["Name"].Length);
			writer.Write (player[i]["Name"].ToCharArray());
			writer.Write ((byte)player[i]["Avatar"].Length);
			writer.Write (player[i]["Avatar"].ToCharArray());
		}
		writer.Close ();
		byte[] data = stream.GetBuffer ();
		stream.Close ();
		return data;
	}
    public void PasswordSelect(GameObject first)
    {
        GameObject current = EventSystem.current.currentSelectedGameObject; // Store the clicked gameObject
        Sprite main = current.GetComponent<Image>().sprite; // Store the sprite of the current gameobject
        // Check if the other gameobject is slected
        if (first.GetComponent<Image>().sprite.name =="Sprites_1")
        {
            current.gameObject.GetComponent<Image>().sprite = first.GetComponent<Image>().sprite; // Make the current gameobject selected
            first.GetComponent<Image>().sprite = main; // Set the other gameobject to the simple sprite
            PlayerPrefs.SetString("selected", current.gameObject.name); // Set what is selected
            PlayerPrefs.Save(); // Save the PlayerPrefs
        }
		PlayerPrefs.SetString ("GroupPasswordBool", current.gameObject.name);
		PlayerPrefs.Save ();
    }
	public void CreateScene(){
		joinGameObject.gameObject.SetActive (false);
		createGameObject.gameObject.SetActive (true);
	}
	public void CreateGroup(){
		string boolPassword = PlayerPrefs.GetString ("GroupPasswordBool");
		string Name = GroupName.GetComponent<InputField> ().text;
		string Password = passwordSe.GetComponent<InputField> ().text;
		if (boolPassword == "Yes") {
			//Handle this Here
		} else if (boolPassword == "No") {
			// Handle this Here
		} else {
			// Handle this here
		}
		List<string> appIdentifiers = new List<string>();
		appIdentifiers.Add("com.gamestream.oldmonk");
		PlayGamesPlatform.Nearby.StartAdvertising(
			Name,  // User-friendly name
			appIdentifiers,  // App bundle Id for this game
			TimeSpan.FromSeconds(0),// 0 = advertise forever
			(AdvertisingResult result) =>{
			Debug.Log("OnAdvertisingResult: " + result);
		},
		(ConnectionRequest request) =>{
			byte[] data = request.Payload;
			string PlayerName = request.RemoteEndpoint.Name;
			MemoryStream stream = new MemoryStream(data);
			BinaryReader reader = new BinaryReader(stream);
			int AvatarLength = reader.ReadByte();
			string Avatar = new string(reader.ReadChars(AvatarLength));
			int PasswordLen = reader.ReadByte();
			string givenPassword = new string(reader.ReadChars (PasswordLen));
			reader.Close ();
			stream.Close();
			if(givenPassword==Password){
				Dictionary<string, string> dic=  new Dictionary<string, string> {
					{"EndPoint", request.RemoteEndpoint.EndpointId},
					{ "Name", PlayerName }, 
					{ "Avatar", Avatar }
				};
				player.Add(dic);
				MemoryStream streamer = new MemoryStream();
				BinaryWriter write = new BinaryWriter(stream);
				const int header = 5000;
				write.Write(header);
				
				write.Close ();
				byte[] responseData = stream.GetBuffer();
				PlayGamesPlatform.Nearby.AcceptConnectionRequest(
					request.RemoteEndpoint.EndpointId,
					(byte[])responseData,
					(IMessageListener)this);
				List<string> endpointIds = new List<string>();
				int count = player.Count;
			
				for(int s = 0; s<count;s++){
					endpointIds.Add(player[s]["EndPoint"]);
				}
				byte[] payload = getplayerDictionary();
				PlayGamesPlatform.Nearby.SendUnreliable(endpointIds, payload); // Send the data
			}
			else{
				PlayGamesPlatform.Nearby.RejectConnectionRequest(
					request.RemoteEndpoint.EndpointId);
			}
			Debug.Log("Received connection request: " +
			          request.RemoteEndpoint.DeviceId + " " +
			          request.RemoteEndpoint.EndpointId + " " +
			          request.RemoteEndpoint.Name);
		}
		);

		connectingScene.SetActive(true);

		createGameObject.SetActive (false);
		myNameinConn.GetComponent<Text> ().text = PlayerPrefs.GetString ("myName");
	}

	public void SceneLoading()
	{
		// Start the loading Process
		transparent.SetActive(true);
	}
	
	public void JoinGroup(){
		createGameObject.gameObject.SetActive (false);
		joinGameObject.gameObject.SetActive (true);
		PlayGamesPlatform.Nearby.StartDiscovery(
			PlayGamesPlatform.Nearby.GetServiceId(),
			TimeSpan.FromSeconds(0),
			this);
	}
	public void CancelAlert(){
		darkBack.SetActive (false);
		alertBox.SetActive (false);
	}
	public void ShowConnectingScene(){

	}
	public void sendPassword(GameObject passwordField){
		string password = passwordField.GetComponent<InputField> ().text;
		Debug.Log (password);
		if (passwordField.GetComponent<InputField> ().interactable == false) {
			password = "";
		}
		string remoteEndPointId = PlayerPrefs.GetString ("ChoosenGroup");
		MemoryStream stream = new MemoryStream ();
		BinaryWriter writer = new BinaryWriter (stream);
		writer.Write ((byte)PlayerPrefs.GetString("myAvatar").Length);
		writer.Write (PlayerPrefs.GetString("myAvatar").ToCharArray());
		writer.Write ((byte)password.Length);
		writer.Write (password.ToCharArray());
		writer.Close ();
		byte[] playerData = stream.GetBuffer ();
		stream.Close ();
		PlayGamesPlatform.Nearby.SendConnectionRequest(
			PlayerPrefs.GetString("myName"),  // the user-friendly name
			remoteEndPointId,  // the discovered endpoint  
			playerData, // byte[] of data
			(response) => {
			Debug.Log (response.ResponseStatus.ToString());
			byte[] data = response.Payload;
			MemoryStream newStream = new MemoryStream(data);
			BinaryReader reader = new BinaryReader(newStream);
			int headerf = reader.ReadInt32();
			reader.Close ();
			stream.Close ();
			if(headerf==5000){
				connectingScene.SetActive(true);
				myNameinConn.GetComponent<Text> ().text = PlayerPrefs.GetString ("myName");

				joinGameObject.SetActive(false);
			}
				// Well i am done now

		},
		(IMessageListener)this);
	}
	public void JoinClick(){
		darkBack.SetActive (true);
		alertBox.SetActive (true);
	}
	public void OnGroupClick(string remoteEndPointId){
		PlayerPrefs.SetString ("ChoosenGroup",remoteEndPointId);
		PlayerPrefs.Save ();

	}

	#region IDiscoveryListener implementation
	public void OnEndpointFound(EndpointDetails discoveredEndpoint)
	{
		Debug.Log ("ALLOW ME");
		GameObject newGroup = Instantiate (Grprefab);
		newGroup.transform.SetParent (GroupContent.transform);
		Vector3 position = newGroup.transform.localPosition;
		position.z = 1;
		newGroup.transform.localPosition = position;
		newGroup.transform.localScale = new Vector3 (1, 1, 1); // Let it be default
		newGroup.transform.GetChild (0).GetComponent<Text> ().text = discoveredEndpoint.Name; // Here is the group
		newGroup.gameObject.name = discoveredEndpoint.EndpointId;
		Button newGroupButton = newGroup.GetComponent<Button>();
		newGroupButton.onClick.AddListener(() => OnGroupClick(discoveredEndpoint.EndpointId));
	}
	public void OnEndpointLost(string lostEndpointId)
	{
		Debug.Log("Endpoint lost: " + lostEndpointId);
		Destroy(GameObject.Find (lostEndpointId));
		Destroy (GameObject.Find  (lostEndpointId)); // Lets destroy the group prefab of this endpoint id
	}
	#endregion
	#region IMessageListener implementation
	
	/// <summary>
	/// Raises the message received event.
	/// </summary>
	/// <param name="remoteEndpointId">Remote endpoint identifier.</param>
	/// <param name="data">Data payload of the message.</param>
	/// <param name="isReliableMessage">If set to <c>true</c> is reliable message.</param>
	public void OnMessageReceived(string remoteEndpointId, byte[] data, bool isReliableMessage)
	{
		MemoryStream stream = new MemoryStream (data);
		BinaryReader reader = new BinaryReader (stream);
		int header = reader.ReadInt32 ();
		reader.Close ();
		stream.Close ();
		if (header == 5000) {
			// Finally I am accepted lets do something for this
		}
		if (header == 4382) {
			MemoryStream stream1 = new MemoryStream(data);
			BinaryReader reader1 = new BinaryReader(stream1);
			int headerf = reader1.ReadInt32 ();
			int playerCount = reader1.ReadInt32 ();
			player = new List<Dictionary<string,string>>();
			for(int s =0;s<playerCount;s++){
				int EndPointLength = reader1.ReadByte();
				string EndPointId = new string(reader1.ReadChars (EndPointLength));
				int NameLength = reader1.ReadByte();
				string NamePlayer = new string(reader1.ReadChars(NameLength));
				int AvatarLength  = reader1.ReadByte();
				string Avatar = new string(reader1.ReadChars(AvatarLength));
				Dictionary<string,string> dicNew = new Dictionary<string,string>{
					{"EndPoint",EndPointId},
					{"Name", NamePlayer },
					{"Avatar",Avatar}
				};
				player.Add (dicNew);
			}
			reader1.Close ();
			stream1.Close ();
			StartTheTimer (); // A new player is added start the countdown again
			// We got the list
		}
		Debug.Log("RECEIVED Message from " + remoteEndpointId);
		
	}
	/// <summary>
	/// Raises the remote endpoint disconnected event.
	/// </summary>
	/// <param name="remoteEndpointId">Remote endpoint identifier.</param>
	public void OnRemoteEndpointDisconnected(string remoteEndpointId)
	{

		
	}
	#endregion
	public void ToggleEvent(Toggle comp){
		bool value = comp.isOn;
		if (!value) {
			inputF.GetComponent<InputField>().interactable = true;
		} else {
			inputF.GetComponent<InputField>().interactable = false;
		}
	}

	public void EditAvatar(){

	}


}
