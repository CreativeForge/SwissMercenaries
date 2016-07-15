using UnityEngine;
using System.Collections;

[RequireComponent(typeof (AudioSource))]
public class AudioPlayer : MonoBehaviour {
			
	public bool randomizePitch = false;
	public float minRandomPitch = 0.95f;
	public float maxRandomPitch = 1.05f;
	public bool randomizeVolume = false;
	public float minRandomVolume = 0.9f;
	public float maxRandomVolume = 1.0f;
	
	float currentFadeValue = 0; // clamp 0,1
	public float fadeSpeed = 0.01f;
	float fadeVolumeLimit; 
	
	bool isFadingIn;
	public AnimationCurve fadeInCurve;
	public float fadeInEndValue = 1;
	
	bool isFadingOut;
	public AnimationCurve fadeOutCurve;
	public float fadeOutEndValue = 0;
	
	void Awake () {
		SetVolume(GetComponent<AudioSource>().volume);
		SetCurrentFadeValue(GetComponent<AudioSource>().volume);
		
		if(fadeInCurve == null) Debug.LogError("no fade out curve set");//fadeInCurve = AnimationCurve.Linear(0,0,1,1);
		if(fadeOutCurve == null) Debug.LogError("no fade out curve set"); //fadeOutCurve = AnimationCurve.Linear(0,0,1,1);
	}
	
	void RandomizeVolume(){
		GetComponent<AudioSource>().volume = Random.Range(minRandomVolume,maxRandomVolume);
		SetVolume(GetComponent<AudioSource>().volume);
		SetCurrentFadeValue(GetComponent<AudioSource>().volume);
	}
	
	void RandomizePitch(){
		GetComponent<AudioSource>().pitch = Random.Range(minRandomPitch,maxRandomPitch);
	}
	
	public void Play(){
		GetComponent<AudioSource>().mute = false;
		GetComponent<AudioSource>().Play();
		if(randomizePitch) RandomizePitch();
		if(randomizeVolume) RandomizeVolume();
	}
	
	public void Stop(){
		GetComponent<AudioSource>().Stop();
	}
	
	public void Pause(){
		GetComponent<AudioSource>().Pause();
	}
	
	public void SetVolumeToZero(){
		SetVolume(0);
		SetCurrentFadeValue(GetComponent<AudioSource>().volume);
	}
	
	public void Mute(){
		GetComponent<AudioSource>().mute = true;
	}
	
	public float GetCurrentVolume(AnimationCurve inCurve) {
		return inCurve.Evaluate(currentFadeValue);
	}
	public void SetCurrentFadeValue(float inValue){
		currentFadeValue = Mathf.Clamp(inValue, 0f, 1.0f);
	}
	public float GetCurrentFadeValue(){
		return currentFadeValue;
	}
	
	public void StartFadeIn(float inSpeed = 0){
		if(inSpeed == 0)inSpeed = fadeSpeed;
		StartFadeIn(fadeInCurve, inSpeed, fadeInEndValue);
	}
	public void StartFadeOut(float inSpeed = 0){
		if(inSpeed == 0)inSpeed = fadeSpeed;
		StartFadeOut(fadeOutCurve, inSpeed, fadeOutEndValue);
	}
	public void StartFadeIn(AnimationCurve inCurve, float inSpeed, float inEndValue, bool fromZero = true, bool startPlaying = true){
		if(fromZero) SetVolumeToZero();
		if(startPlaying && !GetComponent<AudioSource>().isPlaying) Play ();
		
		fadeInCurve = inCurve;
		fadeSpeed = inSpeed;
		fadeVolumeLimit = inEndValue;
		isFadingOut = false;
		isFadingIn = true;
	}
	
	public void StartFadeOut(AnimationCurve inCurve, float inSpeed, float inEndValue){
		fadeOutCurve = inCurve;
		fadeSpeed = inSpeed;
		fadeVolumeLimit = inEndValue;
		isFadingIn = false;
		isFadingOut = true;
	}
	
	void SetVolume(float inVolume){
		GetComponent<AudioSource>().volume = Mathf.Clamp(inVolume, 0f, 1.0f);
	}
	
	public void EndFadeIn(float inLimit = -1f){
		if(inLimit != -1f) SetVolume(fadeVolumeLimit);
		isFadingIn = false;
	}
	
	public void EndFadeOut(float inLimit){
		SetVolume(fadeVolumeLimit);
		isFadingOut = false;
	}
	
	public string GetStatus(){
		string status = name+") ";
		if(isFadingIn) status += "fade in //";
		else if(isFadingOut) status += "fade out //";
		status += " fadevalue: "+GetCurrentFadeValue();
		status += " volume: "+GetComponent<AudioSource>().volume;
		return status;
	}
	
	public void SetClip(AudioClip inClip){
		GetComponent<AudioSource>().clip = inClip;
	}
	
	void Update () {
		if(isFadingIn && isFadingOut) Debug.LogError("isfadingin and isfadingout at the same time!");
		
		if(isFadingIn){
			if(fadeVolumeLimit>GetComponent<AudioSource>().volume){
				float tValue = GetCurrentFadeValue()+fadeSpeed*Time.deltaTime;
				if(tValue<0 || tValue>1)EndFadeIn();
				SetCurrentFadeValue(tValue);
				SetVolume(GetCurrentVolume(fadeInCurve));
				//Debug.Log(GetStatus());;
			}else if(fadeVolumeLimit<GetComponent<AudioSource>().volume){
				EndFadeIn(fadeVolumeLimit);
			}else if(1 <= GetComponent<AudioSource>().volume){
				EndFadeIn(1);
			}
		}else if(isFadingOut){
			if(fadeVolumeLimit<GetComponent<AudioSource>().volume){
				float tValue = GetCurrentFadeValue()-fadeSpeed*Time.deltaTime;
				if(tValue<0 || tValue>1)EndFadeIn();
				SetCurrentFadeValue(tValue);
				SetVolume(GetCurrentVolume(fadeOutCurve));
				//Debug.Log(GetStatus());
			}else if(fadeVolumeLimit>GetComponent<AudioSource>().volume){
				EndFadeOut(fadeVolumeLimit);
			}else if(0 >= GetComponent<AudioSource>().volume){
				EndFadeOut(0);
			}
		}
		
	}
}
