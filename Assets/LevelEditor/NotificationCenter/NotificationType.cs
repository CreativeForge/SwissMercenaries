using UnityEngine;
using System.Collections;


// NotificationType > Notification
[System.Serializable]
public class NotificationType {

	public string subtype = "";
	public string argument = ""; // for example the audioclip

	public GameObject ingameObject = null;
	public int editorIndex = 1; // editor index

}
