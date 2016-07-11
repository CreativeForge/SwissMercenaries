using UnityEngine;
using System.Collections;

public class HitBoxScript : MonoBehaviour {

	HitterScript parentHitterScript;

	// Use this for initialization
	void Start () {
		if (transform.parent) SetParentHitterScript(GetComponentInParent<HitterScript>());
	}

	public void SetParentHitterScript(HitterScript inS){
		parentHitterScript = inS;
		Debug.Log("set hitterscript: "+parentHitterScript + "self: "+name);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider inC){
		DestructibleScript dS = inC.GetComponent<DestructibleScript>();
		if(dS){

			if(!parentHitterScript){
				Debug.Log("hitterscript: "+parentHitterScript + "self: "+name+" self.parent: "+transform.parent);
				Debug.Log("hitterscript: "+parentHitterScript + "self: "+name+" self.parent: "+transform.parent.name);
			}
			if(parentHitterScript.HitsDestructible(dS)){
				if(GetComponent<ProjectileScript>()){
					Destroy(gameObject);
				}
			}


		}
	}
}
