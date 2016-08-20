using UnityEngine;
using System.Collections;

public class MonkController : MonoBehaviour {

	[SerializeField]bool UsingController;

	//Player ID - Team

	public enum PlayerTeam {Team1,Team2};
	public PlayerTeam Team;
	public int PlayerID;


	/// 
	[SerializeField]float SpeedPlayer = 10;
	BoxCollider collider;


	//cursor
	Vector3 startRotation;
	Vector3 endRotation;
	float cursor_t;
	[SerializeField]float cursorSpeed = 10;


	BoxCollider CursorCollider;
	[SerializeField] bool canDoAction = true;

	void Start()
	{
		collider = GetComponent<BoxCollider> ();
		CursorCollider = transform.GetChild (0).GetChild (0).GetComponent<BoxCollider> ();
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




		if ((inputX > 0.01f || inputY > 0.01f) || (inputX < -0.01f || inputY < -0.01f))
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





		transform.position = new Vector3
		(
			transform.position.x + (inputX * SpeedPlayer * Time.deltaTime),
			transform.position.y,
			transform.position.z + (inputY * SpeedPlayer * Time.deltaTime)
		);

	}
	void PushButton()
	{
		if (!canDoAction)
			return;


		if(Input.GetButtonDown("L_Press_" + PlayerID))
		{
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

			StartCoroutine (TimerActionCooldown ("Pull"));

		}

	}
	IEnumerator TimerActionCooldown(string action)
	{
		canDoAction = false;

		if(action == "Pull")
			CursorCollider.enabled = true;
	
		yield return new WaitForSeconds (.5f);
		canDoAction = true;

		if(action == "Pull")
			CursorCollider.enabled = false;

	}

}
