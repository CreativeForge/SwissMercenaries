using UnityEngine;
using System.Collections;

public class SwitchNotification : GameElementBased {

	bool firstTime = false;

	public override void OnPlayerHit() {
		Debug.Log("SwitchNotification.OnPlayerHit()");

		if (gameElement!=null) {
			// Debug.Log("TriggerNotification().OnActivateTrigger() // TriggerType: "+triggerType+"/timed: "+gameElement.timed+"/target: "+gameElement.target+"/argument: "+gameElement.argument+"/argumentsub: "+gameElement.argumentsub);
			if (!firstTime) {
				string notifications = gameElement.argument;
				string[] arrNotifications = notifications.Split(',');
				for (int x=0;x<arrNotifications.Length;x++) {
					string notifcationTypeSubType = arrNotifications[x];
					AddNotification( notifcationTypeSubType, gameElement.target, gameElement.timed, gameElement.argumentsub );
				}
				firstTime = true;
			}
		}

	}

}
