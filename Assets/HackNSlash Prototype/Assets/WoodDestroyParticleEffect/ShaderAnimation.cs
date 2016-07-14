using UnityEngine;
using System.Collections;

public class ShaderAnimation : MonoBehaviour {

	public float LoopDuration = 1;
	public int MaterialIndex = 0;
	float r,g,b,correction;
	public bool ApplyToSharedMaterial = true;


	// Update is called once per frame
	void Update () {
		r = Mathf.Sin((Time.time / LoopDuration) * (2 * Mathf.PI)) * 0.5f + 0.25f;
		g = Mathf.Sin((Time.time / LoopDuration + 0.33333333f) * 2 * Mathf.PI) * 0.5f + 0.25f;
		b = Mathf.Sin((Time.time / LoopDuration + 0.66666667f) * 2 * Mathf.PI) * 0.5f + 0.25f;


		correction = 1 / (r + g + b);

		r *= correction;
		g *= correction;
		b *= correction;

		if (ApplyToSharedMaterial) {
			if (GetComponent<Renderer> ().sharedMaterials [MaterialIndex] != null) {
				GetComponent<Renderer> ().sharedMaterials [MaterialIndex].SetVector ("_ChannelFactor", new Vector4 (r, g, b, 0));
			} else {
				Debug.LogError ("Material-Index " + MaterialIndex.ToString () + " not found. Please specify correct index.");
			}
		} else {
			if (GetComponent<Renderer> ().material != null) {
				GetComponent<Renderer> ().material.SetVector ("_ChannelFactor", new Vector4 (r, g, b, 0));
			}
		}
	}
}
