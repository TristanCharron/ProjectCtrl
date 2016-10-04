using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

    public static UiManager Instance { get { return instance; } }
    private static UiManager instance;

    public GameObject titleContainer, startContainer, readyContainer, GameOverContainer, BlueTeamWin, RedTeamWin;


    public GameObject scoreTeamContainer;

    public CameraActionBoxFollower CameraBoxFollower;

    public List<Text> playerId = new List<Text>();

    public List<Text> roundScoreList = new List<Text>();
    public List<Text> totalScoreList = new List<Text>();

    public Transform Everything;
    public GameObject Sakuras;


    // Use this for initialization
    void Start()
    {

        instance = this;
        OnResetProperties();
        WwiseManager.onPlayWWiseEvent("GAME_OPEN", gameObject);
    }


    public static GameObject OnGetTeamContainer(Team currentTeam)
    {
        switch (currentTeam.Index)
        {
            case 1:
                return instance.BlueTeamWin;
            case 2:
                return instance.RedTeamWin;
            default:
                return instance.BlueTeamWin;
        }
    }

    public static void OnResetProperties()
    {
        UIEffectManager.OnResetProperties();
        instance.scoreTeamContainer.SetActive(false);
    }


    public void EndCinematic()
    {
        Sakuras.SetActive(false);
        CameraBoxFollower.enabled = true;
        GameController.onNextRound();
        StartCoroutine(OnBeginGame());
    }

    public IEnumerator OnBeginGame()
    {
        onGameOverScreen(false);
        onNewRoundScreen(true);
        GameController.onSetGameStartedState(false);
        scoreTeamContainer.SetActive(true);
        yield return new WaitForSeconds(3.2f);
        onNewRoundScreen(false);
        GameController.onSetGameStartedState(true);
        yield break;

    }



    public void onStartGame()
    {
        Sakuras.SetActive(true);
        WwiseManager.onPlayWWiseEvent("UI_SELECT", gameObject);
        WwiseManager.onPlayWWiseEvent("GAME_PLAY", gameObject);
        UIEffectManager.OnFadeToWhite();
        Camera.main.gameObject.GetComponent<Animator>().enabled = true;
    }

    public static void onGameOverScreen(bool state)
    {
        instance.GameOverContainer.SetActive(state);

        if (!state)
        {
            instance.RedTeamWin.SetActive(false);
            instance.BlueTeamWin.SetActive(false);
        }
        
    }

    public static void onNewRoundScreen(bool state)
    {
        instance.readyContainer.SetActive(state);
    }





}
