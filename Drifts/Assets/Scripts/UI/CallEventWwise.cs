using UnityEngine;
using System.Collections;

public class CallEventWwise : MonoBehaviour {

	// Use this for initialization
	public void PlaySound (string name) 
	{
        if (WwiseManager.isWwiseEnabled)
            AkSoundEngine.PostEvent (name, gameObject);
	}

}
