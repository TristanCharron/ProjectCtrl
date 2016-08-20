using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

    public GameObject gameOverContainer;
    public Transform[] SpawnPoints;
    //public GameObject player;
    public GameObject[] players;
   public bool allPlayerDead, aPlayerDead;
    // Use this for initialization
    void Start () {
        gameOverContainer.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	     if(aPlayerDead)
        {
            checkPlayerAlive();
        }
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
        if (allPlayerDead)
        {
            allPlayerDead = false;
            gameOverContainer.SetActive(false);
        }
            
    }

    void checkPlayerAlive()
    {
        for(int x = 0; x < players.Length; x++)
        {
            if (!players[x].activeInHierarchy)
                aPlayerDead = false;
            else
                allPlayerDead = true;
                
        }
        if(allPlayerDead)
        {
       
            gameOverContainer.SetActive(true);
        }
    }
}
