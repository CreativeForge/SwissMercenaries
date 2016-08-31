/*
 * 
 * ### Editor  ###
 * 
 * // Editor: deactivate all children top level
* 
* LevelElement (Editor-CreateElementTypeAtInspector) > GameElement (InEditor)
* 
* DirtyCode: No entry for heaven
	* NextToDo: Refactoring
	*

	// WORKFLOW: CREATE NPC
	// 
	// Architecture:

	// NPCObject
	// [NPCScript] 
	// -- RB [RigidBocy] < NPCObject 
	// -- DS [DestructionScript] < NPCObject 
	// -- HS [NPCScript] < NPCObject 
	// -- HitIntervalTime !!
	// [HitterScript] 
	// -- HitBox1 [GameObject] < [HitBox-GameObject+HitBoxScript]
	// -- ES [RigidBody] < NPCObject 
	// [HitBox-GameObject] HitBox-GameObject  
	// -- [HitBoxScript]
	// -- [Collider] Trigger!

	// Examples:
	// - spearwall (
	// - barricade

	// SwissMercenaries
	// 
	// Mechanics: to decide: 
	// A: Getroffen werden und taummeln (jetzt) 
	// B: oder getroffen werden, aber weiterhin zuschlägen können!
	// 
	// GameElemtentsToDo:
	// - Farmer/Bauer mit Mistgabel oder schlecht ausgerüsteter Soldate > Kill no reaction / no hitten or minimal
	// 


	* */

	// new LevelObjects > levelElements

using UnityEngine;
using System.Collections;
using System;
using GameLab.NotficationCenter;
using System.IO;
using System.Text.RegularExpressions;

public class LevelEditor : MonoBehaviour {

	// debugging
	bool debugGameElements=false;
	bool debugGameElementTypes=false;

	/*
	 * notes: original gui was designed for 1280x800 
	 *        > problem now with unity5.4 > too small > no possibility to change size in gameview 
	 * 		  solution a: > scaler > short term (map everything > Scaler)
	 * 	      solution b: > 4.6 gui (problem - it must also be dynamic!!!) > long term
	 * */
	float scaleX = 2.0f;
	float scaleY = 2.0f;
	float scaleFont = 1.75f;
	void ScalerStart() {
		// scale all the 

		ScalerSetUp();


	}
	void ScalerSetUp() {
		// set scale
		scaleX = (float)Screen.width/(float)GUIScalerScreenWidth();
		scaleY = (float)Screen.height/(float)GUIScalerScreenHeight();
		scaleFont = scaleX; // Screen.width/GUIScalerScreenHeight;

		// update GUIStyles ... 
		int fontSize = (int) (16.0f * scaleFont * 0.75f);

		editorDefault.fontSize = fontSize;
		editorLabel.fontSize = fontSize;
		editorBackground.fontSize = fontSize;
		editorWebLevels.fontSize = fontSize;
		editorInspectorBackground.fontSize = fontSize;
		editorButtonStyle.fontSize = fontSize;
		editorButtonStyleBig.fontSize = fontSize*2;
		editorButtonStyleNotActive.fontSize = fontSize;
		editorButtonActiveStyle.fontSize = fontSize;

		editorSwitchButtonStyle.fontSize = fontSize;
		editorSwitchButtonStyleActive.fontSize = fontSize;
		editorButtonTypeStyle.fontSize = fontSize;
		editorButtonTypeStyleNotActive.fontSize = fontSize;
		editorButtonTypeSubStyle.fontSize = fontSize;
		editorButtonTypeSubStyleNotActive.fontSize = fontSize;

		editorButtonStyleWeb.fontSize = fontSize;
		editorButtonActiveStyleWeb.fontSize = fontSize;

		editorLoading.fontSize = fontSize;
		editorComment.fontSize = fontSize;
		editorIconGUI.fontSize = fontSize;
		editorElementType.fontSize = fontSize;
		guiEvaluation.fontSize = fontSize;
		editorTitleStyle.fontSize = fontSize;

		if (arrSensButtons.Count>0) {

			for (int i=0;i<arrSensButtons.Count;i++) {

			}
			arrSensButtons.Clear();
		}

		// set cursor
		cursorX = GUIScalerScreenWidth()-300; // (int)((float)(Screen.width - 300)*scaleX);
		cursorY = GUIScalerScreenHeight()-200; //(int)((float)(Screen.height - 200)*scaleY);

		// transform
		int inspectorXTmp = cursorX;
		int inspectorYTmp = cursorY;

		AddSensButton(inspectorXTmp,inspectorYTmp, 28,28, cursorIconRotateForward, "rotateforward" );
		inspectorXTmp = inspectorXTmp + 30;
		AddSensButton(inspectorXTmp,inspectorYTmp, 28,28, cursorIconRotateLeft, "rotateleft" );
		inspectorXTmp = inspectorXTmp + 30;
		AddSensButton(inspectorXTmp,inspectorYTmp, 28,28, cursorIconForward, "forward" );
		inspectorXTmp = inspectorXTmp + 30;
		AddSensButton(inspectorXTmp,inspectorYTmp, 28,28, cursorIconRotateRight, "rotateright" );
		inspectorXTmp = inspectorXTmp + 30;
		AddSensButton(inspectorXTmp,inspectorYTmp, 28,28, cursorIconUp, "up" );
		inspectorYTmp = inspectorYTmp + 30; 
		inspectorXTmp = cursorX; 
		AddSensButton(inspectorXTmp,inspectorYTmp, 28,28, cursorIconRotateBackward, "rotatebackward" );
		inspectorXTmp = inspectorXTmp + 30;
		AddSensButton(inspectorXTmp,inspectorYTmp, 28,28, cursorIconLeft, "left" );
		inspectorXTmp = inspectorXTmp + 30;
		AddSensButton(inspectorXTmp,inspectorYTmp, 28,28, cursorIconBackward, "backward" );
		inspectorXTmp = inspectorXTmp + 30;
		AddSensButton(inspectorXTmp,inspectorYTmp, 28,28, cursorIconRight, "right" );
		inspectorXTmp = inspectorXTmp + 30;
		AddSensButton(inspectorXTmp,inspectorYTmp, 28,28, cursorIconDown, "down" );



		// 
	}
	// simulate this ..
	int GUIScalerScreenWidth() {
		return 1280;
	}
	int GUIScalerScreenHeight() {
		return 768;
	}

	// Label
	// default
	void GUIScalerLabel( Rect rect, String text ) {
		GUIScalerLabel( rect, text, editorDefault );
	}
	void GUIScalerLabel( Rect rect, String text, GUIStyle guistyle ) {
		Rect nRect = new Rect( rect.x*scaleX,rect.y*scaleY, rect.width*scaleX, rect.height*scaleY);
		GUI.Label( nRect, text, guistyle );
	}

	void GUIScalerLabel( Rect rect, Texture2D tex , GUIStyle guistyle ) {
		Rect nRect = new Rect( rect.x*scaleX,rect.y*scaleY, rect.width*scaleX, rect.height*scaleY);
		GUI.Label( nRect, tex, guistyle );
	}

	void GUIScalerLabel( Rect rect, Texture2D tex ) {
		Rect nRect = new Rect( rect.x*scaleX,rect.y*scaleY, rect.width*scaleX, rect.height*scaleY);
		GUI.Label( nRect, tex, editorDefault );
	}


	string GUIScalerTextField( Rect rect, string  text ) {
		Rect nRect = new Rect( rect.x*scaleX,rect.y*scaleY, rect.width*scaleX, rect.height*scaleY);
		return GUI.TextField( nRect, text, editorDefault );
	}
	string GUIScalerTextField( Rect rect, string  text, GUIStyle guix ) {
		Rect nRect = new Rect( rect.x*scaleX,rect.y*scaleY, rect.width*scaleX, rect.height*scaleY);
		return GUI.TextField( nRect, text, guix );
	}


	// Button
	bool GUIScalerButton( Rect rect, String text ) {
		return GUIScalerButton( rect, text, editorDefault );
	}
	bool GUIScalerButton( Rect rect, String text, GUIStyle guistyle ) {
		Rect nRect = new Rect( rect.x*scaleX,rect.y*scaleY, rect.width*scaleX, rect.height*scaleY);
		return GUI.Button( nRect, text, guistyle);
	} 

	bool GUIScalerButton( Rect rect, Texture2D tex ) {
		Rect nRect = new Rect( rect.x*scaleX,rect.y*scaleY, rect.width*scaleX, rect.height*scaleY);
		return GUI.Button( rect, tex, editorDefault );
	}
	bool GUIScalerButton( Rect rect, Texture2D tex, GUIStyle guistyle ) {
		Rect nRect = new Rect( rect.x*scaleX,rect.y*scaleY, rect.width*scaleX, rect.height*scaleY);
		return GUI.Button( nRect, tex, guistyle);
	}

	// game logic
	GameLogic gameLogic;
	GameLogic GetGameLogic( ) {

		if (gameLogic == null) {

			GameObject gl = GameObject.Find ("_GameLogic");
			gameLogic = gl.GetComponent<GameLogic>();

		}

		return gameLogic;
	}

	// NotificationCenter
	public NotificationCenter notificationCenter;
	NotificationCenter GetNotificationCenter() {
		GameObject gl = GameObject.Find ("_NotificationCenter");
		if (gl!=null) {
			notificationCenter = gl.GetComponent<NotificationCenter>();
			if (notificationCenter!=null) {
				return notificationCenter;
			} else {
				AddEditorMessage("Could not find _NotificationCenter!");
			} 
		}
		return null;
	}

	// IngameController
	// IngameNotificationCenter
	public InGameController ingameController = null;
	public void SetIngameController( InGameController iingameControllerIn ) {
		ingameController = iingameControllerIn;
		ingameNotificationCenter = null;
		if (iingameControllerIn!=null)	ingameNotificationCenter = ingameController.notificationC;
	}
	public NotificationCenterPrototype ingameNotificationCenter; 

	// notification tester ...
	bool notificationTesterAndHistory = true;
	string fieldType  = "visual";
	string fieldTypeSub  = "explosion";
	string fieldTarget = "PLAYER"; 
	string fieldTimed = "0";
	string fieldArgument = "argument";


	// state special editor ...
	//string stateSpecialEditor=""; // '' > 'saved'
	// float stateSpecialEditorScroll=0.0f;
	// float stateSpecialEditorScrollY=0.0f;

	/*
	 * INGAME
	 * 
	 * */
	bool debugIngame = true;
	public bool GetIngameDebug() {
		return debugIngame;
	}
	float ingameStartTime = 0.0f;
	// -1.0f loading ... | >0.0 ingame
	public float GetIngameTime() {
		if (ingameStartTime==-1.0f) {
			return -1.0f;
		}
		if (ingameStartTime>=0.0f) {
			return Time.time-ingameStartTime;
		}
		return -1.0f;
	}
	public void SetIngameTimeToLoading() {
		ingameStartTime = -1.0f;
	}
	public void StartIngameTime() {
		ingameStartTime = Time.time;
	}
	string ingameState = "";
	string ingameStateSub = "";
	public void SetIngameState( string newIngameState ) {
		// on leave state
		IngameOnLeaveState();
		// do on all gameelements ...
		ingameState = newIngameState;
		// are there some events to trigger?
		IngameOnChangeState();

	}
	public bool CheckState( string statex ) {
		if (statex.Equals(ingameState)) {
			return true;
		}
		return false;
	}

	// on change state!
	void IngameOnLeaveState( ) {
		GameElement gx;
		if (arrLevel.Count>0)
			for (int i=0; i<arrLevel.Count; i++) {
				gx=(GameElement) arrLevel[i];
				if (gx.Active())
				if (gx.gameObject!=null) {
					GameElementBased geb = gx.gameObject.GetComponent<GameElementBased>();
					if (geb!=null) {
						geb.OnLeaveGameState(ingameState);
					}
				}
			} // /for
	}
	void IngameOnChangeState() {
		GameElement gx;
		if (arrLevel.Count>0)
			for (int i=0; i<arrLevel.Count; i++) {
				gx=(GameElement) arrLevel[i];
				if (gx.Active())
				if (gx.gameObject!=null) {
					GameElementBased geb = gx.gameObject.GetComponent<GameElementBased>();
					if (geb!=null) {
						geb.OnChangeGameState(ingameState);
					}
				}
			} // /for
	}


	// languages
	public string[] arrLanguages; //  = { "en", "de", "dech", "fr", "mhd" } ;

	string ingameLanguage = "en"; // de | dech
	public void SetInGameLanguage( string ilanguage, bool reloadLevel ) { // en | de | dech 
		ingameLanguage = ilanguage;
		// reload level !!!
		if (reloadLevel) ReloadActualInGameLevel();
		// Store to Prefs
		PlayerPrefs.SetString("language", ingameLanguage);
	}
	public string GetInGameLanguage() {
		string lan = "en";

		string slanguage = PlayerPrefs.GetString("language");
		if(slanguage!=null) { 
			if(!slanguage.Equals("")) { lan = slanguage; }
		}

		return lan; 
	}
	string ingameNewLanguageKey = "@key";
	string ingameNewLanguageText = "KEY";

	// LoadNextGameLevel
	public int ingameLevel = 0;
	public void LoadNextInGameLevel() {
		ingameLevel++;
		LoadInGameLevel(ingameLevel);
	}

	public void LoadInGameLevel( int newLevel ) {

		ingameLevel = newLevel;
		// actualLevel = newLevel;

		// PlayerPrefs.SetInt("LastEditedLevel", actualEditorLevel);

		// todo: load level ... 
		ClearLevel ();
		LoadLevel (ingameLevel);

		if (flagEvaluation) {
			// in running mode!
			NewSession (evaluationPlayer);
		}

	}


	public void ReloadActualInGameLevel() {
		LoadInGameLevel(ingameLevel);
	}

	// switch from editor to game
	public void LoadActualEditorLevelIngame() {
		// load it ...
		LoadInGameLevel(actualEditorLevel);
	}


	/*
	 *  languages
	 * 
	 * */

	// get with actual 
	public string ParseText( string lankey ) {
		return ParseText( lankey, ingameLanguage );
	}
	public string ParseText( string lankey, string languageKey ) {
		string text = ""+lankey;

		// @ ???
		if (lankey.IndexOf("@")==0) {

			// Debug.Log("LevelEditor.ParseText("+lankey+","+languageKey+")");

			// ok search for the key ..
			// @key
			// language/item
			// a: 'de'
			// as: was ist da los
			GameElement gx;
			if (arrLevel.Count>0)
				for (int i=0; i<arrLevel.Count; i++) {
					gx=(GameElement) arrLevel[i];
					if (gx.name.ToLower().Equals(""+lankey.ToLower())) {
						if (gx.type.Equals ("language")) {
							if (gx.subtype.Equals ("item")) {
								if ((gx.argument.Equals (""+languageKey))||(gx.argument.Equals ("*"))) {
									return gx.argumentsub;
								}
							}
						}
					}
				} // /for
		}

		return ""+text;
	}
	public GameElement GetGameElementLanguage( string lankey, string languageKey ) {
		string text = ""+lankey;

		// @ ???
		if (lankey.IndexOf("@")==0) {

			// Debug.Log("LevelEditor.ParseText("+lankey+","+languageKey+")");

			// ok search for the key ..
			// @key
			// language/item
			// a: 'de'
			// as: was ist da los
			GameElement gx;
			if (arrLevel.Count>0)
				for (int i=0; i<arrLevel.Count; i++) {
					gx=(GameElement) arrLevel[i];
					if (gx.name.Equals(""+lankey)) {
						if (gx.type.Equals ("language")) {
							if (gx.subtype.Equals ("item")) {
								if ( (gx.argument.Equals (""+languageKey)) || (gx.argument.Equals ("*"))  ) {
									return gx;
								}
							}
						}
					}
				} // /for
		}

		return null;
	}



	/*
	 * editor 
	 * 
	 * */
	string editorAutor = "ANONYMOUS";
	string editorPassword = "";
	string editorNewArea = "";



	// overlay
	bool cameraOverlayTypes = true;

	public GameObject dummyEditorPrefab;

	Vector3 editorCursorActualPoint=new Vector3();

	GameElement editorLastTouchedGameElement = null;

	public void LoadEditorLevel( int newLevel ) {

		actualEditorLevel = newLevel;
		// actualLevel = newLevel;

		PlayerPrefs.SetInt("LastEditedLevel", actualEditorLevel);

		// todo: load level ... 
		ClearLevel ();
		LoadLevel (actualEditorLevel);

		if (flagEvaluation) {
			// in running mode!
			NewSession (evaluationPlayer);
		}

	}

	// SWITCH FROM INGAME
	public void LoadActualEditorLevel() {
		LoadEditorLevel( ingameLevel );
	}

	// load game level (used from GameLogic) 
	// remove this!!!!
	/*
	 * public void LoadGameLevel( int newLevel ) {
		 
		Debug.LogError("LevelEditor.LoadGameLevel: Use LoadEditorLevel or LoadIngameLevel");

		actualLevel = newLevel;
		// todo: load level ... 
		ClearLevel ();
		LoadLevel (actualLevel);

		if (flagEvaluation) {
			// in running mode!
			NewSession (evaluationPlayer);
		}

	}
	*/

	// copy & paste
	ArrayList arrClipBoardLevel = new ArrayList();
	void CopyActualLEvelToClipBoard() {
		arrClipBoardLevel.Clear();
		GameElement ge;
		for (int i=0;i<arrLevel.Count;i++) {
			ge = (GameElement) arrLevel[i];
			GameElement gen = ge.Copy();
			arrClipBoardLevel.Add(gen);
		}
		AddEditorMessage("Actual level copied to clipboard");

	}
	void ClipBoardToActualLevel() {
		AddToEditorHistory("Paste level into actual Level");
		ClearLevel();

		// do this as level history
		ClearLevel();

		// add it .. 
		GameElement ge;
		for (int i=0;i<arrClipBoardLevel.Count;i++) {
			ge = (GameElement) arrClipBoardLevel[i];
			// update than ..
			AddElement( ge );  
		}

		UpdateShowEvaluationData ();


		AddToEditorHistory("Copied level");
		AddEditorMessage("Imported actual level");
	}


	// history for undo / redo ...
	ArrayList arrEditorHistory = new ArrayList();

	void AddToEditorHistory(  ) {
		AddToEditorHistory( "NoSpecificInputMessage" );
	}

	void AddToEditorHistory( string msg ) {
		// version 1.0
		AddToEditorHistoryConcrete(  msg ) ;
	}

	float timeToGoHistory = 0.0f;
	void AddToEditoryHistoryOnFixedUpdate() {
		if (timeToGoHistory>Time.time) {
			timeToGoHistory = Time.time + 1.0f;
			// check
		}
	}


	void AddToEditorHistoryConcrete( string msg ) {

		// in undo mode? 
		// remove the over the limit
		ArrayList arr = GetActualEditorHistory();
		if (historyIndexMinus>0) {

			// version 2.0
			if (arr.Count>0) {
				// Debug.Log("LevelEditor.AddToEditorHistory() // historyIndexMinus = "+historyIndexMinus);
				for (int i=0;i<historyIndexMinus;i++) {
					// Debug.Log("LevelEditor.AddToEditorHistory() // "+i+"---"+(arr.Count-1-i)+"--"+arr[arr.Count-1-i].GetType().Name);
					LevelHistory lvx = (LevelHistory)  arr[arr.Count-1-i];
					if (lvx!=null) {
						// Debug.Log("LevelEditor.AddToEditorHistory() Remove // "+i+"---"+(arr.Count-1-i));
						arrEditorHistory.Remove(lvx);
					}
				}
			}
			historyIndexMinus = 0;


		}

		// add now
		LevelHistory lv = new LevelHistory();
		lv.level = actualEditorLevel;
		lv.message = msg;

		// copy level ... 
		GameElement ge;
		for (int i=0;i<arrLevel.Count;i++) {
			ge = (GameElement) arrLevel[i];
			GameElement gen = ge.Copy();
			lv.arrLevel.Add(gen);
		}

		arrEditorHistory.Add(lv);

		// store 
		SaveLevel( actualEditorLevel );
	}


	ArrayList GetActualEditorHistory( ) {
		ArrayList arr = new ArrayList();

		LevelHistory lv;
		for (int i=0;i<arrEditorHistory.Count;i++) {
			lv = (LevelHistory) arrEditorHistory[i];
			if (lv.level==actualEditorLevel) {
				arr.Add(lv);
			}
		}

		return arr;
	}

	// speeds
	float speedCamera = 0.4f;
	float speedObject = 0.4f;

	bool cursorObject = false; // object or ...
	int cursorX = Screen.width - 300;
	int cursorY = Screen.height - 200;
	Rect cursorRect = new Rect(0,0,0,0);
	ArrayList arrSensButtons = new ArrayList();
	class SensButton {
		public Rect rect = new Rect();
		public Texture2D icon ;
		public string key = "";
		public void Init( int ix, int iy, int iwidth, int iheight, Texture2D iicon, string ikey) {
			rect.x = ix	;
			rect.y = iy;
			rect.width = iwidth;
			rect.height = iheight;
			icon = iicon;
			key = ikey;
		}
	}

	void AddSensButton(int x, int y, int width, int height, Texture2D icon, string key) {
		SensButton sensbuttonObj = new SensButton();
		sensbuttonObj.Init( x,  y,  width,  height,  icon,  key);
		arrSensButtons.Add(sensbuttonObj);
	}

	void MoveObjectAlongEditorCamera( string strdirect ) {
		// Debug.Log("LevelEditor.MoveObjectAlongEditorCamera() // "+strdirect);
		if (editorSelected!=null) {
			GameObject container=GameObject.Find ("editorCameraContainer");
			GameObject editorcamera=GameObject.Find ("editorcamera");

			Vector3 direction = container.transform.forward;
			direction.Normalize();
			bool isMovement = true;
			if (strdirect.Equals("forward")) direction = direction;
			if (strdirect.Equals("backward")) direction = -direction;
			if (strdirect.Equals("up")) direction =  new Vector3(0.0f, speedObject*2.0f, 0.0f);
			if (strdirect.Equals("down")) direction =  new Vector3(0.0f, -speedObject*2.0f, 0.0f);
			if (strdirect.Equals("left")) direction = new Vector3(-direction.z, 0.0f, direction.x);
			if (strdirect.Equals("right")) direction =  new Vector3(direction.z, 0.0f, -direction.x);
			if (strdirect.Equals("rotateforward")) { editorSelected.rotationForward = editorSelected.rotationForward + 3.0f;  isMovement= false; /* direction =  new Vector3(0.0f,0.0f,0.0f); AddEditorMessage("Not implemented for objects!"); */ }
			if (strdirect.Equals("rotatebackward")) { editorSelected.rotationForward = editorSelected.rotationForward - 3.0f;  isMovement= false; /* direction =  new Vector3(0.0f,0.0f,0.0f); AddEditorMessage("Not implemented for objects!"); */ }
			if (strdirect.Equals("rotateleft")) { editorSelected.rotation = editorSelected.rotation + 5.0f; isMovement= false; }
			if (strdirect.Equals("rotateright")) { editorSelected.rotation = editorSelected.rotation - 5.0f; isMovement= false; }
			if (isMovement) {
				direction = direction * 0.5f * speedObject;
				// Debug.Log("LevelEditor.MoveObjectAlongEditorCamera() // " + direction);
				editorSelected.position = editorSelected.position + direction;
			}

			UpdateElementVisual(editorSelected);
		} else {
			// sorry no ..
		}
	}

	void DoSensButton( SensButton bu, string strevent) {
		GameObject container=GameObject.Find ("editorCameraContainer");
		GameObject editorcamera=GameObject.Find ("editorcamera");

		// Debug.Log("LevelEditor.DoSensButton("+strevent+")");

		if (cursorObject) {

			if (strevent.Equals("down")) {
				MoveObjectAlongEditorCamera(bu.key);
			}

			if (strevent.Equals("up")) {
				MoveObjectAlongEditorCamera(bu.key);
				AddToEditorHistory("up");
			}
		}

		if (!cursorObject) {
			if (strevent.Equals("down")) {
				// camera 
				if (bu.key.Equals("left")) {
					container.transform.Translate ( new Vector3(-speedCamera, 0.0f, 0.0f));
				}
				if (bu.key.Equals("right")) {
					container.transform.Translate ( new Vector3( speedCamera, 0.0f, 0.0f));
				}
				if (bu.key.Equals("up")) {
					container.transform.Translate ( new Vector3( 0.0f, speedCamera, 0.0f));
				}
				if (bu.key.Equals("down")) {
					container.transform.Translate ( new Vector3( 0.0f, -speedCamera, 0.0f));
				}
				if (bu.key.Equals("forward")) {
					container.transform.Translate ( new Vector3( 0.0f, 0.0f, speedCamera));
				}
				if (bu.key.Equals("backward")) {
					container.transform.Translate ( new Vector3( 0.0f, 0.0f, -speedCamera));
				}

				// 	rotate left & right					
				if (bu.key.Equals("rotateleft")) {
					container.transform.Rotate ( new Vector3(0.0f, -3.0f, 0.0f));
				}
				if (bu.key.Equals("rotateright")) {
					container.transform.Rotate ( new Vector3(0.0f, 3.0f, 0.0f));
				}

				// 	rotate left & right					
				if (bu.key.Equals("rotateforward")) {
					// container.transform.Rotate ( new Vector3(-2.0f,0.0f, 0.0f));
					// MoveObjectAlongEditorCamera("rotateforward");
					editorcamera.transform.Rotate ( new Vector3(-3.0f, 0.0f, 0.0f));
				}
				if (bu.key.Equals("rotatebackward")) {
					// container.transform.Rotate ( new Vector3(2.0f,0.0f,  0.0f));
					// MoveObjectAlongEditorCamera("rotateforward");
					editorcamera.transform.Rotate ( new Vector3( 3.0f, 0.0f, 0.0f));
				}

				// object
			}
		}

	}

	// 0 ... 
	int historyIndexMinus = 0;

	void Undo( ) {
		historyIndexMinus++;
		ArrayList arr = GetActualEditorHistory();
		int concreteIndex = arr.Count -1 - historyIndexMinus; // 12 - 1  -5 
		if (historyIndexMinus>(arr.Count -1)) {
			historyIndexMinus = arr.Count -1;
			concreteIndex = 0;
			AddEditorMessage("Sorry no more UNDOs");
		}
		LevelHistory lvHist = (LevelHistory) arr[concreteIndex];
		UndoTo(lvHist);
	}

	// Militär Putsch in der Turkei


	void Redo( ) {
		historyIndexMinus--;
		ArrayList arr = GetActualEditorHistory();
		int concreteIndex = arr.Count -1 - historyIndexMinus;
		if (historyIndexMinus<0) {
			historyIndexMinus = 0;
			concreteIndex = arr.Count -1;
			AddEditorMessage("Actual!");

		}
		LevelHistory lvHist = (LevelHistory) arr[concreteIndex];
		UndoTo(lvHist);

	}

	void UndoTo( LevelHistory lv ) {

		// do this as level history
		ClearLevel();

		// add it .. 
		GameElement ge;
		for (int i=0;i<lv.arrLevel.Count;i++) {
			ge = (GameElement) lv.arrLevel[i];
			// update than ..
			AddElement( ge );  
		}

		UpdateShowEvaluationData ();

		SaveLevel( actualEditorLevel );

	}

	// actual level
	string leveltype = "local"; // type of level: [local/web/live]
	void SetLevelType( string nleveltype ) {
		leveltype = nleveltype;

		PlayerPrefs.SetString("leveltype", leveltype);

		// ok let's check ... 
		// confusing
		// LoadLevel( actualLevel );
		LoadLevel( 0 );
		// remoteSelection = true;
	}

	bool CheckLevelTypeWeb() { 

		if (leveltype.Equals("web")) {
			return true;
		} 

		return false;
	}

	/*
	 *  REMOTE EDIT (web/live)
	 * 
	 * */
	// remoteEditArea
	string remoteAreaEdit = "";
	string remoteAutorEdit = "";
	void RemoteEdit() {
		error = false;
		remoteSelection = false; 
		// 		LoadLevel (actualLevel);
		SetLevel(0);
	}
	void RemoteDownload() {
		error = false;
		RemoteDownloadsDo();
	}

	string GetRemotePath() {
		// remote
		if (!Directory.Exists ("remote")) {
			Directory.CreateDirectory("remote");
		}
		// remote
		string areaPath = "remote"+Path.DirectorySeparatorChar+remoteAreaEdit;
		if (!Directory.Exists (areaPath)) {
			Directory.CreateDirectory(areaPath);
		}
		// remote
		string autorPath = "remote"+Path.DirectorySeparatorChar+remoteAreaEdit+Path.DirectorySeparatorChar+remoteAutorEdit;
		if (!Directory.Exists (autorPath)) {
			Directory.CreateDirectory(autorPath);
		}

		return "remote"+Path.DirectorySeparatorChar+remoteAreaEdit+Path.DirectorySeparatorChar+remoteAutorEdit+Path.DirectorySeparatorChar;
	}

	// remote areas - selection!
	bool remoteSelection = false;
	string remoteArea = "";
	string remoteAutor = "";
	ArrayList arrRemoteAreas = new ArrayList();
	ArrayList arrRemoteAreasAddOn = new ArrayList();
	ArrayList arrRemoteAreaAutors = new ArrayList();
	ArrayList arrRemoteAreaAutorsAddOn = new ArrayList();
	//arrRemoteAreaAutorsAddOn
	bool offline = false;
	bool loading = false;
	string loadingLabel = "";
	bool error = false;
	string errorLabel = "";


	/*
	 * get 
	 * 
	 * */
	void RemoteGetAreas() {
		error = false;

		if (!Directory.Exists ("remote")) {
			Directory.CreateDirectory("remote");
		}
		StartCoroutine(RemoteGetAreasGet());
	}
	private IEnumerator RemoteGetAreasGet() {
		Debug.Log("LevelEditor.RemoteGetAreasGet() ");
		loading = true;
		loadingLabel = "LOADING AREAS ";
		WWW w = new WWW("http://www.swissmercenariesgame.com/services.php?service=getareas");
		yield return w;
		offline = false;

		if (w.error != null)
		{
			Debug.Log("Error .. " +w.error);
			// for example, often 'Error .. 404 Not Found'
			// tell error
			AddEditorMessage("Sorry, are you connected to internet?");
			offline = true;

			// get ....
			arrRemoteAreas = new ArrayList();
			arrRemoteAreasAddOn = new ArrayList();
			string [] subdirectoryEntries = Directory.GetDirectories("remote");
			foreach(string subdirectory in subdirectoryEntries) {
				// Debug.Log("remote/"+subdirectory);
				string effective = subdirectory.Substring("remote/".Length);
				arrRemoteAreasAddOn.Add("(local-offline)");
				arrRemoteAreas.Add(""+effective);
			}

		}
		else
		{
			// Debug.Log("Found ... ==>" +w.text +"<==");
			Debug.Log("LevelEditor.RemoteGetAreasGet() // text = " +w.text +"<==");
			arrRemoteAreas = new ArrayList();
			arrRemoteAreasAddOn = new ArrayList();
			string jsonText = ""+w.text;
			JSONObject jsonArray = new JSONObject(jsonText);
			if (jsonArray.list!=null)
				for(int i = 0; i < jsonArray.list.Count; i++){
					// string key = (string)jsonArray.keys[i];
					JSONObject levelObj = jsonArray[i];
					JSONObject areaObj =  levelObj.GetField("area");
					if (areaObj!=null) {
						string str = ""+areaObj.str;
						// arrRemoteAreas
						// local?
						string addon = "";
						if (Directory.Exists ("remote"+Path.DirectorySeparatorChar+str)) {
							// Directory.CreateDirectory(""+folderName);
							addon = "(local)";
						}
						arrRemoteAreasAddOn.Add(""+addon);
						arrRemoteAreas.Add(""+str);
					}
				}
		}

		loading = false;

		// go on ...
		if (!remoteAutorEdit.Equals("")) {
			loading = true;
			RemoteGetAreaAutors();
		}

	}
	void RemoteGetAreaAutors() {
		error = false;

		StartCoroutine(RemoteGetAreaAutorsGet( true ));
	}
	void RemoteGetAreaAutorsNoClose() {
		StartCoroutine(RemoteGetAreaAutorsGet( false ));
	}
	private IEnumerator RemoteGetAreaAutorsGet( bool closeDialog ) {
		loading = true;
		loadingLabel = "LOADING AUTORS FOR "+remoteArea;
		string url = "http://www.swissmercenariesgame.com/services.php?service=getareaautors&area="+WWW.EscapeURL(remoteArea);
		Debug.Log("LevelEditor.RemoteGetAreaAutorsGet() // url = "+url);
		WWW w = new WWW( url );
		yield return w;
		if (w.error != null)
		{
			Debug.Log("Error .. " +w.error);
			// for example, often 'Error .. 404 Not Found'
			// tell error
			AddEditorMessage("Sorry, are you connected to internet?");

			// get ....
			arrRemoteAreaAutors = new ArrayList();
			arrRemoteAreaAutorsAddOn = new ArrayList();
			string [] subdirectoryEntries = Directory.GetDirectories("remote"+Path.DirectorySeparatorChar+remoteArea);
			foreach(string subdirectory in subdirectoryEntries) {
				// Debug.Log("remote/"+subdirectory);
				string effective = subdirectory.Substring(("remote"+Path.DirectorySeparatorChar+remoteArea+"/").Length);
				arrRemoteAreaAutorsAddOn.Add("(local-offline)");
				arrRemoteAreaAutors.Add(""+effective);
			}

		}
		else
		{
			Debug.Log("LevelEditor.RemoteGetAreaAutorsGet() // text = " +w.text +"<==");
			arrRemoteAreaAutors = new ArrayList();
			arrRemoteAreaAutorsAddOn = new ArrayList();
			string jsonText = ""+w.text;
			if (jsonText.IndexOf("result\":\"error\"")!=-1) {
				// return ; 
			}
			JSONObject jsonArray = new JSONObject(jsonText);
			// remoteArea
			//			if (jsonArray!=null) 
			//			if (jsonArray.list.Count>0)
			if (jsonArray.list!=null)
			{
				for(int i = 0; i < jsonArray.list.Count; i++){
					// string key = (string)jsonArray.keys[i];
					JSONObject levelObj = jsonArray[i];
					JSONObject areaObj =  levelObj.GetField("autor");
					if (areaObj!=null) {
						string str = "" + areaObj.str;
						string localpath = ""+"remote"+Path.DirectorySeparatorChar+remoteArea+Path.DirectorySeparatorChar+str;
						Debug.Log("LevelEditor.RemoteGetAreaAutorsGet() // localpath = "+localpath);
						string addon = "";
						if (Directory.Exists (localpath)) {
							// Directory.CreateDirectory(""+folderName);
							addon = "(local)";
						}
						arrRemoteAreaAutors.Add(""+str);
						arrRemoteAreaAutorsAddOn.Add(""+addon);
					}
				}
			}
		}

		if (closeDialog){
			loading = false;
		}
	}

	// RemoteDownloadsDo
	void RemoteDownloadsDo() {
		error = false;

		StartCoroutine(RemoteDownloadsDoGet());
	}
	private IEnumerator RemoteDownloadsDoGet() {
		loading = true;
		loadingLabel = "DOWNLOADING LEVELS ";

		for (int i=-1;i<maxLevel;i++) {
			int level = i;
			if (i==-1) level = allLevel;
			loadingLabel = "DOWNLOADING LEVEL "+level+"/"+maxLevel;
			string url = "http://www.swissmercenariesgame.com/services.php?service=get&area="+ WWW.EscapeURL(remoteAreaEdit)+"&autor="+WWW.EscapeURL(remoteAutorEdit)+"&level="+WWW.EscapeURL(""+level);
			Debug.Log("LevelEditor.RemoteDownloadsDoGet() // url = "+url);
			WWW w = new WWW(url);
			yield return w;
			if (w.error != null)
			{
				Debug.Log("Error .. " +w.error);
				// for example, often 'Error .. 404 Not Found'
				// tell error
				AddEditorMessage("Sorry, are you connected to internet?");
			}
			else
			{
				Debug.Log("LevelEditor.RemoteDownloadsDoGet() // level = "+level+" text = " +w.text +"<==");
				string jsontext = ""+w.text;
				if (jsontext.Equals("")) jsontext = "[]";
				// get path ...
				string path = GetRemotePath()+"level"+level+".txt";
				Debug.Log("LevelEditor.RemoteDownloadsDoGet() // "+ path);
				System.IO.File.WriteAllText( path,""+w.text);


			}
		}

		loading = false;
		remoteSelection = false; 
		SetLevel(0);
	}

	// remote upload
	void RemoteUpload() {
		error = false;

		StartCoroutine(RemoteUploadDo( true, -1 ));
	}
	void RemoteUploadOnly( int levelIndex ) {
		StartCoroutine(RemoteUploadDo( true, levelIndex ));
	}

	// used for create a new level
	void RemoteUploadDummy() {
		// remoteAutorEdit
		StartCoroutine(RemoteUploadDo( false, -1 ));
	}
	private IEnumerator RemoteUploadDo( bool uploadExistingLevels, int levelIndex /* or -1 for all */ ) {

		loading = true;
		loadingLabel = "UPLOADING LEVELS ";
		if (!uploadExistingLevels) {
			loadingLabel = "CREATING EMPTY LEVELS FOR AUTOR ";
		}

		for (int i=-1;i<maxLevel;i++) {
			// only one level (very dirty version .-)
			if (levelIndex!=-1) {
				if (levelIndex!=i) {
					continue;
				}
			}
			int level = i;
			if (i==-1) level = allLevel;
			loadingLabel = "UPLOADING LEVEL "+level+"/"+maxLevel;
			string url = "http://www.swissmercenariesgame.com/services.php?service=set&password="+WWW.EscapeURL(editorPassword)+"&area="+ WWW.EscapeURL(remoteAreaEdit)+"&autor="+WWW.EscapeURL(remoteAutorEdit)+"&level="+WWW.EscapeURL(""+level);
			Debug.Log("LevelEditor.RemoteUploadGet() // url = "+url);

			string path = GetRemotePath();
			string jsonText = "[]";

			// version 2
			WWWForm form = new WWWForm();
			if (uploadExistingLevels) {
				if (System.IO.File.Exists( path + "level"+level+".txt" )) {
					jsonText = System.IO.File.ReadAllText( path + "level"+level+".txt");
				}
			}
			form.AddField("argument",jsonText);

			WWW w = new WWW(url,form);
			yield return w;
			if (w.error != null)
			{
				Debug.Log("Error .. " +w.error);
				// for example, often 'Error .. 404 Not Found'
				// tell error
				AddEditorMessage("Sorry, are you connected to internet?");
				error = true;
				errorLabel = "Could not upload. Are you connected to the internet?";
			}
			else
			{

				Debug.Log("LevelEditor.RemoteUploadDo() // level = "+level+" text = " +w.text +"<==");

				if (w.text.IndexOf("result\":\"error\"")!=-1) {
					error = true;
					errorLabel = "Password wrong!";
				}
			}
		}

		if (uploadExistingLevels) {
			loading = false;
			remoteSelection = false; 

			if (!error) {
				AddEditorMessage("Upload done");
			}

		} else {
			remoteAutor = "";
			remoteAutorEdit = "";
			RemoteGetAreaAutors();
		}
	}

	/*
	 *  ADMIN
	 * 
	 * */
	// RemoteDelete
	void RemoteDelete() {
		error = false;

		StartCoroutine(RemoteDeleteDo( ));
	}
	private IEnumerator RemoteDeleteDo(  ) {

		loading = true;
		loadingLabel = "DELETING LEVELS";

		for (int i=0;i<maxLevel;i++) {
			loadingLabel = "DELETING LEVELS "+i+"/"+maxLevel;
			string url = "http://www.swissmercenariesgame.com/services.php?service=delete&password="+WWW.EscapeURL(editorPassword)+"&area="+ WWW.EscapeURL(remoteAreaEdit)+"&autor="+WWW.EscapeURL(remoteAutorEdit)+"&level="+WWW.EscapeURL(""+i);
			Debug.Log("LevelEditor.RemoteDeleteDo() // url = "+url);

			string path = GetRemotePath();
			string jsonText = "[]";

			// version 2
			/*
			WWWForm form = new WWWForm();
			if (uploadExistingLevels) {
				if (System.IO.File.Exists( path + "level"+i+".txt" )) {
					jsonText = System.IO.File.ReadAllText( path + "level"+i+".txt");
				}
			}
			form.AddField("argument",jsonText);
			*/

			WWW w = new WWW(url);
			yield return w;
			if (w.error != null)
			{
				Debug.Log("Error .. " +w.error);
				// for example, often 'Error .. 404 Not Found'
				// tell error
				AddEditorMessage("Sorry, are you connected to internet?");
			}
			else
			{

				Debug.Log("LevelEditor.RemoteUploadDo() // level = "+i+" text = " +w.text +"<==");
				if (w.text.IndexOf("error")!=-1) {
					error = true;
					errorLabel = "WRONG PASSWORD!";
				}

			}
		}

		loading = false;

		if (!error) {
			RemoteGetAreas();
		}
	}

	// RemoteNewArea
	void RemoteNewArea( string newArea ) {
		error = false;

		StartCoroutine(RemoteNewAreaDo(  newArea  ));
	}
	private IEnumerator RemoteNewAreaDo(  string newArea  ) {

		loading = true;
		loadingLabel = "NEW AREA";

		// for (int i=1;i<maxLevel;i++) {
		loadingLabel = "NEW AREA ";
		string url = "http://www.swissmercenariesgame.com/services.php?service=new&password="+WWW.EscapeURL(editorPassword)+"&area="+ WWW.EscapeURL(newArea)+"&autor=xyz&level=1";
		Debug.Log("LevelEditor.RemoteNewAreaDo() // url = "+url);

		string path = GetRemotePath();
		string jsonText = "[]";

		// version 2
		WWW w = new WWW(url);
		yield return w;
		if (w.error != null)
		{
			Debug.Log("Error .. " +w.error);
			// for example, often 'Error .. 404 Not Found'
			// tell error
			AddEditorMessage("Sorry, are you connected to internet?");
		}
		else
		{

			Debug.Log("LevelEditor.RemoteNewAreaDo() " +w.text +"<==");
			if (w.text.IndexOf("error")!=-1) {
				error = true;
				errorLabel = "WRONG PASSWORD!";
			}

		}

		loading = false;

		if (!error) {
			RemoteGetAreas();
		}
	}

	/*
	 * LevelEditor
	 * 
	 * */
	public int actualEditorLevel=0; // level editor level!!!!
	int maxLevel=10; 
	int minMaxLevel = 10;
	int maxMaxLevel = 20;
	int allLevel = 1789; // Level that will be loaded in all levels (> languages etc)

	// toolsx
	int toolsX = 10;
	int toolsY = 20;
	int toolsWidth = 300;
	int toolsHeight = 200;
	Rect toolsRect = new Rect(0,0,200,100);

	// typeselection
	int typeselectionX = 10;
	int typeselectionY = 200;
	int typeselectionWidth = 300;
	int typeselectionHeight = 200;
	Rect typeselectionRect = new Rect(0,0,200,100);

	int selectionSize = 300;
	int minSelectionSize = 300;
	int selectionMaxElements = 15;

	// inspector
	int inspectorX = 10;
	int inspectorY = 140;
	int inspectorWidth = 300;
	int inspectorHeight = 200;
	Rect inspectorRect = new Rect(0,0,200,100);

	// wrap
	int maxXToWrap = 450 - 60;

	// show save
	bool flagShowSaveAs = false;

	// clear and add a simple base object ...
	void NewLevel() {

		ClearLevel ();

		// get level base and add one !
		/*
		ArrayList arrBases = GetElementTypes("base");
		if (arrBases.Count > 0) {
			GameElement ge = (GameElement) arrBases[0];
			ge.position.x = 0.0f;
			ge.position.y = 0.0f;
			ge.position.z = 0.0f;
			AddElement ( ge );
		}
		*/

		DefaultElements();

	}

	void ClearLevel() {

		// default settings
		RenderSettings.fog = false;

		EmptyEditorCursorPreview();

		ClearElements ();

		ingameState = "";

		// Clear not taggeds .. 
		// Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object)).Length
		GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
		ArrayList arrToClear = new ArrayList();
		foreach(GameObject go in allObjects) {
			// Debug.Log("Root.GameObject"+go.name+" "+go.tag);
			if (go.transform.parent==null) {
				if (!go.tag.Equals("Essentials")) {
					arrToClear.Add(go);
				}
			}
		}

		// destroy them all
		if (arrToClear.Count>0)
			for (int i=arrToClear.Count-1;i>=0;i--) {
				GameObject obj = (GameObject)  arrToClear[i] ;
				Destroy( obj );
			}

	}

	void DefaultElements() {
		// prefabY
		GameElement editorPrefabY;

		// title
		editorPrefabY = AddGameElementAtName ("meta","title", new Vector3(0.0f,0.0f,0.0f), "TITLE" );

		// desc
		editorPrefabY = AddGameElementAtName ("meta","description", new Vector3(0.0f,0.0f,0.0f), "DESCRIPTION" );

		// ground
		// editorPrefabY = AddGameElementAtName ("meta","autor", new Vector3(0.0f,0.0f,0.0f), "AUTOR" );


		// ground
		editorPrefabY = AddGameElementAtName ("base","ground_box", new Vector3(0.0f,0.0f,0.0f), "GROUND" );

		// player
		editorPrefabY = AddGameElementAtName ("player","player", new Vector3(1.0f,2.0f,1.0f), "PLAYER");

		// gamelogiccontroller
		editorPrefabY = AddGameElementAtName ("base","ingamecontroller", new Vector3(2.0f,2.0f,2.0f), "INGAMECONTROLLER");

		// directlight
		editorPrefabY = AddGameElementAtName ("light","directlight", new Vector3(3.0f,2.0f,3.0f), "DIRECTLIGHT");

		// sky
		editorPrefabY = AddGameElementAtName ("sky","1200", new Vector3(4.0f,2.0f,4.0f), "SKY");

		// fog
		editorPrefabY = AddGameElementAtName ("weather","fog", new Vector3(4.0f,2.0f,4.0f), "FOG");

	}

	void SetLevel( int iactualLevel ) {

		// Save History ...

		actualEditorLevel = iactualLevel;
		// todo: load level ... 
		ClearLevel ();
		LoadLevel (actualEditorLevel);
	}

	// scroll position
	/*
	 * float scroll=0.0f;
	float scrolly=0.0f;
	// scrollinterval
	public float scrollStep=0.01f; // ...
	public float GetScrollStep( ) {
		return scrollStep;
	}
	public void SetScrollStep( float iscrollStep) {
		scrollStep = iscrollStep;
	}

	void ResetScroll() {
			
	}
*/
	void DoEditorScroll( float dx, float dy, float dz ) {

		// Debug.Log ("DoEditorScroll( "+ dx +", "+ dy +", "+ dz +" )");

		GameObject container=GameObject.Find ("editorCameraContainer");
		// Debug.Log (""+container);
		Vector3 pos = new Vector3 (container.transform.position.x, container.transform.position.y, container.transform.position.z);
		pos = pos + new Vector3 (dx, dy, dz);
		container.transform.position = pos;

	}


	// Messages
	public ArrayList arrEditorMessages=new ArrayList();
	void AddEditorMessage( string msg ) {
		LevelEditorMessage msgx = new LevelEditorMessage ();
		msgx.message = msg;
		msgx.timeToStayTill = Time.time + 2.0f;
		arrEditorMessages.Add(msgx);
	}

	// GameElement (Definition > own class) 


	// elements types
	public ArrayList arrGameElementTypes=new ArrayList();

	// element types
	public GameElement AddElementType() {
		GameElement gameElement = new GameElement ();
		arrGameElementTypes.Add (gameElement);
		return gameElement;
	}

	// add gameelement by type at (0,0,0)
	public GameElement AddGameElementAtName( string type, string subtype, Vector3 pos, string name ) {
		GameElement gaType = GetElementType ( type, subtype );
		if (gaType != null) {
			GameElement gex = gaType.Copy ();
			gex.position = pos;
			gex.name = name;
			return AddElement(gex);
		}

		return null;
	} 

	public GameElement ElementObject( GameObject gameObj ) {

		for (int a=0; a<arrLevel.Count; a++) {
			GameElement gelement = (GameElement)arrLevel [a];
			// Debug.Log ("---"+a+". "+gelement.type+"/"+gelement.subtype+"   searching for: "+gameObj.GetInstanceID());
			if (gelement.gameObject==gameObj) {
				// Debug.Log ("------"+a+". "+gelement.type+"/"+gelement.subtype+"  "+gelement.gameObject.name);
				return gelement;
			}
		}

		return null;
	}

	public GameElement GetElementObject( GameObject gameObj ) {

		for (int a=0; a<arrLevel.Count; a++) {
			GameElement gelement = (GameElement)arrLevel [a];
			// Debug.Log ("---"+a+". "+gelement.type+"/"+gelement.subtype+"   searching for: "+gameObj.GetInstanceID());
			if (gelement.gameObject==gameObj) {
				// Debug.Log ("------"+a+". "+gelement.type+"/"+gelement.subtype+"  "+gelement.gameObject.name);
				return gelement;
			}
		}

		return null;
	}

	// arrLevel

	public int CountElementsType( string elementArea, string elementSubArea ) {
		int count = 0;
		for (int a=0; a<arrLevel.Count; a++) {
			GameElement gelement = (GameElement)arrLevel [a];
			if (gelement.type.Equals (elementArea)) {
				if (gelement.subtype.Equals (elementSubArea)) {
					count++;
				}
			}

		}
		return count;

	}

	public int CountElementsType( string elementArea ) {
		int count = 0;
		for (int a=0; a<arrLevel.Count; a++) {
			GameElement gelement = (GameElement)arrLevel [a];
			if (gelement.type.Equals (elementArea)) {
				count++;
			}

		}
		return count;

	}

	// count elements
	public bool CountElementsTypeIndexOf( string searchFor ) {
		int count = 0;
		for (int a=0; a<arrLevel.Count; a++) {
			GameElement gelement = (GameElement)arrLevel [a];
			if (gelement.type.IndexOf (searchFor)!=-1) {
				return true;
			}
			if (gelement.subtype.IndexOf (searchFor)!=-1) {
				return true;
			}
		}
		return false;
	}



	// RemoveElementsType
	public  void RemoveElementsType( string elementArea, string elementSubArea ) {
		int startx=arrLevel.Count-1;
		for (int a=startx; a>=0; a--) {
			GameElement gelement = (GameElement)arrLevel [a];
			if (gelement.type.Equals (elementArea)) {
				if (gelement.subtype.Equals (elementSubArea)) {
					RemoveElement (gelement);
				}
			}

		}
	}

	public ArrayList GetElementTypes( string elementArea ) {

		ArrayList arr = new ArrayList ();

		for (int a=0; a<arrGameElementTypes.Count; a++) {
			GameElement gelement = (GameElement)arrGameElementTypes [a];
			if (gelement.type.Equals (elementArea)) {
				arr.Add (gelement);
			}
		}

		return arr;

	}

	// arrLevel

	public GameElement GetElementType( string elementArea, string elementSubArea ) {

		for (int a=0; a<arrGameElementTypes.Count; a++) {
			GameElement gelement = (GameElement)arrGameElementTypes [a];
			if (gelement.type.Equals (elementArea)) {
				if (gelement.subtype.Equals (elementSubArea)) {
					GameElement gaex = gelement.Copy();
					return gaex;
					// return gelement;
				}
			}
		}

		return null;

	}

	// update labels!
	void EditorUpdateArgumentLabels(GameElement gobj) {
		if (gobj!=null) {
			GameElement prefabTemplateX = GetElementType (gobj.type,gobj.subtype);
			if (prefabTemplateX!=null) {
				gobj.guiLabel = prefabTemplateX.guiLabel;
				gobj.guiDescription = prefabTemplateX.guiDescription;
				gobj.guiBoolArgument = prefabTemplateX.guiBoolArgument;
			}
		}
	}



	// create
	public GameElement CreateElementTypeAt ( string elementArea, string elementSubArea, Vector3 position  ) {

		GameElement gaType = GetElementType ( elementArea, elementSubArea );
		if (gaType != null) {
			GameElement argObj = gaType.Copy ();
			argObj.position=position;
			return argObj;
		}

		if (gaType == null) {
			Debug.Log ("COULD NOT Create "+elementArea+"/"+elementSubArea+"!");

		}

		return null;
	}



	// get all types (not subtypes) background etc ..
	public ArrayList GetElementTypesUnique(  ) {

		ArrayList arr = new ArrayList ();

		for (int a=0; a<arrGameElementTypes.Count; a++) {
			GameElement gelement = (GameElement)arrGameElementTypes [a];
			bool found=false;
			for (int aa=0; aa<arr.Count; aa++) {
				GameElement gelementInArray = (GameElement)arr [aa];	
				if (gelement.type.Equals (gelementInArray.type)) {
					found=true;
				}
			}
			if (!found) {
				arr.Add (gelement);
			}
		}

		return arr;

	}



	/*
	 * Elements
	 * 
	 * 
	 * */

	// different elements
	// ... 
	// house
	// level ...
	// level + "  " +


	// specific levelElements


	public LevelElement[] BaseLevelElements = { new LevelElement ("town"), new LevelElement ("country") , new LevelElement ("test")  };
	public LevelElement[] LightLevelElements= {  new LevelElement ("light")   };
	public LevelElement[] SkyLevelElements = {    };
	public LevelElement[] MetaLevelElements= {   };

	public LevelElement[] ChallengeLevelElements = { };

	// tiles
	public LevelElement[] TileLevelElements = {   };

	// specials
	public LevelElement[] ImmovablesLevelElements = { new LevelElement ("scheune"), new LevelElement ("city"),  new LevelElement ("fountain")    };
	public LevelElement[] ColliderLevelElements = {    };
	public LevelElement[] MovableLevelElements = { new LevelElement ("box")   };
	public LevelElement[] FurnitureLevelElements = { } ;
	public LevelElement[] PlayerLevelElements = { new LevelElement ("player1"), new LevelElement ("player2") };
	// public LevelElement[] SwissLevelSoldiersEleements = { new LevelElement ("small"), new LevelElement ("idiot") , new LevelElement ("cool")  };

	public LevelElement[] EnemyLevelElements= { new LevelElement ("waiting"), new LevelElement ("landsknecht") , new LevelElement ("dtreich")  };

	public LevelElement[] PathLevelElements = { new LevelElement ("tour"), new LevelElement ("waypoint"),  };

	public LevelElement[] WeaponLevelElements = {  };
	public LevelElement[] ItemsLevelElements = { new LevelElement ("meat") };
	public LevelElement[] TriggerLevelElements = { new LevelElement ("playernotifcation") };

	public LevelElement[] ActionElements = { new LevelElement ("notification") }; // green
	public LevelElement[] EnvElements = { new LevelElement ("light") };

	public LevelElement[] WeatherElements = { new LevelElement ("light") };

	public LevelElement[] CamCutElements = {  };
	public LevelElement[] GUIMenuElements = {  };


	public LevelElement[] RemarkElements = { new LevelElement ("comment") }; // yellow

	// LanguageElements
	public LevelElement[] LanguageElements = {  }; // yellow

	public LevelElement[] EvaluationElements = { new LevelElement ("death") }; // 

	// filter ...
	bool filterActive = true;
	string filterType = "*";
	string filterTypeSub = "*";
	string filterDetail = "";
	Rect filterTypeVisual = new Rect(0,0,0,0);
	Rect filterTypeSubVisual = new Rect(0,0,0,0);

	// nearby
	bool flagNearBy = false;
	int nearbyX = 512;
	int nearbyY = 512;
	int nearbyWidth = 100;
	int nearbyHeight = 100;
	Rect nearbyRect = new Rect(0,0,0,0);

	// undo
	Rect undoVisual = new Rect(0,0,0,0);


	// selectiondialoge
	bool selectionDialoge = true;
	string selectFilter = ""; // names etc.  #name types ...  !abc.* !
	Rect selectionDialogeVisual = new Rect(0,0,0,0);
	GameElement[] selectionAffectedElements = {};

	// notifcations
	bool notificationDialog = false;
	string notificationArea = "";
	string notificationAreaSub = "";
	int notificationDialogX = 300;
	int notificationDialogY = 200 ;
	Rect notificationVisual = new Rect(0,0,0,0);
	string strNotification = "";




	// prefab elements
	// varia 
	public LevelElement[] levelElements = { 

		/* new LevelElement ("waiter"), new LevelElement ("landsknecht") */

	} ;

	// elements
	void InitGameElements() {

		// most important categories
		RegisterLevelElements( "base", BaseLevelElements );

		RegisterLevelElements( "light", LightLevelElements );
		RegisterLevelElements( "sky", SkyLevelElements );

		RegisterLevelElements( "tile", TileLevelElements );


		RegisterLevelElements( "meta", MetaLevelElements );
		RegisterLevelElements( "challenge", ChallengeLevelElements );


		RegisterLevelElements( "immovable", ImmovablesLevelElements );
		RegisterLevelElements( "furniture", FurnitureLevelElements );

		RegisterLevelElements( "collider", ColliderLevelElements );

		RegisterLevelElements( "movable", MovableLevelElements );

		RegisterLevelElements( "player", PlayerLevelElements );
		// RegisterLevelElements( "swisssoldiers", SwissLevelSoldiersEleements );

		RegisterLevelElements( "enemy", EnemyLevelElements );

		RegisterLevelElements( "path", PathLevelElements );

		RegisterLevelElements( "item", ItemsLevelElements );
		RegisterLevelElements( "weapon", WeaponLevelElements );


		RegisterLevelElements( "trigger", TriggerLevelElements );

		RegisterLevelElements( "action", ActionElements );
		RegisterLevelElements( "env", EnvElements );

		RegisterLevelElements( "weather", WeatherElements );

		RegisterLevelElements( "camcut", CamCutElements );
		RegisterLevelElements( "guimenu", GUIMenuElements );

		RegisterLevelElements( "remark", RemarkElements );

		RegisterLevelElements( "language", LanguageElements );

		RegisterLevelElements( "evaluation", EvaluationElements, false ); // mark this (user should no insert this manually)

	}

	// register levelelement arrays
	void RegisterLevelElements( string prefix, LevelElement[] ilevelElements ) {
		RegisterLevelElements (prefix, ilevelElements, true);
	}

	// register levelelement arrays
	void RegisterLevelElements( string prefix, LevelElement[] ilevelElements, bool visibleInEditor ) {

		// Debug.Log ("RegisterLevelElements( "+ prefix + " )");

		// 1. sort: first sort for editorIndex
		LevelElement a;
		LevelElement b;
		for (int i=0;i<ilevelElements.Length;i++) {
			for (int ii=0;ii<ilevelElements.Length;ii++) {
				a = ilevelElements[i];
				b = ilevelElements[ii];
				if (a.editorIndex<b.editorIndex) {
					ilevelElements[i] = b;
					ilevelElements[ii] = a;
				}
			}
		}

		// 2. add: by the order of the array
		LevelElement el;
		for (int i=0;i<ilevelElements.Length;i++) {
			el = ilevelElements[i];

			// add elements
			// if ( el.gameObject != null ) {
			GameElement geType = AddElementType ();
			geType.type = prefix; 
			geType.subtype = el.typetypesub; 
			geType.prefabGameObject = el.gameObject;
			geType.skyBoxMaterial = el.skyBoxMaterial;
			geType.prefabEditorDummyGameObject = el.editorPrefab; // dummy prefab
			// copy and add all prefabs for arguments (evaluation.allover 2,1,0 etc
			geType.prefabPredefinedArguments = el.prefabPredefinedArguments; // .Copy(); // all the same reference
			geType.guiBoolArgument = el.argumentNeeded; 
			// geType.editorIsGround = el.isGround;
			geType.guiLabel = el.argumentLabel;	 
			geType.guiDescription = el.argumentDescription;

			geType.argumentInputType = el.inputType;
			// geType.argumentInputTypeSelect = el.inputTypeSelect;

			//						geType.editorIndex = el.editorIndex;

			// GetType.ingameSource = el.ingameSource;
			geType.editorTileSize = el.editorTileSize;
			geType.editorIsGround = el.isGround;
			geType.randomPredefineds = el.randomPredefineds;
			if (geType.editorIsGround) {
				// Debug.Log("LevelEditor.RegisterLevelElements() // isGROUND: "+geType.type+"/"+geType.subtype);
			}
			geType.guiShowInMenu = visibleInEditor;

			geType.editorDisplaySize = el.editorDisplaySize;

			geType.editorRandom = el.editorRandom;

			// }
		}

	}

	// Evaluation INGAME
	bool flagEvaluationAvailable = false; // is this availabe at all?
	bool showEvaluationDialog = false;

	bool flagEvaluation = false;  // ingame will be evaluated!

	// all over evaluation functions
	// user "0" 
	string evaluationUserAllOver = "0";
	string evaluationUserComment="";

	bool flagSaveToWeb = true; // * save to web?
	//bool flagEvaluateElementsPosition = true; // * log position change?
	string evaluationPlayerId = "";
	EvaluationPlayer evaluationPlayer = new EvaluationPlayer();
	// string evaluationSessionId = "";

	GameObject GetFirstPlayerObject() {
		if (GameObject.Find ("player1") != null)
			return GameObject.Find ("player1");
		if (GameObject.Find ("Walti") != null)
			return GameObject.Find ("Walti");

		return null;
	}


	string evaluationFolder = "evaluation";

	public bool CheckForEvaluate( Notification notifObj ) {
		return true;
	}

	void StartUpEvaluationTools() {

		if (!flagEvaluationAvailable)
			return;

		if (!flagEvaluation)
			return;

		// check for our evaluation
		if (!Directory.Exists (""+evaluationFolder)) {
			Directory.CreateDirectory ("" + evaluationFolder);
			Debug.Log("Evaluation ");
		}




		// is there a player set up?
		string[] arr = GetExistingEvaluationPlayerFiles ();

		if (arr.Length==0) {
			// take the last one !
			// evaluationPlayer
			CreateNewEvaluationPlayer();
			// SaveEvaluationPlayer( evaluationPlayer  );

		} else {

			string filename = arr[0];

			/*
			 * for (int i=0;i<arr.Length;i++) {
				string filename=arr[i];
				Debug.Log (i+". "+filename);
			}
			*/

			evaluationPlayerId = GetPlayerIdFrom( filename );

			// Debug.Log ("evaluationPlayerId: "+evaluationPlayerId);

			EvaluationPlayer ep = LoadEvaluationPlayer( evaluationPlayerId );
			evaluationPlayer = ep;

			// new session
			ep.sessionId ++;
			SaveEvaluationPlayer(ep);

		}


	}

	string[] GetExistingEvaluationPlayerFiles() {
		string[] filePaths = Directory.GetFiles(@""+evaluationFolder+Path.DirectorySeparatorChar, "player_*.txt");	
		DateTime[] creationTimes = new DateTime[filePaths.Length];
		for (int i = 0; i < filePaths.Length; i++)
			creationTimes[i] = new FileInfo(filePaths[i]).CreationTime;
		Array.Sort(creationTimes, filePaths);
		Array.Reverse (filePaths);
		return filePaths;
	}

	string GenerateNewPlayerId() {
		string playId = "" + (int) UnityEngine.Random.Range (10000,90000);
		return playId;
	}

	string GetPlayerIdFrom( string ifilename ) {

		string filename = ""+ifilename;

		string id = "";

		int posP = filename.IndexOf ("player_");
		filename = filename.Substring (posP);
		filename = filename.Substring ("player_".Length);
		int pos = filename.IndexOf (".");
		if (pos != -1) {
			id = filename.Substring (0,pos);
		}

		return id;
	}

	void CreateNewEvaluationPlayer( ) {

		evaluationPlayer = new EvaluationPlayer();
		evaluationPlayerId = GenerateNewPlayerId();
		// Debug.Log ("[EVALUATION] new PlayerId: "+evaluationPlayerId);
		evaluationPlayer.playerId = evaluationPlayerId;
		evaluationPlayer.name = "Heinzer"+(int) UnityEngine.Random.Range (10.0f, 30.0f);
		SaveEvaluationPlayer( evaluationPlayer  );

	}


	void NewSession( EvaluationPlayer playerObj ) {
		playerObj.sessionId++;
		SaveEvaluationPlayer (playerObj);
	}

	EvaluationPlayer LoadEvaluationPlayer( string iplayerId ) {

		EvaluationPlayer ep = new EvaluationPlayer ();

		string jsonText = System.IO.File.ReadAllText( evaluationFolder+Path.DirectorySeparatorChar + "player_"+iplayerId+".txt");
		// Debug.Log (jsonText);
		JSONObject jsonObj = new JSONObject(jsonText);

		ep.GetObjectFromJSON (jsonObj);

		return ep;
	}

	void SaveEvaluationPlayer( EvaluationPlayer playerObj ) {

		string encodedString = playerObj.GetJSONObject().Print();
		System.IO.File.WriteAllText( evaluationFolder + Path.DirectorySeparatorChar +"player_"+playerObj.playerId+".txt",""+encodedString);

	}


	// level evaluations
	string[] GetExistingEvaluationLevelFiles( int level ) {
		string[] filePaths = Directory.GetFiles(@""+evaluationFolder+Path.DirectorySeparatorChar, "level"+level+"_*.txt");	
		DateTime[] creationTimes = new DateTime[filePaths.Length];
		for (int i = 0; i < filePaths.Length; i++)
			creationTimes[i] = new FileInfo(filePaths[i]).CreationTime;
		Array.Sort(creationTimes, filePaths);
		Array.Reverse (filePaths);
		return filePaths;
	}


	// Evaluation Editor
	/*
			 evaluation/notification > argument

			 evaluation/comment > argument

			 evaluation/hate -
			 evaluation/unclear ?
			 evaluation/like +
			 evaluation/interesting !


			 evaluation/playerpos
			 evaluation/playerhealth
			 evaluation/playerhealthrelative
			 evaluation/playermoney

			 evaluation/playerdeath


			# ideen: 

			evaluationsmöglichkeiten user

			- graphics
			- visuals
			- leveldesign
			- spannung
			- fun

			> visualisiert als werte gesichtchen ...

			> vielleicht sogar mit den letzten werten ....


		*/



	/*
	*  GameLogic 
	**/



	void AddEvaluationNotification( string strNotification, string argument, Vector3 pos ) {

		// is notification on...
		bool addThis = false;
		if (flagEvaluation) {
			// log?
			addThis = true;
		}
		if (addThis) {
			AddEvaluationElement(  strNotification,  argument,  pos );
		}
	}

	// update ..
	float timeToGo = 0.0f;
	float timeInterval = 2.0f;
	void UpdateEvaluationTimed() {

		if (Time.time > timeToGo) {
			timeToGo = Time.time + timeInterval;
			if (flagEvaluation) {
				DoTimedEvaluation();
			}
		}

	}
	// DoTimedEvaluation
	void DoTimedEvaluation() {

		// Debug.Log ("DoTimedEvaluation() "+Time.time);

		// player position
		/*				GameObject playerObj = GetFirstPlayerObject ();
				if (playerObj != null) {
					AddEvaluationElement ("player.position", "", playerObj.transform.position);
					
				}
*/

		GameObject playerObj = GetFirstPlayerObject ();
		if (playerObj != null) {
			AddEvaluationElement ("player.position", "", playerObj.transform.position);

		}

	}



	// add evaluation elements
	void AddEvaluationElement( string evalTypeSub, string evalArgument, Vector3 pos ) {

		GameElement evalElem = new GameElement();

		evalElem.subtype = "" + evalTypeSub;
		evalElem.argument = "" + evalArgument;
		evalElem.position = new Vector3(pos.x,pos.y,pos.z);


		AddEvaluationElement( evalElem );

	}

	void AddEvaluationElement( GameElement ge ) {

		// Debug.Log("AddEvaluationElement() "+ge.type+"/"+ge.subtype+" ["+ge.argument+"]");

		ge.evaluationPlayerId = evaluationPlayer.playerId;
		ge.evaluationSessionId = evaluationPlayer.sessionId;
		ge.evaluationPlayTime = Time.time;

		ge.type = "evaluation";

		AddElement( ge );

		// save evaluation now .. 
		SaveLevel( actualEditorLevel, true ); 

	}

	ArrayList GetEvaluationSessionElements( GameElement el ) {
		ArrayList arr = new ArrayList ();

		for (int a=0; a<arrLevel.Count; a++) {
			GameElement gelement = (GameElement)arrLevel [a];
			if (gelement.evaluationSessionId == el.evaluationSessionId) {
				if (gelement.evaluationPlayerId == el.evaluationPlayerId) {
					arr.Add(gelement);					
				}
			}
		}

		return arr;
	}

	// Dialogs
	bool showEvaluationDialogEdit = false;
	string showEvaluationDialogEditType = "";
	void EditEvaluationPlayerStart( string ishowEvaluationDialogType ) {
		showEvaluationDialogEdit = true;
		showEvaluationDialogEditType = ishowEvaluationDialogType;
	}
	void EditEvaluationPlayerStop() {
		showEvaluationDialogEdit = false;		
	}



	// Evaluation Data (Editor visual)
	bool editorShowEvaluationData = false; // editorShowEvaluationData 
	string editorEvaluationFilter = "all"; // filter
	// ArrayList arrPlayers = ArrayList();

	string editorEvaluationPlayerId = "";
	string editorEvaluationSessionId = "";

	string[] arrEvaluationPlayers; //  = GetExistingEvaluationPlayerFiles ();
	EvaluationPlayer[] arrEvaluationPlayersObj;
	ArrayList arrEvaluationSessions = new ArrayList(); //  = GetExistingEvaluationPlayerFiles ();

	void ToggleShowEvaluationData() {
		editorShowEvaluationData = !editorShowEvaluationData;

		// update now
		UpdateShowEvaluationData ();

	}

	// 
	void UpdateShowEvaluationData () {

		// players

		arrEvaluationPlayers = GetExistingEvaluationPlayerFiles ();

		// Debug.Log ("arrEvaluationPlayers " + arrEvaluationPlayers.Length);

		// load the objects
		if (arrEvaluationPlayers.Length > 0) {
			arrEvaluationPlayersObj = new EvaluationPlayer[arrEvaluationPlayers.Length];
			for (int i=0; i<arrEvaluationPlayers.Length; i++) {
				EvaluationPlayer pl = LoadEvaluationPlayer (GetPlayerIdFrom (arrEvaluationPlayers [i]));
				arrEvaluationPlayersObj [i] = pl; 
			}
		}

		// sessions
		// arrEvaluationSessions
		arrEvaluationSessions = new ArrayList ();

		string[] arrLevelFiles = GetExistingEvaluationLevelFiles (actualEditorLevel);
		for (int i = 0; i < arrLevelFiles.Length; i++) {
			string fileN = arrLevelFiles [i];
			Match match = Regex.Match (fileN, @"level" + actualEditorLevel + "_(\\d+)_(\\d+).", RegexOptions.IgnoreCase);
			if (match.Success) {
				string playerId = match.Groups [1].Value;
				string sessionId = match.Groups [2].Value;
				GameElement ge = new GameElement();
				ge.evaluationPlayerId = playerId;
				ge.evaluationSessionId = Int32.Parse(sessionId);
				arrEvaluationSessions.Add(ge);					}
		}

		// default?
		RemoveAllEvaluationGameElements();

		// updateand check

		// load data
		if (editorShowEvaluationData) {

			// editorEvaluationFilter = "all" | "selected"

			// load evaluations
			LoadEvaluationLevels( );


		}

		UpdateRelationVisualisationAndCheckError ();


	}

	void RemoveAllEvaluationGameElements() {

		for (int a=arrLevel.Count-1;a>=0;a--) {
			GameElement gelement=(GameElement)arrLevel[a];
			if (gelement.type.Equals ("evaluation")) { 
				RemoveElement(gelement);
			}
		}

	}

	// arrEvaluationSessions
	ArrayList GetSessionsByPlayerId( string playerId ) {

		// Debug.Log("GetSessionsByPlayerId( "+ playerId +" )");

		ArrayList arrList = new ArrayList ();

		GameElement el;
		for (int i=0;i<arrEvaluationSessions.Count;i++) {
			el = (GameElement) arrEvaluationSessions[i];
			if (el.evaluationPlayerId.Equals(playerId)) {
				arrList.Add(el);
				// Debug.Log(i+". "+el.evaluationSessionId+" added ");
			}
		}

		return arrList;
	}

	// Level ... 

	// all elements ... 
	ArrayList arrLevel=new ArrayList();

	public GameElement AddElement(GameElement elem) {

		// get prefab for this element
		UpdateElementVisual (elem);

		arrLevel.Add (elem);
		// update here..

		return elem;
	}

	public void UpdateElementVisual( GameElement elem ) {

		GameObject levelObject = GameObject.Find ("level");

		if (levelObject == null) {
			levelObject = new GameObject();
			levelObject.name="level";
		}

		// is there one? remove it ..
		if (elem != null) {
			if (elem.gameObject != null) {
				Destroy (elem.gameObject);
				elem.gameObject = null;
			}

			GameElement elPrefab = GetElementType (elem.type,elem.subtype);
			if (elPrefab==null) { 
				Debug.Log("Error: Could not find Type("+elem.type+"/"+elem.subtype+")"); 
				return; 
			}; 
			if (elPrefab!=null) { 
				// Debug.LogError("Could find Type("+elem.type+"/"+elem.subtype+")");
				// elPrefab.prefabGameObject=
				// create gameobject

				/*
				 * if (elPrefab.type.Equals ("base")) {
						elem.position = new Vector3();
					}
				*/

				// rotation 
				Quaternion re = new Quaternion();
				if ((elem.rotation!=0.0f)||(elem.rotationForward!=0.0f)) {
					re = Quaternion.Euler(elem.rotationForward, elem.rotation, 0);
				}

				// elPrefab.prefabGameObject // .prefabEditor
				if (elPrefab.prefabGameObject==null) {
					// take the dummy object
					if (gameLogic !=null && gameLogic.modal==GameLogic.GameLogicModal.Editor ) {
						GameObject go=Instantiate(dummyEditorPrefab, new Vector3(elem.position.x,elem.position.y,elem.position.z), re) as GameObject;
						go.name = "NotFound7";
						// size
						if (elem.size!=1.0f) {
							go.transform.localScale = elem.size * go.transform.localScale;
						}
						elem.gameObject=go;
						if (!elem.name.Equals("")) { go.name=""+elem.name; }
						go.transform.parent = levelObject.transform; 
					}
				}

				// if (elPrefab.prefabGameObject!=null) {
				if (true) {

					// skyBoxMaterial
					if (elem.skyBoxMaterial!=null) {
						// if there is a skybox
						// Debug.Log("LevelEditor.UpdateElementVisual().AddSkybox");
						Camera cam = Camera.main;
						if (cam!=null) {
							Skybox skyBox = cam.GetComponent<Skybox>();
							if (skyBox!=null) {
								skyBox.material = 	elem.skyBoxMaterial	;						
								RenderSettings.skybox = elem.skyBoxMaterial;
							}
						} else {
							Debug.Log("No cam found!")	;
						}

						// ambient light in argument
						RenderSettings.ambientIntensity = 1.0f;
						if (!elem.argument.Equals("")) {
							float ambient = float.Parse(elem.argument);
							RenderSettings.ambientIntensity = ambient;
						}


					}


					// EDITOR: special - only in editor?
					if (gameLogic !=null && gameLogic.modal==GameLogic.GameLogicModal.Editor ) {


						// GameObject go; // new GameObject(); // > create a new bare game object
						GameObject go = null ;// = new GameObject(); // > create a new bare game object
						// go.name = "PROBLEMWITHDUMMY: "+elem.type+"/"+elem.subtype+" ( go = new GameObject())";
						// go.transform.parent = levelObject.transform;

						// editor prefab!! = prefabEditorDummyGameObject
						if (elPrefab.prefabEditorDummyGameObject!=null)  {

							//	if (elem.prefabPredefinedArguments==null) Debug.Log(". LeveLElementOption NULL"+elem.prefabPredefinedArguments);
							// else Debug.Log(". LeveLElementOption FOUND!"+elem.prefabPredefinedArguments);

							// Debug.Log(". LeveLElementOption ??? "+elem.type+" / "+elem.subtype+"  "+elem.argument);

							// argument?
							// no alternative elements for argument

							// default
							if (elem.prefabPredefinedArguments.Length==0) {
								go=Instantiate(elPrefab.prefabEditorDummyGameObject, new Vector3(elem.position.x,elem.position.y,elem.position.z), re) as GameObject;
								go.name = "FOUNDEditorPrefab";

								// use normal gameobject if 
								if (elPrefab.editorIsGround) {
									if (editorSelected!=elem) {
										// Debug.Log("IS GROUND!!! "+elPrefab.type);
										// update with ingameobject ! as ground!!!
										Destroy(go);
										go=Instantiate(elPrefab.prefabGameObject, new Vector3(elem.position.x,elem.position.y,elem.position.z), re) as GameObject;
										go.name = "editorXYZGROUND";	
									} 
								}

								GameElementBased geb = go.GetComponent<GameElementBased>();
								// Debug.Log ("---"+trb.ToString ());
								if (geb!=null) {
									geb.SetGameLogic(gameLogic);
									geb.SetGameElement( elem );
									// trb.SetGameElementArgument(argument);
									// Debug.Log ("ARGUMENT 2: "+trb.argument);
								}

								// options for EVALUATIONS etc.
							} 
							else  {
								// Debug.Log(". LeveLElementOption FOUND! "+elem.argument+" "+elem.prefabPredefinedArguments);
								// editor ...
								bool found=false;
								LevelElementOption leo;
								// editor: no random predefineds!
								if (!elPrefab.randomPredefineds) {
									for (int ix = 0; ix < elem.prefabPredefinedArguments.Length; ix ++) {
										leo = elem.prefabPredefinedArguments[ix];
										// Debug.Log(ix+". LeveLElementOption "+leo.argument+" vs "+elem.argument);
										if (leo.argument.Equals(elem.argument)) {
											if (leo.editorPrefab!=null) {
												found=true;
												go=Instantiate(leo.editorPrefab, new Vector3(elem.position.x,elem.position.y,elem.position.z), re) as GameObject;
												go.name = "NotFound19";

												// use normal gameobject if 
												if (elPrefab.editorIsGround) {
													if (editorSelected!=elem) {
														// Debug.Log("IS GROUND!!! "+elPrefab.type);
														// update with ingameobject ! as ground!!!
														Destroy(go);
														go=Instantiate(leo.gameobjectPrefab, new Vector3(elem.position.x,elem.position.y,elem.position.z), re) as GameObject;
														go.name = "editorXYZGROUNDVARIANT";	
													} 
												}

											}
										}
									} }
								// editor: random predefineds
								if (elPrefab.randomPredefineds) {
									int max = elem.prefabPredefinedArguments.Length;
									if (elem.prefabPredefinedArguments.Length>0) {
										int randomIndex = UnityEngine.Random.Range(0,max);
										leo = elem.prefabPredefinedArguments[randomIndex];
										if (leo.editorPrefab!=null) {
											found=true;
											go=Instantiate(leo.editorPrefab, new Vector3(elem.position.x,elem.position.y,elem.position.z), re) as GameObject;
											go.name = "FoundRandomPredefined";

											// use normal gameobject if 
											if (elPrefab.editorIsGround) {
												if (editorSelected!=elem) {
													// Debug.Log("IS GROUND!!! "+elPrefab.type);
													// update with ingameobject ! as ground!!!
													Destroy(go);
													go=Instantiate(leo.gameobjectPrefab, new Vector3(elem.position.x,elem.position.y,elem.position.z), re) as GameObject;
													go.name = "editorXYZGROUNDVARIANT";	
												} 
											}

										}
									}
								}

								// found 
								if (!found) {
									go=Instantiate(elPrefab.prefabEditorDummyGameObject, new Vector3(elem.position.x,elem.position.y,elem.position.z), re) as GameObject;
									go.name = "NotFoundDUMMY!";
									// prefab
									if (elPrefab.editorIsGround) {
										if (editorSelected!=elem) {
											// Debug.Log("IS GROUND!!! "+elPrefab.type);
											// update with ingameobject ! as ground!!!
											Destroy(go);
											go=Instantiate(elPrefab.prefabGameObject, new Vector3(elem.position.x,elem.position.y,elem.position.z), re) as GameObject;
											go.name = "editorXYZGROUNDVARIANT";	
										} 
									}
								}
							}

							// size
							if (elem.size!=1.0f) {
								go.transform.localScale = elem.size * go.transform.localScale;
							}
							elem.gameObject=go;
							if (!elem.name.Equals("")) { go.name=""+elem.name; }
							go.transform.parent = levelObject.transform; 

						} else {

							if (elem.prefabPredefinedArguments!=null)
							if (elem.prefabPredefinedArguments.Length>0) {
								go=Instantiate(elPrefab.prefabGameObject, new Vector3(elem.position.x,elem.position.y,elem.position.z), re) as GameObject;
								go.name = "NOEDITORPREFABATALLPrefabEditor!!!";

								// size
								if (elem.size!=1.0f) {
									go.transform.localScale = elem.size * go.transform.localScale;
								}
								elem.gameObject=go;
								if (!elem.name.Equals("")) { go.name=""+elem.name; }
								go.transform.parent = levelObject.transform; 
							}



						}

					}

					// game ...

					// Debug.Log("Could find Type("+elem.type+"/"+elem.subtype+") has prefab!");
					if (gameLogic !=null && gameLogic.modal!=GameLogic.GameLogicModal.Editor ) {
						// Debug.Log("PREFAB");			
						// Debug.Log ("[LevelEditor] CREATE["+elem.name+"/"+elem.type+"."+elem.subtype+"/"+elem.release+"]");

						// only instiante pure releases (no waits)
						if (elem.release.Equals ("")) {
							GameObject go=null;

							if (elem.prefabPredefinedArguments.Length==0) {
								go = Instantiate(elPrefab.prefabGameObject, new Vector3(elem.position.x,elem.position.y,elem.position.z), re) as GameObject;
								go.name = "NotFound2";
							}

							// are there some alternatives?
							// variations?
							if (elem.prefabPredefinedArguments.Length>0) {
								bool found=false;
								LevelElementOption leo;

								// ingame: predefineds
								if (!elPrefab.randomPredefineds) {
									for (int ix = 0; ix < elem.prefabPredefinedArguments.Length; ix ++) {
										leo = elem.prefabPredefinedArguments[ix];
										// Debug.Log(ix+". LeveLElementOption "+leo.argument+" vs "+elem.argument);
										if (leo.argument.Equals(elem.argument)) {
											if (leo.gameobjectPrefab!=null) {
												found=true;
												go=Instantiate(leo.gameobjectPrefab, new Vector3(elem.position.x,elem.position.y,elem.position.z), re) as GameObject;
												go.name = "NotFound9GameObject";

											}
										}
									} 
								}
								// ingame: random predefineds
								if (elPrefab.randomPredefineds) {
									int max = elem.prefabPredefinedArguments.Length;
									if (elem.prefabPredefinedArguments.Length>0) {
										int randomIndex = UnityEngine.Random.Range(0,max);
										leo = elem.prefabPredefinedArguments[randomIndex];
										if (leo.gameobjectPrefab!=null) {
											found=true;
											go=Instantiate(leo.gameobjectPrefab, new Vector3(elem.position.x,elem.position.y,elem.position.z), re) as GameObject;
											go.name = "FoundRandomPredefined";

										}
									}
								}

								if (!found) {
									go=Instantiate(elPrefab.prefabGameObject, new Vector3(elem.position.x,elem.position.y,elem.position.z), re) as GameObject;
									go.name = "NotFoundDUMMY!";
								}
							}

							// size
							if (elem.size!=1.0f) {
								go.transform.localScale = elem.size * go.transform.localScale;
							}
							// rotation
							if (elem.rotation!=0.0f) {
								//									go.transform.Rotate ();
							}

							elem.gameObject=go;
							go.transform.parent = levelObject.transform; 
							if (!elem.name.Equals("")) { go.name=""+elem.name; }

							/*
								// register if type is player
							// player1/player2
							if (elem.subtype.Equals ("player1")) {
								if (go!=null) {
									gameLogic.RegisterPlayerForRunning(go,1);
								}
							}
							if (elem.subtype.Equals ("player2")) {
								if (go!=null) {
									gameLogic.RegisterPlayerForRunning(go,2);
								}
							}
							*/
							// specials arguments
							string argument = elem.argument;
							TriggerBase trb = go.GetComponent<TriggerBase>();
							// Debug.Log ("---"+trb.ToString ());
							if (trb!=null) {
								trb.SetGameElement( elem );
								// trb.SetGameElementArgument(argument);
								// Debug.Log ("ARGUMENT 2: "+trb.argument);
							}

							argument = elem.argument;
							GameElementBased geb = go.GetComponent<GameElementBased>();
							// Debug.Log ("---"+trb.ToString ());
							if (geb!=null) {
								geb.SetGameLogic(gameLogic);
								geb.SetGameElement( elem );
								// trb.SetGameElementArgument(argument);
								// Debug.Log ("ARGUMENT 2: "+trb.argument);
							}

							// action notification
							/*
								ActionNotification acn = go.GetComponent<ActionNotification>();
								if (acn!=null) {
									if (!argument.Equals ("")) {
										acn.AddNotification(argument, go.transform.position, NotificationPriority.ThrowAwayAfterProcessing );
									} else {
										Debug.LogError ("[Action.Notification] DoNotification( EMPTY ) ");
									}
								}
								*/

							// Debug.Log("Could find Type("+elem.type+"/"+elem.subtype+").AddedAt("+elem.position.x+","+elem.position.y+","+elem.position.z+")");
						}
					}

				}
			}

		}

	}

	// only deactivate it!!
	public void DeactivateElement( GameElement elem ) {
		if (elem != null) {
			elem.release = "wait";
			if (elem.gameObject != null) {
				Destroy (elem.gameObject);
			}
			elem.gameObject= null;
		}
	}

	public void ActivateElement( GameElement elem ) {
		if (elem!=null) {
			elem.release = "";
			if (elem.gameObject!=null) {
				GameElementBased geb = elem.gameObject.GetComponent<GameElementBased>();
				// Debug.Log ("---"+trb.ToString ());
				if (geb!=null) {
					geb.OnGameStart( );
				}
			} 
			UpdateElementVisual (elem);
		}
	}

	// destroy object
	public void RemoveElement( GameElement elem ) {

		if (elem != null) {
			if (elem.gameObject != null) {
				Destroy (elem.gameObject);
				elem.gameObject = null;
			}
			arrLevel.Remove (elem);
		}
	}

	/*
	public ArrayList GetGameElementsByName( string name ) {

		ArrayList arr = new ArrayList ();

		for (int i=0; i<arrLevel.Count; i++) {
			GameElement gx=(GameElement) arrLevel[i];
			if (gx.name.Equals (name)) {
				arr.Add (gx);
			}
		}

		return arr;
	}
	*/

	public ArrayList GetGameElementsByTypeAndSub( string type, string typesub ) {

		ArrayList arr = new ArrayList ();

		GameElement gx;
		for (int i=0; i<arrLevel.Count; i++) {
			gx=(GameElement) arrLevel[i];
			if (gx.type.Equals (type)) {
				if ((gx.subtype.Equals (typesub))||(typesub.Equals ("*"))) {
					arr.Add (gx);
				}
			}
		}

		return arr;
	}

	public ArrayList GetGameElementsByTypeAndSubSearch( string type, string typesub, string argument, string search ) {

		ArrayList arr = new ArrayList ();

		GameElement gx;
		for (int i=0; i<arrLevel.Count; i++) {
			gx=(GameElement) arrLevel[i];
			if (gx.type.Equals (type)) {
				if ((gx.subtype.Equals (typesub))||(typesub.Equals ("*"))) {
					if ( (gx.argument.Equals (""+argument)) || (gx.argument.Equals ("*"))  ) {
						// search ... add ...
						if (search.Equals("*")) arr.Add (gx);
						if (!search.Equals("*")) {
							if (gx.name.IndexOf(search)!=-1) arr.Add (gx);
							if (gx.argumentsub.IndexOf(search)!=-1) arr.Add (gx);
						}
					}
				}
			}
		}

		return arr;
	}

	public ArrayList GetGameElementsByTypeAndSubTimeSorted(  string type, string typesub ) {
		ArrayList arr = GetGameElementsByTypeAndSub(  type,  typesub );
		arr = ArraySortByTimed( arr );
		return arr;
	}

	public ArrayList GetAllGameElementsByTypeAndSubTimeSorted( ) {
		ArrayList arr = GetGameElementsByTypeAndSub(  "camcut",  "caption" );

		ArrayList arrAdd = GetGameElementsByTypeAndSub(  "camcut",  "timedpicture" );
		for (int z=0;z<arrAdd.Count;z++) {
			GameElement ge = (GameElement) arrAdd[z];
			arr.Add(ge);
		}
		arrAdd = GetGameElementsByTypeAndSub(  "camcut",  "timedexit" );
		for (int z=0;z<arrAdd.Count;z++) {
			GameElement ge = (GameElement) arrAdd[z];
			arr.Add(ge);
		}
		arrAdd = GetGameElementsByTypeAndSub(  "camcut",  "timednotification" );
		for (int z=0;z<arrAdd.Count;z++) {
			GameElement ge = (GameElement) arrAdd[z];
			arr.Add(ge);
		}
		arrAdd = GetGameElementsByTypeAndSub(  "camcut",  "timedloop" );
		for (int z=0;z<arrAdd.Count;z++) {
			GameElement ge = (GameElement) arrAdd[z];
			arr.Add(ge);
		}

		arr = ArraySortByTimed( arr );

		return arr;
	}

	// 
	public GameElement GetGameElementTimedActual( float timex, ArrayList arrTimedGameElementsTimeSorted ) {

		if (arrTimedGameElementsTimeSorted!=null) {

			ArrayList arr = new ArrayList();
			GameElement ge;
			GameElement genext;
			for (int z=0;z<arrTimedGameElementsTimeSorted.Count;z++) {
				ge = (GameElement) arrTimedGameElementsTimeSorted[z];
				if (timex>ge.timed) {
					// ok let's check next ..
					int next = z+1;
					if (next>(arrTimedGameElementsTimeSorted.Count-1)) {
						return ge;
					} else {
						genext = (GameElement) arrTimedGameElementsTimeSorted[next];
						if (timex<genext.timed) {
							return ge;
						}
					}
				}
			}

		}

		return null;
	}


	public ArrayList ArraySortByTimed( ArrayList arr ) {

		if (arr.Count==0) return arr;

		GameElement a;
		GameElement b;

		for (int o = 0; o<arr.Count; o++) {
			for (int oo = 0; oo<arr.Count; oo++) {
				a =	(GameElement) arr[o];
				b =	(GameElement) arr[oo];
				if (a.timed<b.timed) {
					arr[o] = b;
					arr[oo] = a;
				}
			}
		}

		return arr;
	}

	public ArrayList GetGameElementsByNotCleanName( ) {

		ArrayList arr = new ArrayList ();

		for (int i=0; i<arrLevel.Count; i++) {
			GameElement gx=(GameElement) arrLevel[i];
			if (!gx.name.Equals ("")) {
				if (!gx.name.Equals (" ")) {
					arr.Add (gx);
				}
			}
		}

		return arr;
	}

	public ArrayList GetGameElementsByName( String name ) {

		ArrayList arr = new ArrayList ();

		for (int i=0; i<arrLevel.Count; i++) {
			GameElement gx=(GameElement) arrLevel[i];
			if (gx.name.Equals (name)) {
				arr.Add (gx);
			}
		}

		return arr;
	}

	public GameElement GetGameElementByName( String name ) {

		for (int i=0; i<arrLevel.Count; i++) {
			GameElement gx=(GameElement) arrLevel[i];
			if (gx.name.Equals (name)) {
				return gx;
			}
		}

		return null;
	}

	// xyz,def,abc
	public ArrayList GetGameElementsByString( string csvpath ) {
		ArrayList arr = new ArrayList();
		string[] arrNotifications = csvpath.Split(',');
		for (int x=0;x<arrNotifications.Length;x++) {
			string gamelementName = arrNotifications[x];
			GameElement gae = GetGameElementByName( gamelementName );
			if (gae!=null) {
				arr.Add(gae);
			}
		}
		return arr;
	}


	void ClearElements() {
		// Debug.Log ("ClearElements()");
		int counted = arrLevel.Count;
		for (int i=counted-1; i>=0; i--) {
			GameElement gx=(GameElement) arrLevel[i];
			RemoveElement(gx);
		}
	}


	// Editor

	// raster
	int editorRaster=0;
	float[] arrRasters = { 0.0f, 1.0f, 2.0f } ; // { 0.0f, 0.5f, 1.0f, 1.5f, 2.0f, 2.5f, 3.0f, 3.5f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f };
	int editorDegree = 0;

	void SetRasterIndex( int index ) { // Index!
		editorRaster = index;
		AddEditorMessage("Raster: Index: "+editorRaster+" Raster:"+GetRaster());
	}

	int GetRasterIndexFor( float rasterX ) {
		for (int i=0;i<arrRasters.Length;i++) {
			if (rasterX==arrRasters[i]) {
				return i;
			}
			if (i<(arrRasters.Length-1)) {
				if (rasterX>arrRasters[i]) {
					if (rasterX<arrRasters[i+1]) {
						return i;
					}
				}
			}
		}

		return 0;
	}

	// not used anymore
	float GetRaster() {

		/*
		// with shift use also a raster
		if ((Input.GetKey ("left shift"))||(Input.GetKey ("right shift"))) {
			return 1.0f;
		}
		*/

		return arrRasters[editorRaster];
	}

	// tools
	string editorTool= "CREATE"; 
	string editorToolSub = "";
	void SetTool(string ieditorTool) {
		EmptyEditorCursorPreview();
		// deselect ...
		SetSelectedElement( null );
		ActivateCursorPreview(ieditorTool=="CREATE");
		editorTool = ieditorTool;
		if (editorTool.Equals ("SPLIT")) {
			editorToolSub="right";
		}
	}
	string[] arrEditorTools={"CREATE","EDIT","MOVE","SPLIT","DELETE","EVALU"};	

	// special in tools
	// remove
	void RemoveSelectedElement() {
		GameElement elisa = editorSelected;
		// editorSelected = null;
		SetSelectedElement( null );
		RemoveElement(elisa);
	}

	// edit
	void SetSelectedElement( GameElement ga ) {

		EmptyEditorCursorPreview();

		GameElement previousElementX = null;

		if (editorSelected==null) {
			// deselect all
			editDetailName = "";
		}

		// deselect old!
		if (editorSelected!=null) {
			UpdateElementVisual(editorSelected);
			previousElementX= editorSelected;
		}   

		// Debug.Log ("SetSelectedElement()");
		editorSelected = ga;
		// update old element
		if (previousElementX!=null) {
			UpdateElementVisual(previousElementX); 
		}
		if (ga==null) {
			editorArea = "";
			editorSubArea = "";
			SetSubEditorArea( editorSubArea );

			// disable preview!!!
			// SetEditorCursorPreviewToPrefab(null, 1.0f);
			EmptyEditorCursorPreview();
		}
		if (ga!=null) {
			editorArea = ga.type;
			SetSelectedElementToGUI ();

			// also update preview for recognition
			editorArea = ga.type;
			editorSubArea = ga.subtype;
			SetSubEditorArea( editorSubArea );

			UpdateElementVisual(ga); 
		}

	}
	// update to 
	void SetSelectedElementToGUI() {
		EmptyEditorCursorPreview();
		//editDetailX = ""+ editorSelected.position.x;
		//editDetailY = ""+ editorSelected.position.y;
		editDetailName = ""+ editorSelected.name;
		editDetailArgument = ""+ editorSelected.argument;
		editDetailArgumentSub = "" + editorSelected.argumentsub;
	}
	void StoreSelectedElement(  ) {
		EmptyEditorCursorPreview();
		SetSelectedElementFromGUI ();
	}
	// update to 
	void SetSelectedElementFromGUI() {
		EmptyEditorCursorPreview();

		//		editDetailSelected.position.x=editDetailX;
		// editorDetailSelected.position.x=editDetailY;
		editorSelected.name=editDetailName;
		editorSelected.argument=editDetailArgument;
		editorSelected.argumentsub = editDetailArgumentSub;
	}

	// special tools: edit
	/*
	string editDetailX="";
	string editDetailY="";
  */

	string createName = "";
	string editDetailName="";
	string editDetailArgument="";
	string editDetailArgumentSub = "";

	// special tools: move
	string editorToolMove="";

	// special tools: create game elements
	string editorArea = "item";
	string editorSubArea = "apple";

	void SetEditorArea(string ieditorArea) {
		editorArea = ieditorArea;
		// get next sub
	}
	void SetSubEditorArea(string isubeditorArea) {
		editorSubArea = isubeditorArea;
		// get ...
		editorPrefab = GetElementType (editorArea,editorSubArea);
		// update gui!
		// editorPrefab
		if (editorPrefab!=null) {
			float sizeIt = editorPrefab.editorDisplaySize;
			if (sizeIt==0.0f) {
				sizeIt = 1.0f;
			}
			GameObject prefabObj = editorPrefab.prefabEditorDummyGameObject;
			if (prefabObj==null) {
				prefabObj = editorPrefab.prefabGameObject;
			}
			SetEditorPreviewToPrefab( prefabObj, sizeIt);

			if(editorTool.Equals("CREATE")) SetEditorCursorPreviewToPrefab( prefabObj, editorPrefab.size);
		} else {
			// Debug.LogError("SetSubEditorArea() Could not find object for this ...");
		}
	}

	public void SetEditorPreviewToPrefab( GameObject prefab , float scaling) {

		// Debug.Log("SetEditorPreviewToPrefab()");

		// find preview
		GameObject preview = GameObject.Find("editorpreview");

		// delete old one 
		foreach (Transform child in preview.transform) {
			Destroy(child.gameObject);
			break;
		}

		if (preview!=null) {


			// Debug.Log("SetEditorPreviewToPrefab(){ preFabFound = "+prefab+" }");

			// create one an add it 
			if (prefab!=null) {

				GameObject newPreview=Instantiate(prefab, new Vector3(preview.transform.position.x,preview.transform.position.y,preview.transform.position.z), new Quaternion()) as GameObject;
				newPreview.name = "NotFound3";

				float scaleFactor = 0.03f * scaling;
				newPreview.transform.localScale = new Vector3(scaleFactor,scaleFactor,scaleFactor);
				newPreview.transform.parent = preview.transform;
			}
		} else {
			Debug.LogError("SetEditorPreviewToPrefab() Could not find editorpreview-Object!");
		}
	}

	public void EmptyEditorCursorPreview() {

		// find preview
		GameObject preview = GameObject.Find("editorcursorpreview");

		// delete old one 
		foreach (Transform child in preview.transform) {
			Destroy(child.gameObject);
		}

	}

	public void SetEditorCursorPreviewToPrefab( GameObject prefab , float scaling) {

		// Debug.Log("SetEditorPreviewToPrefab()");

		// find preview
		GameObject preview = GameObject.Find("editorcursorpreview");

		// delete old one 
		EmptyEditorCursorPreview();

		if (preview!=null) {


			// Debug.Log("SetEditorPreviewToPrefab(){ preFabFound = "+prefab+" }");

			// create one an add it 
			if (prefab!=null) {
				// rotation 
				Quaternion re = new Quaternion();
				if (editorDegree!=0.0f) {
					re = Quaternion.Euler(0, editorDegree, 0);
				}

				GameObject newPreview=Instantiate(prefab, new Vector3(preview.transform.position.x,preview.transform.position.y,preview.transform.position.z), re) as GameObject;
				newPreview.name = "NotFound4";

				float scaleFactor = 1f * scaling;
				newPreview.transform.localScale = new Vector3(scaleFactor,scaleFactor,scaleFactor);
				/*// size
				if (scaling!=1.0f) {
					newPreview.transform.localScale = elem.size * go.transform.localScale;
				}*/
				newPreview.transform.parent = preview.transform;


			}
		} else {
			Debug.LogError("SetEditorPreviewToPrefab() Could not find editorcursorpreview-Object!");
		}
	}

	// editor Text
	string editorLogText="";


	// GameElement
	GameElement editorPrefab; // [CREATE]: selected prefab
	GameElement editorSelected; // [EDIT]: which element is selected?
	// GameElement editorChangeToSelection = new GameElement(); // [EDIT]: change element type to ... 

	public GUIStyle editorDefault;

	public GUIStyle editorLabel;

	public GUIStyle editorBackground;
	public GUIStyle editorWebLevels;
	public GUIStyle editorInspectorBackground;

	public GUIStyle editorButtonStyle;
	public GUIStyle editorButtonStyleBig;
	public GUIStyle editorButtonStyleNotActive;
	public GUIStyle editorButtonActiveStyle;

	public GUIStyle editorSwitchButtonStyle;
	public GUIStyle editorSwitchButtonStyleActive;

	public GUIStyle editorButtonTypeStyle;
	public GUIStyle editorButtonTypeStyleNotActive;
	public GUIStyle editorButtonTypeSubStyle;
	public GUIStyle editorButtonTypeSubStyleNotActive;

	public GUIStyle editorButtonStyleWeb;
	public GUIStyle editorButtonActiveStyleWeb;


	public GUIStyle editorLoading;


	//	public GUIStyle editorDeleteStyle;
	public GUIStyle editorComment;

	public GUIStyle editorIconGUI;

	public GUIStyle editorElementType;

	public GUIStyle guiEvaluation;

	public GUIStyle editorTitleStyle;

	public Texture2D editorEditImage;
	public Texture2D editorSelectedImage;
	public Texture2D editorMoveImage;
	public Texture2D editorDeleteImage;

	public Texture2D editorAffectedImage;


	// Insert & delete line
	float linePos=4.0f; // x
	float linePosZ = 4.0f;
	float lineWidth=1.0f;

	void UpdateVerticalLinePos() {

		// linePos=scroll-3.0f+lineWidth;
		linePos = editorCursorActualPoint.x;
		linePosZ = editorCursorActualPoint.z;

	}

	void InsertVerticalLine() {
		UpdateVerticalLinePos ();
		// bigger
		for (int a=0;a<arrLevel.Count;a++) {
			GameElement gelement=(GameElement)arrLevel[a];
			if (gelement.position.x>linePos) {
				gelement.position.x=gelement.position.x+lineWidth;
				UpdateElementVisual(gelement);
			}
		}
	}

	void InsertHorizontalLine() {
		UpdateVerticalLinePos ();
		// bigger
		for (int a=0;a<arrLevel.Count;a++) {
			GameElement gelement=(GameElement)arrLevel[a];
			if (gelement.position.z>linePosZ) {
				gelement.position.z=gelement.position.z+lineWidth;
				UpdateElementVisual(gelement);
			}
		}
	}

	void RemoveVerticalLine() {
		UpdateVerticalLinePos ();
		for (int a=(arrLevel.Count-1);a>=0;a--) {
			GameElement gelement=(GameElement)arrLevel[a];
			if ((gelement.position.x>linePos)&&(gelement.position.x<(linePos+lineWidth))) {
				RemoveElement(gelement);
			}
		}
		for (int a=0;a<arrLevel.Count;a++) {
			GameElement gelement=(GameElement)arrLevel[a];
			if (gelement.position.x>linePos) {
				gelement.position.x=gelement.position.x-lineWidth;
				UpdateElementVisual(gelement);
			}
		}
	}

	void RemoveHorizontalLine() {
		UpdateVerticalLinePos ();
		for (int a=(arrLevel.Count-1);a>=0;a--) {
			GameElement gelement=(GameElement)arrLevel[a];
			if ((gelement.position.z>linePosZ)&&(gelement.position.z<(linePosZ+lineWidth))) {
				RemoveElement(gelement);
			}
		}
		for (int a=0;a<arrLevel.Count;a++) {
			GameElement gelement=(GameElement)arrLevel[a];
			if (gelement.position.z>linePosZ) {
				gelement.position.z=gelement.position.z-lineWidth;
				UpdateElementVisual(gelement);
			}
		}
	}

	// Load and Save 
	// LoadLevel
	// load game level
	/*
	 * public void LoadGameLevel( int ilevel ) {
		LoadLevel (ilevel);

	}
	*/



	// base function
	void LoadLevel( int level  ) {

		AddEditorMessage("LoadLevel("+level+")");

		// ingame time (if neede)
		SetIngameTimeToLoading();

		SetSelectedElement(null);

		EmptyEditorCursorPreview();

		ClearLevel();

		// all Level
		if (level!=allLevel) {
			LoadLevelLowLevel(  allLevel,  "", "", ""+allLevel );
		}

		// Correct Level
		LoadLevelLowLevel(level, "", "", "self" ); // load a level raw

		historyIndexMinus = 0;

		SetSelectedElement(null);

		// update 
		//	UpdateShowEvaluationData ();

		Debug.Log("gameLogic: "+gameLogic);
		if (gameLogic && gameLogic.modal == GameLogic.GameLogicModal.Editor) {
			AddToEditorHistory("LoadLevel");
		}

		// IngameTime
		StartIngameTime();
	}

	void LoadEvaluationLevels( ) {

		// Debug.Log("[LevelEditor]LoadEvaluationLevels()");

		// search all local
		string[] arrLevelFiles = GetExistingEvaluationLevelFiles( actualEditorLevel );
		for (int i = 0; i < arrLevelFiles.Length; i++ ) {
			string fileN = arrLevelFiles[i];
			// Debug.Log(i+". "+fileN);

			Match match = Regex.Match(fileN, @"level"+actualEditorLevel+"_(\\d+)_(\\d+).", RegexOptions.IgnoreCase);

			if (!editorEvaluationFilter.Equals("all")) {

				if (editorEvaluationFilter.Equals("player")) {

					// Debug.Log("editorEvaluationPlayerId: "+editorEvaluationPlayerId);

					match = Regex.Match(fileN, @"level"+actualEditorLevel+"_("+editorEvaluationPlayerId+")_(\\d+).", RegexOptions.IgnoreCase);

				}

				// editorEvaluationSessionId
				if (editorEvaluationFilter.Equals("player.session")) {

					// Debug.Log("editorEvaluationPlayerId: player.session "+editorEvaluationPlayerId);
					match = Regex.Match(fileN, @"level"+actualEditorLevel+"_("+editorEvaluationPlayerId+")_("+editorEvaluationSessionId+").", RegexOptions.IgnoreCase);

				}

			}

			// Here we check the Match instance.
			if (match.Success)
			{
				// Finally, we get the Group value and display it.
				string playerId = match.Groups[1].Value;
				string sessionId = match.Groups[2].Value;
				// Debug.Log("-"+i+". FOUND: ("+playerId+"/"+sessionId+")");

				LoadLevel ( actualEditorLevel, playerId, sessionId );
			}
		}

		// search web


	}

	void LoadLevel( int level,  string playerId, string sessionId ) {
		LoadLevelLowLevel(  level,   playerId,  sessionId,  "self" );
	}

	void AppendLevel( int level,  string playerId, string sessionId, string sourcex ) {
		LoadLevelLowLevel(  level,   playerId,  sessionId, sourcex );
	}

	// AppendLevel( int level,  string playerId, string sessionId, string sourcex )

	// playerId, sessionId
	void LoadLevelLowLevel( int level,  string playerId, string sessionId, string source ) {

		bool debugThis = false ;

		bool flagEvaluationTemp = false; 
		if (!playerId.Equals("")) {
			flagEvaluationTemp = true; 
		}

		editorLogText = ""+DateTime.Now.ToString("LOADED LEVEL "+level+": HH:mm:ss");
		AddEditorMessage( editorLogText );

		// add evaluation 
		string addEvaluationsFolder = ""; // all 
		string addFileNameAddOn = "";		
		if (flagEvaluationTemp) {
			addEvaluationsFolder = evaluationFolder+Path.DirectorySeparatorChar; 
			addFileNameAddOn = "_"+playerId+"_"+sessionId;
		}

		try {

			string remoteAddOnPath = "";
			if (CheckLevelTypeWeb()) {
				remoteAddOnPath = GetRemotePath();
			}
			try {
				string jsonText = System.IO.File.ReadAllText( remoteAddOnPath+addEvaluationsFolder+ "level"+level+addFileNameAddOn+".txt");
				JSONObject jsonObj = new JSONObject(jsonText);
				// array
				// Debug.Log ("Load().Size="+jsonObj.list.Count);
				int counter = 0;
				foreach(JSONObject listObj in jsonObj.list){
					GameElement ge=new GameElement();
					// get the default members
					GameElement typege=new GameElement();
					typege.GetObjectFromJSON(listObj);

					// ingame 
					bool foundType = false;
					GameElement gaType = GetElementType ( typege.type, typege.subtype );
					if (gaType != null) {
						ge = gaType.Copy ();
						ge.GetObjectFromJSON(listObj);
						foundType = true;
					} else {
						// what happens to the not recognized?
						// take as normal!
						ge = new GameElement();
						ge.GetObjectFromJSON(listObj);
						// ok an now ..
						ge.descError = "NotFound";
						if (debugThis) 	Debug.Log("LoadLevel() // ---- element("+typege.type+","+typege.subtype+") ");
					}
					// source
					if (source.Equals("")) {
						ge.ingameSource = "self";
					} else {
						ge.ingameSource = ""+source;
					}

					if (debugThis) Debug.Log("LoadLevel() // "+counter+". "+ge.type+"/"+ge.subtype+" [source: "+ge.ingameSource+"]-- found // "+foundType);

					// add it
					bool flagAdd = false;
					if (!flagEvaluationTemp) { if (!ge.type.Equals ("evaluation")) { flagAdd=true; } }
					if (flagEvaluationTemp) { if (ge.type.Equals ("evaluation")) { flagAdd=true; } } 
					if (flagAdd) {
						AddElement (ge);
					}

					counter++;
				}
			} catch( Exception e ) {
				Debug.LogError("_LevelEditor.LoadGameLevel() (1) // ERROR: .LOAD()"+e  );
			}


			// save it now ... 
			// SaveLevel ( 2001 );

			// Game Start
			// ok - now OnGameStart ...
			try {
				GameElement gelement;
				for (int a=0;a<arrLevel.Count;a++) {
					gelement=(GameElement)arrLevel[a];
					if (gelement!=null) {
						if (gelement.gameObject!=null) {
							// Debug.Log("_LevelEditor.LoadGameLevel() (2) // "+gelement.type+"/"+gelement.subtype+" .OnGameStart()");
							GameElementBased geb = gelement.gameObject.GetComponent<GameElementBased>();
							// Debug.Log ("---"+trb.ToString ());
							if (geb!=null) {
								geb.OnGameStart( );
							}
					} } else {

					}
				}
			} catch( Exception e ) {
				Debug.LogError("_LevelEditor.LoadGameLevel() (2) // ERROR .OnGameStart()" + e  );
			}


		} catch( Exception e ) {
			Debug.LogError("_LevelEditor.LoadGameLevel() (3) // CouldNotLoadLevel "+level + " --- "+e );
		}

		// update visualisation
		UpdateRelationVisualisationAndCheckError ();
	}

	void SaveLevel( int level ) {

		SaveLevel( level, false );
	}

	// save the level or save all evaluation elements
	void SaveLevel( int level, bool iflagEvaluation ) {

		// store and load
		// GetJSONObject
		JSONObject arrElementsJSON = new JSONObject(JSONObject.Type.ARRAY);
		for (int a=0;a<arrLevel.Count;a++) {
			GameElement gelement=(GameElement)arrLevel[a];

			bool flagSave = false;
			if (!iflagEvaluation) { if (!gelement.type.Equals ("evaluation")) { flagSave=true; } }
			if (iflagEvaluation) { 

				if (gelement.type.Equals ("evaluation")) { 

					// version 1.0
					// flagSave=true; 

					if (gelement.evaluationPlayerId.Equals(evaluationPlayer.playerId)) {
						if (gelement.evaluationSessionId==evaluationPlayer.sessionId) {
							flagSave=true; 
						}
					}

				} 


			} 
			if (flagSave) {
				// only own levelelements
				if ((gelement.ingameSource.Equals(""))||(gelement.ingameSource.Equals("self"))) {
					JSONObject gelementJSON=gelement.GetJSONObject();
					// Debug.Log (gelementJSON.Print ());
					arrElementsJSON.Add (gelementJSON);
				}
			}
		}
		string encodedString = arrElementsJSON.Print();
		// save to file
		//  string text = System.IO.File.ReadAllText("myfile.txt");

		// folder?
		string folderName = "leveltemp";
		if (!Directory.Exists (""+folderName)) {
			Directory.CreateDirectory ("" + folderName);
		}

		// evaluation
		string evaluationFolderAddOn="";
		string fileEvaluationAddOn = "";
		if (iflagEvaluation)  { 
			evaluationFolderAddOn = evaluationFolder+Path.DirectorySeparatorChar; 
			fileEvaluationAddOn = fileEvaluationAddOn + "_"+evaluationPlayer.playerId+"_"+evaluationPlayer.sessionId;
		}

		// write

		// add on for web
		// remoteAreaEdit+"/"+remoteAutorEdit
		string remoteAddOnPath = "";
		if (CheckLevelTypeWeb()) {
			remoteAddOnPath = GetRemotePath();
		}
		System.IO.File.WriteAllText(remoteAddOnPath+evaluationFolderAddOn+"level"+level+fileEvaluationAddOn+".txt",""+encodedString);



		// save to file ...
		if (!iflagEvaluation) {
			System.IO.File.WriteAllText(folderName+ Path.DirectorySeparatorChar + "level"+level+"_"+DateTime.Now.ToString("yyyyMMddHHmmss")+".txt",""+encodedString);
		}

		editorLogText = ""+DateTime.Now.ToString("SAVED: HH:mm:ss");
		AddEditorMessage( editorLogText );

		// Debug.Log(encodedString);

	}

	// REMOVE AND INSERT
	void RemoveActualLevel() {
		Debug.Log("LevelEditor.RemoveActualLevel() "+actualEditorLevel);

		for (int z=actualEditorLevel;z<(maxMaxLevel-1);z++) {
			string remoteAddOnPath = "";
			if (CheckLevelTypeWeb()) {
				remoteAddOnPath = GetRemotePath();
			}
			string path = remoteAddOnPath + "level"+(z+1)+".txt";
			if (File.Exists(path)) {
				// File.Delete(path);
				string pathTo = remoteAddOnPath + "level"+z+".txt";
				Debug.Log("LevelEditor.RemoveActualLevel() "+path+">"+pathTo);
				if (File.Exists(pathTo)) {
					File.Delete(pathTo);
				}
				File.Move(path,pathTo);
			}
		}
	}
	void InsertLevelAfterActualLevel() {
		Debug.Log("LevelEditor.InsertLevelAfterActualLevel() "+actualEditorLevel);
		for (int z=(maxMaxLevel-1);z>actualEditorLevel;z--) {
			string remoteAddOnPath = "";
			if (CheckLevelTypeWeb()) {
				remoteAddOnPath = GetRemotePath();
			}
			string path = remoteAddOnPath + "level"+z+".txt";
			if (File.Exists(path)) {
				string pathTo = remoteAddOnPath + "level"+(z+1)+".txt";
				if (File.Exists(pathTo)) {
					File.Delete(pathTo);
				}
				File.Move(path,pathTo);
				Debug.Log("LevelEditor.InsertLevelAfterActualLevel() "+path+">"+pathTo);

			}
		}
		AddToEditorHistory();
		AddEditorMessage("Inserted a level after "+actualEditorLevel);
	}

	// Mouse to WorldPosition and reverse
	//   UpdateGameElementToPosition
	void UpdateGameElementToPosition(GameElement elem, Vector3 vecPosition) {

		RaycastHit hit;
		Camera cam  = Camera.main;
		cam = GameObject.Find ("editorcamera").GetComponent<Camera>();
		Ray ray = cam.ScreenPointToRay(vecPosition);   
		if (Physics.Raycast(ray,out hit)) {

			Debug.DrawLine(ray.origin, hit.point);

			Vector3 pos=ray.GetPoint(0);
			elem.position.x=pos.x;
			elem.position.y=pos.y;
			elem.position.z=pos.z;

			// use editor cursor actual
			elem.position.x = editorCursorActualPoint.x;
			elem.position.y = editorCursorActualPoint.y;
			elem.position.z = editorCursorActualPoint.z;

			// Debug.Log ("UpdateGameElementToPosition() AT "+pos);
			UpdateElementVisual(elem);


		}

	}

	// camera
	public string cameraType="";
	public bool cameraInit=false;
	public float cameraOrthoSize=0.0f; // org
	//public GameObject camera;

	public void SetCameraZoom( float zoomlevel ) {

		Debug.Log ("SetCameraZoom( "+ zoomlevel +" )");
		/*
		Camera cam=camera.GetComponent<Camera>();

		if (!cameraInit) {
			cameraInit=true;

			cameraOrthoSize=cam.orthographicSize;
		}

		// zoom 
		cam.orthographicSize=cameraOrthoSize+zoomlevel;
		*/

	}

	Vector3 positionReset;
	Quaternion rotationReset;

	GameObject levelObject = null;

	// Use this for initialization
	void Start () {

		// start 
		ScalerStart();

		// Get Language
		ingameLanguage = GetInGameLanguage();


		// Get GameLogic
		GetGameLogic ();

		// Notification Center
		GetNotificationCenter();

		// levelObject
		levelObject = GameObject.Find("level");

		// ...
		string editorAutorX = PlayerPrefs.GetString("editorAutor");
		if(editorAutorX!=null) editorAutor = editorAutorX;

		string remoteAreaEditX = PlayerPrefs.GetString("remoteAreaEdit");
		string remoteAutorEditX = PlayerPrefs.GetString("remoteAutorEdit");
		if(remoteAreaEditX!=null) remoteAreaEdit = remoteAreaEditX;
		if(remoteAutorEditX!=null) remoteAutorEdit = remoteAutorEditX;

		string levelTypeX = PlayerPrefs.GetString("leveltype");
		if(levelTypeX!=null) leveltype = levelTypeX;

		// game logic
		// gameLogic.SetGameState ( GameLogic.GameLogicModal.Editor );

		// store container reset
		GameObject container=GameObject.Find ("editorCameraContainer");
		positionReset = new Vector3( container.transform.position.x, container.transform.position.y, container.transform.position.z );
		rotationReset = Quaternion.Euler( container.transform.localRotation.eulerAngles.x ,container.transform.localRotation.eulerAngles.y, container.transform.localRotation.eulerAngles.z );


		// LoadElements
		InitGameElements ();

		// editor presets
		editorArea = "playfield";
		editorSubArea = "stone";

		// needed for levels 
		// evaluation tools?
		evaluationPlayer.playerId = "1111";
		evaluationPlayer.name = "anonymous";

		if (flagEvaluationAvailable) {
			StartUpEvaluationTools ();
		}

		// start with level 1
		SetLevel (0);

		// prefs
		int lastEditedLevel = PlayerPrefs.GetInt("LastEditedLevel");
		if(lastEditedLevel>=0) actualEditorLevel = lastEditedLevel;


		// check for playerId
		// > generate playerId
		ingameLevel = 0;

	}

	int editorPrefX = 10;
	int editorPrefY = 10;
	int editorWidth=950;
	int editorHeight=140;

	bool CheckMouseInEditor() {
		float mouseX=Input.mousePosition.x;
		float mouseY=Screen.height-Input.mousePosition.y;

		/*
		if ((mouseX>editorPrefX)&&(mouseX<(editorPrefX+editorWidth))&&(mouseY>editorPrefY)&&(mouseY<(editorPrefY+editorHeight))) {
			// Debug.Log ("CheckMouseInEditor() > TRUE; ");
			return true;

		}
		*/

		if ((mouseX>(Screen.width-200*scaleX))&&(mouseX<(Screen.width))
			&&
			(mouseY>=0)&&(mouseY<(20*scaleX))) {
			return true;
		}


		if ((mouseX>cursorRect.x*scaleX)&&(mouseX<(cursorRect.x*scaleX+cursorRect.width*scaleX))
			&&
			(mouseY>cursorRect.y*scaleY)&&(mouseY<(cursorRect.y+cursorRect.height*scaleY))) {
			return true;
		}

		// inspectorRect
			if ((mouseX>inspectorRect.x*scaleX)&&(mouseX<(inspectorRect.x*scaleX+inspectorRect.width*scaleX))
			&&
				(mouseY>inspectorRect.y*scaleY)&&(mouseY<(inspectorRect.y*scaleY+inspectorRect.height*scaleY))) {
			return true;
		}

			if ((mouseX>filterTypeVisual.x*scaleX)&&(mouseX<(filterTypeVisual.x*scaleX+filterTypeVisual.width*scaleX))
			&&
				(mouseY>filterTypeVisual.y*scaleY)&&(mouseY<(filterTypeVisual.y*scaleY+filterTypeVisual.height*scaleY))) {
			return true;
		}
			if ((mouseX>filterTypeSubVisual.x*scaleX)&&(mouseX<(filterTypeSubVisual.x*scaleX+filterTypeSubVisual.width*scaleX))
			&&
				(mouseY>filterTypeSubVisual.y*scaleY)&&(mouseY<(filterTypeSubVisual.y*scaleY+filterTypeSubVisual.height*scaleY))) {
			return true;
		}

			if ((mouseX>selectionDialogeVisual.x*scaleX)&&(mouseX<(selectionDialogeVisual.x*scaleX+selectionDialogeVisual.width*scaleX))
			&&
				(mouseY>selectionDialogeVisual.y*scaleY)&&(mouseY<(selectionDialogeVisual.y*scaleY+selectionDialogeVisual.height*scaleY))) {
			return true;
		}

			if ((mouseX>undoVisual.x*scaleX)&&(mouseX<(undoVisual.x*scaleX+undoVisual.width*scaleX))
			&&
				(mouseY>undoVisual.y*scaleY)&&(mouseY<(undoVisual.y*scaleY+undoVisual.height*scaleY))) {
			return true;
		}

			if ((mouseX>notificationVisual.x*scaleX)&&(mouseX<(notificationVisual.x*scaleX+notificationVisual.width*scaleX))
			&&
				(mouseY>notificationVisual.y*scaleY)&&(mouseY<(notificationVisual.y*scaleY+notificationVisual.height*scaleY))) {
			return true;
		}

			if ((mouseX>nearbyRect.x*scaleX)&&(mouseX<(nearbyRect.x*scaleX+nearbyRect.width*scaleX))
			&&
				(mouseY>nearbyRect.y*scaleY)&&(mouseY<(nearbyRect.y*scaleY+nearbyRect.height*scaleY))) {
			return true;
		}

			if ((mouseX>toolsRect.x*scaleX)&&(mouseX<(toolsRect.x*scaleX+toolsRect.width*scaleX))
			&&
				(mouseY>toolsRect.y*scaleY)&&(mouseY<(toolsRect.y*scaleY+toolsRect.height*scaleY))) {
			return true;
		}

			if ((mouseX>typeselectionRect.x*scaleX)&&(mouseX<(typeselectionRect.x*scaleX+typeselectionRect.width*scaleX))
			&&
				(mouseY>typeselectionRect.y*scaleY)&&(mouseY<(typeselectionRect.y*scaleY+typeselectionRect.height*scaleY))) {
			return true;
		}

		// Debug.Log ("CheckMouseInEditor() > FALSE; ");
		return false;
	}

	/*
	bool GameElementInEditor( float x, float y ) {
		float mouseX=x;
		float mouseY=Sceen.height-y;
		if ((mouseX>editorPrefX)&&(mouseX<(editorPrefX+editorWidth))&&(mouseY>editorPrefY)&&(mouseY<(editorPrefY+editorHeight))) {
			return true;
		}
		return false;
	}
	*/

	void HandleMouseDownToCreate(){
		HandleMouseDownToCreate( "mousepointer" );
	}

	void HandleMouseDownToCreate( string createAtType ){

		if (!editorTool.Equals ("CREATE"))return;

		if (createAtType.Equals("mousepointer")) if (CheckMouseInEditor()) return;

		//	if (true) {

		// Debug.Log("LeveleEditor.Update() // "+editorTool+" // INEDITOR // CREATE ");


		// Debug.Log("LevelEditor.Update() // "+editorTool);

		// Debug.Log ("CREATE NOW");

		// sorry nothing found
		if (editorPrefab==null) {

			//Debug.Log ("Sorry no correct prefab!");
			editorLogText = "Nothing yet selected to create!";
			AddEditorMessage(editorLogText);
		}

		if (editorPrefab!=null) {

			// check here ...
			// if (editorPrefab.prefabGameObject!=null) {

			GameElement prefabTemplate = GetElementType (editorArea,editorSubArea);

			GameElement editorPrefabX = GetElementType (editorArea,editorSubArea);

			if (editorPrefabX==null) {
				Debug.Log("LevelEditor().Update() // Create object. Could not find GameObjectType: "+editorArea+"/"+editorSubArea);
				AddEditorMessage("Could not find GameObjectType: "+editorArea+"/"+editorSubArea);
				return;
			}

			// (paint-)tools
			bool flagTool = false;
			if (editorSubArea.IndexOf("+")!=-1) { 
				if (editorPrefabX.guiDescription.Equals("")) return;
				string[] words = 	editorPrefabX.guiDescription.Split(',');
				int max = words.Length-1;
				if (max<0) max=0;
				int index = UnityEngine.Random.Range(0,max);
				// Debug.Log("LevelEditor.Update() //  ["+index+"]<"+words.Length+" ");
				string subtypeConf = words[index];
				GameElement editorPrefabXX = GetElementType (editorArea,subtypeConf);
				if (editorPrefabXX!=null) { 
					editorPrefabX = editorPrefabXX ;
					flagTool = true;
				} else {
					// Debug.LogWarning("LevelEditor.Update() //  Searching for Painting Tool. Config Array: Unity3dLevelEditor: "+editorArea+"/"+subtypeConf);
					AddEditorMessage("[Tool "+editorArea+"/"+subtypeConf+"]: Could not find part: "+editorArea+"/"+subtypeConf);
					return; 
				}
			}

			// arg
			GameElement arg = editorPrefabX.Copy();
			AddElement(arg);
			arg.creator = editorAutor;
			UpdateGameElementToPosition(arg, Input.mousePosition);

			// copy to the actualGameElement
			editorLastTouchedGameElement = arg;

			// Debug.Log ("CREATE NOW");

			float offsetX=0.25f;
			float offsetZ=0.25f;
			float raster=GetRaster ();
			if (raster!=0.0f) {
				if (arg!=null) {
					arg.position.x=(Mathf.Floor((arg.position.x+offsetX)/raster))*raster;
					arg.position.z=(Mathf.Floor((arg.position.z+offsetZ)/raster))*raster;
					UpdateElementVisual(arg);
				}
			}

			// rotation
			arg.rotation = editorDegree;

			if (createAtType.Equals("null")) {
				arg.position.x=0.0f;
				arg.position.y=0.0f;
				arg.position.z=0.0f;
				UpdateElementVisual(arg);
			}
			if (createAtType.Equals("camera")) {
				GameObject container=GameObject.Find ("editorCameraContainer");
				GameObject editorcamera=GameObject.Find ("editorcamera");

				arg.position.x=container.transform.position.x;
				arg.position.y=container.transform.position.y;
				arg.position.z=container.transform.position.z;
				// and transform
				arg.rotation = 0 + container.transform.eulerAngles.y; //  - 180; 
				arg.rotationForward = 0 + editorcamera.transform.eulerAngles.x;

				// Debug.Log("HandleMouseDownToCreate('camera) // "+arg.rotation; // +" "+arg.rotationForward);

				UpdateElementVisual(arg); 
			}


			arg.name = createName; // or in case 



			// clear ... add ...
			if (arg.name.Equals("")) {
				// add default !!!

			}

			UpdateElementVisual(arg);

			// tool: add randomness
			// Debug.Log("LevelEditor.HandleMouseDown() // "+prefabTemplate.editorRandom);
			if (prefabTemplate.editorRandom) {
				float factorSize = UnityEngine.Random.Range(5,15)/10.0f;
				float factorRotation = UnityEngine.Random.Range(0,360);
				arg.size = arg.size * factorSize;		
				arg.rotation = arg.rotation + factorRotation;
				UpdateElementVisual(arg);
			}

			// add to editor history
			AddToEditorHistory("[INGAME][CREATE]");


			// }
		}
		// 
	}

	int directMouseInputX = 0;
	int directMouseInputY = 0;

	void ActivateCursorPreview(bool inBool){
		foreach(Transform childT in GameObject.Find("editorcursorpreview").transform){
			childT.gameObject.SetActive(inBool);
		}
	}

	// buttons
	public Texture2D cursorIconCam;
	public Texture2D cursorIconObject;
	public Texture2D cursorIconUp;
	public Texture2D cursorIconDown;
	public Texture2D cursorIconLeft;
	public Texture2D cursorIconForward;
	public Texture2D cursorIconBackward;
	public Texture2D cursorIconRight;
	public Texture2D cursorIconRotateLeft;
	public Texture2D cursorIconRotateRight;
	public Texture2D cursorIconRotateForward;
	public Texture2D cursorIconRotateBackward;
	// toggle
	bool expandedLevelEdit = true;
	public Texture2D cursorIconExpand;
	public Texture2D cursorIconShrink;

	void OnGUI() {

		bool debugThis = false;

		if (Event.current.type == EventType.MouseUp && Event.current.button == 0 && GUIUtility.hotControl == 0 && // This code will be ignored if the user had mouse-downed on a GUI element.
			gameLogic !=null &&  gameLogic.modal==GameLogic.GameLogicModal.Editor) { // check if in editormode, not in playmode

			HandleMouseDownToCreate();
		}


		// SWITCH BETWEEN EDITOR/GAME
		GUIStyle guixt = editorSwitchButtonStyle;

		// debug
		if (debugThis) {
			GUIScalerLabel (new Rect (GUIScalerScreenWidth() - 200, 0, 200, 80), "LEVELEDITOR", guixt);
		}

		// GAME
		if (gameLogic!=null) {
			if (gameLogic.modal == GameLogic.GameLogicModal.Running) {
				guixt = editorSwitchButtonStyleActive ;
			}
			if (GUIScalerButton (new Rect (GUIScalerScreenWidth() - 160, 0, 80, 20), "GAME", guixt)) {
				ActivateCursorPreview(false);
				// deactivate all items of the editor 
				// for example: previews!
				SetSelectedElement( null );
				gameLogic.SetGameState( GameLogic.GameLogicModal.Running );

			}

			// ingame 
			if (debugIngame) {
				float igt = GetIngameTime();
				igt = (int)(igt*10.0f)/10.0f;
				if (GUIScalerButton (new Rect (GUIScalerScreenWidth() - 160, 24, 100, 20), "("+ingameLanguage+")"+ingameLevel+"-"+igt+"", guixt)) {
					actualEditorLevel = ingameLevel;
					gameLogic.SetGameState( GameLogic.GameLogicModal.Editor  );
				}
				// ingameState
				if (GUIScalerButton (new Rect (GUIScalerScreenWidth() - 160, 44, 100, 20), " "+ingameState, guixt)) {
					actualEditorLevel = ingameLevel;
					gameLogic.SetGameState( GameLogic.GameLogicModal.Editor  );
				}
			}

			// EDITOR
			guixt = editorSwitchButtonStyle;
			if (gameLogic.modal == GameLogic.GameLogicModal.Editor) {
				guixt = editorSwitchButtonStyleActive ;

				// default?
				if (!CountElementsTypeIndexOf("directlight")) { 
					GUIScalerLabel (new Rect (500, GUIScalerScreenHeight()*0.3f-24, 500, 20), "1. NO DIRECT LIGHT IN SCENE! USE DEFAULT >> ADD ONE UNDER /ENV/ ", guixt);
				}

				// default?
				if (!CountElementsTypeIndexOf("player")) { 
					GUIScalerLabel (new Rect (500, GUIScalerScreenHeight()*0.3f, 500, 20), "2. NO PLAYER > ADD ONE > /PLAYER/. ", guixt);
				}

				// default?
				if (!CountElementsTypeIndexOf("ingamecontroller")) { 
					GUIScalerLabel (new Rect (500, GUIScalerScreenHeight()*0.3f+24, 500, 20), "3. NO GAMELOGIC CONTROLLER > ADD ONE > /BASE/GAME... ", guixt);
				}

				// messages
				int cou = 0;
				if (arrEditorMessages.Count>0)
					for (int a=(arrEditorMessages.Count-1); a>=0; a--) {
						LevelEditorMessage msgObj = (LevelEditorMessage)arrEditorMessages [a];
						GUIScalerLabel (new Rect (600, GUIScalerScreenHeight()*0.6f+60+cou*22, 500, 20), ""+msgObj.message, guixt);
						cou++;
						if (cou>3) break;
					}

				// check for ..
				// msgx.timeToStayTill 
				if (arrEditorMessages.Count>0)
					for (int a=arrEditorMessages.Count-1;a>=0; a--) {
						LevelEditorMessage msgObj = (LevelEditorMessage)arrEditorMessages [a];
						if (msgObj.timeToStayTill<Time.time) {
							arrEditorMessages.Remove(msgObj);
						}
					}

			}
			if (GUIScalerButton (new Rect (GUIScalerScreenWidth() -160 + 80, 0, 80, 20), "EDITOR", guixt)) {
				gameLogic.SetGameState( GameLogic.GameLogicModal.Editor );
				ActivateCursorPreview(true);
			}


			// get latest
			// version
			float ver = gameLogic.GetVersionGame();
			if (gameLogic.modal == GameLogic.GameLogicModal.Editor) ver = gameLogic.GetVersionEditor();
			if (GUIScalerButton (new Rect (GUIScalerScreenWidth() - 260, 0, 80, 20), " v."+ver, editorSwitchButtonStyle)) {

			}

			/*
		 *  NOTIFICATION CENTER
		 * 
		 * */
			// NOTIFICATIONS
			if (notificationDialog) {
				notificationDialogX = (int) (GUIScalerScreenWidth() * 0.6f);
				notificationDialogY = (int) (GUIScalerScreenHeight() * 0.3f);
				notificationVisual.x = notificationDialogX;
				notificationVisual.y = notificationDialogY;
				notificationVisual.width = 150;
				// notificationVisual.height = 50;
				GUIScalerLabel ( new Rect(notificationVisual.x-5,notificationVisual.y-5,notificationVisual.width+10,notificationVisual.height+10), "", editorBackground);

				int notificationDialogXTmp = notificationDialogX;
				int notificationDialogYTmp = notificationDialogY;

				ArrayList arr = notificationCenter.GetNotificationTypesUnique();

				if (GUIScalerButton (new Rect ( notificationDialogXTmp, notificationDialogYTmp, notificationVisual.width, 20), "ALL NOTIFICATIONS *.* ", editorButtonActiveStyle)) {
					notificationArea = "";
				}
				notificationDialogYTmp = notificationDialogYTmp + 22;

				/*
				strNotification = GUI.TextField (new Rect ( notificationDialogXTmp, notificationDialogYTmp, notificationVisual.width, 20), ""+strNotification, editorButtonActiveStyle) ;
				notificationDialogYTmp = notificationDialogYTmp + 22;
				*/

				notificationDialogYTmp = notificationDialogYTmp + 3;

				if (arr.Count>0)
					for (int i=0;i<arr.Count; i++) {
						Notification nt = (Notification)arr [i];
						string text = "" + nt.type+".*";
						GUIStyle guix = editorButtonStyleNotActive;
						// if (editorSelected==gae) guix = editorButtonActiveStyle;
						bool flagShow = true;
						if (!notificationArea.Equals("")) {
							if (!notificationArea.Equals(nt.type)) {
								flagShow = false;
							} else {
								guix = editorButtonActiveStyle;			
							}
						}
						if (flagShow) {
							bool buttonClicked = GUIScalerButton (new Rect ( notificationDialogXTmp, notificationDialogYTmp, notificationVisual.width, 20), ""+text+"", guix);
							if (buttonClicked) {
								notificationArea = nt.type;
								notificationAreaSub = "";
								strNotification = notificationArea+"."+notificationAreaSub;
							}
							notificationDialogYTmp = notificationDialogYTmp + 22;
						}

						// counter++;
						// if (counter>5) break;
					}

				notificationDialogYTmp = notificationDialogYTmp + 5;

				if (!notificationArea.Equals("")) {
					ArrayList arrxy = notificationCenter.GetNotificationTypes(notificationArea);
					if (arrxy.Count>0)
						for (int i=0;i<arrxy.Count; i++) {
							Notification nt = (Notification)arrxy [i];
							string text = "" + nt.type+"."+nt.subtype;
							GUIStyle guix = editorButtonStyleNotActive;
							bool flagShow = true;
							if (nt.subtype.Equals(notificationAreaSub)) guix = editorButtonActiveStyle;			
							if (flagShow) {
								bool buttonClicked = GUIScalerButton (new Rect ( notificationDialogXTmp, notificationDialogYTmp, notificationVisual.width, 20), " "+text+"", guix);
								if (buttonClicked) {
									notificationAreaSub = nt.subtype;
									strNotification = notificationArea+"."+notificationAreaSub;
								}
								notificationDialogYTmp = notificationDialogYTmp + 22;
							}

							// counter++;
							// if (counter>5) break;
						}
				}

				notificationVisual.height = notificationDialogYTmp - notificationDialogY;

				// NOTIFICATION TESTER

				notificationDialogX = (int) (GUIScalerScreenWidth() * 0.2f) + 220;
				notificationDialogY = (int) (GUIScalerScreenHeight() * 0.2f);
				// GUIScalerLabel ( new Rect(notificationVisual.x-5,notificationVisual.y-5,notificationVisual.width+10,notificationVisual.height+10), "", editorBackground);
				notificationDialogXTmp = notificationDialogX;
				notificationDialogYTmp = notificationDialogY;

				if (notificationTesterAndHistory) {
					bool buttonClickedX = GUIScalerButton (new Rect ( notificationDialogXTmp, notificationDialogYTmp, notificationVisual.width, 20), "ADD NOTIFICATION", editorButtonActiveStyle);
					if (buttonClickedX) {
						float timed = float.Parse( fieldTimed );
						notificationCenter.AddNotification(fieldType,fieldTypeSub,fieldTarget,timed,fieldArgument, new Vector3());
					}
					notificationDialogYTmp = notificationDialogYTmp + 22;
					fieldType = GUIScalerTextField (new Rect ( notificationDialogXTmp, notificationDialogYTmp, notificationVisual.width, 20), ""+fieldType, editorButtonStyle) ;
					notificationDialogYTmp = notificationDialogYTmp + 22;
					fieldTypeSub = GUIScalerTextField (new Rect ( notificationDialogXTmp, notificationDialogYTmp, notificationVisual.width, 20), ""+fieldTypeSub, editorButtonStyle) ;
					notificationDialogYTmp = notificationDialogYTmp + 22;
					fieldTarget = GUIScalerTextField (new Rect ( notificationDialogXTmp, notificationDialogYTmp, notificationVisual.width, 20), ""+fieldTarget, editorButtonStyle) ;
					notificationDialogYTmp = notificationDialogYTmp + 22;
					fieldTimed = GUIScalerTextField (new Rect ( notificationDialogXTmp, notificationDialogYTmp, notificationVisual.width, 20), ""+fieldTimed, editorButtonStyle) ;
					notificationDialogYTmp = notificationDialogYTmp + 22;
					fieldArgument = GUIScalerTextField (new Rect ( notificationDialogXTmp, notificationDialogYTmp, notificationVisual.width, 20), ""+fieldArgument, editorButtonStyle) ;
					notificationDialogYTmp = notificationDialogYTmp + 22;

					// add history ...
					// Debug.Log("LevelEditor.OnGUI(); // "+notificationCenter.arrNotificationPipline.Count);
					notificationDialogYTmp = notificationDialogYTmp + 22;

					if (notificationCenter.arrNotificationPipline.Count>0) {
						for (int i=(notificationCenter.arrNotificationPipline.Count-1);i>=0;i--) {
							Notification nt = (Notification)notificationCenter.arrNotificationPipline [i];
							string text = "[" + nt.state+"] "+nt.type+"/"+nt.subtype+" {>"+nt.targetName+"} "+nt.argument+" ("+nt.timed+")";
							GUIStyle guix = editorButtonStyleNotActive;
							GUIScalerLabel (new Rect ( notificationDialogXTmp, notificationDialogYTmp, notificationVisual.width*2, 20), " "+text+"", guix);
							notificationDialogYTmp = notificationDialogYTmp + 22;
						}
					}
				}

			}

			/*
			 *  SCENE RENDERING (ICONS ETC.)
			 * 
			 * */

			if (gameLogic.modal == GameLogic.GameLogicModal.Editor) {

				// visualize the objects with no gameobject
				float mouseX=Input.mousePosition.x;
				float mouseY=Screen.height-Input.mousePosition.y;

				if (arrLevel.Count>0)
					for (int i=0; i<arrLevel.Count; i++) {
						GameElement gaelement = (GameElement)arrLevel [i];

						if (Camera.main==null) {
							Debug.Log("No Main Camera!");
							break;
						}

						// display or not? filters
						if (!filterType.Equals("*")) {
							if (!gaelement.type.Equals(filterType)) {
								continue;
							}	
							if (!filterTypeSub.Equals("*")) {
								if (!gaelement.subtype.Equals(filterTypeSub)) {
									continue;
								}	
							}
						}

						// screen pos
						Camera cam  = Camera.main;
						GameObject camx = GameObject.Find ("editorcamera");
						if ( camx!=null ) {
							cam = camx.GetComponent<Camera>();
						}
						Vector3 screenPos = cam.WorldToScreenPoint (gaelement.position);

						// visible?
						// if (!GameElementInEditor(screenPos.x,screenPos.y))
						if ((screenPos.x > 0) && (screenPos.x < Screen.width)) 
						if ((screenPos.y > 0) && (screenPos.y < Screen.height)) 
						{


							// default infos 
							// like name & argument
							bool showInfo=false;
							string textInfo="";
							if (!gaelement.name.Equals ("")) { showInfo=true; textInfo=textInfo+"#"+gaelement.name+""; } 
							if (!gaelement.argument.Equals ("")) { showInfo=true; if (!textInfo.Equals ("")) textInfo=textInfo+" ";/* textInfo=textInfo+"{"+gaelement.argument+"}"; */ } 

							if (gaelement.type.Equals("remark")) {
								textInfo=textInfo+" " +"{"+gaelement.argument+"}";  
							}

							// info here 
							string waiting ="";
							if (gaelement.release.Equals ("wait")) {
								waiting = waiting+"[-]";
							}
							string strType="";
							if (cameraOverlayTypes) {
								// strType = ""+gaelement.type+"\n -"+gaelement.subtype;
								strType = ""+gaelement.subtype;
							}
							// edit showinfo
							if (editorTool.Equals ("EDIT")) {
								if (editorSelected!=null)
								if (gaelement!=null)
								if (editorSelected==gaelement) {
									showInfo = true;
								}
							}

							if (!showInfo) {
								GUI.Label (new Rect (screenPos.x+20, Screen.height - screenPos.y , 200*scaleX, 80*scaleY),""+waiting+" "+strType,editorElementType );
							}
							if (showInfo) {
								// GUIScalerLabel (new Rect (screenPos.x+20, GUIScalerScreenHeight() - screenPos.y, 200, 80),+"        "+strType,editorElementType );
								string str = textInfo;
								if (str.Equals("")) {
									str = strType;
								}
								GUI.Label (new Rect (screenPos.x+20, Screen.height - screenPos.y, 200*scaleX, 80*scaleY),waiting+""+str,editorLabel);
							}

							// edit ?
							if (editorTool.Equals ("EDIT")) {
								if (GUI.Button (new Rect (screenPos.x, Screen.height - screenPos.y, 20*scaleX, 20*scaleY), editorEditImage, editorIconGUI)) {
									if (!CheckMouseInEditor()) {
										SetSelectedElement(gaelement);
									}
								}
								if (editorSelected==gaelement) {
									GUI.Label (new Rect (screenPos.x-10, Screen.height - screenPos.y-10, 40*scaleX, 40*scaleY), editorSelectedImage, editorIconGUI);
									nearbyX = (int ) (screenPos.x - 125);
									nearbyY = (int) (Screen.height - screenPos.y);
								} 
							}

							// move ?
							// version 1.0

							if (editorTool.Equals ("MOVE")) {



								if (!gaelement.type.Equals("base")) {
									if (GUI.Button (new Rect (screenPos.x, Screen.height - screenPos.y, 20*scaleX, 20*scaleY), editorMoveImage, editorIconGUI)) {
										// SetSelectedElement(gaelement);
										Debug.Log("LevelEditor.OnGUI() // Pressed Button!");

									}
								} else {
									if (GUI.Button (new Rect (screenPos.x, Screen.height - screenPos.y, 20*scaleX, 20*scaleY), editorEditImage, editorIconGUI)) {
										SetSelectedElement(gaelement);
										SetTool("EDIT");
									}
								}
								if (editorSelected==gaelement) {
									GUI.Label (new Rect (screenPos.x-10, Screen.height - screenPos.y-10, 40*scaleX, 40*scaleY), editorSelectedImage, editorIconGUI);
								} 
							}

							// version 2.0
							if (editorTool.Equals ("MOVE")) {

								float buttonX=screenPos.x;
								float buttonY=Screen.height-screenPos.y;
								float buttonWidth=20.0f;

								// Debug.Log("LevelEditor // EDITORTOOL:"+editorTool);

								// if (!gaelement.type.Equals("base"))
								if (
									(mouseX>buttonX)&&(mouseX<(buttonX+buttonWidth*scaleX)) 
									&&
									(mouseY>buttonY)&&(mouseY<(buttonY+buttonWidth*scaleX)) 
								)
								{
									// Debug.Log("LevelEditor.OnGUI() // MOVE: Mouse over");
									if (Input.GetMouseButtonDown(0)) {
										// Debug.Log("LevelEditor.OnGUI() // MOVE 1 : MouseDown");
										if (editorToolMove.Equals ("")) {
											// Debug.Log("LevelEditor.OnGUI() // MOVE 1: MouseDown");
											// move
											SetSelectedElement(gaelement);
											editorToolMove="drag";
										}
									}


								}
								// dragging
								if (Input.GetMouseButton(0)) {
									if (editorSelected!=null)
									if (gaelement!=null)
									if (gaelement==editorSelected) {
										// move
										if (editorToolMove.Equals ("drag")) {
											// Debug.Log("LevelEditor.OnGUI() // MOVE: Dragging");

											// Debug.Log("Moving "+mouseX);
											UpdateGameElementToPosition(gaelement,Input.mousePosition);

											editorLastTouchedGameElement = editorSelected;
										}
									}
								}

								// mouse up
								if (Input.GetMouseButtonUp(0)) {
									if (gaelement!=null)
									if (editorSelected!=null) {

										// Debug.Log("LevelEditor.OnGUI() // MouseButtonUp(0)");
										// Debug.Log("LevelEditor.OnGUI() // MOVE: Mouse up (Drop)");


										float raster=GetRaster();
										// Debug.Log ("raster: "+raster);
										if (raster!=0.0f) {
											if (editorSelected!=null) {
												// Debug.Log ("Selected: "+editorSelected.position.x+"/"+editorSelected.position.y);



												float offsetX=0.0f;
												float offsetY=0.0f;

												// editorSelected.position.x=((int)((editorSelected.position.x+offsetX)/raster))*raster;
												// editorSelected.position.y=((int)((editorSelected.position.y+offsetY)/raster))*raster;

												editorSelected.position.x=(Mathf.Floor((editorSelected.position.x+offsetX)/raster))*raster;
												editorSelected.position.z=(Mathf.Floor((editorSelected.position.z+offsetY)/raster))*raster;

												UpdateElementVisual(editorSelected);

												AddToEditorHistory("[GUI][OBJECT][MOVE]");
											}
										}



										// move
										SetSelectedElement(null);
										editorToolMove="";

									}
								}

							}

							// delete ?
							if (editorTool.Equals ("DELETE")) {
								if (GUI.Button (new Rect (screenPos.x, Screen.height - screenPos.y, 20*scaleX*0.9f, 20*scaleY*0.90f), editorDeleteImage, editorIconGUI)) {
									// delete it now ..
									RemoveElement (gaelement);

									// add to editor history
									AddToEditorHistory("[GUI][OBJECT][DELETE]");



								}
							}

							// check if possible!
							GameElement gelem=GetElementType(gaelement.type, gaelement.subtype);
							if (gelem==null) {
								GUI.Label (new Rect (screenPos.x, Screen.height - screenPos.y+20, 300, 20), "[NOTFOUND:" + gaelement.type + "/" + gaelement.subtype+"]");
							}
							else {					

								GameObject rep = gaelement.gameObject;
								if (rep == null) {
									GUI.Label (new Rect (screenPos.x, Screen.height - screenPos.y+20, 100, 20), "(" /* + gaelement.type + "/" */ + gaelement.subtype+")");
								}

							}



						}


					} // element	




			}
		}

		// EVALUATION SYSTEM
		if (flagEvaluationAvailable) {

			if (gameLogic!=null) {

				if (gameLogic.modal == GameLogic.GameLogicModal.Running ) {
					if (GUIScalerButton (new Rect (GUIScalerScreenWidth() -160 , 24, 160, 20), "EVALUATION", editorSwitchButtonStyleActive)) {
						// gameLogic.SetGameState( GameLogic.GameLogicModal.Editor );
						showEvaluationDialog=!showEvaluationDialog;
						UpdateRelationVisualisationAndCheckError();
					}
				}

				// show user evaluation 
				if (showEvaluationDialog) {
					if (flagEvaluation)
					if (gameLogic.modal == GameLogic.GameLogicModal.Running ) {

						int edX = GUIScalerScreenWidth() - 760;

						if (GUIScalerButton (new Rect (edX , 0, 78, 20), "EVALUATE! ", guiEvaluation)) {
						}
						edX = edX + 80;

						string[] names = { "++","+"," ","-","--" };
						string[] values = { "2","1","0","-1","-2" };

						// evaluationUserAllOver = "0";

						for (int i=0;i<names.Length;i++) {
							string allover = "";
							if (evaluationUserAllOver.Equals(""+values[i])) {
								// allover = ">";
							}
							if (GUIScalerButton (new Rect (edX , 0, 38, 20), allover+""+names[i], guiEvaluation)) {
								evaluationUserAllOver = ""+values[i]; 
								GameObject firstPlayer = GetFirstPlayerObject();
								Vector3 pos = firstPlayer.transform.position;
								AddEvaluationElement( "allover", ""+values[i],  pos );
							}
							edX = edX + 40;
						}

						// comments
						evaluationUserComment=GUIScalerTextField (new Rect(edX,0,100,20),evaluationUserComment);
						edX = edX + 100;
						if (GUIScalerButton (new Rect (edX , 0, 78, 20), "COMMENT", guiEvaluation)) {
							GameObject firstPlayer = GetFirstPlayerObject();
							Vector3 pos = firstPlayer.transform.position;
							AddEvaluationElement( "comment", ""+evaluationUserComment,  pos );
							evaluationUserComment = "";
						}
						edX = edX + 80;
					}
				}

				// show evaluation dialog 
				if (showEvaluationDialog) {

					int evaluationY=46;

					// INGAME MODE!
					if (gameLogic.modal == GameLogic.GameLogicModal.Running ) {
						string flagEval = "ON";
						if (!flagEvaluation) flagEval = "OFF";
						if (GUIScalerButton (new Rect (GUIScalerScreenWidth() -160 , evaluationY, 160, 20), ""+flagEval, guiEvaluation)) {
							flagEvaluation=!flagEvaluation;
						}
						evaluationY = evaluationY+ 22;




						// only config
						if (flagEvaluation) {


							string str = "NOT DEFINED";  // evaluationPlayer.playerId
							if (evaluationPlayer!=null) { 
								str = ""+evaluationPlayer.name+"" + "("+ evaluationPlayer.playerId +")";
							}
							if (GUIScalerButton (new Rect (GUIScalerScreenWidth() -160 , evaluationY, 160, 20), "[EDIT: "+str +"]", guiEvaluation)) {
								// create new user
								// show dialog ...
								// Debug.Log ("EDIT");
								if (showEvaluationDialogEdit) {
									showEvaluationDialogEdit = false; 
								} else {
									EditEvaluationPlayerStart( "player" );
								}

							}
							evaluationY = evaluationY+ 22;

							// flagSaveToWeb
							string saveTo = "SAVING LOCAL";
							if (flagSaveToWeb) saveTo = "SAVING LOCAL&WEB";
							if (GUIScalerButton (new Rect (GUIScalerScreenWidth() -160 , evaluationY, 160, 20), ""+saveTo, guiEvaluation)) {
								flagSaveToWeb=!flagSaveToWeb;
							}
							evaluationY = evaluationY+ 22;


							// NEW PLAYER
							evaluationY = evaluationY+ 22;
							if (GUIScalerButton (new Rect (GUIScalerScreenWidth() -160 , evaluationY, 160, 20), " + NEW PLAYER", guiEvaluation)) {
								CreateNewEvaluationPlayer();
							}
							evaluationY = evaluationY+ 22;
							evaluationY = evaluationY+ 22;

							evaluationY = evaluationY+ 10;

							// POSSIBLE ADD ONS HERE
							/*
						if (GUIScalerButton (new Rect (GUIScalerScreenWidth() -160 , evaluationY, 160, 20), "POSITIVE - AREA", guiEvaluation)) {
							GameObject firstPlayer = GetFirstPlayerObject();
							Vector3 pos = firstPlayer.transform.position;
							AddEvaluationElement( "allover", "1",  pos );
						}
						evaluationY = evaluationY+ 22;
						if (GUIScalerButton (new Rect (GUIScalerScreenWidth() -160 , evaluationY, 160, 20), "HATE AREA", guiEvaluation)) {
							GameObject firstPlayer = GetFirstPlayerObject();
							Vector3 pos = firstPlayer.transform.position;
							AddEvaluationElement( "allover", "-1", pos );
						}
						evaluationY = evaluationY+ 22;
						*/


							// LOAD LEVEL
							/*
						evaluationY = evaluationY+ 22;
						if (GUIScalerButton (new Rect (GUIScalerScreenWidth() -160 , evaluationY, 160, 20), "LOAD ALL EVALUATIONS", guiEvaluation)) {
							RemoveAllEvaluationGameElements();
							LoadEvaluationLevels( );
						}
						evaluationY = evaluationY+ 22;
						*/




						}

					}

					// show edit ?
					if (showEvaluationDialogEdit) {

						if (flagEvaluation) {

							string title = "";
							//string desc = "";

							int dialogEvaluationStartX = GUIScalerScreenWidth() - 700;
							int dialogEvaluationStartY = 20;
							int dialogEvaluationWidth = 400;
							int dialogEvaluationHeight = 400;
							if (showEvaluationDialogEditType.Equals ("player")) {
								title = "EDIT EVALUATION PLAYER";
							}

							int dialogEvaluationX = dialogEvaluationStartX + 10;
							int dialogEvaluationY = dialogEvaluationStartY + 10;

							GUIScalerLabel (new Rect(dialogEvaluationX,dialogEvaluationY,dialogEvaluationWidth,dialogEvaluationHeight),"",guiEvaluation);
							GUIScalerLabel (new Rect(dialogEvaluationX,dialogEvaluationY,dialogEvaluationWidth,28),""+title,guiEvaluation);
							if (GUIScalerButton (new Rect(dialogEvaluationX+dialogEvaluationWidth-100,dialogEvaluationY,100,28),"[CLOSE]",guiEvaluation)) {
								EditEvaluationPlayerStop();
							}
							dialogEvaluationY = dialogEvaluationY + 22;
							dialogEvaluationY = dialogEvaluationY + 22;

							string arg = "";

							// name
							GUIScalerLabel (new Rect(dialogEvaluationX,dialogEvaluationY,120,20),"NAME: ",guiEvaluation);
							arg=GUIScalerTextField (new Rect(dialogEvaluationX+120,dialogEvaluationY,200,20),evaluationPlayer.name);
							if (!arg.Equals (evaluationPlayer.name)) {
								evaluationPlayer.name = arg;
								SaveEvaluationPlayer( evaluationPlayer );
							}
							dialogEvaluationY = dialogEvaluationY + 22;

							// prename
							GUIScalerLabel (new Rect(dialogEvaluationX,dialogEvaluationY,160,20),"PRENAME: ",guiEvaluation);
							arg=GUIScalerTextField (new Rect(dialogEvaluationX+120,dialogEvaluationY,200,20),evaluationPlayer.prename);
							if (!arg.Equals (evaluationPlayer.prename)) {
								evaluationPlayer.prename = arg;
								SaveEvaluationPlayer( evaluationPlayer );
							}
							dialogEvaluationY = dialogEvaluationY + 22;

							dialogEvaluationY = dialogEvaluationY + 22;

							// age
							GUIScalerLabel (new Rect(dialogEvaluationX,dialogEvaluationY,160,20),"AGE: ",guiEvaluation);
							arg=GUIScalerTextField (new Rect(dialogEvaluationX+120,dialogEvaluationY,200,20),""+evaluationPlayer.age);
							if (!arg.Equals (""+evaluationPlayer.age)) {
								evaluationPlayer.age = Int32.Parse(arg);
								SaveEvaluationPlayer( evaluationPlayer );
							}
							dialogEvaluationY = dialogEvaluationY + 22;

							// game play 
							dialogEvaluationY = dialogEvaluationY + 22;

							// casual
							GUIScalerLabel (new Rect(dialogEvaluationX,dialogEvaluationY,160,20),"CASUAL(x%): ",guiEvaluation);
							arg=GUIScalerTextField (new Rect(dialogEvaluationX+120,dialogEvaluationY,200,20),""+evaluationPlayer.lovedCasual);
							if (!arg.Equals (""+evaluationPlayer.lovedCasual)) {
								evaluationPlayer.lovedCasual = Int32.Parse(arg);
								SaveEvaluationPlayer( evaluationPlayer );
							}
							dialogEvaluationY = dialogEvaluationY + 22;
							// core
							GUIScalerLabel (new Rect(dialogEvaluationX,dialogEvaluationY,160,20),"CORE(x%): ",guiEvaluation);
							arg=GUIScalerTextField (new Rect(dialogEvaluationX+120,dialogEvaluationY,200,20),""+evaluationPlayer.lovedCore);
							if (!arg.Equals (""+evaluationPlayer.lovedCore)) {
								evaluationPlayer.lovedCore = Int32.Parse(arg);
								SaveEvaluationPlayer( evaluationPlayer );
							}
							dialogEvaluationY = dialogEvaluationY + 22;
							// genre
							GUIScalerLabel (new Rect(dialogEvaluationX,dialogEvaluationY,160,20),"GENREPLAYER: ",guiEvaluation);
							arg=GUIScalerTextField (new Rect(dialogEvaluationX+120,dialogEvaluationY,200,20),""+evaluationPlayer.lovedGenre);
							if (!arg.Equals (evaluationPlayer.lovedGenre)) {
								evaluationPlayer.lovedGenre = arg;
								SaveEvaluationPlayer( evaluationPlayer );
							}
							dialogEvaluationY = dialogEvaluationY + 22;


							// comment
							dialogEvaluationY = dialogEvaluationY + 22;
							GUIScalerLabel (new Rect(dialogEvaluationX,dialogEvaluationY,160,20),"COMMENT: ",guiEvaluation);
							arg=GUIScalerTextField (new Rect(dialogEvaluationX+120,dialogEvaluationY,200,20),""+evaluationPlayer.comment);
							if (!arg.Equals (evaluationPlayer.comment)) {
								evaluationPlayer.comment = arg;
								SaveEvaluationPlayer( evaluationPlayer );
							}
							dialogEvaluationY = dialogEvaluationY + 22;

							dialogEvaluationY = dialogEvaluationY + 22;

							// sessions
							GUIScalerLabel (new Rect(dialogEvaluationX,dialogEvaluationY,160,20),"SESSION: "+evaluationPlayer.sessionId,guiEvaluation);
							dialogEvaluationY = dialogEvaluationY + 22;

						}

					}



				}

			}
		}

		// if (!flagEvaluation)

		// editor
		if (gameLogic !=null &&  gameLogic.modal==GameLogic.GameLogicModal.Editor) {

			// textfield > return
			bool enter = false;
			// Input.eatKeyPressOnTextFieldFocus = false;
			if (Event.current.Equals (Event.KeyboardEvent ("return"))) {
				enter = true;
			}
			if (enter) {  
				// Debug.Log("RETURN!!!");
				GUI.FocusControl(null);
				AddToEditorHistory("return");
			}

			// infos about the mouse point (for putting in 

			/*
			 *   TOOLS
			 * 
			 * */
			// background
			GUIStyle toolback = editorBackground;
			if (leveltype.Equals("web")) {
				toolback = editorWebLevels;
			}
			GUIScalerLabel ( new Rect(toolsRect.x-5,toolsRect.y-5,toolsRect.width+5,toolsRect.height+10), "", toolback);


			// editor X / Y
			int toolsXTmp=toolsX;
			int toolsXTmpMax = toolsX;
			int toolsYTmp=toolsY;

			//			GUIScalerLabel (new Rect (editorX, editorY, editorWidth, editorHeight), "", editorBackground);

			if (expandedLevelEdit) {

				// leveltype
				string[] levelTypes = { "local","web" /*,"live" */};
				string[] levelTypesLabel = { "LOCAL LEVELS","WEB LEVELS" /* ,"LIVE*" */ };
				for (int o=0;o<levelTypes.Length;o++) {
					string typeX = levelTypes[o];
					string labelX = levelTypesLabel[o];
					GUIStyle gs = editorButtonStyle;
					if (typeX.Equals(leveltype)) {
						gs = editorButtonActiveStyle;
					}
					if (typeX.Equals("web")) {
						// gs = editorButtonTypeStyleActiveWeb; 
						if (typeX.Equals(leveltype)) {
							gs = editorButtonActiveStyleWeb; 
						}
					}

					if (GUIScalerButton (new Rect (toolsXTmp , toolsYTmp, 110, 20), ""+labelX, gs)) {
						SetLevelType( typeX );
						// GetLevelAeras ... 
						// only in the case of web!
						if (typeX.Equals("web")) {
							// RemoteGetAreas();
						}
					}
					toolsXTmp = toolsXTmp + 120;


				}


				// SET AUTOR 
				// if (typeX.Equals("web")) {
				// get actual 
				toolsXTmp = toolsXTmp + 2;
				GUIScalerLabel (new Rect (toolsXTmp, toolsYTmp, 80, 20), "AUTOR: ", editorButtonStyle);
				toolsXTmp = toolsXTmp + 82;
				string editorAutorX=GUIScalerTextField (new Rect(toolsXTmp, toolsYTmp,120,20),editorAutor);
				if (!editorAutorX.Equals(editorAutor)) {
					editorAutor = editorAutorX;
					PlayerPrefs.SetString("editorAutor", editorAutor);
				}
				toolsXTmp = toolsXTmp + 1;
				// }
			}

			if (!expandedLevelEdit) {
				toolsXTmp = 10;
				if (GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 220, 20), "[....]", editorButtonStyle)) {
					expandedLevelEdit = !expandedLevelEdit;
				}
				toolsXTmp = toolsXTmp + 220;
				toolsXTmp = toolsXTmp + 120 + 80;
				// toolsXTmp=10;
				// toolsYTmp = toolsYTmp + 28;



			} 


			toolsXTmp = 570 - 30;
			Texture2D t2d = cursorIconExpand;
			if (expandedLevelEdit) {
				t2d = cursorIconShrink;
			}
			if (GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 24, 24), t2d, editorButtonStyle)) {
				expandedLevelEdit = !expandedLevelEdit;
			}


			// start again
			toolsXTmp=toolsX;

			if (!expandedLevelEdit) {
				//if (!leveltype.Equals("web")) {
				toolsYTmp = toolsYTmp + 28;
				//}
			}

			if (expandedLevelEdit) {



				// weblevels
				if (leveltype.Equals("web")) {

					toolsYTmp = toolsYTmp + 28;

					// ok - local
					// toolsYTmp = toolsYTmp + 24;
					// get actual 
					/*				GUIScalerLabel (new Rect (toolsXTmp, toolsYTmp, 80, 20), "AUTOR: ", editorButtonActiveStyle);
				toolsXTmp = toolsXTmp + 60;
*/
					toolsXTmp = 10;
					// toolsYTmp = toolsYTmp + 28;

					// remoteSelection = true;
					// LEVEL
					GUIScalerLabel (new Rect (toolsXTmp, toolsYTmp, 78, 20), "REMOTE: ", editorButtonActiveStyleWeb);
					toolsXTmp = toolsXTmp + 80;
					// display
					if (GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 178, 20), "> "+remoteAreaEdit+"/"+remoteAutorEdit, editorButtonStyleWeb)) {
						remoteArea = remoteAreaEdit;
						remoteAutor = remoteAutorEdit;
						remoteSelection = !remoteSelection;
						if (remoteSelection) {
							RemoteGetAreas();
						}
					}
					toolsXTmp = toolsXTmp + 180;
					// select
					if (GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 80, 20), "SELECT", editorButtonActiveStyleWeb)) {
						// select something else!
						remoteArea = remoteAreaEdit;
						remoteAutor = remoteAutorEdit;
						remoteSelection = !remoteSelection;
						if (remoteSelection) {
							RemoteGetAreas();

						}
					}
					toolsXTmp = toolsXTmp + 82;

					// select
					if (leveltype.Equals("web")) {
						if (editorAutor.Equals("admin")) {
							editorNewArea = GUIScalerTextField (new Rect (toolsXTmp, toolsYTmp, 60, 20), ""+editorNewArea, editorButtonStyleWeb);
							toolsXTmp = toolsXTmp + 62;
							if (GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 60, 20), "NEW", editorButtonActiveStyleWeb)) {
								// add this
								RemoteNewArea(editorNewArea);
							}
							toolsXTmp = toolsXTmp + 82;
						}
					}

					// select
					/*
				 * if (GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 100, 20), "^ UPLOAD LEVELS", editorButtonActiveStyle)) {
					// SAVE IT ... 
					RemoteUpload();
				}
				*/

					// toolsXTmp = toolsXTmp + 82;

					toolsYTmp = toolsYTmp + 28;


					// SELECT
					if (loading) {
						toolsXTmp = 10;
						// toolsYTmp = toolsYTmp + 28;
						GUIScalerLabel (new Rect (toolsXTmp, toolsYTmp, 400, 20), "LOADING ... LOADING ... LOADING ...", editorLoading);
						// loadingLabel
						toolsYTmp = toolsYTmp + 22;
						GUIScalerLabel (new Rect (toolsXTmp, toolsYTmp, 400, 20), ""+loadingLabel, editorLoading);
						toolsYTmp = toolsYTmp + 22;
					} 

					// ERROR
					if (error) {
						// error = fal;
						// toolsYTmp = toolsYTmp + 28;
						// GUIScalerLabel (new Rect (toolsXTmp, toolsYTmp, 400, 20), "LOADING ... LOADING ... LOADING ...", editorLoading);
						// loadingLabel
						toolsYTmp = toolsYTmp + 22;
						toolsXTmp = 20;
						if (GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 300, 20), ""+errorLabel+" [OK]", editorLoading)) {
							error = false;
						}
						toolsYTmp = toolsYTmp + 22;
						toolsYTmp = toolsYTmp + 22;
					} 


					// remote selection? 
					if (remoteSelection) {

						toolsXTmp = 10;

						// ...
						int remoteStartXAutor = toolsXTmp + 150 ;
						int remoteStartYAutor = toolsYTmp;

						// AREAS
						if (!loading) {
							string remotea = "";
							GUIStyle gs = editorButtonStyleWeb; // editorButtonActiveStyle;
							if (remoteArea.Equals("")) {
								gs = editorButtonActiveStyleWeb;
							}
							if (GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 140, 20), "------ AREAS ------", gs)) {
								remoteArea = "";
							}
							toolsXTmp = 10;
							toolsYTmp = toolsYTmp + 21;
							// show them all
							for (int ct=0;ct<arrRemoteAreas.Count;ct++) {
								remotea = (string) arrRemoteAreas[ct];
								string addon = (string) arrRemoteAreasAddOn[ct];
								gs = editorButtonStyleWeb; // editorButtonActiveStyle;
								bool showIt = false;
								if (remoteArea.Equals("")) showIt = true;
								if (remotea.Equals(remoteArea)) {
									gs = editorButtonActiveStyleWeb;
									showIt = true;
								}
								showIt = true;
								if (showIt) {
									// remotea
									if (GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 140, 20), ""+remotea+" "+addon, gs)) {
										// ok set it ..
										remoteArea = remotea;
										remoteAutor = "";
										RemoteGetAreaAutors();
									}
									toolsXTmp = 10;
									toolsYTmp = toolsYTmp + 21;
								}
							}

							// AUTOR?
							// if selected autors
							if (!remoteArea.Equals("")) {
								gs = editorButtonStyleWeb; // editorButtonActiveStyle;
								if (remoteAutor.Equals("")) {
									gs = editorButtonActiveStyleWeb;
								}
								if (GUIScalerButton (new Rect (remoteStartXAutor, remoteStartYAutor, 140, 20), "------ AUTORS ------", gs)) {
									remoteAutor = "";
								}	
								remoteStartYAutor = remoteStartYAutor + 22;

								// if not yet exists .
								// check for existing autor remote levels
								bool existingOwn = false;
								for (int ct=0;ct<arrRemoteAreaAutors.Count;ct++) {
									string autorx = (string) arrRemoteAreaAutors[ct];
									if (autorx.Equals(editorAutor)) {
										existingOwn = true;
									}
								}
								// existing ...
								if (!existingOwn) {
									if (GUIScalerButton (new Rect (remoteStartXAutor, remoteStartYAutor, 140, 20), "[ADD "+editorAutor+"]", editorButtonStyleWeb)) {
										// save a level now ... 
										remoteAreaEdit = remoteArea;
										remoteAutorEdit = editorAutor;
										RemoteUploadDummy();

									}	
									remoteStartYAutor = remoteStartYAutor + 22;
								}

								// autors
								for (int ct=0;ct<arrRemoteAreaAutors.Count;ct++) {
									string autorx = (string) arrRemoteAreaAutors[ct];
									string addon = (string) arrRemoteAreaAutorsAddOn[ct];
									gs = editorButtonStyleWeb; // editorButtonActiveStyle;
									if (autorx.Equals(remoteAutor)) {
										gs = editorButtonActiveStyleWeb;
									}
									if (GUIScalerButton (new Rect (remoteStartXAutor, remoteStartYAutor, 140, 20), ""+autorx+" "+addon, gs)) {
										remoteAutor = autorx;
									}

									// edit / download
									if (autorx.Equals(remoteAutor)) {
										// DOWNLOAD
										// EDIT  
										// ADMIN

										// ADDON?
										if (addon.IndexOf("(local)")!=-1) {
											if (GUIScalerButton (new Rect (remoteStartXAutor+150, remoteStartYAutor, 58, 20), "EDIT", gs)) {
												remoteAreaEdit = remoteArea;
												remoteAutorEdit = remoteAutor;
												PlayerPrefs.SetString("remoteAreaEdit", remoteAreaEdit);
												PlayerPrefs.SetString("remoteAutorEdit", remoteAutorEdit);
												RemoteEdit();
											} }
										if (GUIScalerButton (new Rect (remoteStartXAutor+150 + 60, remoteStartYAutor, 130, 20), "DOWNLOAD & EDIT", gs)) {
											remoteAreaEdit = remoteArea;
											remoteAutorEdit = remoteAutor;
											PlayerPrefs.SetString("remoteAreaEdit", remoteAreaEdit);
											PlayerPrefs.SetString("remoteAutorEdit", remoteAutorEdit);
											RemoteDownload();
										}
									}

									// toolsXTmp = 10;
									remoteStartYAutor = remoteStartYAutor + 21;
									if (remoteStartYAutor>toolsYTmp) {
										toolsYTmp = remoteStartYAutor;
									}

									// remoteStartYAutor = remoteStartYAutor + 21;
								}
							}

						} // loading

						// local stored ...
						toolsYTmp = toolsYTmp + 28;

						toolsXTmp = 10;
						// toolsYTmp = toolsYTmp + 28;
					} // remoteSelection

				}


				// weblevels
				if (leveltype.Equals("web")) {
					toolsXTmp = 10;
					// toolsYTmp = toolsYTmp + 28;
					// admin things
					if (editorAutor.Equals("admin")) {
						if (GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 80, 20), "DEL", editorButtonActiveStyleWeb)) {
							RemoteDelete();
						}
						toolsXTmp = toolsXTmp + 82;
					}
					// PASS
					GUIScalerLabel (new Rect (toolsXTmp, toolsYTmp, 40, 20), "PASS:", editorButtonActiveStyleWeb);
					toolsXTmp = toolsXTmp + 42;
					editorPassword = GUIScalerTextField (new Rect (toolsXTmp, toolsYTmp, 120, 20), ""+editorPassword, editorButtonStyleWeb);
					GUIScalerLabel (new Rect (toolsXTmp, toolsYTmp, editorPassword.Length*9, 20), "", editorButtonActiveStyleWeb);
					toolsXTmp = toolsXTmp + 122;

					// UPLOAD SELECTED LEVEL
					if (GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 140, 20), "UPLOAD LEVEL "+actualEditorLevel, editorButtonActiveStyleWeb)) {
						// SAVE IT ... 
						RemoteUploadOnly(actualEditorLevel);
					}
					toolsXTmp = toolsXTmp + 142;


					// UPLOAD ALL
					if (GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 140, 20), "UPLOAD LEVELS 0-"+(maxLevel-1), editorButtonActiveStyleWeb)) {
						// SAVE IT ... 
						RemoteUpload();
					}
					toolsXTmp = toolsXTmp + 140;

					/*
				if (GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 120, 20), "UPLOAD AS "+editorAutor+"", editorButtonActiveStyle)) {
					// SAVE IT ... 
					// RemoteUpload();
				}
				toolsXTmp = toolsXTmp + 122;
				*/

				}

				toolsXTmp = 10;
				toolsYTmp = toolsYTmp + 28;

				// working on level
				// maxLevel
				GUIScalerLabel (new Rect (toolsXTmp, toolsYTmp, 60, 20), "LEVEL: ", editorButtonActiveStyle);
				toolsXTmp = 10;

				// levels
				toolsXTmp = toolsXTmp + 62;

				// clear
				if (GUIScalerButton (new Rect (toolsXTmp , toolsYTmp, 58, 20), "CLEAR", editorButtonStyle)) {
					ClearLevel ();  
					// NewLevel();
					DefaultElements();
					SaveLevel (actualEditorLevel);
				}
				toolsXTmp = toolsXTmp + 60;

				// all level (imported first)
				if (true) {
					GUIStyle gui = editorButtonStyle;
					if (actualEditorLevel == allLevel) {
						gui = editorButtonActiveStyle;
						// text = ">" + text + "";
					}
					bool buttonClicked = GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 20, 20), "A", gui);
					if (buttonClicked) {
						// scroll=0.0f;
						// EditorUpdateCameraToScroll(scroll);
						//stateSpecialEditor="";
						// SetLevel (i);
						LoadEditorLevel( allLevel );
					}
				}
				toolsXTmp = toolsXTmp + 22;

				// levels
				for (int i=0; i<maxLevel; i++) {
					string text = "" + i;
					// actualLevel
					GUIStyle gui = editorButtonStyle;
					if (i == actualEditorLevel) {
						gui = editorButtonActiveStyle;
						// text = ">" + text + "";

						// delete
						if (i!=0) {
							if (GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 10, 20), "-", editorButtonStyle)) {
								RemoveActualLevel();
								LoadEditorLevel(actualEditorLevel);
							}
							toolsXTmp = toolsXTmp + 12;
						}
						// add
						/*
					if (GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 10, 20), " ", editorButtonStyle)) {
						InsertLevelBeforeActualLevel();
					}
					toolsXTmp = toolsXTmp + 12;
*/
					}

					bool buttonClicked = GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 20, 20), text, gui);
					if (buttonClicked) {
						// scroll=0.0f;
						// EditorUpdateCameraToScroll(scroll);
						//stateSpecialEditor="";
						// SetLevel (i);
						LoadEditorLevel( i );
					}

					toolsXTmp = toolsXTmp + 22;

					if (i == actualEditorLevel) {
						// add
						if (GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 10, 20), "+", editorButtonStyle)) {
							InsertLevelAfterActualLevel();
						}
						toolsXTmp = toolsXTmp + 12;

					}
				}

				// MAX
				if (GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 40, 20), "+", editorButtonActiveStyle)) {
					// toggle max
					if (maxLevel==minMaxLevel) { 
						maxLevel=maxMaxLevel; 
					}
					else {
						maxLevel=maxMaxLevel; 
					}
				}
				toolsXTmp = toolsXTmp + 40;

				toolsXTmp = toolsXTmp + 2;

				// COPY
				if (GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 60, 20), "COPY", editorButtonActiveStyle)) {
					CopyActualLEvelToClipBoard();

				}
				toolsXTmp = toolsXTmp + 62;
				// PASTE
				if (GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 60, 20), "PASTE", editorButtonActiveStyle)) {
					ClipBoardToActualLevel();
				}
				toolsXTmp = toolsXTmp + 62;

				// COPYTO > NOT ANYMORE USED > USE COPY/PASTE > YOU SEE WHAT YOU OVERWRITE!
				/*
			if (GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 60, 20), "SAVE AS ", editorButtonActiveStyle)) {
				flagShowSaveAs = !flagShowSaveAs;
			}
			toolsXTmp = toolsXTmp + 62;

			if (flagShowSaveAs) {
				// levels
				for (int i=0; i<maxLevel; i++) {
					string text = "" + i;
					// actualLevel
					GUIStyle gui = editorButtonStyle;

					bool buttonClicked = GUIScalerButton (new Rect (	toolsXTmp, toolsYTmp, 20, 20), text, gui);
					if (buttonClicked) {
						SaveLevel (i);
						gameLogic.SetGameLevel(i);
						flagShowSaveAs = !flagShowSaveAs;
					}
					toolsXTmp = toolsXTmp + 22;
				}
			}
*/
				if (toolsXTmp>toolsXTmpMax) toolsXTmpMax = toolsXTmp;

				toolsYTmp = toolsYTmp + 26;


			} // shrink

			// UNDO/REDO
			// undoVisual
			toolsXTmp = toolsX;
			// toolsXTmp = toolsX;

			// editorY = editorY + 26;
			// tools
			for (int i=0; i<arrEditorTools.Length; i++) {
				string tool = arrEditorTools [i];
				string text = tool;
				GUIStyle gui = editorButtonStyle;
				if (text.Equals (editorTool)) {
					gui = editorButtonActiveStyle;
					text = "[" + text + "]";
				}
				bool buttonClicked = GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 66, 20), text, gui);
				if (buttonClicked) {

					// default deselect
					// editorSelected=null;
					SetSelectedElement(null);

					// set the tool
					SetTool (tool);
				}

				toolsXTmp = toolsXTmp + 68;
			}
			// toolsYTmp = toolsYTmp + 24;

			toolsXTmp = toolsXTmp + 6;

			// UNDO/REDO
			if (GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 48, 20), "UNDO", editorButtonActiveStyle)) {
				Undo();
			}  
			toolsXTmp = toolsXTmp + 50;
			ArrayList arrx = GetActualEditorHistory();
			if (GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 38, 20), "" + (arrx.Count-historyIndexMinus) + "/" + arrx.Count, editorButtonStyle)) {
				Redo();
			}
			toolsXTmp = toolsXTmp + 40;
			if (GUIScalerButton (new Rect (toolsXTmp, toolsYTmp, 40, 20), "REDO", editorButtonActiveStyle)) {
				Redo();
			}
			toolsXTmp = toolsXTmp + 54;





			toolsYTmp = toolsYTmp + 22;


			if (toolsXTmp>toolsXTmpMax) toolsXTmpMax = toolsXTmp;



			toolsRect.x = toolsX;
			toolsRect.y = toolsY;
			toolsRect.width = toolsXTmpMax-toolsX;
			toolsRect.height = toolsYTmp - toolsY;


			// add it ...
			inspectorY = toolsYTmp + 26;


			// inspector ... 
			// background
			GUIScalerLabel ( new Rect(inspectorRect.x-5,inspectorRect.y-5,inspectorRect.width+5,inspectorRect.height+10), "", editorInspectorBackground);
			int inspectorXTmp = inspectorX ;
			int inspectorYTmp = inspectorY;

			if (GUIScalerButton (new Rect(inspectorXTmp,inspectorYTmp,200,20),""+editorTool,editorButtonStyleBig)) {
				SetSelectedElementFromGUI();
			}
			inspectorYTmp = inspectorYTmp +40;

			// RASTER & ROTATE
			if (editorTool.Equals("CREATE")) {

				// tools?
				int county = 0;
				GameElement gelement;
				for (int a=0; a<arrGameElementTypes.Count; a++) {
					gelement = (GameElement)arrGameElementTypes [a];
					GUIStyle gox = editorButtonStyle;
					if (gelement.subtype.Substring(0,1).Equals("+")) {
						if (editorSelected!=null) {
							if (gelement.type.Equals(editorSelected.type)&&gelement.subtype.Equals(editorSelected.subtype)) {
								gox = editorButtonActiveStyle;
							}
						}
						if (GUIScalerButton (new Rect(inspectorXTmp,inspectorYTmp,68,20),""+gelement.subtype,gox)) {
							SetEditorArea (gelement.type); SetSubEditorArea (gelement.subtype);
						}
						inspectorXTmp = inspectorXTmp + 70;
						county ++;
						if (inspectorXTmp>maxXToWrap) {
							inspectorYTmp = inspectorYTmp + 22;
							inspectorXTmp = 10;
						}
					}
				}

				inspectorYTmp = inspectorYTmp + 10;

				inspectorXTmp = 10;
				inspectorYTmp = inspectorYTmp + 22;

				// name
				GUIScalerLabel (new Rect(inspectorXTmp,inspectorYTmp,110,20),"NAME: ",guiEvaluation);
				createName=GUIScalerTextField (new Rect(inspectorXTmp+120,inspectorYTmp,200,20),createName);

				inspectorXTmp = 10;
				inspectorYTmp = inspectorYTmp + 22;

				inspectorYTmp = inspectorYTmp + 10;




				// rasters
				// attention double in code > MOVE & CREATE
				GUIScalerButton (new Rect (inspectorXTmp , inspectorYTmp, 58, 20), "RASTER", editorButtonStyle);
				inspectorXTmp = inspectorXTmp + 70;
				for (int i=0; i<arrRasters.Length; i++) {
					float raster = arrRasters [i];
					string text = ""+raster;
					GUIStyle gui = editorButtonStyle;
					if (editorRaster==i) {
						gui = editorButtonActiveStyle;
						text = "" + text + "";
					}
					bool buttonClicked = GUIScalerButton (new Rect (inspectorXTmp , inspectorYTmp, 22, 20), text, gui);
					if (buttonClicked) {
						SetRasterIndex ( i );
					}

					inspectorXTmp = inspectorXTmp + 24;
				}

				// raster rotation
				inspectorXTmp = 10;
				inspectorYTmp = inspectorYTmp + 24;

				GUIScalerButton (new Rect (inspectorXTmp , inspectorYTmp, 58, 20), "ROTATE", editorButtonStyle);
				inspectorXTmp = inspectorXTmp + 60;

				inspectorXTmp = inspectorXTmp + 10;

				int deg = 0;
				for (int i=0; i<22; i++) {
					/*if (i==1 || i==5 || i==8 || i==10 || i==11 || i==13 || i==14 ||
						i==16 || i==17 || i==19) continue;*/
					if(i==0 || i==3 || i==6 || i==9 || i==12 || i==15 || i==18 || i==21){
						string text = ""+(i*15);
						GUIStyle gui = editorButtonStyle;
						if (editorDegree==(i*15)) {
							gui = editorButtonActiveStyle;
							text = "" + (i*15) + "";
						}
						bool buttonClicked = GUIScalerButton (new Rect (inspectorXTmp , inspectorYTmp, 26, 20), text, gui);
						if (buttonClicked) {
							editorDegree = i * 15;
							Transform cursorPreviewT = GameObject.Find("editorcursorpreview").transform;
							if(cursorPreviewT.childCount>0){
								foreach(Transform childT in cursorPreviewT){
									childT.localRotation = Quaternion.Euler(0,editorDegree,0);
								}
							}
						}

						inspectorXTmp = inspectorXTmp + 28;
					}
				}

				inspectorXTmp = inspectorXTmp + 10;

				// ROTATE
				bool rotated = false;
				// inspectorXTmp = inspectorXTmp + 28;
				if (GUIScalerButton (new Rect (inspectorXTmp , inspectorYTmp, 26, 20), " < ", editorButtonStyle)) {
					editorDegree = editorDegree - 90	;	
					if (editorDegree==-90) editorDegree = 270;
					rotated = true;
				}
				inspectorXTmp = inspectorXTmp + 28;
				if (GUIScalerButton (new Rect (inspectorXTmp , inspectorYTmp, 26, 20), " > ", editorButtonStyle)) {
					editorDegree = editorDegree + 90	;				
					if (editorDegree==360) editorDegree = 0;
					rotated = true;
				}
				inspectorXTmp = inspectorXTmp + 28;
				if (rotated) {
					Transform cursorPreviewT = GameObject.Find("editorcursorpreview").transform;
					if(cursorPreviewT.childCount>0){
						foreach(Transform childT in cursorPreviewT){
							childT.localRotation = Quaternion.Euler(0,editorDegree,0);
						}
					}
				}

				inspectorYTmp = inspectorYTmp + 24;
				inspectorXTmp = 10;



			}


			// inspectorYTmp

			// 3 possibilites
			bool showElements=false;
			bool showElementsSubTypeOnly = false;
			if (editorTool.Equals ("CREATE")) { showElements=true; }
			if (editorTool.Equals ("EDIT")) { 
				if (editorSelected==null) {
					// edit > on edit
					//editorSelected = editorLastTouchedGameElement;
					SetSelectedElement( editorLastTouchedGameElement );
					if (editorSelected!=null) {
						if (!filterType.Equals("*")) {
							if (filterType.Equals(editorSelected.type)) {
								// showElementsSubTypeOnly = true;
							} else {
								filterType = editorSelected.type;
							}
						}
					}
				} else {
					showElementsSubTypeOnly = true; 

				}
			}

			// splitted - no code for heaven
			if (editorTool.Equals ("EDIT")) { 

				if (editorSelected!=null) {
					inspectorXTmp = inspectorXTmp +380;
					if (GUIScalerButton (new Rect(inspectorXTmp,inspectorYTmp-28,60,20),"DELETE",editorButtonStyle)) {
						RemoveSelectedElement();
						// add to editor history
						AddToEditorHistory("[GUI][OBJECT][DELETE]");
					}

					// label
					GUIScalerLabel (new Rect(inspectorXTmp-300,inspectorYTmp-28,280,20),"("+editorSelected.type+"."+editorSelected.subtype+") "+editorSelected.creator+" ["+editorSelected.position.x+","+editorSelected.position.y+","+editorSelected.position.z+"]",editorButtonStyle);
					// inspectorYTmp = inspectorYTmp + 24; 
					inspectorXTmp = 10;

					if (editorSelected!=null) {

						if ((!editorSelected.ingameSource.Equals("self"))&&(!editorSelected.ingameSource.Equals(""))) {
							inspectorYTmp=inspectorYTmp+24;
							GUIScalerLabel (new Rect(inspectorXTmp,inspectorYTmp,280,20),"------------ NOT STORED HERE > A|1987 ------------------",editorButtonStyle);
							inspectorXTmp = 10;
							inspectorYTmp = inspectorYTmp + 24; 
							inspectorYTmp=inspectorYTmp+24;

						}

						// specials refer type?
						string release=""+editorSelected.release;
						string add="";
						if (GUIScalerButton (new Rect(inspectorXTmp,inspectorYTmp,58,20),add+"STATE: ",editorButtonStyle)) {
							editorSelected.release="";
							AddToEditorHistory("state");
						}

						GUIStyle gs = editorButtonStyle;
						inspectorXTmp= inspectorXTmp + 60;
						if (release.Equals ("")) { /* add=">"; */ gs = editorButtonActiveStyle;  }
						if (GUIScalerButton (new Rect(inspectorXTmp,inspectorYTmp,58,20),add+"active",gs)) {
							editorSelected.release="";
							AddToEditorHistory("active");
						}
						inspectorXTmp= inspectorXTmp + 60;
						add = "";
						gs = editorButtonStyle;
						if (release.Equals ("wait")) { /* add=">"; */  gs = editorButtonActiveStyle; }
						if (GUIScalerButton (new Rect(inspectorXTmp,inspectorYTmp,58,20),add+"hidden",gs)) {
							editorSelected.release="wait";
							AddToEditorHistory("wait");
						}
						inspectorXTmp= inspectorXTmp + 60;
						inspectorYTmp = inspectorYTmp + 30; 

						inspectorXTmp = 10;

						// name
						GUIScalerLabel (new Rect(inspectorXTmp,inspectorYTmp,240,20),"NAME: #");
						editDetailName=GUIScalerTextField (new Rect(inspectorXTmp+50,inspectorYTmp,170,20),editDetailName);
						if (!editorSelected.name.Equals(editDetailName)) {
							editorSelected.name=editDetailName + "";
							UpdateElementVisual(editorSelected);
							AddToEditorHistory("[GUI][OBJECT][CHANGEDNAME]");
						}
						editorSelected.name=editDetailName + "";
						inspectorXTmp = inspectorXTmp +218;


						if (GUIScalerButton (new Rect(inspectorXTmp,inspectorYTmp,100,20),"DUPLICATE ++",editorButtonStyle)) {
							// copy here ... 
							GameElement copyThis = editorSelected.Copy ();
							copyThis.position.x=copyThis.position.x+UnityEngine.Random.Range ( 0.3f, 0.9f );
							// copyThis.position.y=UnityEngine.Random.Range ( 0.5f, 1.0f );
							AddElement(copyThis);

							// add to editor history
							AddToEditorHistory("[GUI][OBJECT][DUPLICATE]");


						}

						inspectorYTmp = inspectorYTmp + 32; 
						inspectorXTmp = 10;

						if (editorSelected!=null) {
							showElements=true; 

							float editorDetailY=inspectorY;

							// show size
							inspectorXTmp = 10;
							inspectorYTmp = inspectorYTmp + 10;
							float[] arrScales = { 0.2f, 0.5f, 0.75f, 1.0f, 1.5f, 2.0f, 3.0f, 4.0f, 8.0f };
							GUIScalerButton (new Rect (inspectorXTmp, inspectorYTmp, 60, 20), "SIZE", editorButtonStyle );
							for (int i=0; i<arrScales.Length; i++) {
								float size = arrScales [i];
								string text = ""+size;
								GUIStyle gui = editorButtonStyle;
								if (editorSelected.size==size) {
									gui = editorButtonActiveStyle;
									// text = ">" + text + "";
								}
								bool buttonClicked = GUIScalerButton (new Rect (inspectorXTmp + 62 + i * 24, inspectorYTmp, 22, 20), text , gui );
								if (buttonClicked) {
									editorSelected.size =size;
									UpdateElementVisual( editorSelected );
									AddToEditorHistory("[GUI][OBJECT][SIZE]"+size);
								}
							}

							inspectorYTmp = inspectorYTmp + 22; 

							GUIScalerButton (new Rect (inspectorXTmp , inspectorYTmp, 58, 20), "ROTATE", editorButtonStyle);
							inspectorXTmp = inspectorXTmp + 60;

							int deg = 0;
							for (int i=0; i<22; i++) {
								/*if (i==1 || i==5 || i==8 || i==10 || i==11 || i==13 || i==14 ||
						i==16 || i==17 || i==19) continue;*/
								if(i==0 || i==3 || i==6 || i==9 || i==12 || i==15 || i==18 || i==21){
									string text = ""+(i*15);
									GUIStyle gui = editorButtonStyle;
									if (editorSelected.rotation==(i*15)) {
										gui = editorButtonActiveStyle;
										text = "" + (i*15) + "";
									}
									bool buttonClicked = GUIScalerButton (new Rect (inspectorXTmp , inspectorYTmp, 26, 20), text, gui);
									if (buttonClicked) {
										// editorDegree = i * 15;
										editorSelected.rotation = i * 15;
										UpdateElementVisual(editorSelected);
										AddToEditorHistory("Editor UpdateObject");
									}

									inspectorXTmp = inspectorXTmp + 28;
								}
							}

							// ROTATE
							bool rotated = false;
							inspectorXTmp = inspectorXTmp + 10;
							if (GUIScalerButton (new Rect (inspectorXTmp , inspectorYTmp, 26, 20), " < ", editorButtonStyle)) {
								editorSelected.rotation = editorSelected.rotation - 90;
								if (editorSelected.rotation==-90) editorSelected.rotation = 270;
								rotated = true;
							}
							inspectorXTmp = inspectorXTmp + 28;
							if (GUIScalerButton (new Rect (inspectorXTmp , inspectorYTmp, 26, 20), " > ", editorButtonStyle)) {
								editorSelected.rotation = editorSelected.rotation - 90;
								if (editorDegree==360) editorSelected.rotation = 0;
								rotated = true;
							}
							inspectorXTmp = inspectorXTmp + 28;
							if (rotated) {
								UpdateElementVisual(editorSelected);
								AddToEditorHistory("Editor UpdateObject");
							}

							inspectorYTmp = inspectorYTmp + 22; 
							// inspectorYTmp = inspectorYTmp + 10;

							inspectorXTmp = 10;

							// take from camera
							if (GUIScalerButton (new Rect (inspectorXTmp , inspectorYTmp, 180, 20), "TAKE FROM CAMERA", editorButtonStyle)) {
								GameObject container=GameObject.Find ("editorCameraContainer");
								GameObject editorcamera=GameObject.Find ("editorcamera");

								editorSelected.position.x=container.transform.position.x;
								editorSelected.position.y=container.transform.position.y;
								editorSelected.position.z=container.transform.position.z;
								// and transform
								editorSelected.rotation = 0 + container.transform.eulerAngles.y; //  - 180; 
								editorSelected.rotationForward = 0 + editorcamera.transform.eulerAngles.x;

								// Debug.Log("HandleMouseDownToCreate('camera) // "+arg.rotation; // +" "+arg.rotationForward);

								UpdateElementVisual(editorSelected); 
								AddToEditorHistory("POSITION TAKEN FROM CAMERA");
							}


							inspectorYTmp = inspectorYTmp + 24;
							inspectorXTmp = 10;

							inspectorYTmp = inspectorYTmp + 10; 



							if (GUIScalerButton (new Rect(inspectorXTmp,inspectorYTmp,200,20),"TYPE: "+editorSelected.type+"/"+editorSelected.subtype,editorButtonStyle)) {

							}

							inspectorXTmp = inspectorXTmp +210;

							if (GUIScalerButton (new Rect(inspectorXTmp,inspectorYTmp,60,20),"SAME >",editorButtonStyle)) {
								filterType = editorSelected.type;
								filterTypeSub = editorSelected.subtype;
							}

							inspectorYTmp = inspectorYTmp + 22;



						}
					}
				}

			}



			// show elements
			if ((showElements)||(showElementsSubTypeOnly)) {

				inspectorXTmp = 10;

				GUIScalerLabel (new Rect (inspectorXTmp, inspectorYTmp, 120, 20), "TYPES/SUBTYPES: ", editorButtonStyle);
				//				inspectorXTmp = inspectorXTmp + 124;
				//				GUIScalerLabel (new Rect (inspectorXTmp, inspectorYTmp, 120, 20), "SETTINGS:* (NIGHT,PLUNDER) ", editorButtonStyle);

				inspectorXTmp = 10;
				inspectorYTmp = inspectorYTmp + 22;



				string selectedEditorArea=""+editorArea;

				// categories
				if (!showElementsSubTypeOnly) {
					ArrayList arrTypesUnique = GetElementTypesUnique ();
					int inspectorXTmpTemp = inspectorXTmp;
					int inspectorXAddOnX = 0;
					for (int i=0; i<arrTypesUnique.Count; i++) {
						GameElement unique = (GameElement)arrTypesUnique [i];
						string text = "" + unique.type;
						string ieditorArea = "" + unique.type;
						if (!unique.guiShowInMenu) {
							text = "("+text+")";	
						}

						GUIStyle guix = editorButtonTypeStyleNotActive;

						if (editorTool.Equals ("EDIT")) {  if (editorSelected!=null) { selectedEditorArea=editorSelected.type;  } }
						if (ieditorArea.Equals (selectedEditorArea)) {
							guix = editorButtonTypeStyle;
							// text = ">" + text;

						}
						bool buttonClicked = GUIScalerButton (new Rect (inspectorXTmpTemp, inspectorYTmp, 58, 20), text, guix);
						if (buttonClicked) {
							// do it ...
							// editorArea=ieditorArea;
							if (editorTool.Equals ("CREATE")) {  SetEditorArea (ieditorArea); SetSubEditorArea (unique.subtype);}
							if (editorTool.Equals ("EDIT")) { 
								UpdateElementVisual(editorSelected); 

								editorSelected.type=unique.type; 
								editorSelected.subtype=unique.subtype; 
								// getsize etc ... 
								editorSelected.ChangeTypeInEditMode(unique);
								// attention

								UpdateElementVisual(editorSelected); 
								AddToEditorHistory("update selected");
							}
						}
						// delete objects
						// CountElementsType( string elementArea, string elementSubArea )
						inspectorXTmpTemp = inspectorXTmpTemp + 60;
						if (inspectorXTmpTemp>maxXToWrap) {
							inspectorXAddOnX = inspectorXAddOnX + 3;
							inspectorXTmpTemp = 10 + inspectorXAddOnX;
							inspectorYTmp = inspectorYTmp + 21;
						}

					}
					inspectorYTmp = inspectorYTmp + 22;
				}

				// show subcategories!
				ArrayList arr = GetElementTypes (selectedEditorArea);
				int inspectorXTmpXTemp = 10;
				int inspectorXAddOn = 10;
				for (int a=0; a<arr.Count; a++) {
					GameElement gelement = (GameElement)arr [a];
					string text = "" + gelement.subtype;
					GUIStyle guix = editorButtonTypeSubStyleNotActive;
					string selectedEditorSubArea=""+editorSubArea;
					if (editorTool.Equals ("EDIT")) {  if (editorSelected!=null) { selectedEditorSubArea=editorSelected.subtype;  } }
					if (selectedEditorSubArea.Equals (gelement.subtype)) {
						guix = editorButtonTypeSubStyle;
						// text = ">" + text;
					}
					int count=CountElementsType( gelement.type, gelement.subtype );
					if (count>0) { 
						if (GUIScalerButton (new Rect (inspectorXTmpXTemp , inspectorYTmp, 6, 20), "x", guix)) { 
							RemoveElementsType(gelement.type, gelement.subtype);
							AddToEditorHistory("Delete types");
						} 
					}
					string strCount="";
					if (count>0) strCount="("+count+")";
					bool buttonClicked = GUIScalerButton (new Rect (inspectorXTmpXTemp+5, inspectorYTmp, 70, 20), text+strCount, guix);
					if (buttonClicked) {
						// do it ...
						if (editorTool.Equals ("CREATE")) {  
							// Debug.Log("element selected: "+Time.time);
							SetSubEditorArea (gelement.subtype); 
							if (gelement.editorTileSize!=0.0f) {
								SetRasterIndex(GetRasterIndexFor(gelement.editorTileSize));

							}
						}
						if (editorTool.Equals ("EDIT")) {  
							editorSelected.type=gelement.type; 
							editorSelected.subtype=gelement.subtype;  
							// showElementsSubTypeOnly
							UpdateElementVisual(editorSelected); 
							AddToEditorHistory("edit");
						}

					}

					inspectorXTmpXTemp = inspectorXTmpXTemp + 80;
					if (inspectorXTmpXTemp>maxXToWrap) {
						inspectorXAddOn = inspectorXAddOn + 1;
						inspectorXTmpXTemp = 10 + inspectorXAddOn;
						inspectorYTmp = inspectorYTmp + 21;
					}
				}




				inspectorYTmp = inspectorYTmp + 25;

			}

			if (editorTool.Equals("CREATE")) {

				inspectorYTmp = inspectorYTmp + 2;
				inspectorXTmp = 10;

				bool buttonClickedX = GUIScalerButton (new Rect (inspectorXTmp , inspectorYTmp, 140, 20), "CREATE AT NULL", editorButtonStyle);
				if (buttonClickedX) {
					HandleMouseDownToCreate( "null" );
				}
				inspectorXTmp = inspectorXTmp + 142;
				buttonClickedX = GUIScalerButton (new Rect (inspectorXTmp , inspectorYTmp, 180, 20), "CREATE AT CAMERA", editorButtonStyle);
				if (buttonClickedX) {
					HandleMouseDownToCreate( "camera"  );
				}
				inspectorXTmp = inspectorXTmp + 142;

			}


			// EDIT
			if (editorTool.Equals ("EDIT")) { 





				/*
					if (GUIScalerButton (new Rect(inspectorXTmp,inspectorYTmp,200,20),""+editorTool,editorButtonStyleBig)) {
						SetSelectedElementFromGUI();
					}

					inspectorXTmp = inspectorXTmp +200;
					*/

				inspectorXTmp = 10;

				// todo: check for change!

				// trigger (add some keys)
				if (editorSelected!=null) {

					// use the original labels!
					EditorUpdateArgumentLabels(editorSelected);

					bool showExtendedNotificationInput = false;
					if (editorSelected.type.Equals("trigger")) { 
						if (editorSelected.subtype.Equals("notification")) { showExtendedNotificationInput = true; }
						if (editorSelected.subtype.Equals("switchnotification")) { showExtendedNotificationInput = true; }
						if (editorSelected.subtype.Equals("switcher")) { showExtendedNotificationInput = true; }
						if (editorSelected.subtype.Equals("anykeynotification")) { showExtendedNotificationInput = true; }
						if (editorSelected.subtype.Equals("onstatenotification")) { showExtendedNotificationInput = true; }

					}
					if (editorSelected.type.Equals("action")) { 
						if (editorSelected.subtype.Equals("notification")) { showExtendedNotificationInput = true; }
						if (editorSelected.subtype.Equals("repeaternotification")) { showExtendedNotificationInput = true; }
					}
					if (editorSelected.type.Equals("camcut")) { 
						if (editorSelected.subtype.Equals("timednotification")) { showExtendedNotificationInput = true; }
					}

					if (showExtendedNotificationInput) {
						GUIScalerLabel (new Rect(inspectorXTmp,inspectorYTmp,40,24),"Event:");
						editorSelected.strevent=GUIScalerTextField (new Rect(inspectorXTmp+42,inspectorYTmp,160,20),editorSelected.strevent);
						inspectorYTmp=inspectorYTmp+22;

						GUIScalerLabel (new Rect(inspectorXTmp,inspectorYTmp,40,24),"Target:");
						editorSelected.target=GUIScalerTextField (new Rect(inspectorXTmp+42,inspectorYTmp,160,20),editorSelected.target);
						inspectorYTmp=inspectorYTmp+22;

						GUIScalerLabel (new Rect(inspectorXTmp,inspectorYTmp,40,24),"After:");
						string strTimed =GUIScalerTextField (new Rect(inspectorXTmp+42,inspectorYTmp,160,20),""+editorSelected.timed);
						if (!strTimed.Equals(""+editorSelected.timed)) {
							editorSelected.timed  =   float.Parse( strTimed );
							AddToEditorHistory("after")	;					
						}
						inspectorYTmp=inspectorYTmp+22;
					}


					// timed seperate
					bool timedSeperate = false;
					if (editorSelected.type.Equals("camcut")) { 
						if (editorSelected.subtype.Equals("caption")) { timedSeperate = true; }
						if (editorSelected.subtype.Equals("timedpicture")) { timedSeperate = true; }
						if (editorSelected.subtype.Equals("timedexit")) { timedSeperate = true; }
						if (editorSelected.subtype.Equals("timedsound")) { timedSeperate = true; }
						if (editorSelected.subtype.Equals("timedloop")) { timedSeperate = true; }
					}
					if (timedSeperate) {
						// show it ...
						GUIScalerLabel (new Rect(inspectorXTmp,inspectorYTmp,40,24),"Timed:");
						string strTimed =GUIScalerTextField (new Rect(inspectorXTmp+42,inspectorYTmp,160,20),""+editorSelected.timed);
						if (!strTimed.Equals(""+editorSelected.timed)) {
							editorSelected.timed  =   float.Parse( strTimed );
							AddToEditorHistory("after")	;					
						}
						inspectorYTmp=inspectorYTmp+22;
					}
				}

				if (editorSelected!=null) {	
					if (editorSelected.guiBoolArgument) {

						string showArgumentType = "text";

						// variants
						if (editorSelected.prefabPredefinedArguments.Length>0) {
							showArgumentType = "variants";	
						}

						// 'switch'

						// TEXT
						if (showArgumentType.Equals("text")) {
							// show argument
							GUIScalerLabel (new Rect(inspectorXTmp,inspectorYTmp,40,24),""+editorSelected.guiLabel+":");
							editDetailArgument=GUIScalerTextField (new Rect(inspectorXTmp+42,inspectorYTmp,320,20),editDetailArgument);
							bool changed=false;
							if (editDetailArgument!=editorSelected.argument) {
								changed=true;
								AddToEditorHistory("editix");
							}
							editorSelected.argument=editDetailArgument;
							inspectorYTmp=inspectorYTmp+22*1;
							if (changed) {
								UpdateElementVisual(editorSelected);
								UpdateRelationVisualisationAndCheckError();
								AddToEditorHistory("argument");
							}
						}

						// VARIANTS
						if (showArgumentType.Equals("variants")) {
							LevelElementOption leo;
							GUIScalerLabel (new Rect(inspectorXTmp,inspectorYTmp,78,20),"VARIANTS: ");
							inspectorXTmp = inspectorXTmp + 80;

							bool found = false;
							for (int ix = 0; ix < editorSelected.prefabPredefinedArguments.Length; ix ++) {
								leo = editorSelected.prefabPredefinedArguments[ix];
								// Debug.Log(ix+". LeveLElementOption "+leo.argument+" vs "+elem.argument);
								/*
									 * if (leo.argument.Equals(elem.argument)) {
										if (leo.gameobjectPrefab!=null) {
											found=true;
											go=Instantiate(leo.gameobjectPrefab, new Vector3(elem.position.x,elem.position.y,elem.position.z), re) as GameObject;
											go.name = "NotFound9GameObject";

										}
									}
									*/
								// check for correct ...
								GUIStyle gs =  editorButtonStyle;
								if (editorSelected.argument.Equals(""+leo.argument)) {
									gs = editorButtonActiveStyle;
									found = true;
								}
								if (GUIScalerButton (new Rect (inspectorXTmp, inspectorYTmp, 38, 20), ""+leo.argument, gs )) {
									editorSelected.argument = leo.argument;
									UpdateElementVisual(editorSelected);
									AddToEditorHistory("VariantChanged");
								}
								inspectorXTmp = inspectorXTmp + 40;
								if (inspectorXTmp>maxXToWrap) {
									inspectorXTmp = 40;
									inspectorYTmp = inspectorYTmp + 22;
								}


							} 

							// not found!
							// if (!found) {
							GUIStyle gsx =  editorButtonStyle;
							if (!found) gsx = editorButtonActiveStyle;
							// inspectorXTmp = inspectorXTmp + 40;
							if (GUIScalerButton (new Rect (inspectorXTmp, inspectorYTmp, 68, 20), "default", gsx )) {
								editorSelected.argument = "";
								UpdateElementVisual(editorSelected);
								AddToEditorHistory("VariantChanged");
							}
							// }

							inspectorYTmp=inspectorYTmp+22*1;
							inspectorYTmp=inspectorYTmp+5;
							inspectorXTmp = 10;
						}


					}
					// description?
					if (!editorSelected.guiDescription.Equals ("")) {
						GUIScalerLabel (new Rect(inspectorXTmp+42,inspectorYTmp,240,20),editorSelected.guiDescription);
						inspectorYTmp=inspectorYTmp+22*1;
					}
					// argumentsub
					if (editorSelected.guiBoolArgument) {
						GUIScalerLabel (new Rect(inspectorXTmp,inspectorYTmp,40,24),"ARG:");
						editDetailArgumentSub=GUIScalerTextField (new Rect(inspectorXTmp+42,inspectorYTmp,320,20),editDetailArgumentSub);
						bool changed=false;
						if (editDetailArgumentSub!=editorSelected.argumentsub) {
							changed=true;
							AddToEditorHistory("arg");
						}
						editorSelected.argumentsub=editDetailArgumentSub;
						inspectorYTmp=inspectorYTmp+22*1;
						if (changed) {
							UpdateElementVisual(editorSelected);
							UpdateRelationVisualisationAndCheckError();
							AddToEditorHistory("arg");
						}

					}

				}


				/*
					// inspectorXTmp=inspectorXTmp + 62 + 5 + arrScales.Length*24;
				if (GUIScalerButton (new Rect (inspectorXTmp, inspectorYTmp, 64, 20), "TOP:", editorButtonStyle )) {
					editorSelected.rotation =0.0f;
					UpdateElementVisual( editorSelected );
					AddToEditorHistory("[GUI][OBJECT][Y]Reset");
				}
				inspectorXTmp = inspectorXTmp +64;
				if (GUIScalerButton (new Rect (inspectorXTmp, inspectorYTmp, 18, 20), "-", editorButtonStyle )) {
					editorSelected.position.y =editorSelected.position.y - 0.1f;
					UpdateElementVisual( editorSelected );
					AddToEditorHistory("[GUI][OBJECT][Y]-");
				}
				inspectorXTmp=inspectorXTmp+20;
				if (GUIScalerButton (new Rect (inspectorXTmp, inspectorYTmp, 38, 20), ""+editorSelected.position.y, editorButtonStyle )) {
					//editorSelected.rotation =editorSelected.rotation - 10.0f;
					//UpdateElementVisual( editorSelected );

				}
				inspectorXTmp=inspectorXTmp+40;
				if (GUIScalerButton (new Rect (inspectorXTmp, inspectorYTmp, 20, 20), "+", editorButtonStyle )) {
					editorSelected.position.y =editorSelected.position.y + 0.1f;
					UpdateElementVisual( editorSelected );
					AddToEditorHistory("[GUI][OBJECT][Y]+");
				}
				inspectorXTmp = inspectorXTmp +42;

				inspectorXTmp = 10;
				inspectorYTmp = inspectorYTmp + 22;
				// rotations
				// inspectorXTmp=inspectorXTmp + 62 + 5 + arrScales.Length*24;
				if (GUIScalerButton (new Rect (inspectorXTmp, inspectorYTmp, 38, 20), "ROT:", editorButtonStyle )) {
					editorSelected.rotation =0.0f;
					UpdateElementVisual( editorSelected );
					AddToEditorHistory("[GUI][OBJECT][RX]0");
				}
				inspectorXTmp=inspectorXTmp+40;
				if (GUIScalerButton (new Rect (inspectorXTmp, inspectorYTmp, 8, 20), "<", editorButtonStyle )) {
					editorSelected.rotation =editorSelected.rotation + 10.0f;
					UpdateElementVisual( editorSelected );
					AddToEditorHistory("[GUI][OBJECT][RX]<");
				}
				inspectorXTmp=inspectorXTmp+10;
				if (GUIScalerButton (new Rect (inspectorXTmp, inspectorYTmp, 28, 20), ""+editorSelected.rotation, editorButtonStyle )) {
					//editorSelected.rotation =editorSelected.rotation - 10.0f;
					//UpdateElementVisual( editorSelected );
				}
				inspectorXTmp=inspectorXTmp+30;
				if (GUIScalerButton (new Rect (inspectorXTmp, inspectorYTmp, 8, 20), ">", editorButtonStyle )) {
					editorSelected.rotation =editorSelected.rotation - 10.0f;
					UpdateElementVisual( editorSelected );
					AddToEditorHistory("[GUI][OBJECT][RX]>");
				}
				inspectorXTmp = inspectorXTmp +10;
				if (GUIScalerButton (new Rect (inspectorXTmp, inspectorYTmp, 10, 20), "~", editorButtonStyle )) {
					editorSelected.rotation = UnityEngine.Random.Range(0.0f,360.0f);
					UpdateElementVisual( editorSelected );
					AddToEditorHistory("[GUI][OBJECT][RX]RND");
				}
				inspectorXTmp = inspectorXTmp +12;
				*/ 



				/*
				inspectorXTmp=inspectorXTmp + 62 + 5 + arrScales.Length*24;
				float[] arrRotations = { 0.0f, 30.0f, 60.0f, 90.0f };
				GUIScalerButton (new Rect (inspectorXTmp, inspectorYTmp, 60, 20), "ROT", editorButtonStyle );
				for (int i=0; i<arrRotations.Length; i++) {
					float rotation = arrRotations [i];
					string text = ""+rotation;
					GUIStyle gui = editorButtonStyle;
					if (editorSelected.rotation==rotation) {
						gui = editorButtonActiveStyle;
						text = ">" + text + "";
					}
					bool buttonClicked = GUIScalerButton (new Rect (inspectorXTmp + 62 + i * 24, inspectorYTmp, 22, 20), text , gui );
					if (buttonClicked) {
						editorSelected.rotation =rotation;
						UpdateElementVisual( editorSelected );
					}
				}
				*/

				/*
				* GUIScalerLabel (new Rect(editorDetailX,editorDetailY,40,24),""+editorSelected.guiLabel+":");
				editDetailArgument=GUI.TextField (new Rect(editorDetailX+42,editorDetailY,160,20),editDetailArgument);
				editorSelected.argument=editDetailArgument;

				GUIScalerLabel (new Rect(inspectorXTmp,inspectorYTmp,40,24),""+editorSelected.guiLabel+":");
				editDetailArgument=GUI.TextField (new Rect(inspectorXTmp+42,inspectorYTmp,160,20),editDetailArgument);
				editorSelected.argument=editDetailArgument;
				*/

				inspectorYTmp = inspectorYTmp + 22;
			}

			// if (editorTool.Equals ("MOVE")) { showElements=true; }

			if (editorTool.Equals ("MOVE")) {

				// attention double in code > MOVE & CREATE
				GUIScalerButton (new Rect (inspectorXTmp , inspectorYTmp, 58, 20), "RASTER", editorButtonStyle);
				inspectorXTmp = inspectorXTmp + 70;
				for (int i=0; i<arrRasters.Length; i++) {
					float raster = arrRasters [i];
					string text = ""+raster;
					GUIStyle gui = editorButtonStyle;
					if (editorRaster==i) {
						gui = editorButtonActiveStyle;
						text = "" + text + "";
					}
					bool buttonClicked = GUIScalerButton (new Rect (inspectorXTmp , inspectorYTmp, 22, 20), text, gui);
					if (buttonClicked) {
						SetRasterIndex ( i );
					}

					inspectorXTmp = inspectorXTmp + 24;
				}
			}

			if (editorTool.Equals ("SPLIT")) { 

				// editorToolSub
				string[] arrEditorToolsSub={"right","left","down","up"};
				for (int i=0;i<arrEditorToolsSub.Length;i++) {

					string add="";
					string title=""; 
					GUIStyle gs = editorButtonStyle;
					if (i==0) { title = "ADD |>"; if (editorToolSub.Equals ("right")) {  gs= editorButtonActiveStyle; } }
					if (i==1) { title = "|< DEL"; if (editorToolSub.Equals ("left")) { gs= editorButtonActiveStyle; } }
					if (i==2) { title = "ADD ^"; if (editorToolSub.Equals ("down")) { gs= editorButtonActiveStyle;   } }
					if (i==3) { title = "DEL ^"; if (editorToolSub.Equals ("up")) { gs= editorButtonActiveStyle; } }

					if (GUIScalerButton (new Rect (inspectorXTmp + i*60, inspectorYTmp, 58, 20), add+""+title, gs )) {
						editorToolSub	= arrEditorToolsSub[i];	
					}

				}
				inspectorYTmp = inspectorYTmp + 22;

				// RASTER SPLIT
				inspectorYTmp = inspectorYTmp + 22;
				GUIScalerButton (new Rect (inspectorXTmp , inspectorYTmp, 78, 20), "RASTER SPLIT", editorButtonStyle);
				inspectorXTmp = inspectorXTmp + 80;
				for (int i=0; i<arrRasters.Length; i++) {
					float raster = arrRasters [i];
					string text = ""+raster;
					GUIStyle gui = editorButtonStyle;
					if (lineWidth==arrRasters[i]) {
						gui = editorButtonActiveStyle;
						text = "" + text + "";
					}
					bool buttonClicked = GUIScalerButton (new Rect (inspectorXTmp , inspectorYTmp, 22, 20), text, gui);
					if (buttonClicked) {
						// SetRasterIndex ( i );
						lineWidth = arrRasters [i];
						if (i==0) {
							lineWidth = 0.5f;
						}
					}

					inspectorXTmp = inspectorXTmp + 24;
				}
			}

			// EVALUATION
			if (editorTool.Equals ("EVALU")) { 


				string evData="VISUALIZE ";
				string evDataOn = "ON";
				if (!editorShowEvaluationData) evDataOn = "OFF";
				if (GUIScalerButton (new Rect (inspectorXTmp + 0*60, inspectorYTmp, 118, 20), evData + evDataOn, editorButtonStyle )) {
					ToggleShowEvaluationData(); // editorShowEvaluationData
				}

				if (GUIScalerButton (new Rect (inspectorXTmp + 2*60, inspectorYTmp, 58, 20), "RELOAD", editorButtonStyle )) {
					UpdateShowEvaluationData();
				}

				// FILTERS ...


				inspectorYTmp = inspectorYTmp + 22;

				// PLAYERS
				int dtx = inspectorXTmp; 
				string addOnX = "";
				if (editorEvaluationFilter.Equals("all")) {  } 
				if (GUIScalerButton (new Rect (inspectorXTmp + 0*60, inspectorYTmp, 58, 20), addOnX+"ALL", editorButtonStyle )) {

					if (!editorShowEvaluationData) {
						editorShowEvaluationData = true;
					}

					editorEvaluationFilter = "all";
					editorEvaluationPlayerId = "";
					editorEvaluationSessionId = "";	

					UpdateShowEvaluationData();
				}
				EvaluationPlayer pl;
				if (arrEvaluationPlayersObj!=null)
				if (arrEvaluationPlayersObj.Length>0) 
					for (int o=0;o<arrEvaluationPlayersObj.Length;o++) {
						pl = arrEvaluationPlayersObj[o];
						string addOn = "";
						if (editorEvaluationPlayerId.Equals(pl.playerId))  {
							// addOn = ">";
						}
						if (GUIScalerButton (new Rect (dtx + (o+1)*60, inspectorYTmp, 58, 20), addOn+"" + pl.name, editorButtonStyle )) {
							// UpdateShowEvaluationData();
							// playerId

							if (!editorShowEvaluationData) {
								editorShowEvaluationData = true;
							}

							editorEvaluationFilter = "player";
							editorEvaluationPlayerId = pl.playerId;
							editorEvaluationSessionId = "";	

							UpdateShowEvaluationData();
						}
					}
				inspectorYTmp = inspectorYTmp + 22;

				// session id
				dtx = inspectorXTmp; 
				addOnX = "";
				if (editorEvaluationSessionId.Equals("")) { /* addOnX = ">"; */ } 
				if (GUIScalerButton (new Rect (inspectorXTmp + 0*60, inspectorYTmp, 58, 20), addOnX+"ALL", editorButtonStyle )) {

					if (!editorShowEvaluationData) {
						editorShowEvaluationData = true;
					}

					editorEvaluationFilter = "player";
					// editorEvaluationPlayerId = ""+el.evaluationPlayerId;
					editorEvaluationSessionId = "";	

					UpdateShowEvaluationData();
				}

				ArrayList arrSessions = GetSessionsByPlayerId( editorEvaluationPlayerId );  // arrEvaluationSessions; //  GetSessionsByPlayerId();
				if (editorEvaluationPlayerId.Equals("")) {
					arrSessions = arrEvaluationSessions;
				}
				GameElement el;
				for (int o = 0; o< arrSessions.Count ;o++) {
					el = (GameElement)arrSessions[o];
					string addOn = "";
					if (editorEvaluationSessionId.Equals(""+el.evaluationSessionId)) {
						// addOn = ">";
					}
					if (GUIScalerButton (new Rect (dtx + (o+1)*60, inspectorYTmp, 58, 20), addOn+"/" + el.evaluationSessionId, editorButtonStyle )) {

						editorEvaluationFilter = "player.session";
						editorEvaluationPlayerId = ""+el.evaluationPlayerId;
						editorEvaluationSessionId = ""+el.evaluationSessionId;

						UpdateShowEvaluationData();
					}
				}


			}

			editorHeight = inspectorYTmp + 20;




			// add
			inspectorYTmp = inspectorYTmp + 22;
			// the rect ... 
			inspectorRect.x = inspectorX;
			inspectorRect.y = inspectorY;
			inspectorRect.width = 440; //  inspectorXTmp - inspectorX;
			inspectorRect.height = inspectorYTmp - inspectorY;

			/*
			 * 
			 *  cursors
			 * 
			 * */
			GUIScalerLabel ( new Rect(cursorRect.x-5,cursorRect.y-5,cursorRect.width+5,cursorRect.height+10), "", editorBackground);




			if (true) {
				// render the specials
				SensButton sb;
				// Debug.Log("arrSensButtons.Count: "+arrSensButtons.Count);
				for (int i=0;i<arrSensButtons.Count;i++) {
					sb = (SensButton) arrSensButtons[i];
					GUIScalerLabel ( sb.rect, sb.icon, editorButtonStyle);
				}

			}

			inspectorXTmp = cursorX;
			inspectorYTmp = cursorY + 60; 
			// inspectorYTmp = inspectorXTmp;

			// 1-4
			//				inspectorYTmp = inspectorYTmp + 30; 
			//				inspectorXTmp = cursorX; 
			for (int y=0;y<6;y++) {
				float val = 0.2f + 0.2f * y;
				GUIStyle guixx = editorButtonStyleNotActive;
				if (!cursorObject) {
					if (speedCamera==val) {
						guixx = editorButtonActiveStyle;
					}
				}
				if (cursorObject) {
					if (speedObject==val) {
						guixx = editorButtonActiveStyle;
					}
				}
				if (GUIScalerButton (new Rect (inspectorXTmp, inspectorYTmp, 22, 20), "x"+(y+1), guixx)) {
					if (!cursorObject) speedCamera = val;
					if (cursorObject) speedObject = val;
				} 
				inspectorXTmp = inspectorXTmp + 24; 
			}
			// add raster .. 

			// create cursor
			inspectorXTmp = cursorX+150;
			inspectorYTmp = cursorY;
			GUIStyle guixxx = editorButtonStyleNotActive;
			if (!cursorObject) {
				guixxx = editorButtonActiveStyle;
			}
			if (GUIScalerButton (new Rect (inspectorXTmp, inspectorYTmp, 34, 28), cursorIconCam, guixxx)) {
				cursorObject = false;
			}
			inspectorYTmp = inspectorYTmp + 30;
			guixxx = editorButtonStyleNotActive;
			if (cursorObject) {
				guixxx = editorButtonActiveStyle;
			}
			if (GUIScalerButton (new Rect (inspectorXTmp, inspectorYTmp, 34, 28), cursorIconObject, guixxx)) {
				cursorObject = true;
				SetTool("EDIT" +
					"");
			}
			cursorRect.x = cursorX;
			cursorRect.y = cursorY;
			cursorRect.width = 188;
			cursorRect.height = 80;


			// setup height
			// editorHeight = editorY;

			// the leveltitle
			GameElement titleGE = GetGameElementByName("TITLE");
			string xtitle = "";
			if (titleGE!=null) {
				xtitle = ""+titleGE.argument;
			}
			if (xtitle.Equals("")) {
				xtitle = "[NOT YET A TITLE]";
			}
			xtitle = actualEditorLevel+ ": " + xtitle;
			if (GUIScalerButton ( new Rect(600,20,400,20), ""+xtitle, editorTitleStyle)) {
				// edit here
				SetTool("EDIT");
				SetSelectedElement( titleGE );
			}


			// 	selectiondialoge
			if (selectionDialoge) {


				// FILTER
				// background
				GUIScalerLabel ( new Rect(filterTypeVisual.x,filterTypeVisual.y-5,filterTypeVisual.width+5,filterTypeVisual.height+10), "", editorBackground);
				GUIScalerLabel ( new Rect(filterTypeVisual.x,filterTypeSubVisual.y-5,filterTypeSubVisual.width+5,filterTypeSubVisual.height+10), "", editorBackground);
				GUIScalerLabel ( new Rect(selectionDialogeVisual.x,selectionDialogeVisual.y-5,selectionDialogeVisual.width+5,selectionDialogeVisual.height+10), "", editorBackground);

				/*
			bool selectionDialoge = true;
			string selectFilter = ""; // names etc.  #name types ...  !abc.* !
			Rect selectionDialogeVisual = new Rect(0,0,0,0);
			GameElement[] selectionAffectedElements = {};
*/
				int selectionX = GUIScalerScreenWidth() - selectionSize;
				int selectionY = (int) (GUIScalerScreenHeight() * 0.2f);
				int selectionYStart = selectionY;

				selectionDialogeVisual.x = selectionX;
				selectionDialogeVisual.y = selectionY;
				selectionDialogeVisual.width = selectionSize;


				// ADD IMPORTANT =NAMED THINGS ...
				// SELECTION
				// selectionSize = 220;
				GUIStyle exo = editorButtonStyle;
				if (selectFilter.Equals("")) { exo = editorButtonActiveStyle; selectionSize = minSelectionSize; }
				ArrayList arrNames = GetGameElementsByNotCleanName();
				if (GUIScalerButton (new Rect ( selectionX, selectionY, selectionSize/2, 20), "NAMES "+arrNames.Count+"/"+arrLevel.Count, exo)) {
					selectFilter = "";
				}
				exo = editorButtonStyle;
				if (selectFilter.Equals("selection")) { exo = editorButtonActiveStyle; selectionSize = minSelectionSize; }
				if (GUIScalerButton (new Rect ( selectionX+selectionSize/2+2, selectionY, selectionSize/2, 20), "SELECTION ("+filterType+"/"+filterTypeSub+")", exo)) {
					selectFilter = "selection";
				}

				selectionY = selectionY + 22;
				exo = editorButtonStyle;
				if (selectFilter.Equals("menu")) { exo = editorButtonActiveStyle; selectionSize = minSelectionSize; }
				ArrayList arrListMenu = GetGameElementsByTypeAndSub("guimenu","*");
				string addonpx = "";
				if (arrListMenu.Count>0) addonpx = "("+arrListMenu.Count+")";
				if (GUIScalerButton (new Rect ( selectionX, selectionY, selectionSize/2, 20), "MENU "+addonpx, exo)) {
					selectFilter = "menu";	
				}
				// selectionY = selectionY + 22;
				exo = editorButtonStyle;
				if (selectFilter.Equals("timed")) { exo = editorButtonActiveStyle; selectionSize = minSelectionSize; }
				ArrayList arrListTimed = GetAllGameElementsByTypeAndSubTimeSorted();
				string addonp = "";
				if (arrListTimed.Count>0) addonp = "("+arrListTimed.Count+")";
				if (GUIScalerButton (new Rect ( selectionX + selectionSize/2 +2 , selectionY, selectionSize/2, 20), "TIMED "+addonp+"", exo)) {
					selectFilter = "timed";	
				}
				selectionY = selectionY + 22;
				// triggers
				exo = editorButtonStyle;
				if (selectFilter.Equals("challenges")) { exo = editorButtonActiveStyle; selectionSize = minSelectionSize; }
				ArrayList arrListChallenges =  GetGameElementsByTypeAndSub("challenge","*");
				addonp = "";
				if (arrListChallenges.Count>0) addonp = "("+arrListChallenges.Count+")";
				if (GUIScalerButton (new Rect ( selectionX + 0*selectionSize/3, selectionY, selectionSize/3-2, 20), "CHALLENG "+addonp, exo)) {
					selectFilter = "challenges";	
				}
				// triggers
				exo = editorButtonStyle;
				if (selectFilter.Equals("triggers")) { exo = editorButtonActiveStyle; selectionSize = minSelectionSize; }
				ArrayList arrListTrigger =  GetGameElementsByTypeAndSub("trigger","*");
				addonp = "";
				if (arrListTrigger.Count>0) addonp = "("+arrListTrigger.Count+")";
				if (GUIScalerButton (new Rect ( selectionX + 1*selectionSize/3, selectionY, selectionSize/3-2, 20), "TRIGGERS "+addonp, exo)) {
					selectFilter = "triggers";	
				}
				// actions
				exo = editorButtonStyle;
				if (selectFilter.Equals("actions")) { exo = editorButtonActiveStyle; selectionSize = minSelectionSize; }
				ArrayList arrListLanguage =  GetGameElementsByTypeAndSub("action","*");
				addonp = "";
				if (arrListLanguage.Count>0) addonp = "("+arrListLanguage.Count+")";
				if (GUIScalerButton (new Rect ( selectionX + 2*selectionSize/3, selectionY, selectionSize/3, 20), "ACTIONS "+addonp, exo)) {
					selectFilter = "actions";	
				}
				selectionY = selectionY + 22;
				exo = editorButtonStyle;
				if (selectFilter.Equals("remarks")) { exo = editorButtonActiveStyle; selectionSize = minSelectionSize; }
				ArrayList arrListRemark =  GetGameElementsByTypeAndSub("remark","*");
				addonp = "";
				if (arrListRemark.Count>0) addonp = "("+arrListRemark.Count+")";
				if (GUIScalerButton (new Rect ( selectionX, selectionY, selectionSize, 20), "REMARKS "+addonp, exo)) {
					selectFilter = "remarks";	
				}
				selectionY = selectionY + 22;
				exo = editorButtonStyle;
				if (selectFilter.Equals("language")) { exo = editorButtonActiveStyle; selectionSize = 400; }
				ArrayList arrListX =  GetGameElementsByTypeAndSub("language","*");
				addonp = "";
				if (arrListX.Count>0) addonp = "("+arrListX.Count+")";
				if (GUIScalerButton (new Rect ( selectionX, selectionY, selectionSize, 20), "LANGUAGE "+addonp, exo)) {
					selectFilter = "language";	
				}
				selectionY = selectionY + 22;


				selectionY = selectionY + 6;
				// show [EDIT][SELECTION]
				// types ...


				// selectionY = selectionY + 24;
				int coun = 0;
				if (selectFilter.Equals("")) {
					for (int i=0; i<arrNames.Count; i++) {
						GameElement gae = (GameElement)arrNames [i];
						if (gae.Internal()) {
							string text = "" + gae.name;
							if (gae.name.Equals("")) {
								text = text + "("+gae.subtype+")";
							}
							GUIStyle guix = editorButtonStyleNotActive;
							if (editorSelected==gae) guix = editorButtonActiveStyle;
							bool buttonClicked = GUIScalerButton (new Rect ( selectionX, selectionY, selectionSize, 20), ""+text+"", guix);
							if (buttonClicked) {
								SetTool("EDIT");
								SetSelectedElement(gae);

							}
							selectionY = selectionY + 20;
							coun ++;
							if (i>selectionMaxElements) break;
						}
					} 
					selectionY = selectionY + 20;
				} 
				// selection
				if (selectFilter.Equals("selection")) { 
					ArrayList arrList = GetGameElementsByTypeAndSub(filterType,filterTypeSub);
					for (int i=0; i<arrList.Count; i++) {
						GameElement gae = (GameElement)arrList [i];
						string text = "" + gae.name;
						if (gae.name.Equals("")) {
							// text = text + "("+gae.subtype+")";
						}
						GUIStyle guix = editorButtonStyleNotActive;
						if (editorSelected==gae) guix = editorButtonActiveStyle;
						bool buttonClicked = GUIScalerButton (new Rect ( selectionX, selectionY, selectionSize, 20), ""+text+" ("+gae.type+"/"+gae.subtype+")", guix);
						if (buttonClicked) {
							SetTool("EDIT");
							SetSelectedElement(gae);

						}
						selectionY = selectionY + 20;
						if (i>selectionMaxElements) break;
					} 
					selectionY = selectionY + 20;
				}
				// remarks
				if (selectFilter.Equals("remarks")) { 
					ArrayList arrList = GetGameElementsByTypeAndSub("remark","*");
					for (int i=0; i<arrList.Count; i++) {
						GameElement gae = (GameElement)arrList [i];
						string text = "" + gae.name;
						if (gae.name.Equals("")) {
							// text = text + "("+gae.subtype+")";
						}
						GUIStyle guix = editorButtonStyleNotActive;
						if (editorSelected==gae) guix = editorButtonActiveStyle;
						bool buttonClicked = GUIScalerButton (new Rect ( selectionX, selectionY, selectionSize, 20), ""+text+" ("+gae.type+"/"+gae.subtype+")", guix);
						if (buttonClicked) {
							SetTool("EDIT");
							SetSelectedElement(gae);

						}
						selectionY = selectionY + 20;
						if (i>selectionMaxElements) break;
					} 
					selectionY = selectionY + 20;
				}
				// triggers
				if (selectFilter.Equals("triggers")) { 
					ArrayList arrList = GetGameElementsByTypeAndSub("trigger","*");
					for (int i=0; i<arrList.Count; i++) {
						GameElement gae = (GameElement)arrList [i];
						string text = "" + gae.name;
						if (gae.name.Equals("")) {
							// text = text + "("+gae.subtype+")";
						}
						GUIStyle guix = editorButtonStyleNotActive;
						if (editorSelected==gae) guix = editorButtonActiveStyle;
						bool buttonClicked = GUIScalerButton (new Rect ( selectionX, selectionY, selectionSize, 20), ""+text+" ("+gae.type+"/"+gae.subtype+")", guix);
						if (buttonClicked) {
							SetTool("EDIT");
							SetSelectedElement(gae);

						}
						selectionY = selectionY + 20;
						if (i>selectionMaxElements) break;
					} 
					selectionY = selectionY + 20;
				}
				// triggers
				if (selectFilter.Equals("challenges")) { 

					if (GUIScalerButton (new Rect ( selectionX, selectionY, 50, 20), "TIMED", editorButtonStyleNotActive)) {
						GameElement ge = AddGameElementAtName ("challenge","time", new Vector3(0.0f,0.0f,0.0f), "timechallenge" );
						ge.argument = "60";
						ge.argumentsub = "TIME LEFT";
						AddToEditorHistory();
					}
					selectionY = selectionY + 24;

					// challenges
					ArrayList arrList = GetGameElementsByTypeAndSub("challenge","*");
					for (int i=0; i<arrList.Count; i++) {
						GameElement gae = (GameElement)arrList [i];
						string text = "" + gae.name;
						if (gae.name.Equals("")) {
							// text = text + "("+gae.subtype+")";
						}
						GUIStyle guix = editorButtonStyleNotActive;
						if (editorSelected==gae) guix = editorButtonActiveStyle;
						bool buttonClicked = GUIScalerButton (new Rect ( selectionX, selectionY, selectionSize, 20), ""+text+" ("+gae.type+"/"+gae.subtype+")", guix);
						if (buttonClicked) {
							SetTool("EDIT");
							SetSelectedElement(gae);

						}
						selectionY = selectionY + 20;
						if (i>selectionMaxElements) break;
					} 
					selectionY = selectionY + 20;
				}
				// selection
				if (selectFilter.Equals("menu")) { 
					ArrayList arrList = GetGameElementsByTypeAndSub("guimenu","*");
					// AddGameElementAtName ("meta","title", new Vector3(0.0f,0.0f,0.0f), "TITLE" );
					if (GUIScalerButton (new Rect ( selectionX, selectionY, 42, 20), "SLOGO", editorButtonStyleNotActive)) {
						GameElement ge = AddGameElementAtName ("guimenu","splashlogo", new Vector3(0.0f,0.0f,0.0f), "splashlogo" );
						AddToEditorHistory();
					}
					if (GUIScalerButton (new Rect ( selectionX+45, selectionY, 42, 20), "SCLAIM", editorButtonStyleNotActive)) {
						GameElement ge = AddGameElementAtName ("guimenu","splashtext", new Vector3(0.0f,0.0f,0.0f), "splashtext" );
						AddToEditorHistory();
					}
					if (GUIScalerButton (new Rect ( selectionX+45+45, selectionY, 42, 20), "LOGO", editorButtonStyleNotActive)) {
						GameElement ge = AddGameElementAtName ("guimenu","biglogo", new Vector3(0.0f,0.0f,0.0f), "logo" );
						AddToEditorHistory();
					}

					if (GUIScalerButton (new Rect ( selectionX+45+45+45, selectionY, 42, 20), "MENU", editorButtonStyleNotActive)) {
						GameElement ge = AddGameElementAtName ("guimenu","menu", new Vector3(0.0f,0.0f,0.0f), "menu" );
						ge.argument = "@intro,@play,@setting,@webeditor,@exit";
						ge.argumentsub = "0,2,4,http://www.swissmercenariesgame.com,exit";
						AddToEditorHistory();
					}
					if (GUIScalerButton (new Rect ( selectionX+45+45+45+45, selectionY, 42, 20), "TEXT", editorButtonStyleNotActive)) {
						GameElement ge = AddGameElementAtName ("guimenu","text", new Vector3(0.0f,0.0f,0.0f), "text" );
						ge.argument = "@text";
						AddToEditorHistory();
					}
					selectionY = selectionY + 24;
					// list
					for (int i=0; i<arrList.Count; i++) {
						GameElement gae = (GameElement)arrList [i];
						string text = "" + gae.name;
						if (gae.name.Equals("")) {
							// text = text + "("+gae.subtype+")";
						}
						GUIStyle guix = editorButtonStyleNotActive;
						if (editorSelected==gae) guix = editorButtonActiveStyle;
						bool buttonClicked = GUIScalerButton (new Rect ( selectionX, selectionY, selectionSize, 20), ""+text+" ("+gae.type+"/"+gae.subtype+")", guix);
						if (buttonClicked) {
							SetTool("EDIT");
							SetSelectedElement(gae);

						}
						selectionY = selectionY + 20;
						if (i>selectionMaxElements) break;
					} 
					selectionY = selectionY + 20;
				}
				// timed?
				if (selectFilter.Equals("timed")) { 
					// adds
					// AddGameElementAtName ("meta","title", new Vector3(0.0f,0.0f,0.0f), "TITLE" );
					if (GUIScalerButton (new Rect ( selectionX, selectionY, 30, 20), "CAP", editorButtonStyleNotActive)) {
						GameElement ge = AddGameElementAtName ("camcut","caption", new Vector3(0.0f,0.0f,0.0f), "capx" );
						ge.timed = 5.0f;
						AddToEditorHistory();
					}
					if (GUIScalerButton (new Rect ( selectionX+34, selectionY, 30, 20), "PIC", editorButtonStyleNotActive)) {
						GameElement ge = AddGameElementAtName ("camcut","timedpicture", new Vector3(0.0f,0.0f,0.0f), "timedpicture" );
						ge.timed = 5.0f;
						AddToEditorHistory();
					}
					if (GUIScalerButton (new Rect ( selectionX+68, selectionY, 30, 20), "NOT", editorButtonStyleNotActive)) {
						GameElement ge = AddGameElementAtName ("camcut","timednotification", new Vector3(0.0f,0.0f,0.0f), "timednotification" );
						ge.timed = 5.0f;
						AddToEditorHistory();
					}
					if (GUIScalerButton (new Rect ( selectionX+102, selectionY, 30, 20), "LOP", editorButtonStyleNotActive)) {
						GameElement ge = AddGameElementAtName ("camcut","timedloop", new Vector3(0.0f,0.0f,0.0f), "timedloop" );
						ge.timed = 5.0f;
						AddToEditorHistory();
					}
					if (GUIScalerButton (new Rect ( selectionX+136, selectionY, 30, 20), "EXI", editorButtonStyleNotActive)) {
						GameElement ge = AddGameElementAtName ("camcut","timedexit", new Vector3(0.0f,0.0f,0.0f), "timedexit" );
						ge.timed = 5.0f;
						AddToEditorHistory();
					}
					selectionY = selectionY + 24;

					// list it
					ArrayList arrList = GetAllGameElementsByTypeAndSubTimeSorted();
					for (int i=0; i<arrList.Count; i++) {
						GameElement gae = (GameElement)arrList [i];
						string text = "" + gae.name;
						if (gae.name.Equals("")) {
							// text = text + "("+gae.subtype+")";
						}
						GUIStyle guix = editorButtonStyleNotActive;
						if (editorSelected==gae) guix = editorButtonActiveStyle;
						string timey = "";
						timey = "["+gae.timed+"]";
						string textzz = ""+gae.argument;
						if (textzz.Length>5) textzz = textzz.Substring(0,5);
						bool buttonClicked = GUIScalerButton (new Rect ( selectionX, selectionY, selectionSize, 20), timey+" ("+gae.subtype+") "+text+" "+textzz+" ", guix);
						if (buttonClicked) {
							SetTool("EDIT");
							SetSelectedElement(gae);

						}
						selectionY = selectionY + 20;
						if (i>selectionMaxElements) break;
					} 
					selectionY = selectionY + 20;
				}
				// selection
				if (selectFilter.Equals("actions")) { 
					ArrayList arrList = GetGameElementsByTypeAndSub("action","*");
					for (int i=0; i<arrList.Count; i++) {
						GameElement gae = (GameElement)arrList [i];
						string text = "" + gae.name;
						if (gae.name.Equals("")) {
							// text = text + "("+gae.subtype+")";
						}
						GUIStyle guix = editorButtonStyleNotActive;
						if (editorSelected==gae) guix = editorButtonActiveStyle;
						bool buttonClicked = GUIScalerButton (new Rect ( selectionX, selectionY, selectionSize, 20), ""+text+" ("+gae.type+"/"+gae.subtype+")", guix);
						if (buttonClicked) {
							SetTool("EDIT");
							SetSelectedElement(gae);
						}
						selectionY = selectionY + 20;
						if (i>selectionMaxElements) break;
					} 
					selectionY = selectionY + 20;
				}
				if (selectFilter.Equals("language")) { 
					// languages ...
					for (int i=0;i<arrLanguages.Length;i++) {
						GUIStyle guixx = editorButtonStyleNotActive;
						if (ingameLanguage.Equals(arrLanguages[i])) guixx = editorButtonActiveStyle;
						if (GUIScalerButton (new Rect ( selectionX + i*34, selectionY, 30, 20), ""+arrLanguages[i], guixx)) {
							// ingameLanguage = arrLanguages[i]	;
							SetInGameLanguage( arrLanguages[i], false );
						}
					}
					selectionY = selectionY + 24;
					// add keys ...
					// ingameNewLanguageKey
					ingameNewLanguageKey = GUIScalerTextField (new Rect ( selectionX, selectionY, 80, 20), ingameNewLanguageKey, editorButtonStyleNotActive);
					ingameNewLanguageText = GUIScalerTextField (new Rect ( selectionX+84, selectionY, 80, 20), ingameNewLanguageText, editorButtonStyleNotActive);
					if (GUIScalerButton (new Rect ( selectionX+170, selectionY, 40, 20), "+KEY", editorButtonActiveStyle)) {
						GameElement ge = AddGameElementAtName ("language","item", new Vector3(0.0f,0.0f,0.0f), "" );
						ge.name = ""+ingameNewLanguageKey;
						ge.argument = "en";
						ge.argumentsub = ""+ingameNewLanguageText;
						AddToEditorHistory();
					}
					selectionY = selectionY + 24;

					ArrayList arrList = GetGameElementsByTypeAndSubSearch("language","item","en","*"); // and searchquery!!
					for (int i=0; i<arrList.Count; i++) {
						GameElement gae = (GameElement)arrList [i];
						string text = "" + gae.name;
						if (gae.name.Equals("")) {
							// text = text + "("+gae.subtype+")";
						}
						GUIStyle guix = editorButtonStyleNotActive;
						if (editorSelected==gae) guix = editorButtonActiveStyle;
						bool buttonClicked = GUIScalerButton (new Rect ( selectionX, selectionY, selectionSize/2, 20), ""+gae.name+" "+gae.argumentsub, guix);
						if (buttonClicked) {
							SetTool("EDIT");
							SetSelectedElement(gae);
						}
						// is there a related object?
						GameElement sg = GetGameElementLanguage( gae.name, ingameLanguage );
						if (sg!=null) {
							// add the other ...
							// SetTool("EDIT");
							// SetSelectedElement(gae);
							if (GUIScalerButton (new Rect ( selectionX + selectionSize/2, selectionY, selectionSize/2, 20), ""+sg.argumentsub, guix)) {
								SetTool("EDIT");
								SetSelectedElement(sg);
							}
						}
						// selected ...
						if (sg==null) {
							if (gae==editorSelected) {
								if (GUIScalerButton (new Rect ( selectionX + selectionSize/2+4, selectionY, 40, 20), "ADD", guix)) {
									GameElement ge = AddGameElementAtName ("language","item", new Vector3(0.0f,0.0f,0.0f), "" );
									ge.name = ""+gae.name;
									ge.argument = ""+ingameLanguage;
									ge.argumentsub = "-"+gae.argumentsub+"-";
									AddToEditorHistory();
									SetTool("EDIT");
									SetSelectedElement(ge);
								}
							}
						}


						selectionY = selectionY + 20;
						if (i>selectionMaxElements) break;
					} 
					selectionY = selectionY + 20;
				}



				// nearbyRect
				GUIScalerLabel ( new Rect(nearbyRect.x,nearbyRect.y,nearbyRect.width,nearbyRect.height), "", editorBackground);
				int nearbyXTmp = nearbyX;
				int nearbyYTmp = nearbyY;
				// nearbyWidth
				// nearbyHeight
				// check for nearby ..
				// is there a selected one?NAME
				if (editorSelected!=null) {
					// check

					GameElement xby;
					ArrayList arrxx = new ArrayList();
					int county = 0;
					for (int i=0;i<arrLevel.Count;i++) {
						xby = (GameElement)arrLevel[i];
						float sizewidth = 1.0f;
						float middlex = editorSelected.position.x;
						float middlez = editorSelected.position.z;
						if ((middlex>(xby.position.x-sizewidth)) && (middlex<(xby.position.x+sizewidth))) {
							if ((middlez>(xby.position.z-sizewidth)) && (middlez<(xby.position.z+sizewidth))) {
								arrxx.Add(xby);
								county++;
							}
						}
					}

					// sort?

					// show
					if (county>1) {
						// GUIScalerButton (new Rect ( nearbyXTmp, nearbyYTmp, 140, 20), "NEAR BY ", editorButtonActiveStyle);
						//nearbyYTmp = nearbyYTmp + 20;
						int counterx = 0;
						for (int i=0;i<arrxx.Count; i++) {
							GameElement gae = (GameElement)arrxx [i];
							string text = "" + gae.name;
							if (gae.name.Equals("")) {
								text = text + "("+gae.subtype+")";
							}
							GUIStyle guix = editorButtonStyleNotActive;
							if (editorSelected==gae) guix = editorButtonActiveStyle;
							bool buttonClicked = GUIScalerButton (new Rect ( nearbyXTmp, nearbyYTmp, 120, 20), ""+text+"", guix);
							if (buttonClicked) {
								SetSelectedElement(gae);
							}
							nearbyYTmp = nearbyYTmp + 20;
							counterx++;
							if (counterx>5) break;
						}
					}
				}
				nearbyRect.x = nearbyX;
				nearbyRect.y = nearbyY;
				nearbyRect.width = 120;
				nearbyRect.height = nearbyYTmp - nearbyY;

				// arrEditorHistory
				if (false) {
					ArrayList arr = GetActualEditorHistory();
					selectionX = 0;
					// GUIScalerButton (new Rect ( selectionX+100, selectionY, 240, 20), ""+arr.Count);
					if (arr.Count>0)
						selectionY = (int)(GUIScalerScreenWidth()*0.3f);
					int counter = 0;
					for (int i=(arr.Count-1);i>=0; i--) {
						LevelHistory gae = (LevelHistory)arr [i];
						string text = "[" + gae.level+"] ("+gae.message+") " + gae.arrLevel.Count;
						GUIStyle guix = editorButtonStyleNotActive;
						// if (editorSelected==gae) guix = editorButtonActiveStyle;
						bool buttonClicked = GUIScalerButton (new Rect ( selectionX, selectionY, 240, 20), ""+text+"", guix);
						if (buttonClicked) {
							// SetSelectedElement(gae);
							UndoTo(gae);
						}
						selectionY = selectionY + 20;
						counter++;
						if (counter>5) break;
					}
				}

				selectionDialogeVisual.height = selectionY - selectionDialogeVisual.y;

			}


			// editor
			// float scrollToShow=((int)(scroll*10.0f))/10.0f;
			// GUIScalerLabel (new Rect(0,GUIScalerScreenHeight()-20,60,20),"  ["+scrollToShow+"] ",editorComment);

			// editorCursorActualPoint
			Vector3 px = new Vector3();
			px.x = (float)Math.Floor(editorCursorActualPoint.x*10.0f)/10.0f;
			px.y = (float)Math.Floor(editorCursorActualPoint.y*10.0f)/10.0f;
			px.z = (float)Math.Floor(editorCursorActualPoint.z*10.0f)/10.0f;

			GUIScalerLabel (new Rect(0,GUIScalerScreenHeight()-20,100,20),""+px.x+"/"+px.y+"/"+px.z,editorComment);

			// INFO
			string strSelection="";
			if (editorSelected!=null) {
				strSelection = "[SELECTED ELEMENT: "+editorSelected.type+"/"+editorSelected.subtype+" "+editorSelected.guiDescription+"]";
			}



			//			GUIScalerLabel (filterTypeSubVisual, "", editorBackground);

			// filterDev
			int filterY = GUIScalerScreenHeight()-80;
			int filterX = 0;

			// NAMES
			filterX = filterX + 0;
			GUIStyle activestylext = editorButtonStyle;
			if (cameraOverlayTypes)  activestylext = editorButtonActiveStyle;
			if (GUIScalerButton (new Rect (filterX,filterY-24, 50, 20), "NAMES ", activestylext )) {
				cameraOverlayTypes=!cameraOverlayTypes;
			}
			filterX = filterX + 60;
			if (GUIScalerButton (new Rect(62,filterY-24,140,20),"NOTIFICATIONS" ,editorComment)) {
				notificationDialog = !notificationDialog;
			}

			filterX = 0;

			GUIStyle xyz = editorButtonStyleNotActive;
			if (filterType.Equals("*"))  xyz = editorButtonActiveStyle;
			if (GUIScalerButton (new Rect(0,filterY,60,20),"V.ALL" ,xyz)) {
				filterType = "*";
			}

			filterX = filterX +70;
			// type: *
			// if (filterType.Equals("*")) {
			ArrayList arrTypesUniqueX = GetElementTypesUnique ();
			for (int i=0; i<arrTypesUniqueX.Count; i++) {
				GameElement unique = (GameElement)arrTypesUniqueX [i];
				string text = "" + unique.type;
				GUIStyle guix = editorButtonStyleNotActive;
				if (unique.type.Equals(filterType)) {
					guix = editorButtonActiveStyle;
				}
				int acc = CountElementsType(unique.type);
				string addon = "";
				if (acc>0) addon = ""+acc+"/";
				bool buttonClicked = GUIScalerButton (new Rect ( filterX + i * 50, filterY, 46, 20), addon+""+text+"", guix);
				if (buttonClicked) {
					filterType = unique.type;
					filterTypeSub = "*";
					selectFilter = "selection";
				}
				// delete objects
				// CountElementsType( string elementArea, string elementSubArea )
			}
			filterX = filterX + arrTypesUniqueX.Count*48;
			// }



			filterTypeVisual.x = 0;
			filterTypeVisual.width = filterX;
			filterTypeVisual.y = filterY;
			filterTypeVisual.height = 22;


			// type!=* > selected
			if (!filterType.Equals("*")) {

				filterY = filterY +22;
				filterX = 70;

				int offset = 0;
				ArrayList arr = GetElementTypes (filterType);
				for (int a=0; a<arr.Count; a++) {
					GameElement gelement = (GameElement)arr [a];
					string text = "" + gelement.subtype;
					GUIStyle guix = editorButtonStyleNotActive;
					if (filterTypeSub.Equals(gelement.subtype)) guix = editorButtonActiveStyle;
					bool buttonClicked = GUIScalerButton (new Rect ( filterX + offset + a * 62, filterY, 58, 20), ""+text, guix);
					if (buttonClicked) {
						filterTypeSub = gelement.subtype;
						selectFilter = "selection";
					}
				}	

				filterX = 80 + arr.Count * 60;

				filterTypeSubVisual.x = 0;
				filterTypeSubVisual.width = filterX;
				filterTypeSubVisual.y = filterY;
				filterTypeSubVisual.height = 22;

			}



			// the history with all objects ...


			// MODE
			// 	
			/*
			 * string editorArea = "item";
			string editorSubArea = "apple";
			*/
			if (editorTool.Equals ("CREATE")) {
				// GameElement ge = GetElementType( editorArea,  editorSubArea );
				if (editorPrefab!=null) {
					strSelection = "[SELECTED ELEMENT: "+editorPrefab.type+"/"+editorPrefab.subtype+" "+editorPrefab.guiDescription+"]";
				}
			}

			GUIScalerLabel (new Rect(101,GUIScalerScreenHeight()-20,GUIScalerScreenWidth(),20),"CAMERA: Use <awsd> for moving <qe> rotate side <rf> up/down <x2> lookup/down <c> reset  OBJECT: + <shift> | Objs: ["+arrLevel.Count+"] Elements "+ strSelection ,editorComment);


		}

		if (debugGameElementTypes) {
			string text="";
			for (int i=0; i<arrGameElementTypes.Count; i++) {
				GameElement gaelement = (GameElement)arrGameElementTypes [i];
				string gameobjectname="";
				// if (gaelement.gameObject!=null) gameobjectname=""+gaelement.gameObject.name;
				text=text+"\n"+i+". ("+gaelement.type+"/"+gaelement.subtype+") ["+gameobjectname+"] [s:"+gaelement.strength+"] ";
			}
			GUIScalerLabel (new Rect(0,200,GUIScalerScreenWidth(),GUIScalerScreenHeight()),""+text,editorComment);


		}

		// debug it 
		if (debugGameElements) {

			string text="";
			for (int i=0; i<arrLevel.Count; i++) {
				GameElement gaelement = (GameElement)arrLevel [i];
				string gameobjectname="";
				if (gaelement.gameObject!=null) gameobjectname=""+gaelement.gameObject.name;
				text=text+"\n"+i+". ("+gaelement.type+"/"+gaelement.subtype+") ["+gameobjectname+"] [s:"+gaelement.strength+"] ";
			}
			GUIScalerLabel (new Rect(400,200,GUIScalerScreenWidth(),GUIScalerScreenHeight()),""+text); //  editorComment

		}



	}

	bool enabledEditorEdges =true;

	void Update() {

		// Debug.Log("LeveleEditor.Update() // "+editorTool);

		// editor
		if (gameLogic !=null &&  gameLogic.modal==GameLogic.GameLogicModal.Editor) {

			// Debug.Log("LeveleEditor.Update() // "+editorTool+" // INEDITOR ");

			// handle mouse down and up on cursors ... 
			if (true) { // }Input.GetMouseButton(1)) {
				SensButton sb;
				float mouseXT=Input.mousePosition.x;
				float mouseYT=(Screen.height-Input.mousePosition.y);
				for (int i=0;i<arrSensButtons.Count;i++) {
					sb = (SensButton) arrSensButtons[i];
					if ((mouseXT>sb.rect.x*scaleX)&&(mouseXT<(sb.rect.x+sb.rect.width)*scaleX)) {
						if ((mouseYT>sb.rect.y*scaleY)&&(mouseYT<(sb.rect.y+sb.rect.height)*scaleY)) {
							// Debug.Log("LevelEditor.OnGUI() // sens mouse handler");
							if (Input.GetMouseButton(0)) { 
								DoSensButton( sb, "down" );
								// Debug.Log("LevelEditor.OnGUI() // sens mouse handler down");
							}
							if (Input.GetMouseButtonUp(0)) { 
								DoSensButton( sb, "up" );
								// Debug.Log("LevelEditor.OnGUI() // sens mouse handler up");
							}
						}
					}
				}
			}


			/*
			if (Input.GetMouseButtonDown(0)) {

			}
			if (Input.GetMouseButtonUp(0)) {
				// Debug.Log ("GetMouseButtonUp()");
			}
			*/

			// special move objects
			if ((Input.GetKey ("left shift"))||(Input.GetKey ("right shift"))) {

				// Debug.Log("LeveleEditor.Update() // "+editorTool+" // INEDITOR // SHIFT EDITOR ");

				if (editorSelected!=null) {

					if ((Input.GetKeyUp ("a"))||(Input.GetKeyUp ("left"))) {
						AddToEditorHistory("[GUI][OBJECT][X]--");
					}
					if ((Input.GetKeyUp ("d"))||(Input.GetKeyUp ("right"))) {
						AddToEditorHistory("[GUI][OBJECT][X]++");
					}
					if ((Input.GetKeyUp ("w"))||(Input.GetKeyUp ("up"))) {
						AddToEditorHistory("[GUI][OBJECT][Z]++");
					}
					if ((Input.GetKeyUp ("s"))||(Input.GetKeyUp ("down"))) {
						AddToEditorHistory("[GUI][OBJECT][Z]--");
					}

					if ((Input.GetKeyUp ("r"))) {
						AddToEditorHistory("[GUI][OBJECT][Y]++");
					}
					if ((Input.GetKeyUp ("f"))) {
						AddToEditorHistory("[GUI][OBJECT][Y]--");
					}

					// up & down 
					if ((Input.GetKeyUp ("q"))) {
						AddToEditorHistory("[GUI][OBJECT][RX]--");
					}
					if ((Input.GetKeyUp ("e"))) {
						AddToEditorHistory("[GUI][OBJECT][RX]++");
					}

					// rotate
					if ((Input.GetKeyUp ("2"))) {
						AddToEditorHistory("[GUI][OBJECT][RX]--");
					}
					if ((Input.GetKeyUp ("3"))) {
						AddToEditorHistory("[GUI][OBJECT][RX]--");
					}
					if ((Input.GetKeyUp ("x"))) {
						AddToEditorHistory("[GUI][OBJECT][RX]++");
					}
					if ((Input.GetKeyUp ("y"))) {
						AddToEditorHistory("[GUI][OBJECT][RX]++");
					}
				}

				// rot
			}

		
			// todo: with a array or case
			/*
			if (Input.GetKeyDown ("w")) {
				Debug.Log ("UP");
				if (editorArea.Equals ("background")) {
					SetEditorArea("control");
				} else
				if (editorArea.Equals ("playfield")) {
					SetEditorArea("background");
				} else
				if (editorArea.Equals ("player")) {
						SetEditorArea("playfield");
				} else
				if (editorArea.Equals ("extra")) {
							SetEditorArea("player");
				} else
				if (editorArea.Equals ("enemy")) {
								SetEditorArea("extra");
				} else
				if (editorArea.Equals ("control")) {
								SetEditorArea("enemy");
				} 
			}
			if (Input.GetKeyDown ("s")) {
				Debug.Log ("DOWN");
			}
			*/

			// vertical slides
			// delete & return
			/*
			if (Input.GetKeyDown ("return")) {
				Debug.Log ("RETURN");
				// InsertVerticalLine();
				// GUI.SetNextControlName("");
			}
			if (Input.GetKeyDown ("backspace")) {
				// Debug.Log ("DELETE");
				// RemoveVerticalLine();
			}
			*/

			if (Input.GetKeyDown ("backspace")) {
				// Debug.Log ("DELETE");
				// RemoveVerticalLine();
				if (editorSelected!=null) {
					// RemoveElement(editorSelected);
					// editorSelected = null;
					RemoveSelectedElement();
					// add to editor history
					AddToEditorHistory("[GUI][OBJECT][DELETE]");
				}
			}


			// generate new objects
			float mouseX=Input.mousePosition.x;
			float mouseY=Screen.height-Input.mousePosition.y;



			// IN SCENE
			if (!CheckMouseInEditor()) {

				// Debug.Log("LeveleEditor.Update() // "+editorTool+" // INEDITOR // INSCENE ");


				// SPLIT!
				if (Input.GetMouseButtonDown(0)) {


					// Debug.Log ("GetMouseButtonDown()");
					if (editorTool.Equals ("SPLIT")) {
						if (editorToolSub.Equals ("right")) {
							InsertVerticalLine();
							AddToEditorHistory("[GUI][SPLIT]RIGHT");

						}
						if (editorToolSub.Equals ("up")) {
							RemoveHorizontalLine();
							AddToEditorHistory("[GUI][SPLIT]UP");
						}
						if (editorToolSub.Equals ("left")) {
							RemoveVerticalLine();
							AddToEditorHistory("[GUI][SPLIT]LEFT");
						}
						if (editorToolSub.Equals ("down")) {
							InsertHorizontalLine();
							AddToEditorHistory("[GUI][SPLIT]DOWN");
						}

					}
				}



				// not? create
				if (true) {



					// create something here!!!
					// if (!CheckMouseInEditor()) < coming from above
					if (editorTool.Equals ("CREATE")) {
						/*
						if (Input.GetMouseButtonDown(0)) { // is done in ongui in HandleMouseDownToCreate() to ignore ongui-clicks
							//HandleMouseDownToCreate();
						}
						*/

					} // CREATE

				}

			}


		} // /editor
	}

	// editor change?
	int screenWidth = Screen.width;
	int screenHeightChange = Screen.height;

	// Update is called once per frame
	void FixedUpdate () {

		/*
		 * ScreenChange
		 * 
		 * */
		if ((screenWidth!=Screen.width)||(screenHeightChange!=Screen.height)) {
			ScalerSetUp();
			// cursorsetup
		}

		/*
		 *  NOTIFICATION CENTER
		 * 
		 * */
		if (Input.GetKeyDown("p")) {
			// Debug.Log("LevelEditor.OnGUI() // notificationDialog: "+notificationDialog);
			notificationDialog = !notificationDialog;
		}


		// check for directlight
		// no > activate default 
		// ...
		// DefaultDirectionalLight
		// check if there is a 
		GameObject defaultLight = GameObject.Find("DefaultDirectionalLight");
		if (defaultLight!=null) { 
			// Debug.Log("CountElementsTypeIndexOf(): "+CountElementsTypeIndexOf("directlight"));
			if (CountElementsTypeIndexOf("directlight")) {
				// activate it
				defaultLight.GetComponent<Light>().enabled = false;
			} else {
				defaultLight.GetComponent<Light>().enabled = true;
			}
		}

		// ingame
		if (gameLogic != null && gameLogic.modal == GameLogic.GameLogicModal.Running) {
			UpdateEvaluationTimed ();
		}

		// editor
		if (gameLogic !=null && gameLogic.modal==GameLogic.GameLogicModal.Editor) {

			// rotate preview
			GameObject preview = GameObject.Find("editorpreview");
			preview.transform.Rotate(0.0f, 1.0f, 0.0f);


			// edges
			// generate new objects
			float mouseXX=Input.mousePosition.x;
			float mouseYY=Screen.height-Input.mousePosition.y;

			// hot edges
			if ((mouseXX<20)||(mouseXX>(Screen.width-20*scaleX))||(mouseYY<20*scaleY)||(mouseYY>(Screen.height-20*scaleY))) {

				// Debug.Log("LeveleEditor.Update() // "+editorTool+" // INEDITOR // HOT EDGES");

				GameObject xcontainer=GameObject.Find ("editorCameraContainer");
				GameObject xeditorcamera=GameObject.Find ("editorcamera");

				if (enabledEditorEdges) 
				if (!CheckMouseInEditor())
				{


					float rotatex = 1.0f;
					float rotatey = 1.0f;
					if ((mouseYY>=0)&&(mouseYY<20*scaleY)) {
						xeditorcamera.transform.Rotate( -rotatey, 0.0f, 0.0f );
					}
					if ((mouseYY>(Screen.height-20*scaleY))&&(mouseYY<=(Screen.height))) {
						xeditorcamera.transform.Rotate( +rotatey, 0.0f,  0.0f );
					}

					if ((mouseXX>=0)&&(mouseXX<20*scaleX)) {
						xcontainer.transform.Rotate( 0.0f, -rotatex, 0.0f );
					}
					if ((mouseXX<=(Screen.width))&&(mouseXX>(Screen.width-20*scaleX))) {
						xcontainer.transform.Rotate( 0.0f, rotatex, 0.0f );
					}
					/*
					if (menu==4) {
						xcontainer.transform.Translate( 0.0f, 0.0f , 0.1f);
					}
					if (menu==5) {
						xcontainer.transform.Translate( 0.0f,   0.0f, -0.1f );
					}
					if (menu==6) {
						xcontainer.transform.Translate( 0.0f, 0.1f, 0.0f );
					}
					if (menu==7) {
						xcontainer.transform.Translate( 0.0f,  -0.1f,   0.0f );
					}
					*/

				}

			}


			// no physics
			// Time.timeScale = 0.0f; // move etc is not working too!
			// CAMERA direct input

			Vector3 mouseVec = Input.mousePosition;
			float mouseX = mouseVec.x;
			float mouseY = GUIScalerScreenHeight()-mouseVec.y;
			//			print("---"+mouseVec.x+"/"+mouseVec.y);

			if (mouseY>directMouseInputY) {
				if (mouseY<(directMouseInputY+20)) {
					if (mouseX>directMouseInputX) {
						if (mouseX<(directMouseInputX+8*20)) {
							// print("Do IT!");
							float def = mouseX - directMouseInputX;
							int menu = (int) (def / 20.0f);
							// print("MENU"+menu);

							GameObject xcontainer=GameObject.Find ("editorCameraContainer");
							GameObject xeditorcamera=GameObject.Find ("editorcamera");

							float rotatex = 0.5f;
							float rotatey = 0.3f;
							if (menu==0) {
								xeditorcamera.transform.Rotate( -rotatey, 0.0f, 0.0f );
							}
							if (menu==1) {
								xeditorcamera.transform.Rotate( +rotatey, 0.0f,  0.0f );
							}
							if (menu==2) {
								xcontainer.transform.Rotate( 0.0f, -rotatex, 0.0f );
							}
							if (menu==3) {
								xcontainer.transform.Rotate( 0.0f, rotatex, 0.0f );
							}
							if (menu==4) {
								xcontainer.transform.Translate( 0.0f, 0.0f , 0.1f);
							}
							if (menu==5) {
								xcontainer.transform.Translate( 0.0f,   0.0f, -0.1f );
							}
							if (menu==6) {
								xcontainer.transform.Translate( 0.0f, 0.1f, 0.0f );
							}
							if (menu==7) {
								xcontainer.transform.Translate( 0.0f,  -0.1f,   0.0f );
							}

						}
					}
				}
			}

			// mouse pointer
			RaycastHit hit;
			Camera cam  = Camera.main;
			cam = GameObject.Find ("editorcamera").GetComponent<Camera>();

			Ray ray = cam.ScreenPointToRay(Input.mousePosition);   
			if (Physics.Raycast(ray,out hit)) {

				editorCursorActualPoint = new Vector3( hit.point.x, hit.point.y, hit.point.z );

				float raster = GetRaster ();
				if (raster!= 0.0f) {
					float offsetX=0.25f;
					float offsetY=0.25f;
					editorCursorActualPoint.x=(Mathf.Floor((editorCursorActualPoint.x+offsetX)/raster))*raster;
					editorCursorActualPoint.z=(Mathf.Floor((editorCursorActualPoint.z+offsetY)/raster))*raster;
				}


				Debug.DrawLine(ray.origin, hit.point);
				// version 1.0f
				// GameObject.Find ("mousepointer").transform.position = new Vector3( hit.point.x, hit.point.y, hit.point.z );
				// version 2.0
				GameObject.Find ("mousepointer").transform.position = new Vector3( editorCursorActualPoint.x, editorCursorActualPoint.y, editorCursorActualPoint.z );

				GameObject.Find ("mousepointerraster").transform.position = new Vector3( hit.point.x, hit.point.y, hit.point.z );

				// show only if one splitter is active!!
				GameObject.Find ("mousesplitvertical").transform.position = new Vector3( hit.point.x, hit.point.y, hit.point.z );
				GameObject.Find ("mousesplithorizontal").transform.position = new Vector3( hit.point.x, hit.point.y, hit.point.z );

				if (editorTool.Equals ("SPLIT")) {
					GameObject.Find ("mousesplitvertical").GetComponent<Renderer>().enabled = false;
					GameObject.Find ("mousesplithorizontal").GetComponent<Renderer>().enabled = false;

					if (editorToolSub.Equals ("right")) {
						GameObject.Find ("mousesplitvertical").GetComponent<Renderer>().enabled = true;
					} 
					if (editorToolSub.Equals ("up")) {
						GameObject.Find ("mousesplithorizontal").GetComponent<Renderer>().enabled = true;
					}
					if (editorToolSub.Equals ("left")) {
						GameObject.Find ("mousesplitvertical").GetComponent<Renderer>().enabled = true;
					}
					if (editorToolSub.Equals ("down")) {
						GameObject.Find ("mousesplithorizontal").GetComponent<Renderer>().enabled = true;
					}

				} else {

					GameObject.Find ("mousesplitvertical").GetComponent<Renderer>().enabled = false;
					GameObject.Find ("mousesplithorizontal").GetComponent<Renderer>().enabled = false;

				}
			}

			//bool updateCamera = false;

			// speed
			Vector3 speed = new Vector3();
			speed.x=0.2f;
			speed.y=0.2f;
			speed.z=0.2f;

			// sorry really dirty programming
			// Vector3 matrixSpeed = new 
			GameObject container=GameObject.Find ("editorCameraContainer");
			GameObject editorcamera=GameObject.Find ("editorcamera");

			// Debug.Log ("...."+container.transform.localRotation.y);

			if (gameLogic !=null && gameLogic.modal==GameLogic.GameLogicModal.Editor) {

				// scroll to zoom
				container.transform.position += editorcamera.transform.forward*Input.GetAxis("Mouse ScrollWheel");

				// keys
				// float xspeed = 0.6f;
				// float xspeedFactor = 3.0f;
				// float xspeedEffective = 1.0f;

				// default: move camera
				if (!(Input.GetKey ("left shift"))&&(!Input.GetKey ("right shift"))) {

					if ((Input.GetKey ("a"))||(Input.GetKey ("left"))) {
						// scroll = scroll - 0.3f;
						// DoScroll(-0.3f,0.0f);
						//						DoEditorScroll( -speed.x, 0.0f, 0.0f );

						container.transform.Translate ( new Vector3(-speedCamera, 0.0f, 0.0f));

					}
					if ((Input.GetKey ("d"))||(Input.GetKey ("right"))) {
						// scroll = scroll + 0.3f;
						//						DoEditorScroll( speed.x, 0.0f, 0.0f );

						container.transform.Translate ( new Vector3(speedCamera, 0.0f, 0.0f));
					}

					// forward backward
					if ((Input.GetKey ("w"))||(Input.GetKey ("up"))) {
						// scroll = scroll + 0.3f;
						// DoScroll( 0.0f,0.3f);
						// DoEditorScroll( 0.0f, 0.0f, speed.z );
						container.transform.Translate ( new Vector3(0.0f, 0.0f, speedCamera));

					}
					if ((Input.GetKey ("s"))||(Input.GetKey ("down"))) {
						// scroll = scroll + 0.3f;
						//					"q"	DoEditorScroll( 0.0f, 0.0f, -speed.z );
						container.transform.Translate ( new Vector3(0.0f, 0.0f, -speedCamera));

					}
					if ((Input.GetKey ("2"))||(Input.GetKey ("3"))||(Input.GetKey ("y"))) {
						// scroll = scroll + 0.3f;
						//						DoEditorScroll( 0.0f, 0.0f, -speed.z );
						editorcamera.transform.Rotate ( new Vector3(-3.0f, 0.0f, 0.0f));
					}
					if ((Input.GetKey ("x"))) {
						// scroll = scroll + 0.3f;
						//						DoEditorScroll( 0.0f, 0.0f, -speed.z );
						editorcamera.transform.Rotate ( new Vector3(3.0f, 0.0f, 0.0f));
					}

					// up & down 
					if ((Input.GetKey ("r"))) {
						// scroll = scroll + 0.3f;
						DoEditorScroll( 0.0f, speed.y, 0.0f );

					}
					if ((Input.GetKey ("f"))) {
						// scroll = scroll + 0.3f;
						DoEditorScroll( 0.0f, -speed.y, 0.0f );

					}

					// go through cameras
					if ((Input.GetKey ("q"))) {
						//GameObject containerx=GameObject.Find ("editorCameraContainer");
						// version 1.0
						//						container.transform.Rotate ( new Vector3(0.0f, 90.0f, 0.0f));
						container.transform.Rotate ( new Vector3(0.0f, -2.0f, 0.0f));
					}
					if ((Input.GetKey ("e"))) {
						//GameObject containerx=GameObject.Find ("editorCameraContainer");
						//						container.transform.Rotate ( new Vector3(0.0f, 90.0f, 0.0f));
						container.transform.Rotate ( new Vector3(0.0f,  2.0f, 0.0f));
					}

					if ((Input.GetKey ("c"))) {
						// reset 
						//GameObject containerx=GameObject.Find ("editorCameraContainer");
						//						container.transform.Rotate ( new Vector3(0.0f, 90.0f, 0.0f));
						//						container.transform.Rotate ( new Vector3(0.0f,  2.0f, 0.0f));
						ResetRotation();
					}

				}

				// special move objects
				if ((Input.GetKey ("left shift"))||(Input.GetKey ("right shift"))) {

					if (editorSelected!=null) {

						Vector3 vectorMove = new Vector3();
						vectorMove.x=speedObject;
						vectorMove.y=speedObject;
						vectorMove.z=speedObject;

						if ((Input.GetKey ("a"))||(Input.GetKey ("left"))) {
							// editorSelected.position.x = editorSelected.position.x - vectorMove.x;
							MoveObjectAlongEditorCamera("left");
							UpdateElementVisual(editorSelected);
						}
						if ((Input.GetKey ("d"))||(Input.GetKey ("right"))) {
							// editorSelected.position.x = editorSelected.position.x + vectorMove.x;
							MoveObjectAlongEditorCamera("right");
							UpdateElementVisual(editorSelected);
						}
						if ((Input.GetKey ("w"))||(Input.GetKey ("up"))) {
							// editorSelected.position.z = editorSelected.position.z + vectorMove.z;
							MoveObjectAlongEditorCamera("forward");
							UpdateElementVisual(editorSelected);
						}
						if ((Input.GetKey ("s"))||(Input.GetKey ("down"))) {
							// editorSelected.position.z = editorSelected.position.z - vectorMove.z;
							MoveObjectAlongEditorCamera("backward");
							UpdateElementVisual(editorSelected);
						}

						if ((Input.GetKey ("r"))) {
							// editorSelected.position.y = editorSelected.position.y + vectorMove.y;
							MoveObjectAlongEditorCamera("up");
							UpdateElementVisual(editorSelected);
						}
						if ((Input.GetKey ("f"))) {
							// editorSelected.position.y = editorSelected.position.y - vectorMove.y;
							MoveObjectAlongEditorCamera("down");
							UpdateElementVisual(editorSelected);
						}

						// up & down 
						if ((Input.GetKey ("q"))) {
							// scroll = scroll + 0.3f;
							// editorSelected.rotation = editorSelected.rotation + 3.0f;
							MoveObjectAlongEditorCamera("rotateleft");
							UpdateElementVisual(editorSelected);
						}
						if ((Input.GetKey ("e"))) {
							//							editorSelected.rotation = editorSelected.rotation - 3.0f;
							MoveObjectAlongEditorCamera("rotateright");
							UpdateElementVisual(editorSelected);
						}

						if ((Input.GetKey ("2"))||(Input.GetKey ("3"))) {
							// editorSelected.position.z = editorSelected.position.z + vectorMove.z;
							MoveObjectAlongEditorCamera("rotateforward");
							UpdateElementVisual(editorSelected);
						}
						if ((Input.GetKey ("x"))||(Input.GetKey ("y"))) {
							// editorSelected.position.z = editorSelected.position.z + vectorMove.z;
							MoveObjectAlongEditorCamera("rotatebackward");
							UpdateElementVisual(editorSelected);
						}
					}

					// rot
				}



			}





			// Debug.Log ("mouse: "+mouseX+"/"+mouseY);


		}

	}

	void ResetRotation() {
		GameObject container=GameObject.Find ("editorCameraContainer");
		container.transform.position = new Vector3( positionReset.x, positionReset.y, positionReset.z );
		container.transform.localRotation = Quaternion.Euler( rotationReset.eulerAngles.x, rotationReset.eulerAngles.y, rotationReset.eulerAngles.z  );
	}

	// Relation Visualisation
	// (Lines between the different 
	// 
	public GameObject lineVisualisation;
	public GameObject lineVisualisationError;
	public GameObject lineVisualisationPath;

	ArrayList arrRelation = new ArrayList();
	void UpdateRelationVisualisationAndCheckError () {

		// clear all
		DeleteAllRelationVisuals ();

		// return 
		if (gameLogic!=null)
		if (gameLogic.modal != GameLogic.GameLogicModal.Editor)
			return;

		// all evaluation objects 
		GameElement sessionTmp;
		for (int i=0; i<arrEvaluationSessions.Count; i++) {
			sessionTmp = (GameElement) arrEvaluationSessions[i];
			// search now for things to show here
			ArrayList arr = GetEvaluationSessionElements( sessionTmp );
			if (arr.Count>0) {
				// ...
				Vector3 posTo = new Vector3 (0.0f, 0.0f, 0.0f);
				CreateVisualRelationAction ((GameElement) arr[0], posTo);

				if (arr.Count>1) {
					for (int ii=1;ii<arr.Count;ii++) {
						GameElement geFrom = (GameElement) arr[ii-1];
						GameElement geTo = (GameElement) arr[ii];
						CreateVisualRelationAction (geFrom, geTo.position);
					}
				}
			}

		}

		// all objects > show relations
		// go through all ...
		for (int a=0; a<arrLevel.Count; a++) {
			GameElement gelement = (GameElement)arrLevel [a];
			gelement.flagError = false;
			gelement.descError = "";
			// Debug.Log ("---"+a+". "+gelement.type+"/"+gelement.subtype+"   searching for: "+gameObj.GetInstanceID());
			// if (gelement.release!=null) {
			// check it now ... 
			// trigger/player

			// check type
			string checkType="";
			if (gelement.type.Equals ("trigger")) {
				if (gelement.subtype.Equals ("player")) {
					checkType="notification";
				}
				if (gelement.subtype.Equals ("activate")) {
					checkType="name";
				}
				if (gelement.subtype.Equals ("remove")) {
					checkType="name";
				}
			}
			if (gelement.type.Equals ("action")) {
				if (gelement.subtype.Equals ("notification")) {
					checkType="notification";
				}
			}
			if (gelement.type.Equals ("action")) {
				if (gelement.subtype.Equals ("repeaternotification")) {
					checkType="notification";
				}
			}
			if (gelement.type.Equals ("path")) {
				if (gelement.subtype.Equals ("path")) {
					checkType="names";
				}
			}

			// check types
			switch (checkType) {

			case "notification":
				if (gelement.argument.Equals ("")) {
					gelement.flagError = true;
					gelement.descError = "No notifaction!";
					CreateVisualRelationError (gelement);
				}
				if (!gelement.argument.Equals ("")) {
					Vector3 posTo = gelement.position + new Vector3 (0.0f, 5.0f, 0.0f);
					CreateVisualRelationAction (gelement, posTo);
				}
				break;
			case "name":
				if (gelement.argument.Equals ("")) {
					gelement.flagError = true;
					gelement.descError = "No name!";
					CreateVisualRelationError (gelement);
				}
				if (!gelement.argument.Equals ("")) {
					//bool err = false;
					ArrayList arr = GetGameElementsByName (gelement.argument);
					for (int i=0; i<arr.Count; i++) {
						GameElement gx = (GameElement)arr [i];
						if (gx!=null) {
							Vector3 posTo = gx.position; 
							CreateVisualRelationPath (gelement, posTo);
						} else {
							// err = true;	
						}
					}

					if (arr.Count==0) {
						gelement.flagError = true;
						gelement.descError = "No elements with name ("+gelement.argument+") found !";
						CreateVisualRelationError (gelement);
					}
				}
				break;

			case "names":
				if (gelement.argument.Equals ("")) {
					gelement.flagError = true;
					gelement.descError = "No list of names!";
					CreateVisualRelationError (gelement);
				}
				if (!gelement.argument.Equals ("")) {


					// one or more ,-seperated objects
					//bool err = false;
					string[] words = gelement.argument.Split(',');

					GameElement last = gelement; 
					foreach (string word in words)
					{
						// word 
						// test @abc > use names in unity3d 
						// default: 
						ArrayList arr = GetGameElementsByName (word);
						for (int i=0; i<arr.Count; i++) {
							GameElement gx = (GameElement)arr [i];
							if (gx!=null) {
								Vector3 posTo = gx.position; 
								CreateVisualRelationPath (last, posTo);
								last = gx; 
							}
						}

						if (arr.Count==0) {
							gelement.flagError = true;
							gelement.descError = "No elements with name ("+word+") found !";
							CreateVisualRelationError (gelement);

						}

						// @xyz > direct names in game ...
						// ex. @player1



					}

				}
				break;	

			}


		}

	}

	void DeleteAllRelationVisuals () {
		GameObject gobj;
		if (arrRelation.Count > 0) {
			for (int i=arrRelation.Count-1; i>=0; i--) {
				gobj = (GameObject)arrRelation [i];
				Destroy (gobj);
			}
		}
	}


	// Error
	void CreateVisualRelationError( GameElement elem ) {
		GameObject vl=Instantiate(lineVisualisationError, new Vector3(elem.position.x,elem.position.y,elem.position.z), new Quaternion()) as GameObject;
		vl.name = "NotFound3";

		LineRenderer lr=vl.GetComponent<LineRenderer>();
		lr.SetPosition (0, elem.position );
		lr.SetPosition (1, elem.position + new Vector3(0.0f,5.0f, 0.0f) );
		arrRelation.Add (vl);
	}

	// Path Visuals
	void CreateVisualRelationPath( GameElement elem, Vector3 pointTo ) {
		GameObject vl=Instantiate(lineVisualisationPath, new Vector3(elem.position.x,elem.position.y,elem.position.z), new Quaternion()) as GameObject;
		vl.name = "NotFound4";

		LineRenderer lr=vl.GetComponent<LineRenderer>();
		lr.SetPosition (0, elem.position );
		lr.SetPosition (1, pointTo );
		arrRelation.Add (vl);
	}
	// Action Visuals
	void CreateVisualRelationAction( GameElement elem, Vector3 pointTo ) {
		GameObject vl=Instantiate(lineVisualisation, new Vector3(elem.position.x,elem.position.y,elem.position.z), new Quaternion()) as GameObject;
		vl.name = "NotFound5";
		LineRenderer lr=vl.GetComponent<LineRenderer>();
		lr.SetPosition (0, elem.position );
		lr.SetPosition (1, pointTo );
		arrRelation.Add (vl);
	}

	// CreateVisualRelationEvaluation
	void CreateVisualRelationEvaluation( GameElement elem, Vector3 pointTo ) {
		GameObject vl=Instantiate(lineVisualisation, new Vector3(elem.position.x,elem.position.y,elem.position.z), new Quaternion()) as GameObject;
		vl.name = "NotFound6";
		LineRenderer lr=vl.GetComponent<LineRenderer>();
		lr.SetPosition (0, elem.position );
		lr.SetPosition (1, pointTo );
		arrRelation.Add (vl);
	}




}
