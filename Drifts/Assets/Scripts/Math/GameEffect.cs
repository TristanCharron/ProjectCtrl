//By Dominic Brodeur-Gendron & Patrice Le Nouveau
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class GameEffect {
	
	#region SHAKE
	
	/// <summary>
	/// Shake the specified obj with default intensity of 1 and time of 0.2.
	/// </summary>
	/// <param name="obj">Object.</param>
	public static void Shake(GameObject obj)
	{
		ShakeEffect (obj, 1, .2f);
	}
	/// <summary>
	/// Shake the camera with base settings.
	/// </summary>
	public static void Shake()
	{
		ShakeEffect (Camera.main.gameObject, 1, .2f);
		
	}
	/// <summary>
	/// Shake the specified obj and intensity with default time value of 0.2.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="intensity">Intensity.</param>
	public static void Shake(GameObject obj,float intensity)
	{
		ShakeEffect (obj, intensity, .2f);
		
	}
	public static void Shake(GameObject obj,float intensity,float time, bool enableCameraFollowing)
	{
		ShakeEffect (obj, intensity, time,enableCameraFollowing);
	}
	static void ShakeEffect(GameObject obj,float intensity,float time, bool enableCameraFollowing)
	{
		if (obj.GetComponent<_GEffect.ShakeClass> () == null)
			obj.AddComponent<_GEffect.ShakeClass> ();
		_GEffect.ShakeClass shake = obj.GetComponent<_GEffect.ShakeClass> ();
		shake.shake = time;
		shake.shakeAmount = intensity;
		shake.followCam = enableCameraFollowing;
	}
	/// <summary>
	/// Shake the specified obj, intensity and time.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="intensity">Intensity.</param>
	/// <param name="time">Time.</param>
	public static void Shake(GameObject obj,float intensity,float time)
	{
		ShakeEffect (obj, intensity, time);
		
	}
	static void ShakeEffect(GameObject obj,float intensity,float time)
	{
		if (obj.GetComponent<_GEffect.ShakeClass> () == null)
			obj.AddComponent<_GEffect.ShakeClass> ();
		_GEffect.ShakeClass shake = obj.GetComponent<_GEffect.ShakeClass> ();
		shake.shake = time;
		shake.shakeAmount = intensity;
	}
	#endregion
	
	#region Freeze Frame
	/// <summary>
	/// Freezes the frame.
	/// </summary>
	/// <param name="sec">Sec.</param>
	public static void FreezeFrame(float sec)
	{
		if (Camera.main.gameObject.GetComponent<_GEffect.FreezeFrameClass> () == null)
			Camera.main.gameObject.AddComponent<_GEffect.FreezeFrameClass> ().freezeSec = sec;
	}
	
	/// <summary>
	/// Freezes the frame with default value of 0.1.
	/// </summary>
	public static void FreezeFrame()
	{
		if (Camera.main.gameObject.GetComponent<_GEffect.FreezeFrameClass> () == null)
			Camera.main.gameObject.AddComponent<_GEffect.FreezeFrameClass> ().freezeSec = .1f;
	}
	#endregion
	
	#region Sprite & Color
	
	/// <summary>
	/// Sins the gradient.
	/// </summary>
	/// <returns>The gradient.</returns>
	public static Color ColorLerp(Color color1, Color32 color2, float t)
	{
		return new Color 
			(
				Mathf.Lerp (color1.r, color2.r, t),
				Mathf.Lerp (color1.g, color2.g, t),
				Mathf.Lerp (color1.b, color2.b, t),
				Mathf.Lerp (color1.a, color2.a, t)
				);
	}
	public static Color SinGradient(Color color1, Color color2, float speed)
	{
		float t = (Mathf.Sin(Time.timeSinceLevelLoad * speed)+1) / 2;
		Color color = Color.Lerp(color1, color2,t);
		return color;
	}
	public static Color SinGradient(Color color1, Color color2, float time, float speed)
	{
		float t = (Mathf.Sin(time * speed)+1) / 2;
		Color color = Color.Lerp(color1, color2,t);
		return color;
	}

	public static void FlashSprite(GameObject obj, Color color,float duration)
	{
		if (obj.GetComponent<_GEffect.FlashSpriteClass> () == null)
		{
			obj.AddComponent<_GEffect.FlashSpriteClass> ();
			_GEffect.FlashSpriteClass flashSprite = obj.GetComponent<_GEffect.FlashSpriteClass> ();
			
			flashSprite.flashColor = color;
			flashSprite.duration = duration;
			flashSprite.flashSpriteEnum = _GEffect.FlashSpriteClass.FlashSpriteType.Simple;
		}
		
	}
	
	public static void FlashSprite(GameObject obj, Color color,float duration, int flashCount)
	{
		if (obj.GetComponent<_GEffect.FlashSpriteClass> () == null)
		{
			obj.AddComponent<_GEffect.FlashSpriteClass> ();
			_GEffect.FlashSpriteClass flashSprite = obj.GetComponent<_GEffect.FlashSpriteClass> ();
			
			flashSprite.flashColor = color;
			flashSprite.duration = duration;
			flashSprite.flashCount = flashCount;
			flashSprite.flashSpriteEnum = _GEffect.FlashSpriteClass.FlashSpriteType.Multiple;
		}
		
	}
	
	public static void FlashSpriteLerp(GameObject obj, Color color,float duration)
	{
		if (obj.GetComponent<_GEffect.FlashSpriteClass> () == null)
		{
			obj.AddComponent<_GEffect.FlashSpriteClass> ();
			_GEffect.FlashSpriteClass flashSprite = obj.GetComponent<_GEffect.FlashSpriteClass> ();
			flashSprite.flashColor = color;
			flashSprite.speed = duration;
			flashSprite.flashSpriteEnum = _GEffect.FlashSpriteClass.FlashSpriteType.Lerp;
		}
		
	}
	
	public static void FlashCamera(Color color, float time)
	{
		if(Camera.main.gameObject.GetComponent<_GEffect.FlashCameraClass> () == null)
			Camera.main.gameObject.AddComponent<_GEffect.FlashCameraClass> ();
		
		Camera.main.gameObject.GetComponent<_GEffect.FlashCameraClass> ().Flash (color,null, time,null);
	}
	public static void FlashCamera(Sprite image, float time)
	{
		if(Camera.main.gameObject.GetComponent<_GEffect.FlashCameraClass> () == null)
			Camera.main.gameObject.AddComponent<_GEffect.FlashCameraClass> ();
		
		Camera.main.gameObject.GetComponent<_GEffect.FlashCameraClass> ().Flash (Color.white,image, time,null);
		
	}
	public static void FlashCamera(Color color, float time,Transform canvas)
	{
		if(Camera.main.gameObject.GetComponent<_GEffect.FlashCameraClass> () == null)
			Camera.main.gameObject.AddComponent<_GEffect.FlashCameraClass> ();
		
		Camera.main.gameObject.GetComponent<_GEffect.FlashCameraClass> ().Flash (color,null, time,canvas);
	}
	public static void FlashCamera(Sprite image, float time,Transform canvas)
	{
		if(Camera.main.gameObject.GetComponent<_GEffect.FlashCameraClass> () == null)
			Camera.main.gameObject.AddComponent<_GEffect.FlashCameraClass> ();
		
		Camera.main.gameObject.GetComponent<_GEffect.FlashCameraClass> ().Flash (Color.white,image, time,canvas);
		
	}
	public static void FlashCamera(Color color,Sprite image, float time,Transform canvas)
	{
		if(Camera.main.gameObject.GetComponent<_GEffect.FlashCameraClass> () == null)
			Camera.main.gameObject.AddComponent<_GEffect.FlashCameraClass> ();
		
		Camera.main.gameObject.GetComponent<_GEffect.FlashCameraClass> ().Flash (color,image, time,canvas);
		
	}
	#endregion
	
	public static void DestroyChilds(Transform parent)
	{
		if (parent.childCount != 0) 
		{
			int childs = parent.childCount;
			for (int i = 0; i <= childs - 1; i++)
				MonoBehaviour.Destroy (parent.GetChild (i).gameObject);
		}
		
	}
	public static void DestroyChilds(GameObject parent)
	{
		GameEffect.DestroyChilds (parent.transform);
	}
}
public static class GamePhysics
{
	public static Vector3 BallisticVel(Transform origin,Transform target)
	{
		Vector3 dir = target.position - origin.position;
		float h = dir.y;
		dir.y = 0;
		
		float dist = dir.magnitude;
		
		float vel = Mathf.Sqrt (dist * Physics.gravity.magnitude);
		
		return vel * dir.normalized;
	}
	
	public static Vector3 BallisticVel(Transform origin,Transform target, float angle)
	{
		Vector3 dir = target.position - origin.position;
		float h = dir.y;
		dir.y = 0;
		
		float dist = dir.magnitude;
		float a = angle * Mathf.Deg2Rad;
		
		dir.y = dist * Mathf.Tan (a);
		dist += h / Mathf.Tan (a);
		
		float vel = Mathf.Sqrt (dist * Physics.gravity.magnitude / Mathf.Sin(2.5f * a));
		return vel * dir.normalized ;
	}
}
public static class GameMath
{
	///////***************** Math ********************////////
	public static float CenterAlign(int NumberOfObject, float distance, int i)
	{
		return ((i - (((NumberOfObject) - 1) / 2)) * distance) - (((NumberOfObject + 1) % 2) * (distance / 2));
	}
	
	#region Math Curve
	public static float Sinerp(float t)
	{
		return Mathf.Sin (t * Mathf.PI * 0.5f);
	}
	public static float Smoothstep(float t)
	{
		return t * t * (3f - 2f * t);
	}
	public static float Smootherstep(float t)
	{
		return t * t * t * (t * (6f * t - 15f) + 10);
	}
	public static float SigmoidErf(float t)
	{
		return 1 / ( 1 + Mathf.Exp(-t));
	}
	
	public static float EaseInOut(float A, float speed)
	{
		//return % of speed
		return 1 - ((Mathf.Abs (A - .5f)) * speed);
	}
	public static float Stretch(float A, float stretchAmount)
	{
		if (A > 1)
			A = 1;
		
		float C = Mathf.Abs (A - .5f);
		
		return .5f +  stretchAmount + ((C + .5f) * stretchAmount);
	}
	#region Animation Curves
	
	public enum Curves{Log,Sigmoid,SteepSigmoid,ZigZag,Bounce,Exp}
	public enum AnimateMode{position,localPosition,eulerAngles,localEulerAngles,rotation,localRotation,localScale};
	
	/// <summary>
	/// Animate with the specified curve, bit slower than calling the right function
	/// </summary>
	/// <param name="t">t from 0 to 1</param>
	/// <param name="curve">the curve function</param>
	public static float Curve(float t, Curves curve)
	{
		switch(curve)
		{
		case Curves.Bounce:return Bounce(t);break;
		case Curves.Log:return Log01(t);break;
		case Curves.Sigmoid:return Sigmoid01(t);break;
		case Curves.SteepSigmoid:return SteepSigmoid01(t);break;
		case Curves.ZigZag:return ZigZag01(t);break;
		case Curves.Exp:return ExtremeExp01(t);break;
		}
		return t;
	}
	
	/// <summary>
	/// Return a log function between 0 and 1
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	public static float Log01(float t)
	{
		return Mathf.Log10 ((t + .11f) * 9f);
	}
	/// <summary>
	/// Return a smooth Sigmoid between 0 and 1
	/// </summary>
	/// <param name="x">The t coordinate between 0 and 1.</param>
	public static float Sigmoid01(float t)
	{
		return Mathf.Clamp01 (1 / (1 + Mathf.Exp (-(10 * t - 5.1f))));
	}
	/// <summary>
	/// Return a steep sigmoid between 0 and 1
	/// </summary>
	/// <param name="t">The t coordinate between 0 and 1.</param>
	public static float SteepSigmoid01(float t)
	{
		return Mathf.Clamp01 (1 / (1 + Mathf.Exp (-(30 * t - 15))));
	}
	/// <summary>
	/// Return a zigzag from 0 to 1
	/// </summary>
	/// <returns>The zag01.</returns>
	/// <param name="t">T.</param>
	public static float ZigZag01(float t)
	{
		return Mathf.Clamp01((.9f * Mathf.Sin (t * 1.37f) + .1f * Mathf.Sin (t * 1.37f * 15)) * 1.02f);
	}
	public static float ExtremeExp01(float t)
	{
		return Mathf.Clamp01 (5.9f * Mathf.Exp(t * 10 - 11.77f));
	}
	/// <summary>
	/// Bounce between 0 and 1
	/// </summary>
	public static float Bounce(float t) //from Mathfx
	{
		return Mathf.Abs (Mathf.Sin (6.28f * (t + 1) * (t + 1)) * (1 - t));
	}
	#endregion
	#endregion
	
	#region Distance
	public static bool IsMinimumDistance(GameObject object1,GameObject object2,float minimumDistance)
	{
		if (Mathf.Abs(object1.transform.position.x - object2.transform.position.x) < minimumDistance &&
		    Mathf.Abs(object1.transform.position.y - object2.transform.position.y) < minimumDistance &&
		    Mathf.Abs(object1.transform.position.z - object2.transform.position.z) < minimumDistance)
			return true;
		
		return false;
	}
	public static bool IsMinimumDistance(Transform object1,Transform object2,float minimumDistance)
	{
		if (Mathf.Abs(object1.position.x - object2.position.x) < minimumDistance &&
		    Mathf.Abs(object1.position.y - object2.position.y) < minimumDistance &&
		    Mathf.Abs(object1.position.z - object2.position.z) < minimumDistance)
			return true;
		
		return false;
	}
	public static bool IsMinimumDistance(Vector3 object1,Vector3 object2,float minimumDistance)
	{
		if (Mathf.Abs(object1.x - object2.x) < minimumDistance &&
		    Mathf.Abs(object1.y - object2.y) < minimumDistance &&
		    Mathf.Abs(object1.z - object2.z) < minimumDistance)
			return true;
		
		return false;
	}
	public static bool IsMinimumDistance(Vector2 object1,Vector2 object2,float minimumDistance)
	{
		if (Mathf.Abs(object1.x - object2.x) < minimumDistance &&
		    Mathf.Abs(object1.y - object2.y) < minimumDistance)
			return true;
		
		return false;
	}
	public static float Distance3D(GameObject object1,GameObject object2)
	{
		return Mathf.Sqrt
			( 
			 Mathf.Pow((object1.transform.position.x - object2.transform.position.x), 2) +
			 Mathf.Pow((object1.transform.position.y - object2.transform.position.y), 2) +
			 Mathf.Pow((object1.transform.position.z - object2.transform.position.z), 2)
			 );
	}
	public static float Distance3D(Transform transform1,Transform transform2)
	{
		return Mathf.Sqrt
			( 
			 Mathf.Pow((transform1.position.x - transform2.position.x), 2) +
			 Mathf.Pow((transform1.position.y - transform2.position.y), 2) +
			 Mathf.Pow((transform1.position.z - transform2.position.z), 2)
			 );
	}
	public static float Distance3D(Vector3 transform1,Vector3 transform2)
	{
		return Mathf.Sqrt
			( 
			 Mathf.Pow((transform1.x - transform2.x), 2) +
			 Mathf.Pow((transform1.y - transform2.y), 2) +
			 Mathf.Pow((transform1.z - transform2.z), 2)
			 );
	}
	public static float DistanceXY(GameObject object1,GameObject object2)
	{
		return Mathf.Sqrt
			( 
			 Mathf.Pow((object1.transform.position.x - object2.transform.position.x), 2) +
			 Mathf.Pow((object1.transform.position.y - object2.transform.position.y), 2)
			 );
	}
	public static float DistanceXY(Transform transform1,Transform transform2)
	{
		return Mathf.Sqrt
			( 
			 Mathf.Pow((transform1.position.x - transform2.position.x), 2) +
			 Mathf.Pow((transform1.position.y - transform2.position.y), 2)
			 );
	}
	public static float DistanceXZ(GameObject object1,GameObject object2)
	{
		return Mathf.Sqrt
			( 
			 Mathf.Pow((object1.transform.position.x - object2.transform.position.x), 2) +
			 Mathf.Pow((object1.transform.position.z - object2.transform.position.z), 2)
			 );
	}
	public static float DistanceXZ(Transform transform1,Transform transform2)
	{
		return Mathf.Sqrt
			( 
			 Mathf.Pow((transform1.position.x - transform2.position.x), 2) +
			 Mathf.Pow((transform1.position.z - transform2.position.z), 2)
			 );
	}
	public static float DistanceYZ(GameObject object1,GameObject object2)
	{
		return Mathf.Sqrt
			( 
			 Mathf.Pow((object1.transform.position.y - object2.transform.position.y), 2) +
			 Mathf.Pow((object1.transform.position.z - object2.transform.position.z), 2)
			 );
	}
	public static float DistanceYZ(Transform transform1,Transform transform2)
	{
		return Mathf.Sqrt
			( 
			 Mathf.Pow((transform1.position.y - transform2.position.y), 2) +
			 Mathf.Pow((transform1.position.z - transform2.position.z), 2)
			 );
	}
	#endregion
	
	#region Vectors
	public static Vector2 RotateVector(float angle, Vector2 point)
	{
		float a = angle * Mathf.PI / 180;
		float cosA = Mathf.Cos (a);
		float sinA = Mathf.Sin (a);
		Vector2 newPoint = 
			new Vector2 (
				(point.x * cosA - point.y * sinA),
				(point.x * sinA + point.y * cosA)
				);
		return newPoint;
	}
	
	public static Vector3 RotateVectorY(float angle, Vector3 point)
	{
		Vector2 vec = RotateVector(angle,  new Vector2 (point.x, point.z));
		return new Vector3 (vec.x, point.y, vec.y);
	}
	/// <summary>
	/// Return the angle of two vectors from -180 to 180 degree
	/// </summary>
	public static float Angle(Vector3 from, Vector3 to)
	{
		return (Vector3.Angle (from,to))* (-Mathf.Sign (Vector3.Cross (from, to).y));
	}
	
	/// <summary>
	/// Return the angle of two vectors from -180 to 180 degree (to test lol)
	/// </summary>
	public static float Angle(Vector2 from, Vector2 to)
	{
		///to test
		return (Vector2.Angle (from,to))* (-Mathf.Sign (Vector3.Cross (from, to).y));
	}
	
	public static void Animate(GameObject gameObject,Vector3 from, Vector3 to, float time, AnimateMode animateMode, Curves curve)
	{
		if(gameObject.GetComponent<_GEffect.AnimateClass>() == null)
			gameObject.AddComponent<_GEffect.AnimateClass>().Set(from,to,time,animateMode,curve);
	}
	#endregion
}
#region hidden functions
namespace _GEffect
{
	public class FreezeFrameClass : MonoBehaviour {
		
		public float freezeSec;
		
		void Start()
		{
			StartCoroutine (FreezeFrameEffect());
		}
		
		IEnumerator FreezeFrameEffect()
		{
			Time.timeScale = 0.01f;
			float pauseEndTime = Time.realtimeSinceStartup + freezeSec;
			while (Time.realtimeSinceStartup < pauseEndTime)
				yield return 0;
			
			Time.timeScale = 1;
			Destroy (this);
		}
	}
	
	public class ShakeClass : MonoBehaviour {
		
		public Transform shakeTransform;
		
		// How long the object should shake for.
		public float shake = 0.1f;
		
		// Amplitude of the shake. A larger value shakes the camera harder.
		public float shakeAmount = 0.10f;
		public float decreaseFactor = 0.7f;
		public bool followCam;
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
			if (followCam)
				originalPos = transform.localPosition;
		}
		void LateUpdate()
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
	
	public class FlashSpriteClass : MonoBehaviour
	{
		public enum FlashSpriteType
		{
			Multiple, Simple, Lerp
		};
		
		public FlashSpriteType flashSpriteEnum;
		
		Color originalColor;
		public Color flashColor;
		
		public float speed, duration;
		float t;
		public int flashCount;
		
		SpriteRenderer spriteRender;
		
		void Start()
		{
			spriteRender = gameObject.GetComponent<SpriteRenderer> ();
			originalColor = spriteRender.color;
			
			if (flashSpriteEnum == FlashSpriteType.Simple) 
			{
				StartCoroutine(simpleFlash ());
			}
			else if (flashSpriteEnum == FlashSpriteType.Multiple)
			{
				StartCoroutine(multipleFlash ());
			}
			
		}
		
		void Update()
		{
			
			if (flashSpriteEnum == FlashSpriteType.Lerp)
			{
				lerpFlash ();
			}
		}
		
		IEnumerator simpleFlash()
		{
			
			spriteRender.color = flashColor;
			yield return new WaitForSeconds (duration);
			spriteRender.color = originalColor;
			Destroy (this);
		}
		
		IEnumerator multipleFlash()
		{
			float splitTime = (duration / flashCount) / 2;
			
			for(int i = 0; i < flashCount; i++)
			{
				spriteRender.color = flashColor;
				yield return new WaitForSeconds (splitTime);
				spriteRender.color = originalColor;
				yield return new WaitForSeconds (splitTime);
			}
			Destroy (this);
		}
		
		void lerpFlash()
		{
			t += Time.deltaTime / speed;
			float t2 = (Mathf.Sin(t)+1) / 2;
			
			spriteRender.color = new Color
				(
					Mathf.Lerp(originalColor.r, flashColor.r,t),
					Mathf.Lerp(originalColor.g, flashColor.g,t),
					Mathf.Lerp(originalColor.b, flashColor.b,t),
					Mathf.Lerp(originalColor.a, flashColor.a,t)		
					);
			if (t > 1) 
			{
				spriteRender.color = originalColor;
				Destroy (this);
			}
		}
		
	}
	public class FlashCameraClass : MonoBehaviour
	{
		float t = 0;
		float speed;
		GameObject screen;
		Color color;
		Image image;
		
		bool isFlashing = false;
		bool isIncreasing = true;
		Transform canvas;
		void Awake()
		{
			screen = new GameObject ();
			screen.AddComponent<Image> ();
			screen.GetComponent<Image> ().raycastTarget = false;
			screen.name = "Flashing Screen";
			screen.GetComponent<Image> ().color = new Color (0, 0, 0, 0);
		}
		void SetCanvas()
		{
			if(canvas == null)
				screen.transform.SetParent (GameObject.Find ("Canvas").transform, true);
			else
				screen.transform.SetParent (canvas, true);
			
			screen.GetComponent<Image> ().rectTransform.sizeDelta = new Vector2 (Screen.width, Screen.height);
			screen.GetComponent<Image> ().rectTransform.localPosition = Vector2.zero;
		}
		public void Flash(Color _color, Sprite sprite, float time, Transform _canvas)
		{
			if (_canvas != null)
			{
				if(canvas != _canvas)
					canvas = _canvas;		
				
				SetCanvas ();
			}
			
			speed = 1 / time;
			t = 0;
			color = _color;
			screen.GetComponent<Image> ().sprite = sprite;
			isIncreasing = true;
			isFlashing = true;
		}
		void Update()
		{
			if (!isFlashing)
				return;
			
			t += Time.deltaTime * speed * 2;
			
			
			if (isIncreasing)
			{
				screen.GetComponent<Image> ().color  = new Color (color.r, color.g, color.b, Mathf.Lerp (0,  color.a, t));
				if (t > 1)
				{
					isIncreasing = false;
					t = 0;
				}
			}
			else
			{
				screen.GetComponent<Image> ().color  = new Color (color.r, color.g, color.b, Mathf.Lerp (color.a, 0, t));
				if (t > 1)
					isFlashing = false;
			}
		}
	}
	public class AnimateClass : MonoBehaviour
	{
		Vector3 from;
		Vector3 to;
		float speed;
		delegate float DelegateCurve(float t);
		DelegateCurve curve;
		
		delegate void DelegateAnimation(float t);
		DelegateAnimation animation;
		
		public void Set(Vector3 _from, Vector3 _to,float _time, GameMath.AnimateMode _mode,GameMath.Curves _curve)
		{
			from = _from;
			to = _to;
			speed = 1 / ((_time != 0) ? _time : Mathf.Epsilon);
			switch(_curve)
			{
			case GameMath.Curves.Bounce: curve = GameMath.Bounce;break;
			case GameMath.Curves.Log:curve = GameMath.Log01;break;
			case GameMath.Curves.Sigmoid:curve =  GameMath.Sigmoid01;break;
			case GameMath.Curves.SteepSigmoid:curve = GameMath.SteepSigmoid01;break;
			case GameMath.Curves.ZigZag:curve = GameMath.ZigZag01;break;
			case GameMath.Curves.Exp:curve = GameMath.ExtremeExp01;break;
			}
			switch(_mode)
			{
			case GameMath.AnimateMode.position :animation = AnimatePosition;break;
			case GameMath.AnimateMode.localPosition :animation = AnimateLocalPosition;break;
				
			case GameMath.AnimateMode.eulerAngles : animation = AnimateEulerAngles;break;
			case GameMath.AnimateMode.localEulerAngles: animation = AnimateLocalEulerAngles;break;
				
			case GameMath.AnimateMode.rotation : animation = AnimateRotation;break;
			case GameMath.AnimateMode.localRotation: animation = AnimateLocalRotation;break;
				
			case GameMath.AnimateMode.localScale: animation = AnimateLocalScale;break;
			}
			StartCoroutine(PlayAnimation());
		}
		
		IEnumerator PlayAnimation()
		{
			float t = 0;
			while(t <= 1)
			{
				animation(curve(t));
				t += Time.deltaTime * speed;
				yield return new WaitForEndOfFrame();
			}
			t = 1;
			animation(curve(t));
			
			//Animation completed
			Destroy (this);
		}
		void AnimatePosition(float t)
		{
			transform.position = Vector3.Lerp(from,to,t);
		}
		void AnimateLocalPosition(float t)
		{
			transform.localPosition = Vector3.Lerp(from,to,t);
		}
		void AnimateEulerAngles(float t)
		{
			transform.eulerAngles = Vector3.Lerp(from,to,t);
		}
		void AnimateLocalEulerAngles(float t)
		{
			transform.localEulerAngles = Vector3.Lerp(from,to,t);
		}
		void AnimateRotation(float t)
		{
			transform.rotation = Quaternion.Euler(Vector3.Lerp(from,to,t));
		}
		void AnimateLocalRotation(float t)
		{
			transform.localRotation = Quaternion.Euler(Vector3.Lerp(from,to,t));
		}
		void AnimateLocalScale(float t)
		{
			transform.localScale = Vector3.Lerp(from,to,t);
		}
	}
}
#endregion
