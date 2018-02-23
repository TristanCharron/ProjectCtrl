using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    [SerializeField]
    private Transform[] SpawnPoints;

    [SerializeField]
    private PlayerController[] Players;

    public Transform[] _OrbSpawnPoints;

    public static GameController Instance { private set; get; }

    public  bool IsGameStarted { private set; get; }
    public  int NbRoundsPlayed { private set; get; }


    public delegate void GameDelegate();

    public static event GameDelegate  EndRoundEvent, SetNextRoundEvent, 
        StartGameEvent, EndGameEvent;


    public static GameMode CurrentGameMode { private set; get; }


    void Awake()
    {
        Instance = this;
        IsGameStarted = false;
        NbRoundsPlayed = 0;
        SetGameModeByID();
        EndGameEvent += RemoveGameMode;
    }



    void SetGameModeByID()
    {
        GameModeID id = GameModeID.DEATHMATCH;

        switch (id)
        {
            case GameModeID.DEATHMATCH:
                CurrentGameMode = gameObject.AddComponent<GameModeDeathMatch>();
                CurrentGameMode.Initialize(SpawnPoints, Players);
                break;
            case GameModeID.SCOREMATCH:
                break;
            default:
                break;
        }
    }

    void RemoveGameMode()
    {
        Destroy(CurrentGameMode, 1);
    }


    public void KillPlayer(PlayerController deadPlayer)
    {
        CurrentGameMode.KillPlayer(deadPlayer);
    }



    public void ChangeGameStartedState(bool state)
    {
        IsGameStarted = state;
    }

    public void NextRound()
    {
        WwiseManager.PostEvent("GAME_PLAY", Camera.main.gameObject);

        EndRoundEvent();

        if (IsLastRound())
            EndGame();
        else
        {
            NbRoundsPlayed++;
            SetNextRoundEvent();
        }
    }

    public void StartGame()
    {
        if(StartGameEvent != null)
            StartGameEvent();

        NextRound();
    }

    public void EndGame()
    {
    

        if (TeamController.Instance.TeamList[0].TotalScore > TeamController.Instance.TeamList[1].TotalScore)
            Instance.StartCoroutine(Instance.EndGameCoRoutine("GAME_END_BLUE", TeamController.Instance.TeamList[0]));
        else
            Instance.StartCoroutine(Instance.EndGameCoRoutine("GAME_END_RED", TeamController.Instance.TeamList[1]));

    }


    public IEnumerator EndGameCoRoutine(string wwiseTeamNameEvent, Team winningTeam)
    {
        ChangeGameStartedState(false);
        OrbController.Instance.gameObject.SetActive(false);
        RoundUIController.SetGameOverScreen(true);

        yield return new WaitForSeconds(2f);

        RoundUIController.SetGameOverScreen(false);
        RoundUIController.GetTeamContainer(winningTeam).SetActive(true);

        WwiseManager.PostEvent(wwiseTeamNameEvent, gameObject);

        yield return new WaitForSeconds(5f);
        RoundUIController.GetTeamContainer(winningTeam).SetActive(false);

        UIEffectManager.Instance.FadeToWhite(true);

        OnEndGameEvent();

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(0);
    }




    public bool IsGameCompleted()
    {
        return NbRoundsPlayed > CurrentGameMode.NbRounds;
    }

    public bool IsLastRound()
    {
        return NbRoundsPlayed == CurrentGameMode.NbRounds;
    }


    private void OnEndGameEvent()
    {
        EndGameEvent();
        StartGameEvent = null;
        SetNextRoundEvent = null;
        EndRoundEvent = null;
        EndGameEvent = null;
    }



}
