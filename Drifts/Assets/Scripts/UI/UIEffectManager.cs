using UnityEngine;
using System.Collections;

public class UIEffectManager : MonoBehaviour {

    private static UIEffectManager instance;
    public static UIEffectManager Instance { get { return instance; } }

    public static bool IsFreezeFraming { get { return isFreezeFraming; } }
    private static bool isFreezeFraming = false, isScreenShake = false;

    public static Vector3 ShakeAmount { get { return shakeAmount; } }
    private static Vector3 shakeAmount;

    private static Shake shakeComponent;

    public GameObject FadeWhiteComponent;
    private static Animator fadeWhiteAnimator;

    // Use this for initialization
    void Awake () {
        instance = this;
        shakeComponent = Camera.main.GetComponentInParent<Shake>();
        fadeWhiteAnimator = FadeWhiteComponent.GetComponent<Animator>();

    }

    public static void OnResetProperties()
    {
        isFreezeFraming = false;
        isScreenShake = false;
        shakeAmount = Vector3.zero;
    }


    public static void OnFreezeFrame(float sec)
    {
        instance.StartCoroutine(instance.FreezeFrame(sec));

    }

    public static void OnScreenShake(float sec)
    {
        instance.StartCoroutine(instance.ScreenShake(sec));

    }

    public IEnumerator FreezeFrame(float sec)
    {
        if (!isFreezeFraming)
        {
            isFreezeFraming = true;
            Time.timeScale = 0.01f;
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
                shakeAmount = originalCamPos + Random.insideUnitSphere * 50f;

            }

            shakeAmount = Vector3.zero;
            isScreenShake = false;
        }
        yield break;
    }

    public static void OnFadeToWhite()
    {
        instance.StartCoroutine(instance.OnFadeAnimToWhite());
       
    
    }

    public IEnumerator OnFadeAnimToWhite()
    {
        fadeWhiteAnimator.enabled = true;
        FadeWhiteComponent.SetActive(true);
        yield return new WaitForSeconds(3.9f);
        fadeWhiteAnimator.Rebind();
        FadeWhiteComponent.SetActive(false);
        fadeWhiteAnimator.enabled = false;
        yield break;
    }

  


}
