using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {
	public static UiManager Instance { get {return instance; }} 
	private static UiManager instance;
    public GameObject titleContainer, startContainer;
    public GameObject playerIDcontainer;
    public List<Text> playerId = new List<Text>();
    float duration = 1, alpha = 0, t;
    int nbPlayers;
    bool startGame;



	public static void OnFreezeFrame(float sec)
	{
		instance.StartCoroutine (instance.FreezeFrame (sec));

	}
	// Use this for initialization
	void Awake () {
		instance = this;
        nbPlayers = 4;
        playerIDcontainer.SetActive(false);
        startContainer.SetActive(false);
        //iTween.MoveTo(gameObject, iTween.Hash("x", 3, "time", 4, "delay", 1, "onupdate", "myUpdateFunction", "looptype", iTween.LoopType.pingPong));

    }

	public IEnumerator FreezeFrame(float sec)
	{
		Time.timeScale = 0.01f;
		float pauseEndTime = Time.realtimeSinceStartup + sec;
		while (Time.realtimeSinceStartup < pauseEndTime)
			yield return 0;
				
		Time.timeScale = 1;

	}

    // Update is called once per frame
    void Update () {
        

       

        
        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("L_Press_1") && startGame == false)
        {
            titleContainer.SetActive(false);
            gameObject.GetComponent<Animator>().enabled = true;
            StartCoroutine(makeIDAppear());
            startGame = true;
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
        startContainer.SetActive(true);
        yield return new WaitForSeconds(1f);
        startContainer.SetActive(false);
    }

   /* void chargeBarActive()
    {
        
        chargeBar.SetActive(true);
        chargeBar.transform.localPosition = new Vector3( Mathf.Lerp(-0.255f, 0.2569f, t ) , chargeBar.transform.localPosition.y, chargeBar.transform.localPosition.z);
    }
    **/


    float lerp()
    {
        return Mathf.PingPong(Time.time, duration) / duration;
    }
    

   
}
