using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public const int nbRounds = 3;

    public static bool isGameStarted { get { return gameStarted; } }
    static bool gameStarted = false;

    public Transform[] _OrbSpawnPoints;

    private static int nbRoundsPlayed = 0;
    public static int NbRoundsPlayed { get { return nbRoundsPlayed; } }

    private static GameController instance;
    public static GameController Instance { get { return instance; } }


    public delegate void GameDelegate();
    public static event GameDelegate SetNextRound;


    public static void onSetGameStartedState(bool state)
    {
        gameStarted = state;
    }

    public static void onNextRound()
    {
		WwiseManager.PostEvent("GAME_PLAY", Camera.main.gameObject);
		ScoreController.onAddTeamScores();

        if (isLastRound())
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
        nbRoundsPlayed++;
        SetNextRound();
    }


    public IEnumerator onTeamWinCoRoutine(string wwiseTeamNameEvent, Team winningTeam)
    {
        onSetGameStartedState(false);
		OrbController.Instance.ShouldBallBeEnabled(false);
        RoundUIController.onGameOverScreen(true);

        yield return new WaitForSeconds(2f);

        RoundUIController.onGameOverScreen(false);
        RoundUIController.OnGetTeamContainer(winningTeam).SetActive(true);

        WwiseManager.PostEvent(wwiseTeamNameEvent, gameObject);

        yield return new WaitForSeconds(5f);
        RoundUIController.OnGetTeamContainer(winningTeam).SetActive(false);

        UIEffectManager.Instance.FadeToWhite(true);

        yield return new WaitForSeconds(2f);

        onComplete();
        TeamController.OnComplete();

        SceneManager.LoadScene(0);
    }


    public static void onGameOver()
    {
        if (TeamController.TeamList[0].TotalScore > TeamController.TeamList[1].TotalScore)
            instance.StartCoroutine(instance.onTeamWinCoRoutine("GAME_END_BLUE", TeamController.TeamList[0]));
        else
            instance.StartCoroutine(instance.onTeamWinCoRoutine("GAME_END_RED", TeamController.TeamList[1]));

    }

    public static bool isGameCompleted()
    {
        return nbRoundsPlayed > nbRounds;
    }

    public static bool isLastRound()
    {
        return nbRoundsPlayed == nbRounds;
    }


    void Awake()
    {
        instance = this;
        gameStarted = false;
    }
}
