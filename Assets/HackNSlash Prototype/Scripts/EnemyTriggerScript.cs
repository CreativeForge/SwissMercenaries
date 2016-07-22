using UnityEngine;
using System.Collections;

public class EnemyTriggerScript : MonoBehaviour {

	EnemyScript parentEnemyS;

	// Use this for initialization
	void Awake () {
		parentEnemyS = GetComponentInParent<EnemyScript>();
		if(!parentEnemyS)
			Debug.LogWarning("name: "+name+" parent: "+transform.parent);
	
	}
	
	void OnTriggerEnter (Collider inC) {
		if(InGameController.i.playerS.transform == inC.transform){
			parentEnemyS.PlayerEnteredTrigger();
			//Destroy(gameObject);
		}
	}

	void OnTriggerExit (Collider inC) {
		if(InGameController.i.playerS.transform == inC.transform){
			parentEnemyS.PlayerLeftTrigger();
			//Destroy(gameObject);
		}
	}
}
