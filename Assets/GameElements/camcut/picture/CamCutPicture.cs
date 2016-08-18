using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CamCutPicture : GameElementBasedTimed {

	public Sprite picturePrefab;

	public Image imageScript;

	public override void OnInitGameElementTimed() {


		// scriptText.text = ""+ gameElement.argument+""; 
		if (picturePrefab!=null) {
			if (imageScript!=null) {
				imageScript.sprite = picturePrefab;
			}
		}
	}

	public override void OnAwakeGameElementTimed() {

		if (visualGameObject!=null) {
			visualGameObject.SetActive(false);
		}
	}

	public override void OnActivateGameElementTimed() {

		if (visualGameObject!=null) {
			visualGameObject.SetActive(true);
		}
	}

	public override void OnDeactivateGameElementTimed() {

		if (visualGameObject!=null) {
			visualGameObject.SetActive(false);
		}

	}

	/*
	public override void OnGameStart() {

		// use this!!
		base.OnGameStart();

	}
	*/

}
