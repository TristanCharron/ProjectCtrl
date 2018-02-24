using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptionDeathMatch : MenuOptionState
{
    protected MenuOptionDeathMatch() : base()
    {
        Text = "PLAY DEATHMATCH MODE";
        EnumID = MenuOptionStateID.PLAYDEATHMATCH;
    } 
}
