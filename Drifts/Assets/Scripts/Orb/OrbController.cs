using UnityEngine;
using System.Collections;

public class OrbController : MonoBehaviour
{

    public static OrbController Instance { private set; get; }

	public Vector3 DestinationAngle { private set; get; }

	public float CurrentVelocity { private set; get; }
    public float DestinationVelocity { private set; get; } 
    public float LerpTimer { private set; get; }

	public float VelocityRatio { get { return Mathf.Clamp01(Instance.CurrentVelocity / _MaxVelocity); } }


    public int OrbStateID { private set; get; }

    public Transform[] OrbSpawnPoints { get { return GameController.Instance._OrbSpawnPoints; } }

    // Public variable for game designers to tweek ball velocity.
    public float _MaxVelocity, _MinVelocity, _DecreaseVelocity, _MomentumVelocity, _MomentumBell;

    // Public variable for game designers to tweek ball color.
    public Color _NeutralColor, _Team1Color, _Team2Color;

    void SetDestinationVelocity()
    {
        DestinationVelocity = Mathf.Clamp(DestinationVelocity, _MinVelocity, _MaxVelocity);
    }

    public ParticleSystem ParticleSystemRender { get { return Instance.pSystem; } }

    public Rigidbody RigidBody { private set; get; }

    public TeamController.TeamID PossessedTeam { private set; get; }

    private bool isPushed,isPushable = true;

    [SerializeField]
    public ParticleSystem pSystem;

    [SerializeField]
    public ParticleSystem pSystemBall;


    public GameObject mainOrb;

	#region LayerMask
	int layerMask_Team1;
	int layerMask_Team2;
	int layerMask_Orb;
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
		layerMask_Team1 = LayerMask.NameToLayer("Team1");
		layerMask_Team2 = LayerMask.NameToLayer("Team2");
		layerMask_Orb = LayerMask.NameToLayer("Orb");
	}

	void UpdateCollisionMatrix(TeamController.TeamID newTeam)
	{
		switch(newTeam)
		{
		case TeamController.TeamID.Team1 : 
			Physics.IgnoreLayerCollision(layerMask_Team1, layerMask_Orb, true);
			Physics.IgnoreLayerCollision(layerMask_Team2, layerMask_Orb, false);
			break;
		case TeamController.TeamID.Team2 :
			Physics.IgnoreLayerCollision(layerMask_Team1, layerMask_Orb, false);
			Physics.IgnoreLayerCollision(layerMask_Team2, layerMask_Orb, true);
			break;
		case TeamController.TeamID.Neutral :
			Physics.IgnoreLayerCollision(layerMask_Team1, layerMask_Orb, true);
			Physics.IgnoreLayerCollision(layerMask_Team2, layerMask_Orb, true);
			break;
		}
	}

    public void ShouldBallBeEnabled(bool state)
    {
        Instance.gameObject.SetActive(state);
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
        else if (newTeam == TeamController.TeamID.Team1)
            col = _Team1Color;
        else if (newTeam == TeamController.TeamID.Team2)
            col = _Team2Color;

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
        RigidBody = GetComponent<Rigidbody>();
        Instance = this;
    }

    private void SetProperties()
    {
        CurrentVelocity = _MinVelocity;
        DestinationVelocity = _MaxVelocity / 4;
        isPushable = true;
        RigidBody.velocity = Vector3.zero;
        isPushed = false;
        LerpTimer = 0f;

    }

    public void Reset()
    {
        Instance.gameObject.SetActive(true);
        Instance.SetProperties();
        Instance.gameObject.transform.position = OrbSpawnPoints[Random.Range(0, OrbSpawnPoints.Length)].position;
    }

	//Pour penality? Maybe refactor en 1 Push()
    public void Push(Vector3 angle, TeamController.TeamID teamID)
    {
        isPushed = true;
        DestinationVelocity = _MaxVelocity / 5;
        SetDestinationVelocity();
        DestinationAngle = angle;
        Instance.onSetBallStage();
        ChangeTeamPossession(teamID);
		Debug.Log("pushed player angle");

		Instance.RigidBody.velocity = angle.normalized * DestinationVelocity;

    }


	public void Push(Vector3 angle, PlayerScript player)
    {
        isPushed = true;
        float additionalVel = player.PulledVelocity != 0 ? player.PulledVelocity : 0;
        DestinationVelocity = CurrentVelocity * 1.1f + additionalVel + (_MomentumVelocity * player.Owner.RightTriggerHold.holdingButtonRatio);
        SetDestinationVelocity();
        DestinationAngle = angle;
        Instance.onSetBallStage();
        player.SetPulledVelocity(0);
		Debug.Log("pushed player v3");

		Instance.RigidBody.velocity = angle.normalized * DestinationVelocity;
    }


    public void Push(float destVelocity)
    {
        if (isPushable)
        {
            isPushed = true;
            DestinationVelocity = destVelocity  * 1.1f;
            SetDestinationVelocity();
            Instance.onSetBallStage();
			Debug.Log("pushed velocity");
			//instance.rBody.velocity = urrentVelocity;

        }
    }

    public void Push(Vector3 angle, float destVelocity)
    {
        if (isPushable)
        {
            isPushed = true;
            DestinationVelocity = destVelocity  * 1.1f;
            SetDestinationVelocity();
            DestinationAngle = angle;
            Instance.onSetBallStage();

			Instance.RigidBody.velocity = angle.normalized * DestinationVelocity;

        }
    }

    void onSetBallStage()
    {
        int previousOrbID = OrbStateID;

        if (CurrentVelocity > 100)
        {
            mainOrb.SetActive(true);

            if (CurrentVelocity > 500)
                OrbStateID = 3;

            else if (CurrentVelocity > 300)
                OrbStateID = 2;

            else
                OrbStateID = 1;


            mainOrb.GetComponent<Animator>().Play("stage" + OrbStateID.ToString());
        }
        else
        {
            OrbStateID = 0;
            mainOrb.SetActive(false);
        }
		
        if (previousOrbID != OrbStateID && GameController.IsGameStarted)
        {
            if (previousOrbID < OrbStateID)
                WwiseManager.PostEvent("BALL_STATE_UP", gameObject);
            else
                WwiseManager.PostEvent("BALL_STATE_DOWN", gameObject);
        }
    }


    public void Pull(Vector3 angle, float velocityApplied)
    {
        isPushed = true;
        DestinationVelocity = CurrentVelocity + velocityApplied;
        Instance.RigidBody.velocity = (angle * -DestinationVelocity);
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameController.IsGameStarted)
        {
            RigidBody.velocity = Vector3.zero;
            return;
        }
        else
        {
            ChangeVelocity();
        }
      
    }


    public void ChangeAngle(Vector3 Angle)
    {
        DestinationAngle = Angle;
    }


    private void ChangeVelocity()
    {
        if (isPushed)
        {
            LerpTimer += Time.deltaTime * 3;
            if (LerpTimer >= 1)
            {
                LerpTimer = 0;
                isPushed = false;
            }
            else
            {
                CurrentVelocity -= _DecreaseVelocity;
            }

            // Clamp max and min velocity
            CurrentVelocity = Mathf.Clamp(CurrentVelocity, _MinVelocity, _MaxVelocity);
        }
        CurrentVelocity = Mathf.Lerp(CurrentVelocity, DestinationVelocity, Mathfx.Sinerp(0, 1, LerpTimer));
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