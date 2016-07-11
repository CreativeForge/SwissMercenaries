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
		GameLogicControllerScript.i.playerS.GetComponent<HitterScript>().FromAnimationTriggerDoHitBox();
	}
}
