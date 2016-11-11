using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class throw_card : MonoBehaviour, IDropHandler,IPointerEnterHandler,IPointerExitHandler {
	public void OnPointerEnter(PointerEventData data){
		//Debug.Log ("OnPointerEnter");
	}
	public void OnDrop(PointerEventData data){
		int total_child = this.transform.childCount;
		if (total_child > 2) {
			this.transform.GetChild (0).gameObject.SetActive(false);
			this.transform.GetChild (0).SetParent(this.transform.parent);
		
		}
	
		data.selectedObject.transform.SetParent (this.transform);
	}
	public void OnPointerExit(PointerEventData data){
		//Debug.Log ("OnPointerExit");
	}
}
