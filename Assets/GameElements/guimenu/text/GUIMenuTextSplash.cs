using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIMenuTextSplash : GameElementBased {

	bool started = false;

	public Text scriptText; 
	public Text scriptTextShadow; 

	float timer = 0.0f;
	void Start () {
		timer = Time.time;
	}

	// Update is called once per frame
	void FixedUpdate () {

		if (gameElement!=null) {
			if (!started) {
				started = true;
				UpdateText();
			} else {
				float alpha = Mathf.Abs(Mathf.Sin(Time.time-timer*10.0f));
				scriptText.color = new Color(1.0f,1.0f,1.0f, alpha); // a = 0.5f; // Mathf.Sin(Time.time-timer);
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
