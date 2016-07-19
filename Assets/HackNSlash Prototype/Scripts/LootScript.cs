using UnityEngine;
using System.Collections;

public class LootScript : MonoBehaviour {

	public GameObject particlePrefab;
	public GameObject triggerGO;
	bool isMoving = false;
	Rigidbody rB;
	float speed = 10;
	public int moneyBonus = 0;
	public float healthBonus = 0;
	public float fightingSpiritBonus = 0;
	public float faithBonus = 0;
	GameObject appGO;
	public bool canBeEvil = false;
	public float chanceToBeEvil = 0.3f;
	public GameObject evilPrefab;

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
			Vector3 tDir = Vector3.Normalize( InGameController.i.playerS.transform.position - transform.position);
			speed += Time.fixedDeltaTime*speed*10;
			rB.MovePosition(rB.position + tDir * Time.fixedDeltaTime * speed);
		}
		Quaternion rot = Quaternion.Euler( 0, transform.rotation.eulerAngles.y, 0 );
		transform.rotation = rot;
		if(appGO)appGO.transform.localRotation = Quaternion.identity;
	}

	IEnumerator WaitNActivateTrigger(){
		yield return new WaitForSeconds(0.7f);
		if(canBeEvil && Random.value < chanceToBeEvil){
			Instantiate(evilPrefab, transform.position, Quaternion.identity);
			Destroy(gameObject);

		}else{
			triggerGO.SetActive(true);
		}
	}


	void OnTriggerEnter (Collider other) {

		// if triggerGO not yet active
		if(triggerGO && !isMoving)return;

		if(other.GetComponent<PlayerScript>() != null) {

			// When player picks loot up
			Destroy(this.gameObject);
			Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);
			InGameController.i.playerS.Money += moneyBonus;
			InGameController.i.playerS.dS.Health += healthBonus;
			InGameController.i.playerS.Faith += faithBonus;
		}
	}

	public void PlayerEntersTrigger() {
		isMoving = true;
	}
}
