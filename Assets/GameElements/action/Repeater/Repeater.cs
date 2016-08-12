using UnityEngine;
using System.Collections;

public class Repeater : GameElementBased {

	// repeater
	float interval = 3.0f;
	float timerNextAction = 0.0f;

	// Update is called once per frame
	void Update () {

		RepeaterUpdater();

		if (gameElement!=null) {
			// let's start ...
			if (Time.time>timerNextAction) {
				RepeatedAction();
				timerNextAction = Time.time + interval;
			}
			if (!gameElement.argumentsub.Equals("")) {
				try {
					interval = float.Parse(gameElement.argumentsub);
				} catch {
					interval = 3.0f;
				}
			}
		}

	}

	public virtual void  RepeatedAction() {
		// Debug.Log("Repeater.RepeatedAction()");
	}

	public virtual void  RepeaterUpdater() {
		// Debug.Log("Repeater.RepeatedAction()");
	}

}
