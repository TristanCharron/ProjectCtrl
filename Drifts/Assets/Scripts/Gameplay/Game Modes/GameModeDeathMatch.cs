using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeDeathMatch : GameMode {

    public GameModeDeathMatch() : base()
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
        base.KillPlayer(deadPlayer);
    }

    
    protected override void BeginNextRound()
    {
        base.BeginNextRound();
    }

	public override void BeginNextRoundExtra()
	{
		Debug.Log("death match test");
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
