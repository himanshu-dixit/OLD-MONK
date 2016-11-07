using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
public class DistributeCards : MonoBehaviour {
    public GameObject CardDeck;
    public List<GameObject> allCards;
    public GameObject throwingCase;
    public GameObject emptyPrefab;
    // Use this for initialization
	void Start () {
        allCards = CardDeck.GetComponent<CardDeck>().allCards;
        GameObject newGame =  Instantiate(allCards[3]);
        newGame.gameObject.transform.SetParent(this.transform);
        newGame.name = allCards[3].name;
        newGame.GetComponent<Drag>().container = this.gameObject;
        newGame.GetComponent<Drag>().cards = this.gameObject.transform.parent.gameObject;
        newGame.GetComponent<Drag>().throwed_case = throwingCase;
        newGame.GetComponent<Drag>().EmptyPrefab = emptyPrefab;
        Vector3 position;
        position = newGame.transform.localPosition;
        position.z = 1;
        newGame.transform.localPosition = position;
        newGame.transform.localScale = new Vector3(1, 1, 1);
        int childCount = 0;
        List<int> allIndex = new List<int>();
        while (childCount != 3)
        {
            int index = Random.Range(0, allCards.Count);
            GameObject newGasme = allCards[index];
            if (newGasme.name != "Freeze" && newGasme.name != "SmasingMonk")
            {
                allIndex.Add(index);
                GameObject Allnew = Instantiate(newGasme);
                Allnew.gameObject.transform.SetParent(this.transform);
                Allnew.name = newGasme.name;
                Allnew.GetComponent<Drag>().container = this.gameObject;
                Allnew.GetComponent<Drag>().cards = this.gameObject.transform.parent.gameObject;
                Allnew.GetComponent<Drag>().throwed_case = throwingCase;
                Allnew.GetComponent<Drag>().EmptyPrefab = emptyPrefab;
                Vector3 positiosn;
                positiosn = Allnew.transform.localPosition;
                positiosn.z = 1;
                Allnew.transform.localPosition = positiosn;
                Allnew.transform.localScale = new Vector3(1, 1, 1);
                childCount++;
            }
        }
	}
}