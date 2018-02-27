using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TeamController : MonoBehaviour
{
    public static TeamController Instance { private set; get; }

    public  List<Team> TeamList { private set; get; }

    private int NbPlayersCreated;

    public GameObject BellRoot;
    public GameObject PlayerRoot;


    public enum TeamID
    {
        TeamBlue,
		TeamRed,
        Neutral
    };

    void Awake()
    {
        Instance = this;
        RoundUIController.StartCinematicEvent += GenerateNewTeams; 
        GameController.SetNextRoundEvent += ResetCurrentTeams;
        GameController.EndGameEvent += () => TeamList.Clear();
    }

	public Team GetTeamByID(TeamID teamID)
	{
		Team team = TeamList.Find(t => t.TeamID == teamID);
		return team;
	}

    public Team GetOtherTeam(Team currentTeam)
    {
//        switch (currentTeam.TeamID)
//        {
//		case TeamID.TeamBlue:
//            return TeamList[1];
//		case TeamID.TeamRed:
//            return TeamList[0];
//        default:
//            return currentTeam;
//        }
		switch (currentTeam.TeamID)
		{
		case TeamID.TeamBlue:
			return GetTeamByID(TeamID.TeamRed);
		case TeamID.TeamRed:
			return GetTeamByID(TeamID.TeamBlue);
		default:
			return currentTeam;
		}
	}
	
	
	private void ResetCurrentTeams()
    {
        for (int i = 0; i < TeamList.Count; i++)
        {
            for (int j = 0; j < TeamList[i].PlayerList.Count; j++)
            {
                TeamList[i].PlayerList[j].ResetProperties();
                TeamList[i].Reset();
            }
        }
    }

    void GenerateNewTeams()
    {
        TeamList = new List<Team>();
        NbPlayersCreated = 0;

        for (int i = 0; i < GameController.CurrentGameMode.NbTeams; i++)
        {
            TeamList.Add(GenerateTeam((TeamID)(i), GameController.CurrentGameMode.NbPlayers, i)); 
        }
    }


    Team GenerateTeam(TeamID id, int nbPlayers, int teamNb)
    {
        Team newTeam = new Team(id, nbPlayers, teamNb);

        for (int i = 0; i < nbPlayers; i++)
        {
            newTeam.PlayerList.Add(GeneratePlayer(i, id));
        }

        AssignBell(newTeam);
        AssignScoreTxt(newTeam);
        newTeam.Reset();
     
        return newTeam;
    }

    PlayerController GeneratePlayer(int playerID, TeamID assignedTeamID)
    {
        PlayerController pController = Instance.PlayerRoot.transform.GetChild(NbPlayersCreated).GetComponent<PlayerController>();
        pController.Player = new PlayerScript(NbPlayersCreated, assignedTeamID, pController);
        pController.ResetProperties();
        NbPlayersCreated++;
        return pController;
    }

    void AssignBell(Team assignedTeam)
    {
        Bell currentBell = Instance.BellRoot.transform.GetChild((int)assignedTeam.TeamID).GetComponent<Bell>();
        currentBell.AssignTeam(assignedTeam);
        assignedTeam.AssignBell(currentBell);
    }

    void AssignScoreTxt(Team assignedTeam)
    {
        int teamId = (int)assignedTeam.TeamID;
        assignedTeam.AssignScoreText(RoundUIController.Instance.roundScoreList[teamId], RoundUIController.Instance.totalScoreList[teamId]);
    }
}

