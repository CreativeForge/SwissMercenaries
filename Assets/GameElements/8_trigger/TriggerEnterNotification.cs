using UnityEngine;
using System.Collections;
using GameLab.NotficationCenter;

public class TriggerEnterNotification : TriggerBase {

	public TriggerType triggerType = TriggerType.OnEnter;
	public TriggerNotificationPoint notifcationPoint = TriggerNotificationPoint.Player;

	// notification point

	// public string triggerName="noname";


	string notificationKey = "[player.death]";
	//string notificationArgument = "";

	public override void OnPlayerEnter( PlayerScript PlayerScript, Collider col ) {
		// AddNotification ("[player.death]", PlayerScript.gameObject.transform.position, NotificationPriority.ThrowAwayAfterProcessing);
		if (triggerType == TriggerType.OnEnter) {
			CheckIfPickupItem(PlayerScript);
			AddNotification (NotificationName, NotificationArgument, GetReleasePosition ( PlayerScript ), NotificationPriority.ThrowAwayAfterProcessing);
		}
	}

		public override void OnPlayerFirstEnter( PlayerScript PlayerScript, Collider col ) {
			// AddNotification ("[player.death]", PlayerScript.gameObject.transform.position, NotificationPriority.ThrowAwayAfterProcessing);
			if (triggerType == TriggerType.OnFirstEnter)
			{
				AddNotification (NotificationName, NotificationArgument, GetReleasePosition ( PlayerScript ), NotificationPriority.ThrowAwayAfterProcessing);
			}
		}
	
	public override void OnPlayerExit(  PlayerScript PlayerScript, Collider col ) {
		if (triggerType == TriggerType.OnExit) {
			AddNotification (NotificationName, NotificationArgument, GetReleasePosition ( PlayerScript ), NotificationPriority.ThrowAwayAfterProcessing);
		}
	}

	public override void OnPlayerFirstExit(  PlayerScript PlayerScript, Collider col ) {
		if (triggerType == TriggerType.OnFirstExit) {
			AddNotification (NotificationName, NotificationArgument, GetReleasePosition ( PlayerScript ) , NotificationPriority.ThrowAwayAfterProcessing);
		}
	}

	public Vector3 GetReleasePosition ( PlayerScript PlayerScript ) {

		if ( notifcationPoint == TriggerNotificationPoint.Player ) {
			// return PlayerScript.gameObject.transform.position;
		}
		 
		return transform.position;
	}

	// Checks if item is pickup item and updates player stats
	private void CheckIfPickupItem(PlayerScript PlayerScript)
	{
		/*
		PickupItem item = this.GetComponent<PickupItem>();
		if (item != null)
		{
			PlayerScript.UpdateStats(item.collectHealth, item.collectFaith, item.collectMoney);
			if(GetComponent<SaveCheckpoint>() != null) {
				GetComponent<SaveCheckpoint>().Save(transform.position);
			}
			Destroy(gameObject);
		}
		*/
	}

}
