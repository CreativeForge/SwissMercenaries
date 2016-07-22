using UnityEngine;
using System.Collections;
 
public class NotificationCommandBase : GameElementBased {



	public void OnGameStart() {
		DoNotificationCommand();
	}


	public void DoNotificationCommand() {
		// take things and add now ...
		Debug.Log("NotificationCommand.DoNotificationCommand()");
		Debug.Log("NotificationCommand.DoNotificationCommand() // "+gameLogic);
		// Do it now !!!
		if (gameElement!=null) {
			Debug.Log("NotificationCommandBase().OnActivateTrigger() // TriggerType: timed: "+gameElement.timed+"/target: "+gameElement.target+"/argument: "+gameElement.argument+"/argumentsub: "+gameElement.argumentsub);
			string notifications = gameElement.argument;
			string[] arrNotifications = notifications.Split(',');
			for (int x=0;x<arrNotifications.Length;x++) {
				string notifcationTypeSubType = arrNotifications[x];
				AddNotification( notifcationTypeSubType, gameElement.target, gameElement.timed, gameElement.argumentsub );
			}
		}

	}

}
