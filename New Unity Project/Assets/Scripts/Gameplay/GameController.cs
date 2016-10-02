using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public const int nbRounds = 3;

    public static bool isGameStarted { get { return gameStarted; } }
    static bool gameStarted = false;

    public GameObject _Orb;
    private static GameObject Orb;

    public Transform[] _OrbSpawnPoints;
    private static Transform[] OrbSpawnPoints;

    private static int nbRoundsPlayed = 0;
    public static int NbRoundsPlayed { get { return nbRoundsPlayed; } }



    private static GameController instance;
    public static GameController Instance { get { return instance; } }


    public const int nbBellHits = 1;

    public static void onSetGameStartedState(bool state)
    {
        gameStarted = state;
    }

    public static void onNextRound()
    {
        if (isGameCompleted())
            onGameOver();
        else
            onSetNextRound();
    }

    static void onComplete()
    {
        nbRoundsPlayed = 0;
    }


    static void onSetNextRound()
    {
        TeamController.onReset();
        nbRoundsPlayed++;
        UiManager.Instance.StartCoroutine(UiManager.Instance.OnBeginGame());
        Orb.gameObject.SetActive(true);
        OrbController.onResetOrb();
        Orb.transform.position = OrbSpawnPoints[Random.Range(0, OrbSpawnPoints.Length)].position;
    }

    static void onGameOver()
    {
        onComplete();
        TeamController.OnComplete();
        SceneManager.LoadScene(0);
    }



    public static bool isGameCompleted()
    {
        return nbRoundsPlayed >= nbRounds;
    }


    // Use this for initialization
    void Awake()
    {
        Orb = _Orb;
        OrbSpawnPoints = _OrbSpawnPoints;
        instance = this;
    }




}
