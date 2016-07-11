using UnityEngine;
using System.Collections;

public class HitBoxScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider inC){
		DestructibleScript dS = inC.GetComponent<DestructibleScript>();
		if(dS)
			GetComponentInParent<HitterScript>().HitsDestructible(dS);
	}
}
