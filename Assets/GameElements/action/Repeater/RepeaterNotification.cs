using UnityEngine;
using System.Collections;

public class RepeaterNotification : Repeater {

	public override void RepeatedAction() {
		// Debug.Log("RepeatedNotification.RepeatedAction()");

		string notifications = gameElement.argument;
		string[] arrNotifications = notifications.Split(',');
		for (int x=0;x<arrNotifications.Length;x++) {
			string notifcationTypeSubType = arrNotifications[x];
			AddNotification( notifcationTypeSubType, gameElement.target, gameElement.timed, gameElement.argumentsub );
		}

	}

}
