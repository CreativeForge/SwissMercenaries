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
	EnemyScript eS;

	void Awake() {
		nextSpawnTime = Time.time + Random.Range(minTimeInterval, maxTimeInterval);
		eS = GetComponent<EnemyScript>();
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
				Instantiate(EnemyPrefab, spawnPosT.position, EnemyPrefab.transform.rotation);
			//}

			nextSpawnTime = Time.time + Random.Range(minTimeInterval, maxTimeInterval);
		}
	}
}
