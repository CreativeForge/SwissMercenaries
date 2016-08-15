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

					for (int z=0;z<arrMenuPoints.Length;z++) {
						// do it
						string strMenupoint = arrMenuPoints[z];
						GameObject mp=Instantiate(menuPoint, new Vector3(0.0f,0.0f,0.0f), new Quaternion()) as GameObject;
						GUIMenuPoint gmp = mp.GetComponent<GUIMenuPoint>();
						gmp.SetText(""+strMenupoint);
						mp.name = "MENUFOUND"+strMenupoint;
						// add ..
						mp.transform.SetParent(menuCanvas.transform, false);


						RectTransform rt = mp.GetComponent<RectTransform>();
						rt.anchoredPosition = new Vector3(-200.0f,140.0f+z*-70.0f);
							// .SetSize(size);
					}
				}

			} else {

			}
		}
	}
}
