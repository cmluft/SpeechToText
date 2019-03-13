using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFacingCamera : MonoBehaviour {

   public Camera m_Camera;
    // Use this for initialization
    void LateUpdate()
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            m_Camera.transform.rotation * Vector3.forward);
    }
}
