using UnityEngine;
using System.Collections;

public class onCheckPullState : MonoBehaviour {

    PlayerController playerController;

    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.CompareTag ("Orb"))
            playerController.onPull ();
    }


}
