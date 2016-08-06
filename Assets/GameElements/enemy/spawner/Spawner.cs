using UnityEngine;
using System.Collections;

public class Spawner : GameElementBased {

	bool started = false;
	float timeToSpawn = 0.0f;

	float intervalTime = 3.0f;
	string typeToSpawn = "env.conifer";

	// Update is called once per frame
	void FixedUpdate () {
	
		if (gameElement!=null) {
			// check it ..
			if (!started) {
				started = true;
				Debug.Log("Spawner.FixedUpdate().FixedUpdate() // ["+gameElement.argument+"/"+gameElement.argumentsub+"] // "+typeToSpawn+"/"+intervalTime);
				typeToSpawn = gameElement.argument;
				try {
					float iintervalTime = float.Parse(gameElement.argumentsub);
					intervalTime = iintervalTime;
				} catch {
				}
			} else {
				// do it now ... 
				if (Time.time>timeToSpawn) {
					Debug.Log("Spawner.FixedUpdate().FixedUpdate() // Do: "+typeToSpawn+"/"+intervalTime);
					gameLogic.levelEditor.notificationCenter.AddNotification( "object.create", "vector", 0.0f, ""+typeToSpawn, gameElement.gameObject.transform.position );
					timeToSpawn = Time.time + intervalTime;
				}
			}
		}

	}
}
