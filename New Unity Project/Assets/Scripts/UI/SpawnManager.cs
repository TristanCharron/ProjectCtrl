using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class SpawnManager : MonoBehaviour
{

    public static SpawnManager Instance
    {
        get
        {
            return instance;
        }
    }
    private static SpawnManager instance;
    public GameObject monkReviveCharge;
    public Transform[] _SpawnPoints;
    private static Transform[] spawnPoints;
    //public GameObject player;
    public GameObject[] _Players;
    private static GameObject[] players;
    public static GameObject[] Players { get { return players; } }
    public static bool IsTeamDead { get { return isTeamDead; } }
    private static bool isTeamDead;
    private static List<int> listPlayerDead = new List<int>();
    public static List<int> ListPlayerDead { get { return ListPlayerDead; } }


    // Use this for initialization
    void Awake()
    {
        instance = this;
        OnResetProperties();
        onEnablePlayers();
    }

    void OnResetProperties()
    {
        listPlayerDead = new List<int>();
        isTeamDead = false;
        players = _Players;
        spawnPoints = _SpawnPoints;

    }

    void onEnablePlayers()
    {

        for (int x = 0; x < _Players.Length; x++)
        {
            if (!_Players[x].activeInHierarchy)
                _Players[x].SetActive(true);
        }


    }


    static void onReset()
    {
        instance.OnResetProperties();
        instance.onEnablePlayers();
        GameController.onStartNewRound();
    
    }

    public static void onResetPosition()
    {
        for (int x = 0; x < players.Length; x++)
        {
            players[x].transform.position = spawnPoints[x].transform.position;
        }

        UiManager.onGameOverScreen(false);

        if (IsTeamDead)
        {
            isTeamDead = false;
            UiManager.onGameOverScreen(false);
        }

    }


    public static void onTeamWin()
    {
        if (listPlayerDead.Contains(1) && listPlayerDead.Contains(2))
        {
            instance.StartCoroutine(instance.onTeamWinCoRoutine("GAME_END_RED", UiManager.Instance.RedTeamWin));
            listPlayerDead.Clear();
        }
            
        else if (listPlayerDead.Contains(3) && listPlayerDead.Contains(4))
        {
            instance.StartCoroutine(instance.onTeamWinCoRoutine("GAME_END_BLUE", UiManager.Instance.BlueTeamWin));
            listPlayerDead.Clear();
        }
            

    }

    public IEnumerator onTeamWinCoRoutine(string wwiseTeamNameEvent, GameObject teamUIContainer)
    {
        GameController.onSetGameStartedState(false);
        OrbController.shouldBallBeEnabled(false);
        UiManager.onGameOverScreen(true);
        WwiseManager.onPlayWWiseEvent("GAME_OVER", gameObject);

        yield return new WaitForSeconds(2f);
        UiManager.onGameOverScreen(false);
        teamUIContainer.SetActive(true);
        WwiseManager.onPlayWWiseEvent(wwiseTeamNameEvent, gameObject);

        yield return new WaitForSeconds(5f);
        onReset();

    }



    public void onPlayerDeath(int id)
    {
        WwiseManager.onPlayWWiseEvent("MONK_DEAD", players[id - 1].gameObject);
        listPlayerDead.Add(id);
        players[id - 1].gameObject.SetActive(false);
        onTeamWin();


    }

}
