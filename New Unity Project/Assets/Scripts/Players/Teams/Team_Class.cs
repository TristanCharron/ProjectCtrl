using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Team
{
    private TeamController.teamID team;
    public TeamController.teamID TeamID { get { return team; } }

    private TeamController.powerID power;
    public TeamController.powerID powerID { get { return power; } }

    [SerializeField]
    private Text scoreTxt;
    public Text ScoreTxt { get { return scoreTxt; } }

    int nbPlayers, nbPlayersActive;
    int currentScore, totalScore;
    public int CurrentScore { get { return currentScore; } }
    public int TotalScore { get { return totalScore; } }
    public int NbPlayersActive { get { return nbPlayersActive; } }


    public List<PlayerController> playerList;
    public List<PlayerController> PlayerList { get { return playerList; } }

    public Bell teamBell;

    private bool stunt;
    public bool isStunt { get { return stunt; } }



    public Team()
    {
        team = TeamController.teamID.Team1;
        power = TeamController.powerID.stunt;
        nbPlayers = 2;
        nbPlayersActive = nbPlayers;
        stunt = false;
        playerList = new List<PlayerController>();
    }

    public Team(TeamController.teamID _team, TeamController.powerID _power, int _nbplayers)
    {
        team = _team;
        power = _power;
        nbPlayers = _nbplayers;
        nbPlayersActive = _nbplayers;
        stunt = false;
        playerList = new List<PlayerController>();
    }

    public void onAssignScoreTxt(Text txt)
    {
        scoreTxt = txt;
    }

    public void onAssignBell(Bell bell)
    {
        teamBell = bell;
    }

    public void onStunt(bool state)
    {
        if (stunt != true)
            stunt = state;
    }

    public void onAddScore(int pointsAmount)
    {
        currentScore += pointsAmount;
        ScoreManager.onAddScore(scoreTxt, currentScore);
       
    }

    public void onAddRoundScore(int pointsAmount)
    {
        totalScore += currentScore;
        currentScore = 0;
    }
}
