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

    [SerializeField]
    public List<Text> playerId = new List<Text>();
    [SerializeField]
    public List<Text> roundScoreList = new List<Text>();
    [SerializeField]
    public List<Text> totalScoreList = new List<Text>();
    [SerializeField]
    private Text NbRoundText;


    public GameObject SakuraParticles;


    public delegate void RoundDelegate();

    public static event RoundDelegate StartCinematicEvent, EndCinematicEvent;


    // Use this for initialization
    void Start()
    {
        instance = this;
        GameController.SetNextRoundEvent += ResetUI;
        FadeInGame();
        
    }

    void FadeInGame()
    {
        Animator CamAnim = GetComponent<Animator>();
        CamAnim.enabled = true;
        UIEffectManager.Instance.FadeToWhite(false);
    }


    public static GameObject GetTeamContainer(Team currentTeam)
    {
		Debug.Log(currentTeam.Index);
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

    public void StartCinematic()
    {
        StartCinematicEvent();
        StartCinematicEvent = null;
    }

    public void EndCinematic()
    {
        SakuraParticles.SetActive(true);
        CameraBoxFollower.enabled = true;

        if(EndCinematicEvent != null)
        EndCinematicEvent();

        EndCinematicEvent = null;

        GameController.Instance.StartGame();
    }

    public IEnumerator SetUIForNextRound()
    {
        SetGameOverScreen(false);
        SetNewRoundScreen(true);
        GameController.Instance.ChangeGameStartedState(false);
        scoreTeamContainer.SetActive(true);
        yield return new WaitForSeconds(3.2f);
        SetNewRoundScreen(false);

        GameController.Instance.ChangeGameStartedState(true);

       
        
        yield break;

    }

    public static void SetGameOverScreen(bool state)
    {
        instance.GameOverContainer.SetActive(state);

        if (!state)
        {
            instance.RedTeamWin.SetActive(false);
            instance.BlueTeamWin.SetActive(false);
        }
        
    }

    public static void SetNewRoundScreen(bool state)
    {
        if (state)
        {
            instance.readyAnimator.Play("readyAnim", -1, 0f);
        }
        

        instance.readyContainer.SetActive(state);

    }

    public static void ResetUI()
    {
        instance.StartCoroutine(instance.SetUIForNextRound());
        instance.NbRoundText.text = GameController.Instance.NbRoundsPlayed.ToString("00");
    }





}
