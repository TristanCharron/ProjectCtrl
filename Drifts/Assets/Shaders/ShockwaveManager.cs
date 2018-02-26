using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ShockwaveManager : MonoBehaviour
{
	#region simpleSingleton
	private static ShockwaveManager instance;
	public static ShockwaveManager GetInstance()
	{
		return instance;
	}
	void Awake()
	{
		instance = this;
		EffectMaterial.SetFloat("_Lerp", 0);
	}
	#endregion

	[SerializeField] Material EffectMaterial;

	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		if (EffectMaterial != null)
			Graphics.Blit(src, dst, EffectMaterial);
	}

	public void CastShockwave(float size, Vector2 screenPos, float speed, Color color, float intensity)
	{
		StartCoroutine(ShockWaveAnim(size,screenPos,speed,color,intensity));
	}

	public void CastShockwave(float size, Vector3 worldPos, float speed, Color color, float intensity)
	{
		Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);
		StartCoroutine(ShockWaveAnim(size,screenPos,speed,color,intensity));
	}

	IEnumerator ShockWaveAnim(float size, Vector2 screenPos, float speed, Color color, float intensity)
	{
		float t = 0;
		screenPos = new Vector2(screenPos.x / Screen.width, screenPos.y / Screen.height);
		float screenYRatio = (float)Screen.height / (float)Screen.width;
		EffectMaterial.SetColor("_Color", color);
		EffectMaterial.SetFloat("_Size", size);
		EffectMaterial.SetVector("_Pos", screenPos);
		EffectMaterial.SetFloat("_Intensity", intensity);
		EffectMaterial.SetFloat("_ScreenYRatio", screenYRatio);

		while(t < 1)
		{
			t += Time.deltaTime * speed;
			EffectMaterial.SetFloat("_Lerp", t);
			yield return new WaitForEndOfFrame();
		}
		EffectMaterial.SetFloat("_Lerp", 0);
	}
}
