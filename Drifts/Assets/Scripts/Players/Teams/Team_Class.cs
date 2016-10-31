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
    private Text roundScoretxt,totalScoreTxt;
    public Text RoundScoreTxt { get { return roundScoretxt; } }
    public Text TotalScoreTxt { get { return totalScoreTxt; } }


    int nbPlayers, nbPlayersActive;
    int currentScore, totalScore;
    public int CurrentScore { get { return currentScore; } }
    public int TotalScore { get { return totalScore; } }
    public int NbPlayersActive { get { return nbPlayersActive; } }


    public List<PlayerController> playerList;
    public List<PlayerController> PlayerList { get { return playerList; } }

    public Bell teamBell;

    private bool stunt,winning;
    public bool isStunt { get { return stunt; } }
    public bool isWinning { get { return winning; } }

    int index;
    public int Index { get { return index; } }
    


    public Team()
    {
        team = TeamController.teamID.Team1;
        power = TeamController.powerID.stunt;
        nbPlayers = 2;
        nbPlayersActive = nbPlayers;
        stunt = false;
        playerList = new List<PlayerController>();
    }

    public Team(TeamController.teamID _team, TeamController.powerID _power, int _nbplayers,int _index)
    {
        team = _team;
        power = _power;
        nbPlayers = _nbplayers;
        nbPlayersActive = _nbplayers;
        stunt = false;
        playerList = new List<PlayerController>();
        index = _index;
        currentScore = 0;
        totalScore = 0;
    }

 
    public void onAssignScoreTxt(Text roundtxt, Text totalTxt)
    {
        roundScoretxt = roundtxt;
        totalScoreTxt = totalTxt;
    }

    public void onAssignBell(Bell bell)
    {
        teamBell = bell;
    }

    public void onSetWinningState(bool state)
    {
        winning = state;
    }

    public void onStunt(bool state)
    {
        if (stunt != true)
            stunt = state;
    }

    public void onAddHitScore(int pointsAmount)
    {
        currentScore += pointsAmount;
        ScoreController.onAddScore(roundScoretxt, currentScore);
       
    }

    public void onAddRoundScore(int pointsAmount)
    {
        if(isWinning)
            totalScore += currentScore;
        
        
        currentScore = 0;
        ScoreController.onAddScore(roundScoretxt, 0);
        ScoreController.onAddScore(totalScoreTxt, totalScore);
    }


}
