using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class Bell : MonoBehaviour
{
	private int maxHp = 10;
	private int currentHp = 10;

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
	[SerializeField] float screenShake = 2;

	[Header("Life Shader Param")]
	private Material material;
	[SerializeField] Color normalColor;
	[SerializeField] Color flashColor1;
	[SerializeField] Color flashColor2;
	[SerializeField] float flashColorSpeed = 1;

	[SerializeField] GameObject fracturedBellPrefab;

	void Awake()
	{
		material = GetComponent<MeshRenderer>().material;
		SetShader();
		currentHp = maxHp;
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
		Debug.Log("reset");
		gameObject.SetActive(true);
		currentHp = maxHp;
		SetShader();
		UpdateShader();
	}

    public void AssignTeam(Team newTeam)
    {
        assignedTeam = newTeam;
    }

    private void CheckBellHit()
    {
        //No Team, invalid
		if (OrbController.Instance.PossessedTeam == TeamController.TeamID.Neutral || assignedTeam == null)
            return;

		//A voir si on met un score similaire dans le futur
        //AddScore(TeamController.Instance.GetOtherTeam(assignedTeam));
		
		WwiseManager.PostEvent("STAGE_BELL", gameObject);
		GetComponent<Animator>().Play("DONG", 0, -1);
		UIEffectManager.Instance.OnFreezeFrame(freezeFrame * GetInvRatioLife());

		float ratio = OrbController.Instance.VelocityRatio;
		//ShockwaveManager.GetInstance().CastShockwave(sizeShockWave*(0.1f+GetInvRatioLife()),transform.position,speedShockWave,colorShockWave,intensityShockWave);
		//GameEffect.Shake(Camera.main.gameObject, screenShake * GetInvRatioLife(), 0.5f * GetInvRatioLife(), true);
		ShockwaveManager.GetInstance().CastShockwave(sizeShockWave*ratio,transform.position,speedShockWave,colorShockWave,intensityShockWave);
		GameEffect.Shake(Camera.main.gameObject, screenShake * ratio, 0.5f * ratio, true);

		LooseHp(1);
	}

	private void LooseHp(int damage)
	{
		if(GameController.CurrentGameMode.GetType() != typeof(GameModeBell))
			return;

		currentHp -= damage;
		if(currentHp <= 0)
		{
			BellExplode();
			//GameController.Instance.EndGame(TeamController.Instance.GetOtherTeam(assignedTeam));
			//les bells sont reversed?
			GameController.Instance.EndGame(assignedTeam);

		}
		else
		{
			UpdateShader();
		}
	}

	private void BellExplode()
	{
		//MASS EFFECT NICE
		
		//sound

		//particle
		ShockwaveManager.GetInstance().CastShockwave(sizeShockWave,transform.position,speedShockWave*0.3f,colorShockWave,intensityShockWave*3);

		GameObject fracBell = Instantiate(fracturedBellPrefab, transform.position, Quaternion.identity);
		ShatterMesh shatterFracBell = fracBell.GetComponent<ShatterMesh>();
		shatterFracBell.ColorMesh(normalColor);
		shatterFracBell.Shatter((OrbController.Instance.CurrentDirection));
        //Time.timeScale = .5f;
        DOTween.Kill(Time.timeScale);
        DOTween.To(() => Time.timeScale, timescale => Time.timeScale = timescale, 1f, .2f);
		gameObject.SetActive(false);
	}

	private void SetShader()
	{
		material.SetColor("_Color", normalColor);
		material.SetFloat("_FracSpeed", 0);
		material.SetColor("_FracColor", flashColor1);
		material.SetColor("_FracColor2", flashColor2);
	}
	private void UpdateShader()
	{
		material.SetFloat("_FracRatio", GetInvRatioLife());
		material.SetFloat("_FracSpeed", GetInvRatioLife() * flashColorSpeed);

	}

	/* old score version
    private void AddScore(Team team)
    {
        //curNbBellHits++;
		//team.AddHitScore((int)OrbController.Instance.CurrentVelocity / 3);
    }
	*/

	float GetRatioLife()
	{
		return (float)currentHp / (float)maxHp;
	}

	float GetInvRatioLife()
	{
		return 1 - (float)currentHp / (float)maxHp;
	}
}
