using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {
	public static UiManager Instance { get {return instance; }} 
	public static bool IsFreezeFraming  { get {return isFreezeFraming; }} 
	private static bool isFreezeFraming = false, isScreenShake = false;
	private static UiManager instance;
	public GameObject titleContainer, startContainer,readyContainer,GameOverContainer,BlueTeamWin,RedTeamWin;
    public GameObject playerIDcontainer;
	public Animator FadeToWhite;
    public List<Text> playerId = new List<Text>();
	public static Vector3 ShakeAmount { get {return shakeAmount; }} 
	private static Vector3 shakeAmount;
    float duration = 1, alpha = 0, t;
    int nbPlayers;
    bool startGame;

	public static bool isGameStarted;

	[SerializeField]
	public GameObject theBall;
	[SerializeField]
	public Transform[] nullSpawnBall;
	public Transform Everything;
	public GameObject Sakuras;
	public static void OnFreezeFrame(float sec,float power)
	{
		instance.StartCoroutine (instance.FreezeFrame (sec,power));

	}

	public static void OnScreenShake(float sec)
	{
		instance.StartCoroutine (instance.ScreenShake (sec));

	}


	// Use this for initialization
	void Start () {
		instance = this;
        nbPlayers = 4;
        playerIDcontainer.SetActive(false);
        // startContainer.SetActive(false);
        //iTween.MoveTo(gameObject, iTween.Hash("x", 3, "time", 4, "delay", 1, "onupdate", "myUpdateFunction", "looptype", iTween.LoopType.pingPong));
        if (WwiseManager.isWwiseEnabled)
            AkSoundEngine.PostEvent ("GAME_OPEN", gameObject);
		Debug.Log ("game open");

    }

	void OnResetProperties(){
		isFreezeFraming = false;
		isScreenShake = false;
		isGameStarted = false;
	}

	public IEnumerator FreezeFrame(float sec, float Power)
	{
		if(!isFreezeFraming)
		{
		isFreezeFraming = true;
		Time.timeScale = 0.01f;
		float pauseEndTime = Time.realtimeSinceStartup + sec;
		while (Time.realtimeSinceStartup < pauseEndTime)
			yield return 0;
		Time.timeScale = 1;
		isFreezeFraming = false;
		}

		Camera.main.GetComponentInParent<Shake> ().enabled = true;
		Camera.main.GetComponentInParent<Shake> ().Power = Power;

		yield break;
	}

	public IEnumerator ScreenShake(float sec)
	{


		if(!isScreenShake)
		{
			isScreenShake = true;
			float elapsed = 0.0f;

			Vector3 originalCamPos = Camera.main.transform.position;

			while (elapsed < sec * 2) 
			{
				elapsed += Time.deltaTime;
				shakeAmount = originalCamPos + Random.insideUnitSphere * 50f;

			}
			shakeAmount = Vector3.zero;
			isScreenShake = false;
		}
		yield break;
	}

    // Update is called once per frame
    void Update () {
        


       
	}
	public void EndCinematic()
	{
	
		Sakuras.SetActive (false);
		GetComponent<cameraBoxScriptTry> ().enabled = true;
		SpawnBall ();

	//	AkSoundEngine.PostEvent ("GAME_START", gameObject);

		StartCoroutine (onStartGame2 ());

	}

	IEnumerator onStartGame2()
	{
		readyContainer.SetActive (true);
		yield return new WaitForSeconds (3.2f);
		readyContainer.SetActive (false);
		isGameStarted = true;
		startGame = true;

	}


	public void SpawnBall()
	{
		theBall.SetActive (true);
		int random = Random.Range (0, 2);
		switch (random)
		{
		case 0:
			theBall.transform.position = nullSpawnBall [0].position;
			break;

		case 1:
			theBall.transform.position = nullSpawnBall [1].position;
			break;
		}

	}


    IEnumerator makeIDAppear()
    {
        yield return new WaitForSeconds(6f);
        playerIDcontainer.SetActive(true);

         foreach(Text g in playerId)
        {
           g.CrossFadeAlpha(1.0f, 2.0f, false);
            g.CrossFadeAlpha(0.0f, 2.0f, false);
        }
         
        yield break;
    }
	public void changeUI()
	{

	}

	public void onStartGame()
	{
		Sakuras.SetActive (true);

        if (WwiseManager.isWwiseEnabled)
            AkSoundEngine.PostEvent ("UI_SELECT", gameObject);

        if (WwiseManager.isWwiseEnabled)
            AkSoundEngine.PostEvent ("GAME_PLAY", gameObject);

		gameObject.GetComponent<Animator>().enabled = true;
		FadeToWhite.enabled = true;
		StartCoroutine(makeIDAppear());
	}

    float lerp()
    {
        return Mathf.PingPong(Time.time, duration) / duration;
    }
    

   
}
