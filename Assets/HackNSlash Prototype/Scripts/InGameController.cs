using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;

public class InGameController : MonoBehaviour {

	public static InGameController i;
	public PlayerScript playerS;

	private int gameMode = 0; // GameModes: 0 Kill, 1 Plunder
	public float gameModeCounter = 20;

	public bool isPaused;
	public bool isBulletTime;
	public bool isInShop;

	public Material normalSkyBoxMat;
	public Material plunderSkyBoxMat;

	public GameObject CameraPrefab;

	List<DestructibleScript> allLootableEnemies = new List<DestructibleScript>();
	int countDeadEnemies = 0;

	public GameObject camGO;
	public Camera cam;

	public bool inEditorUsed = true;

	NotificationCenterPrototype notificationC;

	// Use this for initialization
	void Awake () {
		i = this;

		if(!normalSkyBoxMat)normalSkyBoxMat=RenderSettings.skybox;

		playerS = FindObjectOfType<PlayerScript>();
		notificationC = GetComponent<NotificationCenterPrototype>();

		ReloadCamera();

	}

	void ReloadCamera(){
		Camera tCam = Camera.main;
		if(tCam) {
			if(tCam.transform.parent){
				if(tCam.transform.parent.name == "Pivot"){
					tCam.transform.parent.parent.gameObject.SetActive(false);
				}else{
					tCam.gameObject.SetActive(false);
				}
			}else{
				// in editor
				tCam.gameObject.SetActive(false);	
			}

		}

		camGO = Instantiate(CameraPrefab) as GameObject;
		cam = Camera.main;
		if ((camGO!=null) && (cam!=null)) {
			if (playerS!=null) {
				playerS.SetCamT(camGO.transform);
			}
		}
	}

	void Start(){
		AdjustEnemyCountVisualisation();
		AdjustMoneyVisualisation();
		AdjustHolyRageVisualisation();			
	}

	public void RegistrateLootableEnemy(DestructibleScript inDS){
		if(inDS.HasLoot) {
			allLootableEnemies.Add(inDS);
			AdjustEnemyCountVisualisation();
		}
	}

	public void HitButtonPressed(){ // called from player.hitterscript
		notificationC.HideAllInGameMessages();
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
			Debug.Log("Escape Key");
			IsPaused = !IsPaused;
			notificationC.ShowPauseMenu(IsPaused); 
		}
		/*
		if(Input.GetKeyDown(KeyCode.B)){
			Debug.Log("B");
			isBulletTime = true;
			Time.timeScale = 0.2f;
			playerS.anim.SetFloat("VelocityMod",5);
		}*/

		HandleCheatInput();

		HandleHolyRageGUIVisualisation();
	}

	public void QuitGame(){
		Application.Quit();
	}

	void HandleHolyRageGUIVisualisation(){
		if(playerS.isInHolyRage){
			AdjustHolyRageVisualisation();
		}
	}

	void HandleCheatInput(){
		if(Input.GetKeyDown(KeyCode.H)){
			playerS.dS.Health = 100;
		}
		if(Input.GetKeyDown(KeyCode.I)){
			playerS.dS.Invincible = !playerS.dS.Invincible;
		}
		if(Input.GetKeyDown(KeyCode.F)){
			playerS.Faith = 100;
		}
		if(Input.GetKeyDown(KeyCode.K)){
			KillAllLootableEnemies();
		}

		/* // is done in myFreeLookCamScript
		if(Input.GetKeyDown(KeyCode.F)){
			camGO.GetComponent<
		}*/
	}

	void KillAllLootableEnemies(){
		foreach(DestructibleScript ds in allLootableEnemies){
			if(!ds.IsDead)
				ds.Die();
		}
	}

	public void EnemyDies(){
		CheckEnemyDeathCount();
		//playerS.Money += 10;
		playerS.Faith += 20;
	}

	void CheckEnemyDeathCount(){

		countDeadEnemies = 0;

		foreach(DestructibleScript ds in allLootableEnemies){
			if(ds.IsDead)
				countDeadEnemies++;
		}

		// Are all enemies dead?
		if(allLootableEnemies.Count == countDeadEnemies) {

			// Change to plunder game mode
			GameMode = 1;
		}

		AdjustEnemyCountVisualisation();

	}

	public void PlayerDies(){
		StartCoroutine(WaitNRestartLevel(5));
	}

	IEnumerator WaitNRestartLevel(float inTime){
		yield return new WaitForSeconds(inTime);
		ReloadLevel();
	}

	public void ReloadLevel(){
		IsPaused = false;

		if(inEditorUsed){
			//TODO:  next Level!
		}else{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
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
					ReloadCamera();
					gameModeCounter = allLootableEnemies.Count*4;
					notificationC.StartPlunderModeUI();
					RenderSettings.skybox = plunderSkyBoxMat;
					break;

				// unexpected
				default:
					break;

			}
		}
	}

	public bool IsPaused{
		get{return isPaused;}

		set{
			isPaused = value;
			if(isPaused){
				Time.timeScale = 0;
			}else{
				Time.timeScale = 1;
			}
		}
	}

	public void InShop(bool inBool){
		isInShop = inBool;
		IsPaused = inBool;
	}

	public void UpdateHealthEnemy(float inValue, Vector3 inPos){
		notificationC.UpdateHealthEnemy(inValue, inPos);
	}

	public void AdjustHealthVisualisation(float inDiff){
		notificationC.AdjustHealthVisualisation(playerS.dS.health);
		notificationC.UpdateHealthPlayer(inDiff, playerS.transform.position);
	}
	public void AdjustFaithVisualisation(){
		notificationC.AdjustFaithVisualisation(playerS.Faith);
	}
	public void AdjustFaithVisualisation(float inDiff, Vector3 inPos){
		AdjustFaithVisualisation();
		if(inDiff>Mathf.Abs(1))InGameController.i.notificationC.UpdateFaith(inDiff, inPos);
	}

	public void AdjustMoneyVisualisation(){
		notificationC.AdjustMoneyVisualisation(playerS.Money);
	}
		
	public void AdjustMoneyVisualisation(float inDiff, Vector3 inPos){
		AdjustMoneyVisualisation();
		notificationC.UpdateMoney(inDiff, inPos);
	}

	public void AdjustEnemyCountVisualisation(){
		notificationC.AdjustEnemyCountVisualisation(countDeadEnemies, allLootableEnemies.Count);
	}

	public void AdjustHolyRageVisualisation(){
		notificationC.AdjustHolyRageVisualisation(playerS.holyRageEnergy);
	}

	public void ShowInGameMessage(string inMessage, bool inCentered, float inDuration){
		if(inCentered)IsPaused=true;
		notificationC.ShowInGameMessage(inMessage, inCentered, inDuration);
	}

}
