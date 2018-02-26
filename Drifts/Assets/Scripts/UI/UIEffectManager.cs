using UnityEngine;
using System.Collections;

public class UIEffectManager : MonoBehaviour {

    public static UIEffectManager Instance { get; private set; }

    [SerializeField]
    private bool isFreezeFraming = false, 
    isScreenShake = false;

    [SerializeField]
    private Vector3 shakeAmount;

    private Shake shakeComponent;

    [SerializeField] GameObject FadeWhiteComponent;
    private Animator fadeWhiteAnimator;

    // Use this for initialization
    void Awake () {
        Instance = this;
        shakeComponent = Camera.main.GetComponentInParent<Shake>();
        fadeWhiteAnimator = FadeWhiteComponent.GetComponent<Animator>();
        OnResetProperties();
    }

    public  void OnResetProperties()
    {
        isFreezeFraming = false;
        isScreenShake = false;
        shakeAmount = Vector3.zero;
    }


    public void OnFreezeFrame(float sec)
    {
        StartCoroutine(Instance.FreezeFrame(sec));

    }

    public void OnScreenShake(float sec)
    {
		StartCoroutine(Instance.ScreenShake(sec));

    }

	public IEnumerator FreezeFrame(float sec)
    {
        if (!isFreezeFraming)
        {
            isFreezeFraming = true;
            Time.timeScale = 0.001f;
            float pauseEndTime = Time.realtimeSinceStartup + sec;

            while (Time.realtimeSinceStartup < pauseEndTime)
                yield return 0;

            Time.timeScale = 1;
            isFreezeFraming = false;
        }

        shakeComponent.enabled = true;
        yield break;
    }

	public IEnumerator ScreenShake(float sec)
    {


        if (!isScreenShake)
        {
            isScreenShake = true;
            float elapsed = 0.0f;

            Vector3 originalCamPos = Camera.main.transform.position;

            while (elapsed < sec * 2)
            {
                elapsed += Time.deltaTime;
				shakeAmount = originalCamPos + Random.insideUnitSphere * 50;

            }

            shakeAmount = Vector3.zero;
            isScreenShake = false;
        }
        yield break;
    }

    public void FadeToWhite(bool fadeIn)
    {
        StartCoroutine(FadeWhite(fadeIn));
       
    
    }

    public IEnumerator FadeWhite(bool fadeIn)
    {

       
        FadeWhiteComponent.SetActive(true);
        fadeWhiteAnimator.enabled = true;
        fadeWhiteAnimator.Play(Animator.StringToHash(fadeIn ? "fadeInWhite" : "fadeOutWhite"));
        yield return new WaitForSeconds(3.9f);
        fadeWhiteAnimator.Rebind();
        FadeWhiteComponent.SetActive(false);
        fadeWhiteAnimator.enabled = false;
        yield break;
    }

  


}
