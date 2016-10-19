﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RoundUIController : MonoBehaviour
{

    public static RoundUIController Instance { get { return instance; } }
    private static RoundUIController instance;

    public GameObject readyContainer, GameOverContainer, BlueTeamWin, RedTeamWin;

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
        SakuraParticles.SetActive(false);
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