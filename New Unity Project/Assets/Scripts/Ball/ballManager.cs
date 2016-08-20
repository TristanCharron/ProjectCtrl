using UnityEngine;
using System.Collections;

public class ballManager : MonoBehaviour {
    private static float currentSpeed,minSpeed,maxSpeed; //Speed variables

    public static float MaxSpeed { get { return maxSpeed; } }

    public static float MinSpeed { get { return minSpeed; } }

    public static float CurrentSpeed { get { return Mathf.Clamp(currentSpeed,minSpeed,MaxSpeed); } } //Clamp & Return speed

    public float _MaxSpeed, _MinSpeed;

    private Rigidbody rBody;
    


	// Use this for initialization
	void Awake () {
        onSetComponents();
        onSetProperties();

    }

    private void onSetComponents()
    {
        rBody = GetComponent<Rigidbody>();
    }

    private void onSetProperties()
    {
        minSpeed = _MinSpeed;
        maxSpeed = _MaxSpeed;
        currentSpeed = minSpeed;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        rBody.AddForce(Vector3.forward * currentSpeed);

    }
    
    private static void onMove()
    {

    }
}
