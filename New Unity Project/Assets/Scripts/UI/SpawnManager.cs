﻿using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

    public Transform[] SpawnPoints;
    //public GameObject player;
    public GameObject[] players;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	  
	}

    void SpawnPosition(int id)
    {
        for (int x = 0; x <= players.Length; x++)
        {
           
        }
    }

    public void resetPosition()
    {
        foreach(GameObject player in players)
        {
            for(int x = 0; x < players.Length; x++)
            {
                if(!player.activeInHierarchy)
                player.SetActive(true);


                player.transform.position = player.transform.parent.position;
            }
      
        }
    }
}
