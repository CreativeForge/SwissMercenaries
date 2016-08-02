using UnityEngine;
using System.Collections;

public class SimplePlayer : MonoBehaviour {

	GameLogic gamelogicScript;

	// Use this for initialization
	void Start () {

		gamelogicScript = GameObject.Find("_GameLogic").GetComponent<GameLogic>();

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (gamelogicScript!=null)
		if (gamelogicScript.modal == GameLogic.GameLogicModal.Running) {
			
			Vector3 speed = new Vector3( 0.1f, 0.1f, 0.1f );
			if (Input.GetKey("up")) {
				GetComponent<Rigidbody>().MovePosition( transform.position + new Vector3(speed.x,0.0f,0.0f) );
			}
			if (Input.GetKey("down")) {
				GetComponent<Rigidbody>().MovePosition( transform.position + new Vector3(-speed.x,0.0f,0.0f) );
			}
			if (Input.GetKey("left")) {
				GetComponent<Rigidbody>().MovePosition( transform.position + new Vector3(0.0f,0.0f,speed.z) );
			}
			if (Input.GetKey("right")) {
				GetComponent<Rigidbody>().MovePosition( transform.position + new Vector3(0.0f,0.0f,-speed.z) );
			}
		
		}
	}
}
