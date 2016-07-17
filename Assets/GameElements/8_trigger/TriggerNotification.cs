using UnityEngine;
using System.Collections;
using GameLab.NotficationCenter;

public class TriggerNotification : TriggerBase {



	public override void  OnInitTrigger() {
		// Debug.Log("TriggerNotifcation.OnInitTrigger() // "+gameElement.name+"/"+gameElement.strevent);
	}


	public override void OnActivateTrigger( PlayerScript PlayerScript, Collider col ) {

		Debug.Log("TriggerNotification().OnActivateTrigger() // TriggerType: "+triggerType);

		// if (gameElement.)
	}


}
