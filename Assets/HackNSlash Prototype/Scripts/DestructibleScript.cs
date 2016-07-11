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
		originalColor = appearanceAlive.GetComponent<MeshRenderer>().material.color;
		appearanceDead.SetActive(false);
		bloodParticlesGO = Instantiate(bloodParticlesPrefab, transform.position, Quaternion.identity) as GameObject;
		bloodParticlesGO.transform.parent = transform;
		bloodParticlesGO.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
	
		if(isHitted && lastHitTime+0.5f<Time.time){
			isHitted = false;
			appearanceAlive.GetComponent<MeshRenderer>().material.color = originalColor;
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
		appearanceAlive.GetComponent<MeshRenderer>().material.color = Color.red;
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
		if(GetComponent<PlayerScript>())
			StartCoroutine(WaitNRestart());
	}

	IEnumerator WaitNRestart(){
		yield return new WaitForSeconds(2);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public bool GetIsDead(){
		return isDead;
	}
}
