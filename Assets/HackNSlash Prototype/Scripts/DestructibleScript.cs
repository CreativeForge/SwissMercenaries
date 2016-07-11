using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DestructibleScript : MonoBehaviour {

	public float health = 100;
	bool isDead = false;
	public GameObject bloodParticlesPrefab;
	public GameObject bloodParticlesGO;

	public GameObject appearanceAlive;
	public GameObject appearanceDead;

	public float lastHitTime = 0;
	Color originalColor;
	bool isHitted = false;

	public GameObject lootObject;
	public bool hasLoot = true;

	// Use this for initialization
	void Start () {
		originalColor = appearanceAlive.GetComponent<Renderer>().material.color;
		appearanceDead.SetActive(false);
		bloodParticlesGO = Instantiate(bloodParticlesPrefab, transform.position, Quaternion.identity) as GameObject;
		bloodParticlesGO.transform.parent = transform;
		bloodParticlesGO.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
	
		if(isHitted && lastHitTime+0.5f<Time.time){
			isHitted = false;
			appearanceAlive.GetComponent<Renderer>().material.color = originalColor;
		}
	}

	public void IsHitted(float inForce){

		GameObject clone;

		if(isDead) {

			// Plunder mode?
			if((GameLogicControllerScript.i.GameMode == 1) && hasLoot) {

				hasLoot = false;

				// Create loot
				for(int i = 0;i < Random.Range(1, 5);i++) {

					clone = Instantiate(lootObject, gameObject.transform.position, Random.rotation) as GameObject;
					clone.SetActive(true);

				}

			} else { return; }
		}

		isHitted = true;
		lastHitTime = Time.time;
		health -= inForce;
		Debug.Log("is hitted with force: "+inForce+"; health left: "+health);
		bloodParticlesGO.SetActive(false);
		bloodParticlesGO.SetActive(true);
		appearanceAlive.GetComponent<Renderer>().material.color = Color.red;
		GameLogicControllerScript.i.AdjustHealthVisualisation();
		if(health <= 0){
			Die();
		}
	}

	void Die(){
		if(isDead) return;

		isDead = true;
		appearanceDead.SetActive(true);
		appearanceAlive.SetActive(false);
		GetComponent<HitterScript>().hitBox1.SetActive(false);
		if(GetComponent<HitterScript>().hitBox2) GetComponent<HitterScript>().hitBox2.SetActive(false);

		// Am I a player?
		if(GetComponent<PlayerScript>()) {
			
			StartCoroutine(WaitNRestart());
		
		// Or an enemy?
		} else {

			// HitterScript-Component of all enemies
			Component[] allEnemyScripts = transform.parent.GetComponentsInChildren<DestructibleScript>();
			uint countDeadEnemies = 0;

			foreach(DestructibleScript ds in allEnemyScripts) {

				if(ds.GetIsDead)
					countDeadEnemies++;

			}

			// Are all enemies dead?
			if(transform.parent.childCount <= countDeadEnemies) {

				// Change to plunder game mode
				GameLogicControllerScript.i.GameMode = 1;

			}

		}
	}

	IEnumerator WaitNRestart(){
		yield return new WaitForSeconds(2);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public bool GetIsDead {
		get { return isDead; }
	}
}
