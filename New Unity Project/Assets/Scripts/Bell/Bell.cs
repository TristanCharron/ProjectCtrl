using UnityEngine;
using System.Collections;

public class Bell : MonoBehaviour {

    private int curNbBellHits = 0;
    private int nbBellHits = 5;
    private Team assignedTeam;


	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Orb"))
		{
            onBellHit();

            if (shouldEnableTeamPower())
                onEnableTeamPower(true);
            else
                UIEffectManager.OnFreezeFrame(.1f, 1);

            OrbController.onPush (OrbController.MomentumBell + OrbController.CurrentVelocity);
		}

	}



    bool shouldEnableTeamPower()
    {
        return curNbBellHits >= nbBellHits;
    }

    void onEnableTeamPower(bool state)
    {
        if(state)
        {
            WindShader.onEnableWind();
        }
        switch(assignedTeam.powerID)
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
        if(state)
        {
            UIEffectManager.OnFreezeFrame(.5f, 5);
            Invoke("onDisableStuntPower", 2f);
        }
           

        //assignedTeam.onStunt(state);

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
    }

    public void onBellHit()
    {
        curNbBellHits++;
        WwiseManager.onPlayWWiseEvent("STAGE_BELL", gameObject);
        GetComponent<Animator>().Play("DONG", 0, -1);
    }
}
