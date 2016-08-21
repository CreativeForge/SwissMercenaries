using UnityEngine;
using System.Collections;

public class ChallengeKillOne : GUIMenuText {

	bool started = false;
	string nameToFind = "xyz";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// found?
		if (started) {
			// in action
			if (CheckState("")) {
				// done
				bool done = false;
				GameObject obj = GameObject.Find(nameToFind);
				if (obj!=null) {
					// still doing
					if (obj.transform.childCount==0) {
						done = true;
					}
				} else {
					// check if it is a just a gameobject without gameobjects
					done = true;
				}
				if (done) {
					// done
					scriptText.text = gameLogic.levelEditor.ParseText(""+gameElement.argumentsub)+" FOUND! ";
					AddNotification( "ingame.won", "PLAYER", 0.0f, "" );
					AddNotification( "level.next", "PLAYER", 5.0f, "" );
					// load next level ... 
				}
			}
		}
	}

	public override void OnGameStart() {
		started = true;
		nameToFind = gameElement.argument;
		UpdateText();
	}


	void UpdateText() {
		scriptText.text = gameLogic.levelEditor.ParseText(""+gameElement.argumentsub+" FIND: "+nameToFind);
	}

}
