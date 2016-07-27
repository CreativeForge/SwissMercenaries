using UnityEngine;
using System.Collections;

public class MetaTitle: GameElementBased {

	// after set game element!
	public override void OnGameElementInit() {

		Debug.Log("MetaTitle.OnGameElementInit()");
		// OnGameElementInitPathFollowing();

		AddNotification( "message.notify", "", 0.0f, gameElement.argument );

	}

}
