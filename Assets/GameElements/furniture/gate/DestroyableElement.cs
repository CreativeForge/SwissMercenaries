using UnityEngine;
using System.Collections;

public class DestroyableElement : GameElementBased {

	public GameObject[] arrElements; // destroy first > end 0-x

	int index = 0;

	public override void OnPlayerHit() {

		// Debug.Log("DestroyableElement.OnPlayerHit() ");

		if (gameElement!=null) {
			if (index<arrElements.Length) {
				// wood explosion
				AddNotification( "visual.woodexplosion", "self", 0.0f, "" );
				GameObject obj = arrElements[index];
				obj.SetActive(false);
				// add on
				index++;
			} else {
				// deactivate ... 
			}
		
		}
	}

}

