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
			//InGameController.i.ShowInGameMessage("HALBERD GAINED!!!\nNow you have a Halberd. Its a very powerfull weapon!" +
			//	"\nBut its slower than de sword. To change weapons press J on the keyboard oder B on the Controller.");
		}
	}
}
