using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	public float speed = 1;
	public Rigidbody rB;
	public DestructibleScript dS;

	public bool lookAtPlayer = false;
	public bool smoothLookAtPlayer = false;
	public bool stopNearPlayer = false;
	public bool startAttackingWhenPlayerEntersTrigger = false;
	bool isWalking = false;
	bool playerHasEnteredTrigger = false;

	public Animator anim;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(dS && dS.IsDead)return;
		Vector3 tPlayerPos = InGameController.i.playerS.transform.position;
		if(smoothLookAtPlayer){
			Vector3 targetPos = new Vector3(tPlayerPos.x, transform.position.y, tPlayerPos.z);
			Vector3 relativePos = targetPos - transform.position;
			Quaternion TargetRotation = Quaternion.LookRotation(relativePos);
			transform.rotation = Quaternion.Lerp(transform.rotation,TargetRotation,Time.fixedDeltaTime*5);
		}else if(lookAtPlayer){
			transform.LookAt(new Vector3(tPlayerPos.x, transform.position.y, tPlayerPos.z));
		}
		if(isWalking){
			if(Vector3.Distance(rB.position, tPlayerPos)>2 || !stopNearPlayer){
				if(anim){
					rB.MovePosition(rB.position + transform.forward * Time.fixedDeltaTime * speed * speed * speed);
					anim.SetFloat("Velocity",0.5f * speed );
				}else{
					rB.MovePosition(rB.position + transform.forward * Time.fixedDeltaTime * speed);

				}
			}else
				if(anim)anim.SetFloat("Velocity",0);
		}
	}

	public void PlayerEnteredTrigger(){
		playerHasEnteredTrigger = true;
		if(startAttackingWhenPlayerEntersTrigger){
			if(GetComponent<HitterScript>().isShooter){
				GetComponent<HitterScript>().StartShooting(); 
			}else
				isWalking = true;
		}
	}
}
