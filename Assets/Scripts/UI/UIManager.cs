using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public Transform[] uiPositions;
    public static PlayerUIManager[] uiManagers;
    public static UIManager Instance;

    void Awake()
    {
        Instance = this;
        uiManagers = new PlayerUIManager[combatManager.nbPlayers];
    }



    public static void onActivate(int index, playerManager player)
    {
        GameObject ui = Instantiate(Resources.Load("UI/playerUI"),Instance.uiPositions[index].position, Quaternion.identity) as GameObject;
        ui.transform.SetParent(Instance.uiPositions[index].gameObject.transform);
        ui.transform.localScale = Vector3.one;
        uiManagers[index] = ui.GetComponent<PlayerUIManager>();
        uiManagers[index].onActivate(player);
    }

    public static void onDesactivate(int index)
    {
        uiManagers[index].onDesactivate();
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
