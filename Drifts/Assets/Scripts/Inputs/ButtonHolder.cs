using UnityEngine;
using System.Collections;

	public class ButtonHolder
	{

		public float currentHoldPushBtnTime = 0f;
		public const float maxHoldPushBtnTime = 1f;
		public float holdingButtonRatio { get { return Mathf.Clamp01(currentHoldPushBtnTime / maxHoldPushBtnTime); } }

		public ButtonHolder()
		{
			currentHoldPushBtnTime = 0;
		}

		public void OnUpdate()
		{
			if (GameController.isGameStarted)
				currentHoldPushBtnTime = currentHoldPushBtnTime > maxHoldPushBtnTime ? maxHoldPushBtnTime : currentHoldPushBtnTime += Time.fixedDeltaTime;
			else
				currentHoldPushBtnTime = 0;
		}

		public void OnReset()
		{
			currentHoldPushBtnTime = 0;
		}


	}


