using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public const int nbRounds = 3;

    public static bool isGameStarted { get { return gameStarted; } }
    static bool gameStarted = false;

    public GameObject _Orb;
    private static GameObject Orb;

    public Transform[] _OrbSpawnPoints;
    private static Transform[] OrbSpawnPoints;

    private static int nbRoundsPlayed;
    public static int NbRoundsPlayed { get { return nbRoundsPlayed; } }

  
    public const int nbBellHits = 1;

    public static void onSetGameStartedState(bool state)
    {
        gameStarted = state;

        if (state)
            onReset();
    }

    public static void onStartNewRound()
    {
        TeamController.onReset();
        nbRoundsPlayed++;
        onSpawnOrb();
        UiManager.Instance.StartCoroutine(UiManager.Instance.OnBeginGame());
    }

    public static void onReset()
    {
        TeamController.onReset();
        nbRoundsPlayed = 0;
    }

    public static bool isGameCompleted()
    {
        return nbRoundsPlayed >= nbRounds;
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
