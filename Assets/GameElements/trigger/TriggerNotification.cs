using UnityEngine;
using System.Collections;
using GameLab.NotficationCenter;

public class TriggerNotification : TriggerBase {
	
	public override void  OnInitTrigger() {
		// Debug.Log("TriggerNotifcation.OnInitTrigger() // "+gameElement.name+"/"+gameElement.strevent);
	}


	public override void OnActivateTrigger( PlayerScript PlayerScript, Collider col ) {

		if (gameElement!=null) {
			// Debug.Log("TriggerNotification().OnActivateTrigger() // TriggerType: "+triggerType+"/timed: "+gameElement.timed+"/target: "+gameElement.target+"/argument: "+gameElement.argument+"/argumentsub: "+gameElement.argumentsub);
			string notifications = gameElement.argument;
			string[] arrNotifications = notifications.Split(',');
			for (int x=0;x<arrNotifications.Length;x++) {
				string notifcationTypeSubType = arrNotifications[x];
				AddNotification( notifcationTypeSubType, gameElement.target, gameElement.timed, gameElement.argumentsub );
			}
		}


		// if (gameElement.)
	}


}
