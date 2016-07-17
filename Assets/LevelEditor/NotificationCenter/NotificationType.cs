using UnityEngine;
using System.Collections;


// NotificationType > Notification
[System.Serializable]
public class NotificationType {

	public string subtype = "";
	public string argument = ""; // for example the audioclip

	public GameObject prefabGameObject = null;
	public int editorIndex = 1; // editor index

}
