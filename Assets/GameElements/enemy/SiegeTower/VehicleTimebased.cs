using UnityEngine;
using System.Collections;

public class VehicleTimebased : GameElementBasedInterval {

	// public GameObject spear;
	Vector3 position = new Vector3();

	// public string stateVehicle = "drive"; // driving now .. 

	// Vector3 posHistory = new Vector3();

	public GameObject spears;

	public GameObject rootObject;
	Vector3 localRotation = new Vector3();
	public GameObject wheelFront;
	public GameObject wheelBack;

	void Start() {
		SetIntervalTime(0.5f);
	}

	// float shake = 0.0f;

	// FixedUpdate
	void FixedUpdate() { 
		
		if (gameElement!=null)
		if ((gameElement.argument.Equals(""))||(gameElement.argument.Equals("drive"))) {

			if (wheelFront!=null) {
				wheelFront.transform.Rotate(3.0f,0.0f,0.0f);
			}
			if (wheelBack!=null) {
				wheelBack.transform.Rotate(3.0f,0.0f,0.0f);
			}

			Vector3 posT = transform.position;
			Rigidbody rb = transform.GetComponent<Rigidbody>();
			rb.MovePosition(  posT + transform.forward * 0.01f );

			// shake
			if (rootObject!=null) {
				// rootObject.transform.Rotate(0.1f,0.0f,0.0f);
				rootObject.transform.localRotation = Quaternion.Euler ( new Vector3(localRotation.x   - Mathf.Sin(Time.time*3)*1.0f , localRotation.y + Mathf.Sin(Time.time)*3.0f, localRotation.z  + Mathf.Sin(Time.time*4)*1.0f ));
			}

			UpdatePositionToElement();
		
		}

	}

	void Update () {

		if (gameElement!=null)
		if ((gameElement.argument.Equals(""))||(gameElement.argument.Equals("drive"))) {
			
			if (Time.time>timeForNextInterval) {
				timeForNextInterval = Time.time + interval;
				OnInterval();
				intervalCounter++;
			}
		
		}

	}

	// Update used for all

	public override void OnInterval() {

		if (gameElement!=null)
		if ((gameElement.argument.Equals(""))||(gameElement.argument.Equals("drive"))) {
			
			// ok take ... it
			if (FirstInterval()) {
				if (spears!=null) {
					position.x = spears.transform.localPosition.x;
					position.y = spears.transform.localPosition.y;
					position.z = spears.transform.localPosition.z;

					localRotation.x = rootObject.transform.localRotation.x;
					localRotation.y = rootObject.transform.localRotation.y;
					localRotation.z = rootObject.transform.localRotation.z;
				}
			} 

			if (intervalCounter%2==0) {
				// inside
				SetPosition(0.0f);
			}
			if (intervalCounter%2==1) {
				// inside
				// SetPosition(4.0f);
				AddNotificationHere("visual","woodexplosion",0.0f,"",new Vector3(1.5f,0.0f,0.0f));
			}
			if (intervalCounter%2==1) {
				// inside
				SetPosition(0.5f);
			}
		}
	

	}

	void SetPosition( float post ) {
		if (spears!=null) {
			spears.transform.localPosition = new Vector3( position.x,  position.y, position.z + post );
		}
	}
}
