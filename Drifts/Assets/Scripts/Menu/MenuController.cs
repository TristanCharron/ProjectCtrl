using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;
using System;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private Color highlightMenuColor;

    [SerializeField]
    private MenuOptionState[] MenuStates;

    private Dictionary<MenuOptionStateID, MenuOptionState> MenuFSM;

    private MenuOptionState CurrentMenuOptionState;

    static bool isChangingMenu = false, isInMainMenu = true;

    static int selectionIndex = 0;

    public static MenuController Instance { private set; get; }

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        selectionIndex = 0;
        isInMainMenu = false;
        isChangingMenu = false;
        Cursor.visible = false;
    }

    void Start()
    {
       
        AddMenuState(ref MenuStates[0], () => StartGame());
        AddMenuState(ref MenuStates[1], () => StartGame());
        AddMenuState(ref MenuStates[2], () => Application.Quit());
        StartCoroutine(ChangeMenuState(MenuOptionStateID.PLAYDEATHMATCH));
        WwiseManager.PostEvent("GAME_OPEN", gameObject);
        isInMainMenu = true;
    }



    // Update is called once per frame
    void Update()
    {


        if (!isChangingMenu && isInMainMenu)
        {
            for (int i = 0; i < 4; i++)
            {
                if (ReInput.players.GetPlayer(i).GetButtonDown("ConfirmUI"))
                    CurrentMenuOptionState.Select();

                if (ReInput.players.GetPlayer(i).GetAxis("Move Vertical") > 0.9f)
                    ChangeMenuState(--selectionIndex);
                else if (ReInput.players.GetPlayer(i).GetAxis("Move Vertical") < -0.9f)
                    ChangeMenuState(++selectionIndex);
            }


        }



    }

    private void AddMenuState(ref MenuOptionState menuOptionState, Action Select)
    {
        MenuOptionStateID currentMenuID = menuOptionState.EnumID;

        if (MenuFSM == null)
            MenuFSM = new Dictionary<MenuOptionStateID, MenuOptionState>();

        menuOptionState.Select = Select;

        if (!MenuFSM.ContainsKey(currentMenuID))
        {
            MenuFSM.Add(menuOptionState.EnumID, menuOptionState);
        }
        else
        {
            Debug.LogError("State " + currentMenuID.ToString() + " already exists in FSM. ");
        }

    }


    void ChangeMenuState(int newIndex)
    {
        if (newIndex < 0)
            newIndex = MenuStates.Length - 1;

        if (newIndex > MenuStates.Length - 1)
            newIndex = 0;

        selectionIndex = newIndex;
        isChangingMenu = true;
        StartCoroutine(ChangeMenuState(MenuStates[selectionIndex].EnumID));
        WwiseManager.PostEvent("UI_HOVER", Camera.main.gameObject);
    }


    private IEnumerator ChangeMenuState(MenuOptionStateID nextMenuID)
    {
        if (!MenuFSM.ContainsKey(nextMenuID))
        {
            Debug.LogError("State " + nextMenuID.ToString() + " not found in FSM. ");
        }
        else
        {
            if (CurrentMenuOptionState != null)
            {
                yield return CurrentMenuOptionState.ExitState();
            }

            CurrentMenuOptionState = MenuFSM[nextMenuID];
            isChangingMenu = true;
            yield return CurrentMenuOptionState.EnterState();
            isChangingMenu = false;
            yield break;
        }

    }

    void StartGame()
    {
        if (isInMainMenu)
        {
            isInMainMenu = false;
            WwiseManager.PostEvent("UI_SELECT", Camera.main.gameObject);
            UIEffectManager.OnFadeToWhite(true);
            Camera.main.gameObject.GetComponent<Animator>().enabled = true;
        }


    }

    void StartGameCallback()
    {
        SceneManager.LoadScene(1);
    }




}
