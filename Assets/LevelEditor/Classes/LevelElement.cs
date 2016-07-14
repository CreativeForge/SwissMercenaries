using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelElement {


	public string typetypesub="type.typesub"; // type & subtype in one
	public GameObject gameObject ;
	public GameObject editorPrefab; // dummy object
	public LevelElementOption[] prefabEditorDummyArguments;

	public bool isGround = false;

	public bool argumentNeeded = true;
	public string argumentLabel = "arg";
	public string argumentDescription = "";

	public string inputType = ""; // names, name, notification, selection or abc.def
	public string inputTypeSelect = ""; // x,y,z

	public float editorDisplaySize = 1.0f;

	public float editorTileSize = 0.0f;

	public int editorIndex = 1;

	public LevelElement (  ) {
	
	}

	public LevelElement ( string itypetypesub ) {
		typetypesub = itypetypesub;
	}

  /*
	// mime: type/typesub
	public string GetType() {

		string[] words = typetypesub.Split('.');
		return words [0];

	}

	public string GetTypeSub() {
		string[] words = typetypesub.Split('.');
		if (words.Length>1) {
			return words[1];
		}

		return "";
	}
  */

}
