using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System;
using NativeAlert;
public class Events : MonoBehaviour, RealTimeMultiplayerListener
{
	public GameObject transparent;// Transparent medium for loading sign
	public GameObject current;
	public GameObject connectingScene;
	public Text timerCountdown;
	public Sprite[] allAvatars;
	public GameObject GameArea;
	public GameObject myAvatarObject;
	public GameObject AvatarDeck;
	public GameObject ChangeAvatarObject;
	public GameObject avatar;
	public int myIndex;
	public GameObject statusText;
	public GameObject cardsContainer;
	public GameObject cardDeck;
	public bool myChance = false;
	public List<string> participant_ids;
	public string SelfParticipantId;
	public List<string> usernames;
	public List<GameObject> allCardIncluded;
	public GameObject throwingCase;
	public GameObject gameArea_myAvatar;
	public Sprite[] divs;
	public string[] allColorsCode;
	public GameObject playersPanel;
	public GameObject otherPlayerPrefab;
	public GameObject emptySPrefab;
	public Sprite deadAvatar;
	public GameObject cardsDeckScript;
	public int TurnsToTake = 1;
	public GameObject futureScene;
	public GameObject layoutFuture;
	public bool isAttackMode = false;
	public int turnForAttack = 1;
	public bool isWhattheDuck = false;
	public Sprite DuckSprite;
	public bool isReverse = false;
	public bool isSteal = false;
	public GameObject animationHand;
	public GameObject TimerObject;
	public GameObject BlackTrans;
	public bool isSmashingMonk = false;
	public GameObject DeadScreen;
	public bool currentlyGotSteal;
	public bool currentlyBlind = false;
	public List<string> universalList = new List<string>();
	public GameObject connectingToInterentScene;
	public GameObject winnerScreen;
	public class OponentData
	{
		public Participant participantObject { get; set; }
		public Sprite avatar { get; set; }
	}
	
	public class AvatarsData
	{
		public Sprite card { get; set; }
		public Sprite CardsDiv { get; set; }
		public string colorCode { get; set; }
	}
	public List<OponentData> AllOpenentList;
	public List<AvatarsData> avatarDataA;
	public GameObject OtherPlayerGroup;
	public int timeSe = 15;
	public void StartTheTimer()
	{
		if (GameObject.Find ("Empty")) {
			DestroyImmediate(GameObject.Find ("Empty"));
		}
		timeSe = 15; // Lets make it to default
		InvokeRepeating("timer", 1, 1F);
	}
	
	public Sprite GetSpriteFromName(string avatarName)
	{
		Sprite CorrectSprite = null;
		for (int k = 0; k < allAvatars.Length; k++)
		{
			if (allAvatars[k].name == avatarName)
			{
				CorrectSprite = allAvatars[k];
				break;
			}
		}
		if (CorrectSprite == null) {
			CorrectSprite = allAvatars[0];
		}
		return CorrectSprite;
	}
	
	public void RecieveCard(string name)
	{
		
		GameObject finalObject = null;
		for (int e = 0; e < allCardIncluded.Count; e++)
		{
			if (name.Contains(allCardIncluded[e].gameObject.name))
			{
				finalObject = allCardIncluded[e];
				break; // Lets break the loop now because we have find out what we needed to find out
			}
		}
		if (finalObject != null)
		{
			
		}
		else
		{
			Debug.Log("ITS NULL");
		}
		
	}
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
	public string mode = "";
	void OnAlertFinish(string clickedBtn)
	{
		
	}
	void OnAlertCancel()
	{
		
	}
	
	public void ShowAlert(String title,String text,String type){
		// This function shows the alert box of the given text and values
		string newTemp = type.ToLower ();
		switch (newTemp) {
		case "yesno":
			// If the dialog is type of Yes and No (sort of)
			mode=newTemp;
			#if UNITY_ANDROID
			AndroidNativeAlert.ShowAlert(title,text, "Yes", "No");
			#elif UNITY_IPHONE
			IOSNativeAlert.ShowAlert(title,text, "Yes", "No");
			#endif
			break;
		case "okcancel":
			// If the dialog is type of Ok and cancel
			mode=newTemp;
			#if UNITY_ANDROID
			AndroidNativeAlert.ShowAlert(title,text, "Ok", "Cancel");
			#elif UNITY_IPHONE
			IOSNativeAlert.ShowAlert(title,text, "Ok", "Cancel");
			#endif
			break;
		case "ok":
			// If the dialogue should be just OK
			mode=newTemp;
			#if UNITY_ANDROID
			AndroidNativeAlert.ShowAlert(title,text);
			#elif UNITY_IPHONE
			IOSNativeAlert.ShowAlert(title,text);
			#endif
			break;
		default:
		case "warning":
			mode=newTemp;
			// If the dialogue should give warning to the user
			#if UNITY_ANDROID
			AndroidNativeAlert.ShowAlert(title,text);
			#elif UNITY_IPHONE
			IOSNativeAlert.ShowAlert(title,text, "Yes", "No");
			#endif
			break;
		}
	}
	public void showError(string errorMessage)
	{
		Debug.Log (errorMessage);
	}
	public void BackFromChooseAvatar()
	{
		ChangeAvatarObject.SetActive(false);
		connectingScene.SetActive(true);
	}
	public void StopMyChance(){
		cardDeck.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		cardsContainer.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		statusText.GetComponent<Text>().text = "Just choose the player to attack";
	}
	public const int Header = 10000;
	public byte[] AvatarToBytes(GameObject avatar)
	{
		string avatarName = avatar.GetComponent<Image>().sprite.name;
		MemoryStream memStream = new MemoryStream();
		BinaryWriter w = new BinaryWriter(memStream);
		w.Write(Header); // For identification
		w.Write((byte)avatarName.Length);
		w.Write(avatarName.ToCharArray());
		w.Close();
		byte[] buf = memStream.GetBuffer();
		memStream.Close();
		return buf;
	}
	public void MyTurnComplete(string username)
	{
		const int type = 100;
		MemoryStream memStream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(memStream);
		writer.Write(type);
		writer.Write((byte)username.Length);
		writer.Write(username.ToCharArray());
		writer.Close();
		byte[] buffer = memStream.GetBuffer();
		memStream.Close();
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, buffer);
	}
	public void MyTurnCompletes(string username, int turns)
	{
		const int type = 101;
		MemoryStream memStream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(memStream);
		writer.Write(type);
		writer.Write((byte)turns);
		writer.Write((byte)username.Length);
		writer.Write(username.ToCharArray());
		writer.Close();
		byte[] buffer = memStream.GetBuffer();
		memStream.Close();
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, buffer);
	}
	public void UnfoldBlind()
	{
		int childCount = cardsContainer.transform.childCount;
		for (int s = 0; s < childCount; s++)
		{
			cardsContainer.transform.GetChild(s).gameObject.GetComponent<Image>().sprite = cardsContainer.transform.GetChild(s).gameObject.GetComponent<Drag>().myOwnSprite;
		}
		if (GameObject.Find ("Empty"))
			DestroyImmediate (GameObject.Find ("Empty"));
	}
	public void StopMyChances(){
		myChance = false;
		cardsContainer.GetComponent<CanvasGroup> ().blocksRaycasts = false; // We are all done
		cardDeck.GetComponent<CanvasGroup> ().blocksRaycasts = false; // Everything is block now
	}
	public void CanceltheTimerInvoke()
	{
		TimerObject.SetActive(false);
		timeSe = 15;
		CancelInvoke("timer");
	}
	/*
	 * Read This
	 */
	public void TurnEvent()
	{
		TurnsToTake--;
		CanceltheTimerInvoke();
		if (TurnsToTake<=0)
		{
			
			if(isSmashingMonk){
				StartTheTimer();
			}
			else{
				myChance = false;
				if (cardsContainer.transform.GetChild(0).gameObject.GetComponent<Image>().sprite == DuckSprite)
				{
					// This means he is in the duck mode.
					UnfoldBlind();
				}
				
				cardsContainer.GetComponent<CanvasGroup>().blocksRaycasts = false;
				cardDeck.GetComponent<CanvasGroup>().blocksRaycasts = false;
				statusText.GetComponent<Text>().text = "ITS not your turn"; // Just modify the status information
				if (isReverse)
				{
					if (myIndex == 0)
					{
						
						MyTurnComplete(usernames[usernames.Count - 1]);
					}
					else
					{
						MyTurnComplete(usernames[myIndex - 1]);
					}
				}
				else
				{
					if ((usernames.Count - 1) == myIndex)
					{
						
						MyTurnComplete(usernames[0]);
					}
					else
					{
						MyTurnComplete(usernames[myIndex + 1]);
					}
				}
			}
		}
		else
		{
			StartTheTimer();
		}
		
	}
	public void ReverseTurn()
	{
		if (isReverse)
		{
			isReverse = false;
		}
		else
		{
			isReverse = true;
		}
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		const int header = 149;
		writer.Write(header);
		writer.Write(isReverse);
		writer.Close();
		byte[] data = stream.GetBuffer();
		stream.Close();
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, data);
		TurnEvent(); // Lets pass the turn
	}
	public void UpdateTotalCards(int CardsCount)
	{
		MemoryStream stream1 = new MemoryStream();
		BinaryWriter writer1 = new BinaryWriter(stream1);
		const int header = 432;
		writer1.Write(header);
		writer1.Write((byte)CardsCount);
		writer1.Close();
		byte[] dataSe = stream1.GetBuffer();
		stream1.Close();
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, dataSe);
	}
	public void deleteOtherCard(int index)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		const int header = 377;
		writer.Write(header); //Lets write header for identification
		writer.Write((byte)index);
		writer.Close();
		byte[] data = stream.GetBuffer();
		stream.Close();
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, data);
		cardsDeckScript.GetComponent<NumberOfCardsDeck>().UpdateCards(cardDeck); // All done
	}
	public void AddFromDeck(GameObject card)
	{
		GameObject cardNew = Instantiate(card);
		CanceltheTimerInvoke ();
		if (cardNew.name == "SmasingMonk")
		{
			statusText.GetComponent<Text>().text = "THe smashing monks comes up";
		}
		cardNew.transform.SetParent(cardsContainer.transform);
		Vector3 value = cardNew.transform.localPosition;
		value.z = 1;
		cardNew.transform.localPosition = value;
		Vector3 scale = cardNew.transform.localScale;
		scale.x = 1;
		scale.y = 1;
		scale.z = 1;
		cardNew.transform.localScale = scale;
		cardNew.gameObject.GetComponent<Drag>().throwed_case = throwingCase;
		cardNew.gameObject.GetComponent<Drag>().cards = cardsContainer.transform.parent.gameObject;
		cardNew.gameObject.GetComponent<Drag>().container = cardsContainer;
		cardNew.gameObject.name = card.name;
		cardNew.gameObject.GetComponent<Drag>().EmptyPrefab = emptySPrefab;
		TurnEvent();
	}
	public void ReloadAvatar()
	{
		myAvatarObject.GetComponent<Image>().sprite = GetSpriteFromName(PlayerPrefs.GetString("myAvatar"));
		byte[] data = AvatarToBytes(myAvatarObject);
		bool reliable = true;
		Debug.Log("DONE DATA LOADING");
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll(reliable, data);
		Debug.Log("Done Changing AVatar");
	}
	public void changeAvatarsonClick(string main)
	{
		PlayerPrefs.SetString("myAvatar", main);
		PlayerPrefs.Save();
		ReloadAvatar();
	}
	public void Awake()
	{
		if (!PlayerPrefs.HasKey ("myAvatar")) {
			PlayerPrefs.SetString("myAvatar", "avatars_4");
			PlayerPrefs.Save(); // Lets save our avatar
		}
		for (int k = 0; k < allAvatars.Length; k++)
		{
			GameObject avatarsPrefab = Instantiate(avatar);
			avatarsPrefab.transform.SetParent(AvatarDeck.transform);
			avatarsPrefab.GetComponent<Image>().sprite = allAvatars[k];
			avatarsPrefab.transform.localScale = new Vector3(1, 1, 1);
			Button b = avatarsPrefab.GetComponent<Button>();
			string main = (allAvatars[k].name);
			b.onClick.AddListener(() => changeAvatarsonClick(main));
		}
	}
	public GameObject meter;
	public GameObject meterPercent;
	public int FindTotalSmashingMonks()
	{
		int childCount = cardDeck.transform.childCount;
		int totalsmashingMonk = 0;
		for (int i = 0; i < childCount; i++)
		{
			if (cardDeck.transform.GetChild(i).gameObject.name == "SmasingMonk")
			{
				totalsmashingMonk++;
			}
		}
		return totalsmashingMonk;
	}
	public void UpdateMeter(float percent)
	{
		Vector3 rotation = new Vector3(meter.transform.rotation.x, meter.transform.rotation.y, meter.transform.rotation.z);
		rotation.z = 124.13f - (percent / 100f * 208.55f);
		meterPercent.GetComponent<Text> ().text = Math.Ceiling (percent) + "%"; // Show the percent
		meter.transform.rotation = Quaternion.Euler(rotation);
	}
	public void Start()
	{
		string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Zenmate_temp_storage.bin");
		Debug.Log (path);
		// In the starting of the game
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
			// registers a callback to handle game invitations.
			.Build();
		PlayGamesPlatform.InitializeInstance(config);
		
		// recommended for debugging:
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();
		
		/*
		 * TODO: Fix PlayPrefs below Correctly 
		 */
		
		// Lets intialize my info in connecting scene
		myAvatarObject.GetComponent<Image>().sprite = GetSpriteFromName(PlayerPrefs.GetString("myAvatar"));
		
	}
	
	public void Online()
	{
		// Function called when online button is clicked
		const int MinOpponents = 1;
		const int MaxOpponents = 2;
		const int Variant = 0;  // default
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			PlayGamesPlatform.Instance.RealTime.CreateQuickGame (MinOpponents, MaxOpponents, Variant, this); // If it exist just start the game
			connectingToInterentScene.SetActive (true);// Show the connecting scene.
		}
		else
			ShowAlert("Alert","It appears like there is some problem with google play games configuration","Ok");
		
		
	}
	private bool showingWaitingRoom = false;
	public void OnRoomSetupProgress(float progress)
	{
		// show the default waiting room.
		if (!showingWaitingRoom)
		{
			showingWaitingRoom = true;
			PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
		}
	}
	
	public void sendThrow(GameObject thrownCard)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter w = new BinaryWriter(stream);
		const int header = 355;
		w.Write(header); // Lets write the header for specifying this is for the user thrown cade
		w.Write((byte)thrownCard.gameObject.name.Length); // LEts specify the length for byte binary reader
		w.Write(thrownCard.gameObject.name.ToCharArray()); // Lets send our gameobject name
		w.Close();// Lets close the binary writer
		byte[] buffer = stream.GetBuffer();
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, buffer); // Send data to all
		stream.Close(); // Lets close the memory stream
	}
	public void OnLeftRoom()
	{
		// display error message and go back to the menu screen
		
		// (do NOT call PlayGamesPlatform.Instance.RealTime.LeaveRoom() here --
		// you have already left the room!)
	}
	
	public void OnRoomConnected(bool success)
	{
		if (success)
		{
			connectingToInterentScene.SetActive(false);
			myAvatarObject.transform.GetChild (0).gameObject.GetComponent<Text> ().text = PlayGamesPlatform.Instance.RealTime.GetSelf ().DisplayName;
			Debug.Log("Success");

			showError("This is correct");
			List<Participant> participants = PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
			List<string> displayNames = new List<string>();
			List<string> list = new List<string>();
			AllOpenentList = new List<OponentData>();
			for (int i = 0; i < participants.Count; i++)
			{
				displayNames.Add(participants[i].DisplayName);
				if (participants[i].ParticipantId == PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId)
				{
					
				}
				else
				{
					list.Add(participants[i].ParticipantId);
					OponentData Opponentj = new OponentData();
					Opponentj.participantObject = participants[i];
					Opponentj.avatar = null;
					AllOpenentList.Add(Opponentj);
				}
				
			}
			displayNames.Sort();//Sort it asec order
			list.Sort();
			participant_ids = list;
			universalList = list;
			SelfParticipantId = PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId;
			showWaitingGUI(connectingScene);
			//Send the avatar to all the participants
			byte[] data = AvatarToBytes(myAvatarObject);
			bool reliable = true;
			PlayGamesPlatform.Instance.RealTime.SendMessageToAll(reliable, data);
			Debug.Log(SelfParticipantId);
			usernames = displayNames;
			int someIndex = 0;
			for (int u = 0; u < usernames.Count; u++)
			{
				if (usernames[u] == PlayGamesPlatform.Instance.RealTime.GetSelf().DisplayName)
				{
					break;
				}
				someIndex++;
			}
			myIndex = someIndex;// Lets save the player index in a variable to see who is the next participant
			if (usernames[0] == PlayGamesPlatform.Instance.RealTime.GetSelf().DisplayName)
			{
				myChance = true;
				BlackTrans.SetActive(true);
				InvokeRepeating("testin1234", 1, 1);
				cardDeck.GetComponent<CardDeck>().SetupCardDeck();
				MemoryStream newMem = new MemoryStream();
				BinaryWriter newBinary = new BinaryWriter(newMem);
				const int header = 350;
				newBinary.Write(header);//this is to specify that this is for cardDeck informations
				int totalChild = cardDeck.transform.childCount;
				newBinary.Write((byte)totalChild);
				for (int f = 0; f < totalChild; f++)
				{
					newBinary.Write((byte)cardDeck.transform.GetChild(f).gameObject.name.Length);
					newBinary.Write(cardDeck.transform.GetChild(f).gameObject.name.ToCharArray());
				}
				newBinary.Close();
				byte[] allData = newMem.GetBuffer();
				newMem.Close();
				PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, allData);
				cardsContainer.GetComponent<CanvasGroup>().blocksRaycasts = true;
				cardDeck.GetComponent<CanvasGroup>().blocksRaycasts = true;
				statusText.GetComponent<Text>().text = "Its my turn";
			}
		}
		else
		{
			ShowAlert("Alert","Something Went Wrong, Restart the game","Ok");
		}
	}
	void ChangeAvatars(GameObject avatarObject)
	{
		
	}
	public bool CheckIfGameObjectExists(string name)
	{
		if (GameObject.Find(name))
		{
			return true;
		}
		return false;
	}
	public void OnPeersConnected(string[] participantIds)
	{
		// react appropriately (e.g. add new avatars to the game)
	}
	public void OnPeersDisconnected(string[] participantIds)
	{
		if(PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants().Count==1){
			// Finally Everyone is disconnected, so now i can declare that i am the winner
			winnerScreen.SetActive(true);
			myChance = false;
			CancelInvoke("timer"); // Cancel the timer invoke
			timeSe = 15;

		}
		if (participantIds!=null) {
			Participant main = null;
			for (int b=0; b<participantIds.Length; b++) {
				for(int s=0;s<AllOpenentList.Count;s++){
					if(AllOpenentList[s].participantObject.ParticipantId == participantIds[b]){
						main = AllOpenentList[s].participantObject;
					}
				}
				Destroy(GameObject.Find(main.ParticipantId).transform.GetChild(1).gameObject); // Do try this
				if(main==null){
					return;
				}
				int theIndex = 0;
				for (int s = 0; s < usernames.Count; s++)
				{
					if (usernames[s] == main.DisplayName)
					{
						break; // Lets just end it here
					}
					theIndex++;
				}
				if (isReverse)
				{
					if (theIndex == 0)
					{
						if (usernames[usernames.Count - 1] == PlayGamesPlatform.Instance.RealTime.GetSelf().DisplayName)
						{
							myChance = true; // Make it true else skip all of this
						}
					}
					else
					{
						if (theIndex == usernames.Count - 1)
						{
							if (usernames[0] == PlayGamesPlatform.Instance.RealTime.GetSelf().DisplayName)
							{
								myChance = true;
							}
						}
						else
						{
							if (usernames[theIndex - 1] == PlayGamesPlatform.Instance.RealTime.GetSelf().DisplayName)
							{
								myChance = true;
								
							}
						}
						
					}
				}
				else
				{
					if (usernames[theIndex + 1] == PlayGamesPlatform.Instance.RealTime.GetSelf().DisplayName)
					{
						myChance = true;
					}
				}
				GameObject.Find(main.ParticipantId).transform.GetChild(2).GetComponent<Image>().sprite = deadAvatar;
				List<Participant> participants = PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
				List<string> displayNames = new List<string>();
				for (int i = 0; i < participants.Count; i++)
				{
					if (participants[i].ParticipantId == main.ParticipantId)
					{
						
					}
					else
						displayNames.Add(participants[i].DisplayName);
				}
				displayNames.Sort();//Sort it asec order
				usernames = displayNames;
				myIndex = 0;
				for (int l = 0; l < usernames.Count; l++)
				{
					if (usernames[l] == PlayGamesPlatform.Instance.RealTime.GetSelf().DisplayName)
					{
						break;
					}
					Participant toFind = null;
					for (int q = 0; q < participants.Count; q++)
					{
						if (participants[q].DisplayName == usernames[l])
						{
							toFind = participants[q];
						}
					}
					if (toFind.ParticipantId != main.ParticipantId)
					{
						myIndex++;
					}
				}
				main = null; // Lets make it null again
			}
		}
		
	}
	public string GetAvatarFromBytes(byte[] data)
	{
		BinaryReader r = new BinaryReader(new MemoryStream(data));
		int header = r.ReadInt32();
		if (header != Header)
		{
			showError("Board data header " + header +
			          " not recognized.");
		}
		int avatarLength = (int)r.ReadByte();
		string avatarName = new string(r.ReadChars(avatarLength));
		r.Close();
		return avatarName;
	}
	public string getTurnFromBytes(byte[] data)
	{
		MemoryStream stream = new MemoryStream(data);
		BinaryReader reader = new BinaryReader(stream);
		int header = reader.ReadInt32();
		int usernameLength = reader.ReadByte();
		string username = new string(reader.ReadChars(usernameLength));
		reader.Close();
		stream.Close();
		return username;
	}
	public void MakeBlind()
	{
		int childCount = cardsContainer.transform.childCount;
		for (int s = 0; s < childCount; s++)
		{
			cardsContainer.transform.GetChild(s).gameObject.GetComponent<Drag>().myOwnSprite = cardsContainer.transform.GetChild(s).gameObject.GetComponent<Image>().sprite;
			cardsContainer.transform.GetChild(s).gameObject.GetComponent<Image>().sprite = DuckSprite;
		}
		currentlyBlind = true;
		
	}
	
	public void ShowLikeAMonkhand()
	{
		animationHand.SetActive(true); // Show the animation
		cardsContainer.GetComponent<CanvasGroup>().blocksRaycasts = true;
		StartTheTimer();
	}
	public string otherPlayerMonkId;
	public void SendMonkaCard(GameObject card)
	{
		Debug.Log ("SENDING MONK");
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		const int header = 723;
		writer.Write(header);
		writer.Write((byte)card.name.Length);
		writer.Write(card.name.ToCharArray());
		writer.Write((byte)otherPlayerMonkId.Length);
		writer.Write(otherPlayerMonkId.ToCharArray());
		writer.Close();
		byte[] data = stream.GetBuffer();
		stream.Close();
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, data);
		DestroyImmediate (card); // Destroy the card that is now left behind
	}
	
	public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
	{
		Debug.Log("RECIEVED");
		MemoryStream stream = new MemoryStream(data);
		BinaryReader reader = new BinaryReader(stream);
		int header = reader.ReadInt32();
		reader.Close();
		stream.Close();
		if(header==2819){
			MemoryStream streamf = new MemoryStream(data);
			BinaryReader readerf = new BinaryReader(streamf);
			int fHeader = readerf.ReadInt32 ();
			int placingPosition = readerf.ReadByte ();
			cardDeck.transform.GetChild(cardDeck.transform.childCount-1).transform.SetSiblingIndex(placingPosition); // All done
			readerf.Close ();
			streamf.Close ();
		}
		if (header == 723)
		{
			MemoryStream streamse = new MemoryStream(data);
			BinaryReader readerse = new BinaryReader(streamse);
			int fHeader = readerse.ReadInt32();
			int cardnameLength = readerse.ReadByte();
			string cardname = new string(readerse.ReadChars(cardnameLength));
			int otherPlayerLength = readerse.ReadByte();
			string otherPlayerId = new string(readerse.ReadChars(otherPlayerLength));
			if (otherPlayerId == PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId)
			{
				GameObject card = null; // Lets define our object
				for (int ab = 0; ab < allCardIncluded.Count; ab++)
				{
					if (allCardIncluded[ab].name == cardname)
					{
						card = allCardIncluded[ab];
						Debug.Log ("Fiannly received the card");
						break;
					}
				}
				Debug.Log ("Starting to get steal pasted in ours");
				if(card!=null){
					GameObject newObj = Instantiate(card); // Lets instantiate the card
					newObj.transform.SetParent(cardsContainer.transform);
					Vector3 newPos = newObj.transform.localPosition;
					newPos.z = 1;
					newObj.transform.localPosition = newPos;
					newObj.transform.localScale = new Vector3(1, 1, 1); // Lets set it to the default
					// All done we have set it now
					// We have recieved the card
				}
				
			}
			readerse.Close();
			streamse.Close();
		}
		if (header == 271)
		{
			MemoryStream streamse = new MemoryStream(data);
			BinaryReader readerse = new BinaryReader(streamse);
			int fHeader = readerse.ReadInt32();
			int usernameLength = readerse.ReadByte();
			string usernamese = new string(readerse.ReadChars(usernameLength));
			if (usernamese == PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId)
			{
				isSteal = true;
				currentlyGotSteal = true;
				otherPlayerMonkId = senderId;
				ShowLikeAMonkhand();
				// Do something like stealing
			}
			// Otherwise just skip this thing here
			streamse.Close();
			readerse.Close();
		}
		if (header == 113)
		{
			MemoryStream streams = new MemoryStream(data);
			BinaryReader readers = new BinaryReader(streams);
			int headers = readers.ReadInt32();
			int lengthS = readers.ReadByte();
			string participateId = new string(readers.ReadChars(lengthS));
			if (participateId == PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId)
			{
				MakeBlind();
			}
			readers.Close();
			streams.Close();
		}
		if (header == 149)
		{
			MemoryStream streamSe = new MemoryStream(data);
			BinaryReader readerSe = new BinaryReader(streamSe);
			int fHeader = readerSe.ReadInt32();
			isReverse = readerSe.ReadBoolean();
			readerSe.Close(); // Lets close this too
			streamSe.Close(); // Lets close this 
		}
		if (header == 621)
		{
			MemoryStream streamk = new MemoryStream(data);
			BinaryReader readerk = new BinaryReader(streamk);
			int Fheader = readerk.ReadInt32(); // Waste
			int lengthS = readerk.ReadByte();
			string name = new string(readerk.ReadChars(lengthS));
			if (name == "Shuffle")
			{
				List<string> listkx = new List<string>();
				int lengthSq = readerk.ReadByte();
				for (int l = 0; l < lengthSq; l++)
				{
					int lengthxl = readerk.ReadByte();
					string addNew = new string(readerk.ReadChars(lengthxl));
					listkx.Add(addNew);
				}
				cardDeck.GetComponent<CardDeck>().SetupAgainFromRecieveList(listkx);
			}
			readerk.Close();
			streamk.Close();
		}
		if (header == 532)
		{
			// This is where we will remove the user after he leavs the room or try to watch it
			if (GameObject.Find(senderId))
			{
				GameObject.Find(senderId).transform.GetChild(2).GetComponent<Image>().sprite = deadAvatar;
				List<Participant> participants = PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
				List<string> displayNames = new List<string>();
				for (int i = 0; i < participants.Count; i++)
				{
					if (participants[i].ParticipantId == senderId)
					{
						
					}
					else
						displayNames.Add(participants[i].DisplayName);
				}
				displayNames.Sort();//Sort it asec order
				usernames = displayNames;
				myIndex = 0;
				for (int l = 0; l < usernames.Count; l++)
				{
					
					if (usernames[l] == PlayGamesPlatform.Instance.RealTime.GetSelf().DisplayName)
					{
						break;
					}
					Participant toFind = null;
					for (int q = 0; q < participants.Count; q++)
					{
						if (participants[q].DisplayName == usernames[l])
						{
							toFind = participants[q];
						}
					}
					if (toFind.ParticipantId != senderId)
					{
						myIndex++;
					}
				}
				Destroy(GameObject.Find(senderId).transform.GetChild(1).gameObject);
			}
			 if(PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants().Count==1){
				// Everyone has  been disconnected now its our turn. So finally i am the winner
				// TODO: This is a dummy implementation of this method correct this later.
				winnerScreen.SetActive(true);
				//Now just stop the game
				/* Send all the people that who is the winner
				 * MemoryStream streamka = new MemoryStream();
				BinaryWriter writerka = new BinaryWriter(streamka);
				const int headersef = 978;
				writerka.Write(headersef);
				writerka.Write ((byte)PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants()[0].DisplayName.Length);
				writerka.Write(PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants()[0].DisplayName.ToCharArray());
				writerka.Close();
				byte[] dataAll = streamka.GetBuffer();
				streamka.Close ();
				*/
				myChance = false;
				CancelInvoke("timer"); // Cancel the timer invoke
				timeSe = 15;
			}

		}
		if (header == 432)
		{
			// To update playrs total Card
			MemoryStream streamsx = new MemoryStream(data);
			BinaryReader readersx = new BinaryReader(streamsx);
			int headerNo = readersx.ReadInt32();
			int ChildCount = readersx.ReadByte();
			if (GameObject.Find(senderId))
				GameObject.Find(senderId).transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = ChildCount + "";
			else
			{
				Debug.Log("CANT FIND THE USER SPECIFIED");
			}
			readersx.Close();
			streamsx.Close();
		}
		if (header == 377)
		{
			
			//To update the cardDeck
			MemoryStream value = new MemoryStream(data);
			BinaryReader ReaderS = new BinaryReader(value);
			int feHeader = ReaderS.ReadInt32();
			int getIndex = ReaderS.ReadByte();
			Debug.Log ("Trying to Delete the card on top of deck and the index is"+getIndex);
			ReaderS.Close();
			value.Close();
			DestroyImmediate(cardDeck.transform.GetChild(cardDeck.transform.childCount-1).gameObject); // Lets see now
			cardsDeckScript.GetComponent<NumberOfCardsDeck>().UpdateCards(cardDeck); // All done
		}
		if (header == 350)
		{
			// To setup carddeck send from the first player
			MemoryStream aMem = new MemoryStream(data);
			BinaryReader aReader = new BinaryReader(aMem);
			int aHeader = aReader.ReadInt32();
			int totalChild = aReader.ReadByte();
			List<string> allCardsName = new List<string>();
			for (int u = 0; u < totalChild; u++)
			{
				int nameLength = aReader.ReadByte();
				string name = new string(aReader.ReadChars(nameLength));
				allCardsName.Add(name);
			}
			aReader.Close();
			aMem.Close();
			Debug.Log("This is " + allCardsName[0]);
			cardDeck.GetComponent<CardDeck>().setupFromList(allCardsName);
		}
		else
		{
			if (header == 100)
			{
				// For finding out whose the turn is
				// We know who the user is now
				string username = getTurnFromBytes(data);
				UpdateMeter(FindTotalSmashingMonks() / cardDeck.transform.childCount);
				// He is the new user who should play the turn
				if (username == PlayGamesPlatform.Instance.RealTime.GetSelf().DisplayName)
				{
					myChance = true;
					BlackTrans.SetActive(true);
					InvokeRepeating("testin1234", 1, 1);
					StartTheTimer();
					TurnsToTake = 1;
					cardsContainer.GetComponent<CanvasGroup>().blocksRaycasts = true;
					cardDeck.GetComponent<CanvasGroup>().blocksRaycasts = true;
					if (statusText.GetComponent<Text>().text == "THe smashing monks comes up")
					{
						
					}
					else
						statusText.GetComponent<Text>().text = "Its my turn";
				}
			}
			if (header == 101)
			{
				// For finding out whose the turn is
				// We know who the user is now
				MemoryStream newSem = new MemoryStream(data);
				BinaryReader semReader = new BinaryReader(newSem);
				int header1 = semReader.ReadInt32();
				int turnstoS = semReader.ReadByte();
				int userLength = semReader.ReadByte();
				string username = new string(semReader.ReadChars(userLength));
				// He is the new user who should play the turn
				if (username == PlayGamesPlatform.Instance.RealTime.GetSelf().DisplayName)
				{
					myChance = true;
					BlackTrans.SetActive(true);
					InvokeRepeating("testin1234", 1, 1);
					StartTheTimer();
					TurnsToTake += turnstoS; // Try this or replace it
					cardsContainer.GetComponent<CanvasGroup>().blocksRaycasts = true;
					cardDeck.GetComponent<CanvasGroup>().blocksRaycasts = true;
				}
			}
			if (header == 355)
			{
				MemoryStream kMem = new MemoryStream(data);
				BinaryReader kR = new BinaryReader(kMem);
				int headerWaste = kR.ReadInt32(); //Lets move on with bytes
				int lengthCard = kR.ReadByte(); //Lest move on with bytes
				string cardName = new string(kR.ReadChars(lengthCard)); // Lets move on with char of length lengthCard
				GameObject toThrow = null;
				for (int e = 0; e < allCardIncluded.Count; e++)
				{
					if (cardName.Contains(allCardIncluded[e].gameObject.name))
					{
						toThrow = allCardIncluded[e];
						break; // Lets break the loop now because we have find out what we needed to find out
					}
				}
				if (toThrow != null)
				{
					// We have found the card that we needed
					GameObject mainS = Instantiate(toThrow);
					Destroy(mainS.GetComponent<LayoutElement>());
					mainS.transform.SetParent(throwingCase.transform);// Lets make the throwing case parent of this throwed card
					Vector3 positon = mainS.GetComponent<RectTransform>().localPosition;
					positon.z = 1;
					mainS.GetComponent<RectTransform>().localPosition = positon;
					Vector3 scale2 = mainS.GetComponent<RectTransform>().localScale;
					scale2.x = 1;
					scale2.y = 1;
					scale2.z = 1;
					mainS.GetComponent<RectTransform>().localScale = scale2;
					mainS.GetComponent<CanvasGroup>().blocksRaycasts = false; // Now don't allow it to be dragged. Finally We are done here
					Debug.Log("THIS IS THE NAME OF THE OBJECT" + mainS.name);
				}
				else
				{
					Debug.Log("ITS NULL ALL OVER");
				}
				
				kR.Close();
				kMem.Close();
			}
			if (header == 10000)
			{
				//To update avatars of the players
				Debug.Log("working");
				string avatarName = GetAvatarFromBytes(data);
				Debug.Log(avatarName);
				if (!CheckIfGameObjectExists(senderId))
				{
					SetupWatingGUI();
				}
				if (CheckIfGameObjectExists(senderId))
				{
					bool findAvatar = false;
					Sprite CorrectSprite = null;
					for (int k = 0; k < allAvatars.Length; k++)
					{
						if (allAvatars[k].name == avatarName)
						{
							findAvatar = true;
							CorrectSprite = allAvatars[k];
							break;
						}
					}
					Debug.Log("HERE IS YOUR CULPRIT" + AllOpenentList[0].participantObject.DisplayName);
					for (int n = 0; n < AllOpenentList.Count; n++)
					{
						if (AllOpenentList[n].participantObject.ParticipantId == senderId)
						{
							if (findAvatar)
								AllOpenentList[n].avatar = CorrectSprite;
							break;
						}
					}
					if (findAvatar)
					{
						GameObject.Find(senderId).GetComponent<Image>().sprite = CorrectSprite;
					}
					else
					{
						showError("Can't find the avatar send please have a look at the console and see whats going on");
					}
					
				}
				else
				{
					showError("There is some kind of error going in with senderId.");
				}
			}
		}
		
	}
	public GameObject senderIdPrefab;
	public void SetupWatingGUI()
	{
		for (int l = 0; l < participant_ids.Count; l++)
		{
			if(!GameObject.Find (participant_ids[l])){
				GameObject currentGameObject = Instantiate(senderIdPrefab);
				currentGameObject.transform.SetParent(OtherPlayerGroup.transform);
				Vector3 position = currentGameObject.transform.localPosition;
				position.z = 1;
				currentGameObject.transform.localPosition = position;
				currentGameObject.transform.localScale = new Vector3(1, 1, 1);
				Debug.Log("Got it");
				List<Participant> allData = PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
				Participant currentParticipant = null;
				Debug.Log("Again got it");
				for (int s = 0; s < allData.Count; s++)
				{
					if (allData[s].ParticipantId == participant_ids[l])
					{
						currentParticipant = allData[s];
					}
				}
				if (currentParticipant == null)
				{
					showError("Some error Occured in participant ids");
					break;
				}
				Debug.Log(currentParticipant.DisplayName);
				currentGameObject.GetComponentInChildren<Text>().text = currentParticipant.DisplayName;
				currentGameObject.name = currentParticipant.ParticipantId;
			}
		
		}
		
		Debug.Log("DONE");
	}
	void showWaitingGUI(GameObject scene)
	{
		//TODO: Make some transition effect here
		current.SetActive(false);
		scene.SetActive(true);
		SetupWatingGUI();
		InvokeRepeating("TimerCount", 1, 1F);
	}
	public void ChangeAvatar()
	{
		//TODO : DO SOME ANIMATION EFFECT HERE SO THAT THE USER CAN BE PLEASED.
		connectingScene.SetActive(false);
		ChangeAvatarObject.SetActive(true);
		//Show the screen and when the user changes the avatar just do all the formalities.
	}
	int time = 10;
	public void TimerCount()
	{
		time--;
		timerCountdown.text = "00:" + time;
		if (time == 0)
		{
			
			goToplayScreen();// Now lets go to the playscreen
			CancelInvoke("TimerCount");
		}
	}
	public void SendSteal(string participantID)
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		const int header = 271;
		writer.Write(header);
		writer.Write((byte)participantID.Length);
		writer.Write(participantID.ToCharArray()); // Send the usenrame
		writer.Close();
		byte[] data = stream.GetBuffer();
		stream.Close();
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, data); // THis should be reliable
		
	}
	public void ShowArrows(){
		int total_avatars = playersPanel.transform.childCount;
		for (int i=0; i<total_avatars; i++) {
			playersPanel.transform.GetChild(i).GetChild(3).gameObject.SetActive(true);
		}
	}
	public void HideArrows(){
		int total_avatars = playersPanel.transform.childCount;
		for (int i=0; i<total_avatars; i++) {
			playersPanel.transform.GetChild(i).GetChild(3).gameObject.SetActive(false);
		}
	}
	public void OtherPlayerClick(GameObject thisGames)
	{
		if (isAttackMode == true)
		{
			MyTurnCompletes(thisGames.transform.GetChild(0).GetComponent<Text>().text, turnForAttack);
			isAttackMode = false;
		}
		if (isWhattheDuck == true)
		{
			MemoryStream streams = new MemoryStream();
			BinaryWriter writers = new BinaryWriter(streams);
			const int header = 113;
			writers.Write(header);
			writers.Write((byte)thisGames.name.Length);
			writers.Write(thisGames.name.ToCharArray());
			writers.Close();
			byte[] data = streams.GetBuffer();
			streams.Close();
			PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, data); // Send to message to all player
			isWhattheDuck = false;
		}
		if (isSteal == true && currentlyGotSteal==false)
		{
			SendSteal(thisGames.name);
			myChance = false;
			cardsContainer.GetComponent<CanvasGroup>().blocksRaycasts = false;
			cardDeck.GetComponent<CanvasGroup>().blocksRaycasts = false;
			isSteal = false;
		}
		HideArrows (); // Hide the arrows
	}
	void goToplayScreen()
	{
		//TODO: Putdown some animation effect here please,
		connectingToInterentScene.SetActive (false);
		ChangeAvatarObject.SetActive(false);
		if(myChance==true)
			StartTheTimer();
		connectingScene.SetActive(false);
		GameArea.SetActive(true);
		gameArea_myAvatar.GetComponent<Image>().sprite = GetSpriteFromName(PlayerPrefs.GetString("myAvatar"));
		for (int b = 0; b < AllOpenentList.Count; b++)
		{
			GameObject OtherPlayer = Instantiate(otherPlayerPrefab);
			OtherPlayer.transform.SetParent(playersPanel.transform);
			Vector3 position = OtherPlayer.transform.localPosition;
			position.z = 1;
			OtherPlayer.transform.localPosition = position;
			OtherPlayer.transform.localScale = new Vector3(1, 1, 1);
			OtherPlayer.name = AllOpenentList[b].participantObject.ParticipantId;
			OtherPlayer.transform.GetChild(0).GetComponent<Text>().text = AllOpenentList[b].participantObject.DisplayName;
			OtherPlayer.transform.GetChild(2).GetComponent<Image>().sprite = AllOpenentList[b].avatar;
			int Xvalue = 0;
			for (int v = 0; v < allAvatars.Length; v++)
			{
				if (allAvatars[v] == AllOpenentList[b].avatar)
				{
					break;
				}
				Xvalue++;
			}
			OtherPlayer.transform.GetChild(1).GetComponent<Image>().sprite = divs[Xvalue];
			OtherPlayer.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "13";
			OtherPlayer.AddComponent<Button>();
			Button bs = OtherPlayer.GetComponent<Button>();
			bs.onClick.AddListener(() => OtherPlayerClick(OtherPlayer));
			Color myColor = new Color();
			Color.TryParseHexString(allColorsCode[Xvalue], out myColor);
			OtherPlayer.transform.GetChild(1).GetChild(0).GetComponent<Text>().color = myColor;
		}
		//Here just make play screen visible
	}
	void SceneLoading()
	{
		// Start the loading Process
		transparent.SetActive(true);
	}
	public void Back()
	{
		SceneLoading(); // Calls the SceneLoading Screen
		Application.LoadLevelAsync(0); // Load to the homepage
	}
	public void ShuffleCard()
	{
		cardDeck.GetComponent<CardDeck>().ShuffleCards();
		MemoryStream streamer = new MemoryStream();
		BinaryWriter writer1 = new BinaryWriter(streamer);
		const int header = 621;
		writer1.Write(header); // FOr identification
		string cardName = "Shuffle";
		writer1.Write((byte)cardName.Length);
		writer1.Write(cardName.ToCharArray());
		writer1.Write((byte)cardDeck.transform.childCount);
		for (int x = 0; x < cardDeck.transform.childCount; x++)
		{
			writer1.Write((byte)cardDeck.transform.GetChild(x).gameObject.name.Length);
			writer1.Write(cardDeck.transform.GetChild(x).gameObject.name.ToCharArray());
		}
		writer1.Close();
		byte[] data = streamer.GetBuffer();
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, data);
		streamer.Close();
	}
	public void LeaveGame()
	{
		ShowArrows ();
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		const int header = 532;
		writer.Write(header);
		writer.Close();
		byte[] data = stream.GetBuffer();
		stream.Close();
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, data); // Send the message now
		PlayGamesPlatform.Instance.RealTime.LeaveRoom(); // Just leave the room
		transparent.SetActive(true);
		Application.LoadLevelAsync(0); // Load to the homepage
		// React Accordingly
	}
	public void SeeFuture(List<string> allCards)
	{
		// Destroy all the child first
		for (int kas=0; kas<layoutFuture.transform.childCount; kas++) {
			DestroyImmediate(layoutFuture.transform.GetChild(kas).gameObject); 
		}
		// Now allow them to show the cards
		float starting = -1.47f;
		GameArea.SetActive(false); // Make this false
		futureScene.SetActive(true); // Make this true
		List<GameObject> prefabsAll = new List<GameObject>();
		prefabsAll = cardDeck.GetComponent<CardDeck>().allCards;
		if (allCards.Count == 5)
		{
			layoutFuture.GetComponent<HorizontalLayoutGroup>().spacing = -153.87f;
		}
		for (int k = 0; k < allCards.Count; k++)
		{
			GameObject cardTarget = null;
			for (int s = 0; s < prefabsAll.Count; s++)
			{
				if (prefabsAll[s].name == allCards[k])
				{
					cardTarget = prefabsAll[s];
					break;
				}
			}
			GameObject targetInst = Instantiate(cardTarget);
			targetInst.transform.SetParent(layoutFuture.transform);
			targetInst.transform.localScale = new Vector3(1, 1, 1);// FInal
			Vector3 posit = targetInst.transform.localPosition;
			posit.z = 1;
			targetInst.transform.localPosition = posit;
			Vector3 rotation = new Vector3(targetInst.transform.rotation.x, targetInst.transform.rotation.y, targetInst.transform.rotation.z);
			rotation.z = starting;
			targetInst.transform.rotation = Quaternion.Euler(rotation);
			Destroy(targetInst.GetComponent<Drag>());
			starting += 1.47f;
		}
	}
	public void OkayFromfuture()
	{
		layoutFuture.GetComponent<HorizontalLayoutGroup> ().spacing = 0; // Lets do it same
		futureScene.SetActive(false); // Lets make it invisible
		GameArea.SetActive(true); // Lets make it visible
		layoutFuture.GetComponent<HorizontalLayoutGroup>().spacing = 0f;
	}
	
	public void timer()
	{
		timeSe--;
		if (timeSe == 10)
		{
			TimerObject.SetActive(true);
		}
		if (timeSe <= 0)
		{
			TimerObject.SetActive(false);
			timeSe = 15;
			CancelInvoke("timer");
			if (isSmashingMonk)
			{
				Debug.Log("XPLODE");
				myChance = false;
				isSmashingMonk = false;
				// Here is the sreen of the death
				DeadScreen.SetActive(true);
				MemoryStream stream = new MemoryStream();
				BinaryWriter writer = new BinaryWriter(stream);
				const int header = 532;
				writer.Write(header);
				writer.Close();
				byte[] data = stream.GetBuffer();
				stream.Close();
				PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, data); // Send the message now
				PlayGamesPlatform.Instance.RealTime.LeaveRoom(); // Just leave the room
				transparent.SetActive(true); // AllDone
			}
			else
			{
				cardDeck.transform.GetChild(cardDeck.transform.childCount - 1).gameObject.GetComponent<CardDeckDrag>().AutomaticDrag();
			}
			
		}
	}
	public void OnParticipantLeft(Participant part){
		// Just leave it blank we will handle it later
	}
	public void OnDeadBackButtonPressed(){
		Application.LoadLevel (0);
		//Handle it here
		
	}
	public void OnWatchDeadButtonPressed(){
		DeadScreen.SetActive(false);
		GameArea.SetActive(true);
		// Handle it here.
	}
	public GameObject ButtonSmashPrefab;
	public GameObject smashingMonkPlacing;
	public void PlaceSmashing(string place){
		int smashingMonkPosition = cardDeck.transform.childCount - 1;
		GameObject smashingMonk = cardDeck.transform.GetChild (smashingMonkPosition).gameObject;
		Debug.Log (place);
		int positionExpected = 9281; // Just initialze the value
		switch (place) {
		case "Top":
			// Hey do nothing here now because we don't need anything right now
			break;
		case "Second":
			cardDeck.transform.GetChild(smashingMonkPosition).transform.SetSiblingIndex(smashingMonkPosition - 1);
			positionExpected = smashingMonkPosition - 1;
			break;
		case "Third":
			cardDeck.transform.GetChild(smashingMonkPosition).transform.SetSiblingIndex(smashingMonkPosition - 2);
			positionExpected = smashingMonkPosition - 2;
			break;
		case "Fourth":
			cardDeck.transform.GetChild(smashingMonkPosition).transform.SetSiblingIndex(smashingMonkPosition - 3);
			positionExpected = smashingMonkPosition - 3;
			break;
		case "Fifth":
			cardDeck.transform.GetChild(smashingMonkPosition).transform.SetSiblingIndex(smashingMonkPosition - 4);
			positionExpected = smashingMonkPosition - 4;
			break;
		case "Bottom":
			cardDeck.transform.GetChild(smashingMonkPosition).transform.SetSiblingIndex(0);
			positionExpected = 0;
			break;
		default:
			
			break;
		}
		if (positionExpected == 9281) {
			// The value hasn't changed this means that the position choosen is top
			
		} else {
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			const int headerf  = 2819;
			writer.Write(headerf);
			writer.Write ((byte)positionExpected);
			writer.Close ();
			byte[] data = stream.GetBuffer();
			stream.Close ();
			PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true,data); // This should be reliable
		}
		smashingMonkPlacing.SetActive (false);
		TurnEvent (); // Now lets switch the turn
	}
	public void PlaceSmashingMonk(){
		int getChildCount = smashingMonkPlacing.transform.childCount;
		for (int ak = 0; ak<getChildCount; ak++) {
			DestroyImmediate(smashingMonkPlacing.transform.GetChild (ak).gameObject); // Just delete all  the columns before doing anything
		}
		int totalCardsNow = cardDeck.transform.childCount; // Child Count
		List<string> positionList = new List<string> ();
		switch (totalCardsNow) {
		case 5:
			positionList.Add("Top");
			positionList.Add ("Second");
			positionList.Add ("Third");
			positionList.Add ("Fourth");
			positionList.Add ("Bottom");
			break;
		case 4:
			positionList.Add("Top");
			positionList.Add ("Second");
			positionList.Add ("Third");
			positionList.Add ("Bottom");
			break;
		case 3:
			positionList.Add("Top");
			positionList.Add ("Second");
			positionList.Add ("Bottom");
			break;
		case 2:
			positionList.Add("Top");
			positionList.Add ("Bottom");
			break;
		default:
			positionList.Add ("Top");
			positionList.Add ("Second");
			positionList.Add ("Third");
			positionList.Add ("Fourth");
			positionList.Add ("Fifth");
			positionList.Add ("Bottom");
			break;
		}
		smashingMonkPlacing.SetActive (true);
		for(int i=0;i<positionList.Count;i++){
			GameObject newGameObject = Instantiate(ButtonSmashPrefab);
			newGameObject.transform.SetParent(smashingMonkPlacing.transform);
			Vector3 position = newGameObject.transform.localPosition;
			position.z = 1;
			newGameObject.transform.localPosition = position; // Lets do  this
			Vector3 scale = newGameObject.transform.localScale;
			newGameObject.transform.localScale = new Vector3(1,1,1);
			
			newGameObject.transform.GetChild(0).GetComponent<Text>().text = positionList[i];
			Button b = newGameObject.GetComponent<Button>();
			b.onClick.AddListener(() => PlaceSmashing(newGameObject.transform.GetChild(0).GetComponent<Text>().text));
		}
	}
	public int seconds1 = 0;
	public void testin1234()
	{
		// Just to increase total lines
		seconds1++;
		if (seconds1 == 2)
		{
			seconds1 = 0;
			BlackTrans.SetActive(false);
			CancelInvoke("testin1234");
		}
	}
}