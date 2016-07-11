using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

	public Rigidbody rB;
	float speed = 10;

	// Use this for initialization
	void Start () {
		rB = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rB.MovePosition(rB.position + transform.forward * speed * Time.fixedDeltaTime);
	}
}
