using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

    public GameObject titleContainer;
    public GameObject playerIDcontainer;
    public List<Text> playerId = new List<Text>();
    float duration = 1, alpha = 0, t;
    int nbPlayers;


	// Use this for initialization
	void Start () {
        
        nbPlayers = 4;
        playerIDcontainer.SetActive(false);
       
        //iTween.MoveTo(gameObject, iTween.Hash("x", 3, "time", 4, "delay", 1, "onupdate", "myUpdateFunction", "looptype", iTween.LoopType.pingPong));

    }

    // Update is called once per frame
    void Update () {
        

       

        
        if (Input.GetMouseButtonDown(0))
        {
            titleContainer.SetActive(false);
            gameObject.GetComponent<Animator>().enabled = true;
            StartCoroutine(makeIDAppear());
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
