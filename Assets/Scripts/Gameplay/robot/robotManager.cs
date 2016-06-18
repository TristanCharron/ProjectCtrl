using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class robotManager
{
    private static robotManager instance;

    public static robotManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new robotManager();
            }
            return instance;
        }
    }
    public const int nbRobots = 4;

    //List of Robots avaiable in Database
    private static List<Player> database;
    public static List<Player> Database { get { return database; } }

    //List of selected Robots in-game
    private static Player[] selectedInGame;
    public static Player[] SelectedInGame { get { return selectedInGame; } }
    public static void onSetSelectRobot(int robotIndex, int playerIndex) { selectedInGame[playerIndex] = database[robotIndex]; }




    public robotManager()
    {
        onActivate();
    }


    void onActivate()
    {
        database = new List<Player>();
        selectedInGame = new Player[combatManager.nbPlayers];
        for (int i = 0; i <= nbRobots; i++)
        {
            database.Add(onGenerateRobot(i));
        }

    }


    Player onGenerateRobot(int id)
    {
        switch (id)
        {
            default:
                return new Player();
            case 1:
                return new robot1();
            case 2:
                return new robot2();

            case 3:
                return new robot3();
        }

    }


}
