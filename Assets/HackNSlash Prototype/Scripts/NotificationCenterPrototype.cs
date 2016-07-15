using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotificationCenterPrototype : MonoBehaviour {

	public GameObject canvasGO;
	public RectTransform healthT;
	public RectTransform faithT;
	public RectTransform holyRageT;

	public GameObject panelModeChangeGO;
	public GameObject counterTextGO;
	public Text counterText;
	public Text plunderText;
	public Text tutorialText;
	public Text moneyText;
	public Text enemyText;
	public Text inGameMessageTopText;
	public GameObject inGameMessageTopBGGO;
	public Text inGameMessageCenterText;
	public GameObject inGameMessageCenterBGGO;
	public Text inGameMessagePauseText;
	public GameObject pauseButtonQuit;
	public GameObject pauseButtonReload;
	float startMessageTime = 0;

	public GameObject FloatingCombatTextPrefab;

	bool messageIsShowed;
	float messageDuration = 5;

	// Use this for initialization
	void Awake () {		
		canvasGO.SetActive(true);
	}

	void Start(){
		HideAlleInGameMessageObjects();
	}
	
	// Update is called once per frame
	void Update () {
		//if(messageIsShowed && startMessageTime+messageDuration<Time.time){
		//	HideAllInGameMessages();
		//}
	}

	// On GUI drawing
	void OnGUI() {
		if(counterTextGO.activeSelf)
			counterText.text = InGameController.i.gameModeCounter.ToString();

	}

	public void ShowPauseMenu(bool inShow){
		inGameMessagePauseText.gameObject.SetActive(inShow);
		inGameMessageCenterBGGO.SetActive(inShow);
		pauseButtonQuit.SetActive(inShow);
		pauseButtonReload.SetActive(inShow);
	}


	public void StartPlunderModeUI(){
		counterTextGO.SetActive(true);
		plunderText.gameObject.SetActive(true);
		tutorialText.gameObject.SetActive(false);
		moneyText.gameObject.SetActive(true);

		// GUI Flash
		StartCoroutine(FlashGUI(0.4f));
	}


	public void AdjustHealthVisualisation(float inHealth){
		healthT.localScale = new Vector3(inHealth/100f,1,1);
	}
	public void AdjustFaithVisualisation(float inFaith){
		faithT.localScale = new Vector3(inFaith/100f,1,1);
	}
	public void AdjustMoneyVisualisation(uint inMoney){
		moneyText.text = "Gold gesammelt: "+inMoney;
	}

	public void AdjustEnemyCountVisualisation(int inCount, int inAllCount){
		enemyText.text = "Landsknechte getötet: "+inCount+"/"+inAllCount;
	}

	public void AdjustHolyRageVisualisation(float inEnergy){
		holyRageT.localScale = new Vector3(inEnergy/100f,1,1);
		if(Mathf.Round(inEnergy/2) % 2 == 0)
			holyRageT.gameObject.SetActive(true);
		else
			holyRageT.gameObject.SetActive(false);
	}
		
	public void UpdateHealthEnemy(float inValue, Vector3 inPos){

		if(inValue<0)
			ShowCombatText(""+Mathf.Round(inValue),inPos,CombatText.TextType.EnemyDamage);
		else
			ShowCombatText("+"+Mathf.Round(inValue),inPos,CombatText.TextType.EnemyHeal);	
	}
	public void UpdateHealthPlayer(float inValue, Vector3 inPos){

		if(inValue<0)
			ShowCombatText(""+Mathf.Round(inValue),inPos,CombatText.TextType.Damage);
		else
			ShowCombatText("+"+Mathf.Round(inValue),inPos,CombatText.TextType.Heal);	
	}

	public void UpdateMoney(float inValue, Vector3 inPos){
		if(inValue<0)
			ShowCombatText(""+Mathf.Round(inValue),inPos,CombatText.TextType.Money);
		else
			ShowCombatText("+"+Mathf.Round(inValue),inPos,CombatText.TextType.Money);	
	}

	public void UpdateFaith(float inValue, Vector3 inPos){
		if(inValue<0)
			ShowCombatText(""+Mathf.Round(inValue),inPos,CombatText.TextType.Faith);
		else
			ShowCombatText("+"+Mathf.Round(inValue),inPos,CombatText.TextType.Faith);	
	}



	public void ShowCombatText(string value, Vector3 position, CombatText.TextType textType){
		Vector3 screenPos = Camera.main.WorldToScreenPoint (position + new Vector3 (FloatingCombatTextPrefab.GetComponent<CombatText>().OffsetX,
			FloatingCombatTextPrefab.GetComponent<CombatText>().OffsetY,
			FloatingCombatTextPrefab.GetComponent<CombatText>().OffsetZ));
		//screenPos = Camera.main.WorldToScreenPoint(position);
		screenPos.x = screenPos.x / Screen.width;
		screenPos.y = screenPos.y / Screen.height;

		/* this is useless
			// some randomness
			screenPos.x = screenPos.x ; // + UnityEngine.Random.Range(-10,10);
			screenPos.y = screenPos.y ; // + UnityEngine.Random.Range(0,20);
      */

		//screenPos.x =screenPos.x/ Screen.width +0.5f;
		//screenPos.y =screenPos.y/ Screen.height+0.5f;
		GameObject temp;
		CombatText combatText;
		temp = Instantiate (FloatingCombatTextPrefab, screenPos, Quaternion.identity) as GameObject;
		combatText = temp.gameObject.GetComponent<CombatText> ();
		combatText.ShowText (textType,value);
	}

	public void ShowInGameMessage(string inMessage, bool inCentered, float inDuration){
		startMessageTime = Time.time;
		messageIsShowed = true;
		if(inCentered){
			inGameMessageCenterText.gameObject.SetActive(true);
			inGameMessageCenterText.text = inMessage;
			inGameMessageCenterBGGO.SetActive(true);
		}else{
			inGameMessageTopText.gameObject.SetActive(true);
			inGameMessageTopText.text = inMessage;
			inGameMessageTopBGGO.SetActive(true);
		}

		if(inDuration>0)
			StartCoroutine(WaitNHideAllMessages(inDuration));
	}

	IEnumerator WaitNHideAllMessages(float inDuration) {
		yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(inDuration));
		HideAllInGameMessages();
	}
		
	public void HideAllInGameMessages(){
		if(!messageIsShowed)return;

		messageIsShowed = false;
		HideAlleInGameMessageObjects();
		InGameController.i.IsPaused=false;
	}

	void HideAlleInGameMessageObjects(){
		inGameMessageTopText.gameObject.SetActive(false);
		inGameMessageTopBGGO.SetActive(false);
		inGameMessageCenterText.gameObject.SetActive(false);
		inGameMessageCenterBGGO.SetActive(false);
		pauseButtonQuit.SetActive(false);
		pauseButtonReload.SetActive(false);
		inGameMessagePauseText.gameObject.SetActive(false);
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
