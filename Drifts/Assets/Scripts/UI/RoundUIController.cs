using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RoundUIController : MonoBehaviour
{

    public static RoundUIController Instance { get { return instance; } }
    private static RoundUIController instance;

    public GameObject readyContainer, GameOverContainer, BlueTeamWin, RedTeamWin;
    public Animator readyAnimator;

    public GameObject scoreTeamContainer;

    public CameraActionBoxFollower CameraBoxFollower;

    public List<Text> playerId = new List<Text>();
    public List<Text> roundScoreList = new List<Text>();
    public List<Text> totalScoreList = new List<Text>();

    public GameObject SakuraParticles;


    // Use this for initialization
    void Start()
    {
        instance = this;
        GameController.SetNextRound += OnReset;
        FadeInGame();
        
    }

    void FadeInGame()
    {
        Animator CamAnim = GetComponent<Animator>();
        CamAnim.enabled = true;
        UIEffectManager.Instance.FadeToWhite(false);
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

    public void EndCinematic()
    {
        SakuraParticles.SetActive(true);
        CameraBoxFollower.enabled = true;

        GameController.onNextRound();
    }

    public IEnumerator OnTransitionUItoNextRound()
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
        if (state)
        {
            instance.readyAnimator.Play("readyAnim", -1, 0f);
        }
        

        instance.readyContainer.SetActive(state);

    }

    public static void OnReset()
    {
        instance.StartCoroutine(instance.OnTransitionUItoNextRound());
    }





}
