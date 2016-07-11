using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	public float speed = 1;
	public Rigidbody rB;
	public DestructibleScript dS;

	public bool lookAtPlayer = false;
	bool isWalking = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(dS && dS.GetIsDead())return;
		if(lookAtPlayer)transform.LookAt(new Vector3(GameLogicControllerScript.i.playerS.transform.position.x, transform.position.y, GameLogicControllerScript.i.playerS.transform.position.z));
		if(isWalking)rB.MovePosition(rB.position + transform.forward * Time.deltaTime * speed);
	}

	public void PlayerEnteredTrigger(){
		isWalking = true;
	}
}
