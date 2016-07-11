using UnityEngine;
using System.Collections;

public class AutoStartNotification : TriggerBase {

	[Tooltip("Should something happen on start?. If yes NotificationName happens.")]
	public bool triggerOnStart = true;
	[Tooltip("The delay after which the start trigger happens.")]
	public float startDelay=0.05f;

//	[Tooltip("Should something happen when some notification has happened x times?.")]
//	public bool triggerOnNotificationCount = true;
//	[Tooltip("For which notifications should we look for.")]
//	public string NotificationToCount="";
//	[Tooltip("How many such  notifications are needed.")]
//	public int NotificationCountTarget=0;
//	private int notificationCount=0;
//	[Tooltip("What trigger should happen, when the notifications were there.")]
//	public string TriggerAfterCount="";




	// Use this for initialization
	void Start () {


		StartCoroutine (GetNotification (startDelay));	
	}

    IEnumerator GetNotification(float waitTime)
    {
        if (gameLogic == null)
        {
            gameLogic = GameObject.Find("_GameLogic").GetComponent<GameLogic>();
        }
        yield return new WaitForSeconds(waitTime);
        if (NotificationName.Length > 0)
        {
           // gameLogic.AddNotification(NotificationName, NotificationArgument, this.gameObject.transform.position, GameLab.NotficationCenter.NotificationPriority.ThrowAwayAfterProcessing);
        }

    }




//	public void activateObjects(string name){
//		GameObject environment;
//		environment = GameObject.Find ("Environment");
//		int count = environment.transform.childCount;
//		for (int i=0; i<count; i++) {
//			if(environment.transform.GetChild(i).gameObject.name==name){
//				environment.transform.GetChild(i).gameObject.SetActive(true);
//			}
//
//		}
//	}

	
	// Update is called once per frame
}

