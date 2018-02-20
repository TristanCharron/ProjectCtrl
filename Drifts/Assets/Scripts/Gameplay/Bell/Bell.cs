using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class Bell : MonoBehaviour
{

    private int curNbBellHits = 0;
    private Team assignedTeam;
    private bool isActive = true;
    private static float bellLength = 2f;

	[SerializeField] float sizeShockWave = 1;
	[SerializeField] float speedShockWave = .5f;
	[SerializeField] Color colorShockWave;
	[SerializeField] float intensityShockWave = .9f;
	[SerializeField] float freezeFrame = .15f;
	[SerializeField] float screenShake = 15;

    void OnCollisionEnter(Collision other)
    {
        if (isActive)
        {
            if (other.gameObject.CompareTag("Orb"))
            {
				CheckBellHit();

				//OrbController.Instance.Push(transform.right,OrbController.Instance.CurrentVelocity * 1.1f);
                DisableBell();
            }
        }
    }

    void DisableBell()
    {
        isActive = false;
        Invoke("OnEnableBell", bellLength);
    }

    void OnEnableBell()
    {
        isActive = true;
    }


    void EnableTeamPower(bool state)
    {
        switch (assignedTeam.powerID)
        {
            case TeamController.powerID.stunt:
                OnEnableStuntPower(state);
                break;
            default:
                OnEnableStuntPower(state);
                break;
        }

        ResetBell();
    }



	void OnEnableStuntPower(bool state)
    {
        if (state)
        {
			UIEffectManager.Instance.FreezeFrame(OrbController.Instance.velocityRatio / 3);
            Invoke("onDisableStuntPower", 2f);
        }

    }

    void DisableStuntPower()
    {
        OnEnableStuntPower(false);
    }

    public void AssignTeam(Team newTeam)
    {
        assignedTeam = newTeam;
    }

    void ResetBell()
    {
        curNbBellHits = 0;
        isActive = true;

    }

    private void CheckBellHit()
    {
        //No Team, invalid
		if (OrbController.Instance.PossessedTeam == TeamController.teamID.Neutral || assignedTeam == null)
            return;


        AddScore(TeamController.onReturnOtherTeam(assignedTeam));
		WwiseManager.PostEvent("STAGE_BELL", gameObject);
		GetComponent<Animator>().Play("DONG", 0, -1);

		float ratio = OrbController.Instance.velocityRatio;
		ShockwaveManager.GetInstance().CastShockwave(sizeShockWave*ratio,transform.position,speedShockWave*ratio,colorShockWave,intensityShockWave);
		UIEffectManager.Instance.ScreenShake(screenShake * ratio);
		UIEffectManager.Instance.FreezeFrame(freezeFrame * ratio);
	}

    private void AddScore(Team team)
    {
        curNbBellHits++;
		team.onAddHitScore((int)OrbController.Instance.CurrentVelocity / 3);
    }
}
