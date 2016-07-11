using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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
	private Text counterText;

	uint allLootableEnemiesCount;
	DestructibleScript[] allDestructibleScripts;

	// Use this for initialization
	void Awake () {
		i = this;
		canvasGO.SetActive(true);
		counterText = canvasGO.transform.FindChild("Counter").GetComponent<Text>();
	}

	void Start(){
		playerS = FindObjectOfType<PlayerScript>();

		allDestructibleScripts = FindObjectsOfType<DestructibleScript>();
		allLootableEnemiesCount = 0;
		foreach(DestructibleScript tDS in allDestructibleScripts){
			if(tDS.HasLoot)
				allLootableEnemiesCount++;
		}

		Debug.Log("allDestructibleScripts: "+allDestructibleScripts+"all count: "+allLootableEnemiesCount);

	}

	public void AdjustHealthVisualisation(){
		//Debug.Log("healthT: "+healthT);
		//Debug.Log("playerS: "+playerS);
		//Debug.Log("playerS.dS: "+playerS.dS);
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


		HandleCheatInput();
	}

	void HandleCheatInput(){
		if(Input.GetKeyDown(KeyCode.H)){
			Debug.Log("h");
			playerS.dS.Health = 100;
		}
	}

	public void CheckEnemyDeathCount(){

		uint countDeadEnemies = 0;

		foreach(DestructibleScript ds in allDestructibleScripts){
			if(ds.IsDead && ds.HasLoot)
				countDeadEnemies++;
		}

		// Are all enemies dead?
		if(allLootableEnemiesCount == countDeadEnemies) {

			// Change to plunder game mode
			GameMode = 1;
		}
	}

	// On GUI drawing
	void OnGUI() {

		counterText.text = gameModeCounter.ToString();

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
					playerS.SetToOriginalPosition();
					gameModeCounter = 20;
					canvasGO.transform.FindChild("Counter").gameObject.SetActive(true);
					break;
				
				// unexpected
				default:
					break;

			}

			// GUI Flash
			StartCoroutine(FlashGUI(0.4f));

		}

	}

	// Timeout method
	IEnumerator FlashGUI(float timeOut) {

		float flashTime = timeOut / 2f;

		canvasGO.transform.FindChild("WhitePanel").gameObject.SetActive(true);
		canvasGO.transform.FindChild("WhitePanel").GetComponent<CanvasRenderer>().SetAlpha(0.0f);
		canvasGO.transform.FindChild("WhitePanel").GetComponent<Image>().CrossFadeAlpha(1.0f, flashTime, false);
		yield return new WaitForSeconds(flashTime);
		canvasGO.transform.FindChild("WhitePanel").GetComponent<Image>().CrossFadeAlpha(0.0f, flashTime, false);
		yield return new WaitForSeconds(flashTime);
		canvasGO.transform.FindChild("WhitePanel").gameObject.SetActive(false);
		yield return null;

	}

}
