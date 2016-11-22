using UnityEngine;
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

        float inputX = Input.GetAxis("Horizontal_Rotation_" + Owner.player.ID);
        float inputY = Input.GetAxis("Vertical_Rotation_" + Owner.player.ID);

        if ((inputX > 0.5f || inputY > 0.5f) || (inputX < -0.5f || inputY < -0.5f))
        {
            startRotation = transform.GetChild(0).localEulerAngles;
            endRotation = new Vector3(0, 0, Mathf.Atan2(inputX, inputY) * 180 / Mathf.PI);
            cursor_t = 0;
        }
    }
}