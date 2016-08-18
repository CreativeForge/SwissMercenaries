using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CamCutCaption : GameElementBasedTimed {

	public Text scriptText; 
	public Text scriptTextShadow; 

	public override void OnInitGameElementTimed() {

		if (visualGameObject!=null) {
			visualGameObject.SetActive(false);
		}
		UpdateText( );
	}

	public override void OnAwakeGameElementTimed() {
	
		if (visualGameObject!=null) {
			visualGameObject.SetActive(false);
		}
		UpdateText( );
	}
	public override void OnActivateGameElementTimed() {
		if (visualGameObject!=null) {
			visualGameObject.SetActive(true);
		}
		UpdateText();
		Debug.Log("CamCutCaption.OnActivateGameElementTimed().UpdateText() "+gameElement.name+"/"+gameElement.argument);
	}
	public override void OnDeactivateGameElementTimed() {

		if (visualGameObject!=null) {
				visualGameObject.SetActive(false);
			}

	}

	void UpdateText() {
		// Debug.Log("CamCutCaption.UpdateText() "+gameElement.name+"/"+gameElement.argument);
		// visualGameObject.SetActive(true);
		if (scriptText!=null) {
			// Debug.Log("CamCutCaption.UpdateText() "+gameElement.name+"/"+gameElement.argument);
			scriptText.text = gameLogic.levelEditor.ParseText( gameElement.argument+"" ); // ""+ gameElement.timed +" "
			scriptTextShadow.text = gameLogic.levelEditor.ParseText(  gameElement.argument );
		}
	}


	public override void OnGameStart() {

		// use this!!
		base.OnGameStart();
		UpdateText( );

	}

}
