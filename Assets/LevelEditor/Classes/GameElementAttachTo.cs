using UnityEngine;
using System.Collections;

public class GameElementAttachTo : GameElementBased {

	// ...
	bool initDone = false;
	// GameObject foundObj = null;

	public string attachToGameElementName = "PLAYER";

	void FixedUpdate () {

		// only ingame
		if (gameLogic.CheckIngameState()) {

			if (!initDone) {
				// search for ...
				GameElement gae = gameLogic.levelEditor.GetGameElementByName(""+attachToGameElementName);
				if (gae!=null) {
					if (gae.gameObject!=null) {
						this.gameObject.transform.parent = gae.gameObject.transform;
						this.gameObject.transform.localPosition = new Vector3();
						initDone = true;
					}
				}
			}
		}

	}
}
