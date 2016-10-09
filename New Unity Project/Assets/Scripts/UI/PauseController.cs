using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    static Color highlightMenuColor = new Color(0.8f, 0.8f, 0.8f, 0.5f);
    public GameObject pauseCanvas;
    public Text[] pauseMenuTxtList;
    static bool isPauseEnabled = false, isChangingMenu = false;
    static int selectionIndex = 0;


    public static PauseController Instance
    {
        get
        {
            return instance;
        }
    }
    private static PauseController instance;

    // Use this for initialization
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChangingMenu && isPauseEnabled)
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
        OnPause();
        if (selectionIndex == 1)
            GameController.onGameOver();

       

    }

    static void OnChangeOption(int newIndex)
    {
        if (newIndex >= instance.pauseMenuTxtList.Length)
            selectionIndex = 0;
        else if (newIndex < 0)
            selectionIndex = instance.pauseMenuTxtList.Length - 1;
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

    public static void OnPause()
    {
        isPauseEnabled = !isPauseEnabled;
        Time.timeScale = isPauseEnabled ? 0 : 1;
        instance.pauseCanvas.SetActive(isPauseEnabled);
        

        if (isPauseEnabled)
        {
            selectionIndex = 0;
            onSetMenuOptionsColor();
        }
           

    }

    static void onSetMenuOptionsColor()
    {
        for (int i = 0; i < instance.pauseMenuTxtList.Length; i++)
        {
            Color destColor = i == selectionIndex ? Color.white : highlightMenuColor;
            instance.pauseMenuTxtList[i].CrossFadeColor(destColor, 0.3f, true, true);

        }

    }




}
