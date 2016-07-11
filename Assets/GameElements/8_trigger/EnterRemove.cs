using UnityEngine;
using System.Collections;
using GameLab.NotficationCenter;

/*
 * exit now ... 
 * 
 * */


public class EnterRemove : TriggerBase {
	
	
	public override void OnPlayerFirstEnter( LogicPlayer logicPlayer, Collider col ) {
		
		// gameLogic.LoadGameNextLevel ();
		// gameLogic.AddNotification ("[level.done]", logicPlayer.gameObject.transform.position );
		
		// Debug.Log ("[EnterRemove] ARGUMENT: "+argument);
		// timeToGo = Time.time + 5.0f; 
		// activate now
		ArrayList arr = gameLogic.GetGameElementsByName ( NotificationName );
		// Debug.Log ("ARR: "+arr.Count);

		int counted = arr.Count;
		for (int i=counted-1; i>=0; i--) {
			GameElement gx=(GameElement) arr[i];
			gameLogic.RemoveGameElement(gx);
		}
	}
	
}

