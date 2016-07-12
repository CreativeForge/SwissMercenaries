using UnityEngine;
using System.Collections;

public class HitterScript : MonoBehaviour {

	public bool alwaysDangerous = false;
	public bool hitsOnlyPlayer = true;
	public bool shooting = false;
	public GameObject projectilePrefab;
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
	public float hitFastDuration = 0.1f;
	public float hitSlowDuration = 0.5f;
	public Animator anim;

	// Use this for initialization
	void Start () {
		hitIntervalTime += Random.Range(-hitIntervalTimeRandomRange, hitIntervalTimeRandomRange);
		if (hitBox1) hitBox1.SetActive(false);
		if(!pS && hitBox2)originalRotHitBox2 = hitBox2.transform.parent.localRotation;
		else if(pS && pS.anim)anim = pS.anim;

		if(alwaysDangerous)
			hitBox1.SetActive(true);
	}

	void Update(){

		// if is enemy and is dead, or is player and is dead
		if((eS && eS.dS && eS.dS.IsDead) || (pS && pS.dS && pS.dS.IsDead ))return;

		HandleHitting();
	}

	void HandleHitting(){

		if (alwaysDangerous) return;

		if(shooting && lastHitTime+hitFastDuration<Time.time){
			DoShoot();
		}else{

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

			if(isHittingFast && lastHitTime+hitFastDuration<Time.time){
				hitBox1.SetActive(false);
				isHittingFast = false;
			}

			/*
			if(isHittingSlow){
				if(lastHitTime+hitSlowDuration<Time.time){
					hitBox2.transform.parent.Rotate(180*Time.deltaTime,0,0);
					if(lastHitTime+1f<Time.time){
						hitBox2.SetActive(false);
						isHittingSlow = false;
						hitBox2.transform.parent.localRotation = originalRotHitBox2;
					}
				}
			}*/
		}
	}

	void DoShoot(){
		GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;
		projectile.transform.LookAt(GameLogicControllerScript.i.playerS.GetComponent<Collider>().bounds.center);
		projectile.GetComponent<HitBoxScript>().SetParentHitterScript(this);

		lastHitTime = Time.time;
	}

	void DoFastHit(){
		if(pS){
			// For Player

			//hitBox1.SetActive(true); // is called from animationTrigger in FromAnimationTriggerDoHitBox()

			// enable if player should hit in camera direction
			//pS.LookInCamDir();

			if(pS.GetVelocity()<0.1f) {
				anim.SetTrigger("Attack01Trigger");
			}else{
				anim.SetTrigger("Attack01RunTrigger");
			}
		}else{
			// For Enemy

			lastHitTime = Time.time;

			if(anim)
				anim.SetTrigger("Attack01RunTrigger");
			else{

				hitBox1.SetActive(true);
				lastHitTime = Time.time;
				isHittingFast = true;
			}

			/*
			hitBox1.SetActive(true);
			lastHitTime = Time.time;
			isHittingFast = true;
			*/

		}
	}

	public void FromAnimationTriggerDoHitBox(){
		// called in AnimationEvent from the attack-animation "Attack01RunTrigger" or "Attack01Trigger"
		hitBox1.SetActive(true);
		lastHitTime = Time.time;
		isHittingFast = true;

	}

	/*
	void DoSlowHit(){ // not yet in use
		isHittingSlow = true;
		hitBox2.SetActive(true);
		lastHitTime = Time.time;
		if(pS)pS.LookInCamDir();
	}
	*/
	
	public bool HitsDestructible (DestructibleScript inDS) { // is called in HitBoxScript
		if(pS){ // player hits something
			inDS.IsHitted(hitForce);
			return true;
		}else{ // enemy hits something
			if(inDS == GameLogicControllerScript.i.playerS.dS){ // enemy hits player
				inDS.IsHitted(hitForce);
				if(alwaysDangerous) GameLogicControllerScript.i.playerS.Push((GameLogicControllerScript.i.playerS.transform.position-transform.position)*300);
				return true;
			}else if(!hitsOnlyPlayer){ // enemy hits not-player
				inDS.IsHitted(hitForce);
				return true;
			}
		}
		return false;
	}
}
