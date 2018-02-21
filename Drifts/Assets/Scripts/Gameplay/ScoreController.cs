using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreController : MonoBehaviour {


    public static ScoreController Instance { private set; get; }

   
    // Use this for initialization
    void Awake () {
        Instance = this;
        GameController.EndRoundEvent += AddTeamScores;

    }

    public static void AddScore(Text teamScoreTxt, int newScore)
    {
        Instance.StartCoroutine(Instance.AnimateScore(teamScoreTxt, newScore));
    }


    public IEnumerator AnimateScore(Text teamScoreTxt, int newScore)
    {
        teamScoreTxt.text = ReturnFinalNumber(newScore);
        teamScoreTxt.CrossFadeAlpha(0, 0.2f, false);
        yield return new WaitForSeconds(0.2f);
        teamScoreTxt.CrossFadeAlpha(1, 0.2f, false);
        yield break;
    }

    public string ReturnFinalNumber(int num)
    {
        if (num > 9)
            return num.ToString();
        else
            return ("0" + num.ToString());
    }

    public static void AddTeamScores()
    {
        if(GameController.NbRoundsPlayed > 0)
        {
            for (int i = 0; i < TeamController.TeamList.Count; i++)
            {
                TeamController.TeamList[i].AddRoundScore(TeamController.TeamList[i].CurrentScore);
            }
        }
      
    }

}
