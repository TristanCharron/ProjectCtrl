using UnityEngine;
using System.Collections;

public class OrbController : MonoBehaviour
{
	#region Accesor
    public static OrbController Instance { private set; get; }
	public int OrbStateID { private set; get; }
	public Transform[] OrbSpawnPoints { get { return GameController.Instance._OrbSpawnPoints; } }
	public TeamController.TeamID PossessedTeam { private set; get; }
	public ParticleSystem ParticleSystemRender { get { return Instance.pSystem; } }

	public Vector3 CurrentDirection { get { return rigidBody.velocity.normalized; } }
	public float CurrentVelocity { get { return rigidBody.velocity.magnitude; } }
	public float VelocityRatio { get { return Mathf.Clamp01(rigidBody.velocity.magnitude / _MaxVelocity); } }
	#endregion

	#region Param
	[Header("Parameter")]
    [SerializeField] Color _NeutralColor, _TeamBlueColor, _TeamRedColor;
	[SerializeField] float _MaxVelocity, _MinVelocity, _DecreaseVelocity;

	[SerializeField] float thresholdAnim1 = 100;
	[SerializeField] float thresholdAnim2 = 300;
	[SerializeField] float thresholdAnim3 = 500;
	#endregion

	#region Component
	[Header("Components")]
	[SerializeField] Rigidbody rigidBody;
    [SerializeField] ParticleSystem pSystem;
    [SerializeField] ParticleSystem pSystemBall;

	[SerializeField] GameObject orbWithAnimation;

	#endregion

	#region LayerMask
	int layerMask_TeamRed;
	int layerMask_TeamBlue;
	int layerMask_Orb;
	#endregion

	#region private
	float pulledMomentum;  
	bool isPushed,isPushable = true;
	#endregion

    void Awake()
    {
        Instance = this;
        SetComponents();
        ChangeTeamPossession(TeamController.TeamID.Neutral);
        GameController.SetNextRoundEvent += Reset;
		SetLayerMask();
    }

	void SetLayerMask()
	{
		layerMask_TeamRed = LayerMask.NameToLayer("TeamRed");
		layerMask_TeamBlue = LayerMask.NameToLayer("TeamBlue");
		layerMask_Orb = LayerMask.NameToLayer("Orb");
	}

	void UpdateCollisionMatrix(TeamController.TeamID newTeam)
	{
		switch(newTeam)
		{
		case TeamController.TeamID.TeamRed : 
			Physics.IgnoreLayerCollision(layerMask_TeamRed, layerMask_Orb, true);
			Physics.IgnoreLayerCollision(layerMask_TeamBlue, layerMask_Orb, false);
			break;
		case TeamController.TeamID.TeamBlue :
			Physics.IgnoreLayerCollision(layerMask_TeamRed, layerMask_Orb, false);
			Physics.IgnoreLayerCollision(layerMask_TeamBlue, layerMask_Orb, true);
			break;
		case TeamController.TeamID.Neutral :
			Physics.IgnoreLayerCollision(layerMask_TeamRed, layerMask_Orb, true);
			Physics.IgnoreLayerCollision(layerMask_TeamBlue, layerMask_Orb, true);
			break;
		}
	}

    public void ChangeTeamPossession(TeamController.TeamID newTeam)
    {

        if (!Instance.isActiveAndEnabled)
            return;

        PossessedTeam = newTeam;
        Color col = Color.clear;
        Instance.StopCoroutine(Instance.LerpBallColorCoRoutine(col));

        if (newTeam == TeamController.TeamID.Neutral)
            col = _NeutralColor;
        else if (newTeam == TeamController.TeamID.TeamRed)
            col = _TeamRedColor;
        else if (newTeam == TeamController.TeamID.TeamBlue)
            col = _TeamBlueColor;

		Instance.UpdateCollisionMatrix(newTeam);
        Instance.StartCoroutine(Instance.LerpBallColorCoRoutine(col));
    }

    public IEnumerator LerpBallColorCoRoutine(Color dest)
    {
        Instance.pSystem.GetComponent<ParticleSystemRenderer>().material.SetColor("_EmisColor", dest);
        Instance.pSystemBall.GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", dest);
        yield break;
    }

    private void SetComponents()
    {
        rigidBody = GetComponent<Rigidbody>();
        Instance = this;
    }

    private void SetProperties()
    {
		StopVelocity();
		isPushable = true;
        isPushed = false;
    }

	public void StopVelocity()
	{
		rigidBody.velocity = Vector3.zero;
		pulledMomentum = 0;
	}

    public void Reset()
    {
        Instance.gameObject.SetActive(true);
        Instance.SetProperties();
        Instance.gameObject.transform.position = OrbSpawnPoints[Random.Range(0, OrbSpawnPoints.Length)].position;
    }

	public void Pull(TeamController.TeamID teamID)
	{
		ChangeTeamPossession(teamID);
		pulledMomentum = rigidBody.velocity.magnitude;
		rigidBody.velocity =  Vector3.zero;
	}

	public void Push(Vector3 angle, float pushPower, TeamController.TeamID teamID)
	{
		float currentVelocity;

		if(pulledMomentum == 0)
			currentVelocity = rigidBody.velocity.magnitude + pushPower;
		else
			currentVelocity = pulledMomentum + pushPower*2;
		
		pulledMomentum = 0;
		isPushed = true;

		ChangeTeamPossession(teamID);
		onSetBallStage();

		currentVelocity = Mathf.Clamp(currentVelocity, _MinVelocity, _MaxVelocity);
		rigidBody.velocity = angle.normalized * currentVelocity;
	}

	void onSetBallStage()
    {
        int previousOrbID = OrbStateID;
		float trueVelocity = GetTrueVelocity();
		if (trueVelocity > thresholdAnim1)
        {
			orbWithAnimation.SetActive(true);

			if (trueVelocity > thresholdAnim3)
                OrbStateID = 3;
			else if (trueVelocity  > thresholdAnim2)
                OrbStateID = 2;
            else
                OrbStateID = 1;
		
			orbWithAnimation.GetComponent<Animator>().Play("stage" + OrbStateID.ToString());
        }
        else
        {
            OrbStateID = 0;
			orbWithAnimation.SetActive(false);
        }
		
        if (previousOrbID != OrbStateID && GameController.Instance.IsGameStarted)
        {
            if (previousOrbID < OrbStateID)
                WwiseManager.PostEvent("BALL_STATE_UP", gameObject);
            else
                WwiseManager.PostEvent("BALL_STATE_DOWN", gameObject);
        }
    }

	/// <summary>
	/// Return the velocity or if the Orb is pulled, return the momentum
	/// </summary>
	/// <returns>The true velocity.</returns>
	float GetTrueVelocity()
	{
		if(pulledMomentum == 0)
			return rigidBody.velocity.magnitude;
		else
			return pulledMomentum;
	}

    public void DisableOrb()
    {
        isPushable = false;
        Instance.StopAllCoroutines();
    }

    public void EnableOrb()
    {
        isPushable = true;
    }
}