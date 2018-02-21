using UnityEngine;
using System.Collections;
using Rewired;

[System.Serializable]
public class PlayerScript  {

    public float Power { protected set; get; }

    public float PulledVelocity { protected set; get; }

    public float Acceleration { protected set; get; }

    public bool IsDead { protected set; get; }

    public int ID { protected set; get; }

    public TeamController.TeamID TeamID { protected set; get; }

    public PlayerController Owner { protected set; get; }

    public Color ChargedColor { protected set; get; }

	public float Mass { get { return Owner.rBody.mass; } }

	public float Drag { get { return Owner.rBody.drag; } }


    public void SetPulledVelocity(float vel) { PulledVelocity = vel; }



    public PlayerScript(int _id, TeamController.TeamID _currentTeamid, PlayerController _owner)
    {
        ID = _id;
        TeamID = _currentTeamid;
        Owner = _owner;
        IsDead = false;
        ResetCharacter();
    }

  

    public void ResetCharacter()
    {
        PulledVelocity = 0;
        Power = 1;
        Acceleration = 50f;
    }

    public void Move()
    {
		if (ID >= ReInput.players.AllPlayers.Count)
            return;
	
		Vector3 velocity = new Vector3(
			ReInput.players.GetPlayer(ID).GetAxis("Move Horizontal"),
			0,
			ReInput.players.GetPlayer(ID).GetAxis("Move Vertical")
		);
	
		velocity.Normalize();
		Owner.rBody.AddForce(velocity * Acceleration * 100, ForceMode.Force);
    }

}

public class Sumo : PlayerScript
{

    public Sumo(int _id, TeamController.TeamID _currentTeamid, PlayerController _owner) : base(_id, _currentTeamid, _owner)
    {
        Power = 3;
		Acceleration = 25f;
		Owner.rBody.mass = 20;
		Owner.rBody.drag = 5f;
    }

}
