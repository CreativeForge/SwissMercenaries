using UnityEngine;
using System.Collections;

public class ShopTriggerScript : MonoBehaviour {

	public ShopScript sS;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider inC){
		if(inC.transform == InGameController.i.playerS.transform){
			sS.PlayerEnteredTrigger();
		}
	}
}
