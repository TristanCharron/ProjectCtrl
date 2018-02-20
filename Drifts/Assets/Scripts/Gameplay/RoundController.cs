using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RoundController : MonoBehaviour
{

    public static RoundController Instance
    {
        get
        {
            return instance;
        }
    }
    private static RoundController instance;
    public Transform[] _SpawnPoints;
    private static Transform[] spawnPoints;
    //public GameObject player;
    public GameObject[] _Players;

    public Text roundNbText;

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

    public void OnResetProperties()
    {
        listPlayerDead = new List<int>();
        isTeamDead = false;
        players = _Players;
        spawnPoints = _SpawnPoints;

    }

    public void onEnablePlayers()
    {

        for (int x = 0; x < _Players.Length; x++)
        {
            if (!_Players[x].activeInHierarchy)
                _Players[x].SetActive(true);
        }


    }

    void Update()
    {
        roundNbText.text = "0" + GameController.NbRoundsPlayed.ToString();
    }


    static void OnBeginNextRoundEvent()
    {
        instance.OnResetProperties();
        instance.onEnablePlayers();
        GameController.onNextRound();
    }

    public static void onResetPosition()
    {
        for (int x = 0; x < players.Length; x++)
        {
            players[x].transform.position = spawnPoints[x].transform.position;
        }

        RoundUIController.onGameOverScreen(false);

        if (IsTeamDead)
        {
            isTeamDead = false;
            RoundUIController.onGameOverScreen(false);
        }

    }


    public static void OnCheckGameOver()
    {
        if (listPlayerDead.Contains(1) && listPlayerDead.Contains(2))
            OnTeamVictory(TeamController.TeamList[1]);
        else if (listPlayerDead.Contains(3) && listPlayerDead.Contains(4))
            OnTeamVictory(TeamController.TeamList[0]);
    }

    public static void OnTeamVictory(Team team)
    {
        string wwiseEvent = team.Index == 1 ? "GAME_END_BLUE" : "GAME_END_RED";
        instance.StartCoroutine(instance.onTeamWinCoRoutine(wwiseEvent, team));
        listPlayerDead.Clear();
    }

    public IEnumerator onTeamWinCoRoutine(string wwiseTeamNameEvent, Team winningTeam)
    {
        winningTeam.onSetWinningState(true);
        TeamController.onReturnOtherTeam(winningTeam).onSetWinningState(false);
        GameController.onSetGameStartedState(false);
		OrbController.Instance.ShouldBallBeEnabled(false);
        RoundUIController.OnGetTeamContainer(winningTeam).SetActive(true);
        WwiseManager.PostEvent(wwiseTeamNameEvent, gameObject);
        yield return new WaitForSeconds(3f);
        RoundUIController.OnGetTeamContainer(winningTeam).SetActive(false);
        OnBeginNextRoundEvent();

    }



    public static void onPlayerDeath(int id)
    {
        WwiseManager.PostEvent("MONK_DEAD", players[id - 1].gameObject);
        listPlayerDead.Add(id);
        players[id - 1].gameObject.SetActive(false);
        OnCheckGameOver();


    }

}
