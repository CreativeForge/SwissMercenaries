using UnityEngine;
using System.Collections;

public class TrainingsDummyScript : MonoBehaviour {

	DestructibleScript dS;
	//Rigidbody rB;

	// Use this for initialization
	void Start () {
		dS = GetComponent<DestructibleScript>();
		//rB = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!dS.IsDead)transform.Rotate(0, 90 * Time.fixedDeltaTime, 0);
	}
}
