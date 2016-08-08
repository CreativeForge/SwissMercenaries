using UnityEngine;
using System.Collections;

public class DestroyableSpawner : GameElementBased {

	/*
	 * Destroyable
	 * 
	 * */
	public GameObject[] arrElements; // destroy first > end 0-x

	int index = 0;

	public override void OnPlayerHit() {

		Debug.Log("DestroyableElement.OnPlayerHit() ");

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
				spawnerActive = false;
			}

		}
	}

	/*
	 * SPAWNER
	 * 
	 */
	bool spawnerActive = true;

	bool started = false;
	float timeToSpawn = 0.0f;

	float intervalTime = 3.0f;
	string typeToSpawn = "env.conifer";

	// Update is called once per frame
	void FixedUpdate () {
	
		if (gameLogic!=null)
		if (gameLogic.CheckIngameState()) {
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
					if (spawnerActive) {
						Debug.Log("Spawner.FixedUpdate().FixedUpdate() // Do: "+typeToSpawn+"/"+intervalTime);

						// xyz,abc
						string[] arr = typeToSpawn.Split(',');
						string typeToSpawnXY = typeToSpawn; // "";
						/*if (arr.Length==1) {
							typeToSpawnXY= typeToSpawn;
						} */
						if (arr.Length>1) {
							int index = (int) Random.Range(0,arr.Length);	
							typeToSpawnXY = arr[index];
						}
						gameLogic.levelEditor.notificationCenter.AddNotification( "object.create", "vector", 0.0f, ""+typeToSpawnXY, gameElement.gameObject.transform.position );
						timeToSpawn = Time.time + intervalTime;
					}
				}
			}
		}
		}
	}
}
