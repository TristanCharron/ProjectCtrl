using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour {

    // Amplitude of the shake. A larger value shakes the camera harder.
    public const float Power = 0.75f;

    // How long the object should shake for.
    public float shakeDuration = 0.5f;

	public float decreaseFactor = 0.8f;


	float shake;
	Vector3 originalPos;


	void OnEnable()
	{
		originalPos = transform.localPosition;
		shake = shakeDuration;

	}
	
	void Update()
	{
		if (shake > 0)
		{
			transform.localPosition = originalPos + Random.insideUnitSphere * Power;
			shake -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shake = 0f;
			transform.localPosition = new Vector3(0,0,0);
			this.enabled = false;
		}
	}
}
