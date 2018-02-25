using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameModeID
{
    DEATHMATCH,
    BELL,
	NONE
}



public abstract class GameMode : MonoBehaviour
{
	public static GameModeID currentGameMode;

    // Game Mode Dependent

    public int NbRounds { protected set; get; }

    public int NbTeams { protected set; get; }

    public int NbPlayers { protected set; get; }

    public Transform[] SpawnPoints { protected set; get; }

    public PlayerController[] Players { protected set; get; }

    public GameModeID EnumID { protected set; get; }

    public GameMode()
    {

    }

    public void Initialize(Transform[] spawnPoints, PlayerController[] players)
    {
        Players = players;
        SpawnPoints = spawnPoints;
		Debug.Log("init mode");
    }



    protected virtual void BeginNextRound()
    {
        EnablePlayers();
        GameController.Instance.NextRound();
    }

	/// <summary>
	/// Tant que tout est pas fully swapper au game mode
	/// </summary>
	public virtual void BeginNextRoundExtra()
	{
		Debug.Log("extra nothing");

	}

    public virtual void EnablePlayers()
    {
        for (int x = 0; x < Players.Length; x++)
        {
            if (!Players[x].gameObject.activeInHierarchy)
                Players[x].gameObject.SetActive(true);

            Players[x].transform.position = SpawnPoints[x].transform.position;

        }
        RoundUIController.SetGameOverScreen(false);
    }


    public virtual void KillPlayer(PlayerController deadPlayer)
    {
        bool isTeamDead = false;

        TeamController.Instance.TeamList[(int)deadPlayer.CurrentTeamID].KillPlayer(deadPlayer, out isTeamDead);
        if (isTeamDead)
        {
            Team winningTeam = TeamController.Instance.GetOtherTeam(TeamController.Instance.TeamList[(int)deadPlayer.CurrentTeamID]);
            EndRound(winningTeam);
        }
    }


    public virtual void EndRound(Team team)
    {
        string wwiseEvent = team.Index == 1 ? "GAME_END_BLUE" : "GAME_END_RED";
        StartCoroutine(EndRoundCoRoutine(wwiseEvent, team));
    }


    protected virtual IEnumerator EndRoundCoRoutine(string wwiseTeamNameEvent, Team winningTeam)
    {
        GameController.Instance.ChangeGameStartedState(false);
        OrbController.Instance.gameObject.SetActive(false);
        RoundUIController.GetTeamContainer(winningTeam).SetActive(true);
        WwiseManager.PostEvent(wwiseTeamNameEvent, gameObject);
        yield return new WaitForSeconds(3f);
        RoundUIController.GetTeamContainer(winningTeam).SetActive(false);
        BeginNextRound();

    }
}
