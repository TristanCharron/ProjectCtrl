using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class combatManager : MonoBehaviour {

    static spawnPoint[] spawnList;
    static playerManager[] playerList;
    public static int nbPlayers = 2;
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
        for (int i = 0; i < spawnList.Length; i++)
        {

            if (!spawnList[i].isOccupied)
                return i;
            else if (backup == -1)
                backup = i;
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


    // Update is called once per frame
    void Update () {
	
	}
}
