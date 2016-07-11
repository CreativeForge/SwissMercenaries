using UnityEngine;
using System.Collections;

public class HitterScript : MonoBehaviour {

	public float hitForce = 10;
	bool isHittingFast = false;
	bool isHittingSlow = false;
	public GameObject hitBox1;
	public GameObject hitBox2;
	public Quaternion originalRotHitBox2;
	float lastHitTime = 0;
	public PlayerScript pS;
	public EnemyScript eS;
	public float hitIntervalTime = 2;
	public float hitIntervalTimeRandomRange = 1;

	// Use this for initialization
	void Start () {
		hitIntervalTime += Random.Range(-hitIntervalTimeRandomRange, hitIntervalTimeRandomRange);
		hitBox1.SetActive(false);
		if(!pS)originalRotHitBox2 = hitBox2.transform.parent.localRotation;
	}

	void Update(){

		if((eS && eS.dS && eS.dS.GetIsDead()) || (pS && pS.dS && pS.dS.GetIsDead() ))return;

		HandleHitting();
	}

	void HandleHitting(){

		if(pS){
			// is player
			if(Input.GetKeyDown(KeyCode.Mouse0) || Input.GetButtonDown("Jump")){
				DoFastHit();
			}
		}else{
			// is enemy
			if(lastHitTime+hitIntervalTime<Time.time){
				DoSlowHit();
			}
		}

		if(isHittingFast && lastHitTime+0.1f<Time.time){
			hitBox1.SetActive(false);
			isHittingFast = false;
		}

		if(isHittingSlow){
			if(lastHitTime+0.5f<Time.time){
				hitBox2.transform.parent.Rotate(180*Time.deltaTime,0,0);
				if(lastHitTime+1f<Time.time){
					hitBox2.SetActive(false);
					isHittingSlow = false;
					hitBox2.transform.parent.localRotation = originalRotHitBox2;
				}
			}
		}
	}

	void DoFastHit(){
		isHittingFast = true;
		hitBox1.SetActive(true);
		lastHitTime = Time.time;
		if(pS)pS.LookInCamDir();
	}

	void DoSlowHit(){
		isHittingSlow = true;
		hitBox2.SetActive(true);
		lastHitTime = Time.time;
		if(pS)pS.LookInCamDir();
	}
	
	public void HitsDestructible (DestructibleScript inDS) {
		inDS.IsHitted(hitForce);
	}
}
