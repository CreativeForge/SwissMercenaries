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
 //   this.gameLogic = GameObject.FindObjectOfType<GameLogic>();
    //Debug.Log(this + " found gameLogic " + gameLogic);
  }

  /*
	 * Triggers 
	 * */

  void OnTriggerEnter(Collider other) {

		// check for player
		LogicPlayer logicPlayer = other.GetComponent<LogicPlayer>();
		if (logicPlayer != null) {
			// Debug.Log ("OnTriggerPlayerEnter");
			if (!enterFirstTime) {
					OnPlayerFirstEnter( logicPlayer, other );
				enterFirstTime=true;				
			}

			OnPlayerEnter( logicPlayer, other );


		}
		
	}

	void OnTriggerExit(Collider other) {


		// check for player
		LogicPlayer logicPlayer = other.GetComponent<LogicPlayer>();
		if (logicPlayer != null) {
			// Debug.Log ("OnTriggerPlayerEnter");
			if (!exitFirstTime) {
					OnPlayerFirstExit( logicPlayer, other );
				exitFirstTime=true;				
			}

				OnPlayerExit( logicPlayer, other );

		}
		
	}


	/*
	 * Events
	 * 
	 * */

	// first time
	public virtual void OnPlayerFirstEnter( LogicPlayer logicPlayer, Collider col ) {
		
		
		
	}

	// xTimes
	public virtual void OnPlayerEnter( LogicPlayer logicPlayer, Collider col ) {

	}

	public virtual void OnPlayerFirstExit( LogicPlayer logicPlayer, Collider col ) {
		
	}

	public virtual void OnPlayerExit( LogicPlayer logicPlayer, Collider col ) {
		
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
	public void AddNotification( string key, Vector3 point, NotificationPriority np ) {
		
		AddNotification( key, "", point, np );
		
	}
	public void AddNotification( string key, string notificationArgument, Vector3 point, NotificationPriority np ) {
		
		// gameLogic.AddNotification( key, notificationArgument, point, np );
		
	}

}
