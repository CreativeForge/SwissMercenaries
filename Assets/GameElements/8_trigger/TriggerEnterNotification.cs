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

	public override void OnPlayerEnter( LogicPlayer logicPlayer, Collider col ) {
		// AddNotification ("[player.death]", logicPlayer.gameObject.transform.position, NotificationPriority.ThrowAwayAfterProcessing);
		if (triggerType == TriggerType.OnEnter) {
			CheckIfPickupItem(logicPlayer);
			AddNotification (NotificationName, NotificationArgument, GetReleasePosition ( logicPlayer ), NotificationPriority.ThrowAwayAfterProcessing);
		}
	}

		public override void OnPlayerFirstEnter( LogicPlayer logicPlayer, Collider col ) {
			// AddNotification ("[player.death]", logicPlayer.gameObject.transform.position, NotificationPriority.ThrowAwayAfterProcessing);
			if (triggerType == TriggerType.OnFirstEnter)
			{
				AddNotification (NotificationName, NotificationArgument, GetReleasePosition ( logicPlayer ), NotificationPriority.ThrowAwayAfterProcessing);
			}
		}
	
	public override void OnPlayerExit(  LogicPlayer logicPlayer, Collider col ) {
		if (triggerType == TriggerType.OnExit) {
			AddNotification (NotificationName, NotificationArgument, GetReleasePosition ( logicPlayer ), NotificationPriority.ThrowAwayAfterProcessing);
		}
	}

	public override void OnPlayerFirstExit(  LogicPlayer logicPlayer, Collider col ) {
		if (triggerType == TriggerType.OnFirstExit) {
			AddNotification (NotificationName, NotificationArgument, GetReleasePosition ( logicPlayer ) , NotificationPriority.ThrowAwayAfterProcessing);
		}
	}

	public Vector3 GetReleasePosition ( LogicPlayer logicPlayer ) {

		if ( notifcationPoint == TriggerNotificationPoint.Player ) {
			// return logicPlayer.gameObject.transform.position;
		}
		 
		return transform.position;
	}

	// Checks if item is pickup item and updates player stats
	private void CheckIfPickupItem(LogicPlayer logicPlayer)
	{
		/*
		PickupItem item = this.GetComponent<PickupItem>();
		if (item != null)
		{
			logicPlayer.UpdateStats(item.collectHealth, item.collectFaith, item.collectMoney);
			if(GetComponent<SaveCheckpoint>() != null) {
				GetComponent<SaveCheckpoint>().Save(transform.position);
			}
			Destroy(gameObject);
		}
		*/
	}

}
