using UnityEngine;
using System.Collections;

public class EvaluationPlayer {

	public string playerId = "";

	public int sessionId = 1;

	public string name = "";
	public string prename = "";

	public float age = 25;

	// percentage
	public int lovedCasual = 0;
	public int lovedCore = 0;

	// genre
	public string lovedGenre = "";

	// ...
	public string comment = "";

	// Object to JSON
	public JSONObject GetJSONObject() {
		
		JSONObject jsonObj = new JSONObject(JSONObject.Type.OBJECT);
		
		// jsonObj.AddField("state", "");
		jsonObj.AddField("playerId", ""+playerId);

		jsonObj.AddField("sessionId", sessionId);

		jsonObj.AddField("name", ""+name);
		jsonObj.AddField("prename", ""+prename);

		jsonObj.AddField("age", age);

		jsonObj.AddField("lovedCasual", lovedCasual);
		jsonObj.AddField("lovedCore", lovedCore);

		jsonObj.AddField("lovedGenre", lovedGenre);

		jsonObj.AddField("comment", ""+comment);

		
		return jsonObj;
	}
	
	// JSON to Object
	public void GetObjectFromJSON( JSONObject jsonObj  ) {
		
		for(int i = 0; i < jsonObj.list.Count; i++){

			string key = (string)jsonObj.keys[i];
			JSONObject jkeyObj = (JSONObject)jsonObj.list[i]; 
			
			// Debug.Log (" "+i+". "+key+" = "+jkeyObj.str); 

			if (key.Equals ("playerId")) { 	playerId = jkeyObj.str; }

			if (key.Equals ("sessionId")) { sessionId= (int)  jkeyObj.n; }

			if (key.Equals ("name")) { 	name=jkeyObj.str; }
			if (key.Equals ("prename")) { 	prename=jkeyObj.str;  }

			if (key.Equals ("age")) {   	age=(int) jkeyObj.n; }

			if (key.Equals ("lovedCasual")) { 	lovedCasual= (int) jkeyObj.n; } 
			if (key.Equals ("lovedCore")) { 	lovedCore= (int)  jkeyObj.n; }

			if (key.Equals ("lovedGenre")) { 	lovedGenre= jkeyObj.str; }

			if (key.Equals ("comment")) { 	comment=jkeyObj.str;  }
		}
		
		
	}

}
