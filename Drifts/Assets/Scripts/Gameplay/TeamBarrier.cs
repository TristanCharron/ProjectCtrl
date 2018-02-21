using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TeamBarrier : MonoBehaviour
{
    public int teamID;

    public bool InTeamZone { private set; get; } 

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
        InTeamZone = false;
        OnDisableBarrier();
    }



    void OnTriggerEnter(Collider other)
    {
        if (GameController.IsGameStarted)
            if (other.gameObject == OrbController.Instance.gameObject && !InTeamZone)
            {
				OrbController.Instance.EnableOrb();
                OnEnableBarrier();
            }


    }

    void OnTriggerExit(Collider other)
    {
        if (GameController.IsGameStarted)
        {
            if (other.gameObject == OrbController.Instance.gameObject && InTeamZone)
            {
                OnDisableBarrier();
				OrbController.Instance.DisableOrb();
            }

        }

    }

    void OnTriggerStay(Collider other)
    {
        if (GameController.IsGameStarted)
            if (other.gameObject == OrbController.Instance.gameObject && !InTeamZone)
                OnEnableBarrier();
    }

    void OnEnableBarrier()
    {
        PenaltyController.EnableTimer(penaltyUI, this);
        InTeamZone = true;
        penaltyUI.CrossFadeAlpha(1, 0.5f, false);
    }
    public void OnDisableBarrier()
    {
        InTeamZone = false;
        onHidePenaltyText();
    }

    public void onHidePenaltyText()
    {
        penaltyUI.CrossFadeAlpha(0.001f, 0.5f, false);
    }
}
