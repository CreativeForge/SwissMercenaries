﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// using DarkTonic.MasterAudio;
// using GameLab.HackAndSlashFramework;




namespace GameLab.NotficationCenter
{
	/*
	 *   everything that is effect an effect and not an object (vs. LevelElement/GameElement)
	 * 
	 * */
    public class NotificationCenter : MonoBehaviour
	{
		// GameLogic
		// game logic
		GameLogic gameLogic;
		GameLogic GetGameLogic( ) {

			if (gameLogic == null) {

				GameObject gl = GameObject.Find ("_GameLogic");
				gameLogic = gl.GetComponent<GameLogic>();

			}

			return gameLogic;
		}

		// notifcation center prototype (ingame notification center)
		/*
		 * NotificationCenterPrototype notificationIngame = null;
		public void 
		NotificationCenterPrototype GetNotificationCenterIngame() {
			
		}
		*/

		// types
		ArrayList arrNotifcationTypes = new ArrayList();

		public NotificationType[] Sound = { };
		public NotificationType[] Visual= { };
		public NotificationType[] Messages= { };
		public NotificationType[] ObjectManipulation = { };
		public NotificationType[] PlayerTypes = { };
		public NotificationType[] Level = { };
		public NotificationType[] InGame = { };
		public NotificationType[] SoundGUI = { };
		public NotificationType[] VisualGUI = { };

		// pipeline .. 
		public ArrayList arrNotificationPipline = new ArrayList();


		void Start() {

			RegisterNotificationTypes( "sound", Sound );
			RegisterNotificationTypes( "visual", Visual );
			RegisterNotificationTypes( "message", Messages );
			RegisterNotificationTypes( "object", ObjectManipulation );
			RegisterNotificationTypes( "player", PlayerTypes );
			RegisterNotificationTypes( "level", Level );
			RegisterNotificationTypes( "ingame", InGame );
			RegisterNotificationTypes( "soundgui", SoundGUI );
			RegisterNotificationTypes( "visualgui", VisualGUI );

			// get gamelogic
			gameLogic = GetGameLogic();

		}

		// manage the notifications
		void Update() {

			// process the pipline and do it!!!
			ProcessNotificationPipeline();

		}

		// AddNotification("x.y", 
		public void AddNotification( string typesubtype, string targetName, float timed, string argument ) {
			string[] arr = typesubtype.Split('.');
			if (arr.Length>1) {
				AddNotification( arr[0],arr[1],  targetName,  timed,  argument, new Vector3() );
			}

			AddNotification( arr[0], "???",  targetName,  timed,  argument, new Vector3() );
		}

		public void AddNotification( string type, string subtype, string targetName, float timed, string argument, Vector3 pos  ) {
			// 1. get notification
			Notification nt = GetNotificationType( type, subtype );

			if (nt!=null) {
				// 2. identify ..
				nt.type = type;
				nt.subtype = subtype;
				if (!targetName.Equals("target")) nt.targetName = targetName;
				nt.timed = timed;
				if (!argument.Equals("argument")) nt.argument = argument;
				nt.timeStop = Time.time + timed;
				nt.targetPoint = pos;
				// 3. Add To Pipeline
				arrNotificationPipline.Add(nt);
			} else {
				Notification nx = new Notification();
				nx.state = "error";
				nx.type = type;
				nx.subtype = subtype;
				nx.argument = "type not found";
				arrNotificationPipline.Add(nx);
			}

		}

		// ProcessNotificationPipeline
		void ProcessNotificationPipeline() {
			Notification nt;
			for (int i=0;i<arrNotificationPipline.Count;i++) {
				nt = (Notification) arrNotificationPipline[i];
				if (nt.state.Equals("")) {
					if (Time.time>nt.timeStop) {
						ProcessNotification( nt );
					}
				}
			} 

			// done > kill now !!!
			for (int i=0;i<arrNotificationPipline.Count;i++) {
				nt = (Notification) arrNotificationPipline[i];
				if (nt.state.Equals("done")) {
					arrNotificationPipline.Remove(nt);
					break;
				}
			} 

		}

		// dont' forget to register in unity3d editor ...


		// ProcessNotification
		void ProcessNotification( Notification nt ) {


			bool parsed = false;
			if (nt.type.Equals("visual")) { ProcessVisual( nt ); parsed = true;}
			if (nt.type.Equals("object")) { ProcessObject( nt ); parsed = true; }
			if (nt.type.Equals("message")) { ProcessMessage( nt ); parsed = true; }
			if (nt.type.Equals("player")) { ProcessPlayer( nt ); parsed = true; }
			if (nt.type.Equals("level")) { ProcessLevel( nt ); parsed = true; }
			if (nt.type.Equals("ingame")) { ProcessInGame( nt ); parsed = true; }

			if (!parsed) {
				CreatePrefabsFor( nt );
			}

			nt.state = "done";
		}

		// Process it!
		// create prefabs for notification
		void CreatePrefabsFor( Notification nt ) {
			// direct positions
			if (nt.targetName.Equals("")) { if (nt.targetPoint!=null) CreateInstantiatePrefab(nt, nt.targetPoint); }
			if (nt.targetName.Equals("self")) { if (nt.targetPoint!=null)  CreateInstantiatePrefab(nt, nt.targetPoint); }
			if (nt.targetName.Equals("vector")) { if (nt.targetPoint!=null)  CreateInstantiatePrefab(nt, nt.targetPoint); }

			// go for the names
			ArrayList arr = GetGameElementsByTargetName( nt.targetName );
			for (int a=0;a<arr.Count;a++) {
				GameElement ge = (GameElement) arr[a];
				// Debug.LogFormat("NotificationCenter.ProcessVisual3d() a. // "+ge.name)	;	
				// Do it there 
				if (ge.gameObject!=null) {
					CreateInstantiatePrefab(nt, new Vector3(ge.gameObject.transform.position.x,ge.gameObject.transform.position.y,ge.gameObject.transform.position.z) );
				} else {
					// CreateInstantiatePrefab(nt, new Vector3(ge.gameObject.transform.position.x,ge.gameObject.transform.position.y,ge.gameObject.transform.position.z) );
				}
			}
		}

		// create prefab at ...
		void CreateInstantiatePrefab( Notification nt, Vector3 position ) {
			// check for .. 
			// Debug.Log("NotificationCenter.CreateInstantiatePrefab() // "+nt.type+"/"+nt.subtype );
			if (nt.prefabGameObject!=null) {
				// Debug.Log("NotificationCenter.CreateInstantiatePrefab() // Prefab existing");
				GameObject go=Instantiate(nt.prefabGameObject, position, new Quaternion()) as GameObject;
				go.name = "NotFound.Notification10";
			}

		}

// dont' forget to register in unity3d editor ...

		// Process visual
		void ProcessVisual( Notification nt ) {
			// create prefab
			CreatePrefabsFor( nt );

		}

// dont' forget to register in unity3d editor ...

		// Process visual
		void ProcessPlayer( Notification nt ) {
			// create prefab
			if (nt.subtype.Equals("teleport")) {
				// at ... 
				GameElement ge = GetGameElementByTargetName( nt.argument ); 
				if (ge!=null) {
					GameObject gameObj = ge.gameObject;
					if (gameObj!=null) {
						Debug.Log("NotificationCenter.ProcessPlayer() // teleport ");
						gameLogic.levelEditor.ingameController.TeleportPlayerAndNPCs( new Vector3(gameObj.transform.position.x,gameObj.transform.position.y,gameObj.transform.position.z) );
					}
				}
			}

			if (nt.subtype.Equals("moremoney")) {
				gameLogic.levelEditor.ingameController.playerS.Money += 100;
			}

			if (nt.subtype.Equals("lessmoney")) {
				gameLogic.levelEditor.ingameController.playerS.Money -= 100;
			}

		}

		// Process Object
		void ProcessObject( Notification nt ) {
			// gameLogics
			// print(""+nt.targetName);
			ArrayList arr = GetGameElementsByTargetName( nt.targetName );
			for (int a=0;a<arr.Count;a++) {
				GameElement ge = (GameElement) arr[a];
				// Debug.LogFormat("NotificationCenter.ProcessVisual3d() a. // "+ge.name)	;			

				// object/remove
				if (nt.subtype.Equals("activate")) {
					ge.release = "";
					gameLogic.levelEditor.AddElement(ge);

					// start / ...
				}

				// object/remove
				if (nt.subtype.Equals("remove")) {
					gameLogic.levelEditor.RemoveElement( ge );
				}

				// object/rotate
				if (nt.subtype.Equals("rotate")) {
					if (ge.gameObject!=null) {
						ge.gameObject.transform.Rotate( new Vector3(0.0f,15.0f,0.0f));
					}
				}

			}
		}

		void ProcessInGame( Notification nt ) {
			if (nt.subtype.Equals("plunder")) {
				gameLogic.levelEditor.ingameController.GameMode=1	;	
				// playerS.Money+= 100;
			}
		}

// dont' forget to register in unity3d editor ...

		// Process Message
		void ProcessMessage( Notification nt ) {
			// create prefab
			// ingameNotificationCenter
			// Debug.Log("NotificationCenter.ProcessMessage() // "+nt.type+"/"+nt.subtype+"/"+nt.argument+"/");
			if (gameLogic.levelEditor.ingameNotificationCenter!=null) {
				if (nt.subtype.Equals("show")) {
					gameLogic.levelEditor.ingameNotificationCenter.ShowInGameMessage(""+nt.argument,true,4.0f);
				}
				if (nt.subtype.Equals("notify")) {
					gameLogic.levelEditor.ingameNotificationCenter.ShowInGameMessage(""+nt.argument,false,4.0f);
				}
				if (nt.subtype.Equals("confirm")) {
					gameLogic.levelEditor.ingameNotificationCenter.ShowInGameMessage(""+nt.argument,true,4.0f);
				}
			}
		}

		// Process Object
		void ProcessLevel( Notification nt ) {
			// gameLogics
			// Debug.Log("ProcessLevel()"+nt.argument);
			if (nt.subtype.Equals("next")) {
				// gameLogic.levelEditor.
				gameLogic.LoadNextLevel();
			}
			// take argument
			if (nt.subtype.Equals("load")) {
				// Debug.Log("NotificationCenter.ProcessLevel() // "+nt.argument);
				int levelx = Int32.Parse(nt.argument);
				gameLogic.LoadGameLevel(levelx);
			}
		}

		GameElement GetGameElementByTargetName( string targetName  ) {
			return gameLogic.levelEditor.GetGameElementByName( targetName );
		}

		ArrayList GetGameElementsByTargetName( string targetName  ) {
			ArrayList arr = gameLogic.levelEditor.GetGameElementsByName( targetName );
			return arr;
		}

		// GetPointsFor( nt )
		/*
		 * ArrayList GetPointsFor( Notification nt  ) {
			ArrayList arr = new ArrayList();

			if (!nt.targetName.Equals("")) {
				ArrayList arr = gameLogic.levelEditor.GetGameElementsByName( nt.targetName );
				for (int u=0;u<arr.Count;u++) {
					GameElement ge = (GameElement) arr[u];
					// ... 
					Vector3 pos = new Vector3( ge.position.x, ge.position.y, ge.position.z );
					arr.Add(pos);
				}
			} else {
				
				// arr.Add()	;
			}


			return arr;
		}
		*/

		// register levelelement arrays
		void RegisterNotificationTypes( string prefix, NotificationType[] inotificationTypes ) {
			RegisterNotificationTypes (prefix, inotificationTypes, true);
		}

		// register levelelement arrays
		void RegisterNotificationTypes( string prefix, NotificationType[] inotificationTypes, bool visibleInEditor ) {

					// Debug.Log ("RegisterNotificationType( "+ prefix + " )");

					// 1. sort: first sort for editorIndex
					NotificationType a;
					NotificationType b;
					for (int i=0;i<inotificationTypes.Length;i++) {
						for (int ii=0;ii<inotificationTypes.Length;ii++) {
							a = inotificationTypes[i];
							b = inotificationTypes[ii];
							if (a.editorIndex<b.editorIndex) {
								inotificationTypes[i] = b;
								inotificationTypes[ii] = a;
							}
						}
					}

					// 2. add: by the order of the array
					NotificationType nt;
					for (int i=0;i<inotificationTypes.Length;i++) {
						nt = inotificationTypes[i];

						// add elements
						// if ( nt.gameObject != null ) {
						Notification geType = new Notification ();
						geType.type = prefix; 
						geType.subtype = nt.subtype+""; 
						geType.prefabGameObject = nt.prefabGameObject;
						arrNotifcationTypes.Add(geType);

					}

				}

				// get all types (not subtypes) background etc ..
				public ArrayList GetNotificationTypesUnique(  ) {

					ArrayList arr = new ArrayList ();

					for (int a=0; a<arrNotifcationTypes.Count; a++) {
						Notification nelement = (Notification)arrNotifcationTypes [a];
						bool found=false;
						for (int aa=0; aa<arr.Count; aa++) {
							Notification nelementInArray = (Notification)arr [aa];	
							if (nelement.type.Equals (nelementInArray.type)) {
								found=true;
							}
						}
						if (!found) {
							arr.Add (nelement);
						}
					}


					return arr;

				}

				public ArrayList GetNotificationTypes( string strType ) {

					ArrayList arr = new ArrayList ();

					for (int a=0; a<arrNotifcationTypes.Count; a++) {
						Notification nelement = (Notification)arrNotifcationTypes [a];
						if (nelement.type.Equals ( strType )) {
							arr.Add (nelement);
						}
					}


					return arr;

				}

				public Notification GetNotificationType( string elementArea, string elementSubArea ) {

					for (int a=0; a<arrNotifcationTypes.Count; a++) {
						Notification nelement = (Notification)arrNotifcationTypes [a];
						if (nelement.type.Equals (elementArea)) {
							if (nelement.subtype.Equals (elementSubArea)) {
								Notification gaex = nelement.Copy();
								return gaex;
								// return nelement;
							}
						}
					}

					return null;

				}	

			}


						

}

