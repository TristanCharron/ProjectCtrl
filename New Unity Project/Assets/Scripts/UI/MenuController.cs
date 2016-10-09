﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MenuController : MonoBehaviour
{
    static Color highlightMenuColor;
    public Text[] mainMenuTxtList;
    static bool isChangingMenu = false, isInMainMenu = true;
    static int selectionIndex = 0;


    public static MenuController Instance
    {
        get
        {
            return instance;
        }
    }
    private static MenuController instance;

    // Use this for initialization
    void Awake()
    {
        instance = this;
        selectionIndex = 0;
        isInMainMenu = true;
        isChangingMenu = false;
        highlightMenuColor = new Color(0.8f, 0.8f, 0.8f, 0.5f);
        Cursor.visible = false;
       

    }
    void Start()
    {
        onSetMenuOptionsColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChangingMenu && isInMainMenu)
        {
            for (int i = 0; i < RoundController.Players.Length; i++)
            {
                if (Input.GetAxis(InputController.VERTICAL_MOVE + (i + 1)) > 0.9f)
                    OnChangeOption(selectionIndex - 1);
                else if ((Input.GetAxis(InputController.VERTICAL_MOVE + (i + 1)) < -0.9f))
                    OnChangeOption(selectionIndex + 1);
                else if ((Input.GetButtonDown(InputController.PRESS_A + (i + 1))))
                    onPressMenuOption(selectionIndex);
            }
        }

    }

    public void onAbleToChangeMenuOption()
    {
        isChangingMenu = false;
    }

    static void onPressMenuOption(int currentIndex)
    {
        if (selectionIndex == 0)
            instance.onStartGame();
        if (selectionIndex == 1)
            Application.Quit();
    }

    static void OnChangeOption(int newIndex)
    {
        if (newIndex >= instance.mainMenuTxtList.Length)
            selectionIndex = 0;
        else if (newIndex < 0)
            selectionIndex = instance.mainMenuTxtList.Length - 1;
        else
            selectionIndex = newIndex;

        isChangingMenu = true;
        instance.StartCoroutine(instance.onCooldownPauseButton());
        onSetMenuOptionsColor();
    }

    public IEnumerator onCooldownPauseButton()
    {
        isChangingMenu = true;
        yield return new WaitForSecondsRealtime(0.3f);
        isChangingMenu = false;
    }

   

    static void onSetMenuOptionsColor()
    {
        for (int i = 0; i < instance.mainMenuTxtList.Length; i++)
        {
            Color destColor = i == selectionIndex ?  Color.white : highlightMenuColor;
            instance.mainMenuTxtList[i].CrossFadeColor(destColor, 0.3f, true, true);

        }

    }

    public void onStartGame()
    {
        isInMainMenu = false;
        RoundUIController.Instance.SakuraParticles.SetActive(true);
        WwiseManager.onPlayWWiseEvent("UI_SELECT", Camera.main.gameObject);
        WwiseManager.onPlayWWiseEvent("GAME_PLAY", Camera.main.gameObject);
        UIEffectManager.OnFadeToWhite();
        Camera.main.gameObject.GetComponent<Animator>().enabled = true;
    }




}
