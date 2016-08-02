using UnityEngine;
using System.Collections;

public class RemarkText: GameElementBased {

	public GameObject text;
	public TextMesh textmesh;
	
	// Update is called once per frame
	void FixedUpdate () {
		text.transform.Rotate(0.0f, 0.1f, 0.0f);
		if (gameElement!=null) {
			string textx = ""+gameElement.argument;;
			if (textx.Length>30) textx = textx.Substring(0,27)+"[...]";
			textmesh.text = textx; // "--"+gameElement.argument;
		}
		// textmesh.text = "abc";
	}
}
