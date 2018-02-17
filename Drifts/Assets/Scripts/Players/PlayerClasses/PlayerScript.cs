using UnityEngine;
using System.Collections;
using Rewired;

[System.Serializable]
public class PlayerScript  {

    [SerializeField] protected float power, maxSpeed,currentSpeed, pulledVelocity,acceleration,deceleration;
    protected bool isDead;
    protected int id;
    protected Team currentTeam;
    protected PlayerController owner;
    protected Color chargedColor;

    public float Power { get { return power; } }
    public float MaxSpeed { get { return maxSpeed; } }
	public float CurrentSpeed { get { return currentSpeed; }}
    public float RatioSpeed { get { return currentSpeed/ maxSpeed; } }
	public float Acceleration { get { return acceleration; } }
	public float Deceleration { get { return deceleration; } }
    public int ID { get { return id; } }
    public Team CurrentTeam { get { return currentTeam; } }
    public float PulledVelocity { get { return pulledVelocity; } }
    public PlayerController Owner { get { return owner; } }
    public void onSetPulledVelocity(float vel) { pulledVelocity = vel; }
    public bool IsDead { get { return isDead; } }
    public Color ChargedColor { get { return chargedColor; } }



    public PlayerScript(int _id, Team _currentTeam, PlayerController _owner)
    {
        id = _id;
        currentTeam = _currentTeam;
        power = 1;
        maxSpeed = 130;
        owner = _owner;
        acceleration = 8f;
        deceleration = 8f;
        OnReset();
    }

  

    public void OnReset()
    {
        pulledVelocity = 0;
        currentSpeed = 0;
        isDead = false;
      
    }

    public void OnChangeSpeed(float newSpeed)
    {
        currentSpeed = newSpeed;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
    }

    public void OnMove()
    {
        if (id - 1 >= ReInput.players.AllPlayers.Count)
            return;


		float inputX = ReInput.players.GetPlayer(id-1).GetAxis("Move Horizontal");
		float inputY = ReInput.players.GetPlayer(id-1).GetAxis("Move Vertical");

		
		bool isUsingInput = (inputX > 0.2f || inputY > 0.2f) || (inputX < -0.2f || inputY < -0.2f);
        owner.rBody.velocity = Vector3.zero;

        float newSpeed = isUsingInput ? currentSpeed + acceleration : currentSpeed - deceleration;
        OnChangeSpeed(newSpeed);

        Vector3 offset = new Vector3(inputX * 50 * Time.deltaTime, 0, inputY * 50 * Time.deltaTime);
        owner.transform.position = Vector3.Lerp(owner.transform.position, owner.transform.position + offset, RatioSpeed);
    }

}

public class Sumo : PlayerScript
{

    public Sumo(int _id, Team _currentTeam, PlayerController _owner) : base(_id,_currentTeam,_owner)
    {
        power = 3;
        maxSpeed = 30;
    }

}
