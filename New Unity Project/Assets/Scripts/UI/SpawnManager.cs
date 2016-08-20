using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {

    public static SpawnManager Instance
    {
        get
        {
            return instance;
        }
    }
    private static SpawnManager instance;
    public GameObject gameOverContainer, monkReviveCharge;
    public Transform[] SpawnPoints;
    //public GameObject player;
    public GameObject[] _Players;
    private static GameObject[] players;
    public static GameObject[] Players { get { return players; } }
    public static bool IsTeamDead { get { return isTeamDead; } }
    private static bool isTeamDead;
    private static List<int> listPlayerDead = new List<int>();
    public static List<int> ListPlayerDead { get { return ListPlayerDead; } }
    
    // Use this for initialization
    void Start () {
        instance = this;
        players = _Players;
        gameOverContainer.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    static void onReset()
    {
        instance.gameOverContainer.SetActive(true);
        listPlayerDead.Clear();
    }

    static void onSpawnPosition(int id)
    {
        for (int x = 0; x <= players.Length; x++)
        {
           
        }
    }

    public static void onResetPosition()
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

        instance.gameOverContainer.SetActive(false);
        if (IsTeamDead)
        {
            isTeamDead = false;
            instance.gameOverContainer.SetActive(false);
        }
            
    }

  public static void isTeamDefeated()
    {

        if(listPlayerDead.Contains(1) && listPlayerDead.Contains(2))
        {
            Debug.Log("Red Team Wins");
            onReset();
        }
        else if (listPlayerDead.Contains(3) && listPlayerDead.Contains(4))
        {
            Debug.Log("Blue Team Wins");
            onReset();
        }
    }

   public static void onPlayerDeath(int id)
    {
        listPlayerDead.Add(id);
        players[id - 1].gameObject.SetActive(false);
        isTeamDefeated();

    }

    public void onReviveAlly()
    {

    }
}
