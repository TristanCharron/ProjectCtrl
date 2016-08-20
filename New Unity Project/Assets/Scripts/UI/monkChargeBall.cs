using UnityEngine;
using System.Collections;

public class monkChargeBall : MonoBehaviour {

    float t;
    public GameObject chargeBar, meterReload;
    bool isChargeActive;

	// Use this for initialization
	void Start () {
        chargeBar.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (t < 1 && isChargeActive)
        {
            t += Time.deltaTime;

            chargeBar.SetActive(true);
            chargeBar.transform.localPosition = new Vector3(Mathf.Lerp(-0.255f, 0.2569f, t), chargeBar.transform.localPosition.y, chargeBar.transform.localPosition.z);

        }
        else
        {
            meterReload.SetActive(false);
        }

        if(Input.GetButtonDown("Jump"))
        {
            if(chargeBar.transform.localPosition.x < -0.1006f && chargeBar.transform.localPosition.x > -0.1465f)
            {

                Debug.Log(chargeBar.transform.localPosition.x);
            }

            meterReload.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            callCharge();
        }
       
    }

    public void callCharge()
    {
        t = 0;
        isChargeActive = true;
        meterReload.SetActive(true);
    }
}
