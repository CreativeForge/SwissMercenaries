using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameElementBasedPathFollower : GameElementBased {

	// state: at the moment - every object is responsible for its own showing etc
	// todo: perhaps better to check all this in levelEditor (simple manager)

	bool started = false;
	int indexWaypoint = -1;

	bool activeFollowing = true;

	// float minDistanceForReachingWaypoint = 0.2f;
	public virtual float MinDistanceForReachingWayPoint() {
		return 0.2f;
	}

	public virtual float FollowSpeed() {
		return 0.1f;
	}

	public virtual bool SmoothWithNextWayPoint() {
		return false;
	}

	// float timer = 0.0f;
	void Start () {
		// timer = Time.time;
	}

	// on stay in a way point / 
	public  virtual bool OnStayWayPointName( string waypointname, bool parsed ) {

		return parsed;
	}
	// on reach a waypoint
	public  virtual void OnReachWayPoint( GameElement el ) {
		
	}

	// Update is called once per frame
	void FixedUpdate () {

		if (!activeFollowing) return;

		if (gameLogic!=null)
		if (CheckIngame())
		if (gameElement!=null) {
			if (!started) {
				started = true;
				indexWaypoint = 0;
			} else {
				// walking to?
				string[] arrWalkpoints = gameElement.argument.Split(',');
				if (arrWalkpoints.Length>0) {

					// ...
					if (indexWaypoint>(arrWalkpoints.Length-1)) {
						indexWaypoint = 0; // arrWalkpoints.Length-1;
					}
					string waypointName = arrWalkpoints[indexWaypoint];
					bool parsed = false; 
					// search for ...
					bool foundWaypoint = false;
					// loop?
					if (waypointName.Equals("break")) {
						// start again
						activeFollowing = false;
						parsed = true;
					}
					if (waypointName.Equals("loop")) {
						// start again
						activeFollowing = false;
						parsed = true;
					}
					// something to do here?
					parsed = OnStayWayPointName( waypointName, parsed );
					if (!parsed)
					if (!waypointName.Equals("")) { 
						// find it
						GameElement ex = gameLogic.levelEditor.GetGameElementByName(waypointName);
						if (ex!=null) {
							// take gameelement or visual representation (gameObject)
							Vector3 target = new Vector3( ex.position.x, ex.position.y, ex.position.z );
							if (ex.gameObject!=null) {
								target = new Vector3( ex.gameObject.transform.position.x, ex.gameObject.transform.position.y, ex.gameObject.transform.position.z );
							}
							if (SmoothWithNextWayPoint()) {
								// let's get all the rest ..
								if ((indexWaypoint+1)<(arrWalkpoints.Length-1)) {
									string waypointNameNext = arrWalkpoints[indexWaypoint+1];
									if (!waypointNameNext.Equals("")) {
										GameElement exnext = gameLogic.levelEditor.GetGameElementByName(waypointNameNext);
										if (exnext!=null) {
											Vector3 targetNext = new Vector3( exnext.position.x, exnext.position.y, exnext.position.z );
											if (exnext.gameObject!=null) {
												targetNext = new Vector3( exnext.gameObject.transform.position.x, exnext.gameObject.transform.position.y, exnext.gameObject.transform.position.z );
											}
											Vector3 diffV = targetNext - target;
											target = target + diffV * 0.10f;
										}
									}
								}
							}
							// found way point
							foundWaypoint = true; 
							float dist = Vector3.Distance(target, transform.position);
							// dist < ..
							if (dist<MinDistanceForReachingWayPoint()) {
								// take next one!!
								indexWaypoint++;
								// last waypoint?
								OnReachWayPoint( ex );

							} else {

								Vector3 relativePos = target - transform.position;
								Quaternion rotation = Quaternion.LookRotation(relativePos);

								// rotating now ... 
								float alpha = rotation.eulerAngles.y;
								alpha = alpha - transform.localRotation.eulerAngles.y;

								bool rotateRight = true;
								if (alpha>0.0f) {
									rotateRight = false;
								}
								if (alpha<0.0f) {
									// if (alpha<-180.0f) 
									rotateRight = true;
								}
								if ((alpha<-180.0f)&&(alpha<-360.0f)) {
									rotateRight = false;
								}

								float rotationSpeed = 3.0f;
								if (!rotateRight) transform.Rotate(0.0f,  rotationSpeed,0.0f, Space.World);
								if (rotateRight) transform.Rotate(0.0f,  -rotationSpeed,0.0f, Space.World);

								// going there ...
								if (dist>0.0f) {
									Vector3 direction = FollowSpeed() * (1.0f/dist*relativePos);
									transform.Translate( direction, Space.World );
								}

							}
						}
					} 
					// no correct waypoint
					if (!foundWaypoint) {
						// take next one!!
						indexWaypoint++;
					}

				}

			}
		}
	}


	public override void OnGameStart() {
		// Debug.Log("CamCutCaption.OnGameStart()");
		// GetArrTimeEvents();
	}
}
