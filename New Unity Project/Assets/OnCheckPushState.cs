using UnityEngine;
using System.Collections;

public class OnCheckPushState : MonoBehaviour {

    MonkController m_Controller;
    void Start()
    {
        m_Controller = GetComponentInParent<MonkController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Orb"))
            m_Controller.onTriggerPush(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Orb"))
            m_Controller.onTriggerPush(false);
    }
}
