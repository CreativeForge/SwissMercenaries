﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimedNotification : GameElementBasedTimed {

	public override void OnActivateGameElementTimed() {

		string notifications = gameElement.argument;
		string[] arrNotifications = notifications.Split(',');
		for (int x=0;x<arrNotifications.Length;x++) {
			string notifcationTypeSubType = arrNotifications[x];
			AddNotification( notifcationTypeSubType, gameElement.target, gameElement.timed, gameElement.argumentsub );
		}

	}


}