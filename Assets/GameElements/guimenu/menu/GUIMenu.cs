using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * GUIMenu (argument > splitted)
 *  |_GUIMenuPoint
 *  |_GUIMenuPoint
 *  |_GUIMenuPoint
 *  |_GUIMenuPoint
 * 
 * */

public class GUIMenu : GameElementBased {

	bool started = false;

	public Canvas menuCanvas;

	public GameObject menuPointPrefab;

	string[] arrMenuPoints;
	string[] arrTexts;
	string[] arrCommands;

	// ... take first ...
	string hoveredMenupoint = "";

	// mouseover or ! selection
	/*
	 * void HoverMenuPoint( string keypoint ) {

		// disable all the others

		// show special ...


	}
	*/
	// activate menupoint
	/*
	 * void ActivateMenuPoint( string keypoint ) {
		
	}
	*/

	ArrayList arrButtons = new ArrayList();

	// Update is called once per frame
	void FixedUpdate () {
		bool debugThis = false;
		if (gameElement!=null) {
			if (!started) {
				started = true;
				// ADD IT
				if (menuPointPrefab!=null) {
					if (debugThis) Debug.Log("GUIMenu.FixedUpdate() // Create new ...");



					// UpdateButtons();

				}

			} else {

			}
		}
	}

	public override void OnGameStart() {
		Debug.Log("GUIMEnu.OnGameStart()");
		UpdateButtons();
	}

	public void UpdateButtons() {

		bool debugThis = true;

		// targets
		arrMenuPoints = gameElement.argument.Split(',');
		arrTexts = gameElement.argument.Split(',');
		arrCommands = gameElement.argumentsub .Split(',');

		float startPointX = -200.0f;
		float startPointY = 140.0f;
		for (int z=0;z<arrMenuPoints.Length;z++) {
			// do it
			string strMenupoint = arrMenuPoints[z];
			string strText = arrMenuPoints[z];
			// parse
			strText=""+gameLogic.levelEditor.ParseText(""+strText);
			// debug
			if (gameLogic.levelEditor.GetIngameDebug()) {
				string addOn = "";
				string deb = SearchCommandMenuPoint(strMenupoint);
				if (deb!=null) {
					addOn = " ["+deb+"]";
				} else {
					addOn = " [problems]";
				}
				strText = strText + addOn;
			}
			arrTexts[z] = strText;
			if (debugThis) Debug.Log("GUIMenu.FixedUpdate() // CREATE MENUPOINTS "+strMenupoint);
			GameObject mp=Instantiate(menuPointPrefab, new Vector3(0.0f,0.0f,0.0f), new Quaternion()) as GameObject;
			GUIMenuPoint gmp = mp.GetComponent<GUIMenuPoint>(); // button
			RectTransform rt = mp.GetComponent<RectTransform>();
			rt.anchoredPosition = new Vector3(startPointX, startPointY);
			// .SetSize(size);
			gmp.SetText(""+strText);
			mp.name = "MENUFOUND"+strMenupoint;
			// add ..
			mp.transform.SetParent(menuCanvas.transform, false);
			// arrButtons
			arrButtons.Add(mp);

			// listener
			Button buttonScript = mp.GetComponent<Button>();
			buttonScript.onClick.AddListener(() => ButtonAction(""+strMenupoint));

			startPointY = startPointY - 70.0f;

			// 
		}

	}



	// destroy all ...
	void Destroy()
	{
		
		if (gameElement!=null) {
			if (!started) {
				started = true;
				// ADD IT
				if (menuPointPrefab!=null) {
					for (int z=0;z<arrMenuPoints.Length;z++) {
						string strMenupoint = arrMenuPoints[z];
						GameObject mp = (GameObject) arrButtons[z];
						Button buttonScript = mp.GetComponent<Button>();
						buttonScript.onClick.RemoveListener(() => ButtonAction(""+strMenupoint));
					}
				}
			}
		}
	}

	string SearchCommandMenuPoint ( string menupointKey ) {
		// Debug.Log("GUIMenu.SearchCommandMenuPoint("+menupointKey+") // arrCommands.Length = "+arrCommands.Length);
		for (int z=0;z<arrMenuPoints.Length;z++) {
			string strMenupoint = arrMenuPoints[z];
			if (menupointKey.Equals(strMenupoint)) {
				if (z<arrCommands.Length) {
					return arrCommands[z];
				}
			}
		}

		return null;
	}

	void ButtonAction( string menupoint )
	{
		Debug.Log("GUIMenu.ButtonAction()"+menupoint);

		// command
		string command = SearchCommandMenuPoint( menupoint );
		if (command==null) {
			Debug.LogError("LevelEditor.GUIMEnu.ButtonAction("+menupoint+" > "+command+") // NOT FOUND!!");
		}
		if (command!=null) {

		// search menupoint now
		bool parsed = false;

		// LEVELID
		// parse now
		try {
			int levelToLoad = int.Parse(command);
			parsed = true;
			// gameLogic.levelEditor.LoadLevel(levelToLoad);
			gameLogic.SetGameLevel(levelToLoad);
		} catch {
			
		}

		// WEB
		// case: http://
		if (!parsed) {
			if (command.IndexOf("http://")!=-1) {
					Application.OpenURL(""+command);	
					parsed = true;
			}
		}

		// EXIT-COMMAND
		if (!parsed) {
			if (command.Equals("exit")) {
				Application.Quit()	;
				parsed = true;
			}
		}

		// case: notification
		// it is a notification!
		if (!parsed) {
			// LOG
			Debug.Log("LevelEditor.GUIMEnu.ButtonAction("+menupoint+" > "+command+") // NOTIFICATION ");
			// ADD 
			AddNotification( command, "self", 0.0f, "" );

		}
		
		}

	}

}
