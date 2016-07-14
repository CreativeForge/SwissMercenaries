using UnityEngine;
using System.Collections;

public class HalberdToPickupTriggerScript : MonoBehaviour {

	public GameObject holyShineGO;
	public GameObject halberdGO;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider inC){
		if(inC.transform == InGameController.i.playerS.transform){
			//holyShineGO.SetActive(true);
			halberdGO.SetActive(false);
			InGameController.i.playerS.HasPickedUpHalberd = true;
			InGameController.i.playerS.StartHolyRage();
			GetComponent<Collider>().enabled = false;
		}
	}
}
