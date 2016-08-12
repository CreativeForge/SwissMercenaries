using UnityEngine;
using System.Collections;

public class RepeaterRandomActive : Repeater {

	public override void RepeatedAction() {
		Debug.Log("RepeaterRandomActive.RepeatedAction()");

		// activate random out of a list !
		string names = gameElement.argument;
		string[] arrNames = names.Split(',');

		int randomIndex = UnityEngine.Random.Range(0,arrNames.Length);
		string activeName = arrNames[randomIndex];
		Debug.Log("RepeaterRandomActive.RepeatedAction() // activeName = "+activeName);
		if (!activeName.Equals("")) {
			for (int x=0;x<arrNames.Length;x++) {
				string todeactivate = arrNames[x];
				GameElement ga = gameLogic.levelEditor.GetGameElementByName(todeactivate);
//				gameLogic.levelEditor.DeactivateElement(ga);
				gameLogic.levelEditor.DeactivateElement(ga);
				if (activeName.Equals(todeactivate)) {
					Debug.Log("RepeaterRandomActive.RepeatedAction() // activate activeName = "+activeName);
					gameLogic.levelEditor.ActivateElement(ga);
				}	
			}
		}


	}


}
