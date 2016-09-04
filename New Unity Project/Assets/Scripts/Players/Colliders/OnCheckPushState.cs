using UnityEngine;
using System.Collections;

public class OnCheckPushState : MonoBehaviour {

    PlayerController playerController;

    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }


    void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.CompareTag ("Orb"))
            playerController.onPush ();
    }

}
