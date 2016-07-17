using UnityEngine;
using System.Collections;
using GameLab.NotficationCenter;

/*
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
*/

public class TriggerBase : MonoBehaviour {

	string triggerType = "enter";

	bool enterFirstTime = false;
	bool exitFirstTime = false;


	// game logic
	public GameLogic gameLogic;

	public GameElement gameElement; 
	public void SetGameElement( GameElement ga ) {
		gameElement = ga;

		// create trigger type
		if (gameElement.strevent=="enter") { triggerType = "enter"; };
		if (gameElement.strevent=="firstenter") { triggerType = "firstenter"; };
		if (gameElement.strevent=="notfirstenter") { triggerType = "notfirstenter"; };
		if (gameElement.strevent=="exit") { triggerType = "exit"; };
		if (gameElement.strevent=="firstexit") { triggerType = "firstexit"; };
		if (gameElement.strevent=="notfirstexit") { triggerType = "notfirstexit"; };

		OnInitTrigger();
	}

	// after set game element!
	public virtual void OnInitTrigger() {
		
	}

	// argument < GameElement.argument


  protected virtual void Awake() {
 	this.gameLogic = GameObject.FindObjectOfType<GameLogic>();
    //Debug.Log(this + " found gameLogic " + gameLogic);
  }

  /*
   * AddNotification
   * 
   * */

   // "abc.def"
   void AddNotification( string typesubtype, string targetName, float timed, string argument ) {
		gameLogic.levelEditor.notificationCenter.AddNotification(  typesubtype,  targetName,  timed,  argument );
   }
 
	// "abc","def" > self		
	void AddNotification( string type, string subtype, string targetName, float timed, string argument, Vector3 pos ) {
		gameLogic.levelEditor.notificationCenter.AddNotification(  type, subtype,  targetName,  timed,  argument, pos );
	}



  /*
	 * Triggers 
	 * */

  bool debugTriggerEvents = true;

  void OnTriggerEnter(Collider other) {

		// check for player
		PlayerScript PlayerScript = other.GetComponent<PlayerScript>();
		if (PlayerScript != null) {
			if (debugTriggerEvents) Debug.Log ("OnTriggerPlayerEnter");
			if (!enterFirstTime) {
				if (debugTriggerEvents) Debug.Log ("OnTriggerPlayerFirstEnter");
					OnPlayerFirstEnter( PlayerScript, other );
				enterFirstTime=true;				
			} else {
				if (debugTriggerEvents) Debug.Log ("OnTriggerPlayerNotFirstEnter");
				OnPlayerNotFirstEnter( PlayerScript, other );
			}

			OnPlayerEnter( PlayerScript, other );


		}
		
	}

	void OnTriggerExit(Collider other) {


		// check for player
		PlayerScript PlayerScript = other.GetComponent<PlayerScript>();
		if (PlayerScript != null) {
			if (debugTriggerEvents) Debug.Log ("OnTriggerPlayerExit");
			if (!exitFirstTime) {
				if (debugTriggerEvents) Debug.Log ("OnTriggerPlayerFirstExit");
					OnPlayerFirstExit( PlayerScript, other );
				exitFirstTime=true;				
			} else {
				if (debugTriggerEvents) Debug.Log ("OnTriggerPlayerNotFirstExit");
				OnPlayerNotFirstExit( PlayerScript, other );
			}

				OnPlayerExit( PlayerScript, other );

		}
		
	}


	/*
	 * Events
	 * 
	 * */

	// first time
	public virtual void OnActivateTrigger( PlayerScript PlayerScript, Collider col ) {


	}

	/*
	 *  specific trigger
	 * 
	 * */
	// first time
	public virtual void OnPlayerFirstEnter( PlayerScript PlayerScript, Collider col ) {
		if (triggerType.Equals("firstenter")) {
			OnActivateTrigger(  PlayerScript,  col );
		}
	}

	// not first time
	public virtual void OnPlayerNotFirstEnter( PlayerScript PlayerScript, Collider col ) {
		if (triggerType.Equals("notfirstenter")) {
			OnActivateTrigger(  PlayerScript,  col );
		}
	}

	// xTimes
	public virtual void OnPlayerEnter( PlayerScript PlayerScript, Collider col ) {
		if (triggerType.Equals("enter")) {
			OnActivateTrigger(  PlayerScript,  col );
		}
	}

	public virtual void OnPlayerFirstExit( PlayerScript PlayerScript, Collider col ) {
		if (triggerType.Equals("firstexit")) {
			OnActivateTrigger(  PlayerScript,  col );
		}		
	}

	public virtual void OnPlayerNotFirstExit( PlayerScript PlayerScript, Collider col ) {
		if (triggerType.Equals("notfirstexit")) {
			OnActivateTrigger(  PlayerScript,  col );
		}	
	}

	public virtual void OnPlayerExit( PlayerScript PlayerScript, Collider col ) {
		if (triggerType.Equals("exit")) {
			OnActivateTrigger(  PlayerScript,  col );
		}	
	}

	/*
	 * 
	 * 
	 * */

	// SetGameElement(


	
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
