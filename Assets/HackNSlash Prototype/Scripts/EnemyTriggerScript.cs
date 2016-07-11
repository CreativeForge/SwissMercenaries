using UnityEngine;
using System.Collections;

public class EnemyTriggerScript : MonoBehaviour {

	EnemyScript parentEnemyS;

	// Use this for initialization
	void Start () {
		parentEnemyS = GetComponentInParent<EnemyScript>();
	
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider inC) {
		if(GameLogicControllerScript.i.playerS.transform == inC.transform){
			parentEnemyS.PlayerEnteredTrigger();
			Destroy(gameObject);
		}
	}
}
