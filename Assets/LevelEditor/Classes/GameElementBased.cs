﻿using UnityEngine;
using System.Collections;

// todo: add this also to TriggerBase!!!

public class GameElementBased : MonoBehaviour {

	// game logic
	public GameLogic gameLogic;



	public void Start() {
		gameLogic = GameObject.FindObjectOfType<GameLogic>();
	}

	public void FixedUpdate() {

		// processing path ... 
		ProcessPathFollowing();

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

		Debug.Log("GameElementBased.OnGameElementInit()");
		OnGameElementInitPathFollowing();

	}

	public void OnGameStart() {
		
	}

	/*
	 *  path following script
	 * 
	 * 
	 * */
	// path & follow pathsystem
	// path (array of points)
	public bool pathFollowingActive = false;
	string pathFollowing = "";
	GameElement pathGameElement = null;
	string pathFollowingState = "init";
	GameElement actualGameElement = null;
	public ArrayList arrGameElementsPath = new ArrayList(); 
	int pathIndex = 0;
	public void OnGameElementInitPathFollowing() {
		// Debug.Log("GameElementBased.OnGameElementInitPathFollowing() // pathFollowingActive "+pathFollowingActive);
		// overwrite this
		if (pathFollowingActive) {
			// start following 
			pathFollowing = gameElement.argument;
			// Debug.Log("GameElementBased.OnGameElementInitPathFollowing() // pathFollowing "+pathFollowing);
			if (!pathFollowing.Equals("")) {
				pathFollowingState = "pathfound";
			}
		}
	}

	public void ProcessPathFollowing() {

		// init
		if (pathFollowingState.Equals("pathfound")) {
			pathGameElement = gameLogic.GetGameElementByName(pathFollowing);
			if (pathGameElement!=null) {
				Debug.Log("GameElementBased.OnGameElementInitPathFollowing() // pathPoints: "+pathGameElement.argument);
				// pathGameElement.argument
				// arrGameElementsPath = 
				pathFollowingState = "ready";
				arrGameElementsPath = gameLogic.levelEditor.GetGameElementsByString(pathGameElement.argument);
				Debug.Log("GameElementBased.OnGameElementInitPathFollowing() // arrGameElementsPath: "+arrGameElementsPath.Count);
					// Debug.Log("GameElementBased.OnGameElementInitPathFollowing() // arrGameElementsPath "+arrGameElementsPath.Count());
				if (arrGameElementsPath.Count>0) {
					pathIndex = 0;
					actualGameElement = (GameElement)arrGameElementsPath[pathIndex];
				}
			}	
		}
		if (pathFollowingState.Equals("ready")) {
			// find next .. 
			if (actualGameElement!=null) {

				if (actualGameElement.gameObject!=null) {

					// Debug.Log("GameElementBased.OnGameElementInitPathFollowing() // ready / search ");


					// go there now ...
					Vector3 postionGameObject = actualGameElement.gameObject.transform.position ;
					Vector3 position = new Vector3( postionGameObject.x, postionGameObject.y, postionGameObject.z );
					// dirty harry 
					if (transform.position.x<position.x) {
						transform.Translate( new Vector3(0.03f,0.0f,0.0f));
					}					
					if (transform.position.x>position.x) {
						transform.Translate( new Vector3(-0.03f,0.0f,0.0f));
					}					

					if (transform.position.z<position.z) {
						transform.Translate( new Vector3(0.0f,0.0f,0.3f));
					}					
					if (transform.position.z>position.z) {
						transform.Translate( new Vector3(0.0f,0.0f,-0.3f));
					}

					// distance?
					float dist = Vector3.Distance(transform.position,position);
					if (dist<0.5f) {
						FoundPathNode(  actualGameElement );
					}

					// transform.Translate( new Vector3(0.0f, 0.1f, 0.0f));
				}
			}
		}

	}

	public void FoundPathNode( GameElement gameEl ) {
		// execute Node

		// ok go to next one
		pathIndex = pathIndex + 1;
		if (pathIndex>arrGameElementsPath.Count) {
			pathIndex =	0;
		}
		actualGameElement = (GameElement)arrGameElementsPath[pathIndex];
	}

}