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
                DisableBell();
            }
        }
    }

    void DisableBell()
    {
        isActive = false;
        Invoke("EnableBell", bellLength);
    }

    void EnableBell()
    {
        isActive = true;
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
		if (OrbController.Instance.PossessedTeam == TeamController.TeamID.Neutral || assignedTeam == null)
            return;


        AddScore(TeamController.GetOtherTeam(assignedTeam));
		WwiseManager.PostEvent("STAGE_BELL", gameObject);
		GetComponent<Animator>().Play("DONG", 0, -1);

		float ratio = OrbController.Instance.VelocityRatio;
		ShockwaveManager.GetInstance().CastShockwave(sizeShockWave*ratio,transform.position,speedShockWave*ratio,colorShockWave,intensityShockWave);
		UIEffectManager.Instance.ScreenShake(screenShake * ratio);
		UIEffectManager.Instance.FreezeFrame(freezeFrame * ratio);
	}

    private void AddScore(Team team)
    {
        curNbBellHits++;
		team.AddHitScore((int)OrbController.Instance.CurrentVelocity / 3);
    }
}
