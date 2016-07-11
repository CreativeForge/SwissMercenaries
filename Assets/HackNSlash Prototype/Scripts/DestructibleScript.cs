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

	// Use this for initialization
	void Start () {
		originalColor = appearanceAlive.GetComponent<Renderer>().material.color;
		if(appearanceDead)appearanceDead.SetActive(false);
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
		if(isDead) return;

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

		if(!appearanceDead)
			Destroy(gameObject);
		else
		{
			isDead = true;
			appearanceDead.SetActive(true);
			appearanceAlive.SetActive(false);
			GameObject tHitBox1 = GetComponent<HitterScript>().hitBox1;
			if(tHitBox1) tHitBox1.SetActive(false);
			GameObject tHitBox2 = GetComponent<HitterScript>().hitBox2;
			if(tHitBox2) tHitBox2.SetActive(false);
			if(GetComponent<PlayerScript>())
				StartCoroutine(WaitNRestart());
		}
	}

	IEnumerator WaitNRestart(){
		yield return new WaitForSeconds(2);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public bool GetIsDead(){
		return isDead;
	}
}
