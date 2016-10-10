using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PenaltyController : MonoBehaviour
{
    private static PenaltyController instance;
    public static PenaltyController Instance
    {
        get
        {
            return instance;
        }
    }

    private static Vector3 penaltyAngle
    {

        get
        {
            if (currentBarrier == null)
                return Vector3.zero;
            else
            {
                switch (currentBarrier.direction)
                {
                    case TeamBarrier.penaltyDirection.right:
                        return OrbController.Instance.gameObject.transform.right;
                    case TeamBarrier.penaltyDirection.up:
                        return OrbController.Instance.gameObject.transform.up;
                    case TeamBarrier.penaltyDirection.down:
                        return -OrbController.Instance.gameObject.transform.up;
                    case TeamBarrier.penaltyDirection.left:
                        return -OrbController.Instance.gameObject.transform.right;
                    default:
                        return Vector3.zero;
                }
            }
        }
    }

    private static TeamBarrier currentBarrier;


    public static Text currentPenaltyTxt;

    public const float penaltyLength = 4f;

    private static float penaltyTimer = penaltyLength;
    public static float PenaltyTimer { get { return penaltyTimer; } }

    private static bool isEnabled = false;
    public static bool IsEnabled { get { return isEnabled; } }


    // Use this for initialization
    void Awake()
    {
        instance = this;
    }

    public static void OnReset()
    {
        OnDisableTimer();
        foreach(TeamBarrier barrier in FindObjectsOfType<TeamBarrier>())
        {
            barrier.OnDisableBarrier();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
        {
            penaltyTimer -= Time.deltaTime;
            currentPenaltyTxt.text = "0" + penaltyTimer.ToString("N0");
            if (isPenaltyOver())
                OnApplyPenalty();
        }

    }

    static void OnApplyPenalty()
    {
        OrbController.onPush(penaltyAngle, TeamController.teamID.Neutral);
        OrbController.OnDisableOrb();
        currentBarrier.onHidePenaltyText();
        OnDisableTimer();

    }

    static void OnDisableTimer()
    {
        isEnabled = false;
        penaltyTimer = penaltyLength;

        if (currentBarrier != null)
        {
            currentBarrier.onHidePenaltyText();
        }


    }

    bool isPenaltyOver()
    {
        return penaltyTimer <= 0 && isEnabled;
    }

    public static void OnEnableTimer(Text txt, TeamBarrier barrier)
    {
        penaltyTimer = penaltyLength;
        currentPenaltyTxt = txt;
        currentBarrier = barrier;
        isEnabled = true;
    }




}
