using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TeamController : MonoBehaviour
{
    public const int nbPlayers = 2, nbTeams = 2;
    public static int nbPlayerCreated = 0;

    private static TeamController instance;
    public static TeamController Instance { get { return instance; } }

    public List<Team> _teamList;
    static List<Team> teamList;
    public static List<Team> TeamList { get { return teamList; } }


    public GameObject BellRoot;
    public GameObject PlayerRoot;


    public enum teamID
    {
        Neutral,
        Team1,
        Team2
    };

    public enum bellID
    {

    };

    public enum powerID
    {
        stunt,
        superpower,
        croustibat,
    }

    public static void OnComplete()
    {
        teamList.Clear();
        teamList = null;
        nbPlayerCreated = 0;
    }

    public static Team onReturnOtherTeam(Team currentTeam)
    {
       switch(currentTeam.Index)
        {
            case 1:
                return teamList[1]; 
            case 2:
                return teamList[0];
            default:
                return currentTeam;
        }
    }

    // Use this for initialization
    void Awake()
    {
        instance = this;
        instance._teamList = teamList;
        GameController.SetNextRound += onReset;
    }

    public static void onReset()
    {
        onGenerateTeams();
    }

    static void onGenerateTeams()
    {
        if (teamList == null)
            onGenerateNewTeams();
        else
            onResetTeams();
    }

    

    static void onGenerateNewTeams()
    {
        nbPlayerCreated = 0;
        teamList = new List<Team>();
        for (int i = 0; i < nbTeams; i++)
        {
            onGenerateTeam((teamID)(i + 1), powerID.stunt, nbPlayers, i+1);
        }
    }

    static void onResetTeams()
    {
        for(int i = 0; i < teamList.Count; i++) {
            onResetPlayers(teamList[i]);
        }
    }

    static void onResetPlayers(Team team)
    {
        for (int i = 0; i < nbPlayers; i++)
        {
            team.PlayerList[i].OnResetProperties();
        }

        RoundController.onResetPosition();
    }

    static void onGenerateTeam(teamID id, powerID power, int nbPlayers, int teamNb)
    {
       Team newTeam = new Team(id, power, nbPlayers, teamNb);

      
        for(int i = 0; i < nbPlayers;i++)
        {
            OnGeneratePlayer(nbPlayerCreated,newTeam);
        }

        OnAssignBell(newTeam, teamNb-1);
        OnAssignScoreTxt(newTeam, teamNb-1);
        teamList.Add(newTeam);

    }

    static void OnGeneratePlayer(int playerID, Team assignedTeam)
    {
        PlayerController pController = instance.PlayerRoot.transform.GetChild(playerID).GetComponent<PlayerController>();
        pController.player = new Player(playerID + 1, assignedTeam, pController);
        pController.OnResetProperties();
        pController.onAssignTeam(assignedTeam);
        assignedTeam.PlayerList.Add(pController);
        nbPlayerCreated++;

    }

 


    static void OnAssignBell(Team assignedTeam, int bellID)
    {
        Bell currentBell = instance.BellRoot.transform.GetChild(instance.BellRoot.transform.childCount - (bellID + 1)).GetComponent<Bell>();
        currentBell.onAssignTeam(assignedTeam);
        assignedTeam.onAssignBell(currentBell);
    }

    static void OnAssignScoreTxt(Team assignedTeam, int teamID)
    {
        assignedTeam.onAssignScoreTxt(RoundUIController.Instance.roundScoreList[teamID], RoundUIController.Instance.totalScoreList[teamID]);
    }


}

