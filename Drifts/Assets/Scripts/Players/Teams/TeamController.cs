using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TeamController : MonoBehaviour
{
    public static TeamController Instance { private set; get; }

    public static List<Team> TeamList { private set; get; }

    private static int NbPlayersCreated;


    public GameObject BellRoot;
    public GameObject PlayerRoot;


    public enum TeamID
    {
        Team1 = 0,
        Team2 = 1,
        Neutral = 2,
       
    };


    // Use this for initialization
    void Awake()
    {
        Instance = this;
        RoundUIController.StartCinematicEvent += GenerateNewTeams; 
        GameController.SetNextRoundEvent += ResetCurrentTeams;
        GameController.EndGameEvent += () => TeamList.Clear();
    }


    public static Team GetOtherTeam(Team currentTeam)
    {
        switch (currentTeam.Index)
        {
            case 1:
                return TeamList[1];
            case 2:
                return TeamList[0];
            default:
                return currentTeam;
        }
    }


    private static void ResetCurrentTeams()
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



    static void GenerateNewTeams()
    {
      
        TeamList = new List<Team>();
        NbPlayersCreated = 0;

        for (int i = 0; i < GameController.CurrentGameMode.NbTeams; i++)
        {
            TeamList.Add(GenerateTeam((TeamID)(i), GameController.CurrentGameMode.NbPlayers, i)); 
        }

    
    }


    static Team GenerateTeam(TeamID id, int nbPlayers, int teamNb)
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

    static PlayerController GeneratePlayer(int playerID, TeamID assignedTeamID)
    {
        PlayerController pController = Instance.PlayerRoot.transform.GetChild(NbPlayersCreated).GetComponent<PlayerController>();
        pController.Player = new PlayerScript(NbPlayersCreated, assignedTeamID, pController);
        pController.ResetProperties();
        NbPlayersCreated++;
        return pController;
    }


    static void AssignBell(Team assignedTeam)
    {
        Bell currentBell = Instance.BellRoot.transform.GetChild((int)assignedTeam.TeamID).GetComponent<Bell>();
        currentBell.AssignTeam(assignedTeam);
        assignedTeam.AssignBell(currentBell);
    }

    static void AssignScoreTxt(Team assignedTeam)
    {
        int teamId = (int)assignedTeam.TeamID;
        assignedTeam.AssignScoreText(RoundUIController.Instance.roundScoreList[teamId], RoundUIController.Instance.totalScoreList[teamId]);
    }


}

