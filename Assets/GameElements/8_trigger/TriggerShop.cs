using UnityEngine;
using System.Collections;
using GameLab.NotficationCenter;

public class TriggerShop : TriggerBase {

	public TriggerType triggerType = TriggerType.OnEnter;
	public TriggerNotificationPoint notifcationPoint = TriggerNotificationPoint.Player;


	public string notificationKey = "[player.death]";
	public string notificationArgument = "";

	private bool playerEntered = false;

	public override void OnPlayerEnter( PlayerScript PlayerScript, Collider col ) {

		if (triggerType == TriggerType.OnEnter)
		{
			if(!playerEntered) {
		 	 // GameLogic.Instance.playerInShop = PlayerScript;
				// SceneStateManager.instance.changeState(new ShopState(),true,false);
				 playerEntered = true;
			}
		}
	 }

		public override void OnPlayerFirstEnter(PlayerScript PlayerScript, Collider col ) {
		
		}
	
	public override void OnPlayerExit(  PlayerScript PlayerScript, Collider col ) {
		if (triggerType == TriggerType.OnExit)
		{
		  //GameLogic.Instance.playerInShop = null;
          //SceneStateManager.instance.changeToLastState(false);
    	}
		playerEntered = false;
  }

	public override void OnPlayerFirstExit(  PlayerScript PlayerScript, Collider col ) {

	}

	public Vector3 GetReleasePosition ( PlayerScript PlayerScript ) {

		if ( notifcationPoint == TriggerNotificationPoint.Player ) {
			return new Vector3(); // PlayerScript.gameObject.transform.position;
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
