using UnityEngine;
using System.Collections;

public class HitBoxScript : MonoBehaviour {

	HitterScript parentHitterScript;

	// Use this for initialization
	void Awake () {
		if (transform.parent) SetParentHitterScript(GetComponentInParent<HitterScript>());
		/*if(!parentHitterScript){
			Debug.LogError("on start) hitterscript: "+parentHitterScript + "self: "+name+" self.parent: "+transform.parent);
			Debug.LogError("on start) hitterscript: "+parentHitterScript + "self: "+name+" self.parent: "+transform.parent.name);
		}*/
	}

	public void SetParentHitterScript(HitterScript inS){
		if(!parentHitterScript)parentHitterScript = inS;
		//Debug.Log("set hitterscript: "+parentHitterScript + "self: "+name);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider inC){
		DestructibleScript dS = inC.GetComponent<DestructibleScript>();
		if(dS){
			// Debug.Log("ds entered: "+inC.name);
			if(!parentHitterScript){
				Debug.LogWarning("hitterscript: "+parentHitterScript + "self: "+name+" self.parent: "+transform.parent);
				Debug.LogWarning("hitterscript: "+parentHitterScript + "self: "+name+" self.parent: "+transform.parent.name);
			}else if(parentHitterScript.HitsDestructible(dS)){ // hit happens here in HitsDestructible
				if(GetComponent<ProjectileScript>()){
					Destroy(gameObject);
				}
			}


		}
	}
}
