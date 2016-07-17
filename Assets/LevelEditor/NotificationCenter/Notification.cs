using UnityEngine;
using System.Collections;

namespace GameLab.NotficationCenter {

	// Notification < NotificationType
	public class Notification {

		public string state = ""; // "": active / done / "deleted" - delete as soon as possible

		public string type = ""; // no enum > string for filtering !!!!
		public string subtype = "";

		public string targetName = "";

		public string argument = ""; // for example the audioclip

		// recursive ?
		ArrayList arrAttachedNotifications = new ArrayList();

		// timed till executed!
		public float timed = 0.0f; // absolute  
		public float timeStop = 0.0f;


		// attached gameobject
		GameObject gameObject = null;

		// target
		public GameObject targetObj = null; // starting
		public GameObject movingToObj = null; // movingTo
		public Vector3 targetPoint;

		// evaluationLayser player / enemies ... 
		public string evaluationLayer = ""; // is there a layer for evaluation for this?
		public string evaluationLayerSub = ""; // is there a layer for evaluation for this?


		public Notification Copy() {
			Notification copyX = new Notification ();

			copyX.state = state;
			copyX.type = type;
			copyX.subtype = subtype;
			copyX.argument = argument;

			copyX.timed = timed;

			copyX.gameObject = gameObject;

			copyX.targetName = targetName;
			copyX.targetObj = targetObj;
			copyX.movingToObj = movingToObj;

			copyX.targetPoint = new Vector3(targetPoint.x,targetPoint.y,targetPoint.z);

			copyX.evaluationLayer = evaluationLayer;
			copyX.evaluationLayerSub = evaluationLayerSub;

			// copyX.state = state;
			return copyX;
		}

		/*
		public string GetDebugString ( ) {

			return "{"+timestamp+"} "+key+" "+argument+" "+point+"";

		}
		*/

	}

}