using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {

    public GameObject gameOverContainer, monkReviveCharge;
    public Transform[] SpawnPoints;
    //public GameObject player;
    public GameObject[] players;
   public bool allPlayerDead, aPlayerDead;
    public List<int> nbPlayerDead = new List<int>();
    // Use this for initialization
    void Start () {
        gameOverContainer.SetActive(false);
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

        gameOverContainer.SetActive(false);
        if (allPlayerDead)
        {
            allPlayerDead = false;
            gameOverContainer.SetActive(false);
        }
            
    }

  public void checkPlayerAlive()
    {
       
        
      /*
        for(int x = 0; x < players.Length; x++)
        {
            if (!players[x].activeInHierarchy)
                aPlayerDead = false;
            else
                allPlayerDead = true;
                
        }
        */
        if(nbPlayerDead.Contains(1) && nbPlayerDead.Contains(2))
        {
            Debug.Log("Red Team Wins");
            gameOverContainer.SetActive(true);
            nbPlayerDead.Clear();
        }
        else if (nbPlayerDead.Contains(3) && nbPlayerDead.Contains(4))
        {
            Debug.Log("Blue Team Wins");
            gameOverContainer.SetActive(true);
            nbPlayerDead.Clear();
        }
    }

   public void killPlayer(int id)
    {
        nbPlayerDead.Add(id);
        players[id - 1].gameObject.SetActive(false);
        checkPlayerAlive();

    }

    public void reviveAlly()
    {

    }
}
