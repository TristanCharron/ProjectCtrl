using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeBell : GameMode {

	[SerializeField] float respawnDelayTime = 5;

	public GameModeBell() : base()
    {
        NbRounds = 3;
        NbPlayers = 2;
        NbTeams = 2;
    }


    public override void EnablePlayers()
    {
        base.EnablePlayers();
    }

    public override void KillPlayer(PlayerController deadPlayer)
    {
		StartCoroutine(RespawnPlayerDelay(deadPlayer));
    }

	IEnumerator RespawnPlayerDelay(PlayerController deadPlayer)
	{
		yield return new WaitForSeconds(respawnDelayTime);
		RespawnPlayer(deadPlayer);
	}

	void RespawnPlayer(PlayerController deadPlayer)
	{
		deadPlayer.gameObject.SetActive(true);
		deadPlayer.ResetProperties();
	}

    protected override void BeginNextRound()
    {
        base.BeginNextRound();
    }

	public override void BeginNextRoundExtra()
	{
		Debug.Log("reset bells");
		ResetBellsLife();
	}

	private void ResetBellsLife()
	{
		TeamController.Instance.TeamList[0].TeamBell.ResetLife();
		TeamController.Instance.TeamList[1].TeamBell.ResetLife();
	}

	public override void EndRound(Team team)
    {
        base.EndRound(team);
    }

    protected override IEnumerator EndRoundCoRoutine(string wwiseTeamNameEvent, Team winningTeam)
    {
        return base.EndRoundCoRoutine(wwiseTeamNameEvent, winningTeam);
    }
}
