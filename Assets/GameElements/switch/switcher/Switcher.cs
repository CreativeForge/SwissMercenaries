using UnityEngine;
using System.Collections;

public class Switcher : GameElementBased {
	
	bool firstTime = false;

	public GameObject gameObjectOn;
	public GameObject gameObjectOff;

	public override void OnPlayerHit() {
		Debug.Log("Switcher.OnPlayerHit()");

		if (gameElement!=null) {
			// Debug.Log("TriggerNotification().OnActivateTrigger() // TriggerType: "+triggerType+"/timed: "+gameElement.timed+"/target: "+gameElement.target+"/argument: "+gameElement.argument+"/argumentsub: "+gameElement.argumentsub);
			if (!firstTime) {
				// first time
				firstTime = true;
				if (gameObjectOn!=null) {
					gameObjectOn.SetActive(true);
				}
				if (gameObjectOff!=null) {
					gameObjectOff.SetActive(false);
				}
				// wood explosion
				AddNotification( "visual.woodexplosion", "self", 0.0f, "" );
				// do all notifications ... 
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
