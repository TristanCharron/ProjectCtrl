using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class combatManager : MonoBehaviour {

    static spawnPoint[] spawnList;
    public static spawnPoint[] SpawnList { get { return spawnList; } }

    static playerManager[] playerList;
    public static playerManager[] PlayerList { get { return playerList; } }

    public static int NbPlayersAlive
    {
        get { return nbPlayersAlive; }
      
    }

   
    public static int nbPlayers = 2;
    static int nbPlayersAlive = nbPlayers;

    public static combatManager Instance;
    public static List<Player> robotList;

    public class spawnPoint
    {
        protected bool occupied;
        protected Vector3 position;
        public bool isOccupied {get{ return occupied; } }
        public Vector3 worldPosition { get { return position; } }

        public spawnPoint(Vector3 pos)
        {
            occupied = false;
            position = pos;
        }

        public void onOccupy(bool state)
        {
            occupied = state;
        }

        public IEnumerator onCountdown(float length)
        {
            yield return new WaitForSeconds(length);
            occupied = false;
            yield break;
        }

    }

    // Use this for initialization
    void Awake () {
        Instance = this;
        onGenerateSpawnPoints();
        onGeneratePlayers();
    }

   
    void onGeneratePlayers()
    {
        playerList = new playerManager[nbPlayers];
        for (int i = 0; i < nbPlayers; i++)
        {
            GameObject player = Instantiate(Resources.Load("Players/Player"), returnSpawnPoint().worldPosition, Quaternion.identity) as GameObject;
            playerList[i] = player.GetComponent<playerManager>();
            playerList[i].onActivate(i);
            UIManager.onActivate(i, playerList[i]);
        }
        nbPlayersAlive = nbPlayers;

    }

    void onGenerateSpawnPoints()
    {
        if(spawnList == null)
        {
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
            spawnList = new spawnPoint[spawnPoints.Length];

            for(int i = 0; i < spawnList.Length; i++)
            {
                spawnList[i] = new spawnPoint(spawnPoints[i].transform.position);
            }
        }
    }

    static int returnAvailableSpawnIndex()
    {
        int backup = -1;
        int counter = 0;
        while (backup < 0)
        {
            int index = Random.Range(0, spawnList.Length);
            if (!spawnList[index].isOccupied)
                backup = index;

            counter++;

            if (counter == spawnList.Length)
                backup = index;
        }
        return backup;
    }

    static spawnPoint returnSpawnPoint()
    {
        spawnPoint sP = spawnList[returnAvailableSpawnIndex()];
        sP.onOccupy(true);
        Instance.StartCoroutine(sP.onCountdown(5f));
        return sP;
    }

    public static void onPlayerDeath(int index)
    {
        playerList[index].setActive(false);
        playerList[index].setAlive(false);
        UIManager.onDesactivate(index);
        nbPlayersAlive--;
    }


    // Update is called once per frame
    void Update () {


    }


    void onCalculateCameraBounds()
    {

    }
}
