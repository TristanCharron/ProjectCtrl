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

    private static teamManager.teamID possessedTeam = 0;

    public static teamManager.teamID PossessedTeam { get { return possessedTeam; } }

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
        onChangeTeamPossession(teamManager.teamID.Neutral);

    }

    public static void onChangeTeamPossession(teamManager.teamID newTeam)
    {
        possessedTeam = newTeam;
        if (newTeam == teamManager.teamID.Neutral)
            Instance.pSystem.startColor = new Color(255, 255, 255, 255);
        else if (newTeam == teamManager.teamID.Team1)
            Instance.pSystem.startColor = new Color(0, 0, 255, 255);
        else if (newTeam == teamManager.teamID.Team2)
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


    public static void onPush(Vector3 angle, MonkController pushingPlayer)
    {

        isPushed = true;
        Debug.Log(pushingPlayer.ChargingPower);
        destinationVelocity = currentVelocity + (MomentumVelocity * pushingPlayer.ChargingPower);
        velocityAngle = angle;
        Instance.onSetBallStage();

    }


    public static void onPush(float destVelocity)
    {

        isPushed = true;
        destinationVelocity = destVelocity;
        Instance.onSetBallStage();
    }

    public static void onPush(Vector3 angle, float destVelocity)
    {
        isPushed = true;
        destinationVelocity = destVelocity;
        velocityAngle = angle;
        Instance.onSetBallStage();
    }




    void onSetBallStage()
    {
        int oldBallStage = BallStage;

        if (WwiseManager.isWwiseEnabled)
            AkSoundEngine.PostEvent("BALL_IMPACT", gameObject);



        if (CurrentVelocity > 100)
        {
            StageBall.SetActive(true);

            if (CurrentVelocity > 500)
            {
                BallStage = 3;
                StageBall.GetComponent<Animator>().Play("stage3");
            }
            else if (CurrentVelocity > 300)
            {
                BallStage = 2;
                StageBall.GetComponent<Animator>().Play("stage2");
            }
            else
            {
                BallStage = 1;
                StageBall.GetComponent<Animator>().Play("stage1");
            }


        }
        else
        {
            BallStage = 0;
            StageBall.SetActive(false);
        }





        if (oldBallStage != BallStage)
        {
            if (oldBallStage < BallStage)
                if (WwiseManager.isWwiseEnabled)
                    AkSoundEngine.PostEvent("BALL_STATE_UP", gameObject);
                else
                if (WwiseManager.isWwiseEnabled)
                    AkSoundEngine.PostEvent("BALL_STATE_DOWN", gameObject);
        }
        
    }


    public static void onPull(Vector3 angle, float velocityApplied)
    {
        isPushed = true;
        destinationVelocity = currentVelocity + velocityApplied;
        Instance.RigidBody.velocity = (angle * -destinationVelocity);
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        onChangeVelocity();
    }





    private void onChangeVelocity()
    {
        if (isPushed)
        {
            currentVelocity = Mathf.Lerp(currentVelocity, destinationVelocity, LerpTimer);
            rBody.velocity = Vector3.Lerp(rBody.velocity, destinationVelocity * velocityAngle, LerpTimer);
            LerpTimer += Time.fixedDeltaTime * 5;
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
        rBody.velocity = rBody.velocity.normalized * currentVelocity;

    }










}
