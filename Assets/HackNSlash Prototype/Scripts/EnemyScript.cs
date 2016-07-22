using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	public float speed = 1;
	public Rigidbody rB;
	public DestructibleScript dS;
	public HitterScript hS;

	public bool lookAtPlayer = false;
	public bool smoothLookAtPlayer = false;
	public bool stopNearPlayer = false;
	public bool startAttackingWhenPlayerEntersTrigger = false;
	public bool fleeWhenPlayerInTrigger = false;
	bool isMoving = false;
	public bool isStandingStill = false;
	public bool playerIsInTrigger = false;

	public Animator anim;

	// Use this for initialization
	void Start () {
		hS = GetComponent<HitterScript>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if(dS && dS.IsDead)return;

		Vector3 tPlayerPos = InGameController.i.playerS.transform.position;
		HandleRotation(tPlayerPos);

		HandleMovement(tPlayerPos);

	}

	void HandleMovement(Vector3 inPlayerPos){
		if(isMoving){
			if(fleeWhenPlayerInTrigger){
				// fleeing

				transform.LookAt(new Vector3(inPlayerPos.x, transform.position.y, inPlayerPos.z));
				transform.Rotate(0,180,0);
			}

			if(Vector3.Distance(rB.position, inPlayerPos)>2 || !stopNearPlayer){
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

	void HandleRotation(Vector3 inPlayerPos){
		if(smoothLookAtPlayer){
			Vector3 targetPos = new Vector3(inPlayerPos.x, transform.position.y, inPlayerPos.z);
			Vector3 relativePos = targetPos - transform.position;
			Quaternion TargetRotation = Quaternion.LookRotation(relativePos);
			transform.rotation = Quaternion.Lerp(transform.rotation,TargetRotation,Time.fixedDeltaTime*5);
		}else if(lookAtPlayer){
			transform.LookAt(new Vector3(inPlayerPos.x, transform.position.y, inPlayerPos.z));
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


}
