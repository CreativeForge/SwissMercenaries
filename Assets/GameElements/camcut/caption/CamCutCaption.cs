using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CamCutCaption : GameElementBased {

	bool started = false;

	public Text scriptText; 
	public Text scriptTextShadow; 

	// float timer = 0.0f;
	void Start () {
		// timer = Time.time;
	}

	// Update is called once per frame
	void FixedUpdate () {

		if (gameElement!=null) {
			if (!started) {
				started = true;
				scriptText.text = ""+gameElement.timed; // gameElement.argument+"";
				scriptTextShadow.text = ""; // gameElement.argument;
			} else {
				/*
				scriptText.text = gameElement.argument+"";
				float alpha = Mathf.Abs(Mathf.Sin(Time.time-timer*10.0f));
				scriptText.color = new Color(1.0f,1.0f,1.0f, alpha); // a = 0.5f; // Mathf.Sin(Time.time-timer);
				*/
			}
		}
	}

	public override void OnGameStart() {
		Debug.Log("CamCutCaption.OnGameStart()");
		// get all captions now ...
		if (gameElement!=null) {
//			ArrayList arr = gameLogic.levelEditor.GetGameElementsByTypeAndSubTimeSorted(gameElement.type,gameElement.subtype);
			// ArrayList arr = gameLogic.levelEditor.GetAllGameElementsByTypeAndSubTimeSorted();
			// ok debug
//			Debug.Log("CamCutCaption.OnGameStart() // arr: "+arr.Count);
		}
	}
}
