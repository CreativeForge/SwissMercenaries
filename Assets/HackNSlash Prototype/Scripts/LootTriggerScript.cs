using UnityEngine;
using System.Collections;

public class LootTriggerScript : MonoBehaviour {

	LootScript lootS;

	// Use this for initialization
	void Start () {
		lootS = GetComponentInParent<LootScript>();
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider other) {
		if(other.GetComponent<PlayerScript>() != null) {
			lootS.PlayerEntersTrigger();
		}
	}
}
