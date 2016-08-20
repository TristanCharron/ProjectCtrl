using UnityEngine;
using System.Collections;

public class ballManager : MonoBehaviour
{

    public static ballManager Instance { get { return instance; } }
    static ballManager instance;

    private static Vector3 velocityAngle; // angle pushing the ball

    public static Vector3 VelocityAngle { get { return velocityAngle; } }

    private static float currentVelocity; //Speed variables

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

    }

    private void onSetComponents()
    {
        rBody = GetComponent<Rigidbody>();
        instance = this;
    }

    private void onSetProperties()
    {
        currentVelocity = MinVelocity;
        velocityAngle = Vector3.forward;
        isPushed = false;
    }

    public static void onPush(Vector3 newAngle, float velocityApplied)
    {
        velocityAngle = newAngle;
        Instance.StartCoroutine(Instance.onLerpPushedBall(velocityApplied, 0.05f));
    }


    public IEnumerator onLerpPushedBall(float destVelocity, float length)
    {
        if (!isPushed)
        {
            isPushed = true;
            float timer = 0;
            // Lerp velocity
            while (timer <= length)
            {
                currentVelocity = Mathf.Lerp(currentVelocity, destVelocity, timer);
                timer += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            timer = 0;
          
            isPushed = false;
        }
        yield break;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        onMove();
    }

    private void onChangeVelocity()
    {
        currentVelocity -= DecreaseVelocity;
        currentVelocity = Mathf.Clamp(currentVelocity, MinVelocity, MaxVelocity);
    }

    private void onMove()
    {
        Vector3 force = transform.forward * currentVelocity;
        force.y = 0;
        rBody.AddForce(force);
        rBody.velocity = new Vector3(rBody.velocity.x, 0, rBody.velocity.z);
        onChangeVelocity();
    }




}
