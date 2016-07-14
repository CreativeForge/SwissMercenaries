using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour
{
	[Range(0.0f, 45)]
	public float
		secToDestroy = 10f;

	// Use this for initialization
	void Start ()
	{
		StartCoroutine (destorySoon ());
	}
	

	private IEnumerator destorySoon ()
	{
		yield return new WaitForSeconds (secToDestroy);
		Destroy (this.gameObject);
	}

}
