using UnityEngine;
using System.Collections;

public class MonkController : MonoBehaviour {

	[SerializeField]bool UsingController;

	//Player ID - Team

	public enum PlayerTeam {Team1,Team2};
	public PlayerTeam Team;
	public int PlayerID;


	/// 
	Vector3 velocity;

	[SerializeField]float buildingSpeed = 0;


	[SerializeField]float SpeedPlayer = 10;
	BoxCollider PlayerCollider;

	//cursor
	Transform cursorTransform;
	Vector3 startRotation;
	Vector3 endRotation;
	float cursor_t;
	[SerializeField]float cursorSpeed = 10;


	BoxCollider PushCollider;
	BoxCollider PullCollider;



	[SerializeField] bool canDoAction = true;

	void Start()
	{
		BoxCollider[] colliders = transform.GetChild (0).GetChild (0).GetComponents<BoxCollider> ();
		PlayerCollider = GetComponent<BoxCollider> ();
		PushCollider = colliders [0];
		PullCollider = colliders [1];
		cursorTransform = transform.GetChild(0);
	}
	// Update is called once per frame
	void Update ()
	{
		CursorRotation ();
		MoveCharacter ();
		PushButton ();
		PullButton ();
	}

	void CursorRotation()
	{
		
		//lerp
		if (cursor_t < 1) 
		{
			cursor_t += Time.deltaTime * cursorSpeed;
			//transform.GetChild (0).localEulerAngles = new Vector3 (0, 0, Mathf.Lerp (startRotation.z, endRotation.z, cursor_t));
			transform.GetChild (0).localEulerAngles = new Vector3(0,0,Mathf.LerpAngle(startRotation.z, endRotation.z, cursor_t));


		}



		//Control rotation
		float inputX; 
		float inputY;

		if (UsingController) 
		{
			inputX = Input.GetAxis("Horizontal_Rotation_" + PlayerID);
			inputY = Input.GetAxis ("Vertical_Rotation_" + PlayerID);
		}
		else 
		{
			inputX = Input.GetAxis("Horizontal2");
			inputY = Input.GetAxis ("Vertical2");
		}




		if ((inputX > 0.5f || inputY > 0.5f) || (inputX < -0.5f || inputY < -0.5f))
		{


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
	void MoveCharacter()
	{
		float inputX;
		float inputY;


		if (UsingController)
		{
			inputX = Input.GetAxis("Horizontal_Move_" + PlayerID);
			inputY = Input.GetAxis ("Vertical_Move_" + PlayerID);
		}
		else 
		{
			inputX = Input.GetAxis("Horizontal");
			inputY = Input.GetAxis ("Vertical");
		}





		if ((inputX > 0.5f || inputY > 0.5f) || (inputX < -0.5f || inputY < -0.5f)) 
		{
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
				


			Debug.Log (1 / (Mathf.Abs (inputX) + Mathf.Abs (inputY) ));
			float tempSpeed = (1 / (Mathf.Abs (inputX) + Mathf.Abs (inputY) ));







			velocity = new Vector3 
			(
					inputX  *  tempSpeed * SpeedPlayer  * buildingSpeed * Time.deltaTime,
				0,
					inputY  *  tempSpeed * SpeedPlayer  *   buildingSpeed * Time.deltaTime
			);
			
		
			//newPosition.Normalize();
			transform.position += velocity;
				
		} 
		else 
		{
			

		
			



			buildingSpeed -= 0.1f;
			if (buildingSpeed < 0)
				buildingSpeed = 0;

		}
		transform.position += velocity * buildingSpeed;


	}
	void PushButton()
	{
		if (!canDoAction)
			return;


		if(Input.GetButtonDown("L_Press_" + PlayerID))
		{
			//ballManager.onPush (cursorTransform.up, 30);
			Debug.Log ("Push");
			StartCoroutine (TimerActionCooldown ("Push"));
		}
	}
	void PullButton()
	{

		if (!canDoAction)
			return;


		if(Input.GetButtonDown("R_Press_" + PlayerID))
		{
			Debug.Log ("Pull");
		//	ballManager.onPush(cursorTransform.up * -1, -100);

			StartCoroutine (TimerActionCooldown ("Pull"));

		}

	}
	IEnumerator TimerActionCooldown(string action)
	{
		canDoAction = false;

		if (action == "Push")
			PushCollider.enabled = true;
		else if (action == "Pull")
			PullCollider.enabled = true;
		

		yield return new WaitForSeconds (.5f);
		canDoAction = true;

		if (action == "Push")
			PushCollider.enabled = false;
		else if (action == "Pull")
			PullCollider.enabled = false;
	}

}
