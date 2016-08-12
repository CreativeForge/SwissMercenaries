using UnityEngine;
using System.Collections;

public class Fog : GameElementBased {

	bool started = false;

	void FixedUpdate() {
		if (!started) {
		if (gameElement!=null) {
			started = true;
			RenderSettings.fog = true;
			RenderSettings.fogDensity = 0.0075f;
			RenderSettings.fogStartDistance = 300.0f;
			RenderSettings.fogColor = Color.black;
			// Debug.Log("Fog.FixedUpdate() // STARTED");
			}}
	}
}
