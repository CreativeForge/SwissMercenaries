using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

	public Transform rotCenterT;
	Vector3 targetPos;

	bool isFlying = true;
	bool hasHitMonster = false;
	float rotX=0;

	// Use this for initialization
	void Start () {
		targetPos = GameLogicControllerScript.i.playerS.transform.position;
		rotCenterT.parent = null;
		rotCenterT.position = GetCenterPoint();
		rotCenterT.LookAt(new Vector3(targetPos.x,rotCenterT.position.y,targetPos.z) );
		transform.LookAt(rotCenterT);
		transform.parent = rotCenterT;
	}

	Vector3 GetCenterPoint(){
		Vector3 dir = targetPos - transform.position;
		Vector3 middlePoint = transform.position + dir*0.5f;
		return new Vector3(middlePoint.x,transform.position.y-10,middlePoint.z);
	}

	// Update is called once per frame
	void Update () {

		if(isFlying) {
			rotX += 90*Time.deltaTime;
			rotCenterT.Rotate(90*Time.deltaTime,0,0);
			if(rotX>300)Destroy(rotCenterT.gameObject);
		}
	}

}
