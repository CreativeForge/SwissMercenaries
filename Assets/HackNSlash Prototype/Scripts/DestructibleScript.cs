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
	bool invincible = false;

	public GameObject lootObject;
	public bool hasLoot = true;

	PlayerScript pS;

	public Animator anim;

	// Use this for initialization
	void Start () {
		originalColor = appearanceAlive.GetComponent<Renderer>().material.color;
		if(appearanceDead)appearanceDead.SetActive(false);
		bloodParticlesGO = Instantiate(bloodParticlesPrefab, transform.position, Quaternion.identity) as GameObject;
		bloodParticlesGO.transform.parent = transform;
		bloodParticlesGO.SetActive(false);
		pS = GetComponent<PlayerScript>();
	}

	// Update is called once per frame
	void Update () {
		if(isDead){
			if(pS && lastHitTime+5f<Time.time){
				Debug.Log("Restart after death");
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}
			return;
		}

		if(isHitted && lastHitTime+0.5f<Time.time){
			isHitted = false;
			SetColor(originalColor);
		}
	}

	public void IsHitted(float inForce){

		if(Invincible) return;

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

			} 
			return;
		}
			
		if(anim){
			anim.SetTrigger("IsHittedTrigger");
			//Debug.Log("ishittet anim");
		}

		isHitted = true;
		lastHitTime = Time.time;
		Health -= inForce;
		//Debug.Log("is hitted with force: "+inForce+"; health left: "+health);
		bloodParticlesGO.SetActive(false);
		bloodParticlesGO.SetActive(true);
		SetColor(Color.red);
	}

	void SetColor(Color inColor){
		appearanceAlive.GetComponent<Renderer>().material.color = inColor;
	}

	void Die(){
		if(isDead) return;
		isDead = true;

		// If im not a player
		if(!pS) {
			GameLogicControllerScript.i.CheckEnemyDeathCount();
		}

		if(!appearanceDead && !anim){
			Destroy(gameObject);
		}else{
			
			GameObject tHitBox1 = GetComponent<HitterScript>().hitBox1;
			if(tHitBox1) Destroy(tHitBox1);
			GameObject tHitBox2 = GetComponent<HitterScript>().hitBox2;
			if(tHitBox2) Destroy(tHitBox2);

			if(anim){
				anim.SetTrigger("DieTrigger");
			}else{
				appearanceDead.SetActive(true);
				appearanceAlive.SetActive(false);
			}
		}

		/*
		if(pS){
			Debug.Log("restart");
			WaitNRestart1();
		}*/
	}

	/*IEnumerator WaitNRestart1(){
		Debug.Log("restart2");
		yield return new WaitForSeconds(2);
		Debug.Log("restart3");
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}*/

	public float Health {

		get { return health; }

		set {
			health = Mathf.Clamp(value,0,100);
			GameLogicControllerScript.i.AdjustHealthVisualisation();

			if(health <= 0){
				Die();
			}
		}

	}

	public bool IsDead {
		get { return isDead; }
	}

	public bool HasLoot {
		get { return hasLoot; }
	}

	public bool Invincible{
		get { return invincible; }

		set {
			invincible = value;
			if(invincible)SetColor(Color.yellow);
			else SetColor(originalColor);
		}	
	}
}
