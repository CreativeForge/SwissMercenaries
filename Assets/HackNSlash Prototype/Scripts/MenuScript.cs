using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuScript : MonoBehaviour {

	GameLogic mainLogic;
	int levelCount = 1;

	// Screens between levels
	public Sprite[] screenImages;

	void Awake() {
		
		this.GoBack();

	}

	void DebugX() {
		Debug.Log("DEBUGGING GUI");
	}

	// Search for GameLogic
	void Start() {
		
		mainLogic = FindObjectOfType<GameLogic>();
		/*
		if(!mainLogic) {


			Debug.LogWarning("[MenuScript] Could not find an active GameLogic in scene!");
			gameObject.SetActive(false);

		} else */
		
			if (true)
		{

			// Reference object for new button instances
			GameObject clone;

			Transform buttonView = GetChildByNameRecursive(transform, "LevelContent").transform;
			GameObject levelButton = GetChildByNameRecursive(transform, "LevelButton");

			// Count levels
//			levelCount = mainLogic.GetLevelCount	// INEXISTENT (would like to have...)
			levelCount = 7;

			// First level always displayed
			Debug.Log(levelButton.name);
			levelButton.SetActive(true);
			levelButton.transform.FindChild("Text").GetComponent<Text>().text = "Level 1";
			levelButton.GetComponent<Button>().onClick.AddListener(delegate { LoadLevel(1); });

			if (false)
			for(int i = 2;i <= levelCount;i++) {

				clone = Instantiate(levelButton, levelButton.transform.position, Quaternion.identity) as GameObject;	// Create button
				clone.transform.parent = levelButton.transform.parent;													// Share parent / same layer
				clone.transform.FindChild("Text").GetComponent<Text>().text = "Level " + i.ToString();					// Edit button text
				clone.GetComponent<RectTransform>().anchoredPosition = new Vector2(										// Button position
					clone.GetComponent<RectTransform>().anchoredPosition.x,
					clone.GetComponent<RectTransform>().anchoredPosition.y - (float)(i - 1) * 60f
				);
				levelButton.GetComponent<Button>().onClick.AddListener(delegate { LoadLevel(i); });						// Button click event

			}

			buttonView.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (levelCount * 60) + 20);				// Size of the scrollview

		}

	}

	void Update() {

		// If screen between scenes is displayed
		if(transform.FindChild("Screen").gameObject.activeInHierarchy) {

			// User is able to skip -> next level
			if(Input.GetKeyDown(KeyCode.Mouse0) || Input.GetButtonDown("Jump")) {

				mainLogic.levelEditor.LoadNextInGameLevel();

			}
		
		}

	}

	// Invoke LoadGameLevel of GameLogic
	public void LoadLevel(int level) {

		mainLogic.levelEditor.LoadInGameLevel(level);

	}

	// Main menu screen
	public void GoBack() {

		transform.FindChild("Main").gameObject.SetActive(true);
		transform.FindChild("Credits").gameObject.SetActive(false);
		transform.FindChild("Levels").gameObject.SetActive(false);
		transform.FindChild("Screen").gameObject.SetActive(false);

	}

	// Credits screen
	public void Credits() {

		transform.FindChild("Main").gameObject.SetActive(false);
		transform.FindChild("Credits").gameObject.SetActive(true);
		transform.FindChild("Levels").gameObject.SetActive(false);
		transform.FindChild("Screen").gameObject.SetActive(false);

	}

	// Level select screen
	public void Levels() {

		transform.FindChild("Main").gameObject.SetActive(false);
		transform.FindChild("Credits").gameObject.SetActive(false);
		transform.FindChild("Levels").gameObject.SetActive(true);
		transform.FindChild("Screen").gameObject.SetActive(false);

	}

	// Test screen-levels
	public void ShowScreen(int screenNumber) {

		// Reference to screen panel
		GameObject screenLayer = transform.FindChild("Screen").gameObject;

		transform.FindChild("Main").gameObject.SetActive(false);
		transform.FindChild("Credits").gameObject.SetActive(false);
		transform.FindChild("Levels").gameObject.SetActive(false);
		transform.FindChild("Screen").gameObject.SetActive(true);

		screenLayer.GetComponent<Image>().sprite = screenImages[screenNumber];
		screenLayer.SetActive(true);

	}

	// Recursively find a child gameobject by name
	private GameObject GetChildByNameRecursive(Transform currentTransform, string childName)
	{		
		GameObject foundChild = null;

		Transform[] children = currentTransform.GetComponentsInChildren<Transform>(true);

		foreach(Transform child in children) {
			
			if(child.name == childName)
				return child.gameObject;

		}

		return null;
	}
}
