using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
// using GameLab.HackAndSlashFramework;




namespace GameLab.NotficationCenter
{

	// priority
	public enum NotificationPriority
	{
		Top,
		ThrowAwayAfterProcessing
	}

    public delegate void OnPlayer1Activate();
    public delegate void OnPlayer2Activate();


    public class NotificationCenter : MonoBehaviour
	{

		//Notification has the following convention: Type + object + Event

		public const string TYPE_SOUND = "sound",
			TYPE_EFFECT = "effect",
			TYPE_TUTORIAL ="tutorial",
			TYPE_RECURSION = "recursion";

	  public const string EVENT_HIT_BOOST = ".hit.boost";

      public const string OBJ_ = "sound";
		public const string EVT_ = "sound";
		private Dictionary<string, float> timeTable;
		// debug
		public bool debugOnGUI = true;
		public GUIStyle debugGui;

		// Array Notifications 
		bool storeOnlyTopNotification = true;
		ArrayList arrNotifications = new ArrayList ();

		// Prefabs
		public GameObject nfParticleBlood;
		public GameObject nfExplosion;
		public GameObject nfExplosionDark;
		public GameObject nfExplosionFriend;
		public GameObject nfItemPickup;
		public GameObject nfObjectDestroy;
		public GameObject nfObjectHit;
		public GameObject nfTriggerBig;
		public GameObject nfTriggerMedium;
		public GameObject nfTriggerSmall;		
		public GameObject majorNotificationGUI;
		public GameObject FloatingCombatText;
		public GameObject minorNotificationGUI;
//        public CameraManager CameraUsed;

		//private MajorNotificationGUI majorNotification;
		//private MinorNotificationGUI minorNotification;

		private string lastHit="";
		private int deadEnemies = 0;
		private int deadFriends = 0;
		private int killCountPlayer1 = 0;
		private int killCountPlayer2 = 0;

		private int playerDeathCount = 0;
		private bool isInitialized=false;

		//bool playerIsInActive=false;

		// kills without damage
		private int perfectKillCountPlayer1=0;
		private int perfectKillCountPlayer2=0;

		private List<TriggerOnNotification> listenerList =new List<TriggerOnNotification>();

		private GameLogic gameLogic;
//		private LogicPlayer player1;
//		private LogicPlayer player2;


		// Use this for initialization
		void Start ()
		{
			// GameLogic	gameLogic = GameObject.FindObjectOfType<GameLogic>();

			/*
			LogicPlayer[] gos = GameObject.FindObjectsOfType<LogicPlayer>();
			foreach (LogicPlayer go in gos) {
				if (go.GetCharacter().type == CharacterType.Player1) {
					this.player1 = go;
				}
				
				if (go.GetCharacter().type == CharacterType.Player2) {
					this.player2 = go;
				}
				
			}
            CameraUsed = GameObject.FindObjectOfType<CameraManager>();
			
			// testings 
			if (majorNotificationGUI != null) {
				//majorNotification = majorNotificationGUI.GetComponent<MajorNotificationGUI> ();
			}

			if (minorNotificationGUI != null) {
				//minorNotification = minorNotificationGUI.GetComponent<MinorNotificationGUI> ();
			}

			ResetTimeTable ();
			StartCoroutine(NothingHappens(5.0f));

		*/

		}

		void ResetTimeTable ()
		{
			isInitialized = false;
			timeTable = new Dictionary<string, float> ();
			timeTable.Add ("starttime", Time.time);
			timeTable.Add ("last.event", Time.time);

			timeTable.Add ("weapon.same.player1", Time.time);
			timeTable.Add ("last.kill.player1", -20.0f);
			timeTable.Add ("last.block.player1", -20.0f);
			timeTable.Add ("last.box.player1", -20.0f);
			timeTable.Add ("last.faith.player1", -20.0f);
			timeTable.Add ("last.money.player1", -20.0f);
			timeTable.Add ("damage.taken.player1", float.MaxValue);
			timeTable.Add ("being.alone.player1", float.MaxValue);
			timeTable.Add ("walking.wall.player1", float.MaxValue);

			timeTable.Add ("weapon.same.player2", Time.time);
			timeTable.Add ("last.kill.player2", -20.0f);
			timeTable.Add ("last.block.player2", -20.0f);
			timeTable.Add ("last.box.player2", -20.0f);
			timeTable.Add ("last.faith.player2", -20.0f);
			timeTable.Add ("last.money.player2", -20.0f);
			timeTable.Add ("damage.taken.player2", float.MaxValue);
			timeTable.Add ("being.alone.player2", float.MaxValue);
			timeTable.Add ("walking.wall.player2", float.MaxValue);


			timeTable.Add ("cooldown.gained.health", -60.0f);
			timeTable.Add ("cooldown.gained.money", -60.0f);
			timeTable.Add ("cooldown.gained.faith", -60.0f);
			timeTable.Add ("cooldown.gained.spirit", -60.0f);
			timeTable.Add ("cooldown.spirit.high", -60.0f);
			timeTable.Add ("cooldown.spirit.low", -60.0f);
			timeTable.Add ("cooldown.health.low", -60.0f);
			timeTable.Add ("cooldown.faith.full", -60.0f);


			timeTable.Add ("cooldown.damage.taken", -60.0f);
			timeTable.Add ("cooldown.money.low", -60.0f);
			timeTable.Add ("cooldown.weapon.switched", -60.0f);
			timeTable.Add ("cooldown.comment", -60.0f);

			lastHit="";
			deadEnemies = 0;
			deadFriends = 0;
			killCountPlayer1 = 0;
			killCountPlayer2 = 0;
			isInitialized = true;
		}


		public void RegisterListener(TriggerOnNotification newTriggerScript){
			listenerList.Add (newTriggerScript);
		}

		public void UnRegisterListener(TriggerOnNotification newTriggerScript){
			listenerList.Remove (newTriggerScript);
		}

		// Update is called once per frame
		void Update ()
		{

		}

		// FixedUpdate
		void FixedUpdate ()
		{

			// commentator update check
			//CommentatorUpdate ();
		}

		/*
		 * NotificationCenter
		 * 
		 * */



		// AddNotification
		public void AddNotification (string notificationEvent, Vector3 notificationPoint, NotificationPriority notificationPriority)
		{

			ProcessNotification (notificationEvent, "", "", notificationPoint, notificationPriority);

		}

		public void AddNotification (string notificationEvent, string notificationArgument, Vector3 notificationPoint, NotificationPriority notificationPriority)
		{

			ProcessNotification (notificationEvent, notificationArgument, "", notificationPoint, notificationPriority);

		}

		public void AddNotification (string notificationEvent, string notificationArgument, string notificationOrigin, Vector3 notificationPoint, NotificationPriority notificationPriority)
		{

			ProcessNotification (notificationEvent, notificationArgument, notificationOrigin, notificationPoint, notificationPriority);

		}

		void ProcessNotification (string notificationEvent, string notificationArgument, string notificationOrigin, Vector3 notificationPoint, NotificationPriority notificationPriority)
		{
			// Debug.Log ("ProcessNotification ("+ notificationEvent +","+  notificationArgument +"," + notificationOrigin+")");

			notificationEvent = notificationEvent.ToLower ();
			//notificationArgument = notificationArgument.ToLower ();
			notificationOrigin = notificationOrigin.ToLower ();

			bool debugThisMethode = false;
			if (debugThisMethode) {
				Debug.Log ("ProcessNotification ( " + notificationEvent + ", " + notificationPoint + " , " + notificationPriority.ToString () + " )");
			}

			string notificationType = notificationEvent.Split ('.') [0];
			if (notificationEvent.StartsWith ("[")) {
				notificationType = TYPE_RECURSION;
			}
			string notificationMsg = notificationEvent.Substring (notificationEvent.IndexOf (".") + 1);

			switch (notificationType) {

			case TYPE_SOUND:
				Play3DSound (notificationMsg, notificationPoint);
				break;

			case TYPE_EFFECT:
				DoEffect (notificationMsg, notificationArgument, notificationOrigin, notificationPoint, notificationPriority);
				break;

			case TYPE_TUTORIAL:
				DoTutorial (notificationMsg, notificationArgument, notificationOrigin, notificationPoint, notificationPriority);
				break;

			case TYPE_RECURSION:
				DoRecursion (notificationEvent, notificationArgument, notificationOrigin, notificationPoint, notificationPriority);
				break;

			// default
			default:
				Debug.LogError ("[NOTIFICATION CENTER ATTENTION] EVENT TYPE NOT FOUND (" + notificationEvent.ToLower () + ")");
				break;


			}

			// priority 
			if ((!storeOnlyTopNotification) || (notificationPriority == NotificationPriority.Top)) {

				Notification n = new Notification ();
				n.timestamp = Time.time;
				n.point = notificationPoint;
				n.key = notificationEvent;
				n.argument = notificationArgument;
				arrNotifications.Add (n);

			}

		}

        private void DoRecursion(string notificationEvent, string notificationArgument, string notificationOrigin, Vector3 notificationPoint, NotificationPriority notificationPriority)
        {
            for (int i = 0; i < listenerList.Count; i++)
            {
                listenerList[i].NotifyTrigger(notificationEvent);
            }
            //			float value;

            //print(notificationEvent);
            switch (notificationEvent.ToLower())
            {

                // TOP

                case "[message]":
                    //AddNotification ("message.gamestarted", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    //AddNotification ("sound2.default", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    //UiShowCommentator(notificationArgument,NotificationSprite.Tutorial_hitting);
                    UiShowHint(notificationArgument);
                    break;

                case "[quest]":
                    UIShowQuest(notificationArgument);
                    break;

                case "[activate.object]":
                    activateObjects(notificationArgument);
                    break;

                case "[deactivate.object]":
                    deactivateObjects(notificationArgument);
                    break;


                case "[spoken]":
                    // // // // ShowCombatText("" + notificationArgument, notificationPoint, CombatText.TextType.Spoken);
                    break;


                case "[level.reload]":
                    CommentatorUpdate("level.reload", "");
                    break;
                case "[level.load]":
                    CommentatorUpdate("level.load", notificationArgument);
                    break;

                case "[load.level]":
                    //Application.LoadLevel(notificationArgument);
                    // SceneStateManager.instance.changeStateAutonomously(notificationArgument);
                    CommentatorUpdate("level.load", notificationArgument);
                    break;
                // death
                case "[npcenemy.death]":
                    AddNotification(TYPE_EFFECT + ".explosion", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    AddNotification("sound.enemy.death", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    //GUIShowMinorNotification ( "One of your enemies died you did well!");
                    deadEnemies += 1;
                    // add money for every killed person
                    // to player1 or player2 ? 
                    //Debug.Log("notificationOrigin "+notificationOrigin);



                    CommentatorUpdate("death", notificationOrigin);
                    break;

                case "[player1.death]":
                    AddNotification(TYPE_EFFECT + ".explosion.dark", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    AddNotification("sound.player.death", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    UiShowCommentator("You died, that sucks!");
                    CommentatorUpdate("player.death", notificationOrigin);
                    break;

                case "[player2.death]":
                    AddNotification(TYPE_EFFECT + ".explosion.dark", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    AddNotification("sound.player.death", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    UiShowCommentator("You died, that sucks!");
                    CommentatorUpdate("player.death", notificationOrigin);
                    break;

                case "[npc.death]":
                    AddNotification(TYPE_EFFECT + ".explosion.friend", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    AddNotification("sound.friend.death", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    deadFriends += 1;
                    CommentatorUpdate("death", notificationOrigin);
                    break;

                // body hits
                case "[body.hit]":
                    //AddNotification(TYPE_EFFECT + ".blood", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    AddNotification("sound.body.hit", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    lastHit = notificationArgument.ToLower();
                    CommentatorUpdate("hit", notificationOrigin);
                    //GUIShowMinorNotification ("The enemy hit you... Again");
                    break;

                case "[weapon.switch]":
                    AddNotification("sound.weapon.switch", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    //GUIShowMinorNotification ("Oh! You switched Weapons!");
                    if (notificationOrigin == "player1" || notificationOrigin == "player2")
                    {
                        CommentatorUpdate("weapon.switch", notificationOrigin);
                    }
                    break;

                // level done / exit
                case "[level.done]":
                    AddNotification(TYPE_EFFECT + ".explosion", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    AddNotification("sound.enemy.death", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    UiShowCommentator("Youd did it! Perfect! Loading Next Level.");
                    break;

                // destroyable
                // body hits
                case "[item.hit]":
                    //AddNotification("sound.hit.chest", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    AddNotification(TYPE_EFFECT + ".object.hit", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    break;

                case "[damage.taken]":
                    CommentatorUpdate("damage.taken", notificationOrigin);
                    break;

                case "[block.hit]":
                    //AddNotification("sound.hit.block", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    break;

                case "[block.start]":
                    if (notificationOrigin == "player1" || notificationOrigin == "player2")
                    {
                        timeTable["last.block." + notificationOrigin] = Time.time;
                    }
                    break;

                case "[block.success]":
                    CommentatorUpdate("block.success", notificationOrigin);
                    break;

                case "[fastattack.dagger]":
                    AddNotification("sound.fastattack.dagger", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    break;

                case "[slowattack.sword]":
                    AddNotification("sound.slowattack.sword", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    break;

                case "[slowattack.cudgel]":
                    AddNotification("sound.slowattack.sword", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    break;

                case "[fastattack.halberd]":
                    AddNotification("sound.fastattack.halberdfront", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    break;

                case "[slowattack.halberd]":
                    AddNotification("sound.slowattack.halberd", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    break;

                case "[activate.pressed]":
                    if (notificationOrigin == "player1")
                    {
                        Player1Activate();
                    } else if (notificationOrigin == "player2")
                    {
                        Player2Activate();

                    }
                    break;

                case "[shake]":
                    // CameraUsed.Shake(2, 0.25f, 2);
                    break;

                // Pickup
                case "[item.pickup]":
                    AddNotification(TYPE_EFFECT + ".item.pickup", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    AddNotification("sound.pickup.money", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    break;

                //
                // check people around

                case "[people.around]":
                    //print (notificationOrigin + " is with people");

                    if (notificationOrigin == "player1")
                    {
                        timeTable["being.alone.player1"] = float.MaxValue;
                    }
                    else if (notificationOrigin == "player2")
                    {
                        timeTable["being.alone.player2"] = float.MaxValue;
                    }


                    break;

                case "[being.alone]":
                    //print (notificationOrigin + " is alone");

                    if (notificationOrigin == "player1")
                    {
                        timeTable["being.alone.player1"] = Time.time;
                    }
                    else if (notificationOrigin == "player2")
                    {
                        timeTable["being.alone.player2"] = Time.time;
                    }

                    break;
                case "[edge.changed]":
                    if (notificationOrigin == "player1")
                    {
                        CommentatorUpdate("edge.player1.changed", notificationArgument.ToLower());
                    }
                    else if (notificationOrigin == "player2")
                    {
                        CommentatorUpdate("edge.player2.changed", notificationArgument.ToLower());
                    }
                    break;


                case "[changed.spirit]":
                    if (notificationOrigin == "player1")
                    {
                        CommentatorUpdate("edge.player1.changed", notificationArgument.ToLower());
                    }
                    else if (notificationOrigin == "player2")
                    {
                        CommentatorUpdate("edge.player2.changed", notificationArgument.ToLower());
                    }
                    //print ("Spirit changed");
                    break;

                case "[spirit.high]":
                    CommentatorUpdate("spirit.high", "");
                    break;
                case "[spirit.low]":
                    CommentatorUpdate("spirit.low", "");
                    break;

                case "[changed.health]":
                    bool parsed = false;
                    if (notificationOrigin.ToLower().IndexOf("enemy") != -1)
                    {
                        // // // ShowCombatText("" + notificationArgument, notificationPoint, CombatText.TextType.EnemyDamage);
                        parsed = true;
                    }
                    if (notificationOrigin.ToLower().IndexOf("player") != -1)
                    {
                        // // // ShowCombatText("" + notificationArgument, notificationPoint, CombatText.TextType.Damage);
                        parsed = true;
                    }
                    if (!parsed)
                    {
                        // // // ShowCombatText("" + notificationArgument, notificationPoint, CombatText.TextType.NPCDamage);
                        parsed = true;
                    }
                    break;

                case "[gained.health]":
                    // // // ShowCombatText(notificationArgument, notificationPoint, CombatText.TextType.Heal);
                    CommentatorUpdate("gained.health", notificationOrigin);
                    AddNotification("sound.pickup.food", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    break;

                case "[changed.money]":
                    // // // ShowCombatText(notificationArgument, notificationPoint, CombatText.TextType.Money);
                    CommentatorUpdate("gained.money", notificationOrigin);
                    AddNotification("sound.pickup.money", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                    break;

                case "[changed.faith]":
                    try
                    {
                        if (float.Parse(notificationArgument) > 0.0f)
                        {
                            // // // ShowCombatText("+" + notificationArgument, notificationPoint, CombatText.TextType.Faith);
                            AddNotification("sound.pickup.relic", notificationPoint, NotificationPriority.ThrowAwayAfterProcessing);
                            CommentatorUpdate("gained.faith", notificationOrigin);

                        }
                        else {
                            // // // ShowCombatText("" + notificationArgument, notificationPoint, CombatText.TextType.Faith);
                        }
                    }
                    catch
                    {

                    }
                    break;

                case "[faith.full]":
                    UiShowCommentator("Press X A B Y to activate godly rage");
                    CommentatorUpdate("faith.full", "");
                    break;

                case "[moving]":
                    CommentatorUpdate("moving", "");
                    break;

                // default
                default:
                    Debug.LogError("[NOTIFICATION CENTER ATTENTION] RECURSION EVENT NOT FOUND (" + notificationEvent.ToLower() + ")");
                    break;

            }
        }

		private void DoEffect (string notificationEvent, string notificationArgument, string notificationOrigin, Vector3 notificationPoint, NotificationPriority notificationPriority)
		{
      
			switch (notificationEvent.ToLower ()) {
			// ThrowAway
			
			// explosions
			case "explosion":
				Instantiate (nfExplosion, notificationPoint, Quaternion.identity);
				break;
			case "explosion.dark":
				Instantiate (nfExplosionDark, notificationPoint, Quaternion.identity);
				break;
			
			case "explosion.friend":
				Instantiate (nfExplosionFriend, notificationPoint, Quaternion.identity);
				break;

			case "item.pickup":
				Instantiate (nfItemPickup, notificationPoint, Quaternion.identity);
				break;

			case "object.destroy":
				Instantiate (nfObjectDestroy, notificationPoint, Quaternion.identity);
				break;
			case "object.hit":
				Instantiate (nfObjectHit, notificationPoint, Quaternion.identity);
				break;

			case "trigger.big":
				Instantiate (nfTriggerBig, notificationPoint, Quaternion.identity);
				break;

			case "trigger.medium":
				Instantiate (nfTriggerMedium, notificationPoint, Quaternion.identity);
				break;

			case "trigger.small":
				Instantiate (nfTriggerSmall, notificationPoint, Quaternion.identity);
				break;
				
			case "blood":
				// Instantiate blood effect
				bool parsed = false;
				if (notificationOrigin.ToLower().IndexOf("enemy")!=-1) {
					Instantiate (nfParticleBlood, notificationPoint, Quaternion.identity);
					parsed = true;
				}
				if (notificationOrigin.ToLower().IndexOf("player")!=-1) {
					Instantiate (nfParticleBlood, notificationPoint, Quaternion.identity);
					parsed = true;
				} 
				if (!parsed) {
					Instantiate (nfParticleBlood, notificationPoint, Quaternion.identity);
					parsed = true;
				}
				break;
			
			// default
			default:
				Debug.LogError ("[NOTIFICATION CENTER ATTENTION] EFFECT EVENT NOT FOUND (" + notificationEvent.ToLower () + ")");
				break;
			
			}
		}

		void DoTutorial (string notificationMsg, string notificationArgument, string notificationOrigin, Vector3 notificationPoint, NotificationPriority notificationPriority)
		{
			CommentatorUpdate ("tutorial", "");
			switch (notificationMsg){

			case "1":
				MasterAudio.PlaySound("tutorial.1");
				break;
			case "2":
				MasterAudio.PlaySound("tutorial.2");
				UiShowHint("Press B to attack with your sword!                                                                      ");
				break;
			case "3":
				MasterAudio.PlaySound("tutorial.3");
				break;
			case "4":
				MasterAudio.PlaySound("tutorial.4");
				UiShowHint("Press A to attack with your dagger!                                                                     ");
				break;
			case "5":
				MasterAudio.PlaySound("tutorial.5");
				break;
			case "6":
				MasterAudio.PlaySound("tutorial.6");
				UiShowHint("Press Y to change your weapon                                                                          ");
				break;
			case "7":
				MasterAudio.PlaySound("tutorial.7");
				UiShowHint("Press A or B to attack with your Halberd!                                                              ");
				break;
			case "8":
				MasterAudio.PlaySound("tutorial.8");
				UiShowHint("Press X to make a quick backstep!                                                                       ");
				break;
			case "9":
				MasterAudio.PlaySound("tutorial.9");
				break;

			default:
				break;

			}
		}

		private void Play3DSound (string soundname, Vector3 position)
		{
			//Debug.Log("play 3D " + soundname);
			MasterAudio.PlaySound3DAtVector3AndForget (soundname, position);
		}

		private void Play2DSound(string soundname){
			//if (Time.time - timeTable ["cooldown.comment"] > 5.0f) {
				//MasterAudio.PlaySound ("commentator." + soundname);
				//timeTable["cooldown.comment"]=Time.time;
			//}
		}

		void Playsound (string soundname, Vector3 notificationPoint)
		{
			/*
			GameObject temp;
			BasicSoundScript sound;
			temp = Instantiate (hitSound, notificationPoint, Quaternion.identity) as GameObject;
			sound = temp.gameObject.GetComponent<BasicSoundScript> ();
			sound.SetSound (soundname);
			sound.Play ();
      */
		}

/*
		void // // // ShowCombatText(string value, Vector3 position, CombatText.TextType textType){
			Vector3 screenPos = Camera.main.WorldToScreenPoint (position + new Vector3 (FloatingCombatText.GetComponent<CombatText>().OffsetX,
			                                                                            FloatingCombatText.GetComponent<CombatText>().OffsetY,
			                                                                            FloatingCombatText.GetComponent<CombatText>().OffsetZ));
			//screenPos = Camera.main.WorldToScreenPoint(position);
			screenPos.x = screenPos.x / Screen.width;
			screenPos.y = screenPos.y / Screen.height;

     

			//screenPos.x =screenPos.x/ Screen.width +0.5f;
			//screenPos.y =screenPos.y/ Screen.height+0.5f;
			GameObject temp;
			CombatText combatText;
			temp = Instantiate (FloatingCombatText, screenPos, Quaternion.identity) as GameObject;
			combatText = temp.gameObject.GetComponent<CombatText> ();
			combatText.ShowText (textType,value);
		}

*/
        void activateObjects(string name){
			GameObject environment;
			environment = GameObject.Find ("Environment");
			int count = environment.transform.childCount;
			for (int i=0; i<count; i++) {
				if(environment.transform.GetChild(i).gameObject.name.ToLower()==name){
					environment.transform.GetChild(i).gameObject.SetActive(true);
				}
				int count2=environment.transform.GetChild(i).transform.childCount;
				for(int j=0; j<count2; j++){
					if(environment.transform.GetChild(i).transform.GetChild(j).gameObject.name.ToLower()==name){
						environment.transform.GetChild(i).transform.GetChild(j).gameObject.SetActive(true);
					}
				}
				
			}
		}

        void deactivateObjects(string name)
        {
            GameObject environment;
            environment = GameObject.Find("Environment");
            int count = environment.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                if (environment.transform.GetChild(i).gameObject.name.ToLower() == name)
                {
                    environment.transform.GetChild(i).gameObject.SetActive(false);
                }
                int count2 = environment.transform.GetChild(i).transform.childCount;
                for (int j = 0; j < count2; j++)
                {
                    if (environment.transform.GetChild(i).transform.GetChild(j).gameObject.name.ToLower() == name)
                    {
                        environment.transform.GetChild(i).transform.GetChild(j).gameObject.SetActive(false);
                    }
                }

            }
        }


        private void UiShowCommentator (string description)
		{
     // PlayerHUD.Instance.ShowCommmentator(description);
    }

		private void UiShowHint(string notification){
			// PlayerHUD.Instance.ShowHint (notification);
		}

        private void UIShowQuest(string notification)
        {
			//  PlayerHUD.Instance.ShowQuest(notification);
        }



        /*
             * Commentator 
             * 
             * 
             * */
        void CommentatorUpdate (string message, string origin)
		{
			return; 
			/*
			//			print ("commentatorupdate " + message + " from " + origin + " lasthit was" + lastHit);
			if (gameLogic.statesub == GameLogic.GameLogicStateSub.Fight) {
				float lastEvent=Time.time;
				float currentTime=Time.time;
				switch (message) {

				case "death":

					if (lastHit == "player1" || lastHit == "player2") {
						//int currentKillcount=0;

						if (lastHit == "player1") {

							killCountPlayer1 += 1;
							//currentKillcount=killCountPlayer1;
							perfectKillCountPlayer1 += 1;
							if (perfectKillCountPlayer1 > 1) {
								OnCommentatorKillingFrenzy ("player1", perfectKillCountPlayer1);
							}
						} else {
							killCountPlayer2 += 1;
							//currentKillcount=killCountPlayer2;
							perfectKillCountPlayer2 += 2;
							if (perfectKillCountPlayer2 > 1) {
								OnCommentatorKillingFrenzy ("player2", perfectKillCountPlayer2);
							}
						}
						gameLogic.AddBonusFightingSpirit(gameLogic.fightingSpiritOnKill);

						if (Time.time - timeTable ["last.kill." + lastHit] < 5 + killCountPlayer1) {
							OnCommentatorMultikill (lastHit, killCountPlayer1);
						}
						timeTable ["last.kill." + lastHit] = Time.time;
					}
					if ((deadEnemies + deadFriends) == 1) {
						OnCommentatorFirstBlood (Time.time);
					}

					break;

				case "level.reload":
					playerDeathCount+=1;
					OnCommentatorReloadLevel(playerDeathCount);
					break;

				case "level.load":
					playerDeathCount=0;
					OnCommentatorLoadLevel(origin);
					break;

				case "damage.taken":
					if (origin == "player1" || origin == "player2") {
						OnCharacterDamageTaken (origin);
						timeTable ["damage.taken." + origin] = Time.time;
					}
					break;

				case "nothing.happened":
					if(currentTime - timeTable ["last.event"]>60){
						OnCommentatorNothingHappens(currentTime - timeTable ["last.event"]);
					}
					lastEvent=timeTable["last.event"];

          //player1
					if (currentTime - timeTable ["weapon.same.player1"] > 300) {
						OnCommentatorSameWeapon ("player1", Time.time - timeTable ["weapon.same.player1"]);
					}
					if (currentTime - timeTable ["damage.taken.player1"] > 65) {
						OnCommentatorNoDamageTaken ("player1", Time.time - timeTable ["damage.taken.player1"]);
						timeTable ["damage.taken.player1"]=Time.time;
					}

					if (currentTime - timeTable ["being.alone.player1"] > 30) {
						OnCommentatorBeingAlone ("player1", Time.time - timeTable ["being.alone.player1"]);
					}

					if (currentTime - timeTable ["walking.wall.player1"] > 30) {
						OnCommentatorWalkingWall ("player1", Time.time - timeTable ["walking.wall.player1"]);
					}

          //player2

					if (currentTime - timeTable ["weapon.same.player2"] > 300) {
						//OnCommentatorSameWeapon ("player2", Time.time - timeTable ["weapon.same.player2"]);
					}
					if (currentTime - timeTable ["damage.taken.player2"] > 65) {
						//OnCommentatorNoDamageTaken ("player2", Time.time - timeTable ["damage.taken.player2"]);
						timeTable ["damage.taken.player2"]=Time.time;

					}

					if (currentTime - timeTable ["being.alone.player2"] > 30) {
						//OnCommentatorBeingAlone ("player2", Time.time - timeTable ["being.alone.player2"]);
					}

					if (currentTime - timeTable ["walking.wall.player2"] > 30) {
						//OnCommentatorWalkingWall ("player2", Time.time - timeTable ["walking.wall.player2"]);
					}

					break;

				case "block.success":
					if (origin == "player1" || origin == "player2") {
						if (Time.time - timeTable ["last.block." + origin] < 0.25f) {
							OnCommentatorLastMomentBlock (origin);
						}
					}
					break;

				case "hit":
					if (origin == "player1" || origin == "player2") {
						if (origin == "player1") {
							perfectKillCountPlayer1 = 0;
							timeTable["damage.taken.player1"]=Time.time;
						} else {
							perfectKillCountPlayer2 = 0;
							timeTable["damage.taken.player2"]=Time.time;

						}
					}
					break;

				case "gained.health":
					if(origin=="player1" || origin == "player2"){
						OnCommentatorGainedHealth(origin);
					}
					break;
				case "gained.faith":
					if(origin=="player1" || origin == "player2"){
						OnCommentatorGainedFaith(origin);
					}
					break;
				case "gained.money":
					if(origin=="player1" || origin == "player2"){
						OnCommentatorMoneyGained();
					}
					break;
				case "spirit.high":
					OnCommentatorFightingSpiritHigh();
					break;

				case "spirit.low":
					OnCommentatorLowFightingSpirit();
					break;

				case "faith.full":
					if(origin=="player1" || origin == "player2"){
						OnCommentatorFaithFull();
					}
					break;

				case "moving":
					OnCommenttorBackAgain();
					break;

				default:
					break;

				}
				timeTable["last.event"]=lastEvent;
			}
			*/
		}


		void OnCommentatorFightingSpiritHigh(){
			if (Time.time - timeTable ["cooldown.gained.spirit"] >= 60) {
				Play2DSound ("fightingspirit.gained");
				UiShowCommentator("Your fighting spirit is high!");
				timeTable ["cooldown.gained.spirit"] = Time.time;

			}
		}

		void OnCommentatorFaithFull(){

			if (Time.time - timeTable ["cooldown.faith.full"] >= 60) {

				Play2DSound ("faith.full");
        UiShowCommentator("Your faith is full!");
				timeTable ["cooldown.faith.full"] = Time.time;

			}
		}

		void OnCommentatorMoneyGained(){
			if (Time.time - timeTable ["cooldown.gained.money"] >= 60) {

			Play2DSound ("pickup.money");
        UiShowCommentator("You picked up money!");
				timeTable ["cooldown.gained.money"] = Time.time;
			}
		}

		void OnCommentatorLowMoney(){

			if (Time.time - timeTable ["cooldown.money.low"] >= 60) {

			Play2DSound ("money.low");
        UiShowCommentator("Your money is low!");
				timeTable ["cooldown.money.low"]=Time.time;
		}
		}

		void OnCommentatorLowFightingSpirit(){

			if (Time.time - timeTable ["cooldown.spirit.low"] >= 90) {

				Play2DSound ("low.fightingspirit");
        UiShowCommentator("You have low Fighting spirit!");
				timeTable ["cooldown.spirit.low"]=Time.time;
			}

		}

		// Commentator Events

		// Comment
		void OnCommentatorNoKillingsYet (float timesince)
		{

			Debug.Log ("OnCommentatorNoKillingsYet(" + timesince + ")");

		}

		void OnCommentatorGainedHealth(string player){
			if (Time.time - timeTable ["cooldown.gained.health"] >= 60) {
				System.Random random = new System.Random();
				if(random.NextDouble()>0.5){
					Play2DSound ("gained.health.back");
				} else {
					Play2DSound("food.pickup");
				}
				timeTable ["cooldown.gained.health"]=Time.time;
			}

		}

		void OnCommentatorGainedFaith(string player){

			if (Time.time - timeTable ["cooldown.gained.faith"] >= 60) {
        //System.Random random;
        UiShowCommentator("You got some faith!");
				Play2DSound ("gained.faith");
				timeTable ["cooldown.gained.faith"]=Time.time;
			}
			
		}

		void OnCommentatorFirstBlood (float timesince){
      UiShowCommentator("First one to go! Many more to come!");
			Play2DSound ("first.kill");
			//Debug.Log ("OnCommentatorFirstBlood(" + timesince + ")");


		}

		void OnCommentatorKillingFrenzy (string name, int killCount)
		{
			/*
			gameLogic.AddBonusFightingSpirit (gameLogic.fightingSpiritOnFrenzy);
			//GUIShowMinorNotification (name + " has allready killed " + killCount + "people");
			if (killCount == 5) {
				//GUIShowMinorNotification ("So many down and still no damage taken! Incredible!?");
			}

			*/
		}

		void OnCharacterDamageTaken (string origin)
		{
			if (Time.time - timeTable ["cooldown.damage.taken"] >= 60) {

        UiShowCommentator("You took it good, but you still stands!");
				Play2DSound ("low.life");
			}
		}

		void OnCommentatorSameWeapon (string player, float f)
		{
      UiShowCommentator("There are plenty other weapons… Just saying…");
			Play2DSound ("same.weapon");
			timeTable ["weapon.same.player1"] = Time.time;
			timeTable ["weapon.same.player2"] = Time.time;
		}

		void OnCommentatorNoDamageTaken (string player, float f)
		{
			/*
			float health;
			if (player == "player1") {
				health=player1.Health;
			} else {
				health=player2.Health;

			}
			if (health <= 0.25f) {
        UiShowCommentator("One foot in the grave and still kicking! Unbelievable!");
				Play2DSound("long.low.life");
			} else if (health >= 0.8f) {
        UiShowCommentator("What are you? Invincible? The enemies can't even touch you!");
				Play2DSound("long.full.life");

			}
			timeTable ["damage.taken." + player] = Time.time;
			*/
		}




		void OnCommentatorBeingAlone (string player, float f)
		{
      UiShowCommentator("Did you get lost in the Woods? Or are you just trolling?");
			Play2DSound ("got.lost");

		}

		//TODO implement trigger
		void OnCommentatorWalkingWall (string player, float f)
		{
      UiShowCommentator("Have you fallen asleep at your controller?");
		}

		void OnCommentatorMultikill (string player, int killCount)
		{
			//throw new System.NotImplementedException ();
			if (killCount == 3) {
        //GUIShowMinorNotification ("Double Kill");
        UiShowCommentator("How the hell did you do that?");
				Play2DSound ("three.kills");

			} else if (killCount == 6) {

        UiShowCommentator("What a killing spree!");
				Play2DSound ("six.kills");
			} else if (killCount == 10) {
        UiShowCommentator("10Down!");
				Play2DSound ("x.kills");
				//TODO TODO TODO
			}

		}

		void OnCommentatorLastMomentBlock (string origin)
		{
			Play2DSound ("perfect.bock");
      UiShowCommentator("A second later and it would have been to late!!");
		}

		//TODO Fully implement trigger
		void OnCommentatorReloadLevel (int playerDeathCount)
		{
			Play2DSound ("dead.again");
      UiShowCommentator("It seems to be pretty hard! You died for the "+ playerDeathCount+ ". time already!");
		}

		void OnCommentatorLoadLevel (string levelLoaded)
		{
      UiShowCommentator("A new level, time to get some money!");
		}


		void OnCommentatorNothingHappens (float f)
		{
			if (f > 60) {
				timeTable["damage.taken.player1"]=Time.time;
				timeTable["damage.taken.player2"]=Time.time;
				timeTable["weapon.same.player1"]=Time.time;
				timeTable["weapon.same.player2"]=Time.time;
				//playerIsInActive=true;

				
				if(f>150){
					if(f>300){
						if(f>600){
							if(f<605){
                UiShowCommentator("Ok, I am just going for a dump right now. " +
								"You don't take this serious enough. So why should I. See ya.!");
							}
						}
						if(f<305){
              UiShowCommentator("I take the silence as a yes and read to you some lines of the delicate letters between Ueli and Helen. " +
						                         "If I profane with my unworthiest hand this holy shrine, the gentle fine is this. " +
						                         "my lips, two blushing pilgrims, ready stand to smooth that rough touch with a tender kiss. " +
						                         "Wait a minute he just stole those lines from romeo and juliet! But how is that possible?");
							Play2DSound("not.active.5min");

						}
					}
					if(f<155){
            UiShowCommentator("Alright then, I can tell a story instead, when gaming isn't enough anymore to keep you entertained. " +
					                         "Ueli and Helen are sending a lot of letters back and forth, and it looks like the really adore each other. " +
					                         "It's pretty hilarious and sweet. I actually could get hands on those letters. Do you want to hear some?!");
						Play2DSound("not.active.2min");

					}
				}
				if(f<65){
          UiShowCommentator("You still there? I am getting bored here! Bored out of my mind!");
					Play2DSound("not.active.1min");
				}
			}
		
		}

		void OnCommenttorBackAgain (){
			Play2DSound ("active.again");
		}

		IEnumerator NothingHappens(float waitTime) {
			yield return new WaitForSeconds(waitTime);
			if (isInitialized) {
				CommentatorUpdate ("nothing.happened", "");
				// gameLogic.AddBonusFightingSpirit (-gameLogic.fightingSpiritDecay);
			}
			StartCoroutine (NothingHappens (waitTime));
		}
		
		// OnGUI
		void OnGUI ()
		{

			if (debugOnGUI) {

				string allNotifactions = "Notifications";
				Notification nt;
				for (int i = 0; i < arrNotifications.Count; i++) {
					nt = (Notification)arrNotifications [i];
					allNotifactions = allNotifactions + "\n" + i + ". " + nt.GetDebugString ();
				}
				GUI.Label (new Rect ((int)(Screen.width * 0.5f), 0, (int)(Screen.width * 0.25f), Screen.height), allNotifactions, debugGui);
			}


		}


        public event OnPlayer1Activate onPlayer1Activate;
        public event OnPlayer2Activate onPlayer2Activate;


        public void Player1Activate()
        {
            if (this.onPlayer1Activate != null)
            {
                this.onPlayer1Activate();
            }
        }


        public void Player2Activate()
        {
            if (this.onPlayer2Activate != null)
            {
                this.onPlayer2Activate();
            }
        }


    }



}