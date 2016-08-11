using UnityEngine;
using System.Collections;

public class RandomActive : MonoBehaviour {

	public GameObject[] arrObjects;

	// Use this for initialization
	void Start () {
		if (arrObjects.Length>0) {
			int activeIndex = UnityEngine.Random.Range(0,arrObjects.Length);
			for (int i=0;i<arrObjects.Length;i++) {
				if (activeIndex==i) {
					
				} else {
					arrObjects[i].SetActive(false);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
