using UnityEngine;
using System.Collections;

public class OnAnyKeyNotification : GameElementBased {

	// Update is called once per frame
	void Update () {
		// any key
		if (Input.anyKeyDown) {
			// load next level ...
			string notifications = gameElement.argument;
			string[] arrNotifications = notifications.Split(',');
			for (int x=0;x<arrNotifications.Length;x++) {
				string notifcationTypeSubType = arrNotifications[x];
				AddNotification( notifcationTypeSubType, gameElement.target, gameElement.timed, gameElement.argumentsub );
			}
		}
	}
}
