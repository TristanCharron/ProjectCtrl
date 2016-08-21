using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {
	public static UiManager Instance { get {return instance; }} 
	public static bool IsFreezeFraming  { get {return isFreezeFraming; }} 
	private static bool isFreezeFraming = false, isScreenShake = false;
	private static UiManager instance;
    public GameObject titleContainer, startContainer;
    public GameObject playerIDcontainer;
	public Animator FadeToWhite;
    public List<Text> playerId = new List<Text>();
    float duration = 1, alpha = 0, t;
    int nbPlayers;
    bool startGame;

	public static bool isGameStarted;

	[SerializeField]
	public GameObject theBall;
	[SerializeField]
	public Transform[] nullSpawnBall;


	public static void OnFreezeFrame(float sec)
	{
		instance.StartCoroutine (instance.FreezeFrame (sec));

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
		AkSoundEngine.PostEvent ("GAME_OPEN", gameObject);

    }

	public IEnumerator FreezeFrame(float sec)
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
		yield break;
	}

	public IEnumerator ScreenShake(float sec)
	{


		if(!isScreenShake)
		{
			isScreenShake = true;
			float elapsed = 0.0f;

			Vector3 originalCamPos = Camera.main.transform.position;

			while (elapsed < sec) {

				elapsed += Time.deltaTime;          

				float percentComplete = elapsed / sec;         
				float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

				// map value to [-1, 1]
				float x = Random.value * 2.0f - 1.0f;
				float y = Random.value * 2.0f - 1.0f;
				x *= 1 * damper;
				y *= 1 * damper;

				Camera.main.transform.position = new Vector3(x, y, originalCamPos.z);

				yield return null;
			}
			isScreenShake = false;
		}
		yield break;
	}

    // Update is called once per frame
    void Update () {
        


       
	}
	public void EndCinematic()
	{
		UiManager.isGameStarted = true;
		GetComponent<cameraBoxScriptTry> ().enabled = true;
		SpawnBall ();
		AkSoundEngine.PostEvent ("GAME_START", gameObject);

	}

	public void SpawnBall()
	{
 		//Animation ball spawn
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
          /*  float cool = lerp();
            g.canvasRenderer.SetAlpha(cool);
            */
           g.CrossFadeAlpha(1.0f, 2.0f, false);
            g.CrossFadeAlpha(0.0f, 2.0f, false);
        }
        yield return new WaitForSeconds(3f);
       
		//startContainer.SetActive(true);
        yield return new WaitForSeconds(1f);
        
		//startContainer.SetActive(false);
    }

   /* void chargeBarActive()
    {
        
        chargeBar.SetActive(true);
        chargeBar.transform.localPosition = new Vector3( Mathf.Lerp(-0.255f, 0.2569f, t ) , chargeBar.transform.localPosition.y, chargeBar.transform.localPosition.z);
    }
    **/
	public void onStartGame()
	{
		AkSoundEngine.PostEvent ("GAME_PLAY", gameObject);
		//titleContainer.SetActive(false);
		gameObject.GetComponent<Animator>().enabled = true;
		FadeToWhite.enabled = true;
		StartCoroutine(makeIDAppear());
		startGame = true;

	}

    float lerp()
    {
        return Mathf.PingPong(Time.time, duration) / duration;
    }
    

   
}
