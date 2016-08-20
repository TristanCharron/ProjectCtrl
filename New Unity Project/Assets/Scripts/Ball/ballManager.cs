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

    private static bool isPushed;



    // Use this for initialization
    void Awake()
    {
        onSetComponents();
        onSetProperties();
        rBody.AddForce(transform.forward * MinVelocity);

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
        rBody.velocity = new Vector3(Random.Range(0, 2) == 0 ? -1 : 1, 0, Random.Range(0, 2) == 0 ? -1 : 1);
        rBody.velocity = rBody.velocity * MinVelocity;
        isPushed = false;
        LerpTimer = 0;
    }

    public static void onPush(float velocityApplied)
    {
        isPushed = true;
        //velocityAngle = newAngle;
        destinationVelocity = currentVelocity + velocityApplied;
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Mage"))
        {
            onPush(100);
        }
           
    }



}
