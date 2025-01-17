﻿using UnityEngine;
using System.Collections;

/*
 * 	
 * 
 * 
 * GameLogic {
 * 
 * 
 * }
 * 
 *    LevelEditor {
 * 		// Array GameElement

		LoadLevel()
		- UpdateElementVisual (update visual elements)
		- UpdateRelationVisualisationAndCheckError
 *    }
 * 
			GameElement < LevelElement (Unity3dEditor) {

			}
 * 
 * */

public class GameLogic : MonoBehaviour {

	// versions
	private float versionGame = 0.1f; // versioning
	public float GetVersionGame() {
		return versionGame;
	}
	private float versionEditor = 0.8f; // versioning
	public float GetVersionEditor() {
		return versionEditor;
	}

	// camera center
	public void SetCamera( GameObject cameragameObject ) {
		DisableCameras();
		// audiolistener
		if (cameragameObject!=null) {
		Camera newCam = cameragameObject.GetComponent<Camera>();
		if (newCam!=null) {
			newCam.enabled = true	;
		}}
	}

	public void DisableCameras() {
		foreach (Camera cam in FindObjectsOfType<Camera>()) {
			cam.enabled = false;
		}
	}

	// cam
	Camera camGame = null;
	Camera camEditor = null;
	public void SetGameCamera( GameObject camObj ){
		// Debug.Log("SetGameCamera() "+camObj.name);
		if (camObj!=null) {
			camGame = camObj.GetComponent<Camera>();
		}
	//	camObj.SetActive(false);
	}
	public void SetEditorCamera( GameObject camObj ){
		camEditor = camObj.GetComponent<Camera>();
	}

	// modal
	public GameLogicModal modal;
	public enum GameLogicModal {
		Running,
		Pause,
		Preferences,
		Editor
	}

	// player values
	public PlayerStorage Store { get; set; }

	public void Awake () {
			GetLevelEditor();
	}

	public void Start() {

		GetLevelEditor();

		// get autor 


		// get web ...


			
		// Camera
		SetGameCamera( GameObject.Find("Main Camera") );
		SetEditorCamera( GameObject.Find("editorcamera") );
		// SetGameState( GameLogicModal newmodal )
		SetGameState( GameLogicModal.Running );
	}

	public void SetGameState( GameLogicModal newmodal ) {
		
		// Debug.Log ("[GameLogic]SetGameState("+newmodal.ToString ()+")");
		
		// deactivate in the beginning
		/*
		DeactivateEditor();
		DeactivateRunning();
		*/

		DisableCameras();

		// Attention: running > leveleditor
		modal = newmodal;

		// ACTIVATE NEW MODEL
		if (modal == GameLogic.GameLogicModal.Running) {
			ActivateStateIngame();
			// switch to actual levelEditorLevel
			levelEditor.LoadActualEditorLevelIngame();
		}
		if (modal == GameLogic.GameLogicModal.Editor) {
			ActivateStateEditor();
			levelEditor.LoadActualEditorLevel();
		}
		
		
	}


	void ActivateStateIngame() {
		// Debug.Log("GameLogic.ActivateStateIngame();");
		if (camGame!=null) {
			camGame.enabled = true ;
			SetCamera(camGame.gameObject);
		}
	}

	void ActivateStateEditor() {
		// Debug.Log("GameLogic.ActivateStateIngame();");
//		DeactivateBoth();
		// camEditor.enabled = true ;
		SetCamera(camEditor.gameObject);

	}

		// to delete ...
		void _DeactivateBoth() {
			DisableCameras();
			/*
			// Debug.Log("GameLogic.DeactivateBoth();");
			if (camGame!=null) {
				camGame.enabled = false ;	

			}
			camEditor.enabled = false ;
			*/
		}

	public bool CheckIngameState() {
		return !(modal==GameLogic.GameLogicModal.Editor);
	}

	public bool CheckEditorState() {
		return (modal==GameLogic.GameLogicModal.Editor);
	}
	


	// Levels (Load etc.)
	public LevelEditor levelEditor;
	public void GetLevelEditor() {
		if (levelEditor == null) {
			GameObject levelEditorObject = GameObject.Find("_LevelEditor");
			levelEditor = levelEditorObject.GetComponent<LevelEditor>();
		}
	}

	/*
	public int level = 0;



	public void SetGameLevel( int ilevel ) {
		Debug.LogError("SetGameLevel() DONT USE THIS > LEVELEDITOR");
		PlayerPrefs.SetInt("LastEditedLevel", levelEditor.actualEditorLevel);
		level = ilevel;
		LoadGameLevel (level);

	}

	public void LoadNextLevel() {
		Debug.LogError("LoadNextLevel() DONT USE THIS > LEVELEDITOR");
		level++;
		SetGameLevel(level);

	}

	public void LoadActualIngameLevel(  ) {
		levelEditor.LoadInGameLevel (level);
	}

	public void LoadActualEditorLevel(  ) {
		levelEditor.LoadEditorLevel (level);
	}

	public void ReloadLevel(){
		Debug.LogError("ReloadLevel() DONT USE THIS > LEVELEDITOR");
		Application.LoadLevel (Application.loadedLevel);
	}


	public void LoadMenu()
	{
		// SceneStateManager.instance.changeState(new MenuScreenState());
	}

	public void LoadGameLevel( int level ) {

		//try {

			GetLevelEditor();
			levelEditor.LoadEditorLevel(level);

		//}
			//catch {
			// Debug.LogWarning("can't load _LevelEditor");
			//}

			// start now the game ...
			// 

			// ...
			
	}
	public void LoadGameNextLevel( ) {

		level++;
		LoadGameLevel (level);
	}
	*/

	/*
	 * 
	 *   Ingame
	 * 
	 *   (mirrored to the ingamecontrol)
	 * 
	 * */

	// all rewards
	public int gold = 0;
	public int goldMax = 0;
	public int killed = 0;
	public float playTime = 0.0f;
	// artefacts
	public Reward[] arrArtefacts; // all the possible ... 
	// mentals
	public Reward[] arrMentals; // all the possible ... 

	// gold visualisation
	public Reward[] arrGold; // via score

	// outgoing (level beginning)
	// update to ...
	public void UpdateToIngameControl() {
		
	}

	// incmoing (storing ingame)
	public void UpdateFromIngameControl() {
		
	}
	public void HandlePlayerChangeAmountMoney(PlayerScript playerscript, int newMoneyAmount) {
		// handle player change amoutn
		gold = newMoneyAmount;
		if (gold>goldMax) goldMax = gold;


	}

	// GameElements
	// 
	// Management in _LevelEditor!
	// 
	// < types etc defined in _LevelEditor
	// 
	// 
	public GameElement AddGameElement ( string type, string subtype, string name, string argument, Vector3 pos ) {

		return AddGameElement ( "",  type,  subtype,  name,  argument,  pos, 0.0f, ""  );

	}

	public GameElement AddGameElement ( string release, string type, string subtype, string name,  string argument, Vector3 posx, float rot, string stringNotification ) {

		GameElement ge = new GameElement ();

		ge.release = release;

		ge.type = type;
		ge.subtype = subtype;

		ge.name= name;
		ge.argument = argument;

		ge.position = posx;
		ge.rotation = rot;

		GameElement geinArray = AddGameElement( ge );

		// add notification there
		// stringNotifcation
		// not notification needed
		if (!stringNotification.Equals ("")) {

			// Vector3 pos = new Vector3 ( geinArray.position.x, geinArray.position.y, geinArray.position.z );

			// AddNotification (stringNotification, pos, NotificationPriority.ThrowAwayAfterProcessing);
		}


		return ge;

	}

	// Add Element > levelEditor
	public GameElement AddGameElement( GameElement newelement ) {

		return levelEditor.AddElement ( newelement  );
	}


	public ArrayList GetGameElementsByName( string name ) {

		return levelEditor.GetGameElementsByName( name );

	}

	public GameElement GetGameElementByName( string name ) {
		GetLevelEditor();
		return levelEditor.GetGameElementByName( name );
	}

	public void AddGameElements ( ArrayList arr ) {

		for (int i=0; i<arr.Count; i++) {
			GameElement ge = (GameElement) arr[i];
			// Debug.Log (i+". "+ge.name+" "+ge.type);
			AddGameElement( ge );
		}

	}

	// Remove Element 
	public void RemoveGameElement( GameElement deleteThisGameElement ) {

		//Vector3 pos = new Vector3 (deleteThisGameElement.position.x, deleteThisGameElement.position.y, deleteThisGameElement.position.z );

		levelEditor.RemoveElement ( deleteThisGameElement);

		// not notification needed
		/*
		 * if (!stringNotification.Equals ("")) {
			AddNotification (stringNotification, pos, NotificationPriority.ThrowAwayAfterProcessing);
		}
		*/
	}
}
