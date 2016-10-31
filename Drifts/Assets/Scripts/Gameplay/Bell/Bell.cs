using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class Bell : MonoBehaviour
{

    private int curNbBellHits = 0;
    private Team assignedTeam;
    private bool isActive = true;
    private static float bellLength = 2f;


    void OnTriggerEnter(Collider other)
    {
        if (isActive)
        {
            if (other.gameObject.CompareTag("Orb"))
            {
                onBellHit();

                if (shouldEnableTeamPower())
                    onEnableTeamPower(true);

                OrbController.onPush(transform.right,OrbController.MaxVelocity / 3);
                onDisableBell();

            }
        }


    }

    void onDisableBell()
    {
        isActive = false;
        Invoke("onEnableBell", bellLength);
    }

    void onEnableBell()
    {
        isActive = true;
    }


    bool shouldEnableTeamPower()
    {
        return curNbBellHits >= GameController.nbBellHits;
    }

    void onEnableTeamPower(bool state)
    {
        switch (assignedTeam.powerID)
        {
            case TeamController.powerID.stunt:
                onEnableStuntPower(state);
                break;
            default:
                onEnableStuntPower(state);
                break;
        }

        onResetBell();
    }



    void onEnableStuntPower(bool state)
    {
        if (state)
        {
            UIEffectManager.OnFreezeFrame(OrbController.velocityRatio / 3);
            Invoke("onDisableStuntPower", 2f);
        }

    }

    void onDisableStuntPower()
    {
        onEnableStuntPower(false);
    }

    public void onAssignTeam(Team newTeam)
    {
        assignedTeam = newTeam;

    }

    void onResetBell()
    {
        curNbBellHits = 0;
        isActive = true;

    }

    public void onBellHit()
    {

        onCheckBellHit();

    }

    private void onCheckBellHit()
    {
        //No Team, invalid
        if (OrbController.PossessedTeam == TeamController.teamID.Neutral || assignedTeam == null)
            return;


        onAddScore(TeamController.onReturnOtherTeam(assignedTeam));
        onPlayBellSound();


    }

    private void onAddScore(Team team)
    {

        curNbBellHits++;
        team.onAddHitScore((int)OrbController.CurrentVelocity / 3);
    }

    private void onPlayBellSound()
    {
        WwiseManager.onPlayWWiseEvent("STAGE_BELL", gameObject);
        GetComponent<Animator>().Play("DONG", 0, -1);
    }

}
