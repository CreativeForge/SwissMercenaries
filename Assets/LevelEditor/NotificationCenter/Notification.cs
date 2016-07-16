using UnityEngine;
using System.Collections;

namespace GameLab.NotficationCenter {

	// Notification < NotificationType
	public class Notification {

		public string state = ""; // "": active / "deleted" - delete as soon as possible

		public string type = ""; // no enum > string for filtering !!!!
		public string subtype = "";

		public string argument = ""; // for example the audioclip

		// recursive ?
		ArrayList arrAttachedNotifications = new ArrayList();

		// timed till executed!
		public float timestamp = 0.0f; // absolute  

		// attached gameobject
		GameObject gameObject = null;

		// target
		public GameObject targetObj = null; // starting
		public GameObject movingToObj = null; // movingTo
		public Vector3 targetPoint;

		// evaluationLayser player / enemies ... 
		public string evaluationLayer = ""; // is there a layer for evaluation for this?
		public string evaluationLayerSub = ""; // is there a layer for evaluation for this?

		/*
		public string GetDebugString ( ) {

			return "{"+timestamp+"} "+key+" "+argument+" "+point+"";

		}
		*/

	}

}