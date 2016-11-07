using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Drop : MonoBehaviour,IDropHandler {
    public GameObject selectedG { get;set; }
    public GameObject EventHandler;
    public GameObject cardsContainer;
    public void CheckIfCardTurn(GameObject card)
    {
        EventHandler.GetComponent<Events>().sendThrow(card);
    }
    public void AddTothis(GameObject cardPrefab)
    {
        GameObject newT = Instantiate(cardPrefab);
        newT.transform.SetParent(this.transform);
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (selectedG) 
		{
            EventHandler.GetComponent<Events>().CanceltheTimerInvoke();
            if (GameObject.Find("BlackBackground"))
            {

            }
            else
            {
                selectedG.transform.SetParent(this.transform);
                Destroy(selectedG.GetComponent<LayoutElement>());
                CheckIfCardTurn(selectedG);
                EventHandler.GetComponent<Events>();
                selectedG.GetComponent<CanvasGroup>().blocksRaycasts = false;
                EventHandler.SendMessage("UpdateTotalCards", cardsContainer.transform.childCount);
            }
       
        }
        if (GameObject.Find("Empty"))
        {
            Destroy(GameObject.Find("Empty"));
        }
        else
        {

  
        }
    }
}