﻿using UnityEngine;
using System.Collections;


public class ButtonHolder
{

    public float currentHoldPushBtnTime = 0f;
    public const float maxHoldPushBtnTime = 1f;
    public float ChargingPower { get { return Mathf.Clamp01(currentHoldPushBtnTime / maxHoldPushBtnTime); } }

    public ButtonHolder()
    {
        currentHoldPushBtnTime = 0;
    }

    public void OnUpdate()
    {
        currentHoldPushBtnTime = currentHoldPushBtnTime > maxHoldPushBtnTime ? 0 : currentHoldPushBtnTime += Time.fixedDeltaTime;
    }

    public void OnReset()
    {
        currentHoldPushBtnTime = 0;
    }
}


public class PlayerController : MonoBehaviour
{
    public const string PRESS_L = "L_Press_";
    public const string PRESS_R = "R_Press_";

    private ButtonHolder leftTriggerHold;
    public ButtonHolder LeftTriggerHold { get { return leftTriggerHold; } }

    ButtonHolder rightTriggerHold;
    public ButtonHolder RightTriggerHold { get { return rightTriggerHold; } }

    Team currentTeam;

    [SerializeField]
    bool UsingController;
    
    
    //Player ID - Team
    public int PlayerID;


    Vector3 velocity;

    [SerializeField]
    float buildingSpeed = 0;

   

    [SerializeField]
    float SpeedPlayer = 10;
    BoxCollider PlayerCollider;
    [SerializeField]
    Animator handAnimator;
    //cursor
    [SerializeField]
    Transform cursorTransform;
    [SerializeField]
    Transform LookAtTransform;
    Vector3 startRotation;
    Vector3 endRotation;
    float cursor_t;
    [SerializeField]
    float cursorSpeed = 10;

    [SerializeField]
    bool isDead = false;
    [SerializeField]
    BoxCollider PushCollider;


    private float pulledVelocity = 0;
    public float PulledVelocity { get { return pulledVelocity; } }
    public void onSetPulledVelocity(float vel) { pulledVelocity = vel; }

    [SerializeField]
    BoxCollider PullCollider;
    [SerializeField]
    bool canDoAction = true;
    [SerializeField]
    GameObject WindGust;
    Rigidbody rBody;
    [SerializeField]
    SpawnManager accesSpawn;

    bool isChangingOrbAngle = false;


    SpriteRenderer sRenderer;

    private Color idleColor;

    [SerializeField]
    Color chargedColor;







    void Awake()
    {
        OnResetProperties();
    }

    void OnResetProperties()
    {
        PlayerCollider = GetComponent<BoxCollider>();
        rBody = GetComponent<Rigidbody>();
        sRenderer = GetComponent<SpriteRenderer>();
        idleColor = sRenderer.color;
        pulledVelocity = 0;
        buildingSpeed = 0;
        isDead = false;
        leftTriggerHold = new ButtonHolder();
        isChangingOrbAngle = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!GameController.isGameStarted)
            return;


        if (!currentTeam.isStunt && !isDead)
        {
            MoveCharacter();
            PushButton();
            PullButton();
            CursorRotation();
          
        }


    }

    public void onCharge()
    {
        Color currentChargingColor = Color.Lerp(idleColor, chargedColor, leftTriggerHold.ChargingPower);
        currentChargingColor.a = 1;
        sRenderer.color = currentChargingColor;
    }

    public void onAssignTeam(Team assignedTeam)
    {
        currentTeam = assignedTeam;
    }

    bool isHitValid()
    {
        return currentTeam.TeamID != OrbController.PossessedTeam && OrbController.PossessedTeam != TeamController.teamID.Neutral;
    }


    IEnumerator onGoingThrough()
    {
        PlayerCollider.enabled = false;
        rBody.isKinematic = true;
        yield return new WaitForSeconds(.2f);
        rBody.isKinematic = false;
        PlayerCollider.enabled = true;
    }



    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Orb")
        {

            if (isHitValid())
            {
                if (!isDead)
                {
                    GameObject DeathAnimParticle = Instantiate(Resources.Load<GameObject>("DeathMonkParticle"), gameObject.transform.position, Quaternion.identity) as GameObject;
                    Destroy(DeathAnimParticle, 5);
                    accesSpawn.onPlayerDeath(PlayerID);

                }
            }
            else
                StartCoroutine(onGoingThrough());

        }
    }


    void CursorRotation()
    {
        //lerp
        if (cursor_t < 1)
        {
            cursor_t += Time.deltaTime * cursorSpeed;
            transform.GetChild(0).localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(startRotation.z, endRotation.z, cursor_t));
        }
        //Control rotation
        float inputX;
        float inputY;
        if (UsingController)
        {
            inputX = Input.GetAxis("Horizontal_Rotation_" + PlayerID);
            inputY = Input.GetAxis("Vertical_Rotation_" + PlayerID);
        }
        else {
            inputX = Input.GetAxis("Horizontal2");
            inputY = Input.GetAxis("Vertical2");
        }
        if ((inputX > 0.5f || inputY > 0.5f) || (inputX < -0.5f || inputY < -0.5f))
        {
            startRotation = transform.GetChild(0).localEulerAngles;
            endRotation = new Vector3(0, 0, Mathf.Atan2(inputX, inputY) * 180 / Mathf.PI);
            cursor_t = 0;
        }
    }
  
    void MoveCharacter()
    {

        float inputX;
        float inputY;
        if (UsingController)
        {
            inputX = Input.GetAxis("Horizontal_Move_" + PlayerID);
            inputY = Input.GetAxis("Vertical_Move_" + PlayerID);
        }
        else {
            inputX = Input.GetAxis("Horizontal");
            inputY = Input.GetAxis("Vertical");
        }
        if ((inputX > 0.5f || inputY > 0.5f) || (inputX < -0.5f || inputY < -0.5f))
        {
            buildingSpeed += 0.05f;
            if (buildingSpeed > 2)
                buildingSpeed = 2;
            float tempSpeed = (1 / (Mathf.Abs(inputX) + Mathf.Abs(inputY)));
            velocity = new Vector3(
                inputX * tempSpeed * SpeedPlayer * buildingSpeed * Time.deltaTime,
                0,
                inputY * tempSpeed * SpeedPlayer * buildingSpeed * Time.deltaTime
            );
            transform.position += velocity;
        }
        else {
            buildingSpeed -= 0.1f;
            if (buildingSpeed < 0)
                buildingSpeed = 0;
        }
        transform.position += velocity * buildingSpeed;

    }
    void PushButton()
    {
        if (canDoAction)
        {
            if (Input.GetButton(PRESS_R + PlayerID))
            {
                leftTriggerHold.OnUpdate();
            }
            else if (Input.GetButtonUp(PRESS_R + PlayerID))
            {
                handAnimator.Play("Push");
                if (WwiseManager.isWwiseEnabled)
                    AkSoundEngine.PostEvent("MONK_WIND", gameObject);
                StartCoroutine(onCoolDown("Push"));
            }
                
        }
        onCharge();


    }
    void PullButton()
    {
        if (!canDoAction)
            return;
        if (Input.GetButtonDown(PRESS_L + PlayerID))
        {
            handAnimator.Play("Pull");
            StartCoroutine(onCoolDown("Pull"));

            if (WwiseManager.isWwiseEnabled)
                AkSoundEngine.PostEvent("MONK_WIND", gameObject);
        }
        
    }

    public void onPush()
    {
      
       OrbController.onPush(Quaternion.LookRotation(LookAtTransform.position - cursorTransform.transform.position) * -transform.up, this);
       OrbController.onChangeTeamPossession(currentTeam.TeamID);


        if (OrbController.CurrentVelocity > 300)
            UIEffectManager.OnFreezeFrame(OrbController.velocityRatio / 6);

        WindGust.GetComponent<BoxCollider>().enabled = false;
        if (WwiseManager.isWwiseEnabled)
            AkSoundEngine.PostEvent("MONK_PITCH", gameObject);
    }
    public void onPull()
    {
        if (WwiseManager.isWwiseEnabled)
            AkSoundEngine.PostEvent("MONK_CATCH", gameObject);
        pulledVelocity = OrbController.CurrentVelocity;
        OrbController.onPull(Vector3.zero, -OrbController.CurrentVelocity);
        OrbController.onChangeTeamPossession(currentTeam.TeamID);
    }



    void onChangeCoolDownState(string action, bool state)
    {
        switch (action)
        {
            case "Push":
                WindGust.SetActive(state);
                WindGust.GetComponent<BoxCollider>().enabled = state;
                break;
            case "Pull":
                PullCollider.enabled = state;
                break;
        }
    }

    IEnumerator onCoolDown(string action)
    {
        canDoAction = false;
        onChangeCoolDownState(action,true);
        yield return new WaitForSeconds(.2f);
        leftTriggerHold.OnReset();
        onChangeCoolDownState(action, false);
        yield return new WaitForSeconds(.4f);
        WindGust.SetActive(false);
        canDoAction = true;
    }
}