using UnityEngine;
using System.Collections;

public class LootScript : MonoBehaviour {

	public GameObject particlePrefab;
	public GameObject triggerGO;
	bool isMoving = false;
	Rigidbody rB;
	float speed = 10;
	public uint moneyBonus = 0;
	public float healthBonus = 0;
	public float faithBonus = 0;

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
		yield return new WaitForSeconds(0.7f);
		triggerGO.SetActive(true);
	}


	void OnTriggerEnter (Collider other) {

		// if triggerGO not yet active
		if(triggerGO && !isMoving)return;

		if(other.GetComponent<PlayerScript>() != null) {

			// When player picks loot up
			Destroy(this.gameObject);
			Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);
			GameLogicControllerScript.i.playerS.Money += moneyBonus;
			GameLogicControllerScript.i.playerS.dS.Health += healthBonus;
		}
	}

	public void PlayerEntersTrigger() {
		isMoving = true;
	}
}
