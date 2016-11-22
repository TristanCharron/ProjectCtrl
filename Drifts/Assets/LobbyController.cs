using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{

    static Color highlightMenuColor;
    public Text[] mainMenuTxtList;
    public GameObject selectablesObject, typesObject, optionsObject, playersObject,ReadyBtn;
    static bool isChangingMenu = false, isInLobbyMenu = false;
    static int selectionIndex = 0;
    static int currentSelectionIndex = 0;
    static int nmbPlayers = 4;




    public static LobbyController Instance
    {
        get
        {
            return instance;
        }
    }
    private static LobbyController instance;

    // Use this for initialization
    void Awake()
    {
        instance = this;
        selectionIndex = 0;
        isChangingMenu = false;
        isInLobbyMenu = false;
        highlightMenuColor = new Color(0.8f, 0.8f, 0.8f, 0.5f);
        Cursor.visible = false;
      
    }


    void Start()
    {
        onSetMenuOptionsColor();
        WwiseManager.onPlayWWiseEvent("GAME_OPEN", gameObject);
    }



    // Update is called once per frame
    void Update()
    {
        if (!isChangingMenu && isInLobbyMenu)
        {
            for (int i = 0; i < nmbPlayers; i++)
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


        switch (selectionIndex)
        {
            case 0:
                SwitchToSelectMode();
                break;
            case 1:
                SwitchToPlayerJoin();
                break;
            case 2:
                SwitchToPlayerSelect();
                break;
            case 3:
                SwitchToPlayerSelect();
                break;



        }

    }

    static void SwitchToPlayerJoin()
    {
        selectionIndex = 4;
        instance.selectablesObject.SetActive(false);
        instance.optionsObject.SetActive(true);
        LobbyPlayer.ChangeLobbyState(false);
    }

    static void SwitchToSelectMode()
    {
        selectionIndex = 2;
        currentSelectionIndex = 1;
        instance.selectablesObject.SetActive(false);
        instance.typesObject.SetActive(true);
        LobbyPlayer.ChangeLobbyState(false);
    }

    static void SwitchToPlayerSelect()
    {
        selectionIndex = 4;
        instance.selectablesObject.SetActive(false);
        instance.optionsObject.SetActive(false);
        instance.playersObject.SetActive(true);
        LobbyPlayer.ChangeLobbyState(true);
    }

    static void onPressLobbyOption(int currentIndex)
    {
        if (selectionIndex == 0)
            onStartGame();
        if (selectionIndex == 1)
            SceneManager.LoadScene(0);
    }

    static void OnChangeOption(int newIndex)
    {
        if (newIndex >= instance.mainMenuTxtList.Length - 4 && currentSelectionIndex == 0)
            selectionIndex = 0;
        else if ((newIndex >= instance.mainMenuTxtList.Length - 2 && currentSelectionIndex == 1))
            selectionIndex = 2;
        else if (newIndex < 0 && currentSelectionIndex == 0)
            selectionIndex = instance.mainMenuTxtList.Length - 1;
        else if (newIndex < 0 && currentSelectionIndex == 1)
            selectionIndex = instance.mainMenuTxtList.Length - 3;
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
            Color destColor = i == selectionIndex ? Color.white : highlightMenuColor;
            instance.mainMenuTxtList[i].CrossFadeColor(destColor, 0.3f, true, true);

        }

    }

    public static void onStartGame()
    {
        if (isInLobbyMenu)
        {
            isInLobbyMenu = false;

            WwiseManager.onPlayWWiseEvent("UI_SELECT", Camera.main.gameObject);
            UIEffectManager.OnFadeToWhite(true);
            Camera.main.gameObject.GetComponent<Animator>().enabled = true;
            Camera.main.gameObject.GetComponent<Animator>().Rebind();
            Camera.main.gameObject.GetComponent<Animator>().Play(Animator.StringToHash("fadeOutLobby"));
        }


    }

    public static void OnReadyToBeginGame(bool isReady)
    {
        instance.ReadyBtn.SetActive(isReady);
    }

    public void onEndFadeToWhite()
    {

        isInLobbyMenu = true;
        selectionIndex = 0;
    }

    public void onEndSelectionType()
    {

        SceneManager.LoadScene(1);
    }

    public void StopAnimator()
    {
        Camera.main.gameObject.GetComponent<Animator>().Stop();
    }
}
