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
        switch (GameMode.currentGameMode)
        {
            case GameModeID.DEATHMATCH:
                CurrentGameMode = gameObject.AddComponent<GameModeDeathMatch>();
                CurrentGameMode.Initialize(SpawnPoints, Players);
                break;
            case GameModeID.BELL:
				CurrentGameMode = gameObject.AddComponent<GameModeBell>();
				CurrentGameMode.Initialize(SpawnPoints, Players);
				break;
			case GameModeID.NONE:
				Debug.Log("QUIT");
				break;
		default:
			break;
        }
    }

    void RemoveGameMode()
    {
        Destroy(CurrentGameMode, 1);
    }

	// Bientot obsolete
    public void KillPlayer(PlayerController deadPlayer)
    {
        CurrentGameMode.KillPlayer(deadPlayer);
    }


    public void ChangeGameStartedState(bool state)
    {
        IsGameStarted = state;
    }

	/// <summary>
	/// A refactor pour le placer dans les GameMode
	/// </summary>
    public void NextRound()
    {
		CurrentGameMode.BeginNextRoundExtra();

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

	/// <summary>
	/// Le endgame trigger par la vielle version avec les rounds, discussion a avoir sur les rounds/points etc
	/// </summary>
    public void EndGame()
    {
		Debug.Log("score end game");
		if (TeamController.Instance.TeamList[0].TotalScore > TeamController.Instance.TeamList[1].TotalScore)
            StartCoroutine(EndGameCoRoutine("GAME_END_BLUE", TeamController.Instance.TeamList[0]));
        else
            StartCoroutine(EndGameCoRoutine("GAME_END_RED", TeamController.Instance.TeamList[1]));
    }

	/// <summary>
	/// Le endgame trigger par different gameMode
	/// </summary>
	/// <param name="winningTeam">Winning team.</param>
	public void EndGame(Team winningTeam)
	{
		CurrentGameMode.EndRound(winningTeam);
	}


    public IEnumerator EndGameCoRoutine(string wwiseTeamNameEvent, Team winningTeam)
    {
		Debug.Log("real end game");
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
