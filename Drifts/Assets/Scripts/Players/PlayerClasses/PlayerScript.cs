using UnityEngine;
using System.Collections;
using Rewired;

[System.Serializable]
public class PlayerScript  {

	[SerializeField] protected float power, pulledVelocity, acceleration;
    protected bool isDead;
    protected int id;
    protected Team currentTeam;
    protected PlayerController owner;
    protected Color chargedColor;

	public float Mass { get { return owner.rBody.mass; } }
	public float Acceleration { get { return acceleration; } }
	public float Drag { get { return owner.rBody.drag; } }

	public float Power { get { return power; } }
	public float PulledVelocity { get { return pulledVelocity; } }

    public int ID { get { return id; } }
    public Team CurrentTeam { get { return currentTeam; } }
    public PlayerController Owner { get { return owner; } }
    public void onSetPulledVelocity(float vel) { pulledVelocity = vel; }
    public bool IsDead { get { return isDead; } }
    public Color ChargedColor { get { return chargedColor; } }

    public PlayerScript(int _id, Team _currentTeam, PlayerController _owner)
    {
        id = _id;
        currentTeam = _currentTeam;
        owner = _owner;

		power = 1;
        acceleration = 50f;
		owner.rBody.mass = 10;
		owner.rBody.drag = 5f;

        OnReset();
    }

  

    public void OnReset()
    {
        pulledVelocity = 0;
        isDead = false;
    }

    public void OnMove()
    {
		int rId = id-1;

		if (rId >= ReInput.players.AllPlayers.Count)
            return;
	
		Vector3 velocity = new Vector3(
			ReInput.players.GetPlayer(rId).GetAxis("Move Horizontal"),
			0,
			ReInput.players.GetPlayer(rId).GetAxis("Move Vertical")
		);
	
		velocity.Normalize();
		owner.rBody.AddForce(velocity * acceleration * 100, ForceMode.Force);
    }

}

public class Sumo : PlayerScript
{

    public Sumo(int _id, Team _currentTeam, PlayerController _owner) : base(_id,_currentTeam,_owner)
    {
        power = 3;

		acceleration = 25f;
		owner.rBody.mass = 20;
		owner.rBody.drag = 5f;
    }

}
