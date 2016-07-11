using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameLogicControllerScript : MonoBehaviour {

	public static GameLogicControllerScript i;
	public PlayerScript playerS;

	public GameObject canvasGO;
	public RectTransform healthT;

	// Game modes:
	// 0	kill
	// 1	plunder
	// ...
	private int gameMode = 0;
	private float gameModeCounter = 20;


	// Use this for initialization
	void Awake () {
		i = this;
		canvasGO.SetActive(true);
	}

	void Start(){
		playerS = FindObjectOfType<PlayerScript>();
	}

	public void AdjustHealthVisualisation(){
		Debug.Log("healthT: "+healthT);
		Debug.Log("playerS: "+playerS);
		Debug.Log("playerS.dS: "+playerS.dS);
		healthT.localScale = new Vector3(playerS.dS.health/100f,1,1);
	}
	
	// Update is called once per frame
	void Update() {

		// If plunder mode
		if(gameMode == 1) {

			gameModeCounter -= Time.deltaTime;

			if(gameModeCounter <= 0) {

				// Set GameMode back to kill (0)
				GameMode = 0;

			}

		}

	}

	// On GUI drawing
	void OnGUI() {

		canvasGO.transform.FindChild("Counter").GetComponent<Text>().text = gameModeCounter.ToString();

	}

	// GameMode property
	public int GameMode {

		get { return gameMode; }

		set {
			gameMode = value;
			Debug.Log("GameMode changed to value: " + gameMode.ToString());

			// Resolve gamemode
			switch(gameMode) {

				// kill
				case 0:
					gameModeCounter = 20;
					canvasGO.transform.FindChild("Counter").gameObject.SetActive(false);
					break;
				
				// plunder
				case 1:
					gameModeCounter = 20;
					canvasGO.transform.FindChild("Counter").gameObject.SetActive(true);
					break;
				
				// unexpected
				default:
					break;

			}
		}

	}
}
