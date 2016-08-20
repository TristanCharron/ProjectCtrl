using UnityEngine;
using System.Collections;

public class ballManager : MonoBehaviour {

    public static ballManager Instance { get { return instance; } }
    static ballManager instance;

    private static float currentVelocity,minVelocity,maxVelocity; //Speed variables

    public static float MaxVelocity { get { return maxVelocity; } }

    public static float MinVelocity { get { return minVelocity; } }

    public static float CurrentSpeed { get { return Mathf.Clamp(currentVelocity, minVelocity, maxVelocity); } } //Clamp & Return speed

    public float _MaxVelocity, _MinVelocity;

    private Rigidbody rBody;
    


	// Use this for initialization
	void Awake () {
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
        minVelocity = _MinVelocity;
        maxVelocity = _MaxVelocity;
        currentVelocity = minVelocity;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        onMove();

    }

    private void onChangeVelocity()
    {

    }
    
    private void onMove()
    {
        rBody.AddForce(Vector3.forward * currentVelocity);
        currentVelocity--;

    }
}
