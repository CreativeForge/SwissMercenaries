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



		// state special editor ...
		//string stateSpecialEditor=""; // '' > 'saved'
		// float stateSpecialEditorScroll=0.0f;
		// float stateSpecialEditorScrollY=0.0f;

		// overlay
		bool cameraOverlayTypes = true;

		public GameObject dummyEditorPrefab;

		Vector3 editorCursorActualPoint=new Vector3();

		GameElement editorLastTouchedGameElement = null;

	// load game level (used from GameLogic) 
	public void LoadGameLevel( int newLevel ) {

		actualLevel = newLevel;
		// todo: load level ... 
		ClearLevel ();
		LoadLevel (actualLevel);

		// if (flagEvaluation) {
		// in running mode!
		NewSession (evaluationPlayer);
		//}

	}


	// actual level
	int actualLevel=1;
	int maxLevel=8;

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

	}

	void ClearLevel() {
		ClearElements ();
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


	// GameElement (Definition > own class) 


	// elements types
	public ArrayList arrGameElementTypes=new ArrayList();

	// element types
	public GameElement AddElementType() {
		GameElement gameElement = new GameElement ();
		arrGameElementTypes.Add (gameElement);
		return gameElement;
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
					return gelement;
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
	public LevelElement[] GoalLevelElements= { new LevelElement ("survivetime"), new LevelElement ("killamount")   };
	public LevelElement[] ImmovablesLevelElements = { new LevelElement ("scheune"), new LevelElement ("city"),  new LevelElement ("fountain")    };
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

	// selectiondialoge
	bool selectionDialoge = true;
	string selectFilter = ""; // names etc.  #name types ...  !abc.* !
	Rect selectionDialogeVisual = new Rect(0,0,0,0);
	GameElement[] selectionAffectedElements = {};

	// prefab elements
	// varia 
	public LevelElement[] levelElements = { 

		/* new LevelElement ("waiter"), new LevelElement ("landsknecht") */

	} ;

	// elements
	void InitGameElements() {

		// most important categories
		RegisterLevelElements( "base", BaseLevelElements );
		RegisterLevelElements( "goal", GoalLevelElements );
		RegisterLevelElements( "immovable", ImmovablesLevelElements );
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

				LevelElement el;
				for (int i=0;i<ilevelElements.Length;i++) {
					el = ilevelElements[i];
					
					// add elements
					// if ( el.gameObject != null ) {
						GameElement geType = AddElementType ();
						geType.type = prefix; 
						geType.subtype = el.typetypesub; 
						geType.prefabGameObject = el.gameObject;
						geType.prefabEditorDummyGameObject = el.editorPrefab; // dummy prefab
						// copy and add all prefabs for arguments (evaluation.allover 2,1,0 etc
						geType.prefabEditorDummyArguments = el.prefabEditorDummyArguments; // .Copy(); // all the same reference
						geType.guiBoolArgument = el.argumentNeeded; 
						geType.guiLabel = el.argumentLabel;	 
						geType.guiDescription = el.argumentDescription;

						geType.argumentInputType = el.inputType;
						geType.argumentInputTypeSelect = el.inputTypeSelect;

						geType.guiShowInMenu = visibleInEditor;

						geType.editorDisplaySize = el.editorDisplaySize;


			// }
				}
				
			}

	// Evaluation INGAME
	bool flagEvaluationAvailable = false; // is this availabe at all?
		bool showEvaluationDialog = true;

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
				if (elPrefab==null) { Debug.Log("Error: Could not find Type("+elem.type+"/"+elem.subtype+")");  } 
				if (elPrefab!=null) { 
					// Debug.LogError("Could find Type("+elem.type+"/"+elem.subtype+")");
					// elPrefab.prefabGameObject=
					// create gameobject

					// specials
					// base only 
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
									trb.SetGameElementArgument(argument);
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
	float[] arrRasters = { 0.0f, 0.5f, 1.0f, 2.0f, 4.0f, 8.0f };

	void SetRasterIndex( int index ) { // Index!
		editorRaster = index;
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
		SetSelectedElementToGUI ();
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

		Debug.Log("SetEditorPreviewToPrefab()");

		// find preview
		GameObject preview = GameObject.Find("editorpreview");

		if (preview!=null) {
			// delete old one 
			foreach (Transform child in preview.transform) {
				Destroy(child.gameObject);
				break;
			}

			Debug.Log("SetEditorPreviewToPrefab(){ preFabFound = "+prefab+" }");

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

	public GUIStyle editorBackground;
		public GUIStyle editorButtonStyle;
		public GUIStyle editorButtonStyleNotActive;
		public GUIStyle editorButtonActiveStyle;

		public GUIStyle editorSwitchButtonStyle;
		public GUIStyle editorSwitchButtonStyleActive;
		
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

			// update 
			UpdateShowEvaluationData ();
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

		try {
			
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

		} catch( Exception e ) {
			Debug.LogWarning("_LevelEditor.CouldNotLoadLevel "+level);
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
	int editorHeight=120;

	bool CheckMouseInEditor() {
		float mouseX=Input.mousePosition.x;
		float mouseY=Screen.height-Input.mousePosition.y;

		if ((mouseX>editorPrefX)&&(mouseX<(editorPrefX+editorWidth))&&(mouseY>editorPrefY)&&(mouseY<(editorPrefY+editorHeight))) {
			// Debug.Log ("CheckMouseInEditor() > TRUE; ");
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


	int directMouseInputX = 0;
	int directMouseInputY = 0;

	void OnGUI() {

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
			if (!CountElementsTypeIndexOf("gamelogiccontroller")) { 
				GUI.Label (new Rect (10, Screen.height*0.5f+24, 500, 20), "3. NO GAMELOGIC CONTROLLER > ADD ONE > /BASE/GAME... ", guixt);
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
			}

			// infos about the mouse point (for putting in 

			// editor X / Y
			int editorX=editorPrefX;
			int editorY=editorPrefY;

			GUI.Label (new Rect (editorX, editorY, editorWidth, editorHeight), "", editorBackground);

			// working on level
			// maxLevel
			GUI.Label (new Rect (editorX, editorY, 120, 20), "WORKING LEVEL: ", editorButtonActiveStyle);
			// levels
			for (int i=1; i<maxLevel; i++) {
				string text = "" + i;
				// actualLevel
				GUIStyle gui = editorButtonStyle;
				if (i == actualLevel) {
					gui = editorButtonActiveStyle;
					text = ">" + text + "";
				}

				bool buttonClicked = GUI.Button (new Rect (editorX + 122 + (i - 1) * 22, editorY, 20, 20), text, gui);
				if (buttonClicked) {
					// scroll=0.0f;
					// EditorUpdateCameraToScroll(scroll);
					//stateSpecialEditor="";
					// SetLevel (i);
					gameLogic.SetGameLevel( i );
				}
			}
			int widthWorking = 280;
			// COPYTO
			GUI.Label (new Rect (editorX + widthWorking, editorY, 80, 20), "COPYTO: ", editorButtonActiveStyle);
			// levels
			for (int i=1; i<maxLevel; i++) {
				string text = "" + i;
				// actualLevel
				GUIStyle gui = editorButtonStyle;

				bool buttonClicked = GUI.Button (new Rect (editorX + widthWorking + 82 + (i - 1) * 22, editorY, 20, 20), text, gui);
				if (buttonClicked) {
					SaveLevel (i);
				}
			}
			editorY = editorY + 22;

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
					}

					// save
					if (i == 1) {
						//stateSpecialEditor="";
						SaveLevel (actualLevel);
					}

					if (i == 2) { 
						//stateSpecialEditor="";
						ClearLevel ();  
						// NewLevel();
					}
					if (i == 3) { 
						//stateSpecialEditor="";
						NewLevel();
					}
				}
				
			}
			// error log
			// editorLogText="";
			GUI.Label (new Rect (editorX + 250, editorY, 150, 20), "" + editorLogText);
			// camera
			widthWorking=widthWorking+80;
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

			editorY = editorY + 22;

			// tools
			for (int i=0; i<arrEditorTools.Length; i++) {
				string tool = arrEditorTools [i];
				string text = tool;
				GUIStyle gui = editorButtonStyle;
				if (text.Equals (editorTool)) {
					gui = editorButtonActiveStyle;
					text = "[" + text + "]";
				}
				bool buttonClicked = GUI.Button (new Rect (editorX + i * 68, editorY, 66, 20), text, gui);
				if (buttonClicked) {

					// default deselect
					editorSelected=null;

					// set the tool
					SetTool (tool);
				}
			}
			// rasters
			GUI.Button (new Rect (editorX + arrEditorTools.Length*70+10, editorY, 60, 20), "RASTER", editorButtonStyle);
			for (int i=0; i<arrRasters.Length; i++) {
				float raster = arrRasters [i];
				string text = ""+raster;
				GUIStyle gui = editorButtonStyle;
				if (editorRaster==i) {
					gui = editorButtonActiveStyle;
					text = "" + text + "<";
				}
				bool buttonClicked = GUI.Button (new Rect (editorX + arrEditorTools.Length*70+60+12 + i * 24, editorY, 22, 20), text, gui);
				if (buttonClicked) {
					SetRasterIndex ( i );
				}
			}
			editorY = editorY + 22;


			// 3 possibilites
			bool showElements=false;
			if (editorTool.Equals ("CREATE")) { showElements=true; }
			if (editorTool.Equals ("EDIT")) { 
				if (editorSelected==null) {
					// edit > on edit
					editorSelected = editorLastTouchedGameElement;
					if (editorSelected!=null) {
						if (!filterType.Equals("*")) {
							if (filterType.Equals(editorSelected.type)) {
								
							} else {
								filterType = editorSelected.type;
							}
						}
					}
				}
				if (editorSelected!=null) {
					// showElements=true; 
					editorY = editorY + 22;

					// transform

					// editorX=editorX + 62 + 5 + arrScales.Length*24;
					if (GUI.Button (new Rect (editorX, editorY, 64, 20), "TOP:", editorButtonStyle )) {
						editorSelected.rotation =0.0f;
						UpdateElementVisual( editorSelected );
					}
					editorX=editorX+66;
					if (GUI.Button (new Rect (editorX, editorY, 18, 20), "-", editorButtonStyle )) {
						editorSelected.position.y =editorSelected.position.y - 0.1f;
						UpdateElementVisual( editorSelected );
					}
					editorX=editorX+20;
					if (GUI.Button (new Rect (editorX, editorY, 38, 20), ""+editorSelected.position.y, editorButtonStyle )) {
						//editorSelected.rotation =editorSelected.rotation - 10.0f;
						//UpdateElementVisual( editorSelected );
					}
					editorX=editorX+40;
					if (GUI.Button (new Rect (editorX, editorY, 20, 20), "+", editorButtonStyle )) {
						editorSelected.position.y =editorSelected.position.y + 0.1f;
						UpdateElementVisual( editorSelected );
					}
					editorX = editorX +42;

					// rotations
					// editorX=editorX + 62 + 5 + arrScales.Length*24;
					if (GUI.Button (new Rect (editorX, editorY, 38, 20), "ROT:", editorButtonStyle )) {
						editorSelected.rotation =0.0f;
						UpdateElementVisual( editorSelected );
					}
					editorX=editorX+40;
					if (GUI.Button (new Rect (editorX, editorY, 8, 20), "<", editorButtonStyle )) {
						editorSelected.rotation =editorSelected.rotation + 10.0f;
						UpdateElementVisual( editorSelected );
					}
					editorX=editorX+10;
					if (GUI.Button (new Rect (editorX, editorY, 28, 20), ""+editorSelected.rotation, editorButtonStyle )) {
						//editorSelected.rotation =editorSelected.rotation - 10.0f;
						//UpdateElementVisual( editorSelected );
					}
					editorX=editorX+30;
					if (GUI.Button (new Rect (editorX, editorY, 8, 20), ">", editorButtonStyle )) {
						editorSelected.rotation =editorSelected.rotation - 10.0f;
						UpdateElementVisual( editorSelected );
					}
					editorX = editorX +10;
					if (GUI.Button (new Rect (editorX, editorY, 10, 20), "~", editorButtonStyle )) {
						editorSelected.rotation = UnityEngine.Random.Range(0.0f,360.0f);
						UpdateElementVisual( editorSelected );
					}
					editorX = editorX +12;

					// show size
					editorX = editorX +12;
					float[] arrScales = { 0.2f, 0.5f, 0.75f, 1.0f, 1.5f, 2.0f, 3.0f, 4.0f, 8.0f };
					GUI.Button (new Rect (editorX, editorY, 60, 20), "SIZE", editorButtonStyle );
					for (int i=0; i<arrScales.Length; i++) {
						float size = arrScales [i];
						string text = ""+size;
						GUIStyle gui = editorButtonStyle;
						if (editorSelected.size==size) {
							gui = editorButtonActiveStyle;
							text = ">" + text + "";
						}
						bool buttonClicked = GUI.Button (new Rect (editorX + 62 + i * 24, editorY, 22, 20), text , gui );
						if (buttonClicked) {
							editorSelected.size =size;
							UpdateElementVisual( editorSelected );
						}
					}


					/*
					editorX=editorX + 62 + 5 + arrScales.Length*24;
					float[] arrRotations = { 0.0f, 30.0f, 60.0f, 90.0f };
					GUI.Button (new Rect (editorX, editorY, 60, 20), "ROT", editorButtonStyle );
					for (int i=0; i<arrRotations.Length; i++) {
						float rotation = arrRotations [i];
						string text = ""+rotation;
						GUIStyle gui = editorButtonStyle;
						if (editorSelected.rotation==rotation) {
							gui = editorButtonActiveStyle;
							text = ">" + text + "";
						}
						bool buttonClicked = GUI.Button (new Rect (editorX + 62 + i * 24, editorY, 22, 20), text , gui );
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
					*/
					editorY = editorY + 22;
				}
			}
			// if (editorTool.Equals ("MOVE")) { showElements=true; }

			if (editorTool.Equals ("SPLIT")) { 

				// editorToolSub
				string[] arrEditorToolsSub={"right","left","down","up"};
				for (int i=0;i<arrEditorToolsSub.Length;i++) {

					string add="";
					string title=""; 
					if (i==0) { title = "ADD |>"; if (editorToolSub.Equals ("right")) { add=">";  } }
					if (i==1) { title = "|< DEL"; if (editorToolSub.Equals ("left")) { add=">"; } }
					if (i==2) { title = "ADD ^"; if (editorToolSub.Equals ("down")) { add=">";   } }
					if (i==3) { title = "DEL ^"; if (editorToolSub.Equals ("up")) { add=">"; } }

					if (GUI.Button (new Rect (editorX + i*60, editorY, 58, 20), add+""+title, editorButtonStyle )) {
						editorToolSub	= arrEditorToolsSub[i];	
					}

				}
				editorY = editorY + 22;
			}

			// EVALUATION
			if (editorTool.Equals ("EVALU")) { 


				string evData="VISUALIZE ";
				string evDataOn = "ON";
				if (!editorShowEvaluationData) evDataOn = "OFF";
				if (GUI.Button (new Rect (editorX + 0*60, editorY, 118, 20), evData + evDataOn, editorButtonStyle )) {
					ToggleShowEvaluationData(); // editorShowEvaluationData
				}

				if (GUI.Button (new Rect (editorX + 2*60, editorY, 58, 20), "RELOAD", editorButtonStyle )) {
					UpdateShowEvaluationData();
				}

				// FILTERS ...


				editorY = editorY + 22;

				// PLAYERS
				int dtx = editorX; 
				string addOnX = "";
				if (editorEvaluationFilter.Equals("all")) { addOnX = ">"; } 
				if (GUI.Button (new Rect (editorX + 0*60, editorY, 58, 20), addOnX+"ALL", editorButtonStyle )) {

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
						if (GUI.Button (new Rect (dtx + (o+1)*60, editorY, 58, 20), addOn+"" + pl.name, editorButtonStyle )) {
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
				editorY = editorY + 22;

				// session id
				dtx = editorX; 
				addOnX = "";
				if (editorEvaluationSessionId.Equals("")) { addOnX = ">"; } 
				if (GUI.Button (new Rect (editorX + 0*60, editorY, 58, 20), addOnX+"ALL", editorButtonStyle )) {
					
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
					if (GUI.Button (new Rect (dtx + (o+1)*60, editorY, 58, 20), addOn+"/" + el.evaluationSessionId, editorButtonStyle )) {

						editorEvaluationFilter = "player.session";
						editorEvaluationPlayerId = ""+el.evaluationPlayerId;
						editorEvaluationSessionId = ""+el.evaluationSessionId;

						UpdateShowEvaluationData();
					}
				}


			}

			// show elements
			if (showElements) {


				string selectedEditorArea=""+editorArea;
				ArrayList arrTypesUnique = GetElementTypesUnique ();
				for (int i=0; i<arrTypesUnique.Count; i++) {
					GameElement unique = (GameElement)arrTypesUnique [i];
					string text = "" + unique.type;
					string ieditorArea = "" + unique.type;
					if (!unique.guiShowInMenu) {
						text = "("+text+")";	
					}

					GUIStyle guix = editorButtonStyleNotActive;

					if (editorTool.Equals ("EDIT")) {  if (editorSelected!=null) { selectedEditorArea=editorSelected.type;  } }
					if (ieditorArea.Equals (selectedEditorArea)) {
						guix = editorButtonActiveStyle;
						text = ">" + text;

					}
					bool buttonClicked = GUI.Button (new Rect (editorX + i * 60, editorY, 58, 20), text, guix);
					if (buttonClicked) {
						// do it ...
						// editorArea=ieditorArea;
						if (editorTool.Equals ("CREATE")) {  SetEditorArea (ieditorArea); SetSubEditorArea (unique.subtype);  }
						if (editorTool.Equals ("EDIT")) { 
							editorSelected.type=unique.type; 
							editorSelected.subtype=unique.subtype; 
							UpdateElementVisual(editorSelected); 
						}
					}
					// delete objects
					// CountElementsType( string elementArea, string elementSubArea )
				}
				editorY = editorY + 22;

				// show subcategories!
				ArrayList arr = GetElementTypes (selectedEditorArea);
				for (int a=0; a<arr.Count; a++) {
					GameElement gelement = (GameElement)arr [a];
					string text = "" + gelement.subtype;
					GUIStyle guix = editorButtonStyleNotActive;
					string selectedEditorSubArea=""+editorSubArea;
					if (editorTool.Equals ("EDIT")) {  if (editorSelected!=null) { selectedEditorSubArea=editorSelected.subtype;  } }
					if (selectedEditorSubArea.Equals (gelement.subtype)) {
						guix = editorButtonActiveStyle;
						text = ">" + text;
					}
					int count=CountElementsType( gelement.type, gelement.subtype );
					if (count>0) { if (GUI.Button (new Rect (editorX + 3 + a * 90 , editorY, 6, 20), "x", guix)) { RemoveElementsType(gelement.type, gelement.subtype);} }
					string strCount="";
					if (count>0) strCount="("+count+")";
					bool buttonClicked = GUI.Button (new Rect (editorX + 10 + a * 90, editorY, 80, 20), text+strCount, guix);
					if (buttonClicked) {
						// do it ...
						if (editorTool.Equals ("CREATE")) {  SetSubEditorArea (gelement.subtype); }
						if (editorTool.Equals ("EDIT")) {  editorSelected.type=gelement.type; editorSelected.subtype=gelement.subtype;  }

					}
				}

					
			
			}

			// display level elements 

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
				if (!GameElementInEditor(screenPos.x,screenPos.y))
				if ((screenPos.x > 0) && (screenPos.x < Screen.width)) {

					// default infos 
					// like name & argument
					bool showInfo=false;
					string textInfo="";
					if (!gaelement.name.Equals ("")) { showInfo=true; textInfo=textInfo+"'"+gaelement.name+"'"; } 
					if (!gaelement.argument.Equals ("")) { showInfo=true; if (!textInfo.Equals ("")) textInfo=textInfo+" "; textInfo=textInfo+"{"+gaelement.argument+"}";} 

					// info here 
					string waiting ="";
					if (gaelement.release.Equals ("wait")) {
						waiting = waiting+"[-]";
					}
					string strType="";
					if (cameraOverlayTypes) {
						strType = ""+gaelement.type+"\n -"+gaelement.subtype;
					}
					if (!showInfo) {
						GUI.Label (new Rect (screenPos.x+20, Screen.height - screenPos.y , 200, 80),"."+waiting+" "+strType,editorElementType );
					}
					if (showInfo) {
						// GUI.Label (new Rect (screenPos.x+20, Screen.height - screenPos.y, 200, 80),+"        "+strType,editorElementType );
						GUI.Label (new Rect (screenPos.x+20, Screen.height - screenPos.y+20, 200, 80),waiting+""+textInfo+"\n"+strType);
					}
					
					// edit ?
					if (editorTool.Equals ("EDIT")) {
						if (GUI.Button (new Rect (screenPos.x, Screen.height - screenPos.y, 20, 20), editorEditImage, editorIconGUI)) {
							SetSelectedElement(gaelement);
						}
						if (editorSelected==gaelement) {
								GUI.Label (new Rect (screenPos.x-10, Screen.height - screenPos.y-10, 40, 40), editorSelectedImage, editorIconGUI);
						}
					}

					// move ?
					// version 1.0
				
					if (editorTool.Equals ("MOVE")) {

						if (GUI.Button (new Rect (screenPos.x, Screen.height - screenPos.y, 20, 20), editorMoveImage, editorIconGUI)) {
							// SetSelectedElement(gaelement);
							//		Debug.Log("Move Pressed");

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

							// move
							editorSelected=null;
							editorToolMove="";
						}
						
					}

					// delete ?
					if (editorTool.Equals ("DELETE")) {
						if (GUI.Button (new Rect (screenPos.x, Screen.height - screenPos.y, 20, 20), editorDeleteImage, editorIconGUI)) {
							// delete it now ..
							RemoveElement (gaelement);
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
		

			// edit: detail editor
			// if (editorTool.Equals ("EDIT")) {
				if (editorSelected!=null) {
					// editorX
					// name
					float editorDetailX=editorPrefX+650;
					float editorDetailY=10;
						GUI.Label (new Rect(editorDetailX+124,10,100,20),""+editorSelected.type+"/"+editorSelected.subtype);
					if (GUI.Button (new Rect(editorDetailX,10,120,20),"INSPECTOR",editorButtonStyle)) {
						SetSelectedElementFromGUI();
					}
					// x/y/z
					// editDetailX=GUI.TextArea (new Rect(editorDetailX+122,editorDetailY,40,20),editDetailX);
					// editDetailY=GUI.TextArea (new Rect(editorDetailX+162,editorDetailY,40,20),editDetailY);
					// editDetailZ=GUI.TextArea (new Rect(editorDetailX+142,editorDetailY,20,20),editDetailZ);
					editorDetailY=editorDetailY+22;
					GUI.Label (new Rect(editorDetailX,editorDetailY,240,20),"name:                       ");
					editDetailName=GUI.TextField (new Rect(editorDetailX+42,editorDetailY,160,20),editDetailName);
					if (!editorSelected.name.Equals(editDetailName)) {
						UpdateElementVisual(editorSelected);
					}
					editorSelected.name=editDetailName + "";
					if (GUI.Button (new Rect(editorDetailX+42+165,editorDetailY,40,20),"COPY",editorButtonStyle)) {
						// copy here ... 
						GameElement copyThis = editorSelected.Copy ();
						copyThis.position.x=copyThis.position.x+UnityEngine.Random.Range ( 0.3f, 0.9f );
						// copyThis.position.y=UnityEngine.Random.Range ( 0.5f, 1.0f );
						AddElement(copyThis);
					}
					if (GUI.Button (new Rect(editorDetailX+42+165+42,editorDetailY,30,20),"DEL",editorButtonStyle)) {
						RemoveElement(editorSelected);
						editorSelected = null;
					}

					editorDetailY=editorDetailY+22;
					if (editorSelected!=null) {	
						if (editorSelected.guiBoolArgument) {
							GUI.Label (new Rect(editorDetailX,editorDetailY,40,24),""+editorSelected.guiLabel+":");
							editDetailArgument=GUI.TextField (new Rect(editorDetailX+42,editorDetailY,160,20),editDetailArgument);
						    bool changed=false;
							if (editDetailArgument!=editorSelected.argument) {
								changed=true;
							}
							editorSelected.argument=editDetailArgument;
							editorDetailY=editorDetailY+22*1;
							if (changed) {
								UpdateElementVisual(editorSelected);
								UpdateRelationVisualisationAndCheckError();
							}
							
						}
						// description?
						if (!editorSelected.guiDescription.Equals ("")) {
							GUI.Label (new Rect(editorDetailX+42,editorDetailY,240,20),editorSelected.guiDescription);
							editorDetailY=editorDetailY+22*1;
						}
						// specials refer type?
						string release=""+editorSelected.release;
						string add="";
						if (release.Equals ("")) { add=">"; }
						if (GUI.Button (new Rect(editorDetailX+42,editorDetailY,58,20),add+"active",editorButtonStyle)) {
							editorSelected.release="";
						}
						add = "";
						if (release.Equals ("wait")) { add=">"; }
						if (GUI.Button (new Rect(editorDetailX+42+60,editorDetailY,58,20),add+"standby",editorButtonStyle)) {
							editorSelected.release="wait";
						}
					}


				 }
			// }
			
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



			// FILTER
			// background
			GUI.Label ( new Rect(filterTypeVisual.x,filterTypeVisual.y-5,filterTypeVisual.width+5,filterTypeVisual.height+10), "", editorBackground);
			GUI.Label ( new Rect(filterTypeVisual.x,filterTypeSubVisual.y-5,filterTypeSubVisual.width+5,filterTypeSubVisual.height+10), "", editorBackground);
//			GUI.Label (filterTypeSubVisual, "", editorBackground);

			// filterDev
			int filterY = Screen.height-80;
			int filterX = 0;
			if (GUI.Button (new Rect(0,filterY,60,20),"VIEW *.* " ,editorComment)) {
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

			// NAMES
			filterX = filterX + 10;
			GUIStyle activestylex = editorButtonStyle;
			if (cameraOverlayTypes)  activestylex = editorButtonActiveStyle;
			if (GUI.Button (new Rect (filterX, filterY, 50, 20), "NAMES ", activestylex )) {
				cameraOverlayTypes=!cameraOverlayTypes;
				// SetCameraY(0.0f);
			}
			filterX = filterX + 60;

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

	bool enabledEditorEdges =false;

	void Update() {



		// editor
		if (gameLogic !=null &&  gameLogic.modal==GameLogic.GameLogicModal.Editor) {

			if (Input.GetMouseButtonDown(0)) {

			}
			if (Input.GetMouseButtonUp(0)) {
				// Debug.Log ("GetMouseButtonUp()");
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
			if (Input.GetKeyDown ("return")) {
				Debug.Log ("RETURN");
				// InsertVerticalLine();
				// GUI.SetNextControlName("");
			}
			if (Input.GetKeyDown ("backspace")) {
				// Debug.Log ("DELETE");
				// RemoveVerticalLine();
			}


			// generate new objects
			float mouseX=Input.mousePosition.x;
			float mouseY=Screen.height-Input.mousePosition.y;

			// hot edges
			if ((mouseX<20)||(mouseX>(Screen.width-20))||(mouseY<20)||(mouseY>(Screen.height-20))) {

				GameObject xcontainer=GameObject.Find ("editorCameraContainer");
				GameObject xeditorcamera=GameObject.Find ("editorcamera");

				if (enabledEditorEdges) {
					
					float rotatex = 0.75f;
					float rotatey = 0.5f;
					if (mouseY<20) {
						xeditorcamera.transform.Rotate( -rotatey, 0.0f, 0.0f );
					}
					if (mouseY>(Screen.height-20)) {
						xeditorcamera.transform.Rotate( +rotatey, 0.0f,  0.0f );
					}

					if (mouseX<20) {
						xcontainer.transform.Rotate( 0.0f, -rotatex, 0.0f );
					}
					if (mouseX>(Screen.width-20)) {
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
			
			// ... 
			if (mouseY>90) {
				
				// over something?
				if (Input.GetMouseButtonDown(0)) {
					// Debug.Log ("GetMouseButtonDown()");
					if (editorTool.Equals ("SPLIT")) {
						if (editorToolSub.Equals ("right")) {
							InsertVerticalLine();
						}
						if (editorToolSub.Equals ("up")) {
							RemoveHorizontalLine();
						}
						if (editorToolSub.Equals ("left")) {
							RemoveVerticalLine();
						}
						if (editorToolSub.Equals ("down")) {
							InsertHorizontalLine();
						}
						
					}
				}


				
				// not? create
				if (true) {
					
					// create something here!!!
					if (!CheckMouseInEditor())
					if (editorTool.Equals ("CREATE")) {
					 if (Input.GetButtonDown("Fire1")) {
					//	if (true) {

							// Debug.Log ("CREATE NOW");

							// sorry nothing found
							if (editorPrefab==null) {

								Debug.Log ("Sorry no correct prefab!");
								editorLogText = "[Create] select!";

							}

							if (editorPrefab!=null) {

// check here ...
								// if (editorPrefab.prefabGameObject!=null) {

									GameElement editorPrefabX = GetElementType (editorArea,editorSubArea);

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

								// }
							}
							// 
						}
					} // CREATE
					
				}
				
			}

			// /editor
		}
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
				float xspeed = 0.2f;
					float xspeedFactor = 3.0f;
				float xspeedEffective = 1.0f;

					// default: move camera
					if (!(Input.GetKey ("left shift"))&&(!Input.GetKey ("right shift"))) {

					if ((Input.GetKey ("a"))||(Input.GetKey ("left"))) {
						// scroll = scroll - 0.3f;
						// DoScroll(-0.3f,0.0f);
						//						DoEditorScroll( -speed.x, 0.0f, 0.0f );

						container.transform.Translate ( new Vector3(-xspeed, 0.0f, 0.0f));
					}
					if ((Input.GetKey ("d"))||(Input.GetKey ("right"))) {
						// scroll = scroll + 0.3f;
						//						DoEditorScroll( speed.x, 0.0f, 0.0f );

						container.transform.Translate ( new Vector3(xspeed, 0.0f, 0.0f));
					}

					// forward backward
					if ((Input.GetKey ("w"))||(Input.GetKey ("up"))) {
						// scroll = scroll + 0.3f;
						// DoScroll( 0.0f,0.3f);
						// DoEditorScroll( 0.0f, 0.0f, speed.z );
						container.transform.Translate ( new Vector3(0.0f, 0.0f, xspeed));
							
					}
					if ((Input.GetKey ("s"))||(Input.GetKey ("down"))) {
						// scroll = scroll + 0.3f;
//						DoEditorScroll( 0.0f, 0.0f, -speed.z );
						container.transform.Translate ( new Vector3(0.0f, 0.0f, -xspeed));

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
						vectorMove.x=0.1f;
						vectorMove.y=0.1f;
						vectorMove.z=0.1f;

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
