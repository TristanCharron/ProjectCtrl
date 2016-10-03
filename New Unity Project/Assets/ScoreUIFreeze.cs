using UnityEngine;
using System.Collections;

public class ScoreUIFreeze : MonoBehaviour {
    Vector3 originalPosition;
	// Use this for initialization
	void Start () {
        originalPosition = transform.localPosition;

    }
	
	// Update is called once per frame
	void Update () {
        if(transform.localPosition != originalPosition)
            transform.localPosition = originalPosition - transform.localPosition;

    }
}
