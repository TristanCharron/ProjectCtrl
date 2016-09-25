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

   

    // Use this for initialization
    void Awake()
    {
        instance = this;
    }

    public static void onReset()
    {
        nbPlayerCreated = 0;
        onGenerateTeams();
        instance._teamList = teamList;
    }

    static void onGenerateTeams()
    {
        teamList = new List<Team>();
        for (int i = 0; i < nbTeams; i++)
        {
            onGenerateTeam((teamID)(i + 1), powerID.stunt, nbPlayers,i);
        }
    }

    static void onGenerateTeam(teamID id, powerID power, int nbPlayers, int teamNb)
    {
       Team newTeam = new Team(id, power, nbPlayers);

      
        for(int i = 0; i < nbPlayers;i++)
        {
            OnConfigurePlayer(nbPlayerCreated,newTeam);
        }
        OnAssignBell(newTeam, teamNb);
        OnAssignScoreTxt(newTeam, teamNb);
        teamList.Add(newTeam);

    }

    static void OnConfigurePlayer(int playerID, Team assignedTeam)
    {
        PlayerController player = instance.PlayerRoot.transform.GetChild(playerID).GetComponent<PlayerController>();
        player.onAssignTeam(assignedTeam);
        assignedTeam.PlayerList.Add(player);
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
        assignedTeam.onAssignScoreTxt(UiManager.scoreId[teamID]);
    }


}

