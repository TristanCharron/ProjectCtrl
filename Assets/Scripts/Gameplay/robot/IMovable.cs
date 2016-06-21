using UnityEngine;
using System.Collections;
using System;

public class Movable : IMovable  {

    protected Character origin;
    protected float speed;
    protected bool canMove, moving;


    public Movable(Character _origin)
    {
        origin = _origin;
        speed = origin.Speed ;
        canMove = true;
        moving = false;
    }

    public bool CanMove
    {
        get
        {
            return canMove;
        }
    }

    public bool Moving
    {
        get
        {
            return moving;
        }
    }

    public float Speed
    {
        get
        {
            return speed;
        }
    }

    public void onMove(GameObject origin)
    {
        if(canMove)
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            origin.transform.position += new Vector3(move.x, move.y, move.z) * speed * Time.deltaTime;
        }

    }

    public void isAbleToMove(bool state)
    {
        canMove = state;
    }
}



public interface IMovable
{

    void onMove(GameObject origin);
    void isAbleToMove(bool state);
    float Speed
    {
        get;
    }
    bool CanMove { get; }
    bool Moving { get; }

}
