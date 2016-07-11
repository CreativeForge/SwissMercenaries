using UnityEngine;
using System.Collections;

namespace GameLab.NotficationCenter {

	public class Notification {

		public string key=""; // no enum > string for filtering !!!!
		public string argument = "";
		public Vector3 point = new Vector3 ();
		public float timestamp = 0.0f; // absolute  

		public string GetDebugString ( ) {

			return "{"+timestamp+"} "+key+" "+argument+" "+point+"";

		}

	}

}