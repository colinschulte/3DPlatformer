using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CenterCamera : MonoBehaviour
{
    private CinemachineFreeLook freelookCam;

    void Start()
    {
        freelookCam = GetComponent<CinemachineFreeLook>();
    }

    public void centerCamera(bool isPressed)
    {
        if (isPressed)
        {
            freelookCam.m_RecenterToTargetHeading.m_enabled = true;
        }
        else
        {
            freelookCam.m_RecenterToTargetHeading.m_enabled = false;
        }
    }
}
