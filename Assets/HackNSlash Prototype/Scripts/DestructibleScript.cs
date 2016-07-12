using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DestructibleScript : MonoBehaviour {

	public float health = 100;
	bool isDead = false;
	public GameObject bloodParticlesPrefab;
	public GameObject bloodParticlesGO;

	public GameObject colliderAlive;
	public GameObject colliderDead;
	public GameObject appearanceAlive;
	public GameObject appearanceDead;
	public Transform ragdollCenter;

	public float lastHitTime = 0;
	Color originalColor;
	bool isHitted = false;
	bool invincible = false;

	public GameObject lootObject;
	public bool hasLoot = true;

	PlayerScript pS;

	public Animator anim;
	public bool useAnimOnDeath = false;

	// Use this for initialization
	void Start () {
		originalColor = appearanceAlive.GetComponent<Renderer>().material.color;
		if(appearanceDead)appearanceDead.SetActive(false);
		if(colliderDead)colliderDead.SetActive(false);
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
				GameLogicControllerScript.i.PlayerDies();
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


		if(isDead) {

			// Plunder mode?
			if((GameLogicControllerScript.i.GameMode == 1) && hasLoot) {

				hasLoot = false;

				ragdollCenter.GetComponent<Rigidbody>().AddForce(Vector3.up*1000);

				// Create loot
				for(int i = 0;i < Random.Range(3, 8);i++) {
					GameObject clone;
					clone = Instantiate(lootObject, transform.position+Vector3.up*0.3f, Random.rotation) as GameObject;
					clone.GetComponent<Rigidbody>().AddExplosionForce(30, transform.position, 1);

				}
				SetColor(Color.black);

			} 
			return;
		}
			
		if(anim){
			anim.SetTrigger("IsHittedTrigger");
			//Debug.Log("ishittet anim");
		}

		SetColor(Color.red);
		isHitted = true;
		lastHitTime = Time.time;
		Health -= inForce;
		//Debug.Log("is hitted with force: "+inForce+"; health left: "+health);
		bloodParticlesGO.SetActive(false);
		bloodParticlesGO.SetActive(true);
	}

	void SetColor(Color inColor){
		appearanceAlive.GetComponent<Renderer>().material.color = inColor;
		appearanceDead.GetComponent<Renderer>().material.color = inColor;
	}

	void Die(){
		if(isDead) return;
		isDead = true;

		SetColor(new Color(0.4f,0,0));

		// If im not a player
		if(!pS) {
			// Enemy dies
			GameLogicControllerScript.i.EnemyDies();
		}

		if(!appearanceDead && !anim){
			Destroy(gameObject);
		}else{
			
			GameObject tHitBox1 = GetComponent<HitterScript>().hitBox1;
			if(tHitBox1) Destroy(tHitBox1);
			GameObject tHitBox2 = GetComponent<HitterScript>().hitBox2;
			if(tHitBox2) Destroy(tHitBox2);

			if(useAnimOnDeath){
				anim.SetTrigger("DieTrigger");
			}else{
				appearanceDead.SetActive(true);
				colliderDead.SetActive(true);
				appearanceAlive.SetActive(false);
				colliderAlive.SetActive(false);

				//Move MainCollider to ragdoll position
				if(ragdollCenter){
					transform.DetachChildren();
					GetComponent<Rigidbody>().isKinematic = true;
					transform.parent = ragdollCenter;
					transform.localPosition = Vector3.zero;
				}

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
