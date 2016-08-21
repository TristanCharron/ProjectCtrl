using UnityEngine;
using System.Collections;

public class MonkController : MonoBehaviour
{
    public const string PRESS_L = "L_Press_";
    public const string PRESS_R = "R_Press_";

    [SerializeField]
    bool UsingController;

    //Player ID - Team

    public enum PlayerTeam
    {
        Neutral,
        Team1,
        Team2
    }

    ;

    public PlayerTeam Team;
    public int PlayerID;


    /// 
    Vector3 velocity;

    [SerializeField]
    float buildingSpeed = 0;
    [SerializeField]
    float SpeedPlayer = 10;
    [SerializeField]
    float coolDownLength = 10;
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
    bool isPushActionTriggered = false;
    [SerializeField]
    bool isPullActionTriggered = false;
    [SerializeField]
    bool isDead = false;

    [SerializeField]
    BoxCollider PushCollider;
    [SerializeField]
    BoxCollider PullCollider;


    [SerializeField]
    bool canDoAction = true;

	[SerializeField]
	GameObject WindGust;

	Rigidbody rBody;
	[SerializeField]SpawnManager accesSpawn;

    void Start()
    {
		PlayerCollider = GetComponent<BoxCollider> ();
		rBody = GetComponent<Rigidbody> ();
    }
    // Update is called once per frame
    void Update()
    {
		if (!UiManager.isGameStarted)
			return;


		if (!isDead)
        {
            CursorRotation();
            MoveCharacter();
            PushButton();
            PullButton();
        }
    }

    bool isHitValid()
    {
        return Team != ballManager.PossessedTeam && ballManager.PossessedTeam != PlayerTeam.Neutral;
    }


    IEnumerator onGoingThrough()
    {
		PlayerCollider.enabled = false;
		GetComponent<Rigidbody> ().isKinematic = true;
        yield return new WaitForSeconds(1.2f);
		GetComponent<Rigidbody> ().isKinematic = false;
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
					accesSpawn.onPlayerDeath(PlayerID);
                    //Destroy(gameObject);
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
            //transform.GetChild (0).localEulerAngles = new Vector3 (0, 0, Mathf.Lerp (startRotation.z, endRotation.z, cursor_t));
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


            //Child(0) == null du cursor
            startRotation = transform.GetChild(0).localEulerAngles;

            endRotation = new Vector3(0, 0, Mathf.Atan2(inputX, inputY) * 180 / Mathf.PI);




            /*


			//If rotation is broken

			if (endRotation.z + startRotation.z >= 180)
				endRotation = new Vector3 (0, 0,startRotation.z + 360);
			else if (endRotation.z - startRotation.z <= -180)
				endRotation = new Vector3 (0, 0,startRotation.z - 360);
			
			*/

            cursor_t = 0;
            //	transform.GetChild (0).localEulerAngles = new Vector3 (0, 0, Mathf.Atan2 (inputX, inputY) * 180 / Mathf.PI);
        }
    }
	IEnumerator OnLerpBackInertia()
	{
		float timer = 0;
		while(timer <= 1)
		{
			timer += Time.fixedDeltaTime;
			rBody.velocity = Vector3.Lerp(rBody.velocity, Vector3.zero, timer);
		}
		yield break;
	}
    void MoveCharacter()
    {
		/*
		float inputX = UsingController ? Input.GetAxis("Horizontal_Move_" + PlayerID) : Input.GetAxis("Horizontal");
		float inputY = UsingController ? Input.GetAxis("Vertical_Move_" + PlayerID) : Input.GetAxis("Vertical");
		Vector3 input = new Vector3(inputX, 0, inputY);

		if ((inputX > 0.5f || inputY > 0.5f) || (inputX < -0.5f || inputY < -0.5f)) 
		{
			

			if (Mathf.Abs (input.magnitude) > 0.25f) {
				rBody.AddForce (input.normalized * 200);
				rBody.velocity = Vector3.ClampMagnitude (rBody.velocity, 125);
			}

		}

		else	
			StartCoroutine(OnLerpBackInertia());

		/*
*/
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


            //newPosition.Normalize();
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
        if (!canDoAction)
            return;


        if (Input.GetButtonDown(PRESS_R + PlayerID))
        {
            handAnimator.Play("Push");
			AkSoundEngine.PostEvent ("MONK_WIND", gameObject);

            StartCoroutine(TimerActionCooldown("Push"));
        }
    }

    void PullButton()
    {

        if (!canDoAction)
            return;



        if (Input.GetButtonDown(PRESS_L + PlayerID))
        {
            handAnimator.Play("Pull");
            StartCoroutine(TimerActionCooldown("Pull"));
			AkSoundEngine.PostEvent ("MONK_WIND", gameObject);

        }




    }

    public void onTriggerPush(bool state)
    {
        isPushActionTriggered = state;
    }

    public void onTriggerPull(bool state)
    {
        isPullActionTriggered = state;
    }

    public void onPush()
    {
        Debug.Log("tappe la balle");
        ballManager.onPush(Quaternion.LookRotation(LookAtTransform.position - cursorTransform.transform.position) * -transform.up);
        ballManager.onChangeTeamPossession(Team);
		UiManager.OnFreezeFrame (0 + ((ballManager.CurrentVelocity / ballManager.MaxVelocity) / 2) );
		UiManager.OnScreenShake(0 + ((ballManager.CurrentVelocity / ballManager.MaxVelocity) / 2) );

		WindGust.GetComponent<BoxCollider>().enabled = false;
		AkSoundEngine.PostEvent ("MONK_PITCH", gameObject);

    }

    public void onPull()
    {
		AkSoundEngine.PostEvent ("MONK_CATCH", gameObject);

        ballManager.onPull(Vector3.zero, -ballManager.CurrentVelocity);
        ballManager.onChangeTeamPossession(Team);
    }


	IEnumerator TimerActionCooldown(string action)
	{
		canDoAction = false;

		switch (action)
		{
		case "Push":
			WindGust.SetActive(true);
			WindGust.GetComponent<BoxCollider>().enabled = true;
			//yield return new WaitForSeconds (0.1f);
			/*if (isPushActionTriggered)
                    onPush ();*/
			break;
		case "Pull":
			PullCollider.enabled = true;
			//yield return new WaitForSeconds (0.1f);
			/*if (isPullActionTriggered)
                    onPull ();*/
			break;
		}

		yield return new WaitForSeconds(.2f);



		switch (action)
		{
		case "Push":
			WindGust.GetComponent<BoxCollider>().enabled = false;
			break;
		case "Pull":
			PullCollider.enabled = false;
			break;
		}

		yield return new WaitForSeconds(.4f);
		WindGust.SetActive(false);

		canDoAction = true;
		//isPushActionTriggered = false;
		//isPullActionTriggered = false;


	}
}
