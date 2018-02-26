using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Rewired;

public class PlayerController : MonoBehaviour
{

    public ButtonHolder RightTriggerHold { private set; get; }

    public TeamController.TeamID CurrentTeamID { get { return Player.TeamID; } }

    PlayerCursor Cursor;

    public PlayerScript Player;

	[Header("Component")]
    [SerializeField] Animator handAnimator;
	[SerializeField] Animator pushAnimator;
    [SerializeField] BoxCollider PushCollider;
	[SerializeField] BoxCollider PullCollider;
	[SerializeField] GameObject WindGust;
    [SerializeField] ParticleSystem chargeParticles, catchParticles;
	public Rigidbody rBody;

	SpriteRenderer sRenderer;
	BoxCollider PlayerCollider;

	[Header("Param")]
    private Color idleColor;
	[SerializeField] Color chargedColor;


	[Header("Text")]
    public Text DisplayUI;


	#region private
	bool canDoAction = true;
	
	#endregion


    void Awake()
    {
        ResetComponents();
        ResetProperties();
    }

    void ResetComponents()
    {
        PlayerCollider = GetComponent<BoxCollider>();
        rBody = GetComponent<Rigidbody>();
        Cursor = GetComponent<PlayerCursor>();
        sRenderer = GetComponent<SpriteRenderer>();
        DisplayUI = GetComponentInChildren<Text>();
        idleColor = sRenderer.color;
    }

    public void ResetProperties()
    {
        sRenderer.color = idleColor;
        canDoAction = true;
        RightTriggerHold = new ButtonHolder();
        rBody.velocity = Vector3.zero;
        rBody.mass = 10;
        rBody.drag = 5f;
        if (Player != null)
            Player.ResetCharacter();
    }



	void Update()
    {
        if (!GameController.Instance.IsGameStarted)
        {
            rBody.velocity = Vector3.zero;
            return;
        }
        if (!Player.IsDead)
        {
            Player.Move();
            PushButton();
            PullButton();
            DisplayUIButton();
            Cursor.OnRotate();
            PressPauseButton();
        }
    }

    void PressPauseButton()
    {
        if (ReInput.players.GetPlayer(Player.ID).GetButtonDown("Pause"))
        PauseController.Instance.Pause();
    }

    public void Charge()
    {
        Color currentChargingColor = Color.Lerp(idleColor, chargedColor, RightTriggerHold.holdingButtonRatio);
        currentChargingColor.a = 1;
        sRenderer.color = currentChargingColor;
    }

    bool isHitValid()
    {
		return CurrentTeamID != OrbController.Instance.PossessedTeam && 
            OrbController.Instance.PossessedTeam != TeamController.TeamID.Neutral;
    }


    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Orb"))
        {
			if (isHitValid() && !Player.IsDead)
            {
				WwiseManager.PostEvent("MONK_DEAD", gameObject);
                GameObject DeathAnimParticle = Instantiate(Resources.Load<GameObject>("DeathMonkParticle"), gameObject.transform.position, Quaternion.identity) as GameObject;
                Destroy(DeathAnimParticle, 5);

				float deathShakeRatio = 5;
				UIEffectManager.Instance.OnFreezeFrame(0.3f);
				GameEffect.Shake(Camera.main.gameObject,deathShakeRatio,.5f);

                GameController.Instance.KillPlayer(this);
				gameObject.SetActive(false);
            }
        }
    }

    void PushButton()
    {
        if (Player.ID >= ReInput.players.AllPlayers.Count)
            return;

        if (canDoAction)
        {
            if (ReInput.players.GetPlayer(Player.ID).GetAxis("Push") >= 0.5f)
                RightTriggerHold.OnUpdate();
       
            else if (ReInput.players.GetPlayer(Player.ID).GetAxisTimeInactive("Push") > 0.01f && RightTriggerHold.holdingButtonRatio > 0)
            {
                handAnimator.Play("Push");
                WwiseManager.PostEvent("MONK_WIND", gameObject);
                StartCoroutine(CoolDown("Push"));
			}
        }
        Charge();
    }

    void PullButton()
    {
        if (!canDoAction)
            return;

        if (Player.ID >= ReInput.players.AllPlayers.Count)
            return;

        if (ReInput.players.GetPlayer(Player.ID).GetAxis("Stop") > 0.5f)
        {
            handAnimator.Play("Pull");
            StartCoroutine(CoolDown("Pull"));
            WwiseManager.PostEvent("MONK_WIND", gameObject);
        }

    }

    public void OnPush()
    {
		float currentPower = Player.Power * (1 + RightTriggerHold.holdingButtonRatio);
		OrbController.Instance.Push(Cursor.LookingAtAngle * -transform.up, Player.Power, CurrentTeamID);
		OrbController.Instance.ChangeTeamPossession(CurrentTeamID);
		PushEffects();
        WindGust.GetComponent<BoxCollider>().enabled = false;
        WwiseManager.PostEvent("MONK_PITCH", gameObject);
    }

	void PushEffects()
	{
		float velRatio = OrbController.Instance.VelocityRatio;
		if (velRatio > 0.3f)
		{	
			float shakeEffect = 2;
			UIEffectManager.Instance.OnFreezeFrame(velRatio * 0.6f);
			GameEffect.Shake(Camera.main.gameObject,velRatio * shakeEffect, velRatio * 0.6f);
		}
	}
    public void OnPull()
    {
        WwiseManager.PostEvent("MONK_CATCH", gameObject);
		OrbController.Instance.Pull(CurrentTeamID);
    }

    void DisplayUIButton()
    {
        if (Player.ID >= ReInput.players.AllPlayers.Count)
        {
            DisplayUI.CrossFadeAlpha(0, 0.2f, false);
            return;
        }
	    float alpha = ReInput.players.GetPlayer(Player.ID).GetButton("ShowUI") ? 1 : 0;
        DisplayUI.CrossFadeAlpha(alpha, 0.2f, false);
    }

    void ChangeCoolDownState(string action, bool state)
    {
        switch (action)
        {
            case "Push":
                WindGust.SetActive(state);
                WindGust.GetComponent<BoxCollider>().enabled = state;
				if(state)
					pushAnimator.Play ("pushAnim");

                break;
            case "Pull":
                PullCollider.enabled = state;
                break;
        }
    }

    IEnumerator CoolDown(string action)
    {
        canDoAction = false;
        ChangeCoolDownState(action, true);
        RightTriggerHold.OnReset();
        yield return new WaitForSeconds(.2f);
        ChangeCoolDownState(action, false);
        yield return new WaitForSeconds(.2f);
        WindGust.SetActive(false);
        canDoAction = true;
    }
}