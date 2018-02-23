using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameModeID
{
    DEATHMATCH = 0,
    SCOREMATCH = 1
}



public abstract class GameMode : MonoBehaviour
{


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
    }



    protected virtual void BeginNextRound()
    {
        EnablePlayers();
        GameController.NextRound();
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

        WwiseManager.PostEvent("MONK_DEAD", deadPlayer.gameObject);

        bool isTeamDead = false;

        TeamController.TeamList[(int)deadPlayer.CurrentTeamID].KillPlayer(deadPlayer, out isTeamDead);

        deadPlayer.gameObject.SetActive(false);

        if (isTeamDead)
        {
            Team winningTeam = TeamController.GetOtherTeam(TeamController.TeamList[(int)deadPlayer.CurrentTeamID]);
            EndRound(winningTeam);
        }
    }

    protected virtual void EndRound(Team team)
    {
        string wwiseEvent = team.Index == 1 ? "GAME_END_BLUE" : "GAME_END_RED";
        StartCoroutine(EndRoundCoRoutine(wwiseEvent, team));
    }


    protected virtual IEnumerator EndRoundCoRoutine(string wwiseTeamNameEvent, Team winningTeam)
    {
        GameController.ChangeGameStartedState(false);
        OrbController.Instance.gameObject.SetActive(false);
        RoundUIController.GetTeamContainer(winningTeam).SetActive(true);
        WwiseManager.PostEvent(wwiseTeamNameEvent, gameObject);
        yield return new WaitForSeconds(3f);
        RoundUIController.GetTeamContainer(winningTeam).SetActive(false);
        BeginNextRound();

    }
}
