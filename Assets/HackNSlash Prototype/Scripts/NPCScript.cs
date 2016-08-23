using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCScript : MonoBehaviour {

	// enemies (enemy,fugitives) & friendly npcs

	public bool isEnemy = true;
	public float speed = 1;
	public Rigidbody rB;
	public DestructibleScript dS; // component hitable < same like player
	public HitterScript hS;  // component hit the other < same like player

	public bool lookAtPlayer = false;  // rotate to player
	public bool smoothLookAtPlayer = false; // delayed rotate to player
	public bool stopNearPlayer = false; // wait before the player 
	public float stopNearPlayerDistance = 2; // distance
	public bool startAttackingWhenPlayerEntersTrigger = false; // player in trigger
	public bool fleeWhenPlayerInTrigger = false; // flee > fugitve
	bool isMoving = false; // for checking is moving
	public bool isStandingStill = false; // for checking stands still
	public bool playerIsInTrigger = false; // for checking player is in detecter trigger

	public Animator anim; // animation of the npc

	int checkForEnemiesCounter = 0; // how many npcs in trigger
	int enemyCounterFreq = 30; // 
	List<NPCScript> enemiesInTrigger = new List<NPCScript>(); // list of  ( isEnemy )
	public Transform attackTargetT; // attack ( isEnemy )

	// Use this for initialization
	void Start () {
		hS = GetComponent<HitterScript>();
		if(!isEnemy) {
			isMoving= true;
		}
		attackTargetT = InGameController.i.playerS.transform; // default kill player
	}

	void Update(){
		
		//if(isEnemy && !fleeWhenPlayerInTrigger)return;
		checkForEnemiesCounter++;
		if(checkForEnemiesCounter>enemyCounterFreq){
			checkForEnemiesCounter = 0;
				
			CheckForTargets();
		}
	}

	void CheckForTargets(){
		attackTargetT = InGameController.i.playerS.transform;
		if(!isEnemy && Vector3.Distance(transform.position, attackTargetT.position)>10){
			enemyCounterFreq = 100;
			return;
		}else{
			enemyCounterFreq = 30;
		}
			
		if(fleeWhenPlayerInTrigger && !playerIsInTrigger)
			isMoving = false;
		float tDist = 1000;
		List<NPCScript> tDeadEnemies = new List<NPCScript>();
		foreach(NPCScript tNPC in enemiesInTrigger){
			if(tNPC != null && !tNPC.dS.IsDead && 
				((isEnemy && !tNPC.isEnemy) || (!isEnemy && tNPC.isEnemy))){ // check enemy relation
				// enemy found
				float currentDist = Vector3.Distance(transform.position, tNPC.transform.position);
				if(tDist>currentDist){ 

					if(fleeWhenPlayerInTrigger)
						isMoving = true;
					attackTargetT = tNPC.transform;
					tDist = currentDist;
				}
			}else{
				// enemy dead or not enemy or not existent anymore
				tDeadEnemies.Add(tNPC);
			}
		}
		foreach(NPCScript tNPCDead in tDeadEnemies){
			enemiesInTrigger.Remove(tNPCDead);
		}
	}

	void FixedUpdate () {

		if(dS && dS.IsDead)return; // are you dead ... 

		// movement of the npc (enemy or friendly enemy)

		// if (InGameController!=null) {
		HandleRotation(InGameController.i.playerS.transform); // rotation

		HandleMovement(InGameController.i.playerS.transform); // handle movement ...
		//}
	}

	void HandleMovement(Transform inPlayerT){
		if(isMoving){
			if(Vector3.Distance(rB.position, attackTargetT.position)>stopNearPlayerDistance || !stopNearPlayer){
				isStandingStill = false;
				if(anim){
					rB.MovePosition(rB.position + transform.forward * Time.fixedDeltaTime * speed * speed * speed);
					anim.SetFloat("Velocity",0.5f * speed );
				}else{
					rB.MovePosition(rB.position + transform.forward * Time.fixedDeltaTime * speed);
				}
			}else{
				StandStill();
			}
		}else StandStill();
	}

	void StandStill(){
		isStandingStill = true;
		if(anim)anim.SetFloat("Velocity",0);
	}

	void HandleRotation(Transform inPlayerT){

		if(isEnemy){
			if(fleeWhenPlayerInTrigger){
				// fleeing
				transform.LookAt(new Vector3(attackTargetT.position.x, transform.position.y, attackTargetT.position.z));
				transform.Rotate(0,180,0);
			}else{
				// attacking, not fleeing
				if(smoothLookAtPlayer){
					Vector3 targetPos = new Vector3(attackTargetT.position.x, transform.position.y, attackTargetT.position.z);
					Vector3 relativePos = targetPos - transform.position;
					Quaternion TargetRotation = Quaternion.LookRotation(relativePos);
					transform.rotation = Quaternion.Lerp(transform.rotation,TargetRotation,Time.fixedDeltaTime*5);
				}else if(lookAtPlayer){
					transform.LookAt(new Vector3(attackTargetT.position.x, transform.position.y, attackTargetT.position.z));
				}
			}
		}else{
			//friend NPC
			Vector3 targetPos = inPlayerT.position+ inPlayerT.forward * 2 - inPlayerT.right * 2;
			if(attackTargetT){
				targetPos = attackTargetT.position;
			}

			// smooth look at
			targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
			Vector3 relativePos = targetPos - transform.position;
			Quaternion TargetRotation = Quaternion.LookRotation(relativePos);
			transform.rotation = Quaternion.Lerp(transform.rotation,TargetRotation,Time.fixedDeltaTime*5);

		}
	}

	public void PlayerEnteredTrigger(){
		if(playerIsInTrigger)return;

		playerIsInTrigger = true;
		if(fleeWhenPlayerInTrigger){
			isMoving = true;
		}else if(startAttackingWhenPlayerEntersTrigger){
			if(GetComponent<HitterScript>().isShooter){
				GetComponent<HitterScript>().StartShooting(); 
			}else
				isMoving = true;
		}
	}

	public void PlayerLeftTrigger(){
		if(!playerIsInTrigger)return;
		playerIsInTrigger = false;
		if(fleeWhenPlayerInTrigger)isMoving = false;
	}
		
	public void NPCEnteredTrigger(NPCScript inNPC){ // other npc enters trigger
		//if(isEnemy && !fleeWhenPlayerInTrigger)return;

		if(inNPC.dS && !inNPC.dS.IsDead && !enemiesInTrigger.Contains(inNPC)){
			enemiesInTrigger.Add(inNPC);
			CheckForTargets();
		}
	}

	public void  NPCLeftTrigger(NPCScript inNPC){
		//if(isEnemy && !fleeWhenPlayerInTrigger)return;
		if(enemiesInTrigger.Contains(inNPC))
			enemiesInTrigger.Remove(inNPC);

	}
}
