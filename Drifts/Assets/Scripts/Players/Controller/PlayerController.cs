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
        if (!GameController.IsGameStarted)
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
        if (other.gameObject.tag == "Orb")
        {

            if (isHitValid())
            {
                if (!Player.IsDead)
                {
                    GameObject DeathAnimParticle = Instantiate(Resources.Load<GameObject>("DeathMonkParticle"), gameObject.transform.position, Quaternion.identity) as GameObject;
                    Destroy(DeathAnimParticle, 5);
                    GameController.KillPlayer(this);

                }
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
		Debug.Log("on push");
		OrbController.Instance.Push(Cursor.LookingAtAngle * -transform.up, Player);
		OrbController.Instance.ChangeTeamPossession(CurrentTeamID);

		if (OrbController.Instance.CurrentVelocity > 300)
			UIEffectManager.Instance.FreezeFrame(OrbController.Instance.VelocityRatio / 6);

        WindGust.GetComponent<BoxCollider>().enabled = false;
        WwiseManager.PostEvent("MONK_PITCH", gameObject);
    }

    public void OnPull()
    {
        WwiseManager.PostEvent("MONK_CATCH", gameObject);
		Player.SetPulledVelocity(OrbController.Instance.CurrentVelocity);
		OrbController.Instance.Pull(Vector3.zero, -OrbController.Instance.CurrentVelocity);
		OrbController.Instance.ChangeTeamPossession(CurrentTeamID);
    }

    void DisplayUIButton()
    {
        if (Player.ID >= ReInput.players.AllPlayers.Count)
        {
            DisplayUI.CrossFadeAlpha(0, 0.1f, false);
            return;
        }
	    float alpha = ReInput.players.GetPlayer(Player.ID).GetButton("ShowUI") ? 1 : 0;
        DisplayUI.CrossFadeAlpha(alpha, 0.1f, false);
    }

    void ChangeCoolDownState(string action, bool state)
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

    IEnumerator CoolDown(string action)
    {
        canDoAction = false;
        ChangeCoolDownState(action, true);
        RightTriggerHold.OnReset();
        yield return new WaitForSeconds(.2f);
        ChangeCoolDownState(action, false);
        yield return new WaitForSeconds(.4f);
        WindGust.SetActive(false);
        canDoAction = true;
    }
}