using UnityEngine;
using System.Collections;

public class Scalable : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   public void onScale(float newValue)
    {
        transform.localScale = new Vector3(newValue, transform.localScale.y, transform.localScale.z);
    }
}
