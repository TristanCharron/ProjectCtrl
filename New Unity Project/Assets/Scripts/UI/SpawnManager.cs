using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class SpawnManager : MonoBehaviour {

    public static SpawnManager Instance
    {
        get
        {
            return instance;
        }
    }
    private static SpawnManager instance;
    public GameObject gameOverContainer, monkReviveCharge;
    public Transform[] SpawnPoints;
    //public GameObject player;
    public GameObject[] _Players;
    private static GameObject[] players;
    public static GameObject[] Players { get { return players; } }
    public static bool IsTeamDead { get { return isTeamDead; } }
    private static bool isTeamDead;
    private static List<int> listPlayerDead = new List<int>();
    public static List<int> ListPlayerDead { get { return ListPlayerDead; } }
	[SerializeField] UiManager accesUI;
    // Use this for initialization
    void Start () {
		instance = this;
		OnResetProperties();
	}

	void OnResetProperties(){
		listPlayerDead = new List<int>();
		isTeamDead = false;
		players = _Players;

	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    static void onReset()
    {
		Debug.Log ("RESET2");

		instance.Reset ();
    }

	void Reset()
	{

		Debug.Log ("RESET");
		//SceneManager.LoadScene (0);
		//SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		SceneManager.LoadScene (0);
	}
    

    public static void onResetPosition()
    {
        foreach(GameObject player in players)
        {
            for(int x = 0; x < players.Length; x++)
            {
                if(!player.activeInHierarchy)
                player.SetActive(true);
                player.transform.position = player.transform.parent.position;
            }
      
        }

        instance.gameOverContainer.SetActive(false);
        if (IsTeamDead)
        {
            isTeamDead = false;
            instance.gameOverContainer.SetActive(false);
        }
            
    }


	IEnumerator TestTeamWins()
	{

		if(listPlayerDead.Contains(1) && listPlayerDead.Contains(2))
		{
			UiManager.isGameStarted = false;

			accesUI.GameOverContainer.SetActive (true);
			AkSoundEngine.PostEvent ("GAME_OVER", gameObject);

			yield return new WaitForSeconds (2f);

			accesUI.GameOverContainer.SetActive (false);
			accesUI.RedTeamWin.SetActive (true);
			AkSoundEngine.PostEvent ("GAME_END_RED", gameObject);

			Debug.Log("Red Team Wins");

			yield return new WaitForSeconds (5f);

			onReset();
		}
		else if (listPlayerDead.Contains(3) && listPlayerDead.Contains(4))
		{

			UiManager.isGameStarted = false;

			AkSoundEngine.PostEvent ("GAME_OVER", gameObject);
			accesUI.GameOverContainer.SetActive (true);

			yield return new WaitForSeconds (2f);
			accesUI.GameOverContainer.SetActive (false);
			accesUI.BlueTeamWin.SetActive (true);

			AkSoundEngine.PostEvent ("GAME_END_BLUE", gameObject);

			Debug.Log("Blue Team Wins");

			yield return new WaitForSeconds (5f);
			onReset();
		}


	

	}
   public void onPlayerDeath(int id)
    {
		AkSoundEngine.PostEvent ("MONK_DEAD", players [id - 1].gameObject);


        listPlayerDead.Add(id);
        players[id - 1].gameObject.SetActive(false);
		StartCoroutine (TestTeamWins ());


    }

    public void onReviveAlly()
    {

    }
}
