using UnityEngine;
using System.Collections;
using GameLab.NotficationCenter;

/*
 * exit now ... 
 * 
 * */


public class TriggerEnterExit : TriggerBase {

	float timeToGo = 0.0f;

	public override void OnPlayerFirstEnter( LogicPlayer logicPlayer, Collider col ) {

		// gameLogic.LoadGameNextLevel ();
		// gameLogic.AddNotification ("[level.done]", logicPlayer.gameObject.transform.position );

		timeToGo = Time.time + 2.0f; 
	
	}

	// delayed ... 
	public void FixedUpdate() {

		if (timeToGo!=0.0f) {
			// Debug.Log ("[EnterExit] "+(Time.time-timeToGo));
			if (  Time.time > timeToGo ) {

				gameLogic.LoadGameNextLevel ();
			}
		}

	}

}
