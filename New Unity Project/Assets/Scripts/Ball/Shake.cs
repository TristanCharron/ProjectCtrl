using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour {
	
	public Transform shakeTransform;
	
	// How long the object should shake for.
	public float shake = 0.4f;
	
	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.01f;
	public float decreaseFactor = 0.8f;
	
	Vector3 originalPos;


	void Awake()
	{
		if (shakeTransform == null)
		{
			shakeTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}
	
	void OnEnable()
	{
		originalPos = shakeTransform.localPosition;
	}
	
	void Update()
	{
		if (shake > 0)
		{
			shakeTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			shake -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shake = 0f;
			shakeTransform.localPosition = originalPos;
			Destroy(this);
		}
	}
}
