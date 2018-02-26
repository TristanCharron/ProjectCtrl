using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Team
{
    public TeamController.TeamID TeamID { private set; get; }


    public Text RoundScoretxt,TotalScoreTxt;

    public int CurrentScore { private set; get; }
    public int TotalScore { private set; get; }
    public int NbPlayersActive { private set; get; }

    public List<PlayerController> PlayerList { private set; get; }
    public List<PlayerController> PlayerListDead { protected set; get; }

    public Bell TeamBell;

    public int Index { private set; get; }


    public Team()
    {
        TeamID = TeamController.TeamID.TeamRed; //why tho
        NbPlayersActive = GameController.CurrentGameMode.NbPlayers;
        PlayerList = new List<PlayerController>();
    }

    public Team(TeamController.TeamID _team, int _nbplayers,int _index)
    {
        TeamID = _team;
        NbPlayersActive = _nbplayers;
        PlayerList = new List<PlayerController>();
        Index = _index;
        CurrentScore = 0;
        TotalScore = 0;
    }

    public void Reset()
    {
        PlayerListDead = new List<PlayerController>();
    }

 
    public void AssignScoreText(Text roundtxt, Text totalTxt)
    {
        RoundScoretxt = roundtxt;
        TotalScoreTxt = totalTxt;
    }

    public void AssignBell(Bell bell)
    {
        TeamBell = bell;
    }

    public void AddHitScore(int pointsAmount)
    {
        CurrentScore += pointsAmount;
        ScoreController.AddScore(RoundScoretxt, CurrentScore);
       
    }

    public void AddRoundScore(int pointsAmount)
    {
        TotalScore += CurrentScore;
        CurrentScore = 0;
        ScoreController.AddScore(RoundScoretxt, 0);
        ScoreController.AddScore(TotalScoreTxt, TotalScore);
    }

    public void KillPlayer(PlayerController player, out bool isTeamDead)
    {
        PlayerListDead.Add(player);
        isTeamDead = PlayerListDead.Count >= GameController.CurrentGameMode.NbPlayers;
    }


}
