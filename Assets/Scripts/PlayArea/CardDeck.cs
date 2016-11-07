using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
public class CardDeck : MonoBehaviour {
    public Transform prefab;
    public List<GameObject> allCards;
    public GameObject Handler;
    public GameObject thisGame;
    public GameObject cardDeckScript;
    public GameObject smasingMonkScreen;
	public GameObject cardsinHand;
    public static System.Random rng = new System.Random();
    public void ClickDeck(GameObject objectGame)
    {
        
    }
    public void SetupAgainFromList(List<string> allNames)
    {
        for (int i = 0; i < allNames.Count; i++)
        {
            Transform test = Instantiate(prefab);
            test.SetParent(thisGame.transform);
            test.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            test.gameObject.AddComponent<CardDeckDrag>();
            test.gameObject.GetComponent<CardDeckDrag>().allCards = allCards;
            test.gameObject.GetComponent<CardDeckDrag>().smasingMonkScreen = smasingMonkScreen;
            test.gameObject.GetComponent<CardDeckDrag>().infoGa = test.GetChild(0).gameObject;
            test.gameObject.GetComponent<CardDeckDrag>().cardDeck = this.gameObject;
			test.gameObject.GetComponent<CardDeckDrag>().cardsInHands = cardsinHand;
            test.gameObject.name = allNames[i];
            Vector3 Pos1 = test.position;
            Pos1.z = 1;
            test.position = Pos1;
            Button b;
            b = test.gameObject.GetComponent<Button>();
            if (!b)
                test.gameObject.AddComponent<Button>();
            b = test.gameObject.GetComponent<Button>();
            b.onClick.AddListener(() => ClickDeck(test.gameObject));
            test.GetChild(0).gameObject.SetActive(false);// Correct this thing later
        }
    }
    public  void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    int thisTimeValue = 0;
    public  void DestroyChildren(GameObject go)
    {

        List<GameObject> children = new List<GameObject>();
        foreach (Transform tran in go.transform)
        {
            children.Add(tran.gameObject);
        }
        children.ForEach(child => GameObject.Destroy(child));
    }
    public void ShuffleCards()
    {
        List<string> allGameS = new List<string>();
        for (int s = thisGame.transform.childCount - 1; s >= 0; s--)
        {
            allGameS.Add(thisGame.transform.GetChild(0).gameObject.name);
            thisGame.transform.GetChild(0).gameObject.SetActive(true);
            DestroyImmediate(thisGame.transform.GetChild(0).gameObject); // Lets destroy this
        }
        Shuffle(allGameS); // Shuffles the list
            
		SetupAgainFromList(allGameS);
        thisTimeValue++;
    }
    public void SetupAgainFromRecieveList(List<string> allGameS)
    {
        for (int s = 0; s < this.gameObject.transform.childCount; s++)
        {
            Destroy(this.gameObject.transform.GetChild(s).gameObject); // Lets destroy this
        }
        SetupAgainFromList(allGameS);
        for (int k = 0; k < this.gameObject.transform.childCount; k++)
        {
            if (k == (this.gameObject.transform.childCount - 3))
            {
                for (int s = 0; s < (this.gameObject.transform.childCount - 3); s++)
                {
                    this.gameObject.transform.GetChild(s).gameObject.SetActive(false);
                }
            }
        }
    }
    public void ArrangeShuffleCards()
    {

    }
    public  GameObject FindObject(this GameObject parent, string name)
    {
        Transform[] trs = (Transform[])parent.GetComponentsInChildren<Transform>();
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }
   
    public void Test()
    {
    }
    public void SetupCardDeck()
    {
        int Oponnets = 2;
        int playersPlaying = Oponnets;
        int totalChild = 24 - (playersPlaying);
        if (Oponnets == 3)
        {
            totalChild = 30 - (playersPlaying);
        }
        if (Oponnets == 4)
        {
			totalChild = 37 - (playersPlaying);
        }
        for (int s = 0; s < (playersPlaying-1); s++)
        {
            Transform test = Instantiate(prefab);
            test.gameObject.transform.SetParent(thisGame.transform);
            Vector3 newsx = test.gameObject.transform.localPosition;

            test.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            bool isNotAllowed = true;
            string RightCard = null;
            string name = "SmasingMonk";
            GameObject t = this.gameObject;
            RightCard = name;
            isNotAllowed = false;
            test.gameObject.name = RightCard;
            test.gameObject.AddComponent<CardDeckDrag>();
            test.gameObject.GetComponent<CardDeckDrag>().allCards = allCards;
            test.gameObject.GetComponent<CardDeckDrag>().smasingMonkScreen = smasingMonkScreen;
            test.gameObject.GetComponent<CardDeckDrag>().infoGa = test.GetChild(0).gameObject;
            test.gameObject.GetComponent<CardDeckDrag>().cardDeck = this.gameObject;
			test.gameObject.GetComponent<CardDeckDrag>().cardsInHands = cardsinHand;
			Vector3 Pos1 = test.position;
            Pos1.z = 1;
            test.position = Pos1;
            Button b;
            b = test.gameObject.GetComponent<Button>();
            if (!b)
                test.gameObject.AddComponent<Button>();
            b = test.gameObject.GetComponent<Button>();
            b.onClick.AddListener(() => ClickDeck(test.gameObject));
            test.GetChild(0).gameObject.SetActive(false);// Correct this thing later
        }
            for (int i = playersPlaying - 1; i < totalChild; i++)
            {
             
                Transform test = Instantiate(prefab);
                test.gameObject.transform.SetParent(thisGame.transform);
                Vector3 newsx = test.gameObject.transform.localPosition;

                test.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                bool isNotAllowed = true;
                string RightCard = null;
                while (isNotAllowed)
                {
                    string name = allCards[Random.Range(0, allCards.Count)].name;
                if (name != "SmasingMonk"){
                    int probablity = Random.Range(0, 100000);
                    GameObject t = this.gameObject;
                    RightCard = name;
                    isNotAllowed = false;
                }
              
                }
                test.gameObject.name = RightCard;
                test.gameObject.AddComponent<CardDeckDrag>();
                test.gameObject.GetComponent<CardDeckDrag>().allCards = allCards;
                test.gameObject.GetComponent<CardDeckDrag>().smasingMonkScreen = smasingMonkScreen;
                test.gameObject.GetComponent<CardDeckDrag>().infoGa = test.GetChild(0).gameObject;
                test.gameObject.GetComponent<CardDeckDrag>().cardDeck = this.gameObject;
			test.gameObject.GetComponent<CardDeckDrag>().cardsInHands = cardsinHand;
			Vector3 Pos1 = test.position;
                Pos1.z = 1;
                test.position = Pos1;
                Button b;
                b = test.gameObject.GetComponent<Button>();
                if (!b)
                    test.gameObject.AddComponent<Button>();
                b = test.gameObject.GetComponent<Button>();
                b.onClick.AddListener(() => ClickDeck(test.gameObject));
                test.GetChild(0).gameObject.SetActive(false);// Correct this thing later
        }
        cardDeckScript.GetComponent<NumberOfCardsDeck>().UpdateCards(this.gameObject);
        ShuffleCards();
        ShuffleCards();
        ShuffleCards();
        float value = (float) (1f / this.transform.childCount);
        Handler.GetComponent<Events>().UpdateMeter( value * 100);
    } 
    public void setupFromList(List<string> allNames)
    {
        for (int i = 0; i < allNames.Count; i++)
        {
            Transform test = Instantiate(prefab);
            test.gameObject.transform.SetParent(thisGame.transform);
            test.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            test.gameObject.AddComponent<CardDeckDrag>();
            test.gameObject.GetComponent<CardDeckDrag>().allCards = allCards;
            test.gameObject.GetComponent<CardDeckDrag>().smasingMonkScreen = smasingMonkScreen;
            test.gameObject.GetComponent<CardDeckDrag>().infoGa  = test.GetChild(0).gameObject ;
            test.gameObject.GetComponent<CardDeckDrag>().cardDeck = this.gameObject;
            test.gameObject.name = allNames[i];
			test.gameObject.GetComponent<CardDeckDrag>().cardsInHands = cardsinHand;
			Vector3 Pos1 = test.position;
            Pos1.z = 1;
            test.position = Pos1;
            Button b;
            b = test.gameObject.GetComponent<Button>();
            if (!b)
                test.gameObject.AddComponent<Button>();
            b = test.gameObject.GetComponent<Button>();
            b.onClick.AddListener(() => ClickDeck(test.gameObject));
            test.GetChild(0).gameObject.SetActive(false);// Correct this thing later
        }
        cardDeckScript.GetComponent<NumberOfCardsDeck>().UpdateCards(this.gameObject);
    }
}