using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ButtonHolder
{

    public float currentHoldPushBtnTime = 0f;
    public const float maxHoldPushBtnTime = 1f;
    public float holdingButtonRatio { get { return Mathf.Clamp01(currentHoldPushBtnTime / maxHoldPushBtnTime); } }

    public ButtonHolder()
    {
        currentHoldPushBtnTime = 0;
    }

    public void OnUpdate()
    {
        if (GameController.isGameStarted)
            currentHoldPushBtnTime = currentHoldPushBtnTime > maxHoldPushBtnTime ? 0 : currentHoldPushBtnTime += Time.fixedDeltaTime;
        else
            currentHoldPushBtnTime = 0;
    }

    public void OnReset()
    {
        currentHoldPushBtnTime = 0;
    }


}


public class PlayerController : MonoBehaviour
{


    private ButtonHolder leftTriggerHold, rightTriggerHold;

    public ButtonHolder LeftTriggerHold { get { return leftTriggerHold; } }
    public ButtonHolder RightTriggerHold { get { return rightTriggerHold; } }

    private Quaternion LookingAtAngle { get { return Quaternion.LookRotation(LookAtTransform.position - cursorTransform.transform.position); } }


    Team currentTeam;

    //Player ID - Team
    public int PlayerID;

    float SpeedPlayer = 55;


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

    float acceleration = 0;

    [SerializeField]
    bool isDead = false;
    [SerializeField]
    BoxCollider PushCollider;


    private float pulledVelocity = 0;
    public float PulledVelocity { get { return pulledVelocity; } }
    public void onSetPulledVelocity(float vel) { pulledVelocity = vel; }

    [SerializeField]
    BoxCollider PullCollider;
    bool canDoAction = true;
    [SerializeField]
    GameObject WindGust;
    Rigidbody rBody;

    private bool changingOrbAngle = false;
    public bool isChangingOrbAngle { get { return changingOrbAngle; } }



    SpriteRenderer sRenderer;

    private Color idleColor;

    [SerializeField]
    Color chargedColor;


    Text displayUI;
    public Text DisplayUI { get { return displayUI; } }





    void Awake()
    {
        onResetComponents();
        OnResetProperties();
    }

    void onResetComponents()
    {
        PlayerCollider = GetComponent<BoxCollider>();
        rBody = GetComponent<Rigidbody>();
        sRenderer = GetComponent<SpriteRenderer>();
        displayUI = GetComponentInChildren<Text>();
        idleColor = sRenderer.color;
    }

    public void OnResetProperties()
    {
        sRenderer.color = idleColor;
        pulledVelocity = 0;
        isDead = false;
        canDoAction = true;
        leftTriggerHold = new ButtonHolder();
        changingOrbAngle = false;
        rBody.velocity = Vector3.zero;
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
            onDisplayUIButton();
            CursorRotation();
            onPressPauseButton();


        }

    }


    void onPressPauseButton()
    {
        if (Input.GetButtonDown(InputController.PRESS_START + PlayerID))
            PauseController.OnPause();
    }


    void onChangeBallAngle()
    {
        if (changingOrbAngle)
        {
            Vector3 newAngle = Quaternion.LookRotation(LookAtTransform.position - cursorTransform.transform.position) * -transform.up;
            OrbController.onChangeAngle(newAngle);
        }

    }

    public void onCharge()
    {
        Color currentChargingColor = Color.Lerp(idleColor, chargedColor, leftTriggerHold.holdingButtonRatio);
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
                    RoundController.onPlayerDeath(PlayerID);

                }
            }
            else
                OrbController.onPush(LookingAtAngle * -transform.up, OrbController.CurrentVelocity / 2.5f);



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

        float inputX = Input.GetAxis("Horizontal_Rotation_" + PlayerID);
        float inputY = Input.GetAxis("Vertical_Rotation_" + PlayerID);

        if ((inputX > 0.5f || inputY > 0.5f) || (inputX < -0.5f || inputY < -0.5f))
        {
            startRotation = transform.GetChild(0).localEulerAngles;
            endRotation = new Vector3(0, 0, Mathf.Atan2(inputX, inputY) * 180 / Mathf.PI);
            cursor_t = 0;
        }
    }

    void MoveCharacter()
    {
        float inputX = Input.GetAxis("Horizontal_Move_" + PlayerID);
        float inputY = Input.GetAxis("Vertical_Move_" + PlayerID);
        rBody.velocity = Vector3.zero;
        bool isUsingInput = (inputX > 0.5f || inputY > 0.5f) || (inputX < -0.5f || inputY < -0.5f);
        acceleration = isUsingInput ? acceleration += (Time.deltaTime * 3) : acceleration -= (Time.deltaTime * 30);
        acceleration = Mathf.Clamp01(acceleration);
        Vector3 offset = new Vector3(inputX * SpeedPlayer * Time.deltaTime, 0, inputY * SpeedPlayer * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, transform.position + offset, acceleration);
    }
void PushButton()
{
    if (canDoAction)
    {
        if (Input.GetAxis(InputController.PRESS_R + PlayerID) >= 0.5f)
            leftTriggerHold.OnUpdate();
        else if (Input.GetAxis(InputController.PRESS_R + PlayerID) <= 0.25f && leftTriggerHold.holdingButtonRatio > 0)
        {
            handAnimator.Play("Push");
            WwiseManager.onPlayWWiseEvent("MONK_WIND", gameObject);
            StartCoroutine(onCoolDown("Push"));

        }

    }

    onCharge();


}
void PullButton()
{
    if (!canDoAction)
        return;

    if (Input.GetAxis(InputController.PRESS_L + PlayerID) > 0.5f)
    {
        handAnimator.Play("Pull");
        StartCoroutine(onCoolDown("Pull"));
        WwiseManager.onPlayWWiseEvent("MONK_WIND", gameObject);
    }

}

public void onPush()
{

    OrbController.onPush(LookingAtAngle * -transform.up, this);
    OrbController.onChangeTeamPossession(currentTeam.TeamID);


    if (OrbController.CurrentVelocity > 300)
        UIEffectManager.OnFreezeFrame(OrbController.velocityRatio / 6);

    WindGust.GetComponent<BoxCollider>().enabled = false;
    WwiseManager.onPlayWWiseEvent("MONK_PITCH", gameObject);
}

public void onPull()
{
    WwiseManager.onPlayWWiseEvent("MONK_CATCH", gameObject);
    pulledVelocity = OrbController.CurrentVelocity;
    OrbController.onPull(Vector3.zero, -OrbController.CurrentVelocity);
    OrbController.onChangeTeamPossession(currentTeam.TeamID);
}

void onDisplayUIButton()
{
    float alpha = Input.GetButton(InputController.PRESS_Y + PlayerID) ? 1 : 0;
    displayUI.CrossFadeAlpha(alpha, 0.1f, false);
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
    onChangeCoolDownState(action, true);
    yield return new WaitForSeconds(.2f);
    leftTriggerHold.OnReset();
    onChangeCoolDownState(action, false);
    yield return new WaitForSeconds(.4f);
    WindGust.SetActive(false);
    canDoAction = true;
}


void onChangingOrbAngleState(bool state)
{
    changingOrbAngle = state;

}


void OnMouseDown()
{
    if (!isDead)
    {
        GameObject DeathAnimParticle = Instantiate(Resources.Load<GameObject>("DeathMonkParticle"), gameObject.transform.position, Quaternion.identity) as GameObject;
        Destroy(DeathAnimParticle, 5);
        RoundController.onPlayerDeath(PlayerID);

    }

}
}