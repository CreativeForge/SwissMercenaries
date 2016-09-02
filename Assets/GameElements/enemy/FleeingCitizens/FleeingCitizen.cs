using UnityEngine;
using System.Collections;

public class FleeingCitizen : GameElementBasedPathFollower {

	public override float FollowSpeed() {
		return 0.075f;
	}

	public  override void OnReachWayPoint( GameElement el ) {
		AddNotificationHere( "visual", "woodexplosion", 0.0f, "", new Vector3());
	}

}
