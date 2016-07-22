using UnityEngine;
using System.Collections;


public class NPCTriggerScript : MonoBehaviour {

	NPCScript parentNPCS;

	// Use this for initialization
	void Awake () {
		parentNPCS = GetComponentInParent<NPCScript>();
		if(!parentNPCS)
			Debug.LogWarning("name: "+name+" parent: "+transform.parent);
	
	}
	
	void OnTriggerEnter (Collider inC) {
		NPCScript npcS = inC.GetComponent<NPCScript>();
		if(InGameController.i.playerS.transform == inC.transform){
			parentNPCS.PlayerEnteredTrigger();
		}else if(npcS) {
			parentNPCS.NPCEnteredTrigger(npcS);
		}
	}

	void OnTriggerExit (Collider inC) {
		NPCScript npcS = inC.GetComponent<NPCScript>();
		if(InGameController.i.playerS.transform == inC.transform){
			parentNPCS.PlayerLeftTrigger();
		}else if(npcS) {
			parentNPCS.NPCLeftTrigger(npcS);
		}
	}
}
