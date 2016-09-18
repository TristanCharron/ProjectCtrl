using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

    public static UiManager Instance { get { return instance; } }
    private static UiManager instance;

    public GameObject titleContainer, startContainer, readyContainer, GameOverContainer, BlueTeamWin, RedTeamWin;


    public GameObject playerIDcontainer;

    public CameraActionBoxFollower CameraBoxFollower;

    public Animator FadeToWhite;

    public List<Text> playerId = new List<Text>();

    public Transform Everything;
    public GameObject Sakuras;


    // Use this for initialization
    void Start()
    {

        instance = this;
        OnResetProperties();
        WwiseManager.onPlayWWiseEvent("GAME_OPEN", gameObject);
    }

   


    public static void OnResetProperties()
    {
        UIEffectManager.OnResetProperties();
        instance.playerIDcontainer.SetActive(false);
    }


    public void EndCinematic()
    {
        Sakuras.SetActive(false);
        CameraBoxFollower.enabled = true;
        GameController.onSpawnOrb();
        StartCoroutine(OnBeginGame());
    }

    IEnumerator OnBeginGame()
    {
        
        readyContainer.SetActive(true);
        yield return new WaitForSeconds(3.2f);
        readyContainer.SetActive(false);
        GameController.onSetGameStartedState(true);
        yield break;

    }


   

    IEnumerator makeIDAppear()
    {
        yield return new WaitForSeconds(4f);
        playerIDcontainer.SetActive(true);

        foreach (Text g in playerId)
        {
            g.CrossFadeAlpha(1.0f, 1.0f, false);
            g.CrossFadeAlpha(0.0f, 5.0f, false);
        }

        yield break;
    }

    public void onStartGame()
    {
        Sakuras.SetActive(true);

        WwiseManager.onPlayWWiseEvent("UI_SELECT", gameObject);
        WwiseManager.onPlayWWiseEvent("GAME_PLAY", gameObject);
        gameObject.GetComponent<Animator>().enabled = true;
        FadeToWhite.enabled = true;
        StartCoroutine(makeIDAppear());
    }

    public static void onGameOverScreen(bool state)
    {
        instance.GameOverContainer.SetActive(state);
    }





}
