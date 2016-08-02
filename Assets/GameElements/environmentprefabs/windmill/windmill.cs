using UnityEngine;
using System.Collections;

public class windmill : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		Transform trf = transform.Find("windwheele");
		if (trf!=null) {
			GameObject obj = trf.gameObject;
			obj.transform.Rotate(new Vector3(-0.5f,0.0f,0.0f));
		}
	}
}
