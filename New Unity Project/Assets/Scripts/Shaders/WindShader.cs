using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[AddComponentMenu("Image Effects/WindEffect")]
public class WindShader : ImageEffectBase
{
    private static WindShader instance;
    public static WindShader Instance { get { return instance; } }

    //Shader parameters transfered to Shader
    [Range(0.1f, 100f)]
    public float intensity = 100f;
    private float disabledIntensity = 0.5f;
    private float currentIntensity = 0;
    static bool isEnabled = false;

    void Awake()
    {
        currentIntensity = intensity;
        instance = this;
    }


    public static void onEnableWind()
    {
        instance.StartCoroutine(instance.onWindEffect(3f));
    }

    public IEnumerator onWindEffect(float length)
    {
        isEnabled = true;
        yield return new WaitForSeconds(length);
        isEnabled = false;
        yield break;
    }




    // Called by camera to apply image effect
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        currentIntensity = isEnabled ? intensity : disabledIntensity;
        material.SetFloat("_Intensity", currentIntensity);
        Graphics.Blit(source, destination, material);
    }
}

