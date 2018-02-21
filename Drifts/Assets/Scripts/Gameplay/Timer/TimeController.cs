using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeController : MonoBehaviour
{

    public static TimeController Instance { private set; get; }
  
    public const float roundLength = 60f;
    public static float Timer { get; private set; }

    public Text TimerUI;

    public static int TimeRounded { get { return (int)Timer; } }

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        Timer = roundLength;
        SetText((int)Timer);


    }

    void Start()
    {
        GameController.SetNextRoundEvent += Reset;
    }

    void Update()
    {
        if (GameController.IsGameStarted)
            UpdateTime();


    }

    void UpdateTime()
    {
        Timer -= Time.deltaTime;
        SetText(TimeRounded);
        if (Timer < 0)
            EndTimer();
    }



    public static void Reset()
    {
        Timer = roundLength;
        SetText((int)Timer);
    }

    static void SetText(int seconds)
    {
        if (seconds > 59)
            Instance.TimerUI.text = "01:00";
        else if (seconds < 10)
            Instance.TimerUI.text = "00:0" + TimeRounded.ToString();
        else
            Instance.TimerUI.text = "00:" + TimeRounded.ToString();

    }

  

    void EndTimer()
    {
        /*
        if (TeamController.TeamList[1].CurrentScore > TeamController.TeamList[0].CurrentScore)
            GameModeController.EndRound(TeamController.TeamList[1]);
        else
            GameModeController.EndRound(TeamController.TeamList[0]);
        */
    }

}
