using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimedLoop : GameElementBasedTimed {

	public override void OnActivateGameElementTimed() {
		Debug.Log("TimedLoop.OnActivateGameElementTimed() ");
		gameLogic.levelEditor.StartIngameTime();
	}


}
