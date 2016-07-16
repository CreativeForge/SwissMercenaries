using UnityEngine;
using System.Collections;
using GameLab.NotficationCenter;

public enum TriggerType {
	OnEnter,
	OnFirstEnter,
	OnExit,
	OnFirstExit
}

public enum TriggerNotificationPoint {
	Player,
	ObjectPivot
}


public class TriggerBase : MonoBehaviour {
		
	bool enterFirstTime = false;
	bool exitFirstTime = false;

	// game logic
	public GameLogic gameLogic;

	// argument < GameElement.argument

	[Tooltip("What should happen when this is triggered.")]
	public string NotificationName = "";

	[Tooltip("Additional Argument for the Notification.")]
	public string NotificationArgument = "";

	public void SetGameElementArgument( string iargument ) {
		NotificationName = iargument;
	}

  protected virtual void Awake() {
 	this.gameLogic = GameObject.FindObjectOfType<GameLogic>();
    //Debug.Log(this + " found gameLogic " + gameLogic);
  }

  /*
	 * Triggers 
	 * */

  void OnTriggerEnter(Collider other) {

		// check for player
		PlayerScript PlayerScript = other.GetComponent<PlayerScript>();
		if (PlayerScript != null) {
			// Debug.Log ("OnTriggerPlayerEnter");
			if (!enterFirstTime) {
					OnPlayerFirstEnter( PlayerScript, other );
				enterFirstTime=true;				
			}

			OnPlayerEnter( PlayerScript, other );


		}
		
	}

	void OnTriggerExit(Collider other) {


		// check for player
		PlayerScript PlayerScript = other.GetComponent<PlayerScript>();
		if (PlayerScript != null) {
			// Debug.Log ("OnTriggerPlayerEnter");
			if (!exitFirstTime) {
					OnPlayerFirstExit( PlayerScript, other );
				exitFirstTime=true;				
			}

				OnPlayerExit( PlayerScript, other );

		}
		
	}


	/*
	 * Events
	 * 
	 * */

	// first time
	public virtual void OnPlayerFirstEnter( PlayerScript PlayerScript, Collider col ) {
		
		
		
	}

	// xTimes
	public virtual void OnPlayerEnter( PlayerScript PlayerScript, Collider col ) {

	}

	public virtual void OnPlayerFirstExit( PlayerScript PlayerScript, Collider col ) {
		
	}

	public virtual void OnPlayerExit( PlayerScript PlayerScript, Collider col ) {
		
	}



	
	/*
	 * CheckPaused
	 * 
	 * */
	public bool CheckPaused() {
		
		// return gameLogic.CheckPaused( );
		return false;
	}
	
	/*
	 * AddNotification
	 * 
	 * */
/*	public void AddNotification( string key, Vector3 point, NotificationPriority np ) {
		
		// AddNotification( key, "", point, np );
		
	}
	public void AddNotification( string key, string notificationArgument, Vector3 point, NotificationPriority np ) {
		
		// gameLogic.AddNotification( key, notificationArgument, point, np );
		
	}
*/

}
