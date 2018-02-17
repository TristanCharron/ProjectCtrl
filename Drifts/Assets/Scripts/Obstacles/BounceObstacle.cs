using UnityEngine;
using System.Collections;

public class BounceObstacle : MonoBehaviour {

	void OnCollisionEnter(Collision col)
	{
		
		if(col.gameObject.CompareTag("Orb"))
		{
			WwiseManager.onPlayWWiseEvent("STAGE_FORCE_FIELD", gameObject);
            onSpawnRipple(col.transform.position);

			//Vector3 newDir = OrbController.DestinationAngle;
			//OrbController.Push(newDir,OrbController.CurrentVelocity);
		}

	}

    void onSpawnRipple(Vector3 spawnlocation)
    {
        GameObject Force_Field = Instantiate(Resources.Load<GameObject>("Force_Field"), spawnlocation, Quaternion.identity) as GameObject;
        Force_Field.transform.localEulerAngles = new Vector3(10, transform.localEulerAngles.y, 0);
        Destroy(Force_Field, 2);
    }
}
