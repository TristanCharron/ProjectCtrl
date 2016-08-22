using UnityEngine;
using System.Collections;

public class Cloche : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Orb"))
		{
			AkSoundEngine.PostEvent ("STAGE_BELL", gameObject);

			GetComponent<Animator> ().Play ("DONG",0,-1);
			UiManager.OnFreezeFrame (.1f,1);

			ballManager.onPush (ballManager.MomentumBell + ballManager.CurrentVelocity);
		}
	}

}
