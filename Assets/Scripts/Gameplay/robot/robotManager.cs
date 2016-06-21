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

public class robot1 : Player
{
    public robot1()
    {
        maxHealth = health;
        speed = 10;
        regenRate = 0.2f;
        health = 71;
        pausing = false; firing = false;
        ultimate = new ultimate1();
        movable = new Movable(this);
        name = "Donkey Konga 3";
    }

    protected override void onCheckUltimate()
    {
        if (Input.GetMouseButtonDown(1) && ultimate.isAvailable)
        {
            weapon.onEnableUltimate();
            ultimate.onEnable();
        }
    }

    public override void onUpdate()
    {
        base.onUpdate();
        onCheckUltimate();
    }



}

public class robot2 : Player
{
    public robot2()
    {
        maxHealth = health;
        speed = 50;
        regenRate = 0.2f;
        health = 123;
        pausing = false; firing = false;
        ultimate = new ultimate1();
        movable = new Movable(this);
        name = "Vaporwave";
    }

    protected override void onCheckUltimate()
    {
        if (Input.GetMouseButtonDown(1) && ultimate.isAvailable)
        {

            weapon.onEnableUltimate();
            ultimate.onEnable();
        }
    }

    public override void onUpdate()
    {
        base.onUpdate();
        onCheckUltimate();
    }
}

public class robot3 : Player
{
    public robot3()
    {
        maxHealth = health;
        speed = 75;
        regenRate = 0.2f;
        health = 71;
        pausing = false; firing = false;
        ultimate = new ultimate1();
        movable = new Movable(this);
        name = "Jean-Daniel";
    }

    protected override void onCheckUltimate()
    {
        if (Input.GetMouseButtonDown(1) && ultimate.isAvailable)
        {
            weapon.onEnableUltimate();
            ultimate.onEnable();
        }
    }

    public override void onUpdate()
    {
        base.onUpdate();
        onCheckUltimate();
    }
}





