using UnityEngine;
using System.Collections;
using GameLab.NotficationCenter;

public class OnStateNotification : GameElementBased {

	public override void OnChangeGameState (string newstate)
	{
		if (newstate.Equals(gameElement.strevent)) {
			if (gameElement!=null) {
				// Debug.Log("TriggerNotification().OnActivateTrigger() // TriggerType: "+triggerType+"/timed: "+gameElement.timed+"/target: "+gameElement.target+"/argument: "+gameElement.argument+"/argumentsub: "+gameElement.argumentsub);
				string notifications = gameElement.argument;
				string[] arrNotifications = notifications.Split(',');
				for (int x=0;x<arrNotifications.Length;x++) {
					string notifcationTypeSubType = arrNotifications[x];
					AddNotification( notifcationTypeSubType, gameElement.target, gameElement.timed, gameElement.argumentsub );
				}
			}
		}
	}


}
