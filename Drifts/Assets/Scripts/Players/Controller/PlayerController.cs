using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Rewired;

public class PlayerController : MonoBehaviour
{
    ButtonHolder rightTriggerHold;
    public ButtonHolder RightTriggerHold { get { return rightTriggerHold; } }

    Team currentTeam;
    PlayerCursor cursor;
    public PlayerScript player;

	[Header("Component")]
    [SerializeField] Animator handAnimator;
    [SerializeField] BoxCollider PushCollider;
	[SerializeField] BoxCollider PullCollider;
	[SerializeField] GameObject WindGust;

	public Rigidbody rBody;

	SpriteRenderer sRenderer;
	BoxCollider PlayerCollider;

	[Header("Param")]
    private Color idleColor;
	[SerializeField] Color chargedColor;


	[Header("Text")]
    Text displayUI;
    public Text DisplayUI { get { return displayUI; } }


	#region private
	bool canDoAction = true;
	
	#endregion


    void Awake()
    {
        OnResetComponents();
        OnResetProperties();
    }

    void OnResetComponents()
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
        rightTriggerHold = new ButtonHolder();
        rBody.velocity = Vector3.zero;
        if (player != null)
            player.OnReset();

    }

	void Update()
    {
        if (!GameController.isGameStarted)
        {
            rBody.velocity = Vector3.zero;
            return;
        }
        if (!currentTeam.isStunt && !player.IsDead)
        {
            player.OnMove();
            PushButton();
            PullButton();
            OnDisplayUIButton();
            cursor.OnRotate();
            onPressPauseButton();
        }
    }

    void onPressPauseButton()
    {
        if (ReInput.players.GetPlayer(player.ID - 1).GetButtonDown("Pause"))
        PauseController.Instance.Pause();
    }

    public void onCharge()
    {
        Color currentChargingColor = Color.Lerp(idleColor, chargedColor, rightTriggerHold.holdingButtonRatio);
        currentChargingColor.a = 1;
        sRenderer.color = currentChargingColor;
    }

    public void onAssignTeam(Team assignedTeam)
    {
        currentTeam = assignedTeam;
    }

    bool isHitValid()
    {
		return currentTeam.TeamID != OrbController.Instance.PossessedTeam && OrbController.Instance.PossessedTeam != TeamController.teamID.Neutral;
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
          //  else
               // OrbController.onPush(cursor.LookingAtAngle * -transform.up, OrbController.CurrentVelocity / 1.5f);
        }
    }

    void PushButton()
    {
        if (player.ID >= ReInput.players.AllPlayers.Count)
            return;

        if (canDoAction)
        {
            if (ReInput.players.GetPlayer(player.ID -1 ).GetAxis("Push") >= 0.5f)
                rightTriggerHold.OnUpdate();
       
            else if (ReInput.players.GetPlayer(player.ID -1 ).GetAxisTimeInactive("Push") > 0.01f && rightTriggerHold.holdingButtonRatio > 0)
            {
                handAnimator.Play("Push");
                WwiseManager.PostEvent("MONK_WIND", gameObject);
                StartCoroutine(OnCoolDown("Push"));
			}
        }
        onCharge();
    }

    void PullButton()
    {
        if (!canDoAction)
            return;

        if (player.ID >= ReInput.players.AllPlayers.Count)
            return;

        if (ReInput.players.GetPlayer(player.ID -1).GetAxis("Stop") > 0.5f)
        {
            handAnimator.Play("Pull");
            StartCoroutine(OnCoolDown("Pull"));
            WwiseManager.PostEvent("MONK_WIND", gameObject);
        }

    }

    public void OnPush()
    {
		Debug.Log("on push");
		OrbController.Instance.Push(cursor.LookingAtAngle * -transform.up, player);
		OrbController.Instance.ChangeTeamPossession(currentTeam.TeamID);

		if (OrbController.Instance.CurrentVelocity > 300)
			UIEffectManager.OnFreezeFrame(OrbController.Instance.velocityRatio / 6);

        WindGust.GetComponent<BoxCollider>().enabled = false;
        WwiseManager.PostEvent("MONK_PITCH", gameObject);
    }

    public void OnPull()
    {
        WwiseManager.PostEvent("MONK_CATCH", gameObject);
		player.onSetPulledVelocity(OrbController.Instance.CurrentVelocity);
		OrbController.Instance.Pull(Vector3.zero, -OrbController.Instance.CurrentVelocity);
		OrbController.Instance.ChangeTeamPossession(currentTeam.TeamID);
    }

    void OnDisplayUIButton()
    {
        if (player.ID-1 >= ReInput.players.AllPlayers.Count)
        {
            displayUI.CrossFadeAlpha(0, 0.1f, false);
            return;
        }
	    float alpha = ReInput.players.GetPlayer(player.ID -1).GetButton("ShowUI") ? 1 : 0;
        displayUI.CrossFadeAlpha(alpha, 0.1f, false);
    }

    void OnChangeCoolDownState(string action, bool state)
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

    IEnumerator OnCoolDown(string action)
    {
        canDoAction = false;
        OnChangeCoolDownState(action, true);
        rightTriggerHold.OnReset();
        yield return new WaitForSeconds(.2f);
        OnChangeCoolDownState(action, false);
        yield return new WaitForSeconds(.4f);
        WindGust.SetActive(false);
        canDoAction = true;
    }
}