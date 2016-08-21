using UnityEngine;
using System.Collections;
using System;

// SERIALIZABLE FOR UNITYEDITOR > LEVELELEMENT.CS

public class GameElement {

	// public string state = ""; // "": active-direct / "wait": wait > released
	// > release

	// 
	public string id = "-1"; // id ... 

	// default objects
	public string name="";
	public string genericName = "";
	
	public string type="";
	public string subtype="";
	
	public string argument="";

	public string argumentInputType = ""; // name, names, notifcation, path, points ... env.* || path.point || select
	public string argumentInputTypeSelect = ""; // name, names, notifcation

	public string argumentsub = "";

	public string strevent = "enter";
	public float timed = 0.0f;
	public string target = "self"; // default self .. 

	public int index = 0; // index of the gameelement ... 

	public string creator = "";

	public bool editorRandom = false;

	// condition
	// todo: use relatedto, condition etc just only one parameter!
	// public string argumentsub


	public string release = ""; // '': always there | 'scroll' 
	public string releaseArg = ""; // '':  | '': relative 
	public string ReleaseString() {
		if (release.Equals ("wait")) {
			return "wait";
		}
		return "on";
	}
	public bool Active() {
		if (release.Equals("")) {
			return true;
		}
		return false;
	}
	public bool Internal() {
		if (ingameSource.Equals("")) {
			return true;
		}
		if (ingameSource.Equals("self")) {
			return true;
		}
		return false;
	}
	// * not used ...
	public float scrollX = 0.0f; // release on this ...

	public float offsetEditorScreenY = 0.0f;


	// strength
	public int strength = 1; // * copied but not saved!!!

	// position ... 
	public Vector3 position=new Vector3(0.0f,0.0f,0.0f);
	public float rotation = 0.0f; // in euler!
	public float rotationForward = 0.0f; // in euler!

	public float size = 1.0f;
//	public Vector3 size=new Vector3(1.0f,1.0f,1.0f); // * not yet used

	/*
	 * Ingame
	 * 
	 * */
	// coming from level
	public string ingameSource = "self"; // "all" - extern level ... for example 

	/*
	 * Editor Things 
	 * 
	 * */

	public float editorDisplaySize = 1.0f;

	public float editorTileSize = 0.0f;
	public bool editorIsGround = false;
	public bool randomPredefineds = false;
	// public int editorIndex = 1;

	// add ons (only visual)
	public bool guiBoolArgument = false;
	public string guiLabel = "";
	public string guiDescription = "";
	public bool guiShowInMenu=true; // show in menu or at least mark

	// Evaluation
	public DateTime created = DateTime.Now; // .ToString("yyyyMMddHHmmss"); DateTime.UtcNow.Millisecond
	// public string evaluationLevelVersion = ""; 
	public string evaluationPlayerId = ""; // playerId
//	public string evaluationGameId = ""; // gameId
	public int evaluationSessionId = 0; // playerId
	public float evaluationPlayTime = 0.0f;  // play time ingame

	// error
	public bool flagError = false;
	public string descError = "";
	   
	public GameElement() {

		// not used ... 
		offsetEditorScreenY = UnityEngine.Random.Range (0.0f, 10.0f); 
	
	}

	// copy - type > object (template to object)
	public GameElement Copy() {
		GameElement copyX = new GameElement ();
		copyX.id = id;
		copyX.name = name;
		copyX.genericName = genericName;
		copyX.type = type;
		copyX.subtype = subtype;
		copyX.position = new Vector3(position.x,position.y,position.z);
		// copyX.size = new Vector3(size.x,size.y,size.z);
		copyX.argument = argument;
		copyX.argumentInputType = argumentInputType;
		copyX.argumentInputTypeSelect = argumentInputTypeSelect;

		copyX.argumentsub = argumentsub;

		copyX.strevent = strevent;
		copyX.target = target;
		copyX.creator = creator;
		copyX.timed = timed;

		copyX.index = index;

		copyX.release = release;
		copyX.releaseArg = releaseArg;
		copyX.scrollX = scrollX;
		copyX.strength = strength;
		copyX.rotation = rotation;
		copyX.rotationForward = rotationForward;
		copyX.size = size;

		copyX.guiBoolArgument = guiBoolArgument;
		copyX.guiDescription = guiDescription;
		copyX.guiLabel = guiLabel;

		copyX.ingameSource = ingameSource;

		copyX.editorTileSize = editorTileSize;
		copyX.editorIsGround = editorIsGround;
		copyX.randomPredefineds = randomPredefineds;

		copyX.editorRandom = editorRandom;
		copyX.editorDisplaySize = editorDisplaySize;
		copyX.skyBoxMaterial = skyBoxMaterial;
		copyX.prefabGameObject = prefabGameObject;
		copyX.prefabEditorDummyGameObject = prefabEditorDummyGameObject;

		copyX.prefabPredefinedArguments = prefabPredefinedArguments;

		// copyX.state = state;
		return copyX;
	}

	// UpdateTo (On Change Type)
	public void ChangeTypeInEditMode( GameElement template ) {

		guiBoolArgument = template.guiBoolArgument;
		size = template.size;

		guiBoolArgument = template.guiBoolArgument;
		guiDescription = template.guiDescription;
		guiLabel = template.guiLabel;

		editorDisplaySize = template.editorDisplaySize;
		editorRandom = template.editorRandom;

		skyBoxMaterial = template.skyBoxMaterial;
		prefabGameObject = template.prefabGameObject;
		prefabEditorDummyGameObject = template.prefabEditorDummyGameObject;

			
	}
	
	// representations
	// material
	public Material skyBoxMaterial;
	// gameobject
	public GameObject gameObject;
	
	// prefab (used in editor)
	public  GameObject prefabGameObject;

	// prefab dummy
	public  GameObject prefabEditorDummyGameObject;
	public LevelElementOption[] prefabPredefinedArguments;

	// Object to JSON
	public JSONObject GetJSONObject() {
		
		JSONObject jsonObj = new JSONObject(JSONObject.Type.OBJECT);

		// jsonObj.AddField("state", "");
		jsonObj.AddField("id", ""+id);
		jsonObj.AddField("name", ""+name);
		jsonObj.AddField("type", type);
		jsonObj.AddField("subtype", subtype);
		jsonObj.AddField("x", position.x);
		jsonObj.AddField("y", position.y);
		jsonObj.AddField("z", position.z);
		jsonObj.AddField("index", index);
		jsonObj.AddField("argument", argument);
		jsonObj.AddField("argumentsub", argumentsub);

		jsonObj.AddField("target", target);
		// creator
		jsonObj.AddField("creator", creator);
		jsonObj.AddField("strevent", strevent);
		jsonObj.AddField("timed", timed);

		jsonObj.AddField("release", release); // target?
		jsonObj.AddField("releasearg", releaseArg);
		jsonObj.AddField("rotation", rotation);
		jsonObj.AddField("rotationforward", rotationForward);
		jsonObj.AddField("size", size);

		// evaluation stuff
		jsonObj.AddField("evaluationPlayerId", evaluationPlayerId);
		jsonObj.AddField("evaluationSessionId", evaluationSessionId);
		jsonObj.AddField("evaluationPlayTime", evaluationPlayTime);


		return jsonObj;
	}
	
	// JSON to Object
	public void GetObjectFromJSON( JSONObject jsonObj  ) {
		
		for(int i = 0; i < jsonObj.list.Count; i++){
			string key = (string)jsonObj.keys[i];
			JSONObject jkeyObj = (JSONObject)jsonObj.list[i]; 
			
			// Debug.Log (" "+i+". "+key); 
			// if (key.Equals ("state")) { 	state=jkeyObj.str; }
			if (key.Equals ("id")) { 	id=jkeyObj.str; }
			if (key.Equals ("name")) { 	name=jkeyObj.str; }
			if (key.Equals ("type")) { 	type=jkeyObj.str;  }
			if (key.Equals ("subtype")) { 	subtype=jkeyObj.str; }
			if (key.Equals ("x")) { 	position.x=(float)jkeyObj.n; }
			if (key.Equals ("y")) { 	position.y=(float)jkeyObj.n; }
			if (key.Equals ("z")) { 	position.z=(float)jkeyObj.n; }
			if (key.Equals ("timed")) { 	timed=(float)jkeyObj.n; }
			if (key.Equals ("argument")) { 	argument=jkeyObj.str; }
			if (key.Equals ("target")) { 	target=jkeyObj.str; }
			if (key.Equals ("creator")) { 	creator=jkeyObj.str; }

			if (key.Equals ("strevent")) { 	strevent=jkeyObj.str; }
			if (key.Equals ("index")) { 	index=(int)jkeyObj.n; }
			if (key.Equals ("argumentsub")) { 	argumentsub=jkeyObj.str; }
			if (key.Equals ("release")) { 	release=jkeyObj.str; }
			if (key.Equals ("releasearg")) { 	releaseArg=jkeyObj.str; }
			if (key.Equals ("rotation")) { 	rotation=(float)jkeyObj.n; } 
			if (key.Equals ("rotationforward")) { 	rotationForward=(float)jkeyObj.n; } 
			if (key.Equals ("size")) { 	size=(float)jkeyObj.n; }

			// evaluation stuff
			if (key.Equals ("evaluationPlayerId")) { 	evaluationPlayerId=jkeyObj.str; }
			if (key.Equals ("evaluationSessionId")) { 	evaluationSessionId=(int)jkeyObj.n; }
			if (key.Equals ("evaluationPlayTime")) { 	evaluationPlayTime=(float)jkeyObj.n; }


		}
		
		
	}
	
	
}