using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
	private GameObject panelModeChangeGO;
	private GameObject counterTextGO;
	private Text counterText;
	public Text plunderText;
	public Text tutorialText;
	public Text moneyText;
	public Material normalSkyBoxMat;
	public Material plunderSkyBoxMat;

	public GameObject CameraPrefab;

	uint allLootableEnemiesCount;
	DestructibleScript[] allDestructibleScripts;

	// Use this for initialization
	void Awake () {
		i = this;
		canvasGO.SetActive(true);
		counterTextGO = canvasGO.transform.FindChild("Counter").gameObject;
		counterText = counterTextGO.GetComponent<Text>();

		panelModeChangeGO = canvasGO.transform.FindChild("WhitePanel").gameObject;

		if(!normalSkyBoxMat)normalSkyBoxMat=RenderSettings.skybox;

		playerS = FindObjectOfType<PlayerScript>();

		Camera cam = Camera.main;
		if(cam) {
			if(cam.transform.parent){
				if(cam.transform.parent.name == "Pivot"){
					cam.transform.parent.parent.gameObject.SetActive(false);
				}else{
					cam.gameObject.SetActive(false);
				}
			}else{
				cam.gameObject.SetActive(false);	
			}

		}
			
	}

	void Start(){
		GameObject camGO = Instantiate(CameraPrefab) as GameObject;

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

	public void AdjustMoneyVisualisation(){
		//Debug.Log("healthT: "+healthT);
		//Debug.Log("playerS: "+playerS);
		//Debug.Log("playerS.dS: "+playerS.dS);
		moneyText.text = "Gold: "+playerS.Money;
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
		if(Input.GetKeyDown(KeyCode.I)){
			Debug.Log("i");
			playerS.dS.Invincible = true;
		}
	}

	public void EnemyDies(){
		CheckEnemyDeathCount();
		playerS.Money += 10;
	}

	void CheckEnemyDeathCount(){

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
					
					// next Level!
					SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

					/*gameModeCounter = 20;
					counterTextGO.gameObject.SetActive(false);
					plunderText.gameObject.SetActive(false);
					tutorialText.gameObject.SetActive(true);
					RenderSettings.skybox = normalSkyBoxMat;
					*/
					break;
				
				// plunder
				case 1:
					playerS.SetToOriginalPosition();
					gameModeCounter = 20;
					counterTextGO.SetActive(true);
					plunderText.gameObject.SetActive(true);
					tutorialText.gameObject.SetActive(false);
				moneyText.gameObject.SetActive(true);
					RenderSettings.skybox = plunderSkyBoxMat;
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

		panelModeChangeGO.gameObject.SetActive(true);
		panelModeChangeGO.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
		panelModeChangeGO.GetComponent<Image>().CrossFadeAlpha(1.0f, flashTime, false);
		yield return new WaitForSeconds(flashTime);
		panelModeChangeGO.GetComponent<Image>().CrossFadeAlpha(0.0f, flashTime, false);
		yield return new WaitForSeconds(flashTime);
		panelModeChangeGO.SetActive(false);
		yield return null;

	}

}
