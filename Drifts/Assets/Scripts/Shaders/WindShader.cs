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
    static float _intensity;
    private static float disabledIntensity, currentIntensity, destinationVelocity;

    void Awake()
    {
        _intensity = intensity;
        currentIntensity = intensity;
        disabledIntensity = 0;
        destinationVelocity = currentIntensity;
        instance = this;
    }




    // Called by camera to apply image effect
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetFloat("_Intensity", intensity);
        Graphics.Blit(source, destination, material);
    }

    public static void onWindWave()
    {
        destinationVelocity = _intensity;
    }

}

