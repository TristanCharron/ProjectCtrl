using UnityEngine;
using System.Collections;

public class ballManager : MonoBehaviour
{

    public static ballManager Instance { get { return instance; } }
    static ballManager instance;

    private static Vector3 velocityAngle; // angle pushing the ball

    public static Vector3 VelocityAngle { get { return velocityAngle; } }

    private static float currentVelocity, destinationVelocity, LerpTimer; //Speed variables

    public static float MaxVelocity { get { return Instance._MaxVelocity; } }

    public static float MinVelocity { get { return Instance._MinVelocity; } }

    public static float CurrentVelocity { get { return Mathf.Clamp(currentVelocity, MinVelocity, MaxVelocity); } } //Clamp & Return speed

    public static float DecreaseVelocity { get { return Instance._DecreaseVelocity; } }

    public float _MaxVelocity, _MinVelocity, _DecreaseVelocity;

    private Rigidbody rBody;

    public Rigidbody RigidBody { get { return rBody; } }

    private static bool isPushed;



    // Use this for initialization
    void Awake()
    {
        onSetComponents();
        onSetProperties();


    }

    private void onSetComponents()
    {
        rBody = GetComponent<Rigidbody>();
        instance = this;
    }

    private void onSetProperties()
    {
        currentVelocity = MinVelocity;
        destinationVelocity = currentVelocity;
        velocityAngle = Vector3.forward;
		rBody.velocity = Vector3.zero;
        rBody.velocity = rBody.velocity * MinVelocity;
        isPushed = false;
        LerpTimer = 0;
    }


    public static void onPush(Vector3 angle, float velocityApplied)
    {
        isPushed = true;
        //velocityAngle = newAngle;
        destinationVelocity = currentVelocity + velocityApplied;
        Instance.RigidBody.velocity = (angle * destinationVelocity);
    }

    public static void onPull(Vector3 angle, float velocityApplied)
    {
        isPushed = true;
        //velocityAngle = newAngle;
        destinationVelocity = currentVelocity + velocityApplied;
        Instance.RigidBody.velocity = (angle * -destinationVelocity);
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        onMove();
    }

    private void onChangeVelocity()
    {
        if(isPushed)
        {
            currentVelocity = Mathf.Lerp(currentVelocity, destinationVelocity, LerpTimer);
            LerpTimer += Time.fixedDeltaTime;
            if (LerpTimer >= 1)
            {
                LerpTimer = 0;
                isPushed = false;
            }
           
        }
        else
        {
            currentVelocity -= DecreaseVelocity;
         
        }

        currentVelocity = Mathf.Clamp(currentVelocity, MinVelocity, MaxVelocity);
        rBody.velocity = rBody.velocity.normalized * currentVelocity;
    }

    private void onMove()
    {
        onChangeVelocity();
    }





}
