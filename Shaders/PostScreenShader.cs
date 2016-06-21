using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;
[ExecuteInEditMode]
[AddComponentMenu("Hidden/PostScreenEffect")]

public class PostScreenShader : ImageEffectBase {
    public Material mat;

    //Shader parameters transfered to Shader.
    public enum PostScreenEffect { none = 0, Cinema3D = 1, Vaporwave = 2,};
    public PostScreenEffect effect;


    [Range(0, 1)]
    public float intensity, distortion;

    [Range(0, 2)]
    public float Red, Green, Blue;

    public bool flipDown = false, flipUp = false;
    static bool isGlitching = false;
    public bool isInversedColor;
    public Color inversedColor;


    // Use this for initialization
    void Awake () {
    }
	
	

    void onSelectEffect()
    {
        switch(effect)
        {
            case PostScreenEffect.Cinema3D:
                Red = 1;
                isInversedColor = true;
                inversedColor.r = 1;
                break;
            case PostScreenEffect.Vaporwave:
                intensity = 0.3f;
                distortion = 0.2f;
                Red = 0.7f;
                Green = 0.7f;
                Blue = 1;
                isInversedColor = true;
                inversedColor.r = 1;
                break;
        }

    }


    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        onSelectEffect();


        float _intensity = isGlitching ? intensity : 0;
        isGlitching = intensity / 3 > Random.value;

        mat.SetFloat("distortion", distortion);
        mat.SetFloat("_Intensity", _intensity);
        mat.SetFloat("_Chroma",1);
        mat.SetFloat("Red", Red);
        mat.SetFloat("Green", Green);
        mat.SetFloat("Blue", Blue);
        mat.SetFloat("flip_up", isGlitching ? Random.Range(0.3f, 0.6f) : 0);
        mat.SetFloat("flip_down", isGlitching ? Random.Range(0.3f, 0.6f) : 1);
        mat.SetFloat("displace", isGlitching ? Random.Range(0, _intensity) : 0);
        mat.SetFloat("scale", isGlitching ? 1 - Random.Range(0, _intensity) : 0);
        mat.SetFloat("isInversedColor", isInversedColor ? 1 : 0);
        mat.SetFloat("inverseRed", inversedColor.r);
        mat.SetFloat("inverseGreen", inversedColor.g);
        mat.SetFloat("inverseBlue", inversedColor.b);



        Graphics.Blit(src, dest, mat);
    }
}
