using UnityEngine;
using System.Collections;

public class BounceObstacle : MonoBehaviour {

	void OnCollisionEnter(Collision collision)
	{
		
		if(collision.gameObject.CompareTag("Orb"))
		{
			WwiseManager.onPlayWWiseEvent("STAGE_FORCE_FIELD", gameObject);
            onSpawnRipple(collision.transform.position);

		}

	}

    void onSpawnRipple(Vector3 spawnlocation)
    {
        GameObject Force_Field = Instantiate(Resources.Load<GameObject>("Force_Field"), spawnlocation, Quaternion.identity) as GameObject;
        Force_Field.transform.localEulerAngles = new Vector3(10, transform.localEulerAngles.y, 0);
        Destroy(Force_Field, 2);
    }
}
