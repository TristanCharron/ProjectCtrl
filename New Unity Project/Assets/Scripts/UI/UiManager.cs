using UnityEngine;
using System.Collections;

public class UiManager : MonoBehaviour {

    public GameObject titleContainer;
    public GameObject player;
    public GameObject[] players;
    public Vector3 SpawnPoints;
    int nbPlayers;

	// Use this for initialization
	void Start () {
        nbPlayers = 4;
        foreach(GameObject g in players)
        {

        }
	}
	
	// Update is called once per frame
	void Update () {
	
        if(Input.GetMouseButtonDown(0))
        {
            titleContainer.SetActive(false);
            SpawnPosition(nbPlayers);
        }
	}

    void SpawnPosition(int id)
    {
        for(int x = 0; x <= players.Length; x++)
        {
          GameObject clone = Instantiate(player, new Vector3(SpawnPoints.x, 2, SpawnPoints.z), Quaternion.identity) as GameObject;
        }
    }
}
