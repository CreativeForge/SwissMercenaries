using UnityEngine;
using System.Collections;

public class PlayerAnimationTriggerScript : MonoBehaviour {

	public HitterScript hitterS;

	// Use this for initialization
	void Start () {
		if(!hitterS) Debug.Log("no hitterscript: "+name+" id: "+GetInstanceID()+" parent: "+transform.parent.name);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void FromAnimationTriggerDoHitBox(){
		if (hitterS!=null)
		hitterS.FromAnimationTriggerDoHitBox();
	}
	public void FromAnimationTriggerDoHitHalberdBox(){
		if (hitterS!=null)
		hitterS.FromAnimationTriggerDoHitHalberdBox();
	}


}
