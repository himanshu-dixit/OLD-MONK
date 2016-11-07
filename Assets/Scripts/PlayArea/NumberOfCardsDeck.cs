using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class NumberOfCardsDeck : MonoBehaviour {
    public GameObject container;
    public void UpdateCards(GameObject containerss)
    {
        this.gameObject.GetComponent<Text>().text = container.transform.childCount + "\n" + "Cards Left";
    }
}
