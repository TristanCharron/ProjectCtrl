using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UiManager : MonoBehaviour {

    public GameObject titleContainer;
    public GameObject playerIDcontainer;
    public List<GameObject> playerId = new List<GameObject>();
    
    int nbPlayers;

	// Use this for initialization
	void Start () {
        nbPlayers = 4;
        playerIDcontainer.SetActive(false);
        //iTween.MoveTo(gameObject, iTween.Hash("x", 3, "time", 4, "delay", 1, "onupdate", "myUpdateFunction", "looptype", iTween.LoopType.pingPong));

    }

    // Update is called once per frame
    void Update () {
	
        if(Input.GetMouseButtonDown(0))
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

       foreach(GameObject g in playerId)
        {
            g.GetComponent<Animator>().Play("playerIDAnimation");
        }
    }

    

   
}
