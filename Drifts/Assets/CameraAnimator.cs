using UnityEngine;
using System.Collections;

public class CameraAnimator : MonoBehaviour {

    private static CameraAnimator instance;
    public static CameraAnimator Instance { get { return instance; } }

    private static Animator camAnim;

    // Use this for initialization
    void Awake () {
        instance = this;
        camAnim = instance.GetComponent<Animator>();
        instance.StartCoroutine(instance.OnCameraAnim(true));
    }
	

    public IEnumerator OnCameraAnim(bool isMoving)
    {

        camAnim.enabled = true;
        // fadeWhiteAnimator.Play(Animator.StringToHash(fadeIn ? "fadeInWhite" : "fadeOutWhite"));
        yield return new WaitForSeconds(3f);
        //camAnim.Rebind();
       
        camAnim.enabled = false;
        yield break;
    }
}
