using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler{
    // Declare the variables
    private Vector2 ContainerPos;
    private GameObject main;
    private bool inside;
    public GameObject cards;
    public GameObject throwed_case;
    public GameObject container;
    public List<GameObject> allCards;
    public GameObject EmptyPrefab;
    public GameObject EventHandles;
    public Sprite myOwnSprite;
    public void Start()
    {
        inside = true;
        EventHandles = GameObject.Find("_HANDLER"); 
    }
    public void OnBeginDrag(PointerEventData data)
    {
        Debug.Log("This is called again");

        ContainerPos = this.gameObject.transform.parent.transform.position; // Store the position of the container of cards
        main = this.gameObject; // Set the value of GameObject to be accessed later
        throwed_case.GetComponent<Drop>().selectedG = main;
        cards.SendMessage("OnBeginDrag", data);
        int childCount = container.transform.childCount;
        allCards = new List<GameObject>();
        for (int s = 0; s < childCount; s++)
        {
            if (container.transform.GetChild(s).gameObject.name == this.gameObject.name)
            {

            }
            else
            {
                if(container.transform.GetChild(s).gameObject.name=="Empty"){

                }
                else
                     allCards.Add(container.transform.GetChild(s).gameObject);
            }
           
        }
    }
    public class DataCards
    {
        public GameObject card { get; set; }
        public float distance { get; set; }
    }
    private class sort : IComparer<DataCards>
    {
        int IComparer<DataCards>.Compare(DataCards _objA, DataCards _objB)
        {
            float t1 = _objA.distance;
            float t2 = _objB.distance;
            return t1.CompareTo(t2);
        }
    }
    public float Svalue;
    public float Fvalue;
    public GameObject cardSem;
    public void ChangeWidth(GameObject main, float finalValue)
    {
        cardSem = main;
        Fvalue = finalValue; // Lets start defining hte variable
        Svalue = main.GetComponent<LayoutElement>().preferredWidth;
        InvokeRepeating("RepeatChange", 0.02f, 0.02f);
    }
    public void RepeatChange()
    {
        if (Svalue >= Fvalue)
        {
            CancelInvoke("RepeatChange");
        }
        else
        {
            Svalue += Fvalue / 2;
            if(cardSem)
            cardSem.GetComponent<LayoutElement>().preferredWidth = Svalue;
        }
    }
    public void DestroyEmptyCard()
    {
        if (GameObject.Find("Empty"))
        {
            GameObject main = GameObject.Find("Empty");
            cardSem = main;
            Fvalue = 0; // Lets start defining hte variable
            Svalue = main.GetComponent<LayoutElement>().preferredWidth;
            InvokeRepeating("RepeatChanges", 0.02f, 0.02f);
        }
    }
    public void RepeatChanges()
    {
        if (Svalue <= 0)
        {
            CancelInvoke("RepeatChanges");
        }
        else
        {
            
            Svalue -= Fvalue / 2;
            if (cardSem)
                cardSem.GetComponent<LayoutElement>().preferredWidth = Svalue;
           
        }
    }
    public void OnDrag(PointerEventData data)
    {
		if(inside==true){
            Vector3 value = data.position - ContainerPos;
            if (Mathf.Abs(value.y) > 125)
            {
                this.gameObject.transform.SetParent(this.gameObject.transform.parent.parent.parent);
                this.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
                inside = false;
            }
            else
            {
                cards.SendMessage("OnDrag",data);
            }
        }
        else
        {
            Vector3 position1 = data.position;
            position1.z = 21;
            main.gameObject.transform.GetComponent<RectTransform>().position = GameObject.Find("Camera").GetComponent<Camera>().ScreenToWorldPoint(position1);
            List<DataCards> dataAll = new List<DataCards>();
            for (int l = 0; l < allCards.Count; l++)
            {
                float distance = Vector3.Distance(transform.position, allCards[l].transform.position);
                DataCards dC = new DataCards();
                dC.card = allCards[l];
                dC.distance = distance;
                dataAll.Add(dC);
            }
            dataAll.Sort((IComparer<DataCards>)new sort());
            if (dataAll!=null)
            {
                GameObject first = dataAll[0].card;
                GameObject second = dataAll[1].card;
                int index = first.gameObject.transform.GetSiblingIndex();
                if (GameObject.Find("Empty"))
                {
                    if (GameObject.Find("Empty").transform.GetSiblingIndex() == (index+1))
                    {
            
                    }
                    else
                    {
                        GameObject.Find("Empty").transform.SetSiblingIndex(index);
                        GameObject.Find("Empty").GetComponent<LayoutElement>().preferredWidth = 114.86f;
                        ChangeWidth(GameObject.Find("Empty"), 230);
                    }
                }
                else
                {
                    GameObject EmptyCard = Instantiate(EmptyPrefab);
                    EmptyCard.transform.SetParent(container.transform);
                    EmptyCard.gameObject.name = "Empty";
                    EmptyCard.transform.SetSiblingIndex(index);
                    ChangeWidth(EmptyCard, 230);
                }
            }
            else
            {
                 int index = container.transform.childCount - 1;
                if (dataAll[0].card.name == container.transform.GetChild(index).gameObject.name)
                {
                    // Its the last card
                    if (!GameObject.Find("Empty"))
                    {
                        GameObject EmptyCard = Instantiate(EmptyPrefab);
                        EmptyCard.transform.SetParent(container.transform);
                        EmptyCard.gameObject.name = "Empty";
                        EmptyCard.transform.SetAsLastSibling();
                        ChangeWidth(EmptyCard, 220);
                    }
                    else
                    {
                        GameObject.Find("Empty").transform.SetAsLastSibling();
                        // There already exists Empty Card
                    }
                }
                else
                {
                    // This means it should be first card
                    if (!GameObject.Find("Empty"))
                    {
                        if (dataAll[0].card.name == container.transform.GetChild(0).gameObject.name)
                        {
                            GameObject EmptyCard = Instantiate(EmptyPrefab);
                            EmptyCard.transform.SetParent(container.transform);
                            EmptyCard.gameObject.name = "Empty";
                            EmptyCard.transform.SetAsFirstSibling();
                            ChangeWidth(EmptyCard, 230);
                        }
                    }
                }
            }
        }        
    }
    public void OnEndDrag(PointerEventData data)
    {
        if (main.transform.parent.gameObject.name != "ThrowingCase")
        {
			float fromcase =  Vector2.Distance(throwed_case.transform.position, this.gameObject.transform.position);
            float fromCards = Vector2.Distance(cards.transform.position, this.gameObject.transform.position);
            if (fromcase < fromCards)
            {
                // TODO: ANIMATE THIS THING HERE
                if (main.gameObject.name == "Freeze")
                {
                    if (GameObject.Find("BlackBackground"))
                    {
						//Stop the monk if the smashing monk has apeeared
                        main.transform.SetParent(throwed_case.transform);
                        Destroy(main.GetComponent<LayoutElement>());
                        EventHandles.GetComponent<Events>().isSmashingMonk = false;
                        GameObject.Find("BlackBackground").SetActive(false);
						EventHandles.GetComponent<Events>().PlaceSmashingMonk();
                    }
					EventHandles.GetComponent<Events>().CanceltheTimerInvoke(); // Cancel the TImerInvoke For Reintalization
					EventHandles.GetComponent<Events>().StartTheTimer(); // Start the TimerInvoke again
		 }
                else
                {
                    if (GameObject.Find("BlackBackground"))
                    {
						// If card other than Freeze card is passed to the placeholder. Just Return it back
                        Debug.Log("DOING THIS");
                        // Do nothing
                        main.transform.SetParent(container.transform);
                        if (GameObject.Find("Empty"))
                            main.transform.SetSiblingIndex(GameObject.Find("Empty").transform.GetSiblingIndex());
                        DestroyImmediate(GameObject.Find("Empty"));
                        Vector3 positionSe = this.gameObject.GetComponent<RectTransform>().position;
                        positionSe.z = 1;
                        this.gameObject.GetComponent<RectTransform>().localPosition = positionSe;
                        main.GetComponent<CanvasGroup>().blocksRaycasts = true; // Lets make it Default as true
                        inside = true;
                    }
                    else
                    {
						if(main.gameObject.name=="Freeze"){
							// THis means the user have given this card without any reason. Just return it back
							Debug.Log("DOING THIS");
							// Do nothing
							main.transform.SetParent(container.transform);
							if (GameObject.Find("Empty"))
								main.transform.SetSiblingIndex(GameObject.Find("Empty").transform.GetSiblingIndex());
							DestroyImmediate(GameObject.Find("Empty"));
							Vector3 positionSe = this.gameObject.GetComponent<RectTransform>().position;
							positionSe.z = 1;
							this.gameObject.GetComponent<RectTransform>().localPosition = positionSe;
							main.GetComponent<CanvasGroup>().blocksRaycasts = true; // Lets make it Default as true
							inside = true;
						}
						else{
                        if (EventHandles.GetComponent<Events>().isSteal == true && EventHandles.GetComponent<Events>().currentlyGotSteal == true )
                        {
							// If i am in the steal mode, means i have to give a card to other just send the selected card to him.

                            EventHandles.GetComponent<Events>().SendMonkaCard(main.gameObject);
							EventHandles.GetComponent<Events>().StopMyChances();
							EventHandles.GetComponent<Events>().animationHand.SetActive(false);
                            // This is only to give cards
                            EventHandles.GetComponent<Events>().isSteal = false;
							EventHandles.GetComponent<Events>().currentlyGotSteal = false;
                            Destroy(main.GetComponent<LayoutElement>());
							if(GameObject.Find("Empty"))
                            	DestroyImmediate(GameObject.Find("Empty"));
                            
                        }
                        else{
                            if (this.gameObject.name == "Shuffle")
                            {
								// If the dragged card is Shuffle just shuffle the cardDeck
                                main.transform.SetParent(throwed_case.transform);
                                Destroy(main.GetComponent<LayoutElement>());
                                Destroy(GameObject.Find("Empty"));
                                EventHandles.GetComponent<Events>().ShuffleCard();
                            }
                            if (this.gameObject.name == "Skip")
                            {
								// If the dragged Card is Skip just skip your one turn
                                EventHandles.GetComponent<Events>().CanceltheTimerInvoke();
                                main.transform.SetParent(throwed_case.transform);
                                Destroy(main.GetComponent<LayoutElement>());
                                Destroy(GameObject.Find("Empty"));
                                EventHandles.GetComponent<Events>().TurnEvent();
                            }
                            if (this.gameObject.name == "Futurex3")
                            {
								// If The dragged card is Future(x3) just show it in the future scene
                                main.transform.SetParent(throwed_case.transform);
                                Destroy(main.GetComponent<LayoutElement>());
                                Destroy(GameObject.Find("Empty"));
                                List<string> topCards = new List<string>();
                                GameObject cardDeck = GameObject.Find("CardDeck");
                                topCards.Add(cardDeck.transform.GetChild(cardDeck.transform.childCount - 1).gameObject.name); // Add toppest card
                                topCards.Add(cardDeck.transform.GetChild(cardDeck.transform.childCount - 2).gameObject.name); // Add second card
                                topCards.Add(cardDeck.transform.GetChild(cardDeck.transform.childCount - 3).gameObject.name); // Add third card
                                EventHandles.GetComponent<Events>().SeeFuture(topCards);
                            }
                            if (this.gameObject.name == "Futurex1")
                            {
								// If the dragged card is Future(x1) just show them in the future scene
                                main.transform.SetParent(throwed_case.transform);
                                Destroy(main.GetComponent<LayoutElement>());
                                Destroy(GameObject.Find("Empty"));
                                List<string> topCards = new List<string>();
                                GameObject cardDeck = GameObject.Find("CardDeck");
                                topCards.Add(cardDeck.transform.GetChild(cardDeck.transform.childCount - 1).gameObject.name); // Add toppest card
                                EventHandles.GetComponent<Events>().SeeFuture(topCards);
                            }
                            if (this.gameObject.name == "Futurex5")
                            {
								// If the dragged card is Future(x5) just show them the 5 future cards in future scene
                                main.transform.SetParent(throwed_case.transform);
                                Destroy(main.GetComponent<LayoutElement>());
                                Destroy(GameObject.Find("Empty"));
                                List<string> topCards = new List<string>();
                                GameObject cardDeck = GameObject.Find("CardDeck");
                                topCards.Add(cardDeck.transform.GetChild(cardDeck.transform.childCount - 1).gameObject.name); // Add toppest card
                                topCards.Add(cardDeck.transform.GetChild(cardDeck.transform.childCount - 2).gameObject.name); // Add second card
                                topCards.Add(cardDeck.transform.GetChild(cardDeck.transform.childCount - 3).gameObject.name); // Add third card
                                topCards.Add(cardDeck.transform.GetChild(cardDeck.transform.childCount - 4).gameObject.name); // Add fourth card
                                topCards.Add(cardDeck.transform.GetChild(cardDeck.transform.childCount - 5).gameObject.name); // Add fifth card
                                EventHandles.GetComponent<Events>().SeeFuture(topCards);
                            }
                            if (this.gameObject.name == "Attackx1")
                            {
								// If the dragged car is Attack(x1) just ask the user the player to attack on the  desired player.... Meanwhile don't allow him to play any other card
                                EventHandles.GetComponent<Events>().isAttackMode = true;
                                EventHandles.GetComponent<Events>().turnForAttack = 1;
								EventHandles.GetComponent<Events>().myChance = false;
								EventHandles.GetComponent<Events>().ShowArrows();
								EventHandles.GetComponent<Events>().StopMyChance();

                            }
                            if (this.gameObject.name == "Attackx2")
                            {
								// If the dragged car is Attack(x2) just ask the user the player to attack on the  desired player.... Meanwhile don't allow him to play any other card
                                EventHandles.GetComponent<Events>().isAttackMode = true;
                                EventHandles.GetComponent<Events>().turnForAttack = 2;
								EventHandles.GetComponent<Events>().myChance = false;
								EventHandles.GetComponent<Events>().ShowArrows();
								EventHandles.GetComponent<Events>().StopMyChance();
                            }
                            if (this.gameObject.name == "Attackx3")
                            {
								// If the dragged car is Attack(x3) just ask the user the player to attack on the  desired player.... Meanwhile don't allow him to play any other card
                                EventHandles.GetComponent<Events>().isAttackMode = true;
                                EventHandles.GetComponent<Events>().turnForAttack = 3;
								EventHandles.GetComponent<Events>().myChance = false;
								EventHandles.GetComponent<Events>().ShowArrows();
								EventHandles.GetComponent<Events>().StopMyChance();
                            }
                            if (this.gameObject.name == "WhatTheDuck")
                            {
								// If the dragged card is what the duck. Ask the user to select his desired player..... Meanwhile stop him to do any activity related to cards
                                EventHandles.GetComponent<Events>().isWhattheDuck = true;
								EventHandles.GetComponent<Events>().myChance = false;
								EventHandles.GetComponent<Events>().ShowArrows();
			
                            }
                            if (this.gameObject.name == "Reverse")
                            {
								// If the dragged card is reverse just reverse the turn sequence
                                EventHandles.GetComponent<Events>().ReverseTurn();
                            }
                            if (this.gameObject.name == "Steal")
                            {
								// Here is somebug so i am not describing it now.
                                EventHandles.GetComponent<Events>().isSteal = true;
								EventHandles.GetComponent<Events>().myChance = false;
								EventHandles.GetComponent<Events>().ShowArrows();
								EventHandles.GetComponent<Events>().StopMyChance();
								
								// Lets do something
                            }
                            Debug.Log("DOING THIS");
                            main.transform.SetParent(throwed_case.transform);
                            Destroy(main.GetComponent<LayoutElement>());
                            Destroy(GameObject.Find("Empty"));
                        }
                    }
				}
                }
				EventHandles.GetComponent<Events>().UpdateTotalCards(container.transform.childCount);
               // Lets just remove this
            }
            else
            {
                // TODO: ANIMATE THIS THING HERE
                main.transform.SetParent(container.transform);
                main.GetComponent<CanvasGroup>().blocksRaycasts = true;
                inside = true;
                List<DataCards> dataAll = new List<DataCards>();
                for (int l = 0; l < allCards.Count; l++)
                {
                    float distance = Vector3.Distance(transform.position, allCards[l].transform.position);
                    DataCards dC = new DataCards();
                    dC.card = allCards[l];
                    dC.distance = distance;
                    dataAll.Add(dC);
                }
                dataAll.Sort((IComparer<DataCards>)new sort());
                if ((dataAll[1].distance - dataAll[0].distance) < 0.1)
                {
                    if(main)
                        if(GameObject.Find("Empty"))
                             main.transform.SetSiblingIndex(GameObject.Find("Empty").transform.GetSiblingIndex());
                    Vector3 position = main.transform.position;
                    position.z = 1;
                    main.transform.position = position;
                    Destroy(GameObject.Find("Empty"));
                    Vector3 positionSe = this.gameObject.GetComponent<RectTransform>().position;
                    positionSe.z = 1;
                    this.gameObject.GetComponent<RectTransform>().localPosition = positionSe;
                }
                else
                {
                    int index = container.transform.childCount - 1;
                    if (dataAll[0].card.name == container.transform.GetChild(index).gameObject.name)
                    {
                        // Its the last card
                        if (!GameObject.Find("Empty"))
                        {
                            GameObject EmptyCard = Instantiate(EmptyPrefab);
                            EmptyCard.transform.SetParent(container.transform);
                            EmptyCard.gameObject.name = "Empty";
                            EmptyCard.transform.SetAsFirstSibling();
                        }
                        else
                        {
                            // THere already exists Empty Card
                            main.transform.SetAsLastSibling();
                            Destroy(GameObject.Find("Empty"));
                            Vector3 positionSe = this.gameObject.GetComponent<RectTransform>().position;
                            positionSe.z = 1;
                            this.gameObject.GetComponent<RectTransform>().localPosition = positionSe;
                        }
                    }
                    else
                    {
                        // This means it should be first card
                        if (!GameObject.Find("Empty"))
                        {
                            GameObject EmptyCard = Instantiate(EmptyPrefab);
                            EmptyCard.transform.SetParent(container.transform);
                            EmptyCard.gameObject.name = "Empty";
                            EmptyCard.transform.SetAsFirstSibling();
                            ChangeWidth(EmptyCard, 230);
                        }
                        else
                        {
                            main.transform.SetAsFirstSibling();
                            Destroy(GameObject.Find("Empty"));
                            Vector3 positionSe = this.gameObject.GetComponent<RectTransform>().position;
                            positionSe.z = 1;
                            this.gameObject.GetComponent<RectTransform>().localPosition = positionSe;
                        }

                    }
                }
            }
        }
        cards.SendMessage("OnEndDrag", data);
    }
}
