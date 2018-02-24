using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptionQuit : MenuOptionState
{
    protected MenuOptionQuit() : base()
    {
        Text = "QUIT GAME";
        EnumID = MenuOptionStateID.QUIT;
    }
}

