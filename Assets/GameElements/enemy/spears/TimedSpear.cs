using UnityEngine;
using System.Collections;

public class TimedSpear : GameElementBasedInterval {

	public GameObject spear;
	public Vector3 position = new Vector3();

	void Start() {
		SetIntervalTime(1.0f);
	}

	public override void OnInterval() {
		// ok take ... it
		if (FirstInterval()) {
			position.x = spear.transform.localPosition.x;
			position.y = spear.transform.localPosition.y;
			position.z = spear.transform.localPosition.z;
		} 

		if (intervalCounter%2==0) {
			// inside
			SetPosition(0.0f);
		}
		if (intervalCounter%2==1) {
			// inside
			SetPosition(4.0f);
		}

	}

	void SetPosition( float post ) {
		spear.transform.localPosition = new Vector3( 0.0f,  0.0f, position.z + post );
	}
}
