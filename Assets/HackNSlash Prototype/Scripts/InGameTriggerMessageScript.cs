using UnityEngine;
using System.Collections;

public class InGameTriggerMessageScript : MonoBehaviour {

	//TextAreaAttribute(int minLines, int maxLines);
	[TextArea(3,10)]
	public string message;
	public bool centered = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider inC){
		if(InGameController.i.playerS.transform == inC.transform){
			InGameController.i.ShowInGameMessage(message, centered);
		}
	}
}
