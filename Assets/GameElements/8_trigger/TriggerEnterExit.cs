using UnityEngine;
using System.Collections;
using GameLab.NotficationCenter;

/*
 * exit now ... 
 * 
 * */


public class TriggerEnterExit : TriggerBase {

	float timeToGo = 0.0f;

	public override void OnPlayerFirstEnter( PlayerScript PlayerScript, Collider col ) {

		// gameLogic.LoadGameNextLevel ();
		// gameLogic.AddNotification ("[level.done]", PlayerScript.gameObject.transform.position );

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
