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
	public Animator anim;

	// Use this for initialization
	void Start () {
		hitIntervalTime += Random.Range(-hitIntervalTimeRandomRange, hitIntervalTimeRandomRange);
		hitBox1.SetActive(false);
		if(!pS)originalRotHitBox2 = hitBox2.transform.parent.localRotation;
		else if(pS && pS.anim)anim = pS.anim;
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
				//DoSlowHit();
				DoFastHit();
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
		if(pS){
			//hitBox1.SetActive(true); // is called from animationTrigger in FromAnimationTriggerDoHitBox()

			//pS.LookInCamDir();

			if(pS.GetVelocity()<0.1f) {
				anim.SetTrigger("Attack01Trigger");
			}else{
				anim.SetTrigger("Attack01RunTrigger");
			}
		}else{

			anim.SetTrigger("Attack01Trigger");


			lastHitTime = Time.time;
			/*
			hitBox1.SetActive(true);
			lastHitTime = Time.time;
			isHittingFast = true;
			*/

		}
	}

	public void FromAnimationTriggerDoHitBox(){
		hitBox1.SetActive(true);
		lastHitTime = Time.time;
		isHittingFast = true;

	}

	void DoSlowHit(){

		isHittingSlow = true;
		hitBox2.SetActive(true);
		lastHitTime = Time.time;
		if(pS)pS.LookInCamDir();

	}
	
	public void HitsDestructible (DestructibleScript inDS) { // is called in HitBoxScript
		inDS.IsHitted(hitForce);
	}
}
