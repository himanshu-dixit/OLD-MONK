using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
public class CardDeckDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IPointerUpHandler{

    public GameObject handler;
    public GameObject main;
    public GameObject smasingMonkScreen;
    public GameObject cardDeck;
    public GameObject infoGa;
    public List<GameObject> allCards;
	public GameObject cardsInHands;
	void Start () {
        handler = GameObject.Find("_HANDLER");
        main = this.gameObject;
	}

    public void OnBeginDrag(PointerEventData data)
    {
        main.transform.SetParent(main.transform.parent.parent); //Set it to parent
    }
    public void OnDrag(PointerEventData data)
    {
        Vector3  se =  GameObject.Find("Camera").GetComponent<Camera>().ScreenToWorldPoint(data.position);
        se.z = 1;
        main.gameObject.transform.GetComponent<RectTransform>().position = se; // Position it
    }
    public void AutomaticDrag()
    {
        if (this.gameObject.name == "SmasingMonk")
        {
            smasingMonkScreen.SetActive(true);
            int indexSib = this.gameObject.transform.GetSiblingIndex();
            handler.GetComponent<Events>().isSmashingMonk = true;
            infoGa.GetComponent<NumberOfCardsDeck>().UpdateCards(this.gameObject);
			handler.GetComponent<Events>().TurnEvent();
        }
        else
        {
            Debug.Log("YES");
            string name = this.gameObject.name;
            GameObject final = null;
            for (int o = 0; o < allCards.Count; o++)
            {
                if (allCards[o].name == name)
                {
                    final = allCards[o];
                    break;
                }
            }
			// Fix this
			int indexSib = this.transform.childCount-1; // THE Top one
			handler.GetComponent<Events>().deleteOtherCard(indexSib);
			handler.GetComponent<Events>().AddFromDeck(final);
            if (cardDeck.gameObject.transform.GetChild(indexSib - 3))
            {
                cardDeck.gameObject.transform.GetChild(indexSib - 3).gameObject.SetActive(true);
            }
            else if (cardDeck.gameObject.transform.GetChild(indexSib - 1))
                cardDeck.gameObject.transform.GetChild(indexSib - 1).gameObject.SetActive(true);
            Destroy(this.gameObject);
            cardDeck.GetComponent<NumberOfCardsDeck>().UpdateCards(this.gameObject);
        }
    }
	public void OnPointerUp(PointerEventData data)
    {
		/*
		 * I am using this event for now, just fix it later 
		 */
        if (this.gameObject.name == "SmasingMonk")
        {
            smasingMonkScreen.SetActive(true);
            int indexSib = this.gameObject.transform.GetSiblingIndex();
            handler.GetComponent<Events>().isSmashingMonk = true;
			this.transform.SetParent(cardDeck.transform);
            infoGa.GetComponent<NumberOfCardsDeck>().UpdateCards(this.gameObject);
			handler.GetComponent<Events>().TurnEvent();
        }
        else
        {
            Debug.Log("YES");
            string name = this.gameObject.name;
            GameObject final = null;
            for (int o = 0; o < allCards.Count; o++)
            {
                if (allCards[o].name == name)
                {
                    final = allCards[o];
                    break;
                }
            }
			int indexSib = this.transform.childCount-1; // THE Top one
			handler.GetComponent<Events>().deleteOtherCard(indexSib);
			handler.GetComponent<Events>().AddFromDeck(final);
            Destroy(this.gameObject);
            cardDeck.GetComponent<NumberOfCardsDeck>().UpdateCards(this.gameObject);
        }
		handler.GetComponent<Events> ().UpdateTotalCards (cardsInHands.transform.childCount);
    }
}