using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Bell : MonoBehaviour
{
	private int maxHp = 10;
	private int currentHp;

    //private int curNbBellHits = 0;
    private Team assignedTeam;
    private bool isActive = true;
    private static float bellLength = .5f;

	[Header("On hit effect Param")]
	[SerializeField] float sizeShockWave = 1;
	[SerializeField] float speedShockWave = .5f;
	[SerializeField] Color colorShockWave;
	[SerializeField] float intensityShockWave = .9f;
	[SerializeField] float freezeFrame = .15f;
	[SerializeField] float screenShake = 15;

	[Header("Life Shader Param")]
	private Material material;
	[SerializeField] Color normalColor;
	[SerializeField] Color flashColor1;
	[SerializeField] Color flashColor2;
	[SerializeField] float flashColorSpeed = 1;

	void Awake()
	{
		material = GetComponent<MeshRenderer>().material;
		SetShader();
	}

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
	public void ResetLife()
	{
		currentHp = maxHp;
	}

    public void AssignTeam(Team newTeam)
    {
        assignedTeam = newTeam;
    }

    void ResetBell()
    {
		currentHp = maxHp;
		SetShader();
        //curNbBellHits = 0;
        isActive = true;
    }

    private void CheckBellHit()
    {
        //No Team, invalid
		if (OrbController.Instance.PossessedTeam == TeamController.TeamID.Neutral || assignedTeam == null)
            return;
		
        AddScore(TeamController.Instance.GetOtherTeam(assignedTeam));
		WwiseManager.PostEvent("STAGE_BELL", gameObject);
		GetComponent<Animator>().Play("DONG", 0, -1);

		float ratio = OrbController.Instance.VelocityRatio;
		ShockwaveManager.GetInstance().CastShockwave(sizeShockWave*(0.1f+GetInvRatioLife()),transform.position,speedShockWave,colorShockWave,intensityShockWave);
		UIEffectManager.Instance.OnScreenShake(screenShake * GetInvRatioLife());
		UIEffectManager.Instance.OnFreezeFrame(freezeFrame * GetInvRatioLife());
		LooseHp(1);
	}

	private void LooseHp(int damage)
	{
		if(GameController.CurrentGameMode.GetType() != typeof(GameModeBell))
			return;

		currentHp -= damage;
		if(currentHp <= 0)
		{
			//MASS EFFECT NICE

			//sound
			//particle
			//shatter?

			GameController.Instance.EndGame(TeamController.Instance.GetOtherTeam(assignedTeam));
		}
		UpdateShader();
	}

	private void SetShader()
	{
		material.SetColor("_Color", normalColor);
		material.SetFloat("_FracSpeed", flashColorSpeed);
		material.SetColor("_FracColor", flashColor1);
		material.SetColor("_FracColor2", flashColor2);
	}
	private void UpdateShader()
	{
		material.SetFloat("_FracRatio", GetInvRatioLife());
	}

    private void AddScore(Team team)
    {
        //curNbBellHits++;
		team.AddHitScore((int)OrbController.Instance.CurrentVelocity / 3);
    }

	float GetRatioLife()
	{
		return (float)currentHp / (float)maxHp;
	}

	float GetInvRatioLife()
	{
		return 1 - (float)currentHp / (float)maxHp;
	}
}
