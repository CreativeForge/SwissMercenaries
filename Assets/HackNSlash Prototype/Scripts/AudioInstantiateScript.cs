using UnityEngine;
using System.Collections;

public class AudioInstantiateScript : MonoBehaviour {

			
	public GameObject[] prefabs;
	
	AudioPlayer[] aPList;
	AudioPlayer aP;
	
	public bool playOnAwake = false;

	public bool alwaysRandomize = true;
	
	void Awake () {
		aPList = new AudioPlayer[prefabs.Length];
		int i = 0;
		foreach (GameObject p in prefabs){
			GameObject obj = Instantiate(p, transform.position, Quaternion.identity) as GameObject;
			obj.transform.parent = transform;
			aPList[i] = obj.GetComponent<AudioPlayer>();
			i++;
		}
		Randomize();
		if(playOnAwake) Play();
	}
	
	public void Play() {
		//Debug.Log("play: "+aP.name);
		if(alwaysRandomize) Randomize();
		aP.Play();
	}

	void Randomize(){
		aP = aPList[Random.Range(0,aPList.Length)];
	}
	
	public void StartFadeIn(float inSpeed = 0) {
		aP.StartFadeIn(inSpeed);
	}	
	
	public void StartFadeOut(float inSpeed = 0){
		aP.StartFadeOut(inSpeed);
	}
	
	public void StartFadeIn(AnimationCurve inCurve, float inSpeed, float inEndValue){
		aP.StartFadeIn(inCurve, inSpeed, inEndValue);
	}
	
	public void StartFadeOut(AnimationCurve inCurve, float inSpeed, float inEndValue){
		aP.StartFadeOut(inCurve, inSpeed, inEndValue);
	}
	
	public void SetVolumeToZero(){
		aP.SetVolumeToZero();
	}
	
	public string GetStatus(){
		return aP.GetStatus();
	}

	
}
