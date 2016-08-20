using UnityEngine;
using System.Collections;

public class Mage : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
     
        //transform.Rotate(Vector3.up * Time.fixedDeltaTime * 300);
        onRotate();
        if(Input.GetMouseButtonDown(0))
        {
            //ballManager.onPush(transform.forward, 100);
        }


    }

    

    void onRotate()
    {
        Vector3 v3T = Input.mousePosition;
        v3T.z = Mathf.Abs(Camera.main.transform.position.y - transform.position.y);
        v3T = Camera.main.ScreenToWorldPoint(v3T);
        v3T -= transform.position;
        v3T = v3T * 10000.0f + transform.position;
        transform.LookAt(v3T);

    }


}
