using UnityEngine;
using System.Collections;

public class RepeaterActivatorMorph : GameElementBased {

	int index = 0;

	/*
	 * Repeater
	 * */
	public Transform startMarker;
	public Transform endMarker;
	public float speed = 1.0F;
	private float startTime;
	private float journeyLength;

	void StartMorph() {
		startTime = Time.time;
		journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
	}

	void UpdateMorph() {
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
		Debug.Log("RepeaterActivatorMorph.UpdateMorph() journeyLength="+journeyLength);
	}

	string actualName = "";
	string nextName = "";
	GameElement actualGameElement;

	public void ReachedNextName() {
		Debug.Log("RepeaterActivatorMorph.ReachedNextName()");

		// activate random out of a list !
		string names = gameElement.argument;
		string[] arrNames = names.Split(',');

		string activeName = arrNames[index];
		Debug.Log("RepeaterActivatorMorph.ReachedNextName() // activeName = "+activeName);
		if (!activeName.Equals("")) {
			actualName = activeName;
			// get next ...
			if ((index+1)<arrNames.Length) {
				nextName = arrNames[index+1];
			} else {
				nextName = arrNames[0];
			}
			Debug.Log("RepeaterActivatorMorph.ReachedNextName() // nextName = "+nextName);
			/*
			for (int x=0;x<arrNames.Length;x++) {
				string todeactivate = arrNames[x];
				GameElement ga = gameLogic.levelEditor.GetGameElementByName(todeactivate);
				//				gameLogic.levelEditor.DeactivateElement(ga);
				// gameLogic.levelEditor.DeactivateElement(ga);
				if (activeName.Equals(todeactivate)) {
					Debug.Log("RepeaterActivatorMorph.ReachedNextName() // activate activeName = "+activeName);
				//	gameLogic.levelEditor.ActivateElement(ga);
					actualGameElement = ga;
				}	
			}
			*/
		}
		index++;
		if (index>(arrNames.Length-1)) {
			index = 0;
		}
		actualGameElement = gameLogic.levelEditor.GetGameElementByName(actualName);
		startMarker = actualGameElement.gameObject.transform;
		GameElement gax = gameLogic.levelEditor.GetGameElementByName(nextName);
		GameObject gaxObj = gax.gameObject;
// problem!!
		// endMarker = new GameObject().transform;
		// endMarker.position = new Vector3(gax.position.x,gax.position.y,gax.position.z);
		endMarker = gaxObj.transform;

	}

	bool started = false;
	void Update() {
		// Debug.Log("Repeater.RepeatedAction()");
		if (!started) {
			if (gameElement!=null) {
				ReachedNextName();
				started = true;
				StartMorph();
			}
		}
		if (started) {
			UpdateMorph();
		}
	}




}
