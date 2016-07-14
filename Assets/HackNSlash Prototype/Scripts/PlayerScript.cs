using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerScript : MonoBehaviour {

	public float speed = 0.4f;
	float originalSpeed;
	public float jumpForce = 200;
	float originalSpeedMod = 1;
	float currentSpeedMod = 1;

	private Transform m_Cam;                  // A reference to the main camera in the scenes transform
	private Vector3 m_CamForward;             // The current forward direction of the camera
	private Vector3 m_Move;						// the world-relative desired move direction, calculated from the camForward and user input.
	private bool m_Jump;                      

	Rigidbody myR;
	float lastJumpTime = 0;
	float lastPushTime =0;
	Vector3 pushForce;
	bool doPush = false;
	bool isRecovering = false;

	public bool isInHolyRoge = false;
	public GameObject holyHaloGO;
	public GameObject holyShineGO;
	float holyRageDuration = 10;
	float holyRageStartTime = 0;

	private Vector3 curNormal  = Vector3.up; // smoothed terrain normal 
	private Quaternion iniRot ; // initial rotation

	public Animator anim;
	public DestructibleScript dS;
	public HitterScript hS;

	bool isGrounded;

	uint money = 0;
	float faith = 0;

	public bool hasPickedUpHalberd = false;
	public GameObject swordGO;
	bool isWithHalberd = false;
	public GameObject halberdGO;

	Vector3 originalPosition;


	void Awake(){
		myR = GetComponent<Rigidbody>();
		dS = GetComponent<DestructibleScript>();
		hS = GetComponent<HitterScript>();
		originalPosition = transform.position;
		originalSpeed = speed;

		halberdGO.SetActive(false);
	}

	public void SetCamT(Transform inT){
		m_Cam = inT;
	}

	void Start () {
		iniRot = transform.rotation;
		// get the transform of the main camera
		if(!m_Cam){
			if (Camera.main != null)
			{
				m_Cam = Camera.main.transform;
			}
			else
			{
				Debug.LogWarning(
					"Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
				// we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
			}
		}

	}
	
	void Update () {
		if(dS.IsDead) return;

		/*
		if (!m_Jump){
			//if(CrossPlatformInputManager.GetButtonDown("Jump") || inputDevice.Action1){
			if(isGrounded && (Input.GetKey(KeyCode.Space)) && lastJumpTime+0.3f < Time.time ){
				m_Jump = true;
			}else{
				m_Jump = false;
			}
		}*/

		if(isRecovering && lastPushTime+1<Time.time){
			isRecovering = false;
			//GameLogicScript.i.girlS.ChangeColor(Color.white);
		}

		HandleFaithShrinking();
		HandleHolyRage();

		if(Input.GetButtonDown("Fire1")){
			IsWithHalberd = !IsWithHalberd;
		}
	}

	public bool IsWithHalberd{

		get{
			return isWithHalberd;
		}
		set{
			if(!HasPickedUpHalberd){
				Debug.LogError("you have to pickup halberd");
				return;
			}
			isWithHalberd = value;
			if(isWithHalberd){
				anim.SetBool("IsWithHalberd", true);
				halberdGO.SetActive(true);
				swordGO.SetActive(false);
			}else{
				anim.SetBool("IsWithHalberd", false);
				halberdGO.SetActive(false);
				swordGO.SetActive(true);
				
			}
		}
	}
		
	void HandleHolyRage(){
		if(holyRageStartTime+holyRageDuration<Time.time){
			EndHolyRage();
		}
	}

	void HandleFaithShrinking(){
		Faith -= Time.deltaTime;
	}

	void HandleIsGrounded(){
		isGrounded = false;
		RaycastHit hit;
		if (Physics.Raycast(transform.position+Vector3.up*0.01f, Vector3.down, out hit, 0.08f)){
			//float distanceToGround = hit.distance;

			//Debug.Log("hit.distance: "+hit.distance);
			//if (distanceToGround < 0.2f){
			isGrounded = true;
			//Debug.Log("isGrounded");
			//}
			Debug.DrawLine(transform.position+Vector3.up, hit.point, Color.red);
		}else{
			Debug.DrawLine(transform.position+Vector3.up, transform.position+Vector3.down*2, Color.green);
		}	
	}

	private void FixedUpdate()
	{
		if(dS.IsDead) return;

		HandleIsGrounded();

		// read inputs

		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		float v = CrossPlatformInputManager.GetAxis("Vertical");
		if(doPush || isRecovering){
			h=0;
			v=0;
		}else{
			myR.velocity = new Vector3(0, myR.velocity.y, 0);
		}
		// calculate move direction to pass to character
		if (m_Cam != null){
			// calculate camera relative direction to move:
			m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
			m_Move = v*m_CamForward + h*m_Cam.right;
			transform.LookAt(transform.position +m_Move);
		}else{
			// we use world-relative directions in the case of no main camera
			m_Move = v*Vector3.forward + h*Vector3.right;
		}

		NormalizeCurrentSpeedMod();

		m_Move = m_Move*speed*currentSpeedMod;
		myR.MovePosition(myR.position+m_Move);

		if(m_Jump){
			myR.AddForce(0,jumpForce,0);
			lastJumpTime = Time.time;
		}
		m_Jump = false;

		if(doPush) {
			myR.MovePosition(myR.position+pushForce.normalized);
			myR.AddForce(pushForce);
			doPush = false;
			isRecovering = true;
		}
			
		if(anim)anim.SetFloat("Velocity",GetVelocity()/8);

		if(!isGrounded) myR.AddForce(Vector3.down*20);
	}


	public void Push(Vector3 inForce){ // can be pushed by an enemy
		if(doPush || isRecovering)return;
		pushForce = inForce;
		doPush = true;
		lastPushTime = Time.time;

	}

	void NormalizeCurrentSpeedMod(){
		if(Mathf.Abs(currentSpeedMod-originalSpeedMod)>0.01){
			currentSpeedMod = Mathf.Lerp(currentSpeedMod, originalSpeedMod, Time.deltaTime);
		}
	}

	public void BoostSpeed(float inBoost){
		currentSpeedMod = inBoost;
	}

	public float GetVelocity(){
		return m_Move.magnitude*20;
	}


	public void HitBoxTriggerEnter(Collider inC){
		Debug.Log("hit "+inC.name+" "+Time.time);
	}


	public void LookInCamDir(){
		Vector3 tDir = new Vector3(m_Cam.position.x, transform.position.y, m_Cam.position.z) - transform.position;
		//Debug.DrawLine(transform.position,transform.position - tDir);
		transform.LookAt(transform.position - tDir);	
	}

	public void SetToOriginalPosition(){
		transform.position = originalPosition;
	}

	public uint Money{
		get {
			return money;
		}

		set{
			float diff = value-money;
			if(diff==0)return;

			if(value<0)
				money = 0;
			else
				money = value;
			InGameController.i.AdjustMoneyVisualisation();
			InGameController.i.notificationC.UpdateMoney(diff,transform.position);
		}
	}

	public float Faith{
		get {
			return faith;
		}

		set{			
			float diff = value-faith;
			if(diff==0)return;

			faith = Mathf.Clamp(value,0,100);
			if(faith>99){
				StartHolyRage();
			}
			InGameController.i.AdjustFaithVisualisation();
			if(diff>Mathf.Abs(1))InGameController.i.notificationC.UpdateFaith(diff,transform.position);
		}
		
	}


	public void StartHolyRage(){
		isInHolyRoge = true;
		holyHaloGO.SetActive(true);
		holyShineGO.SetActive(true);
		holyRageStartTime = Time.time;
		dS.Invincible = true;
		Faith = 10;
		dS.Health = 100;
		speed = originalSpeed*2;
		hS.ModHitForce(2);
		hS.weaponTrailCurrentGO = hS.weaponTrailHolyGO;
		hS.weaponTrailCurrentGO.SetActive(true);
	}


	public void EndHolyRage(){
		isInHolyRoge = false;
		dS.Invincible = false;
		holyHaloGO.SetActive(false);
		holyShineGO.SetActive(false);
		speed = originalSpeed;
		hS.ResetHitForce();
		hS.weaponTrailCurrentGO = hS.weaponTrailNormalGO;
		if(hS.weaponTrailHolyGO)hS.weaponTrailHolyGO.SetActive(false);
	}


	public bool HasPickedUpHalberd{
		get{
			return hasPickedUpHalberd;
		}
		set{
			hasPickedUpHalberd = value;
			IsWithHalberd = true;

		}
	}

	public void Die(){
		EndHolyRage();
		InGameController.i.PlayerDies();
	}
}









