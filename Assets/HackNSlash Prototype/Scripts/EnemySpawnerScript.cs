using UnityEngine;
using System.Collections;

public class EnemySpawnerScript : MonoBehaviour {
	
	public GameObject EnemyPrefab;
	public bool spawnerEnabled = false;
	public float minTimeInterval = 3f;
	public float maxTimeInterval = 7f;

	public Transform spawnPosT;
	private float nextSpawnTime;
	DestructibleScript dS;
	NPCScript eS;

	void Awake() {
		nextSpawnTime = Time.time + Random.Range(minTimeInterval, maxTimeInterval);
		eS = GetComponent<NPCScript>();
		dS = GetComponent<DestructibleScript>();
	}

	// Update is called once per frame
	void Update () {
		if (!spawnerEnabled || !eS.playerIsInTrigger || (dS && dS.IsDead)) {
			return;
		}

		if (Time.time >= nextSpawnTime) {
			//int numToSpawn = Random.Range(1, 3);

			//for (var i = 0; i < numToSpawn; i++) {
				GameObject xob = (GameObject)Instantiate(EnemyPrefab, spawnPosT.position, EnemyPrefab.transform.rotation);
				// add to level
				GameObject level = GameObject.Find("level");
				if (level!=null) {
					xob.transform.parent = level.transform;
				}
			//}

			nextSpawnTime = Time.time + Random.Range(minTimeInterval, maxTimeInterval);
		}
	}
}
