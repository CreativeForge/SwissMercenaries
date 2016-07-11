using UnityEngine;
using System.Collections;

public class GameLogicControllerScript : MonoBehaviour {

	public static GameLogicControllerScript i;
	public PlayerScript playerS;

	public GameObject canvasGO;
	public RectTransform healthT;

	// Use this for initialization
	void Awake () {
		i = this;
		canvasGO.SetActive(true);
	}

	void Start(){
		playerS = FindObjectOfType<PlayerScript>();
	}

	public void AdjustHealthVisualisation(){
		//Debug.Log("healthT: "+healthT);
		//Debug.Log("playerS: "+playerS);
		//Debug.Log("playerS.dS: "+playerS.dS);
		healthT.localScale = new Vector3(playerS.dS.health/100f,1,1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
