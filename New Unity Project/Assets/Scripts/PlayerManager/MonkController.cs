using UnityEngine;
using System.Collections;

public class MonkController : MonoBehaviour
{
	public const string PRESS_L = "L_Press_";
	public const string PRESS_R = "R_Press_";

	[SerializeField]
	bool UsingController;

	//Player ID - Team

	public enum PlayerTeam
	{
		Team1,
		Team2}

	;

	public PlayerTeam Team;
	public int PlayerID;


	/// 
	Vector3 velocity;

	[SerializeField]
	float buildingSpeed = 0;
	[SerializeField]
	float SpeedPlayer = 10;
	[SerializeField]
	float coolDownLength = 10;
	BoxCollider PlayerCollider;
	[SerializeField]
	Animator handAnimator;

	//cursor
	[SerializeField]
	Transform cursorTransform;
	[SerializeField]
	Transform LookAtTransform;
	Vector3 startRotation;
	Vector3 endRotation;
	float cursor_t;
	[SerializeField]
	float cursorSpeed = 10;
	[SerializeField]
	bool isPushActionTriggered = false;
	[SerializeField]
	bool isPullActionTriggered = false;

	[SerializeField]
	BoxCollider PushCollider;
	[SerializeField]
	BoxCollider PullCollider;



	[SerializeField]
	bool canDoAction = true;

	void Start ()
	{
		PlayerCollider = GetComponent<BoxCollider> ();
	}
	// Update is called once per frame
	void Update ()
	{
		CursorRotation ();
		MoveCharacter ();
		PushButton ();
		PullButton ();
	}

	bool isHitValid ()
	{
		return Team != ballManager.PossessedTeam;
	}


	void OnTriggerEnter (Collider other)
	{
        
		if (isHitValid ())
			SpawnManager.onPlayerDeath (PlayerID);
        
	}

	void CursorRotation ()
	{

		//lerp
		if (cursor_t < 1) {
			cursor_t += Time.deltaTime * cursorSpeed;
			//transform.GetChild (0).localEulerAngles = new Vector3 (0, 0, Mathf.Lerp (startRotation.z, endRotation.z, cursor_t));
			transform.GetChild (0).localEulerAngles = new Vector3 (0, 0, Mathf.LerpAngle (startRotation.z, endRotation.z, cursor_t));


		}



		//Control rotation
		float inputX;
		float inputY;

		if (UsingController) {
			inputX = Input.GetAxis ("Horizontal_Rotation_" + PlayerID);
			inputY = Input.GetAxis ("Vertical_Rotation_" + PlayerID);
		} else {
			inputX = Input.GetAxis ("Horizontal2");
			inputY = Input.GetAxis ("Vertical2");
		}




		if ((inputX > 0.5f || inputY > 0.5f) || (inputX < -0.5f || inputY < -0.5f)) {


			//Child(0) == null du cursor
			startRotation = transform.GetChild (0).localEulerAngles;

			endRotation = new Vector3 (0, 0, Mathf.Atan2 (inputX, inputY) * 180 / Mathf.PI);




			/*


			//If rotation is broken

			if (endRotation.z + startRotation.z >= 180)
				endRotation = new Vector3 (0, 0,startRotation.z + 360);
			else if (endRotation.z - startRotation.z <= -180)
				endRotation = new Vector3 (0, 0,startRotation.z - 360);
			
			*/

			cursor_t = 0;
			//	transform.GetChild (0).localEulerAngles = new Vector3 (0, 0, Mathf.Atan2 (inputX, inputY) * 180 / Mathf.PI);
		}
	}

	void MoveCharacter ()
	{
		float inputX;
		float inputY;


		if (UsingController) {
			inputX = Input.GetAxis ("Horizontal_Move_" + PlayerID);
			inputY = Input.GetAxis ("Vertical_Move_" + PlayerID);
		} else {
			inputX = Input.GetAxis ("Horizontal");
			inputY = Input.GetAxis ("Vertical");
		}





		if ((inputX > 0.5f || inputY > 0.5f) || (inputX < -0.5f || inputY < -0.5f)) {
			/*
			Vector3 velocity = new Vector3
				(
					transform.position.x + (inputX * SpeedPlayer * Time.deltaTime),
					transform.position.y,
					transform.position.z + (inputY * SpeedPlayer * Time.deltaTime)
				);
			*/


			buildingSpeed += 0.05f;
			if (buildingSpeed > 2)
				buildingSpeed = 2;



          
			float tempSpeed = (1 / (Mathf.Abs (inputX) + Mathf.Abs (inputY)));







			velocity = new Vector3 (
				inputX * tempSpeed * SpeedPlayer * buildingSpeed * Time.deltaTime,
				0,
				inputY * tempSpeed * SpeedPlayer * buildingSpeed * Time.deltaTime
			);


			//newPosition.Normalize();
			transform.position += velocity;

		} else {







			buildingSpeed -= 0.1f;
			if (buildingSpeed < 0)
				buildingSpeed = 0;

		}
		transform.position += velocity * buildingSpeed;


	}

	void PushButton ()
	{
		if (!canDoAction)
			return;


		if (Input.GetButtonDown (PRESS_R + PlayerID)) {
			handAnimator.Play ("Push");
			StartCoroutine (TimerActionCooldown ("Push"));
		}
	}

	void PullButton ()
	{

		if (!canDoAction)
			return;


        
		if (Input.GetButtonDown (PRESS_L + PlayerID)) {	
			handAnimator.Play ("Pull");
			StartCoroutine (TimerActionCooldown ("Pull"));
		}
       

        

	}

	public void onTriggerPush (bool state)
	{
		isPushActionTriggered = state;
	}

	public void onTriggerPull (bool state)
	{
		isPullActionTriggered = state;
	}

	public void onPush ()
	{
		Debug.Log ("tappe la balle");
		ballManager.onPush (Quaternion.LookRotation (LookAtTransform.position - cursorTransform.transform.position) * -transform.up, 75);
		ballManager.onChangeTeamPossession (Team);
		//UiManager.OnFreezeFrame (0.1f);
       
	}

	public void onPull ()
	{
		ballManager.onPull (Vector3.zero, -ballManager.CurrentVelocity);
		ballManager.onChangeTeamPossession (Team);
	}


	IEnumerator TimerActionCooldown (string action)
	{
		canDoAction = false;

		switch (action) {
		case "Push":
			PushCollider.enabled = true;
			//yield return new WaitForSeconds (0.1f);
			/*if (isPushActionTriggered)
				onPush ();*/
			break;
		case "Pull":
			PullCollider.enabled = true;
			//yield return new WaitForSeconds (0.1f);
			/*if (isPullActionTriggered)
				onPull ();*/
			break;
		}

		yield return new WaitForSeconds (.1f);



		switch (action) {
		case "Push":
			PushCollider.enabled = false;
			break;
		case "Pull":
			PullCollider.enabled = false;
			break;
		}

		yield return new WaitForSeconds (coolDownLength - .1f);

		canDoAction = true;
		//isPushActionTriggered = false;
		//isPullActionTriggered = false;


	}

}
