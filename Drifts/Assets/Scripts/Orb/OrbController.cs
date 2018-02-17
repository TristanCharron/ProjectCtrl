using UnityEngine;
using System.Collections;

public class OrbController : MonoBehaviour
{

    public static OrbController Instance { get { return instance; } }
    static OrbController instance;

	private static Vector3 destinationAngle; // angle pushing the ball
	public static Vector3 DestinationAngle { get { return destinationAngle; } }	

	[SerializeField] private float currentVelocity, destinationVelocity, LerpTimer; //Speed variables

	public float velocityRatio { get { return Mathf.Clamp01(instance.currentVelocity / MaxVelocity); } }

	public float MaxVelocity { get { return Instance._MaxVelocity; } }

	public float MinVelocity { get { return Instance._MinVelocity; } }

	public float MomentumVelocity { get { return Instance._MomentumVelocity; } }

	public float CurrentVelocity { get { return Mathf.Clamp(currentVelocity, MinVelocity, MaxVelocity); } } //Clamp & Return speed

	public float DecreaseVelocity { get { return Instance._DecreaseVelocity; } }

    public float MomentumBell { get { return Instance._MomentumBell; } }

    public Color NeutralColor { get { return Instance._NeutralColor; } }
    public Color Team1Color { get { return Instance._Team1Color; } }
    public Color Team2Color { get { return Instance._Team2Color; } }

    private int orbStateID = 0;
    public int OrbStateID { get { return orbStateID; } }

    public Transform[] OrbSpawnPoints { get { return GameController.Instance._OrbSpawnPoints; } }

    // Public variable for game designers to tweek ball velocity.
    public float _MaxVelocity, _MinVelocity, _DecreaseVelocity, _MomentumVelocity, _MomentumBell;
    void onSetDestinationVelocity()
	{
		destinationVelocity = Mathf.Clamp(destinationVelocity, MinVelocity, MaxVelocity);
	}

    // Public variable for game designers to tweek ball color.
    public Color _NeutralColor, _Team1Color, _Team2Color;

    public ParticleSystem ParticleSystemRender { get { return Instance.pSystem; } }

    private Rigidbody rBody;

    public Rigidbody RigidBody { get { return rBody; } }

    private TeamController.teamID possessedTeam = 0;

    public TeamController.teamID PossessedTeam { get { return possessedTeam; } }

    public TeamController.teamID _PossessedTeam;

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
        instance = this;
        SetComponents();
        ChangeTeamPossession(TeamController.teamID.Neutral);
        GameController.SetNextRound += Reset;
		SetLayerMask();
    }

	void SetLayerMask()
	{
		layerMask_Team1 = LayerMask.NameToLayer("Team1");
		layerMask_Team2 = LayerMask.NameToLayer("Team2");
		layerMask_Orb = LayerMask.NameToLayer("Orb");
	}

	void UpdateCollisionMatrix(TeamController.teamID newTeam)
	{
		switch(newTeam)
		{
		case TeamController.teamID.Team1 : 
			Physics.IgnoreLayerCollision(layerMask_Team1, layerMask_Orb, true);
			Physics.IgnoreLayerCollision(layerMask_Team2, layerMask_Orb, false);
			break;
		case TeamController.teamID.Team2 :
			Physics.IgnoreLayerCollision(layerMask_Team1, layerMask_Orb, false);
			Physics.IgnoreLayerCollision(layerMask_Team2, layerMask_Orb, true);
			break;
		case TeamController.teamID.Neutral :
			Physics.IgnoreLayerCollision(layerMask_Team1, layerMask_Orb, true);
			Physics.IgnoreLayerCollision(layerMask_Team2, layerMask_Orb, true);
			break;
		}
	}

    public void ShouldBallBeEnabled(bool state)
    {
        instance.gameObject.SetActive(state);
    }

    public void ChangeTeamPossession(TeamController.teamID newTeam)
    {

        if (!instance.isActiveAndEnabled)
            return;

        possessedTeam = newTeam;
        Color col = Color.clear;
        instance.StopCoroutine(instance.LerpBallColorCoRoutine(col));

        if (newTeam == TeamController.teamID.Neutral)
            col = NeutralColor;
        else if (newTeam == TeamController.teamID.Team1)
            col = Team1Color;
        else if (newTeam == TeamController.teamID.Team2)
            col = Team2Color;

		instance.UpdateCollisionMatrix(newTeam);
        instance.StartCoroutine(instance.LerpBallColorCoRoutine(col));
    }

    public IEnumerator LerpBallColorCoRoutine(Color dest)
    {
        instance.pSystem.GetComponent<ParticleSystemRenderer>().material.SetColor("_EmisColor", dest);
        instance.pSystemBall.GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", dest);
        yield break;
    }

    private void SetComponents()
    {
        rBody = GetComponent<Rigidbody>();
        instance = this;
    }

    private void SetProperties()
    {
        currentVelocity = MinVelocity;
        destinationVelocity = MaxVelocity / 4;
        isPushable = true;
        rBody.velocity = Vector3.zero;
        isPushed = false;
        LerpTimer = 0f;

    }

    public void Reset()
    {
        instance.gameObject.SetActive(true);
        instance.SetProperties();
        instance.gameObject.transform.position = OrbSpawnPoints[Random.Range(0, OrbSpawnPoints.Length)].position;
    }

	//Pour penality? Maybe refactor en 1 Push()
    public void Push(Vector3 angle, TeamController.teamID teamID)
    {
            isPushed = true;
            destinationVelocity = MaxVelocity / 5;
            onSetDestinationVelocity();
            destinationAngle = angle;
            Instance.onSetBallStage();
            ChangeTeamPossession(teamID);

		instance.rBody.velocity = angle.normalized * destinationVelocity;

    }


	public void Push(Vector3 angle, PlayerScript player)
    {
        if (isPushable)
        {
            isPushed = true;
            float additionalVel = player.PulledVelocity != 0 ? player.PulledVelocity : 0;
            destinationVelocity = currentVelocity * 1.1f + additionalVel + (MomentumVelocity * player.Owner.RightTriggerHold.holdingButtonRatio);
            onSetDestinationVelocity();
            destinationAngle = angle;
            Instance.onSetBallStage();
            player.onSetPulledVelocity(0);

			instance.rBody.velocity = angle.normalized * destinationVelocity;
        }
    }


    public void Push(float destVelocity)
    {
        if (isPushable)
        {
            isPushed = true;
			destinationVelocity = destVelocity  * 1.1f;
            onSetDestinationVelocity();
            Instance.onSetBallStage();

			//instance.rBody.velocity = urrentVelocity;

        }
    }

    public void Push(Vector3 angle, float destVelocity)
    {
        if (isPushable)
        {
            isPushed = true;
			destinationVelocity = destVelocity  * 1.1f;
            onSetDestinationVelocity();
            destinationAngle = angle;
            Instance.onSetBallStage();

			instance.rBody.velocity = angle.normalized * destinationVelocity;

        }
    }

    void onSetBallStage()
    {
        int previousOrbID = OrbStateID;

        if (CurrentVelocity > 100)
        {
            mainOrb.SetActive(true);

            if (CurrentVelocity > 500)
                orbStateID = 3;

            else if (CurrentVelocity > 300)
                orbStateID = 2;

            else
                orbStateID = 1;


            mainOrb.GetComponent<Animator>().Play("stage" + OrbStateID.ToString());
        }
        else
        {
            orbStateID = 0;
            mainOrb.SetActive(false);
        }
		
        if (previousOrbID != OrbStateID && GameController.isGameStarted)
        {
            if (previousOrbID < OrbStateID)
                WwiseManager.onPlayWWiseEvent("BALL_STATE_UP", gameObject);
            else
                WwiseManager.onPlayWWiseEvent("BALL_STATE_DOWN", gameObject);
        }
    }


    public void Pull(Vector3 angle, float velocityApplied)
    {
        isPushed = true;
        destinationVelocity = currentVelocity + velocityApplied;
        Instance.RigidBody.velocity = (angle * -destinationVelocity);
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameController.isGameStarted)
        {
            rBody.velocity = Vector3.zero;
            return;
        }
        else
        {
            ChangeVelocity();
        }
      
    }


    public void ChangeAngle(Vector3 Angle)
    {
        destinationAngle = Angle;
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
                currentVelocity -= DecreaseVelocity;
            }

            // Clamp max and min velocity
            currentVelocity = Mathf.Clamp(currentVelocity, _MinVelocity, _MaxVelocity);
        }
        currentVelocity = Mathf.Lerp(currentVelocity, destinationVelocity, Mathfx.Sinerp(0, 1, LerpTimer));
   //     rBody.velocity = Vector3.Lerp(rBody.velocity, destinationVelocity * destinationAngle, Mathfx.Sinerp(0, 1, LerpTimer));
    }

    public void DisableOrb()
    {
        isPushable = false;
        instance.StopAllCoroutines();
    }

    public void EnableOrb()
    {
        isPushable = true;
    }
}