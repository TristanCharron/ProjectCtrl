using UnityEngine;
using System.Collections;

public class Player  {

    protected float power, maxSpeed,currentSpeed, pulledVelocity,acceleration,deceleration;
    protected bool isDead;
    protected int id;
    protected Team currentTeam;
    protected PlayerController owner;
    protected Color chargedColor;

    public float Power { get { return power; } }
    public float MaxSpeed { get { return maxSpeed; } }
    public float CurrentSpeed { get { return currentSpeed; } }
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



    public Player(int _id, Team _currentTeam, PlayerController _owner)
    {
        id = _id;
        currentTeam = _currentTeam;
        power = 1;
        maxSpeed = 50;
        owner = _owner;
        acceleration = 1f;
        deceleration = 3f;
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
        float inputX = Input.GetAxis("Horizontal_Move_" + id);
        float inputY = Input.GetAxis("Vertical_Move_" + id);
        bool isUsingInput = (inputX > 0.5f || inputY > 0.5f) || (inputX < -0.5f || inputY < -0.5f);
        owner.rBody.velocity = Vector3.zero;
        float newSpeed = isUsingInput ? currentSpeed + acceleration : currentSpeed - deceleration;
        OnChangeSpeed(newSpeed);
        Vector3 offset = new Vector3(inputX * RatioSpeed * Time.deltaTime, 0, inputY * RatioSpeed * Time.deltaTime);
        owner.transform.position = Vector3.Lerp(owner.transform.position, owner.transform.position + offset, RatioSpeed);
    }

}

public class Sumo : Player
{

    public Sumo(int _id, Team _currentTeam, PlayerController _owner) : base(_id,_currentTeam,_owner)
    {
        power = 3;
        maxSpeed = 30;
    }

}
