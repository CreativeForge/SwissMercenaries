using UnityEngine;
using System.Collections;

public class AudioHolderScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void Play (string inName) {
		transform.FindChild(inName).GetComponent<AudioInstantiateScript>().Play();
	}
}
