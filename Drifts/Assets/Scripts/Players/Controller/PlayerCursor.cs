using UnityEngine;
using Rewired;
using UnityEngine.UI;
using System.Collections;


public class PlayerCursor : MonoBehaviour
{
	[SerializeField]
	Transform cursorTransform;
	[SerializeField]
	Transform LookAtTransform;
	private Vector3 startRotation;
	private Vector3 endRotation;
	float cursor_t, cursorSpeed;

    PlayerController Owner;

    public Quaternion LookingAtAngle { get { return Quaternion.LookRotation(LookAtTransform.position - cursorTransform.transform.position); } }

    void Awake()
    {
        Owner = transform.GetComponentInParent<PlayerController>();
        cursor_t = 0;
        cursorSpeed = 10;
    }

    public void OnRotate()
    {
        //lerp
        if (cursor_t < 1)
        {
            cursor_t += Time.deltaTime * cursorSpeed;
            transform.GetChild(0).localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(startRotation.z, endRotation.z, cursor_t));
        }

        float inputX = ReInput.players.GetPlayer(Owner.player.ID - 1).GetAxis("Rotate Horizontal");
        float inputY = ReInput.players.GetPlayer(Owner.player.ID - 1).GetAxis("Rotate Vertical");

        if (Mathf.Abs(inputX) > 0.15f || Mathf.Abs(inputY) > 0.15f)
        {
            startRotation = transform.GetChild(0).localEulerAngles;
            endRotation = new Vector3(0, 0, Mathf.Atan2(inputX, inputY) * 180 / Mathf.PI);
            cursor_t = 0;
        }
    }
}