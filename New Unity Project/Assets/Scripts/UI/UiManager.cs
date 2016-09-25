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

    public Animator FadeToWhite;

    public List<Text> playerId = new List<Text>();

    [SerializeField]
    public static List<Text> scoreId = new List<Text>();


    public List<Text> scoreIDInsp = new List<Text>();

    public Transform Everything;
    public GameObject Sakuras;


    // Use this for initialization
    void Start()
    {

        instance = this;
        OnResetProperties();
        WwiseManager.onPlayWWiseEvent("GAME_OPEN", gameObject);
    }

    void Update()
    {
        scoreId = scoreIDInsp;
        /*
        for(int i = 0; i < playerId.Count; i++)
        {
            playerId[i].transform.position = SpawnManager.Players[i].transform.position;
            playerId[i].transform.Translate(new Vector3(0, -10f, 0));
        }
        */
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
        GameController.onSpawnOrb();
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
        gameObject.GetComponent<Animator>().enabled = true;
        FadeToWhite.enabled = true;
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
