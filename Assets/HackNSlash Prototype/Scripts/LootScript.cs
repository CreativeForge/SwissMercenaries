using UnityEngine;
using System.Collections;

public class LootScript : MonoBehaviour {

	public GameObject particlePrefab;
	public GameObject triggerGO;
	bool isMoving = false;
	Rigidbody rB;
	float speed = 10;

	void Start(){
		rB = GetComponent<Rigidbody>();
		StartCoroutine(WaitNActivateTrigger());
	}

	void FixedUpdate(){
		if(isMoving){
			transform.LookAt(GameLogicControllerScript.i.playerS.transform);
			speed += Time.fixedDeltaTime*speed*10;
			rB.MovePosition(rB.position + transform.forward * Time.fixedDeltaTime * speed);
		}
	}

	IEnumerator WaitNActivateTrigger(){
		yield return new WaitForSeconds(1);
		triggerGO.SetActive(true);
	}


	void OnTriggerEnter (Collider other) {

		// if triggerGO not yet active
		if(!isMoving)return;

		if(other.GetComponent<PlayerScript>() != null) {

			// When player picks loot up
			Destroy(this.gameObject);
			Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);
			GameLogicControllerScript.i.playerS.Money += 10;
		}
	}

	public void PlayerEntersTrigger() {
		isMoving = true;
	}
}
