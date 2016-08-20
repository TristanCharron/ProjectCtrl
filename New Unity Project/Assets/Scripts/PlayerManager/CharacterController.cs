using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {


	//Player ID - Team

	public enum PlayerTeam {Team1,Team2};
	public PlayerTeam Team;
	public int PlayerID;


	/// 
	[SerializeField]float SpeedPlayer = 10;
	BoxCollider collider;

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
		float inputX = Input.GetAxis("Horizontal_Rotation_" + PlayerID);
		float inputY = Input.GetAxis ("Vertical_Rotation_" + PlayerID);


		transform.eulerAngles = new Vector3( 0, Mathf.Atan2(inputY,inputX) * 180 / Mathf.PI, 0 );

	}
	void MoveCharacter()
	{
		float inputX = Input.GetAxis("Horizontal_Move_" + PlayerID);
		float inputY = Input.GetAxis ("Vertical_Move_" + PlayerID);


		transform.position = new Vector3
		(
			transform.position.x + (inputX * SpeedPlayer * Time.deltaTime),
			transform.position.y,
			transform.position.z + (inputY * SpeedPlayer * Time.deltaTime)
		);

	}
	void PushButton()
	{
		if(Input.GetKeyDown("L_Press_" + PlayerID))
		{
			Debug.Log ("Push");
		}
	}
	void PullButton()
	{
		if(Input.GetKeyDown("R_Press_" + PlayerID))
		{
			Debug.Log ("Press");

		}

	}
}
