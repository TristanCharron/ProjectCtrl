using UnityEngine;
using System.Collections;

public class combatManager : MonoBehaviour {

    static playerManager[] playerList;
    public static int nbPlayers = 1;

    // Use this for initialization
    void Awake () {
        onGeneratePlayers();
       
    }

    void onGeneratePlayers()
    {
        playerList = new playerManager[nbPlayers];
        for (int i = 0; i < nbPlayers; i++)
        {
            GameObject player = Instantiate(Resources.Load("Players/Player"), Vector3.zero, Quaternion.identity) as GameObject;
            playerList[i] = player.GetComponent<playerManager>();
            UIManager.onAddPlayer(i, playerList[i]);
            playerList[i].onStart(i);

        }

    }


    // Update is called once per frame
    void Update () {
	
	}
}
