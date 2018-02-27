using UnityEngine;
using System.Collections;

public class WwiseManager : MonoBehaviour
{
    static bool isWwiseEnabled = true;

    public static WwiseManager Instance { private set; get; }

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        SetWwiseFromState();
    }

    void SetWwiseFromState()
    {

		Camera.main.gameObject.GetComponent<AkAudioListener>().enabled = WwiseManager.isWwiseEnabled;

		if (!WwiseManager.isWwiseEnabled)
            Destroy(gameObject);
    }




    public static void PostEvent(string nameEvent, GameObject gObject)
    {
		if(isWwiseEnabled)
       	 AkSoundEngine.PostEvent(nameEvent, gObject);
    }
}
