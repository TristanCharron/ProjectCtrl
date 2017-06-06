using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    static Color highlightMenuColor;
    public Text[] mainMenuTxtList;
    static bool isChangingMenu = false, isInMainMenu = true, isInLobbyMenu = false;
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
        if (!isChangingMenu && isInMainMenu)
        {
            for (int i = 0; i < mainMenuTxtList.Length; i++)
            {
                if (Input.GetAxis(InputController.VERTICAL_MOVE + (i + 1)) > 0.9f)
                    OnChangeOption(selectionIndex - 1);
                else if ((Input.GetAxis(InputController.VERTICAL_MOVE + (i + 1)) < -0.9f))
                    OnChangeOption(selectionIndex + 1);
                else if ((Input.GetButtonDown(InputController.PRESS_A + (i + 1))))
                    onPressMenuOption(selectionIndex);
            }
        }

		if (Input.GetMouseButton (0))
			SceneManager.LoadScene (1);

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

    static void onPressLobbyOption(int currentIndex)
    {
        if (selectionIndex == 0)
            instance.onStartGame();
        if (selectionIndex == 1)
            SceneManager.LoadScene(0);
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
        if(isInMainMenu)
        {
            isInMainMenu = false;
            
            WwiseManager.onPlayWWiseEvent("UI_SELECT", Camera.main.gameObject);
            UIEffectManager.OnFadeToWhite(true);
            Camera.main.gameObject.GetComponent<Animator>().enabled = true;
        }
        else if(isInLobbyMenu)
        {
            isInLobbyMenu = false;
            WwiseManager.onPlayWWiseEvent("UI_SELECT", Camera.main.gameObject);
          
            Camera.main.gameObject.GetComponent<Animator>().enabled = true;
        }
        
    }

    public void onEndFadeToWhite()
    {
        SceneManager.LoadScene(2);
        isInLobbyMenu = true;
        selectionIndex = 0;
    }

    public void onEndSelectionType()
    {
      
        SceneManager.LoadScene(1);
    }




}
