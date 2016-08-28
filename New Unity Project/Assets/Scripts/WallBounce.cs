using UnityEngine;
using System.Collections;

public class WallBounce : MonoBehaviour {

	void OnCollisionEnter(Collision collision)
	{
		
		if(collision.gameObject.CompareTag("Orb"))
		{

			Debug.Log ("bounce");
			AkSoundEngine.PostEvent ("STAGE_FORCE_FIELD", gameObject);
			GameObject Force_Field = Instantiate (Resources.Load<GameObject> ("Force_Field"), collision.transform.position, Quaternion.identity) as GameObject;
			Force_Field.transform.localEulerAngles = new Vector3 (10, transform.localEulerAngles.y, 0);
			Destroy (Force_Field, 2);
		}

	}
}
