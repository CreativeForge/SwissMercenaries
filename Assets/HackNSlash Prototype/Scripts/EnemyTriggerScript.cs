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
	
	// Update is called once per frame
	void OnTriggerEnter (Collider inC) {
		if(InGameController.i.playerS.transform == inC.transform){
			parentEnemyS.PlayerEnteredTrigger();
			Destroy(gameObject);
		}
	}
}
