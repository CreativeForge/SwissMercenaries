/*
 * 
 * ### Editor  ###
 * 
 * // Editor: deactivate all children top level
  * 
* LevelElement (Editor-CreateElementTypeAtInspector) > GameElement (InEditor)
  * 
  * Ideen: Polls Ingame (Time or triggers)
  * 
  * NextToDo: Umstrukturieren
  * 
  * Problem: CameraManager auf der Editorcamera!
  * 
 * */

// new LevelObjects > levelElements

using UnityEngine;
using System.Collections;
using System;
using GameLab.LanguageCenter;
using GameLab.NotficationCenter;
using System.IO;
using System.Text.RegularExpressions;

public class LevelEditor : MonoBehaviour {

	// debugging
	bool debugGameElements=false;
	bool debugGameElementTypes=false;
	
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

		// overlay
		bool cameraOverlayTypes = false;

		public GameObject dummyEditorPrefab;

		Vector3 editorCursorActualPoint=new Vector3();

		GameElement editorLastTouchedGameElement = null;

	// load game level (used from GameLogic) 
	public void LoadGameLevel( int newLevel ) {

		actualLevel = newLevel;
		// todo: load level ... 
		ClearLevel ();
		LoadLevel (actualLevel);

		if (flagEvaluation) {
			// in running mode!
			NewSession (evaluationPlayer);
		}

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
		lv.level = actualLevel;
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
		SaveLevel( actualLevel );
	}


	ArrayList GetActualEditorHistory( ) {
		ArrayList arr = new ArrayList();

		LevelHistory lv;
		for (int i=0;i<arrEditorHistory.Count;i++) {
			lv = (LevelHistory) arrEditorHistory[i];
			if (lv.level==actualLevel) {
				arr.Add(lv);
			}
		}

		return arr;
	}

	// speeds
	float speedCamera = 0.2f;
	float speedObject = 0.2f;

	// buttons
	bool cursorObject = false; // object or ...
	int cursorX = Screen.width - 300;
	int cursorY = Screen.height - 200;
	Rect cursorRect = new Rect(0,0,0,0);
	ArrayList arrSensButtons = new ArrayList();
	class SensButton {
		public Rect rect = new Rect();
		public string text = "";
		public string key = "";
		public void Init( int ix, int iy, int iwidth, int iheight, string itext, string ikey) {
			rect.x = ix	;
			rect.y = iy;
			rect.width = iwidth;
			rect.height = iheight;
			text = itext;
			key = ikey;
		}
	}

	void AddSensButton(int x, int y, int width, int height, string text, string key) {
		SensButton sensbuttonObj = new SensButton();
		sensbuttonObj.Init( x,  y,  width,  height,  text,  key);
		arrSensButtons.Add(sensbuttonObj);
	}

	void MoveObjectAlongEditorCamera( string strdirect ) {
		// Debug.Log("LevelEditor.MoveObjectAlongEditorCamera() // "+strdirect);
		if (editorSelected!=null) {
			GameObject container=GameObject.Find ("editorCameraContainer");
			Vector3 direction = container.transform.forward;
			direction.Normalize();
			bool isMovement = true;
			if (strdirect.Equals("forward")) direction = direction;
			if (strdirect.Equals("backward")) direction = -direction;
			if (strdirect.Equals("up")) direction =  new Vector3(0.0f, speedObject, 0.0f);
			if (strdirect.Equals("down")) direction =  new Vector3(0.0f, -speedObject, 0.0f);
			if (strdirect.Equals("left")) direction = new Vector3(-direction.z, 0.0f, direction.x);
			if (strdirect.Equals("right")) direction =  new Vector3(direction.z, 0.0f, -direction.x);
			if (strdirect.Equals("rotateforward")) { direction =  new Vector3(0.0f,0.0f,0.0f); AddEditorMessage("Not implemented for objects!"); }
			if (strdirect.Equals("rotatebackward")) { direction =  new Vector3(0.0f,0.0f,0.0f); AddEditorMessage("Not implemented for objects!"); }
			if (strdirect.Equals("rotateleft")) { editorSelected.rotation = editorSelected.rotation - 5.0f; isMovement= false; }
				if (strdirect.Equals("rotateright")) { editorSelected.rotation = editorSelected.rotation + 5.0f; isMovement= false; }
			if (isMovement) {
				direction = direction * 0.5f;
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

		// Debug.Log("LevelEditor.DoSensButton("+strevent+")");

		if (cursorObject) {
			
			if (strevent.Equals("down")) {
				MoveObjectAlongEditorCamera(bu.key);
			}

			if (strevent.Equals("up")) {
				AddToEditorHistory();
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
				container.transform.Rotate ( new Vector3(0.0f, -2.0f, 0.0f));
			}
			if (bu.key.Equals("rotateright")) {
				container.transform.Rotate ( new Vector3(0.0f, 2.0f, 0.0f));
			}
			// 	rotate left & right					
			if (bu.key.Equals("rotateforward")) {
				container.transform.Rotate ( new Vector3(-2.0f,0.0f, 0.0f));
			}
			if (bu.key.Equals("rotatebackward")) {
				container.transform.Rotate ( new Vector3(2.0f,0.0f,  0.0f));
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

		SaveLevel( actualLevel );

	}

	// actual level
	int actualLevel=1;
	int maxLevel=8;

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

	// inspector
	int inspectorX = 10;
	int inspectorY = 100;
	int inspectorWidth = 300;
	int inspectorHeight = 200;
	Rect inspectorRect = new Rect(0,0,200,100);

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
		ClearElements ();

	}

		void DefaultElements() {
			// prefabY
			GameElement editorPrefabY;

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

		}

	void SetLevel( int iactualLevel ) {

		// Save History ...

		actualLevel = iactualLevel;
		// todo: load level ... 
		ClearLevel ();
		LoadLevel (actualLevel);
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
	public LevelElement[] GoalLevelElements= { new LevelElement ("survivetime"), new LevelElement ("killamount")   };
	public LevelElement[] ImmovablesLevelElements = { new LevelElement ("scheune"), new LevelElement ("city"),  new LevelElement ("fountain")    };
	public LevelElement[] ColliderLevelElements = {    };
	public LevelElement[] MovableLevelElements = { new LevelElement ("box")   };

	public LevelElement[] PlayerLevelElements = { new LevelElement ("player1"), new LevelElement ("player2") };
	public LevelElement[] SwissLevelSoldiersEleements = { new LevelElement ("small"), new LevelElement ("idiot") , new LevelElement ("cool")  };

	public LevelElement[] EnemyLevelElements= { new LevelElement ("waiting"), new LevelElement ("landsknecht") , new LevelElement ("dtreich")  };

	public LevelElement[] PathLevelElements = { new LevelElement ("tour"), new LevelElement ("waypoint"),  };

	public LevelElement[] ItemsLevelElements = { new LevelElement ("meat") };
	public LevelElement[] TriggerLevelElements = { new LevelElement ("playernotifcation") };

	public LevelElement[] ActionElements = { new LevelElement ("notification") }; // green
	public LevelElement[] EnvElements = { new LevelElement ("light") };

	public LevelElement[] RemarkElements = { new LevelElement ("comment") }; // yellow
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

		RegisterLevelElements( "goal", GoalLevelElements );
		RegisterLevelElements( "immovable", ImmovablesLevelElements );

		RegisterLevelElements( "collider", ColliderLevelElements );

		RegisterLevelElements( "movable", MovableLevelElements );

		RegisterLevelElements( "player", PlayerLevelElements );
		RegisterLevelElements( "swisssoldiers", SwissLevelSoldiersEleements );

		RegisterLevelElements( "enemy", EnemyLevelElements );

		RegisterLevelElements( "path", PathLevelElements );

		RegisterLevelElements( "item", ItemsLevelElements );
		RegisterLevelElements( "trigger", TriggerLevelElements );

		RegisterLevelElements( "action", ActionElements );
		RegisterLevelElements( "env", EnvElements );

		RegisterLevelElements( "remark", RemarkElements );
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
						geType.prefabEditorDummyArguments = el.prefabEditorDummyArguments; // .Copy(); // all the same reference
						geType.guiBoolArgument = el.argumentNeeded; 
						geType.guiLabel = el.argumentLabel;	 
						geType.guiDescription = el.argumentDescription;

						geType.argumentInputType = el.inputType;
						geType.argumentInputTypeSelect = el.inputTypeSelect;

//						geType.editorIndex = el.editorIndex;

						geType.editorTileSize = el.editorTileSize;
						geType.editorIsGround = el.isGround;

						geType.guiShowInMenu = visibleInEditor;

						geType.editorDisplaySize = el.editorDisplaySize;


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
			SaveLevel( actualLevel, true ); 

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
				
				string[] arrLevelFiles = GetExistingEvaluationLevelFiles (actualLevel);
				for (int i = 0; i < arrLevelFiles.Length; i++) {
					string fileN = arrLevelFiles [i];
					Match match = Regex.Match (fileN, @"level" + actualLevel + "_(\\d+)_(\\d+).", RegexOptions.IgnoreCase);
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

		void UpdateElementVisual( GameElement elem ) {
			
			GameObject levelObject = GameObject.Find ("level");

			if (levelObject == null) {
				levelObject = new GameObject();
				levelObject.name="level";
			}

			// is there one? remove it ..
			if (elem != null) {
				if (elem.gameObject != null) {
					Destroy (elem.gameObject);
				}

			GameElement elPrefab = GetElementType (elem.type,elem.subtype);
			if (elPrefab==null) { Debug.Log("Error: Could not find Type("+elem.type+"/"+elem.subtype+")"); return; } 
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
					if (elem.rotation!=0.0f) {
						re = Quaternion.Euler(0, elem.rotation, 0);
					}

// elPrefab.prefabGameObject // .prefabEditor
					if (elPrefab.prefabGameObject==null) {
						// take the dummy object
						if (gameLogic !=null && gameLogic.modal==GameLogic.GameLogicModal.Editor ) {
						   GameObject go=Instantiate(dummyEditorPrefab, new Vector3(elem.position.x,elem.position.y,elem.position.z), re) as GameObject;
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

							
						}
						

						// special - only in editor?
						if (gameLogic !=null && gameLogic.modal==GameLogic.GameLogicModal.Editor ) {
							
							GameObject go = new GameObject();
							if (elPrefab.prefabEditorDummyGameObject!=null) {
								
						 	//	if (elem.prefabEditorDummyArguments==null) Debug.Log(". LeveLElementOption NULL"+elem.prefabEditorDummyArguments);
							// else Debug.Log(". LeveLElementOption FOUND!"+elem.prefabEditorDummyArguments);

							// Debug.Log(". LeveLElementOption ??? "+elem.type+" / "+elem.subtype+"  "+elem.argument);

								// argument?
								// no alternative elements for argument
								if (elem.prefabEditorDummyArguments==null) {
									go=Instantiate(elPrefab.prefabEditorDummyGameObject, new Vector3(elem.position.x,elem.position.y,elem.position.z), re) as GameObject;
								} else {
								// Debug.Log(". LeveLElementOption FOUND! "+elem.argument+" "+elem.prefabEditorDummyArguments);

									bool found=false;
									LevelElementOption leo;
									for (int ix = 0; ix < elem.prefabEditorDummyArguments.Length; ix ++) {
										leo = elem.prefabEditorDummyArguments[ix];
										// Debug.Log(ix+". LeveLElementOption "+leo.argument+" vs "+elem.argument);
										if (leo.argument.Equals(elem.argument)) {
											if (leo.editorPrefab!=null) {
												found=true;
												go=Instantiate(leo.editorPrefab, new Vector3(elem.position.x,elem.position.y,elem.position.z), re) as GameObject;
											}
										}
									} 
									if (!found) {
										go=Instantiate(elPrefab.prefabEditorDummyGameObject, new Vector3(elem.position.x,elem.position.y,elem.position.z), re) as GameObject;
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
								go=Instantiate(elPrefab.prefabGameObject, new Vector3(elem.position.x,elem.position.y,elem.position.z), re) as GameObject;
								// size
								if (elem.size!=1.0f) {
									go.transform.localScale = elem.size * go.transform.localScale;
								}
								elem.gameObject=go;
								if (!elem.name.Equals("")) { go.name=""+elem.name; }
								go.transform.parent = levelObject.transform; 
							}



						}
						
						// game ...
						
						// Debug.Log("Could find Type("+elem.type+"/"+elem.subtype+") has prefab!");
						if (gameLogic !=null && gameLogic.modal!=GameLogic.GameLogicModal.Editor ) {
// Debug.Log("PREFAB");			
							// Debug.Log ("[LevelEditor] CREATE["+elem.name+"/"+elem.type+"."+elem.subtype+"/"+elem.release+"]");
						
							// only instiante pure releases (no waits)
							if (elem.release.Equals ("")) {
								GameObject go=Instantiate(elPrefab.prefabGameObject, new Vector3(elem.position.x,elem.position.y,elem.position.z), re) as GameObject;
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

	public void RemoveElement( GameElement elem ) {

		if (elem != null) {
			if (elem.gameObject != null)
				Destroy (elem.gameObject);
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
	float[] arrRasters = { 0.0f, 0.5f, 1.0f, 2.0f, 3.0f, 4.0f, 8.0f };
	int editorDegree = 0;

	void SetRasterIndex( int index ) { // Index!
		editorRaster = index;
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
		editorTool = ieditorTool;
		if (editorTool.Equals ("SPLIT")) {
			editorToolSub="right";
		}
	}
	string[] arrEditorTools={"CREATE","EDIT","MOVE","SPLIT","DELETE","EVALU"};	

	// special in tools
	// edit
	void SetSelectedElement( GameElement ga ) {
		// Debug.Log ("SetSelectedElement()");
		editorSelected = ga;
		if (ga!=null) {
			editorArea = ga.type;
			SetSelectedElementToGUI ();

			// also update preview for recognition
			editorArea = ga.type;
			editorSubArea = ga.subtype;
			SetSubEditorArea( editorSubArea );
		}

	}
		// update to 
		void SetSelectedElementToGUI() {
			//editDetailX = ""+ editorSelected.position.x;
			//editDetailY = ""+ editorSelected.position.y;
			editDetailName = ""+ editorSelected.name;
			editDetailArgument = ""+ editorSelected.argument;
		}
	void StoreSelectedElement(  ) {
		SetSelectedElementFromGUI ();
	}
		// update to 
		void SetSelectedElementFromGUI() {
		//		editDetailSelected.position.x=editDetailX;
		// editorDetailSelected.position.x=editDetailY;
		editorSelected.name=editDetailName;
		editorSelected.argument=editDetailArgument;
	}

	// special tools: edit
  /*
	string editDetailX="";
	string editDetailY="";
  */

	string createName = "";
	string editDetailName="";
	string editDetailArgument="";
	
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
		} else {
			Debug.LogError("SetSubEditorArea() Could correct");
		}
	}

	public void SetEditorPreviewToPrefab( GameObject prefab , float scaling) {

		// Debug.Log("SetEditorPreviewToPrefab()");

		// find preview
		GameObject preview = GameObject.Find("editorpreview");

		if (preview!=null) {
			// delete old one 
			foreach (Transform child in preview.transform) {
				Destroy(child.gameObject);
				break;
			}

			// Debug.Log("SetEditorPreviewToPrefab(){ preFabFound = "+prefab+" }");

			// create one an add it 
			if (prefab!=null) {

				GameObject newPreview=Instantiate(prefab, new Vector3(preview.transform.position.x,preview.transform.position.y,preview.transform.position.z), new Quaternion()) as GameObject;
				float scaleFactor = 0.03f * scaling;
				newPreview.transform.localScale = new Vector3(scaleFactor,scaleFactor,scaleFactor);
				newPreview.transform.parent = preview.transform;
			}
		} else {
			Debug.LogError("SetEditorPreviewToPrefab() Could not find editorpreview-Object!");
		}
	}

	// editor Text
	string editorLogText="";


	// GameElement
	GameElement editorPrefab; // [CREATE]: selected prefab
	GameElement editorSelected; // [EDIT]: which element is selected?
	// GameElement editorChangeToSelection = new GameElement(); // [EDIT]: change element type to ... 

	public GUIStyle editorBackground;
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

		
	//	public GUIStyle editorDeleteStyle;
		public GUIStyle editorComment;

		public GUIStyle editorIconGUI;

		public GUIStyle editorElementType;

		public GUIStyle guiEvaluation;
		
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
	// 


	void LoadLevel( int level  ) {


			LoadLevel(level, "", "" ); // load a level raw

			historyIndexMinus = 0;

			SetSelectedElement(null);

			// update 
			UpdateShowEvaluationData ();

			Debug.Log("gameLogic: "+gameLogic);
			if (gameLogic && gameLogic.modal == GameLogic.GameLogicModal.Editor) {
				AddToEditorHistory("LoadLevel");
			}
	}

	void LoadEvaluationLevels( ) {

		// Debug.Log("[LevelEditor]LoadEvaluationLevels()");

		// search all local
		string[] arrLevelFiles = GetExistingEvaluationLevelFiles( actualLevel );
		for (int i = 0; i < arrLevelFiles.Length; i++ ) {
			string fileN = arrLevelFiles[i];
			// Debug.Log(i+". "+fileN);

			Match match = Regex.Match(fileN, @"level"+actualLevel+"_(\\d+)_(\\d+).", RegexOptions.IgnoreCase);

			if (!editorEvaluationFilter.Equals("all")) {

				if (editorEvaluationFilter.Equals("player")) {

					// Debug.Log("editorEvaluationPlayerId: "+editorEvaluationPlayerId);

					match = Regex.Match(fileN, @"level"+actualLevel+"_("+editorEvaluationPlayerId+")_(\\d+).", RegexOptions.IgnoreCase);

				}

				// editorEvaluationSessionId
				if (editorEvaluationFilter.Equals("player.session")) {
					
					// Debug.Log("editorEvaluationPlayerId: player.session "+editorEvaluationPlayerId);
					match = Regex.Match(fileN, @"level"+actualLevel+"_("+editorEvaluationPlayerId+")_("+editorEvaluationSessionId+").", RegexOptions.IgnoreCase);
					
				}

			}

			// Here we check the Match instance.
			if (match.Success)
			{
			    // Finally, we get the Group value and display it.
			    string playerId = match.Groups[1].Value;
			    string sessionId = match.Groups[2].Value;
			    // Debug.Log("-"+i+". FOUND: ("+playerId+"/"+sessionId+")");

			    LoadLevel ( actualLevel, playerId, sessionId );
			}
		}

		// search web

		
	}

	// playerId, sessionId
	void LoadLevel( int level,  string playerId, string sessionId ) {

		bool flagEvaluationTemp = false; 
		if (!playerId.Equals("")) {
			flagEvaluationTemp = true; 
		}

		editorLogText = ""+DateTime.Now.ToString("LOADED: HH:mm:ss");

		// add evaluation 
 		string addEvaluationsFolder = ""; // all 
		string addFileNameAddOn = "";		
		if (flagEvaluationTemp) {
			addEvaluationsFolder = evaluationFolder+Path.DirectorySeparatorChar; 
			addFileNameAddOn = "_"+playerId+"_"+sessionId;
		}

	//	try {
			
			string jsonText = System.IO.File.ReadAllText( addEvaluationsFolder+ "level"+level+addFileNameAddOn+".txt");
			JSONObject jsonObj = new JSONObject(jsonText);
			// array
			// Debug.Log ("Load().Size="+jsonObj.list.Count);
			foreach(JSONObject listObj in jsonObj.list){
				GameElement ge=new GameElement();
					// get the default members
					GameElement typege=new GameElement();
								typege.GetObjectFromJSON(listObj);

					// ingame 
					GameElement gaType = GetElementType ( typege.type, typege.subtype );
					if (gaType != null) {
						ge = gaType.Copy ();
						ge.GetObjectFromJSON(listObj);
					}
					
				// add it
				bool flagAdd = false;
				if (!flagEvaluationTemp) { if (!ge.type.Equals ("evaluation")) { flagAdd=true; } }
				if (flagEvaluationTemp) { if (ge.type.Equals ("evaluation")) { flagAdd=true; } } 
				if (flagAdd) {
						AddElement (ge);
				}
			}

			// save it now ... 
			SaveLevel ( 2001 );

	//	} catch( Exception e ) {
	//		Debug.LogWarning("_LevelEditor.LoadGameLevel() // CouldNotLoadLevel "+level );
		//	}
		
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
				JSONObject gelementJSON=gelement.GetJSONObject();
				// Debug.Log (gelementJSON.Print ());
				arrElementsJSON.Add (gelementJSON);
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
		System.IO.File.WriteAllText(evaluationFolderAddOn+"level"+level+fileEvaluationAddOn+".txt",""+encodedString);

		// save to file ...
		if (!iflagEvaluation) {
			System.IO.File.WriteAllText(folderName+ Path.DirectorySeparatorChar + "level"+level+"_"+DateTime.Now.ToString("yyyyMMddHHmmss")+".txt",""+encodedString);
		}

		editorLogText = ""+DateTime.Now.ToString("SAVED: HH:mm:ss");

		// Debug.Log(encodedString);

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
	
	// Use this for initialization
	void Start () {

		// Get GameLogic
		GetGameLogic ();

		// Notification Center
		GetNotificationCenter();

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
		SetLevel (1);


		// check for playerId
		// > generate playerId

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

		if ((mouseX>(Screen.width-200))&&(mouseX<(Screen.width))
			&&
			(mouseY>=0)&&(mouseY<(20))) {
			return true;
		}


		if ((mouseX>cursorRect.x)&&(mouseX<(cursorRect.x+cursorRect.width))
			&&
			(mouseY>cursorRect.y)&&(mouseY<(cursorRect.y+cursorRect.height))) {
			return true;
		}

		// inspectorRect
		if ((mouseX>inspectorRect.x)&&(mouseX<(inspectorRect.x+inspectorRect.width))
			&&
			(mouseY>inspectorRect.y)&&(mouseY<(inspectorRect.y+inspectorRect.height))) {
			return true;
		}

		if ((mouseX>filterTypeVisual.x)&&(mouseX<(filterTypeVisual.x+filterTypeVisual.width))
			&&
			(mouseY>filterTypeVisual.y)&&(mouseY<(filterTypeVisual.y+filterTypeVisual.height))) {
				return true;
		}
		if ((mouseX>filterTypeSubVisual.x)&&(mouseX<(filterTypeSubVisual.x+filterTypeSubVisual.width))
			&&
				(mouseY>filterTypeSubVisual.y)&&(mouseY<(filterTypeSubVisual.y+filterTypeSubVisual.height))) {
				return true;
			}

		if ((mouseX>selectionDialogeVisual.x)&&(mouseX<(selectionDialogeVisual.x+selectionDialogeVisual.width))
			&&
			(mouseY>selectionDialogeVisual.y)&&(mouseY<(selectionDialogeVisual.y+selectionDialogeVisual.height))) {
			return true;
		}

		if ((mouseX>undoVisual.x)&&(mouseX<(undoVisual.x+undoVisual.width))
			&&
			(mouseY>undoVisual.y)&&(mouseY<(undoVisual.y+undoVisual.height))) {
			return true;
		}

		if ((mouseX>notificationVisual.x)&&(mouseX<(notificationVisual.x+notificationVisual.width))
			&&
			(mouseY>notificationVisual.y)&&(mouseY<(notificationVisual.y+notificationVisual.height))) {
			return true;
		}

		if ((mouseX>nearbyRect.x)&&(mouseX<(nearbyRect.x+nearbyRect.width))
			&&
			(mouseY>nearbyRect.y)&&(mouseY<(nearbyRect.y+nearbyRect.height))) {
			return true;
		}

		if ((mouseX>toolsRect.x)&&(mouseX<(toolsRect.x+toolsRect.width))
			&&
			(mouseY>toolsRect.y)&&(mouseY<(toolsRect.y+toolsRect.height))) {
			return true;
		}

		if ((mouseX>typeselectionRect.x)&&(mouseX<(typeselectionRect.x+typeselectionRect.width))
			&&
			(mouseY>typeselectionRect.y)&&(mouseY<(typeselectionRect.y+typeselectionRect.height))) {
			return true;
		}

		// Debug.Log ("CheckMouseInEditor() > FALSE; ");
		return false;
	}

	bool GameElementInEditor( float x, float y ) {
		float mouseX=x;
		float mouseY=Screen.height-y;
		if ((mouseX>editorPrefX)&&(mouseX<(editorPrefX+editorWidth))&&(mouseY>editorPrefY)&&(mouseY<(editorPrefY+editorHeight))) {
			return true;
		}
		return false;
	}

	void HandleMouseDownToCreate(){
		if (!editorTool.Equals ("CREATE"))return;

		if (CheckMouseInEditor()) return;

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

			arg.name = createName;

			// rotation
			arg.rotation = editorDegree;
			UpdateElementVisual(arg);

			// tool: add randomness
			if (flagTool) {
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

	void OnGUI() {


		if (Event.current.type == EventType.MouseUp && Event.current.button == 0 && GUIUtility.hotControl == 0) {
			// This code will be ignored if the user had mouse-downed on a GUI element.
			HandleMouseDownToCreate();
		}



		bool debugThis = false; 
		
		// SWITCH BETWEEN EDITOR/GAME
		GUIStyle guixt = editorSwitchButtonStyle;

		// debug
		if (debugThis) {
			GUI.Label (new Rect (Screen.width - 200, 0, 200, 80), "LEVELEDITOR", guixt);
		}

		// GAME
		if (gameLogic!=null) {
		if (gameLogic.modal == GameLogic.GameLogicModal.Running) {
			guixt = editorSwitchButtonStyleActive ;
		}
		if (GUI.Button (new Rect (Screen.width - 160, 0, 80, 20), "GAME", guixt)) {
			gameLogic.SetGameState( GameLogic.GameLogicModal.Running );
		}

		// EDITOR
		guixt = editorSwitchButtonStyle;
		if (gameLogic.modal == GameLogic.GameLogicModal.Editor) {
			guixt = editorSwitchButtonStyleActive ;

			// default?
			if (!CountElementsTypeIndexOf("directlight")) { 
				GUI.Label (new Rect (10, Screen.height*0.5f-24, 500, 20), "1. NO DIRECT LIGHT IN SCENE! USE DEFAULT >> ADD ONE UNDER /ENV/ ", guixt);
			}

			// default?
			if (!CountElementsTypeIndexOf("player")) { 
				GUI.Label (new Rect (10, Screen.height*0.5f, 500, 20), "2. NO PLAYER > ADD ONE > /PLAYER/. ", guixt);
			}

			// default?
			if (!CountElementsTypeIndexOf("ingamecontroller")) { 
				GUI.Label (new Rect (10, Screen.height*0.5f+24, 500, 20), "3. NO GAMELOGIC CONTROLLER > ADD ONE > /BASE/GAME... ", guixt);
			}

			// messages
			for (int a=0; a<arrEditorMessages.Count; a++) {
					LevelEditorMessage msgObj = (LevelEditorMessage)arrEditorMessages [a];
					GUI.Label (new Rect (10, Screen.height*0.5f+60+a*22, 500, 20), ""+msgObj.message, guixt);
						
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
		if (GUI.Button (new Rect (Screen.width -160 + 80, 0, 80, 20), "EDITOR", guixt)) {
			gameLogic.SetGameState( GameLogic.GameLogicModal.Editor );

		}
	
		
		// get latest
		// version
			float ver = gameLogic.GetVersionGame();
			if (gameLogic.modal == GameLogic.GameLogicModal.Editor) ver = gameLogic.GetVersionEditor();
		if (GUI.Button (new Rect (Screen.width - 260, 0, 80, 20), " v."+ver, editorSwitchButtonStyle)) {
			
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
					cam = GameObject.Find ("editorcamera").GetComponent<Camera>();
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
						if (!gaelement.argument.Equals ("")) { showInfo=true; if (!textInfo.Equals ("")) textInfo=textInfo+" "; /* textInfo=textInfo+"{"+gaelement.argument+"}"; */ } 

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
						if (!showInfo) {
							GUI.Label (new Rect (screenPos.x+20, Screen.height - screenPos.y , 200, 80),""+waiting+" "+strType,editorElementType );
						}
						if (showInfo) {
							// GUI.Label (new Rect (screenPos.x+20, Screen.height - screenPos.y, 200, 80),+"        "+strType,editorElementType );
							string str = textInfo;
							if (str.Equals("")) {
								str = strType;
							}
							GUI.Label (new Rect (screenPos.x+20, Screen.height - screenPos.y, 200, 80),waiting+""+str);
						}

						// edit ?
						if (editorTool.Equals ("EDIT")) {
							if (GUI.Button (new Rect (screenPos.x, Screen.height - screenPos.y, 20, 20), editorEditImage, editorIconGUI)) {
								if (!CheckMouseInEditor()) {
									SetSelectedElement(gaelement);
								}
							}
							if (editorSelected==gaelement) {
								GUI.Label (new Rect (screenPos.x-10, Screen.height - screenPos.y-10, 40, 40), editorSelectedImage, editorIconGUI);
								nearbyX = (int ) (screenPos.x - 125);
								nearbyY = (int) (Screen.height - screenPos.y);
							} 
						}

						// move ?
						// version 1.0

						if (editorTool.Equals ("MOVE")) {

							if (!gaelement.type.Equals("base")) {
								if (GUI.Button (new Rect (screenPos.x, Screen.height - screenPos.y, 20, 20), editorMoveImage, editorIconGUI)) {
									// SetSelectedElement(gaelement);
									//		Debug.Log("Move Pressed");

								}
							} else {
								if (GUI.Button (new Rect (screenPos.x, Screen.height - screenPos.y, 20, 20), editorEditImage, editorIconGUI)) {
									SetSelectedElement(gaelement);
									SetTool("EDIT");
								}
							}
							if (editorSelected==gaelement) {
								GUI.Label (new Rect (screenPos.x-10, Screen.height - screenPos.y-10, 40, 40), editorSelectedImage, editorIconGUI);
							} 
						}

						// version 2.0
						if (editorTool.Equals ("MOVE")) {

							float buttonX=screenPos.x;
							float buttonY=Screen.height-screenPos.y;
							float buttonWidth=20.0f;

							if (!gaelement.type.Equals("base"))
							if (
								(mouseX>buttonX)&&(mouseX<(buttonX+buttonWidth)) 
								&&
								(mouseY>buttonY)&&(mouseY<(buttonY+buttonWidth)) 
							)
							{
								if (Input.GetMouseButtonDown(0)) {
									if (editorToolMove.Equals ("")) {
										// move
										SetSelectedElement(gaelement);
										editorToolMove="drag";
									}
								}


							}
							// dragging
							if (Input.GetMouseButton(0)) {
								if (gaelement==editorSelected) {
									// move
									if (editorToolMove.Equals ("drag")) {
										// Debug.Log("Moving "+mouseX);
										UpdateGameElementToPosition(gaelement,Input.mousePosition);

										editorLastTouchedGameElement = editorSelected;
									}
								}
							}

							// mouse up
							if (Input.GetMouseButtonUp(0)) {

								if (editorSelected!=null) {

									Debug.Log("LevelEditor.OnGUI() // MouseButtonUp(0)");

									float raster=GetRaster();
									// Debug.Log ("raster: "+raster);
									if (raster!=0.0f) {
										if (editorSelected!=null) {
											// Debug.Log ("Selected: "+editorSelected.position.x+"/"+editorSelected.position.y);



											float offsetX=0.25f;
											float offsetY=0.25f;

											// editorSelected.position.x=((int)((editorSelected.position.x+offsetX)/raster))*raster;
											// editorSelected.position.y=((int)((editorSelected.position.y+offsetY)/raster))*raster;

											editorSelected.position.x=(Mathf.Floor((editorSelected.position.x+offsetX)/raster))*raster;
											editorSelected.position.z=(Mathf.Floor((editorSelected.position.z+offsetY)/raster))*raster;

											UpdateElementVisual(editorSelected);


										}
									}

									AddToEditorHistory("[GUI][OBJECT][MOVE]");

									// move
									editorSelected=null;
									editorToolMove="";

								}
							}

						}

						// delete ?
						if (editorTool.Equals ("DELETE")) {
							if (GUI.Button (new Rect (screenPos.x, Screen.height - screenPos.y, 20, 20), editorDeleteImage, editorIconGUI)) {
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
				if (GUI.Button (new Rect (Screen.width -160 , 24, 160, 20), "EVALUATION", editorSwitchButtonStyleActive)) {
					// gameLogic.SetGameState( GameLogic.GameLogicModal.Editor );
					showEvaluationDialog=!showEvaluationDialog;
					UpdateRelationVisualisationAndCheckError();
				}
			}

			// show user evaluation 
			if (showEvaluationDialog) {
				if (flagEvaluation)
				if (gameLogic.modal == GameLogic.GameLogicModal.Running ) {

					int edX = Screen.width - 760;

					if (GUI.Button (new Rect (edX , 0, 78, 20), "EVALUATE! ", guiEvaluation)) {
					}
					edX = edX + 80;

					string[] names = { "++","+"," ","-","--" };
					string[] values = { "2","1","0","-1","-2" };

					// evaluationUserAllOver = "0";

					for (int i=0;i<names.Length;i++) {
						string allover = "";
						if (evaluationUserAllOver.Equals(""+values[i])) {
							allover = ">";
						}
						if (GUI.Button (new Rect (edX , 0, 38, 20), allover+""+names[i], guiEvaluation)) {
							evaluationUserAllOver = ""+values[i]; 
							GameObject firstPlayer = GetFirstPlayerObject();
							Vector3 pos = firstPlayer.transform.position;
							AddEvaluationElement( "allover", ""+values[i],  pos );
						}
						edX = edX + 40;
					}

					// comments
					evaluationUserComment=GUI.TextField (new Rect(edX,0,100,20),evaluationUserComment);
					edX = edX + 100;
					if (GUI.Button (new Rect (edX , 0, 78, 20), "COMMENT", guiEvaluation)) {
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
					if (GUI.Button (new Rect (Screen.width -160 , evaluationY, 160, 20), ""+flagEval, guiEvaluation)) {
						flagEvaluation=!flagEvaluation;
					}
					evaluationY = evaluationY+ 22;




					// only config
					if (flagEvaluation) {


						string str = "NOT DEFINED";  // evaluationPlayer.playerId
						if (evaluationPlayer!=null) { 
							str = ""+evaluationPlayer.name+"" + "("+ evaluationPlayer.playerId +")";
						}
						if (GUI.Button (new Rect (Screen.width -160 , evaluationY, 160, 20), "[EDIT: "+str +"]", guiEvaluation)) {
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
						if (GUI.Button (new Rect (Screen.width -160 , evaluationY, 160, 20), ""+saveTo, guiEvaluation)) {
							flagSaveToWeb=!flagSaveToWeb;
						}
						evaluationY = evaluationY+ 22;


						// NEW PLAYER
						evaluationY = evaluationY+ 22;
						if (GUI.Button (new Rect (Screen.width -160 , evaluationY, 160, 20), " + NEW PLAYER", guiEvaluation)) {
							CreateNewEvaluationPlayer();
						}
						evaluationY = evaluationY+ 22;
						evaluationY = evaluationY+ 22;

						evaluationY = evaluationY+ 10;

						// POSSIBLE ADD ONS HERE
						/*
						if (GUI.Button (new Rect (Screen.width -160 , evaluationY, 160, 20), "POSITIVE - AREA", guiEvaluation)) {
							GameObject firstPlayer = GetFirstPlayerObject();
							Vector3 pos = firstPlayer.transform.position;
							AddEvaluationElement( "allover", "1",  pos );
						}
						evaluationY = evaluationY+ 22;
						if (GUI.Button (new Rect (Screen.width -160 , evaluationY, 160, 20), "HATE AREA", guiEvaluation)) {
							GameObject firstPlayer = GetFirstPlayerObject();
							Vector3 pos = firstPlayer.transform.position;
							AddEvaluationElement( "allover", "-1", pos );
						}
						evaluationY = evaluationY+ 22;
						*/


						// LOAD LEVEL
						/*
						evaluationY = evaluationY+ 22;
						if (GUI.Button (new Rect (Screen.width -160 , evaluationY, 160, 20), "LOAD ALL EVALUATIONS", guiEvaluation)) {
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

						int dialogEvaluationStartX = Screen.width - 700;
						int dialogEvaluationStartY = 20;
						int dialogEvaluationWidth = 400;
						int dialogEvaluationHeight = 400;
						if (showEvaluationDialogEditType.Equals ("player")) {
							title = "EDIT EVALUATION PLAYER";
						}

						int dialogEvaluationX = dialogEvaluationStartX + 10;
						int dialogEvaluationY = dialogEvaluationStartY + 10;

						GUI.Label (new Rect(dialogEvaluationX,dialogEvaluationY,dialogEvaluationWidth,dialogEvaluationHeight),"",guiEvaluation);
						GUI.Label (new Rect(dialogEvaluationX,dialogEvaluationY,dialogEvaluationWidth,28),""+title,guiEvaluation);
						if (GUI.Button (new Rect(dialogEvaluationX+dialogEvaluationWidth-100,dialogEvaluationY,100,28),"[CLOSE]",guiEvaluation)) {
							EditEvaluationPlayerStop();
						}
						dialogEvaluationY = dialogEvaluationY + 22;
						dialogEvaluationY = dialogEvaluationY + 22;

						string arg = "";

						// name
						GUI.Label (new Rect(dialogEvaluationX,dialogEvaluationY,160,20),"NAME: ",guiEvaluation);
						arg=GUI.TextField (new Rect(dialogEvaluationX+120,dialogEvaluationY,200,20),evaluationPlayer.name);
						if (!arg.Equals (evaluationPlayer.name)) {
							evaluationPlayer.name = arg;
							SaveEvaluationPlayer( evaluationPlayer );
						}
						dialogEvaluationY = dialogEvaluationY + 22;

						// prename
						GUI.Label (new Rect(dialogEvaluationX,dialogEvaluationY,160,20),"PRENAME: ",guiEvaluation);
						arg=GUI.TextField (new Rect(dialogEvaluationX+120,dialogEvaluationY,200,20),evaluationPlayer.prename);
						if (!arg.Equals (evaluationPlayer.prename)) {
							evaluationPlayer.prename = arg;
							SaveEvaluationPlayer( evaluationPlayer );
						}
						dialogEvaluationY = dialogEvaluationY + 22;

						dialogEvaluationY = dialogEvaluationY + 22;

						// age
						GUI.Label (new Rect(dialogEvaluationX,dialogEvaluationY,160,20),"AGE: ",guiEvaluation);
						arg=GUI.TextField (new Rect(dialogEvaluationX+120,dialogEvaluationY,200,20),""+evaluationPlayer.age);
						if (!arg.Equals (""+evaluationPlayer.age)) {
							evaluationPlayer.age = Int32.Parse(arg);
							SaveEvaluationPlayer( evaluationPlayer );
						}
						dialogEvaluationY = dialogEvaluationY + 22;

						// game play 
						dialogEvaluationY = dialogEvaluationY + 22;
						
						// casual
						GUI.Label (new Rect(dialogEvaluationX,dialogEvaluationY,160,20),"CASUAL(x%): ",guiEvaluation);
						arg=GUI.TextField (new Rect(dialogEvaluationX+120,dialogEvaluationY,200,20),""+evaluationPlayer.lovedCasual);
						if (!arg.Equals (""+evaluationPlayer.lovedCasual)) {
							evaluationPlayer.lovedCasual = Int32.Parse(arg);
							SaveEvaluationPlayer( evaluationPlayer );
						}
						dialogEvaluationY = dialogEvaluationY + 22;
						// core
						GUI.Label (new Rect(dialogEvaluationX,dialogEvaluationY,160,20),"CORE(x%): ",guiEvaluation);
						arg=GUI.TextField (new Rect(dialogEvaluationX+120,dialogEvaluationY,200,20),""+evaluationPlayer.lovedCore);
						if (!arg.Equals (""+evaluationPlayer.lovedCore)) {
							evaluationPlayer.lovedCore = Int32.Parse(arg);
							SaveEvaluationPlayer( evaluationPlayer );
						}
						dialogEvaluationY = dialogEvaluationY + 22;
						// genre
						GUI.Label (new Rect(dialogEvaluationX,dialogEvaluationY,160,20),"GENREPLAYER: ",guiEvaluation);
						arg=GUI.TextField (new Rect(dialogEvaluationX+120,dialogEvaluationY,200,20),""+evaluationPlayer.lovedGenre);
						if (!arg.Equals (evaluationPlayer.lovedGenre)) {
							evaluationPlayer.lovedGenre = arg;
							SaveEvaluationPlayer( evaluationPlayer );
						}
						dialogEvaluationY = dialogEvaluationY + 22;


						// comment
						dialogEvaluationY = dialogEvaluationY + 22;
						GUI.Label (new Rect(dialogEvaluationX,dialogEvaluationY,160,20),"COMMENT: ",guiEvaluation);
						arg=GUI.TextField (new Rect(dialogEvaluationX+120,dialogEvaluationY,200,20),""+evaluationPlayer.comment);
						if (!arg.Equals (evaluationPlayer.comment)) {
							evaluationPlayer.comment = arg;
							SaveEvaluationPlayer( evaluationPlayer );
						}
						dialogEvaluationY = dialogEvaluationY + 22;

						dialogEvaluationY = dialogEvaluationY + 22;

						// sessions
						GUI.Label (new Rect(dialogEvaluationX,dialogEvaluationY,160,20),"SESSION: "+evaluationPlayer.sessionId,guiEvaluation);
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
				AddToEditorHistory();
			}

			// infos about the mouse point (for putting in 

			/*
			 *   TOOLS
			 * 
			 * */
			// background
			GUI.Label ( new Rect(toolsRect.x-5,toolsRect.y-5,toolsRect.width+5,toolsRect.height+10), "", editorBackground);


			// editor X / Y
			int toolsXTmp=toolsX;
			int toolsXTmpMax = toolsX;
			int toolsYTmp=toolsY;

//			GUI.Label (new Rect (editorX, editorY, editorWidth, editorHeight), "", editorBackground);

			// working on level
			// maxLevel
			GUI.Label (new Rect (toolsXTmp, toolsYTmp, 60, 20), "LEVEL: ", editorButtonActiveStyle);
			// levels
			toolsXTmp = toolsXTmp + 62;

			// clear
			if (GUI.Button (new Rect (toolsXTmp , toolsYTmp, 58, 20), "CLEAR", editorButtonActiveStyle)) {
				ClearLevel ();  
				// NewLevel();
				DefaultElements();
				SaveLevel (actualLevel);
			}
			toolsXTmp = toolsXTmp + 60;

			// levels
			for (int i=1; i<maxLevel; i++) {
				string text = "" + i;
				// actualLevel
				GUIStyle gui = editorButtonStyle;
				if (i == actualLevel) {
					gui = editorButtonActiveStyle;
					text = ">" + text + "";
				}

				bool buttonClicked = GUI.Button (new Rect (toolsXTmp, toolsYTmp, 20, 20), text, gui);
				if (buttonClicked) {
					// scroll=0.0f;
					// EditorUpdateCameraToScroll(scroll);
					//stateSpecialEditor="";
					// SetLevel (i);
					gameLogic.SetGameLevel( i );
				}

				toolsXTmp = toolsXTmp + 22;
			}

			toolsXTmp = toolsXTmp + 2;



			// COPYTO
			if (GUI.Button (new Rect (toolsXTmp, toolsYTmp, 60, 20), "SAVE AS ", editorButtonActiveStyle)) {
				flagShowSaveAs = !flagShowSaveAs;
			}
			toolsXTmp = toolsXTmp + 62;

			if (flagShowSaveAs) {
			// levels
			for (int i=1; i<maxLevel; i++) {
				string text = "" + i;
				// actualLevel
				GUIStyle gui = editorButtonStyle;

					bool buttonClicked = GUI.Button (new Rect (	toolsXTmp, toolsYTmp, 20, 20), text, gui);
				if (buttonClicked) {
					SaveLevel (i);
					gameLogic.SetGameLevel(i);
					flagShowSaveAs = !flagShowSaveAs;
				}
				toolsXTmp = toolsXTmp + 22;
			}
			}

			if (toolsXTmp>toolsXTmpMax) toolsXTmpMax = toolsXTmp;

			toolsYTmp = toolsYTmp + 26;

			// UNDO/REDO
			// undoVisual
			toolsXTmp = toolsX;
			// toolsXTmp = toolsX;

				
// 			toolsYTmp = toolsYTmp + 22;
			/*
			// load & save
			for (int i=0; i<4; i++) {
				string text = "";
				if (i == 0) {
					text = "LOAD";
				}
				if (i == 1) {
					text = "SAVE";
				}
				if (i == 2) {
					text = "CLEAR";
				}
				if (i == 3) {
					// text
					text = "NEW LEV";
				}
				bool buttonClicked = GUI.Button (new Rect (editorX + i * 62, editorY, 60, 20), text, editorButtonActiveStyle);
				if (buttonClicked) {

					// load
					if (i == 0) {
						//stateSpecialEditor="";
						ClearLevel ();
						LoadLevel (actualLevel);
						// AddToEditorHistory("LoadLevel");
					}

					// save
					if (i == 1) {
						//stateSpecialEditor="";
					}

					if (i == 2) { 
						//stateSpecialEditor="";
						ClearLevel ();  
						// NewLevel();
						DefaultElements();
						SaveLevel (actualLevel);

					}

					if (i == 3) { 
						//stateSpecialEditor="";
						NewLevel();
						SaveLevel (actualLevel);

					}
				}
				
			}
			*/

			/*
			// error log
			// editorLogText="";
			GUI.Label (new Rect (editorX + 250, editorY, 150, 20), "" + editorLogText);
			*/
			/*
			// camera
			editorX = 10;
			editorY = editorY + 26;
			*/

			/*
			// OVERLAY
			GUIStyle activestyle = editorButtonStyle;
			if (cameraOverlayTypes)  activestyle = editorButtonActiveStyle;
			if (GUI.Button (new Rect (editorX + widthWorking, editorY, 38, 20), "INFO ", activestyle )) {
				cameraOverlayTypes=!cameraOverlayTypes;
				// SetCameraY(0.0f);
			}
			widthWorking=widthWorking+40;
			// OVERLAY
			if (GUI.Button (new Rect (editorX + widthWorking, editorY, 38, 20), "EVAL ", editorButtonActiveStyle)) {
				editorLogText = "L. EVALUATIONS!";
				DeleteAllRelationVisuals ();
				ToggleShowEvaluationData();
			}
/*
			widthWorking=widthWorking+44;
			if (GUI.Button (new Rect (editorX + widthWorking, editorY, 38, 20), "CAM ", editorButtonActiveStyle)) {
				// SetCameraZoom(0);
				// SetCameraY(0.0f);
				ResetRotation();
			}

			// direct 
			widthWorking=widthWorking+40;
			directMouseInputX = editorX+widthWorking;
			directMouseInputY = editorY;
			// < >				
			GUI.Label (new Rect (editorX + widthWorking, editorY, 18, 20), " /\\", editorButtonActiveStyle);
			widthWorking=widthWorking+20;
			GUI.Label (new Rect (editorX + widthWorking, editorY, 18, 20), " \\/", editorButtonActiveStyle);
			widthWorking=widthWorking+20;
			GUI.Label (new Rect (editorX + widthWorking, editorY, 18, 20), "<", editorButtonActiveStyle);
			widthWorking=widthWorking+20;
			GUI.Label (new Rect (editorX + widthWorking, editorY, 18, 20), " >", editorButtonActiveStyle);
			widthWorking=widthWorking+20;
			GUI.Label (new Rect (editorX + widthWorking, editorY, 18, 20), " ^", editorButtonActiveStyle);
			widthWorking=widthWorking+20;
			GUI.Label (new Rect (editorX + widthWorking, editorY, 18, 20), "\\/", editorButtonActiveStyle);
			widthWorking=widthWorking+20;
			GUI.Label (new Rect (editorX + widthWorking, editorY, 18, 20), "  -", editorButtonActiveStyle);
			widthWorking=widthWorking+20;
			GUI.Label (new Rect (editorX + widthWorking, editorY, 18, 20), "__", editorButtonActiveStyle);
			widthWorking=widthWorking+20;
*/

			// INFO

/*
			// levels
			int startx=-2;
			for (int i=startx; i<5; i++) {
				string text = "" + i;
				// actualLevel
				GUIStyle gui = editorButtonStyle;
				
				bool buttonClicked = GUI.Button (new Rect (editorX + widthWorking + 90 + (i - startx - 1 +1) * 23, editorY, 20, 20), text, gui);
				if (buttonClicked) {
					// SetCameraZoom(i); // i
					SetCameraZoom ( i );
				}
			}
*/

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
				bool buttonClicked = GUI.Button (new Rect (toolsXTmp, toolsYTmp, 66, 20), text, gui);
				if (buttonClicked) {

					// default deselect
					editorSelected=null;

					// set the tool
					SetTool (tool);
				}

				toolsXTmp = toolsXTmp + 68;
			}
			// toolsYTmp = toolsYTmp + 24;

			toolsXTmp = toolsXTmp + 6;

			// UNDO/REDO
			if (GUI.Button (new Rect (toolsXTmp, toolsYTmp, 48, 20), "UNDO", editorButtonActiveStyle)) {
				Undo();
			}  
			toolsXTmp = toolsXTmp + 50;
			ArrayList arrx = GetActualEditorHistory();
			if (GUI.Button (new Rect (toolsXTmp, toolsYTmp, 38, 20), "" + (arrx.Count-historyIndexMinus) + "/" + arrx.Count, editorButtonStyle)) {
				Redo();
			}
			toolsXTmp = toolsXTmp + 40;
			if (GUI.Button (new Rect (toolsXTmp, toolsYTmp, 40, 20), "REDO", editorButtonActiveStyle)) {
				Redo();
			}
			toolsXTmp = toolsXTmp + 54;

			toolsYTmp = toolsYTmp + 22;


			if (toolsXTmp>toolsXTmpMax) toolsXTmpMax = toolsXTmp;


			toolsRect.x = toolsX;
			toolsRect.y = toolsY;
			toolsRect.width = toolsXTmpMax-toolsX;
			toolsRect.height = toolsYTmp - toolsY;

			// inspector ... 
			// background
			GUI.Label ( new Rect(inspectorRect.x-5,inspectorRect.y-5,inspectorRect.width+5,inspectorRect.height+10), "", editorInspectorBackground);
			int inspectorXTmp = inspectorX ;
			int inspectorYTmp = inspectorY;

			if (GUI.Button (new Rect(inspectorXTmp,inspectorYTmp,200,20),""+editorTool,editorButtonStyleBig)) {
				SetSelectedElementFromGUI();
			}
			inspectorYTmp = inspectorYTmp +40;

			// RASTER & ROTATE
			if (editorTool.Equals("CREATE")) {

				// name
				GUI.Label (new Rect(inspectorXTmp,inspectorYTmp,160,20),"NAME: ",guiEvaluation);
				createName=GUI.TextField (new Rect(inspectorXTmp+120,inspectorYTmp,200,20),createName);

				inspectorXTmp = 10;
				inspectorYTmp = inspectorYTmp + 22;

				inspectorYTmp = inspectorYTmp + 10;

				// rasters
				GUI.Button (new Rect (inspectorXTmp , inspectorYTmp, 58, 20), "RASTER", editorButtonStyle);
				inspectorXTmp = inspectorXTmp + 70;
				for (int i=0; i<arrRasters.Length; i++) {
					float raster = arrRasters [i];
					string text = ""+raster;
					GUIStyle gui = editorButtonStyle;
					if (editorRaster==i) {
						gui = editorButtonActiveStyle;
						text = "" + text + "";
					}
					bool buttonClicked = GUI.Button (new Rect (inspectorXTmp , inspectorYTmp, 22, 20), text, gui);
					if (buttonClicked) {
						SetRasterIndex ( i );
					}

					inspectorXTmp = inspectorXTmp + 24;
				}
				// raster rotation
				inspectorXTmp = inspectorXTmp + 24;

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
						bool buttonClicked = GUI.Button (new Rect (inspectorXTmp , inspectorYTmp, 22, 20), text, gui);
						if (buttonClicked) {
							editorDegree = i * 15;
						}

						inspectorXTmp = inspectorXTmp + 24;
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
					editorSelected = editorLastTouchedGameElement;
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
				if (GUI.Button (new Rect(inspectorXTmp,inspectorYTmp-28,60,20),"DELETE",editorButtonStyle)) {
					RemoveElement(editorSelected);
					editorSelected = null;
					// add to editor history
					AddToEditorHistory("[GUI][OBJECT][DELETE]");

				}


				// inspectorYTmp = inspectorYTmp + 24; 
				inspectorXTmp = 10;

					// specials refer type?
					string release=""+editorSelected.release;
					string add="";
					if (GUI.Button (new Rect(inspectorXTmp,inspectorYTmp,58,20),add+"STATE: ",editorButtonStyle)) {
						editorSelected.release="";
						AddToEditorHistory();
					}
					inspectorXTmp= inspectorXTmp + 60;
					if (release.Equals ("")) { add=">"; }
					if (GUI.Button (new Rect(inspectorXTmp,inspectorYTmp,58,20),add+"active",editorButtonStyle)) {
						editorSelected.release="";
						AddToEditorHistory();
					}
					inspectorXTmp= inspectorXTmp + 60;
					add = "";
					if (release.Equals ("wait")) { add=">"; }
					if (GUI.Button (new Rect(inspectorXTmp,inspectorYTmp,58,20),add+"hidden",editorButtonStyle)) {
						editorSelected.release="wait";
						AddToEditorHistory();
					}
					inspectorXTmp= inspectorXTmp + 60;
					inspectorYTmp = inspectorYTmp + 30; 

					inspectorXTmp = 10;

				// name
				GUI.Label (new Rect(inspectorXTmp,inspectorYTmp,240,20),"NAME: #");
				editDetailName=GUI.TextField (new Rect(inspectorXTmp+50,inspectorYTmp,170,20),editDetailName);
				if (!editorSelected.name.Equals(editDetailName)) {
					editorSelected.name=editDetailName + "";
					UpdateElementVisual(editorSelected);
					AddToEditorHistory("[GUI][OBJECT][CHANGEDNAME]");
				}
				editorSelected.name=editDetailName + "";
				inspectorXTmp = inspectorXTmp +218;


				if (GUI.Button (new Rect(inspectorXTmp,inspectorYTmp,100,20),"DUPLICATE ++",editorButtonStyle)) {
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

				if (GUI.Button (new Rect(inspectorXTmp,inspectorYTmp,200,20),"TYPE: "+editorSelected.type+"/"+editorSelected.subtype,editorButtonStyle)) {

				}

				inspectorXTmp = inspectorXTmp +210;

				if (GUI.Button (new Rect(inspectorXTmp,inspectorYTmp,60,20),"SAME >",editorButtonStyle)) {
					filterType = editorSelected.type;
					filterTypeSub = editorSelected.subtype;
				}

				inspectorYTmp = inspectorYTmp + 22;

				
				
				}
			}

			// show elements
			if ((showElements)||(showElementsSubTypeOnly)) {

				inspectorXTmp = 10;

				GUI.Label (new Rect (inspectorXTmp, inspectorYTmp, 120, 20), "TYPES/SUBTYPES: ", editorButtonStyle);

				inspectorYTmp = inspectorYTmp + 22;

				// wrap
				int maxXToWrap = 450 - 60;

				string selectedEditorArea=""+editorArea;

				// categories
				if (!showElementsSubTypeOnly) {
					ArrayList arrTypesUnique = GetElementTypesUnique ();
					int inspectorXTmpTemp = inspectorXTmp;
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
							text = ">" + text;

						}
						bool buttonClicked = GUI.Button (new Rect (inspectorXTmpTemp, inspectorYTmp, 58, 20), text, guix);
						if (buttonClicked) {
							// do it ...
							// editorArea=ieditorArea;
							if (editorTool.Equals ("CREATE")) {  SetEditorArea (ieditorArea); SetSubEditorArea (unique.subtype);  }
							if (editorTool.Equals ("EDIT")) { 
								editorSelected.type=unique.type; 
								editorSelected.subtype=unique.subtype; 
								// getsize etc ... 
								editorSelected.ChangeTypeInEditMode(unique);
								// attention

								UpdateElementVisual(editorSelected); 
								AddToEditorHistory();
							}
						}
						// delete objects
						// CountElementsType( string elementArea, string elementSubArea )
						inspectorXTmpTemp = inspectorXTmpTemp + 60;
						if (inspectorXTmpTemp>maxXToWrap) {
							inspectorXTmpTemp = 10;
							inspectorYTmp = inspectorYTmp + 21;
						}

					}
					inspectorYTmp = inspectorYTmp + 22;
				}

				// show subcategories!
				ArrayList arr = GetElementTypes (selectedEditorArea);
				int inspectorXTmpXTemp = 10;
				for (int a=0; a<arr.Count; a++) {
					GameElement gelement = (GameElement)arr [a];
					string text = "" + gelement.subtype;
					GUIStyle guix = editorButtonTypeSubStyleNotActive;
					string selectedEditorSubArea=""+editorSubArea;
					if (editorTool.Equals ("EDIT")) {  if (editorSelected!=null) { selectedEditorSubArea=editorSelected.subtype;  } }
					if (selectedEditorSubArea.Equals (gelement.subtype)) {
						guix = editorButtonTypeSubStyle;
						text = ">" + text;
					}
					int count=CountElementsType( gelement.type, gelement.subtype );
					if (count>0) { if (GUI.Button (new Rect (inspectorXTmpXTemp , inspectorYTmp, 6, 20), "x", guix)) { RemoveElementsType(gelement.type, gelement.subtype);} }
					string strCount="";
					if (count>0) strCount="("+count+")";
					bool buttonClicked = GUI.Button (new Rect (inspectorXTmpXTemp+5, inspectorYTmp, 70, 20), text+strCount, guix);
					if (buttonClicked) {
						// do it ...
						if (editorTool.Equals ("CREATE")) {  
							SetSubEditorArea (gelement.subtype); 
							if (gelement.editorTileSize!=0.0f) {
								editorRaster = GetRasterIndexFor(gelement.editorTileSize);
							}
						}
						if (editorTool.Equals ("EDIT")) {  
							editorSelected.type=gelement.type; 
							editorSelected.subtype=gelement.subtype;  
							// showElementsSubTypeOnly
							UpdateElementVisual(editorSelected); 
							AddToEditorHistory();
						}

					}

					inspectorXTmpXTemp = inspectorXTmpXTemp + 80;
					if (inspectorXTmpXTemp>maxXToWrap) {
						inspectorXTmpXTemp = 10;
						inspectorYTmp = inspectorYTmp + 21;
					}
				}

				inspectorYTmp = inspectorYTmp + 25;


			}



			// EDIT
			if (editorTool.Equals ("EDIT")) { 
				if (editorSelected!=null) {
					showElements=true; 
					 
					float editorDetailY=inspectorY;

					// show size
					inspectorXTmp = 10;
					inspectorYTmp = inspectorYTmp + 10;
					float[] arrScales = { 0.2f, 0.5f, 0.75f, 1.0f, 1.5f, 2.0f, 3.0f, 4.0f, 8.0f };
					GUI.Button (new Rect (inspectorXTmp, inspectorYTmp, 60, 20), "SIZE", editorButtonStyle );
					for (int i=0; i<arrScales.Length; i++) {
						float size = arrScales [i];
						string text = ""+size;
						GUIStyle gui = editorButtonStyle;
						if (editorSelected.size==size) {
							gui = editorButtonActiveStyle;
							text = ">" + text + "";
						}
						bool buttonClicked = GUI.Button (new Rect (inspectorXTmp + 62 + i * 24, inspectorYTmp, 22, 20), text , gui );
						if (buttonClicked) {
							editorSelected.size =size;
							UpdateElementVisual( editorSelected );
							AddToEditorHistory("[GUI][OBJECT][SIZE]"+size);
						}
					}

					inspectorYTmp = inspectorYTmp + 10;

					inspectorYTmp = inspectorYTmp + 22; 

					/*
					if (GUI.Button (new Rect(inspectorXTmp,inspectorYTmp,200,20),""+editorTool,editorButtonStyleBig)) {
						SetSelectedElementFromGUI();
					}

					inspectorXTmp = inspectorXTmp +200;
					*/

					inspectorXTmp = 10;

					// trigger (add some keys)
					if (editorSelected!=null)
					if (editorSelected.type.Equals("trigger")) {
						GUI.Label (new Rect(inspectorXTmp,inspectorYTmp,40,24),"Event:");
						editorSelected.strevent=GUI.TextField (new Rect(inspectorXTmp+42,inspectorYTmp,160,20),editorSelected.strevent);
						inspectorYTmp=inspectorYTmp+22;

						GUI.Label (new Rect(inspectorXTmp,inspectorYTmp,40,24),"Target:");
						editorSelected.target=GUI.TextField (new Rect(inspectorXTmp+42,inspectorYTmp,160,20),editorSelected.target);
						inspectorYTmp=inspectorYTmp+22;

						GUI.Label (new Rect(inspectorXTmp,inspectorYTmp,40,24),"After:");
						string strTimed =GUI.TextField (new Rect(inspectorXTmp+42,inspectorYTmp,160,20),""+editorSelected.timed);
						if (!strTimed.Equals(""+editorSelected.timed)) {
							editorSelected.timed  =   float.Parse( strTimed );
							AddToEditorHistory()	;					
						}
						inspectorYTmp=inspectorYTmp+22;
					}

					if (editorSelected!=null) {	
						if (editorSelected.guiBoolArgument) {
							GUI.Label (new Rect(inspectorXTmp,inspectorYTmp,40,24),""+editorSelected.guiLabel+":");
							editDetailArgument=GUI.TextField (new Rect(inspectorXTmp+42,inspectorYTmp,160,20),editDetailArgument);
							bool changed=false;
							if (editDetailArgument!=editorSelected.argument) {
								changed=true;
								AddToEditorHistory();
							}
							editorSelected.argument=editDetailArgument;
							inspectorYTmp=inspectorYTmp+22*1;
							if (changed) {
								UpdateElementVisual(editorSelected);
								UpdateRelationVisualisationAndCheckError();
								AddToEditorHistory();
							}

						}
						// description?
						if (!editorSelected.guiDescription.Equals ("")) {
							GUI.Label (new Rect(inspectorXTmp+42,inspectorYTmp,240,20),editorSelected.guiDescription);
							inspectorYTmp=inspectorYTmp+22*1;
						}

					}


					/*
					// inspectorXTmp=inspectorXTmp + 62 + 5 + arrScales.Length*24;
					if (GUI.Button (new Rect (inspectorXTmp, inspectorYTmp, 64, 20), "TOP:", editorButtonStyle )) {
						editorSelected.rotation =0.0f;
						UpdateElementVisual( editorSelected );
						AddToEditorHistory("[GUI][OBJECT][Y]Reset");
					}
					inspectorXTmp = inspectorXTmp +64;
					if (GUI.Button (new Rect (inspectorXTmp, inspectorYTmp, 18, 20), "-", editorButtonStyle )) {
						editorSelected.position.y =editorSelected.position.y - 0.1f;
						UpdateElementVisual( editorSelected );
						AddToEditorHistory("[GUI][OBJECT][Y]-");
					}
					inspectorXTmp=inspectorXTmp+20;
					if (GUI.Button (new Rect (inspectorXTmp, inspectorYTmp, 38, 20), ""+editorSelected.position.y, editorButtonStyle )) {
						//editorSelected.rotation =editorSelected.rotation - 10.0f;
						//UpdateElementVisual( editorSelected );

					}
					inspectorXTmp=inspectorXTmp+40;
					if (GUI.Button (new Rect (inspectorXTmp, inspectorYTmp, 20, 20), "+", editorButtonStyle )) {
						editorSelected.position.y =editorSelected.position.y + 0.1f;
						UpdateElementVisual( editorSelected );
						AddToEditorHistory("[GUI][OBJECT][Y]+");
					}
					inspectorXTmp = inspectorXTmp +42;

					inspectorXTmp = 10;
					inspectorYTmp = inspectorYTmp + 22;
					// rotations
					// inspectorXTmp=inspectorXTmp + 62 + 5 + arrScales.Length*24;
					if (GUI.Button (new Rect (inspectorXTmp, inspectorYTmp, 38, 20), "ROT:", editorButtonStyle )) {
						editorSelected.rotation =0.0f;
						UpdateElementVisual( editorSelected );
						AddToEditorHistory("[GUI][OBJECT][RX]0");
					}
					inspectorXTmp=inspectorXTmp+40;
					if (GUI.Button (new Rect (inspectorXTmp, inspectorYTmp, 8, 20), "<", editorButtonStyle )) {
						editorSelected.rotation =editorSelected.rotation + 10.0f;
						UpdateElementVisual( editorSelected );
						AddToEditorHistory("[GUI][OBJECT][RX]<");
					}
					inspectorXTmp=inspectorXTmp+10;
					if (GUI.Button (new Rect (inspectorXTmp, inspectorYTmp, 28, 20), ""+editorSelected.rotation, editorButtonStyle )) {
						//editorSelected.rotation =editorSelected.rotation - 10.0f;
						//UpdateElementVisual( editorSelected );
					}
					inspectorXTmp=inspectorXTmp+30;
					if (GUI.Button (new Rect (inspectorXTmp, inspectorYTmp, 8, 20), ">", editorButtonStyle )) {
						editorSelected.rotation =editorSelected.rotation - 10.0f;
						UpdateElementVisual( editorSelected );
						AddToEditorHistory("[GUI][OBJECT][RX]>");
					}
					inspectorXTmp = inspectorXTmp +10;
					if (GUI.Button (new Rect (inspectorXTmp, inspectorYTmp, 10, 20), "~", editorButtonStyle )) {
						editorSelected.rotation = UnityEngine.Random.Range(0.0f,360.0f);
						UpdateElementVisual( editorSelected );
						AddToEditorHistory("[GUI][OBJECT][RX]RND");
					}
					inspectorXTmp = inspectorXTmp +12;
					*/ 



					/*
					inspectorXTmp=inspectorXTmp + 62 + 5 + arrScales.Length*24;
					float[] arrRotations = { 0.0f, 30.0f, 60.0f, 90.0f };
					GUI.Button (new Rect (inspectorXTmp, inspectorYTmp, 60, 20), "ROT", editorButtonStyle );
					for (int i=0; i<arrRotations.Length; i++) {
						float rotation = arrRotations [i];
						string text = ""+rotation;
						GUIStyle gui = editorButtonStyle;
						if (editorSelected.rotation==rotation) {
							gui = editorButtonActiveStyle;
							text = ">" + text + "";
						}
						bool buttonClicked = GUI.Button (new Rect (inspectorXTmp + 62 + i * 24, inspectorYTmp, 22, 20), text , gui );
						if (buttonClicked) {
							editorSelected.rotation =rotation;
							UpdateElementVisual( editorSelected );
						}
					}
					*/

					/*
					* GUI.Label (new Rect(editorDetailX,editorDetailY,40,24),""+editorSelected.guiLabel+":");
					editDetailArgument=GUI.TextField (new Rect(editorDetailX+42,editorDetailY,160,20),editDetailArgument);
					editorSelected.argument=editDetailArgument;

					GUI.Label (new Rect(inspectorXTmp,inspectorYTmp,40,24),""+editorSelected.guiLabel+":");
					editDetailArgument=GUI.TextField (new Rect(inspectorXTmp+42,inspectorYTmp,160,20),editDetailArgument);
					editorSelected.argument=editDetailArgument;
					*/

					inspectorYTmp = inspectorYTmp + 22;
				}
			}
			// if (editorTool.Equals ("MOVE")) { showElements=true; }

			if (editorTool.Equals ("SPLIT")) { 

				// editorToolSub
				string[] arrEditorToolsSub={"right","left","down","up"};
				for (int i=0;i<arrEditorToolsSub.Length;i++) {

					string add="";
					string title=""; 
					GUIStyle gs = editorButtonStyle;
					if (i==0) { title = "ADD |>"; if (editorToolSub.Equals ("right")) { add=">";  gs= editorButtonActiveStyle; } }
					if (i==1) { title = "|< DEL"; if (editorToolSub.Equals ("left")) { add=">"; gs= editorButtonActiveStyle; } }
					if (i==2) { title = "ADD ^"; if (editorToolSub.Equals ("down")) { add=">"; gs= editorButtonActiveStyle;   } }
					if (i==3) { title = "DEL ^"; if (editorToolSub.Equals ("up")) { add=">"; gs= editorButtonActiveStyle; } }

					if (GUI.Button (new Rect (inspectorXTmp + i*60, inspectorYTmp, 58, 20), add+""+title, gs )) {
						editorToolSub	= arrEditorToolsSub[i];	
					}

				}
				inspectorYTmp = inspectorYTmp + 22;
			}

			// EVALUATION
			if (editorTool.Equals ("EVALU")) { 


				string evData="VISUALIZE ";
				string evDataOn = "ON";
				if (!editorShowEvaluationData) evDataOn = "OFF";
				if (GUI.Button (new Rect (inspectorXTmp + 0*60, inspectorYTmp, 118, 20), evData + evDataOn, editorButtonStyle )) {
					ToggleShowEvaluationData(); // editorShowEvaluationData
				}

				if (GUI.Button (new Rect (inspectorXTmp + 2*60, inspectorYTmp, 58, 20), "RELOAD", editorButtonStyle )) {
					UpdateShowEvaluationData();
				}

				// FILTERS ...


				inspectorYTmp = inspectorYTmp + 22;

				// PLAYERS
				int dtx = inspectorXTmp; 
				string addOnX = "";
				if (editorEvaluationFilter.Equals("all")) { addOnX = ">"; } 
				if (GUI.Button (new Rect (inspectorXTmp + 0*60, inspectorYTmp, 58, 20), addOnX+"ALL", editorButtonStyle )) {

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
							addOn = ">";
						}
						if (GUI.Button (new Rect (dtx + (o+1)*60, inspectorYTmp, 58, 20), addOn+"" + pl.name, editorButtonStyle )) {
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
				if (editorEvaluationSessionId.Equals("")) { addOnX = ">"; } 
				if (GUI.Button (new Rect (inspectorXTmp + 0*60, inspectorYTmp, 58, 20), addOnX+"ALL", editorButtonStyle )) {

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
						addOn = ">";
					}
					if (GUI.Button (new Rect (dtx + (o+1)*60, inspectorYTmp, 58, 20), addOn+"/" + el.evaluationSessionId, editorButtonStyle )) {

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
			GUI.Label ( new Rect(cursorRect.x-5,cursorRect.y-5,cursorRect.width+5,cursorRect.height+10), "", editorBackground);


			// transform
			inspectorXTmp = cursorX;
			inspectorYTmp = cursorY;
			if (arrSensButtons.Count==0) {
				AddSensButton(inspectorXTmp,inspectorYTmp, 28,28, "|", "rotateforward" );
				inspectorXTmp = inspectorXTmp + 30;
				AddSensButton(inspectorXTmp,inspectorYTmp, 28,28, "\\", "rotateleft" );
				inspectorXTmp = inspectorXTmp + 30;
				AddSensButton(inspectorXTmp,inspectorYTmp, 28,28, "^", "up" );
				inspectorXTmp = inspectorXTmp + 30;
				AddSensButton(inspectorXTmp,inspectorYTmp, 28,28, "/", "rotateright" );
				inspectorXTmp = inspectorXTmp + 30;
				AddSensButton(inspectorXTmp,inspectorYTmp, 28,28, "-", "forward" );
				inspectorYTmp = inspectorYTmp + 30; 
				inspectorXTmp = cursorX; 
				AddSensButton(inspectorXTmp,inspectorYTmp, 28,28, "||", "rotatebackward" );
				inspectorXTmp = inspectorXTmp + 30;
				AddSensButton(inspectorXTmp,inspectorYTmp, 28,28, "<", "left" );
				inspectorXTmp = inspectorXTmp + 30;
				AddSensButton(inspectorXTmp,inspectorYTmp, 28,28, "u", "down" );
				inspectorXTmp = inspectorXTmp + 30;
				AddSensButton(inspectorXTmp,inspectorYTmp, 28,28, ">", "right" );
				inspectorXTmp = inspectorXTmp + 30;
				AddSensButton(inspectorXTmp,inspectorYTmp, 28,28, "_", "backward" );

			}

			if (true) {
				// render the specials
				SensButton sb;
				// Debug.Log("arrSensButtons.Count: "+arrSensButtons.Count);
				for (int i=0;i<arrSensButtons.Count;i++) {
					sb = (SensButton) arrSensButtons[i];
					GUI.Label ( sb.rect, ""+sb.text, editorButtonStyle);


				}

			}

			inspectorXTmp = cursorX;
			inspectorYTmp = cursorY + 60; 
			// inspectorYTmp = inspectorXTmp;

			// 1-4
			//				inspectorYTmp = inspectorYTmp + 30; 
			//				inspectorXTmp = cursorX; 
			for (int y=0;y<4;y++) {
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
				if (GUI.Button (new Rect (inspectorXTmp, inspectorYTmp, 28, 20), "x"+(y+1), guixx)) {
					if (!cursorObject) speedCamera = val;
					if (cursorObject) speedObject = val;
				}
				inspectorXTmp = inspectorXTmp + 30;
			}

			// create cursor
			inspectorXTmp = cursorX+150;
			inspectorYTmp = cursorY;
			GUIStyle guixxx = editorButtonStyleNotActive;
			if (!cursorObject) {
				guixxx = editorButtonActiveStyle;
			}
			if (GUI.Button (new Rect (inspectorXTmp, inspectorYTmp, 34, 28), "CAM", guixxx)) {
				cursorObject = false;
			}
			inspectorYTmp = inspectorYTmp + 30;
			guixxx = editorButtonStyleNotActive;
			if (cursorObject) {
				guixxx = editorButtonActiveStyle;
			}
			if (GUI.Button (new Rect (inspectorXTmp, inspectorYTmp, 34, 28), "OBJ", guixxx)) {
				cursorObject = true;
			}
			cursorRect.x = cursorX;
			cursorRect.y = cursorY;
			cursorRect.width = 188;
			cursorRect.height = 80;
 			

			// setup height
			// editorHeight = editorY;




			// 	selectiondialoge
			if (selectionDialoge) {


				// FILTER
				// background
				GUI.Label ( new Rect(filterTypeVisual.x,filterTypeVisual.y-5,filterTypeVisual.width+5,filterTypeVisual.height+10), "", editorBackground);
				GUI.Label ( new Rect(filterTypeVisual.x,filterTypeSubVisual.y-5,filterTypeSubVisual.width+5,filterTypeSubVisual.height+10), "", editorBackground);
				GUI.Label ( new Rect(selectionDialogeVisual.x,selectionDialogeVisual.y-5,selectionDialogeVisual.width+5,selectionDialogeVisual.height+10), "", editorBackground);

/*
			bool selectionDialoge = true;
			string selectFilter = ""; // names etc.  #name types ...  !abc.* !
			Rect selectionDialogeVisual = new Rect(0,0,0,0);
			GameElement[] selectionAffectedElements = {};
*/
				int selectionX = Screen.width -140;
				int selectionY = (int) (Screen.height * 0.2f);
				int selectionYStart = selectionY;

				selectionDialogeVisual.x = selectionX;
				selectionDialogeVisual.y = selectionY;
				selectionDialogeVisual.width = 140;


				// ADD IMPORTANT =NAMED THINGS ...


				// show [EDIT][SELECTION]
				ArrayList arrNames = GetGameElementsByNotCleanName();
				GUI.Button (new Rect ( selectionX, selectionY, 140, 20), "NAMES "+arrNames.Count+"/"+arrLevel.Count, editorButtonActiveStyle);
				selectionY = selectionY + 24;
				for (int i=0; i<arrNames.Count; i++) {
					GameElement gae = (GameElement)arrNames [i];
					string text = "" + gae.name;
					if (gae.name.Equals("")) {
						text = text + "("+gae.subtype+")";
					}
					GUIStyle guix = editorButtonStyleNotActive;
					if (editorSelected==gae) guix = editorButtonActiveStyle;
					bool buttonClicked = GUI.Button (new Rect ( selectionX, selectionY, 140, 20), ""+text+"", guix);
					if (buttonClicked) {
						SetSelectedElement(gae);
						SetTool("EDIT");
					}
					selectionY = selectionY + 20;
					if (i>30) break;
				} 
				selectionY = selectionY + 20;


				// nearbyRect
				GUI.Label ( new Rect(nearbyRect.x,nearbyRect.y,nearbyRect.width,nearbyRect.height), "", editorBackground);
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
						// GUI.Button (new Rect ( nearbyXTmp, nearbyYTmp, 140, 20), "NEAR BY ", editorButtonActiveStyle);
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
							bool buttonClicked = GUI.Button (new Rect ( nearbyXTmp, nearbyYTmp, 120, 20), ""+text+"", guix);
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
				// GUI.Button (new Rect ( selectionX+100, selectionY, 240, 20), ""+arr.Count);
				if (arr.Count>0)
				selectionY = (int)(Screen.width*0.3f);
				int counter = 0;
				for (int i=(arr.Count-1);i>=0; i--) {
					LevelHistory gae = (LevelHistory)arr [i];
					string text = "[" + gae.level+"] ("+gae.message+") " + gae.arrLevel.Count;
					GUIStyle guix = editorButtonStyleNotActive;
					// if (editorSelected==gae) guix = editorButtonActiveStyle;
					bool buttonClicked = GUI.Button (new Rect ( selectionX, selectionY, 240, 20), ""+text+"", guix);
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
			// GUI.Label (new Rect(0,Screen.height-20,60,20),"  ["+scrollToShow+"] ",editorComment);

			// editorCursorActualPoint
			Vector3 px = new Vector3();
			px.x = (float)Math.Floor(editorCursorActualPoint.x*10.0f)/10.0f;
			px.y = (float)Math.Floor(editorCursorActualPoint.y*10.0f)/10.0f;
			px.z = (float)Math.Floor(editorCursorActualPoint.z*10.0f)/10.0f;

			GUI.Label (new Rect(0,Screen.height-20,100,20),""+px.x+"/"+px.y+"/"+px.z,editorComment);

			// INFO
			string strSelection="";
			if (editorSelected!=null) {
				strSelection = "[SELECTED ELEMENT: "+editorSelected.type+"/"+editorSelected.subtype+" "+editorSelected.guiDescription+"]";
			}



//			GUI.Label (filterTypeSubVisual, "", editorBackground);

			// filterDev
			int filterY = Screen.height-80;
			int filterX = 0;

			// NAMES
			filterX = filterX + 0;
			GUIStyle activestylext = editorButtonStyle;
			if (cameraOverlayTypes)  activestylext = editorButtonActiveStyle;
			if (GUI.Button (new Rect (filterX,filterY-24, 50, 20), "NAMES ", activestylext )) {
				cameraOverlayTypes=!cameraOverlayTypes;
			}
			filterX = filterX + 60;
			if (GUI.Button (new Rect(62,filterY-24,140,20),"NOTIFICATIONS" ,editorComment)) {
				notificationDialog = !notificationDialog;
			}

			filterX = 0;

			GUIStyle xyz = editorButtonActiveStyle;
			if (filterType.Equals(""))  xyz = editorButtonStyle;
			if (GUI.Button (new Rect(0,filterY,60,20),"VIEW *.* " ,xyz)) {
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
						guix = editorButtonStyle;
					}
				bool buttonClicked = GUI.Button (new Rect ( filterX + i * 80, filterY, 78, 20), ""+text+".*", guix);
					if (buttonClicked) {
						filterType = unique.type;
						filterTypeSub = "*";
					}
					// delete objects
					// CountElementsType( string elementArea, string elementSubArea )
				}
				filterX = filterX + arrTypesUniqueX.Count*80;
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
					if (filterTypeSub.Equals(gelement.subtype)) guix = editorButtonStyle;
					bool buttonClicked = GUI.Button (new Rect ( filterX + offset + a * 60, filterY, 58, 20), "."+text, guix);
					if (buttonClicked) {
						filterTypeSub = gelement.subtype;
					}
				}	

				filterX = 80 + arr.Count * 60;

				filterTypeSubVisual.x = 0;
				filterTypeSubVisual.width = filterX;
				filterTypeSubVisual.y = filterY;
				filterTypeSubVisual.height = 22;

			}

			// NOTIFICATIONS
			if (notificationDialog) {
				notificationDialogX = (int) (Screen.width * 0.2f);
				notificationDialogY = (int) (Screen.height * 0.2f);
				notificationVisual.x = notificationDialogX;
				notificationVisual.y = notificationDialogY;
				notificationVisual.width = 150;
				// notificationVisual.height = 50;
				GUI.Label ( new Rect(notificationVisual.x-5,notificationVisual.y-5,notificationVisual.width+10,notificationVisual.height+10), "", editorBackground);

				int notificationDialogXTmp = notificationDialogX;
				int notificationDialogYTmp = notificationDialogY;

				ArrayList arr = notificationCenter.GetNotificationTypesUnique();

				if (GUI.Button (new Rect ( notificationDialogXTmp, notificationDialogYTmp, notificationVisual.width, 20), "ALL NOTIFICATIONS *.* ", editorButtonActiveStyle)) {
					notificationArea = "";
				}
				notificationDialogYTmp = notificationDialogYTmp + 22;

				strNotification = GUI.TextField (new Rect ( notificationDialogXTmp, notificationDialogYTmp, notificationVisual.width, 20), ""+strNotification, editorButtonActiveStyle) ;
				notificationDialogYTmp = notificationDialogYTmp + 22;

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
							bool buttonClicked = GUI.Button (new Rect ( notificationDialogXTmp, notificationDialogYTmp, notificationVisual.width, 20), ""+text+"", guix);
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
								bool buttonClicked = GUI.Button (new Rect ( notificationDialogXTmp, notificationDialogYTmp, notificationVisual.width, 20), " "+text+"", guix);
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

				notificationDialogX = (int) (Screen.width * 0.2f) + 220;
				notificationDialogY = (int) (Screen.height * 0.2f);
				// GUI.Label ( new Rect(notificationVisual.x-5,notificationVisual.y-5,notificationVisual.width+10,notificationVisual.height+10), "", editorBackground);
				notificationDialogXTmp = notificationDialogX;
				notificationDialogYTmp = notificationDialogY;

				if (notificationTesterAndHistory) {
				bool buttonClickedX = GUI.Button (new Rect ( notificationDialogXTmp, notificationDialogYTmp, notificationVisual.width, 20), "ADD NOTIFICATION", editorButtonActiveStyle);
				if (buttonClickedX) {
					float timed = float.Parse( fieldTimed );
						notificationCenter.AddNotification(fieldType,fieldTypeSub,fieldTarget,timed,fieldArgument, new Vector3());
				}
				notificationDialogYTmp = notificationDialogYTmp + 22;
				fieldType = GUI.TextField (new Rect ( notificationDialogXTmp, notificationDialogYTmp, notificationVisual.width, 20), ""+fieldType, editorButtonStyle) ;
				notificationDialogYTmp = notificationDialogYTmp + 22;
				fieldTypeSub = GUI.TextField (new Rect ( notificationDialogXTmp, notificationDialogYTmp, notificationVisual.width, 20), ""+fieldTypeSub, editorButtonStyle) ;
				notificationDialogYTmp = notificationDialogYTmp + 22;
				fieldTarget = GUI.TextField (new Rect ( notificationDialogXTmp, notificationDialogYTmp, notificationVisual.width, 20), ""+fieldTarget, editorButtonStyle) ;
				notificationDialogYTmp = notificationDialogYTmp + 22;
				fieldTimed = GUI.TextField (new Rect ( notificationDialogXTmp, notificationDialogYTmp, notificationVisual.width, 20), ""+fieldTimed, editorButtonStyle) ;
				notificationDialogYTmp = notificationDialogYTmp + 22;
				fieldArgument = GUI.TextField (new Rect ( notificationDialogXTmp, notificationDialogYTmp, notificationVisual.width, 20), ""+fieldArgument, editorButtonStyle) ;
				notificationDialogYTmp = notificationDialogYTmp + 22;

				// add history ...
				// Debug.Log("LevelEditor.OnGUI(); // "+notificationCenter.arrNotificationPipline.Count);
				notificationDialogYTmp = notificationDialogYTmp + 22;

				if (notificationCenter.arrNotificationPipline.Count>0) {
				for (int i=(notificationCenter.arrNotificationPipline.Count-1);i>=0;i--) {
					Notification nt = (Notification)notificationCenter.arrNotificationPipline [i];
							string text = "[" + nt.state+"] "+nt.type+"/"+nt.subtype+" {>"+nt.targetName+"} "+nt.argument+" ("+nt.timed+")";
					GUIStyle guix = editorButtonStyleNotActive;
					GUI.Label (new Rect ( notificationDialogXTmp, notificationDialogYTmp, notificationVisual.width*2, 20), " "+text+"", guix);
					notificationDialogYTmp = notificationDialogYTmp + 22;
				}
				}
				}

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

			GUI.Label (new Rect(101,Screen.height-20,Screen.width,20),"CAMERA: Use <awsd> for moving <qe> rotate side <rf> up/down <x2> lookup/down <c> reset  OBJECT: + <shift> | Objs: ["+arrLevel.Count+"] Elements "+ strSelection ,editorComment);
			  

		}

		if (debugGameElementTypes) {
			string text="";
			for (int i=0; i<arrGameElementTypes.Count; i++) {
				GameElement gaelement = (GameElement)arrGameElementTypes [i];
				string gameobjectname="";
				// if (gaelement.gameObject!=null) gameobjectname=""+gaelement.gameObject.name;
				text=text+"\n"+i+". ("+gaelement.type+"/"+gaelement.subtype+") ["+gameobjectname+"] [s:"+gaelement.strength+"] ";
			}
			GUI.Label (new Rect(0,200,Screen.width,Screen.height),""+text,editorComment);
			
			
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
			GUI.Label (new Rect(400,200,Screen.width,Screen.height),""+text); //  editorComment

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
				float mouseYT=Screen.height-Input.mousePosition.y;
				for (int i=0;i<arrSensButtons.Count;i++) {
					sb = (SensButton) arrSensButtons[i];
					if ((mouseXT>sb.rect.x)&&(mouseXT<(sb.rect.x+sb.rect.width))) {
						if ((mouseYT>sb.rect.y)&&(mouseYT<(sb.rect.y+sb.rect.height))) {
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
				}

				// rot
			}

			GameObject preview = GameObject.Find("editorpreview");
			preview.transform.Rotate(0.0f, 1.0f, 0.0f);

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
				RemoveElement(editorSelected);
				editorSelected = null;
				// add to editor history
				AddToEditorHistory("[GUI][OBJECT][DELETE]");
				}
			}


			// generate new objects
			float mouseX=Input.mousePosition.x;
			float mouseY=Screen.height-Input.mousePosition.y;

			// hot edges
			if ((mouseX<20)||(mouseX>(Screen.width-20))||(mouseY<20)||(mouseY>(Screen.height-20))) {

				// Debug.Log("LeveleEditor.Update() // "+editorTool+" // INEDITOR // HOT EDGES");

				GameObject xcontainer=GameObject.Find ("editorCameraContainer");
				GameObject xeditorcamera=GameObject.Find ("editorcamera");

				if (enabledEditorEdges) 
				if (!CheckMouseInEditor())
				{
					
					
					float rotatex = 1.0f;
					float rotatey = 1.0f;
					if ((mouseY>=0)&&(mouseY<20)) {
						xeditorcamera.transform.Rotate( -rotatey, 0.0f, 0.0f );
					}
					if ((mouseY>(Screen.height-20))&&(mouseY<=(Screen.height))) {
						xeditorcamera.transform.Rotate( +rotatey, 0.0f,  0.0f );
					}

					if ((mouseX>=0)&&(mouseX<20)) {
						xcontainer.transform.Rotate( 0.0f, -rotatex, 0.0f );
					}
					if ((mouseX<=(Screen.width))&&(mouseX>(Screen.width-20))) {
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


	// Update is called once per frame
	void FixedUpdate () {

		// DefaultLight

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

			// no physics
			// Time.timeScale = 0.0f; // move etc is not working too!
			// CAMERA direct input

			Vector3 mouseVec = Input.mousePosition;
			float mouseX = mouseVec.x;
			float mouseY = Screen.height-mouseVec.y;
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
//						DoEditorScroll( 0.0f, 0.0f, -speed.z );
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
							editorSelected.position.x = editorSelected.position.x - vectorMove.x;
							UpdateElementVisual(editorSelected);
						}
						if ((Input.GetKey ("d"))||(Input.GetKey ("right"))) {
							editorSelected.position.x = editorSelected.position.x + vectorMove.x;
							UpdateElementVisual(editorSelected);
						}
						if ((Input.GetKey ("w"))||(Input.GetKey ("up"))) {
							editorSelected.position.z = editorSelected.position.z + vectorMove.z;
							UpdateElementVisual(editorSelected);
						}
						if ((Input.GetKey ("s"))||(Input.GetKey ("down"))) {
							editorSelected.position.z = editorSelected.position.z - vectorMove.z;
							UpdateElementVisual(editorSelected);
						}

						if ((Input.GetKey ("r"))) {
							editorSelected.position.y = editorSelected.position.y + vectorMove.y;
							UpdateElementVisual(editorSelected);
						}
						if ((Input.GetKey ("f"))) {
							editorSelected.position.y = editorSelected.position.y - vectorMove.y;
							UpdateElementVisual(editorSelected);
						}

						// up & down 
						if ((Input.GetKey ("q"))) {
							// scroll = scroll + 0.3f;
							editorSelected.rotation = editorSelected.rotation + 3.0f;
							UpdateElementVisual(editorSelected);
						}
						if ((Input.GetKey ("e"))) {
							editorSelected.rotation = editorSelected.rotation - 3.0f;
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
			LineRenderer lr=vl.GetComponent<LineRenderer>();
			lr.SetPosition (0, elem.position );
			lr.SetPosition (1, elem.position + new Vector3(0.0f,5.0f, 0.0f) );
			arrRelation.Add (vl);
		}
	
		// Path Visuals
		void CreateVisualRelationPath( GameElement elem, Vector3 pointTo ) {
			GameObject vl=Instantiate(lineVisualisationPath, new Vector3(elem.position.x,elem.position.y,elem.position.z), new Quaternion()) as GameObject;
			LineRenderer lr=vl.GetComponent<LineRenderer>();
			lr.SetPosition (0, elem.position );
			lr.SetPosition (1, pointTo );
			arrRelation.Add (vl);
		}
		// Action Visuals
		void CreateVisualRelationAction( GameElement elem, Vector3 pointTo ) {
			GameObject vl=Instantiate(lineVisualisation, new Vector3(elem.position.x,elem.position.y,elem.position.z), new Quaternion()) as GameObject;
			LineRenderer lr=vl.GetComponent<LineRenderer>();
			lr.SetPosition (0, elem.position );
			lr.SetPosition (1, pointTo );
			arrRelation.Add (vl);
		}

	// CreateVisualRelationEvaluation
		void CreateVisualRelationEvaluation( GameElement elem, Vector3 pointTo ) {
			GameObject vl=Instantiate(lineVisualisation, new Vector3(elem.position.x,elem.position.y,elem.position.z), new Quaternion()) as GameObject;
			LineRenderer lr=vl.GetComponent<LineRenderer>();
			lr.SetPosition (0, elem.position );
			lr.SetPosition (1, pointTo );
			arrRelation.Add (vl);
		}

	
	
	
}
