using UnityEngine;
using System.Collections;

public class UIEffectManager : MonoBehaviour {

    private static UIEffectManager instance;
    public static UIEffectManager Instance { get { return instance; } }

    public static bool IsFreezeFraming { get { return isFreezeFraming; } }
    private static bool isFreezeFraming = false, isScreenShake = false;

    public static Vector3 ShakeAmount { get { return shakeAmount; } }
    private static Vector3 shakeAmount;


    // Use this for initialization
    void Start () {
	
	}

    public static void OnResetProperties()
    {
        isFreezeFraming = false;
        isScreenShake = false;
        shakeAmount = Vector3.zero;
    }


    public static void OnFreezeFrame(float sec, float power)
    {
        instance.StartCoroutine(instance.FreezeFrame(sec, power));

    }

    public static void OnScreenShake(float sec)
    {
        instance.StartCoroutine(instance.ScreenShake(sec));

    }

    public IEnumerator FreezeFrame(float sec, float Power)
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

        Camera.main.GetComponentInParent<Shake>().enabled = true;
        Camera.main.GetComponentInParent<Shake>().Power = Power;

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

    /*public static float onLerp()
    {
        return Mathf.PingPong(Time.time, duration) / duration;
    }
    */
}
