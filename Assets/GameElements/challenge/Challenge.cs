using UnityEngine;
using System.Collections;

public class Challenge : GUIMenuText {

	float startTime = 0.0f;
	float timeLeft = 60.0f;
	float timed  = 0.0f;
	// bool startedNow = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		UpdateText();
	}

	public override void OnGameStart() {
		// Debug.Log("GUIMEnu.OnGameStart()");
		startTime = Time.time;
		try {
			timeLeft = float.Parse(gameElement.argument);
		} catch {
				
		}
		UpdateText();
	}


	void UpdateText() {
		scriptTextShadow.text = ""; // gameLogic.levelEditor.ParseText(""+gameElement.argumentsub+"/"+);
		if (CheckState("")) {
			timed = timeLeft+startTime - Time.time;
			int timedTime = (int) timed;
			if (timedTime<0) timedTime = 0;
			scriptText.text = gameLogic.levelEditor.ParseText(""+gameElement.argumentsub) +"\n"+ timedTime;
			if (timed<0.0f) {
				// kill him!!!
				AddNotification( "player.die", "PLAYER", 0.0f, "" );
			}
		}
	}

}
