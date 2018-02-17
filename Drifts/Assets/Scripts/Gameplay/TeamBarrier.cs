using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TeamBarrier : MonoBehaviour
{
    public int teamID;
    private bool inTeamZone = false;
    public bool isInTeamZone { get { return inTeamZone; } }

    public Text penaltyUI;

    public enum penaltyDirection
    {
        up = 1,
        down = 2,
        left = 3,
        right = 4,
    };

    public penaltyDirection direction;


    // Use this for initialization
    void Start()
    {
        OnDisableBarrier();
    }



    void OnTriggerEnter(Collider other)
    {
        if (GameController.isGameStarted)
            if (other.gameObject == OrbController.Instance.gameObject && !inTeamZone)
            {
                OrbController.EnableOrb();
                OnEnableBarrier();
            }


    }

    void OnTriggerExit(Collider other)
    {
        if (GameController.isGameStarted)
        {
            if (other.gameObject == OrbController.Instance.gameObject && inTeamZone)
            {
                OnDisableBarrier();
                OrbController.DisableOrb();
            }

        }

    }

    void OnTriggerStay(Collider other)
    {
        if (GameController.isGameStarted)
            if (other.gameObject == OrbController.Instance.gameObject && !inTeamZone)
                OnEnableBarrier();
    }

    void OnEnableBarrier()
    {
        PenaltyController.OnEnableTimer(penaltyUI, this);
        inTeamZone = true;
        penaltyUI.CrossFadeAlpha(1, 0.5f, false);
    }
    public void OnDisableBarrier()
    {
        inTeamZone = false;
        onHidePenaltyText();
    }

    public void onHidePenaltyText()
    {
        penaltyUI.CrossFadeAlpha(0.001f, 0.5f, false);
    }
}
