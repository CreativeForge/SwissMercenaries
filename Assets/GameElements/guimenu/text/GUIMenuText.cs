using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIMenuText : GameElementBased {

	// No parse Text 

	bool started = false;

	public Text scriptText; 
	public Text scriptTextShadow; 

	// Update is called once per frame
	void FixedUpdate () {

		if (gameElement!=null) {
			if (!started) {
				started = true;
				UpdateText();
			} else {
			}
		}
	}

	public override void OnGameStart() {
		// Debug.Log("GUIMEnu.OnGameStart()");
		UpdateText();
	}


	void UpdateText() {
		scriptTextShadow.text = gameLogic.levelEditor.ParseText(""+gameElement.argument);
		scriptText.text = gameLogic.levelEditor.ParseText(""+gameElement.argument);
	}

}
