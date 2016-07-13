using UnityEngine;
using System.Collections;
using GameLab.NotficationCenter;

/*
 * exit now ... 
 * 
 * */


public class EnterActivate : TriggerBase {
	
	
	public override void OnPlayerFirstEnter( PlayerScript PlayerScript, Collider col ) {
		
		// gameLogic.LoadGameNextLevel ();
		// gameLogic.AddNotification ("[level.done]", PlayerScript.gameObject.transform.position );
		
		Debug.Log ("[EnterActivate] ARGUMENT: "+NotificationName);
		// timeToGo = Time.time + 5.0f; 
		// activate now
		ArrayList arr = gameLogic.GetGameElementsByName ( NotificationName );
		// Debug.Log ("ARR: "+arr.Count);

		// activate them 
		for (int i=0; i<arr.Count; i++) {
			GameElement gx=(GameElement) arr[i];
			// hidden yet ?
			if (gx.release.Equals ("wait")) {
				gx.release = "";
			}
		}

		gameLogic.AddGameElements (arr);
	}
	
}

