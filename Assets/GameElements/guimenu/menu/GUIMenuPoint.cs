using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GUIMenuPoint : MonoBehaviour {

	public Text scriptText; 
	// listeners in GUIMenu
	 
    // SetText
	public void SetText( string newrtftext ) {
		if (scriptText!=null) {
			scriptText.text = newrtftext;
		}
	}

	/*
	 * OnMouseOver
	 * 
	 * */
/*
	void Start() {
		InitCallBacks(UnityAction<GameObject> Enter, UnityAction<GameObject> Exit);
	}

	UnityAction<GameObject> OnPointerEnter;
	UnityAction<GameObject> OnPointerExit;

	public void InitCallBacks(UnityAction<GameObject> Enter, UnityAction<GameObject> Exit){
		OnPointerEnter = Enter;
		OnPointerExit = Exit;
	}

	void IPointerEnterHandler.OnPointerEnter (PointerEventData eventData){
		OnPointerEnter(this.gameObject);
	}
	void IPointerExitHandler.OnPointerExit (PointerEventData eventData){
		OnPointerExit(this.gameObject);
	}
*/
}
