using UnityEngine;
using System.Collections;

public class ParticleSystemAutoDestroyScript : MonoBehaviour {


	private void Start()
	{
		ParticleSystem pS = GetComponent<ParticleSystem>();
		Destroy(gameObject, pS.duration+pS.startLifetime); 
	}

}