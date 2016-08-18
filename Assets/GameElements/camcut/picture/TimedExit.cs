using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimedExit : GameElementBasedTimed {

	public override void OnActivateGameElementTimed() {
		Debug.Log("TimedExit.OnActivateGameElementTimed() ");
		gameLogic.levelEditor.LoadNextInGameLevel();
	}


}
