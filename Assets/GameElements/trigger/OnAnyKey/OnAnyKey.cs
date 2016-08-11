using UnityEngine;
using System.Collections;

public class OnAnyKey : GameElementBased {


	
	// Update is called once per frame
	void Update () {
		// any key
		if (Input.anyKeyDown) {
			// load next level ...
			gameLogic.LoadGameNextLevel();
		}
	}
}
