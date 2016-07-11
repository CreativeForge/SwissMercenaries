using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	public float speed = 1;
	public Rigidbody rB;
	public DestructibleScript dS;

	public bool lookAtPlayer = false;
	public bool stopNearPlayer = false;
	bool isWalking = false;

	public Animator anim;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(dS && dS.IsDead)return;
		Vector3 tPlayerPos = GameLogicControllerScript.i.playerS.transform.position;
		if(lookAtPlayer)transform.LookAt(new Vector3(tPlayerPos.x, transform.position.y, tPlayerPos.z));
		if(isWalking){
			if(Vector3.Distance(rB.position, tPlayerPos)>2 || !stopNearPlayer){
				rB.MovePosition(rB.position + transform.forward * Time.fixedDeltaTime * speed);
				if(anim)anim.SetFloat("Velocity",0.5f * speed);
			}else
				if(anim)anim.SetFloat("Velocity",0);
		}
	}

	public void PlayerEnteredTrigger(){
		isWalking = true;
	}
}
