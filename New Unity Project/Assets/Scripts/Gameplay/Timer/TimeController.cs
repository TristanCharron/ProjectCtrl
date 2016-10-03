using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeController : MonoBehaviour {

    public static TimeController Instance
    {
        get
        {
            return instance;
        }
    }
    private static TimeController instance;



    public const float roundLength = 60f;
    private static float timer = roundLength;

    public Text TimerUI;
    public static float Timer { get { return timer; } }
    public static int TimeRounded { get { return (int)Timer; } }

    void Update()
    {
        if(GameController.isGameStarted)
        {
            timer -= Time.deltaTime;
            onSetText(TimeRounded);
            if (timer < 0)
            {
                onComplete();
            }
        }
        
    }

    public static void OnReset()
    {
        timer = roundLength;
        onSetText((int)timer);
    }

    static void onSetText(int seconds)
    {
        if(seconds > 59)
            instance.TimerUI.text = "01:00";
        else
            instance.TimerUI.text = "00:" + TimeRounded.ToString();
    }

    // Use this for initialization
    void Awake () {
        instance = this;
        timer = roundLength;
        onSetText((int)timer);
        

    }

    void onComplete()
    {
        if (TeamController.TeamList[1].CurrentScore > TeamController.TeamList[0].CurrentScore)
            RoundController.OnTeamVictory(TeamController.TeamList[1]);
        else
            RoundController.OnTeamVictory(TeamController.TeamList[0]);
    }
	
}
