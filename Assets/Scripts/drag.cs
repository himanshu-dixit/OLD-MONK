using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class drag : MonoBehaviour, IDragHandler {
	public void OnDrag(PointerEventData data){

		this.transform.position = data.position;
	}

}
