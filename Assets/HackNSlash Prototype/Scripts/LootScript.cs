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
	public float fightingSpiritBonus = 0;
	public float faithBonus = 0;
	GameObject appGO;

	public GameObject[] AppereancePrefabs;

	void Start(){
		rB = GetComponent<Rigidbody>();
		StartCoroutine(WaitNActivateTrigger());

		if(AppereancePrefabs != null && AppereancePrefabs.Length>0){
			GetComponent<MeshRenderer>().enabled = false;
			int rand = Random.Range(0, AppereancePrefabs.Length);
			GameObject randPrefab = AppereancePrefabs[rand];
			appGO = Instantiate(randPrefab, transform.position, randPrefab.transform.rotation) as GameObject;
			appGO.transform.parent = transform;
		}
	}

	void FixedUpdate(){
		if(isMoving){
			transform.LookAt(GameLogicControllerScript.i.playerS.transform);
			speed += Time.fixedDeltaTime*speed*10;
			rB.MovePosition(rB.position + transform.forward * Time.fixedDeltaTime * speed);
		}
		Quaternion rot = Quaternion.Euler( 0, transform.rotation.eulerAngles.y, 0 );
		//rB.MoveRotation(rot);
		transform.rotation = rot;
		if(appGO)appGO.transform.localRotation = Quaternion.identity;
		Debug.Log ("rot: "+transform.eulerAngles);
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
