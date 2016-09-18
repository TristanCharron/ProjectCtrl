using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[AddComponentMenu("Image Effects/WindEffect")]
public class WindShader : ImageEffectBase
{
    private static WindShader instance;
    public static WindShader Instance { get { return instance; } }

    public const float length = 3;

    //Shader parameters transfered to Shader
    [Range(0.1f, 20f)]
    public float intensity = 100f;
    private float disabledIntensity = 0.5f, currentIntensity;
    static bool isEnabled = false;

    void Awake()
    {
        currentIntensity = 0.1f;
        instance = this;
        onEnableWind();
      
    }



    public static void onEnableWind()
    {
        instance.StartCoroutine(instance.onWindEffect());
    }

    public IEnumerator onWindEffect()
    {
        isEnabled = true;
        yield return new WaitForSeconds(length);
        isEnabled = false;
        yield break;
    }




    // Called by camera to apply image effect
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (isEnabled)
            currentIntensity = Mathf.Lerp(currentIntensity, intensity, currentIntensity / intensity);
        else
            currentIntensity = Mathf.Lerp(currentIntensity, disabledIntensity, disabledIntensity / currentIntensity);

        material.SetFloat("_Intensity", currentIntensity);
        Graphics.Blit(source, destination, material);
    }
}

