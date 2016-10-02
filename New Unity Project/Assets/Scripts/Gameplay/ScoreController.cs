using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreController : MonoBehaviour {
    private static ScoreController instance;
    public static ScoreController Instance { get { return instance; } }
    
    // Use this for initialization
    void Awake () {
        instance = this;
	}

    public static void onAddScore(Text teamScoreTxt, int newScore)
    {
        instance.StartCoroutine(instance.OnAnimateScore(teamScoreTxt, newScore));
    }


    public IEnumerator OnAnimateScore(Text teamScoreTxt, int newScore)
    {
        teamScoreTxt.text = newScore.ToString();
        teamScoreTxt.CrossFadeAlpha(0, 0.2f, false);
        yield return new WaitForSeconds(0.2f);
        teamScoreTxt.CrossFadeAlpha(1, 0.2f, false);
        yield break;
    }

}
