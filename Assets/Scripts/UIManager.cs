using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static PlayerUIManager[] uiManagers;
    public static UIManager Instance;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        uiManagers = new PlayerUIManager[combatManager.nbPlayers];
    }

    // Update is called once per frame
    void Update()
    {
        if (uiManagers != null)
        {
            for (int i = 0; i < uiManagers.Length; i++)
                if(uiManagers[i] != null)
                    uiManagers[i].onUpdate();
        }
            
    }

    public static void onAddPlayer(int index, playerManager player)
    {
        if (uiManagers == null)
        {
            uiManagers = new PlayerUIManager[combatManager.nbPlayers];
        }
        uiManagers[index] = player.PlayerUIManager;
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
