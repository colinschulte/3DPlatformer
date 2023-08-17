using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSensitivty : MonoBehaviour
{
    public CinemachineFreeLook freeLook;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        freeLook = FindObjectOfType<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSensitivity(float sliderValue)
    {
        gameManager = FindObjectOfType<GameManager>();
        freeLook.m_XAxis.m_MaxSpeed = gameManager.CameraXMax * sliderValue;
        freeLook.m_YAxis.m_MaxSpeed = gameManager.CameraYMax * sliderValue;
        gameManager.CameraXCurrent = freeLook.m_XAxis.m_MaxSpeed;
        gameManager.CameraYCurrent = freeLook.m_YAxis.m_MaxSpeed;
    }

    public void InvertX(bool isXinverted)
    {
        freeLook.m_XAxis.m_InvertInput = isXinverted;
        gameManager.xInvert = isXinverted;
    }

    public void InvertY(bool isYinverted)
    {
        freeLook.m_YAxis.m_InvertInput = isYinverted;
        gameManager.yInvert = isYinverted;
    }
}
