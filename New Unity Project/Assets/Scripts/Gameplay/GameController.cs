using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {


    public static bool isGameStarted { get { return gameStarted; } }
    static bool gameStarted = false;

    public GameObject _Orb;
    private static GameObject Orb;

    public Transform[] _OrbSpawnPoints;
    private static Transform[] OrbSpawnPoints;

    public const int nbBellHits = 1;

    public static void onSetGameStartedState(bool state)
    {
        gameStarted = state;

        if (state)
            onReset();
    }

    public static void onReset()
    {
        TeamController.onReset();
    }

    public static void onSpawnOrb()
    {
        Orb.SetActive(true);
        Orb.transform.position = OrbSpawnPoints[Random.Range(0, OrbSpawnPoints.Length)].position;
    }


    // Use this for initialization
    void Awake () {
        Orb = _Orb;
        OrbSpawnPoints = _OrbSpawnPoints;
    }

  
	
}
