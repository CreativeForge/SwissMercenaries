using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameElementBasedTimed : GameElementBased {

	// state: at the moment - every object is responsible for its own showing etc
	// todo: perhaps better to check all this in levelEditor (simple manager)

	bool started = false;

	// gui data ... 
	public GameObject visualGameObject;

	// timed
	public ArrayList arrTimedEvents;
	string timedState = "start"; // start | before | show | after

	// float timer = 0.0f;
	void Start () {
		// timer = Time.time;
	}

	public virtual void OnInitGameElementTimed() {
		/*
		 * scriptText.text = ""+ gameElement.timed +" "+ gameElement.argument+"";
		scriptTextShadow.text = "" + gameElement.argument;
		*/
	}

	public virtual void OnAwakeGameElementTimed() {
		/*
			if (visualGameObject!=null) {
				visualGameObject.SetActive(false);
			}
		*/
	}

	public virtual void OnActivateGameElementTimed() {
		/*
			if (visualGameObject!=null) {
				visualGameObject.SetActive(true);
			}
		*/
	}

	public virtual void OnDeactivateGameElementTimed() {
		/*
				if (visualGameObject!=null) {
					visualGameObject.SetActive(false);
				}
		*/
	}


	// Update is called once per frame
	void FixedUpdate () {

		if (gameElement!=null) {
			if (!started) {
				started = true;
				OnInitGameElementTimed();
			} else {
				/*
				scriptText.text = gameElement.argument+"";
				float alpha = Mathf.Abs(Mathf.Sin(Time.time-timer*10.0f));
				scriptText.color = new Color(1.0f,1.0f,1.0f, alpha); // a = 0.5f; // Mathf.Sin(Time.time-timer);
				*/

				if (CheckIngame()) {
					float ingametime = gameLogic.levelEditor.GetIngameTime();

					if (timedState.Equals("start")) {
						// check if visible
						// deactivate !
						timedState = "before";
						// disable rendering!
						OnAwakeGameElementTimed();

					}

					if (timedState.Equals("before")) {
						// check if visible
						GameElement act = gameLogic.levelEditor.GetGameElementTimedActual( ingametime, arrTimedEvents );
						if (act!=null) {
							if (act==gameElement) {
								// ok show now ..
								// scriptText.text = gameElement.argument;
								timedState = "show";

								OnActivateGameElementTimed();
							}
						}
					}

					if (timedState.Equals("show")) {
						// check if visible
						GameElement act = gameLogic.levelEditor.GetGameElementTimedActual( ingametime, arrTimedEvents );
						if (act!=null) {
							if (act!=gameElement) {
								// ok show now ..
								// scriptText.text = gameElement.argument;
								timedState = "after";
								// deactivate !!! don't destroy otherwise ... there is no gameelement processing - ok we could do it hmmm
								OnDeactivateGameElementTimed();
								// "doppelt gemoppelt"
								gameLogic.levelEditor.DeactivateElement( gameElement );

							}
						}
					}

				}

			}
		}
	}

	public void GetArrTimeEvents() {
		// get all captions now ...
		if (gameElement!=null) {
			arrTimedEvents = gameLogic.levelEditor.GetGameElementsByTypeAndSubTimeSorted(gameElement.type,gameElement.subtype);
			// ArrayList arr = gameLogic.levelEditor.GetAllGameElementsByTypeAndSubTimeSorted();
			// ok debug
			// Debug.Log("CamCutCaption.OnGameStart() // arr: "+arrTimedEvents.Count);
		}
	}

	public override void OnGameStart() {
		// Debug.Log("CamCutCaption.OnGameStart()");
		GetArrTimeEvents();
	}
}
