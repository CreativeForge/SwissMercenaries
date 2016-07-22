using UnityEngine;
using System.Collections;

// todo: add this also to TriggerBase!!!

public class GameElementBased : MonoBehaviour {

	// game logic
	public GameLogic gameLogic;

	public void Start() {
		gameLogic = GameObject.FindObjectOfType<GameLogic>();
	}

	/*
   * AddNotification
   * 
   * */

	//
	// use this one!!! if you want to add something on self position
	// "abc.def"
	public void AddNotification( string typesubtype, string targetName, float timed, string argument ) {

		string[] arrType = typesubtype.Split('.') ;
		string type = arrType[0];
		string subtype = "???";
		if (arrType.Length>1)  subtype = arrType[1];

		this.gameLogic = GameObject.FindObjectOfType<GameLogic>();

		Vector3 pos = new Vector3(transform.position.x,transform.position.y,transform.position.z);
		if (gameLogic!=null) {
			gameLogic.levelEditor.notificationCenter.AddNotification(  type, subtype ,  targetName,  timed,  argument, pos );
		} else {
			
		}
	}

	// "abc","def" > self		
	public void AddNotification( string type, string subtype, string targetName, float timed, string argument, Vector3 pos ) {
		if (pos!=null) targetName = "vector";
		gameLogic.levelEditor.notificationCenter.AddNotification(  type, subtype,  targetName,  timed,  argument, pos );
	}

	public GameElement gameElement; 
	public void SetGameElement( GameElement ga ) {
		gameElement = ga;

		OnGameElementInit();
	}

	// after set game element!
	public virtual void OnGameElementInit() {
		// overwrite this
	}

	public void OnGameStart() {
		
	}

}
