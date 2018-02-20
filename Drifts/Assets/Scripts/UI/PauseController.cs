using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

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
            for (int i = 0; i < 4; i++)
            {
                if (ReInput.players.GetPlayer(i).GetButtonDown("ConfirmUI"))
                    PressMenuOption(selectionIndex);

                if (ReInput.players.GetPlayer(i).GetAxis("Move Vertical") > 0.9f)
                    ChangeOption(selectionIndex == 0 ? 1 : 0);
                else if (ReInput.players.GetPlayer(i).GetAxis("Move Vertical") < -0.9f)
                    ChangeOption(selectionIndex == 0 ? 1 : 0);
            }

            
        }

    }

    public void OnAbleToChangeMenuOption()
    {
        isChangingMenu = false;
    }

    void PressMenuOption(int currentIndex)
    {
        Pause();
        if (selectionIndex == 1)
            GameController.onGameOver();

       

    }

    void ChangeOption(int newIndex)
    {
        if (newIndex >= instance.pauseMenuTxtList.Length)
            selectionIndex = 0;
        else if (newIndex < 0)
            selectionIndex = instance.pauseMenuTxtList.Length - 1;
        else
            selectionIndex = newIndex;

        isChangingMenu = true;
        instance.StartCoroutine(instance.OnCooldownPauseButton());
        SetMenuOptionsColor();
    }

    public IEnumerator OnCooldownPauseButton()
    {
        isChangingMenu = true;
        yield return new WaitForSecondsRealtime(0.3f);
        isChangingMenu = false;
    }

    public void Pause()
    {
        isPauseEnabled = !isPauseEnabled;
        Time.timeScale = isPauseEnabled ? 0 : 1;
        instance.pauseCanvas.SetActive(isPauseEnabled);
        

        if (isPauseEnabled)
        {
            selectionIndex = 0;
            SetMenuOptionsColor();
        }
           

    }

    void SetMenuOptionsColor()
    {
        for (int i = 0; i < instance.pauseMenuTxtList.Length; i++)
        {
            Color destColor = i == selectionIndex ? Color.white : highlightMenuColor;
            instance.pauseMenuTxtList[i].CrossFadeColor(destColor, 0.3f, true, true);

        }

    }




}
