using UnityEngine;
using System.Collections;

public class PlayerAnimationTriggerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void FromAnimationTriggerDoHitBox(){
		HitterScript hitterS = GetComponentInParent<HitterScript>();
		if(hitterS)
			hitterS.FromAnimationTriggerDoHitBox();
		else
			Debug.LogError("no hitterscript found: "+name+" id: "+GetInstanceID());
	}
	public void FromAnimationTriggerDoHitHalberdBox(){
		HitterScript hitterS = GetComponentInParent<HitterScript>();
		if(hitterS)
			hitterS.FromAnimationTriggerDoHitHalberdBox();
		else
			Debug.LogError("no hitterscript found: "+name+" id: "+GetInstanceID());
	}


}
