using UnityEngine;
using System.Collections;

public class TriggerOnNotification : TriggerBase {

		[Tooltip("Should something happen when some notification has happened x times? If so NotificationTrigger happens")]
		public bool triggerOnNotificationCount = true;
		[Tooltip("For which notifications should we look for.")]
		public string NotificationNeeded="";
		[Tooltip("How many such  notifications are needed.")]
		public int NotificationAmount=0;
		private int notificationCount=0;
		//[Tooltip("What trigger should happen, when the notifications were there.")]
		//public string TriggerAfterCount="";

	// Use this for initialization
	void Start () {
		// gameLogic.notificationCenter.RegisterListener (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void NotifyTrigger (string notification){
        //(print(notification);
        if (notification == NotificationNeeded) {
			notificationCount++;
			if(notificationCount==NotificationAmount){
				// gameLogic.AddNotification (""+NotificationName,NotificationArgument, this.gameObject.transform.position, GameLab.NotficationCenter.NotificationPriority.ThrowAwayAfterProcessing );
				// gameLogic.notificationCenter.UnRegisterListener (this);
			}
		}
	}
}
