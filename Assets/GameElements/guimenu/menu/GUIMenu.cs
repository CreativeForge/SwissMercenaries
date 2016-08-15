using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIMenu : GameElementBased {

	bool started = false;

	public Canvas menuCanvas;

	public GameObject menuPoint;

	string[] arrMenuPoints;
	string[] arrTexts;
	string[] arrTargets;

	string hoveredMenupoint = "";

	// mouseover or ! selection
	void HoverMenuPoint( string keypoint ) {

	}
	// activate menupoint
	void ActivateMenuPoint( string keypoint ) {
		
	}


	ArrayList arrButtons = new ArrayList();

	// Update is called once per frame
	void FixedUpdate () {

		if (gameElement!=null) {
			if (!started) {
				started = true;
				// ADD IT
				if (menuPoint!=null) {
					Debug.Log("GUIMenu.FixedUpdate() // Create new ...");


					// targets
					arrMenuPoints = gameElement.argument.Split(',');
					arrTexts = gameElement.argument.Split(',');
					arrTargets = gameElement.argumentsub .Split(',');

					float startPointX = -200.0f;
					float startPointY = 140.0f;
					for (int z=0;z<arrMenuPoints.Length;z++) {
						// do it
						string strMenupoint = arrMenuPoints[z];
						GameObject mp=Instantiate(menuPoint, new Vector3(0.0f,0.0f,0.0f), new Quaternion()) as GameObject;
						GUIMenuPoint gmp = mp.GetComponent<GUIMenuPoint>();
						RectTransform rt = mp.GetComponent<RectTransform>();
						rt.anchoredPosition = new Vector3(startPointX, startPointY);
						// .SetSize(size);
						gmp.SetText(""+strMenupoint);
						mp.name = "MENUFOUND"+strMenupoint;
						// add ..
						mp.transform.SetParent(menuCanvas.transform, false);
						// arrButtons
						arrButtons.Add(mp);

						startPointY = startPointY - 70.0f;


					}
				}

			} else {

			}
		}
	}
}
