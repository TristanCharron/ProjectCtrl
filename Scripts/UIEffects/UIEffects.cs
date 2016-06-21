using UnityEngine;
using System.Collections;

public class UIEffects : MonoBehaviour
{

    private static bool isCurrentlyShaking = false, isShaking = false, isLightFlickering = false;
    public float intensity = 0.01f;
    public Light ambientLight;
    public static UIEffects Instance;

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShaking)
            StartCoroutine(onShake());
        if (isLightFlickering)
            StartCoroutine(onLightFlickering(ambientLight));
    }

    public static void onEnableShake(float length)
    {
        isShaking = true;
        Instance.Invoke("onDisableShake", length);
    }

    void onDisableShake()
    {
        isShaking = false;
    }

    IEnumerator onShake()
    {
        if(!isCurrentlyShaking)
        {
            isCurrentlyShaking = true;

            Transform origin = Camera.main.transform;
            Vector2 shake = Random.insideUnitCircle * intensity;
            Vector3 Shake = new Vector3(shake.x, shake.y, 0);

            Camera.main.transform.position += Shake;

            yield return new WaitForEndOfFrame();
            Camera.main.transform.position = origin.position;
            isCurrentlyShaking = false;
        }
        
        yield break;
    }

    IEnumerator onLightFlickering(Light light)
    {
        float origin = light.intensity;
        float flicker = (Random.value * intensity * 3);
        light.intensity += Random.value > 0.5f ? flicker : -flicker;
        Mathf.Clamp(light.intensity,0.6f,0.9f);
        yield return new WaitForEndOfFrame();
        light.intensity = origin;
        yield break;
    }
}
