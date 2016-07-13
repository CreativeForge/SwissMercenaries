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
	public RectTransform faithT;

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
	public Text enemyText;
	public Material normalSkyBoxMat;
	public Material plunderSkyBoxMat;

	public GameObject CameraPrefab;

	uint allLootableEnemiesCount;
	DestructibleScript[] allDestructibleScripts;

	public GameObject camGO;
	public Camera cam;

	public bool inEditorUsed = true;

	public NotificationCenterPrototype notificationC;

	// Use this for initialization
	void Awake () {
		i = this;
		canvasGO.SetActive(true);
		counterTextGO = canvasGO.transform.FindChild("Counter").gameObject;
		counterText = counterTextGO.GetComponent<Text>();

		panelModeChangeGO = canvasGO.transform.FindChild("WhitePanel").gameObject;

		if(!normalSkyBoxMat)normalSkyBoxMat=RenderSettings.skybox;

		playerS = FindObjectOfType<PlayerScript>();
		notificationC = GetComponent<NotificationCenterPrototype>();

		ReloadCamera();

	}

	void ReloadCamera(){
		Camera cam = Camera.main;
		if(cam) {
			if(cam.transform.parent){
				if(cam.transform.parent.name == "Pivot"){
					cam.transform.parent.parent.gameObject.SetActive(false);
				}else{
					cam.gameObject.SetActive(false);
				}
			}else{
				// in editor
				cam.gameObject.SetActive(false);	
			}

		}

		camGO = Instantiate(CameraPrefab) as GameObject;
		cam = Camera.main;
		playerS.SetCamT(camGO.transform);
	}

	void Start(){

		allDestructibleScripts = FindObjectsOfType<DestructibleScript>();
		allLootableEnemiesCount = 0;
		foreach(DestructibleScript tDS in allDestructibleScripts){
			if(tDS.HasLoot)
				allLootableEnemiesCount++;
		}
		AdjustEnemyCountVisualisation(0);
		AdjustMoneyVisualisation();
			
	}

	public void AdjustHealthVisualisation(){
		//Debug.Log("healthT: "+healthT);
		//Debug.Log("playerS: "+playerS);
		//Debug.Log("playerS.dS: "+playerS.dS);
		healthT.localScale = new Vector3(playerS.dS.health/100f,1,1);
	}
	public void AdjustFaithVisualisation(){
		faithT.localScale = new Vector3(playerS.Faith/100f,1,1);
	}
	public void AdjustMoneyVisualisation(){
		//Debug.Log("healthT: "+healthT);
		//Debug.Log("playerS: "+playerS);
		//Debug.Log("playerS.dS: "+playerS.dS);
		moneyText.text = "Gold gesammelt: "+playerS.Money;
	}

	public void AdjustEnemyCountVisualisation(uint inCount){
		enemyText.text = "Landsknechte getötet: "+inCount+"/"+allLootableEnemiesCount;
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


		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}

		HandleCheatInput();
	}

	void HandleCheatInput(){
		if(Input.GetKeyDown(KeyCode.H)){
			playerS.dS.Health = 100;
		}
		if(Input.GetKeyDown(KeyCode.I)){
			playerS.dS.Invincible = !playerS.dS.Invincible;
		}

		/* // is done in myFreeLookCamScript
		if(Input.GetKeyDown(KeyCode.F)){
			camGO.GetComponent<
		}*/
	}

	public void EnemyDies(){
		CheckEnemyDeathCount();
		playerS.Money += 10;
		playerS.Faith += 10;
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

		AdjustEnemyCountVisualisation(countDeadEnemies);

	}

	public void PlayerDies(){
		ReloadLevel();
	}

	void ReloadLevel(){
		if(inEditorUsed){
			//TODO:  next Level!
		}else{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
					
					ReloadLevel();

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
