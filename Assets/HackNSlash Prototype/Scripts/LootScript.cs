using UnityEngine;
using System.Collections;

public class LootScript : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		
		if(other.GetComponent<PlayerScript>() != null) {

			// When player picks loot up
			Destroy(this.gameObject);

		}

	}
}
