using UnityEngine;
using System.Collections;

public class TriggerPredefined : TriggerBase {

	public string notifcationType = "object.remove";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void  OnInitTrigger() {
		// Debug.Log("TriggerNotifcation.OnInitTrigger() // "+gameElement.name+"/"+gameElement.strevent);
	}


	public override void OnActivateTrigger( PlayerScript PlayerScript, Collider col ) {

		if (gameElement!=null) {
			// Debug.Log("TriggerNotification().OnActivateTrigger() // TriggerType: "+triggerType+"/timed: "+gameElement.timed+"/target: "+gameElement.target+"/argument: "+gameElement.argument+"/argumentsub: "+gameElement.argumentsub);
			/*
			 * string notifications = gameElement.argument;
			string[] arrNotifications = notifications.Split(',');
			for (int x=0;x<arrNotifications.Length;x++) {
				string notifcationTypeSubType = arrNotifications[x];
				AddNotification( notifcationTypeSubType, gameElement.target, gameElement.timed, gameElement.argumentsub );
			}
			*/
// 			notifcationType
			AddNotification( notifcationType, gameElement.target, gameElement.timed, gameElement.argumentsub );
		}


		// if (gameElement.)
	}

}
