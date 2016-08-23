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
	Renderer[] appearanceAliveAllRenderers;
	public GameObject appearanceDead;
	public Transform ragdollCenter;

	public float lastHitTime = 0;
	Color originalColor;
	bool isHitted = false;
	bool invincible = false;

	public GameObject lootObject;
	public bool hasLoot = true;

	PlayerScript pS;
	public NPCScript eS;
	HitterScript hS;

	public Animator anim;
	public bool useAnimOnDeath = false;

	bool foundGL = false;

	// Use this for initialization
	void Awake () {
		if(appearanceAlive.GetComponent<Renderer>()){
			appearanceAliveAllRenderers = new Renderer[1];
			appearanceAliveAllRenderers[0] = appearanceAlive.GetComponent<Renderer>();

		}else{
			appearanceAliveAllRenderers = appearanceAlive.GetComponentsInChildren<Renderer>();
		}
		originalColor = appearanceAliveAllRenderers[0].GetComponent<Renderer>().material.color;

		if(appearanceDead)appearanceDead.SetActive(false);
		if(colliderDead)colliderDead.SetActive(false);
		bloodParticlesGO = Instantiate(bloodParticlesPrefab, transform.position, Quaternion.identity) as GameObject;
		bloodParticlesGO.transform.parent = transform;
		bloodParticlesGO.SetActive(false);
		pS = GetComponent<PlayerScript>();
		eS = GetComponent<NPCScript>();
		hS = GetComponent<HitterScript>();

		// player should store its values globally
		if(FindObjectOfType<GameLogic>() != null)
			foundGL = true;
	}

	void Start(){
		if (InGameController.i!=null) {
		InGameController.i.RegistrateLootableEnemy(this);
		}
	}

	// Update is called once per frame
	void Update () {
		if(isDead){
			return;
		}

		if(isHitted && lastHitTime+0.5f<Time.time){
			isHitted = false;
			SetColor(originalColor);
		}
	}

	public void IsHitted(float inForce){

		if(Invincible) return;

		if(eS && eS.hS && eS.hS.IsBlocking){
			eS.rB.AddForce(transform.forward * -100);
			return;
		}

		if(isDead) {

			// Plunder mode?
			if((InGameController.i.GameMode == 1) && hasLoot) {

				hasLoot = false;

				if(ragdollCenter)ragdollCenter.GetComponent<Rigidbody>().AddForce(Vector3.up*1500);

				// Create loot
				for(int i = 0;i < Random.Range(3, 8);i++) {
					GameObject clone;
					clone = Instantiate(lootObject, transform.position+Vector3.up*0.2f*i, Random.rotation) as GameObject;
					clone.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1,2), 0.5f, Random.Range(-1,2))*60);

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
		if(appearanceAlive){
			foreach(Renderer tR in appearanceAliveAllRenderers){
				tR.material.color = inColor;
			}
		}
		if(appearanceDead.GetComponent<Renderer>())
			appearanceDead.GetComponent<Renderer>().material.color = inColor;
	}

	public void Die(){
		if(isDead) return;
		isDead = true;

		SetColor(new Color(0.4f,0,0));
		bloodParticlesGO.SetActive(false);

		// If im not a player
		if(pS){
			pS.Die();
		}else if(HasLoot) {
			// Enemy dies
			InGameController.i.EnemyDies();
		}

		if(!appearanceDead && !anim){
			Destroy(gameObject);
		}else{

			if(hS){
				GameObject tHitBox1 = hS.hitBox1;
				if(tHitBox1) Destroy(tHitBox1);
				GameObject tHitBox2 = hS.hitBox2;
				if(tHitBox2) Destroy(tHitBox2);
			}

			if(useAnimOnDeath){
				anim.SetTrigger("DieTrigger");
			}else{
				appearanceDead.SetActive(true);
				colliderDead.SetActive(true);
				appearanceAlive.SetActive(false);
				StartCoroutine(WaitNDestroyAliveCollider());

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

	IEnumerator WaitNDestroyAliveCollider(){
		yield return 0;
		Destroy(colliderAlive);
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
			float diff = value-health;
			if(diff==0)return;

			health = Mathf.Clamp(value,0,100);
			if(pS){
				InGameController.i.AdjustHealthVisualisation(diff);

				// store health
				if(foundGL)
					FindObjectOfType<GameLogic>().Store.Health = health;
			}else
				InGameController.i.UpdateHealthEnemy(diff, transform.position);
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
