using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{


    ButtonHolder leftTriggerHold, rightTriggerHold;
    public ButtonHolder LeftTriggerHold { get { return leftTriggerHold; } }
    public ButtonHolder RightTriggerHold { get { return rightTriggerHold; } }
    
	Team currentTeam;
    PlayerCursor cursor;
    public Player player;

    BoxCollider PlayerCollider;

    [SerializeField]
    Animator handAnimator;

    [SerializeField]
    BoxCollider PushCollider;



    [SerializeField]
    BoxCollider PullCollider;
    bool canDoAction = true;
    [SerializeField]
    GameObject WindGust;
    public Rigidbody rBody;


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
        cursor = GetComponent<PlayerCursor>();
        sRenderer = GetComponent<SpriteRenderer>();
        displayUI = GetComponentInChildren<Text>();
        idleColor = sRenderer.color;
     
    }

    public void OnResetProperties()
    {
        sRenderer.color = idleColor;
        canDoAction = true;
        leftTriggerHold = new ButtonHolder();
        rBody.velocity = Vector3.zero;
        if(player != null)
            player.OnReset();

    }
    // Update is called once per frame
    void Update()
    {
        if (!GameController.isGameStarted)
            return;


        if (!currentTeam.isStunt && !player.IsDead)
        {
            player.OnMove();
            PushButton();
            PullButton();
            onDisplayUIButton();
            cursor.OnRotate();
            onPressPauseButton();


        }

    }


    void onPressPauseButton()
    {
        if (Input.GetButtonDown(InputController.PRESS_START + player.ID))
            PauseController.OnPause();
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
                if (!player.IsDead)
                {
                    GameObject DeathAnimParticle = Instantiate(Resources.Load<GameObject>("DeathMonkParticle"), gameObject.transform.position, Quaternion.identity) as GameObject;
                    Destroy(DeathAnimParticle, 5);
                    RoundController.onPlayerDeath(player.ID);

                }
            }
            else
                OrbController.onPush(cursor.LookingAtAngle * -transform.up, OrbController.CurrentVelocity / 2.5f);



        }
    }


   

   
void PushButton()
{
    if (canDoAction)
    {
        if (Input.GetAxis(InputController.PRESS_R + player.ID) >= 0.5f)
            leftTriggerHold.OnUpdate();
        else if (Input.GetAxis(InputController.PRESS_R + player.ID) <= 0.25f && leftTriggerHold.holdingButtonRatio > 0)
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

    if (Input.GetAxis(InputController.PRESS_L + player.ID) > 0.5f)
    {
        handAnimator.Play("Pull");
        StartCoroutine(onCoolDown("Pull"));
        WwiseManager.onPlayWWiseEvent("MONK_WIND", gameObject);
    }

}

public void onPush()
{

    OrbController.onPush(cursor.LookingAtAngle * -transform.up, player);
    OrbController.onChangeTeamPossession(currentTeam.TeamID);


    if (OrbController.CurrentVelocity > 300)
        UIEffectManager.OnFreezeFrame(OrbController.velocityRatio / 6);

    WindGust.GetComponent<BoxCollider>().enabled = false;
    WwiseManager.onPlayWWiseEvent("MONK_PITCH", gameObject);
}

public void onPull()
{
    WwiseManager.onPlayWWiseEvent("MONK_CATCH", gameObject);
    player.onSetPulledVelocity(OrbController.CurrentVelocity);
    OrbController.onPull(Vector3.zero, -OrbController.CurrentVelocity);
    OrbController.onChangeTeamPossession(currentTeam.TeamID);
}

void onDisplayUIButton()
{
    float alpha = Input.GetButton(InputController.PRESS_Y + player.ID) ? 1 : 0;
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





}