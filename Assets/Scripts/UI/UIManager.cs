using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public PlayerUIManager[] _uiManagers;
    public static PlayerUIManager[] uiManagers;
    public static UIManager Instance;

    void Awake()
    {
        Instance = this;
        uiManagers = _uiManagers;
    }


    // Update is called once per frame
    void Update()
    {
        if (uiManagers != null)
        {
            foreach(PlayerUIManager pUI in uiManagers)
            {  
                    pUI.onUpdate();
            }
                   
        }
            
    }

    public static void onActivate(int index, playerManager player)
    {
        uiManagers[index].onActivate(player);
    }


    public static Color returnNewAlpha(Graphic graphic, float alpha)
    {
        return new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
    }

    public static void onHealthChange(int index)
    {
        if (index < uiManagers.Length)
        {
            uiManagers[index].onHealthChange();
        }
    }

    public static void onUltimateChange(int index)
    {
        if (index < uiManagers.Length)
        {
            uiManagers[index].onUltimateChange();
        }
    }




}
