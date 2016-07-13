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

	public override void OnPlayerFirstEnter( PlayerScript PlayerScript, Collider col ) {

		// gameLogic.LoadGameNextLevel ();
		//gameLogic.AddNotification ("[level.done]", PlayerScript.gameObject.transform.position );

		// barrel.StartRolling();
	
	}


}
