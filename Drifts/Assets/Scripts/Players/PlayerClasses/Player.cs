using UnityEngine;
using System.Collections;

public class Player  {

    protected float power;
    public float Power { get { return power; } }

    protected float speed;
    public float Speed { get { return speed; } }

    public Player()
    {
        power = 1;
        speed = 30;
    }
 
}

public class Sumo : Player
{

    public Sumo()
    {
        power = 3;
        speed = 30;
    }

}
