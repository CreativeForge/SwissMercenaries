﻿using UnityEngine;
using System.Collections;

public class HitterScript : MonoBehaviour {

	public bool notDangerous = false;
	public bool alwaysDangerous = false;
	public bool hitsOnlyPlayer = true;
	public bool isShooter = false;
	bool shooting = false;
	public bool canBlock = false;
	bool isBlocking = false;
	public GameObject projectilePrefab;
	public float hitForce = 10;
	float originalHitForce;
	public float hitStrongForce = 30;
	float originalHitStrongForce;
	bool isHittingFast = false;
	bool isHittingSlow = false;
	public GameObject hitBox1;
	public GameObject hitBox2;
	public Quaternion originalRotHitBox2;
	float lastHitTime = 0;
	public PlayerScript pS;
	public NPCScript eS;
	public float hitIntervalTime = 2;
	public float hitIntervalTimeRandomRange = 1;
	public float hitFastDuration = 0.1f;
	public float hitSlowDuration = 0.5f;
	public Animator anim;
	public GameObject[] weaponAppearances;
	public bool hideAllWeapons = false;

	bool isBackJumping;
	float startBackJumpTime = 0;

	/*public GameObject weaponTrailCurrentGO;
	public GameObject weaponTrailNormalGO;
	public GameObject weaponTrailHalberdGO;
	public GameObject weaponTrailHolyGO;
	public GameObject weaponTrailHolyGO;*/


	Rigidbody rB;

	// Use this for initialization
	void Start () {
		hitIntervalTime += Random.Range(-hitIntervalTimeRandomRange, hitIntervalTimeRandomRange);
		if (hitBox1) hitBox1.SetActive(false);
		if(!pS && hitBox2)originalRotHitBox2 = hitBox2.transform.parent.localRotation;
		else if(pS && pS.anim)anim = pS.anim;

		if(alwaysDangerous)
			hitBox1.SetActive(true);
		else if(hitBox1)
			hitBox1.SetActive(false);
			

		//Debug.Log("deactivate trail1");
		StartCoroutine(WaitNSetWeaponTrail());

		originalHitForce = hitForce;
		originalHitStrongForce = hitStrongForce;

		rB = GetComponent<Rigidbody>();

		if(hitBox2)hitBox2.SetActive(false);

		if(hideAllWeapons)ShowAllWeapons(false);
	}

	IEnumerator WaitNSetWeaponTrail(){

		//Debug.Log("deactivate trail2");
		yield return 0;
		if(pS) {
			ActivateTrail(false);
			//Debug.Log("deactivate trail3");
			//weaponTrailNormalGO = GameObject.Find("_XWeaponTrailMesh: X-WeaponTrail");
			//weaponTrailHolyGO = GameObject.Find("_XWeaponTrailMesh: X-WeaponTrail Holy");
			//weaponTrailHalberdGO = GameObject.Find("_XWeaponTrailMesh: X-WeaponTrail 2");
			//weaponTrailNormalGO.SetActive(false);
			//weaponTrailHolyGO.SetActive(false);
			//if(weaponTrailHalberdGO)weaponTrailHalberdGO.SetActive(false);
			//weaponTrailCurrentGO = weaponTrailNormalGO;
		}
	}

	public void ShowAllWeapons(bool inShow){
		foreach(GameObject tGO in weaponAppearances){
			tGO.SetActive(inShow);
		}
	}

	public void ModHitForce(float inMod){
		hitForce = originalHitForce * inMod;
		hitStrongForce = originalHitStrongForce * inMod;
	}

	public void ResetHitForce(){
		hitForce = originalHitForce;
		hitStrongForce = originalHitStrongForce;
	}

	void Update(){

		// if is enemy and is dead, or is player and is dead
		if((eS && eS.dS && eS.dS.IsDead) || (pS && pS.dS && pS.dS.IsDead ))return;

		HandleFighting();


		if(!pS) return;

		if(Input.GetButtonDown("BumperL")){
			BackJump();
		}

		if(Input.GetButtonDown("BumperR")){
			BackJump();
		}
	}

	void FixedUpdate(){
		HandleBackJumping();
	}


	void HandleBackJumping(){
		if(isBackJumping){
			rB.MovePosition(rB.position + transform.forward * -20 * Time.fixedDeltaTime);
			if(startBackJumpTime+0.5f<Time.time)
				isBackJumping = false;
		}

	}


	void BackJump(){
		isBackJumping = true;
		anim.SetTrigger("StepBackTrigger");
		startBackJumpTime = Time.time;

	}


	public void StartShooting(){
		if(isShooter)
			shooting = true;
		else
			Debug.LogWarning("is not a shooter");
	}

	void HandleFighting(){

		if (alwaysDangerous || notDangerous) return;

		if(shooting && lastHitTime+hitFastDuration<Time.time){
			DoShoot();
		}else{

			if(pS){
				// is player
				if(Input.GetKeyDown(KeyCode.Mouse0) || Input.GetButtonDown("Jump")){ // keyboard Space, controller B
					InGameController.i.HitButtonPressed();
					if(pS.IsUsingHalberd){
						DoSlowHalberdHit();
					}else{
						DoFastSwordHit();
					}
				}


			}else{
				// is enemy
				if(lastHitTime+hitIntervalTime<Time.time){
					if(IsBlocking) IsBlocking = false;
					//DoSlowHit();
					DoFastSwordHit();
				}else if(canBlock && !isHittingFast && !isHittingSlow && !IsBlocking && eS.isStandingStill){
					IsBlocking = true;
				}
			}

			if(isHittingFast && lastHitTime+hitFastDuration<Time.time){
				if(hitBox1)hitBox1.SetActive(false);
				isHittingFast = false;

				if(pS && !pS.isInHolyRage) ActivateTrail(false); // weaponTrailCurrentGO.SetActive(false);
			}


			if(isHittingSlow && lastHitTime+hitSlowDuration<Time.time){
				if(hitBox2)hitBox2.SetActive(false);
				isHittingSlow = false;

				if(pS && !pS.isInHolyRage) ActivateTrail(false); // weaponTrailCurrentGO.SetActive(false);

				/*
				if(lastHitTime+hitSlowDuration<Time.time){
					hitBox2.transform.parent.Rotate(180*Time.deltaTime,0,0);
					if(lastHitTime+1f<Time.time){
						hitBox2.SetActive(false);
						isHittingSlow = false;
						hitBox2.transform.parent.localRotation = originalRotHitBox2;
					}
				}
				*/
			}
		}
	}


	public bool IsBlocking{
		get{
			return isBlocking;
		}
		set{
			isBlocking = value;
			if(anim) anim.SetBool("IsBlockingBool", isBlocking);
		}
	}


	void DoShoot(){
		Vector3 centerP = InGameController.i.playerS.GetComponent<Collider>().bounds.center;
		float tDist = Vector3.Distance(transform.position, centerP);
		lastHitTime = Time.time;

		if(tDist>40)return;

		GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;
		projectile.GetComponent<HitBoxScript>().SetParentHitterScript(this);
		GameObject level = GameObject.Find("level");
		if (level!=null) {
			projectile.transform.parent = level.transform;
		}

	}

	void DoFastSwordHit(){
		if(pS){
			// For Player

			//hitBox1.SetActive(true); // is called from animationTrigger in FromAnimationTriggerDoHitBox()

			// enable if player should hit in camera direction
			//pS.LookInCamDir();

			ActivateTrail(true);//weaponTrailCurrentGO.SetActive(true);

			if(pS.GetVelocity()<0.1f) {
				anim.SetTrigger("Attack01Trigger");
			}else{
				anim.SetTrigger("Attack01RunTrigger");
			}
		}else{
			// For Enemy

			lastHitTime = Time.time;
				
			if(eS.isEnemy || (!eS.isEnemy && eS.attackTargetT != InGameController.i.playerS.transform)){
				if(anim)
					anim.SetTrigger("Attack01RunTrigger");
				else{
					
					if(hitBox1)hitBox1.SetActive(true);
					lastHitTime = Time.time;
					isHittingFast = true;
					
				}
			}
		}
	}
	public void FromAnimationTriggerDoHitBox(){
		// called in AnimationEvent from the attack-animation "Attack01RunTrigger" or "Attack01Trigger"

		if((eS && eS.dS && eS.dS.IsDead) || (pS && pS.dS && pS.dS.IsDead ))return;

		if(hitBox1)hitBox1.SetActive(true);
		lastHitTime = Time.time;
		isHittingFast = true;
	}

	void DoSlowHalberdHit(){
		if(pS){
			// For Player

			//hitBox1.SetActive(true); // is called from animationTrigger in FromAnimationTriggerDoHitBox()

			// enable if player should hit in camera direction
			//pS.LookInCamDir();

			ActivateTrail(true); //weaponTrailCurrentGO.SetActive(true);

			if(pS.GetVelocity()<0.1f) {
				anim.SetTrigger("AttackHalberd01Trigger");
			}else{
				anim.SetTrigger("AttackHalberd01RunTrigger");
			}
		}

	}

	public void FromAnimationTriggerDoHitHalberdBox(){
		// called in AnimationEvent from the attack-animation "Attack01RunTrigger" or "Attack01Trigger"

		if((eS && eS.dS && eS.dS.IsDead) || (pS && pS.dS && pS.dS.IsDead ))return;

		if(hitBox2)hitBox2.SetActive(true);
		lastHitTime = Time.time;
		isHittingSlow = true;
	}
	/*
	void DoSlowHalberdHit(){ // not yet in use
		isHittingSlow = true;
		hitBox2.SetActive(true);
		lastHitTime = Time.time;
		//if(pS)pS.LookInCamDir();
	}*/
	
	public bool HitsDestructible (DestructibleScript inDS) { // is called in HitBoxScript
		if(pS){ // player hits something

			float tHitForce = hitForce;
			if(pS.IsUsingHalberd)
				tHitForce = hitStrongForce;
			inDS.IsHitted(tHitForce);
			return true;
		}else if(eS && eS.isEnemy){ // enemy hits something
			if(inDS == InGameController.i.playerS.dS){ // enemy hits player
				inDS.IsHitted(hitForce);
				if(alwaysDangerous) InGameController.i.playerS.Push((InGameController.i.playerS.transform.position-transform.position)*300);
				return true;
			}else if(!inDS.eS.isEnemy || !hitsOnlyPlayer){ 
				// enemy hits npc OR enemy hits enemy if !hitsOnlyPlayer
				inDS.IsHitted(hitForce);
				return true;
			}
		}else if(eS && !eS.isEnemy){ // NPC hits something
			if(inDS != InGameController.i.playerS.dS && inDS.eS.isEnemy){ // npc hits enemy
				inDS.IsHitted(hitForce);
				return true;
			}
		}
		return false;
	}
		
	public void ActivateTrail(bool inActive){
		//Debug.Log("trail: "+inActive+" "+Time.time);
		if((!inActive && pS.IsUsingHalberd) || !pS.IsUsingHalberd){
			GameObject wTrail = GameObject.Find("_XWeaponTrailMesh: X-WeaponTrail Sword");
			if(wTrail)wTrail.GetComponent<MeshRenderer>().enabled = inActive;// SetActive(inActive);
		}
		GameObject wTrail2 = GameObject.Find("_XWeaponTrailMesh: X-WeaponTrail 2");
		if(wTrail2)wTrail2.GetComponent<MeshRenderer>().enabled = inActive;// 
	}
}
