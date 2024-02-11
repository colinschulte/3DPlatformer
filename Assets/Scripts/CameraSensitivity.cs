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

    public void SetSensitivity(float sliderValue)
    {
        gameManager = FindObjectOfType<GameManager>();
        freeLook.m_XAxis.m_MaxSpeed = gameManager.cameraXMax * sliderValue;
        freeLook.m_YAxis.m_MaxSpeed = gameManager.cameraYMax * sliderValue;
        gameManager.cameraXCurrent = freeLook.m_XAxis.m_MaxSpeed;
        gameManager.cameraYCurrent = freeLook.m_YAxis.m_MaxSpeed;
        gameManager.cameraSliderValue = sliderValue;
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
