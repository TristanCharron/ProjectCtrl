using UnityEngine;
using System.Collections;

public class ballManager : MonoBehaviour
{

    public static ballManager Instance { get { return instance; } }
    static ballManager instance;

    private static Vector3 velocityAngle; // angle pushing the ball

    public static Vector3 VelocityAngle { get { return velocityAngle; } }

    private static float currentVelocity, destinationVelocity, LerpTimer; //Speed variables

    public static float MaxVelocity { get { return Instance._MaxVelocity; } }

    public static float MinVelocity { get { return Instance._MinVelocity; } }

	public static float MomentumVelocity { get { return Instance._MomentumVelocity; } }

    public static float CurrentVelocity { get { return Mathf.Clamp(currentVelocity, MinVelocity, MaxVelocity); } } //Clamp & Return speed

    public static float DecreaseVelocity { get { return Instance._DecreaseVelocity; } }

	public static float MomentumBell { get { return Instance._MomentumBell; } }

    public static ParticleSystem ParticleSystemRender { get { return Instance.pSystem; } }

	public float _MaxVelocity, _MinVelocity, _DecreaseVelocity, _MomentumVelocity, _MomentumBell;

    private Rigidbody rBody;

    public Rigidbody RigidBody { get { return rBody; } }

    private static MonkController.PlayerTeam possessedTeam = 0;

    public static MonkController.PlayerTeam PossessedTeam { get { return possessedTeam; } }

    public MonkController.PlayerTeam CurrentPossessedTeam;

    private static bool isPushed;

    [SerializeField]
    public ParticleSystem pSystem;


	public GameObject StageBall;
	int BallStage = 0;



    // Use this for initialization
    void Awake()
    {
        onSetComponents();
        onSetProperties();
        onChangeTeamPossession(MonkController.PlayerTeam.Neutral);

    }

    public static void onChangeTeamPossession(MonkController.PlayerTeam newTeam)
    {
        Debug.Log("THIS");
        possessedTeam = newTeam;
        if (newTeam == MonkController.PlayerTeam.Neutral)
            Instance.pSystem.startColor = new Color(255, 255, 255, 255);
        else if (newTeam == MonkController.PlayerTeam.Team1)
            Instance.pSystem.startColor = new Color(0, 0, 255, 255);
        else if (newTeam == MonkController.PlayerTeam.Team2)
            Instance.pSystem.startColor = new Color(255, 0, 0, 255);

        Instance.pSystem.GetComponent<ParticleSystemRenderer>().material.SetColor("_EmisColor", Instance.pSystem.startColor);





    }






    private void onSetComponents()
    {
        rBody = GetComponent<Rigidbody>();
        instance = this;
    }

    private void onSetProperties()
    {
        currentVelocity = MinVelocity;
        destinationVelocity = currentVelocity;
        rBody.velocity = rBody.velocity * MinVelocity;
        isPushed = false;
        LerpTimer = 0;
    }


    public static void onPush(Vector3 angle)
    {
		
        isPushed = true;
		destinationVelocity = currentVelocity + MomentumVelocity;
        Instance.onSetVelocity(angle * destinationVelocity);

    }


	public static void onPush(float destVelocity)
	{

		isPushed = true;
		destinationVelocity = destVelocity;

	}

	public static void onPush(Vector3 angle, float destVelocity)
	{
		Debug.Log (destVelocity);
		isPushed = true;
		destinationVelocity = destVelocity;
		Instance.onSetVelocity(angle * destinationVelocity);

	}


    void onSetVelocity(Vector3 vel)
    {
		int oldBallStage = BallStage;


		//AkSoundEngine.PostEvent ("BALL_IMPACT", gameObject);



		if (ballManager.CurrentVelocity > 100) {
			StageBall.SetActive (true);

			if (ballManager.CurrentVelocity > 500)
			{
				BallStage = 3;
				StageBall.GetComponent<Animator> ().Play ("stage3");
			}
			else if (ballManager.CurrentVelocity > 300)
			{
				BallStage = 2;
				StageBall.GetComponent<Animator> ().Play ("stage2");
			} 
			else 
			{
				BallStage = 1;
				StageBall.GetComponent<Animator> ().Play ("stage1");
			}
			

		}
		else 
		{
			BallStage = 0;
			StageBall.SetActive (false);
		}


        rBody.velocity = vel;


		if (oldBallStage != BallStage) 
		{
			if (oldBallStage < BallStage)
				AkSoundEngine.PostEvent ("BALL_STATE_UP", gameObject);
			else
				AkSoundEngine.PostEvent ("BALL_STATE_DOWN", gameObject);
		}

    }


    public static void onPull(Vector3 angle, float velocityApplied)
    {
        isPushed = true;
        //velocityAngle = newAngle;
        destinationVelocity = currentVelocity + velocityApplied;
        Instance.RigidBody.velocity = (angle * -destinationVelocity);
        Debug.Log("on pull");
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        onChangeVelocity();
        CurrentPossessedTeam = PossessedTeam;
    }





    private void onChangeVelocity()
    {
        //transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + Time.deltaTime * 10);

        if (isPushed)
        {
            currentVelocity = Mathf.Lerp(currentVelocity, destinationVelocity, LerpTimer);
            LerpTimer += Time.fixedDeltaTime;
            if (LerpTimer >= 1)
            {
                LerpTimer = 0;
                isPushed = false;
            }

        }
        else
        {
            currentVelocity -= DecreaseVelocity;

        }

        currentVelocity = Mathf.Clamp(currentVelocity, _MinVelocity, _MaxVelocity);

        //rBody.velocity = rBody.velocity * (currentVelocity * 0.01f);

        //	Debug.Log (rBody.velocity.normalized);

        rBody.velocity = rBody.velocity.normalized * currentVelocity;

        //rBody.velocity =  rBody.velocity * currentVelocity;

    }










}
