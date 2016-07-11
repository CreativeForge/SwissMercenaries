using UnityEngine;
using System.Collections;
using GameLab.NotficationCenter;

/*
 * exit now ... 
 * 
 * */


public class TriggerEnterBarrel: TriggerBase {

	// Barrel barrel;

	void Start(){
		// barrel = transform.parent.GetComponentInChildren<Barrel>();
		
	}

	public override void OnPlayerFirstEnter( LogicPlayer logicPlayer, Collider col ) {

		// gameLogic.LoadGameNextLevel ();
		//gameLogic.AddNotification ("[level.done]", logicPlayer.gameObject.transform.position );

		// barrel.StartRolling();
	
	}


}
