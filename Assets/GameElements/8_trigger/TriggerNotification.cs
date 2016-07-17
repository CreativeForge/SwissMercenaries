using UnityEngine;
using System.Collections;
using GameLab.NotficationCenter;

public class TriggerNotification : TriggerBase {



	public override void  OnInitTrigger() {
		Debug.Log("TriggerNotifcation.OnInitTrigger() // "+gameElement.name+"/"+gameElement.strevent);
	}

	public override void OnPlayerEnter( PlayerScript PlayerScript, Collider col ) {
		
	}

	public override void OnPlayerFirstEnter( PlayerScript PlayerScript, Collider col ) {
		
	}

	public override void OnPlayerExit(  PlayerScript PlayerScript, Collider col ) {


	}

	public override void OnPlayerFirstExit(  PlayerScript PlayerScript, Collider col ) {
	

	}

}
