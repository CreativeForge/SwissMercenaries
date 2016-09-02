using UnityEngine;
using System.Collections;

public class KnightOnHorsePathFollower : GameElementBasedPathFollower {

	public  virtual void OnReachWayPoint( GameElement el ) {
		AddNotificationHere( "visual", "woodexplosion", 0.0f, "", new Vector3());
	}

}
