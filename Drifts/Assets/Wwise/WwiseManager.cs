using UnityEngine;
using System.Collections;

public class WwiseManager : MonoBehaviour
{
    [SerializeField]
    private bool IsWwiseEnabled;

    public static WwiseManager Instance { private set; get; }

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        SetWwiseFromState();
    }

    void SetWwiseFromState()
    {
        Camera.main.gameObject.GetComponent<AkAudioListener>().enabled = IsWwiseEnabled;

        if (!IsWwiseEnabled)
            Destroy(gameObject);
    }




    public static void PostEvent(string nameEvent, GameObject gObject)
    {
        AkSoundEngine.PostEvent(nameEvent, gObject);
    }
}
