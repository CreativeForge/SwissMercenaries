using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
// using GameLab.HackAndSlashFramework;




namespace GameLab.NotficationCenter
{
	/*
	 *   everything that is effect an effect and not an object (vs. LevelElement/GameElement)
	 * 
	 * */
    public class NotificationCenter : MonoBehaviour
	{
		// types
		ArrayList arrNotifcationTypes = new ArrayList();

		public NotificationType[] Sound2D = { };
		public NotificationType[] Visual2D = { };
		public NotificationType[] Sound3D = { };
		public NotificationType[] Visual3D= { };
		public NotificationType[] PlayerTypes = { };

		// pipeline .. 
		ArrayList arrNotificationPipline = new ArrayList();


		void Start() {

			RegisterNotificationTypes( "sound2d", Sound2D );
			RegisterNotificationTypes( "visual2d", Visual2D );
			RegisterNotificationTypes( "sound3d", Sound3D );
			RegisterNotificationTypes( "visual3d", Visual3D );
			RegisterNotificationTypes( "player", PlayerTypes );

		}

		// manage the notifications
		void Update() {


		}

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
						geType.subtype = nt.subtype; 
						arrNotifcationTypes.Add(geType);

					}

				}

			}



}

