using UnityEngine;
using System.Collections;

public class WwiseManager : MonoBehaviour {
    public static bool isWwiseEnabled;
    public bool _isWwiseEnabled;
    public GameObject WwiseGlobalObject;

	// Use this for initialization
	void Awake () {
        isWwiseEnabled = _isWwiseEnabled;
        WwiseGlobalObject = GameObject.Find("WwiseGlobal");
        onSetWwiseByState();
     
        
    }

    void onSetWwiseByState()
    {
        
        Camera.main.gameObject.GetComponent<AkAudioListener>().enabled = isWwiseEnabled;
        if(!isWwiseEnabled)
        {
            Destroy(WwiseGlobalObject);
        }
        
    }

	


    public static void onPlayWWiseEvent(string nameEvent, GameObject gObject)
    {
        if (isWwiseEnabled)
            AkSoundEngine.PostEvent(nameEvent, gObject);
    }
}
