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

    private static List<Player> robots;

    public static List<Player> Robots { get { return robots; } }

    public robotManager()
    {
        onActivate();
    }


    void onActivate()
    {
        robots = new List<Player>();
        for (int i = 0; i <= nbRobots; i++)
        {
            robots.Add(onGenerateRobot(i));
        }

    }


    Player onGenerateRobot(int id)
    {
        switch (id)
        {
            case 1:
                return new robot1();
            case 2:
                return new robot2();

            case 3:
                return new robot3();
        }

        return null;
    }


}
