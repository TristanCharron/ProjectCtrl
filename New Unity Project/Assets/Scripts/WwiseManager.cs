using UnityEngine;
using System.Collections;

public class WwiseManager : MonoBehaviour {
    private static bool wwiseEnabled;
    public static bool isWwiseEnabled;
    public bool _isWwiseEnabled;

	// Use this for initialization
	void Awake () {
        isWwiseEnabled = _isWwiseEnabled;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
