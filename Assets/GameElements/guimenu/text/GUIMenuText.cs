using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIMenuText : GameElementBased {

	bool started = false;

	public Text scriptText; 
	public Text scriptTextShadow; 

	// Update is called once per frame
	void FixedUpdate () {

		if (gameElement!=null) {
			if (!started) {
				started = true;
			} else {
				scriptText.text = gameElement.argument;
				scriptTextShadow.text = gameElement.argument;
			}
		}
	}
}
