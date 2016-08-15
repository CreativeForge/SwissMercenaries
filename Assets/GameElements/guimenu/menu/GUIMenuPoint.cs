using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIMenuPoint : MonoBehaviour {

	public Text scriptText; 
	 
	public void SetText( string newrtftext ) {
		if (scriptText!=null) {
			scriptText.text = newrtftext;
		}
	}
}
